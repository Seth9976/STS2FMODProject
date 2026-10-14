using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// Selection screen for choosing a card from in-combat generators like Attack Potion.
	/// NOTE: The card reward selection screen is only set up to look good with exactly 3 cards right now. Any more or less
	/// than that and things may start to look wonky. We'll need do some UI work to make it look nicer if that's required.
	/// </summary>
	/// TODO: this is very similar to NCardRewardScreen... if we have a 3rd screen set up like this, might be
	/// worth having them inherit from a common class.
	// Token: 0x02000349 RID: 841
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChooseACardSelectionScreen.cs")]
	public class NChooseACardSelectionScreen : Control, IOverlayScreen, IScreenContext, ICardSelector
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x00160BC3 File Offset: 0x0015EDC3
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/choose_a_card_selection_screen");
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x00160BCF File Offset: 0x0015EDCF
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NChooseACardSelectionScreen.ScenePath);
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x00160BDB File Offset: 0x0015EDDB
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.CardSelection;
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00160BE0 File Offset: 0x0015EDE0
		[return: Nullable(2)]
		public static NChooseACardSelectionScreen ShowScreen(IReadOnlyList<CardModel> cards, bool canSkip)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NChooseACardSelectionScreen nchooseACardSelectionScreen = PreloadManager.Cache.GetScene(NChooseACardSelectionScreen.ScenePath).Instantiate<NChooseACardSelectionScreen>(0L);
			nchooseACardSelectionScreen.Name = "NChooseACardSelectionScreen";
			nchooseACardSelectionScreen._cards = cards;
			nchooseACardSelectionScreen._canSkip = canSkip;
			NOverlayStack.Instance.Push(nchooseACardSelectionScreen);
			return nchooseACardSelectionScreen;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x00160C38 File Offset: 0x0015EE38
		public override void _Ready()
		{
			this._banner = base.GetNode<NCommonBanner>("Banner");
			this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER").GetRawText());
			this._banner.AnimateIn();
			this._cardRow = base.GetNode<Control>("CardRow");
			this._combatPiles = base.GetNode<NCombatPilesContainer>("%CombatPiles");
			if (CombatManager.Instance.IsInProgress)
			{
				this._combatPiles.Initialize(this._cards.First<CardModel>().Owner);
			}
			this._combatPiles.Disable();
			this._combatPiles.Visible = false;
			this._inspectPrompt = base.GetNode<Control>("%InspectPrompt");
			Vector2 vector = Vector2.Left * (float)(this._cards.Count - 1) * 340f * 0.5f;
			this._cardTween = base.CreateTween().SetParallel(true);
			for (int i = 0; i < this._cards.Count; i++)
			{
				CardModel cardModel = this._cards[i];
				NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
				NGridCardHolder ngridCardHolder = NGridCardHolder.Create(ncard);
				this._cardRow.AddChildSafely(ngridCardHolder);
				ngridCardHolder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.SelectHolder)), 0U);
				ngridCardHolder.Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OpenPreviewScreen)), 0U);
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				ngridCardHolder.Scale = ngridCardHolder.SmallScale;
				this._cardTween.TweenProperty(ngridCardHolder, "position", vector + Vector2.Right * 340f * (float)i, 0.5).SetEase(1L).SetTrans(5L);
				this._cardTween.TweenProperty(ngridCardHolder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L)
					.From(Colors.Black);
				ncard.ActivateRewardScreenGlow();
			}
			this._skipButton = base.GetNode<NChoiceSelectionSkipButton>("SkipButton");
			if (this._canSkip)
			{
				this._skipButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSkipButtonReleased)), 0U);
				this._skipButton.AnimateIn();
			}
			else
			{
				this._skipButton.Disable();
				this._skipButton.Visible = false;
			}
			this._peekButton = base.GetNode<NPeekButton>("%PeekButton");
			this._peekButton.AddTargets(new Control[] { this._banner, this._cardRow, this._skipButton, this._inspectPrompt });
			this._peekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(delegate(NPeekButton _)
			{
				if (this._peekButton.IsPeeking)
				{
					base.MouseFilter = 2L;
					this._combatPiles.Visible = true;
					this._combatPiles.Enable();
					this._skipButton.Disable();
					return;
				}
				base.MouseFilter = 0L;
				this._combatPiles.Visible = false;
				this._combatPiles.Disable();
				if (this._canSkip)
				{
					this._skipButton.Enable();
				}
			}), 0U);
			for (int j = 0; j < this._cardRow.GetChildCount(false); j++)
			{
				Control child = this._cardRow.GetChild<Control>(j, false);
				child.FocusNeighborBottom = child.GetPath();
				child.FocusNeighborTop = child.GetPath();
				child.FocusNeighborLeft = ((j > 0) ? this._cardRow.GetChild(j - 1, false).GetPath() : this._cardRow.GetChild(this._cardRow.GetChildCount(false) - 1, false).GetPath());
				child.FocusNeighborRight = ((j < this._cardRow.GetChildCount(false) - 1) ? this._cardRow.GetChild(j + 1, false).GetPath() : this._cardRow.GetChild(0, false).GetPath());
			}
			this.UpdateControllerIcons();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x00161094 File Offset: 0x0015F294
		public override void _ExitTree()
		{
			if (!this._completionSource.Task.IsCompleted)
			{
				this._completionSource.SetCanceled();
			}
			foreach (NGridCardHolder ngridCardHolder in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
			{
				ngridCardHolder.QueueFreeSafely();
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00161108 File Offset: 0x0015F308
		private void SelectHolder(NCardHolder cardHolder)
		{
			if (this._completionSource == null)
			{
				throw new InvalidOperationException("CardsSelected must be awaited before a card is selected!");
			}
			if (Time.GetTicksMsec() - this._openedTicks <= 350UL)
			{
				return;
			}
			CardModel cardModel = cardHolder.CardModel;
			this._screenComplete = true;
			this._cardSelected = true;
			this._completionSource.SetResult(new CardModel[] { cardModel });
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x00161168 File Offset: 0x0015F368
		private unsafe void OpenPreviewScreen(NCardHolder cardHolder)
		{
			NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
			int num = 1;
			List<CardModel> list = new List<CardModel>(num);
			CollectionsMarshal.SetCount<CardModel>(list, num);
			Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
			int num2 = 0;
			*span[num2] = cardHolder.CardModel;
			inspectCardScreen.Open(list, 0, false);
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x001611B0 File Offset: 0x0015F3B0
		public async Task<IEnumerable<CardModel>> CardsSelected()
		{
			IEnumerable<CardModel> enumerable = await this._completionSource.Task;
			IEnumerable<CardModel> enumerable2 = enumerable;
			NOverlayStack.Instance.Remove(this);
			return enumerable2;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x001611F3 File Offset: 0x0015F3F3
		private void OnSkipButtonReleased(NButton _)
		{
			this._screenComplete = true;
			this._completionSource.SetResult(Array.Empty<CardModel>());
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x0016120C File Offset: 0x0015F40C
		public void AfterOverlayOpened()
		{
			base.Modulate = Colors.Transparent;
			this._openedTicks = Time.GetTicksMsec();
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this._fadeTween = base.CreateTween();
			this._fadeTween.TweenProperty(this, "modulate:a", 1f, 0.2);
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x00161276 File Offset: 0x0015F476
		public void AfterOverlayClosed()
		{
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this._peekButton.SetPeeking(false);
			this.QueueFreeSafely();
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x0016129B File Offset: 0x0015F49B
		public void AfterOverlayShown()
		{
			base.Visible = true;
			if (CombatManager.Instance.IsInProgress)
			{
				this._peekButton.Enable();
			}
			if (this._canSkip && !this._peekButton.IsPeeking)
			{
				this._skipButton.Enable();
			}
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x001612DB File Offset: 0x0015F4DB
		public void AfterOverlayHidden()
		{
			this._peekButton.Disable();
			this._skipButton.Disable();
			base.Visible = false;
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x001612FA File Offset: 0x0015F4FA
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x00161300 File Offset: 0x0015F500
		public Control DefaultFocusedControl
		{
			get
			{
				if (this._peekButton.IsPeeking && NCombatRoom.Instance != null)
				{
					return NCombatRoom.Instance.DefaultFocusedControl;
				}
				List<NGridCardHolder> list = this._cardRow.GetChildren(false).OfType<NGridCardHolder>().ToList<NGridCardHolder>();
				return list[list.Count / 2];
			}
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x00161354 File Offset: 0x0015F554
		private void UpdateControllerIcons()
		{
			this._inspectPrompt.Modulate = (NControllerManager.Instance.IsUsingController ? Colors.White : Colors.Transparent);
			this._inspectPrompt.GetNode<TextureRect>("ControllerIcon").Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.accept);
			this._inspectPrompt.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("gameplay_ui", "TO_INSPECT_PROMPT").GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003027 RID: 12327 RVA: 0x001613E0 File Offset: 0x0015F5E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NChooseACardSelectionScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.SelectHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.OpenPreviewScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseACardSelectionScreen.MethodName.UpdateControllerIcons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003028 RID: 12328 RVA: 0x0016165C File Offset: 0x0015F85C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NChooseACardSelectionScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.SelectHolder && args.Count == 1)
			{
				this.SelectHolder(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.OpenPreviewScreen && args.Count == 1)
			{
				this.OpenPreviewScreen(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased && args.Count == 1)
			{
				this.OnSkipButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseACardSelectionScreen.MethodName.UpdateControllerIcons && args.Count == 0)
			{
				this.UpdateControllerIcons();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003029 RID: 12329 RVA: 0x00161810 File Offset: 0x0015FA10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NChooseACardSelectionScreen.MethodName._Ready || (ref method) == NChooseACardSelectionScreen.MethodName._ExitTree || (ref method) == NChooseACardSelectionScreen.MethodName.SelectHolder || (ref method) == NChooseACardSelectionScreen.MethodName.OpenPreviewScreen || (ref method) == NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased || (ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayOpened || (ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayClosed || (ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayShown || (ref method) == NChooseACardSelectionScreen.MethodName.AfterOverlayHidden || (ref method) == NChooseACardSelectionScreen.MethodName.UpdateControllerIcons || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600302A RID: 12330 RVA: 0x001618BC File Offset: 0x0015FABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardRow)
			{
				this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._skipButton)
			{
				this._skipButton = VariantUtils.ConvertTo<NChoiceSelectionSkipButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._combatPiles)
			{
				this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._inspectPrompt)
			{
				this._inspectPrompt = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._peekButton)
			{
				this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._openedTicks)
			{
				this._openedTicks = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._screenComplete)
			{
				this._screenComplete = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardSelected)
			{
				this._cardSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._canSkip)
			{
				this._canSkip = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600302B RID: 12331 RVA: 0x00161A18 File Offset: 0x0015FC18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NChooseACardSelectionScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._skipButton)
			{
				value = VariantUtils.CreateFrom<NChoiceSelectionSkipButton>(ref this._skipButton);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._combatPiles)
			{
				value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._inspectPrompt)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inspectPrompt);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._peekButton)
			{
				value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._openedTicks)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._openedTicks);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._screenComplete)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._screenComplete);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._cardSelected);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._canSkip)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._canSkip);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			if ((ref name) == NChooseACardSelectionScreen.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600302C RID: 12332 RVA: 0x00161C18 File Offset: 0x0015FE18
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._cardRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._skipButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._combatPiles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._inspectPrompt, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._peekButton, 0L, "", 4096L, false),
				new PropertyInfo(2L, NChooseACardSelectionScreen.PropertyName._openedTicks, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseACardSelectionScreen.PropertyName._screenComplete, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseACardSelectionScreen.PropertyName._cardSelected, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseACardSelectionScreen.PropertyName._canSkip, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName._fadeTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NChooseACardSelectionScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseACardSelectionScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseACardSelectionScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600302D RID: 12333 RVA: 0x00161E18 File Offset: 0x00160018
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._skipButton, Variant.From<NChoiceSelectionSkipButton>(ref this._skipButton));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._inspectPrompt, Variant.From<Control>(ref this._inspectPrompt));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._openedTicks, Variant.From<ulong>(ref this._openedTicks));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._screenComplete, Variant.From<bool>(ref this._screenComplete));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardSelected, Variant.From<bool>(ref this._cardSelected));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._canSkip, Variant.From<bool>(ref this._canSkip));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
			info.AddProperty(NChooseACardSelectionScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
		}

		/// <inheritdoc />
		// Token: 0x0600302E RID: 12334 RVA: 0x00161F34 File Offset: 0x00160134
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._banner, ref variant))
			{
				this._banner = variant.As<NCommonBanner>();
			}
			Variant variant2;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardRow, ref variant2))
			{
				this._cardRow = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._skipButton, ref variant3))
			{
				this._skipButton = variant3.As<NChoiceSelectionSkipButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._combatPiles, ref variant4))
			{
				this._combatPiles = variant4.As<NCombatPilesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._inspectPrompt, ref variant5))
			{
				this._inspectPrompt = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._peekButton, ref variant6))
			{
				this._peekButton = variant6.As<NPeekButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._openedTicks, ref variant7))
			{
				this._openedTicks = variant7.As<ulong>();
			}
			Variant variant8;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._screenComplete, ref variant8))
			{
				this._screenComplete = variant8.As<bool>();
			}
			Variant variant9;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardSelected, ref variant9))
			{
				this._cardSelected = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._canSkip, ref variant10))
			{
				this._canSkip = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardTween, ref variant11))
			{
				this._cardTween = variant11.As<Tween>();
			}
			Variant variant12;
			if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._fadeTween, ref variant12))
			{
				this._fadeTween = variant12.As<Tween>();
			}
		}

		// Token: 0x0400128B RID: 4747
		private const float _cardXSpacing = 340f;

		// Token: 0x0400128C RID: 4748
		private const ulong _noSelectionTimeMsec = 350UL;

		// Token: 0x0400128D RID: 4749
		private NCommonBanner _banner;

		// Token: 0x0400128E RID: 4750
		private Control _cardRow;

		// Token: 0x0400128F RID: 4751
		private NChoiceSelectionSkipButton _skipButton;

		// Token: 0x04001290 RID: 4752
		private NCombatPilesContainer _combatPiles;

		// Token: 0x04001291 RID: 4753
		private Control _inspectPrompt;

		// Token: 0x04001292 RID: 4754
		private NPeekButton _peekButton;

		// Token: 0x04001293 RID: 4755
		private readonly TaskCompletionSource<IEnumerable<CardModel>> _completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();

		// Token: 0x04001294 RID: 4756
		private ulong _openedTicks;

		// Token: 0x04001295 RID: 4757
		private bool _screenComplete;

		// Token: 0x04001296 RID: 4758
		private bool _cardSelected;

		// Token: 0x04001297 RID: 4759
		private bool _canSkip;

		// Token: 0x04001298 RID: 4760
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x04001299 RID: 4761
		[Nullable(2)]
		private Tween _fadeTween;

		// Token: 0x0400129A RID: 4762
		private IReadOnlyList<CardModel> _cards;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001609 RID: 5641
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005577 RID: 21879
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005578 RID: 21880
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SelectHolder' method.
			/// </summary>
			// Token: 0x04005579 RID: 21881
			public static readonly StringName SelectHolder = "SelectHolder";

			/// <summary>
			/// Cached name for the 'OpenPreviewScreen' method.
			/// </summary>
			// Token: 0x0400557A RID: 21882
			public static readonly StringName OpenPreviewScreen = "OpenPreviewScreen";

			/// <summary>
			/// Cached name for the 'OnSkipButtonReleased' method.
			/// </summary>
			// Token: 0x0400557B RID: 21883
			public static readonly StringName OnSkipButtonReleased = "OnSkipButtonReleased";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x0400557C RID: 21884
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x0400557D RID: 21885
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x0400557E RID: 21886
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x0400557F RID: 21887
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";

			/// <summary>
			/// Cached name for the 'UpdateControllerIcons' method.
			/// </summary>
			// Token: 0x04005580 RID: 21888
			public static readonly StringName UpdateControllerIcons = "UpdateControllerIcons";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200160A RID: 5642
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005581 RID: 21889
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005582 RID: 21890
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005583 RID: 21891
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x04005584 RID: 21892
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_cardRow' field.
			/// </summary>
			// Token: 0x04005585 RID: 21893
			public static readonly StringName _cardRow = "_cardRow";

			/// <summary>
			/// Cached name for the '_skipButton' field.
			/// </summary>
			// Token: 0x04005586 RID: 21894
			public static readonly StringName _skipButton = "_skipButton";

			/// <summary>
			/// Cached name for the '_combatPiles' field.
			/// </summary>
			// Token: 0x04005587 RID: 21895
			public static readonly StringName _combatPiles = "_combatPiles";

			/// <summary>
			/// Cached name for the '_inspectPrompt' field.
			/// </summary>
			// Token: 0x04005588 RID: 21896
			public static readonly StringName _inspectPrompt = "_inspectPrompt";

			/// <summary>
			/// Cached name for the '_peekButton' field.
			/// </summary>
			// Token: 0x04005589 RID: 21897
			public static readonly StringName _peekButton = "_peekButton";

			/// <summary>
			/// Cached name for the '_openedTicks' field.
			/// </summary>
			// Token: 0x0400558A RID: 21898
			public static readonly StringName _openedTicks = "_openedTicks";

			/// <summary>
			/// Cached name for the '_screenComplete' field.
			/// </summary>
			// Token: 0x0400558B RID: 21899
			public static readonly StringName _screenComplete = "_screenComplete";

			/// <summary>
			/// Cached name for the '_cardSelected' field.
			/// </summary>
			// Token: 0x0400558C RID: 21900
			public static readonly StringName _cardSelected = "_cardSelected";

			/// <summary>
			/// Cached name for the '_canSkip' field.
			/// </summary>
			// Token: 0x0400558D RID: 21901
			public static readonly StringName _canSkip = "_canSkip";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x0400558E RID: 21902
			public static readonly StringName _cardTween = "_cardTween";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x0400558F RID: 21903
			public static readonly StringName _fadeTween = "_fadeTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200160B RID: 5643
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
