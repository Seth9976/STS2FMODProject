using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// Selection screen for card rewards after winning combat.
	/// NOTE: The card reward selection screen is only set up to look good with exactly 3 cards right now. Any more or less
	/// than that and things may start to look wonky. We'll need do some UI work to make it look nicer if that's required.
	/// </summary>
	// Token: 0x02000346 RID: 838
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCardRewardSelectionScreen.cs")]
	public class NCardRewardSelectionScreen : Control, IOverlayScreen, IScreenContext
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x0015D45E File Offset: 0x0015B65E
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/card_reward_selection_screen");
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x0015D46A File Offset: 0x0015B66A
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new string[] { NCardRewardSelectionScreen.ScenePath }.Concat<string>(NCardRewardAlternativeButton.AssetPaths);
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x0015D484 File Offset: 0x0015B684
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.CardSelection;
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x0015D488 File Offset: 0x0015B688
		[return: Nullable(2)]
		public static NCardRewardSelectionScreen ShowScreen(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> extraOptions)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardRewardSelectionScreen ncardRewardSelectionScreen = PreloadManager.Cache.GetScene(NCardRewardSelectionScreen.ScenePath).Instantiate<NCardRewardSelectionScreen>(0L);
			ncardRewardSelectionScreen.Name = "NCardRewardSelectionScreen";
			ncardRewardSelectionScreen._options = options;
			ncardRewardSelectionScreen._extraOptions = extraOptions;
			NOverlayStack.Instance.Push(ncardRewardSelectionScreen);
			return ncardRewardSelectionScreen;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x0015D4DF File Offset: 0x0015B6DF
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x0015D4EC File Offset: 0x0015B6EC
		public override void _Ready()
		{
			this._ui = base.GetNode<Control>("UI");
			this._cardRow = base.GetNode<Control>("UI/CardRow");
			this._banner = base.GetNode<NCommonBanner>("UI/Banner");
			this._rewardAlternativesContainer = base.GetNode<Control>("UI/RewardAlternatives");
			this._inspectPrompt = base.GetNode<Control>("%InspectPrompt");
			this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER").GetRawText());
			this._banner.AnimateIn();
			this.RefreshOptions(this._options, this._extraOptions);
			this.UpdateControllerIcons();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
		}

		/// <summary>
		/// Called both in _Ready and if someone re-rolls the options of the associated CardReward.
		/// </summary>
		// Token: 0x06002FD1 RID: 12241 RVA: 0x0015D614 File Offset: 0x0015B814
		public void RefreshOptions(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> extraOptions)
		{
			this._options = options;
			this._extraOptions = extraOptions;
			Vector2 vector = Vector2.Left * (float)(this._options.Count - 1) * 350f * 0.5f;
			this._lastFocusedControl = null;
			foreach (NGridCardHolder ngridCardHolder in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
			{
				ngridCardHolder.QueueFreeSafely();
			}
			foreach (NCardRewardAlternativeButton ncardRewardAlternativeButton in this._rewardAlternativesContainer.GetChildren(false).OfType<NCardRewardAlternativeButton>())
			{
				ncardRewardAlternativeButton.QueueFreeSafely();
			}
			this._cardTween = base.CreateTween().SetParallel(true);
			for (int i = 0; i < this._options.Count; i++)
			{
				CardCreationResult cardCreationResult = this._options[i];
				NCard ncard = NCard.Create(cardCreationResult.Card, ModelVisibility.Visible);
				NGridCardHolder holder = NGridCardHolder.Create(ncard);
				this._cardRow.AddChildSafely(holder);
				holder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.SelectCard)), 0U);
				holder.Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.InspectCard)), 0U);
				holder.Connect(Control.SignalName.FocusEntered, Callable.From<Control>(() => this._lastFocusedControl = holder), 0U);
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				holder.Scale = holder.SmallScale;
				this._cardTween.TweenProperty(holder, "position", vector + Vector2.Right * 350f * (float)i, 0.5).SetEase(1L).SetTrans(5L);
				this._cardTween.TweenProperty(holder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L)
					.From(Colors.Black);
				ncard.ActivateRewardScreenGlow();
				foreach (RelicModel relicModel in cardCreationResult.ModifyingRelics)
				{
					relicModel.Flash();
					ncard.FlashRelicOnCard(relicModel);
				}
			}
			for (int j = 0; j < this._extraOptions.Count; j++)
			{
				int capturedIndex = j;
				CardRewardAlternative cardRewardAlternative = this._extraOptions[j];
				NCardRewardAlternativeButton ncardRewardAlternativeButton2 = NCardRewardAlternativeButton.Create(cardRewardAlternative.Title.GetFormattedText(), cardRewardAlternative.Hotkey);
				this._rewardAlternativesContainer.AddChildSafely(ncardRewardAlternativeButton2);
				ncardRewardAlternativeButton2.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
				{
					this.OnAlternateRewardSelected(capturedIndex);
				}), 0U);
			}
			for (int k = 0; k < this._cardRow.GetChildCount(false); k++)
			{
				Control child = this._cardRow.GetChild<Control>(k, false);
				child.FocusNeighborBottom = child.GetPath();
				child.FocusNeighborTop = child.GetPath();
				child.FocusNeighborLeft = ((k > 0) ? this._cardRow.GetChild(k - 1, false).GetPath() : this._cardRow.GetChild(this._cardRow.GetChildCount(false) - 1, false).GetPath());
				child.FocusNeighborRight = ((k < this._cardRow.GetChildCount(false) - 1) ? this._cardRow.GetChild(k + 1, false).GetPath() : this._cardRow.GetChild(0, false).GetPath());
			}
			ActiveScreenContext.Instance.FocusOnDefaultControl();
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0015DA60 File Offset: 0x0015BC60
		public override void _ExitTree()
		{
			this._cts.Cancel();
			TaskCompletionSource<int?> completionSource = this._completionSource;
			if (completionSource != null)
			{
				Task<int?> task = completionSource.Task;
				if (task != null && !task.IsCompleted)
				{
					this._completionSource.SetException(new TaskCanceledException());
				}
			}
			foreach (NGridCardHolder ngridCardHolder in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
			{
				ngridCardHolder.QueueFreeSafely();
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x0015DAF0 File Offset: 0x0015BCF0
		public NCardHolder GetCardHolder(CardModel card)
		{
			return this._cardRow.GetChildren(false).OfType<NGridCardHolder>().First<NGridCardHolder>((NGridCardHolder h) => h.CardModel == card);
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x0015DB2C File Offset: 0x0015BD2C
		private void OnAlternateRewardSelected(int index)
		{
			TaskCompletionSource<int?> completionSource = this._completionSource;
			if (completionSource == null)
			{
				return;
			}
			completionSource.SetResult(new int?(this._options.Count + index));
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x0015DB50 File Offset: 0x0015BD50
		private void SelectCard(NCardHolder cardHolder)
		{
			if (this._completionSource == null)
			{
				throw new InvalidOperationException("CardsSelected must be awaited before a card is selected!");
			}
			this._completionSource.SetResult(new int?(this._options.FirstIndex<CardCreationResult>((CardCreationResult o) => o.Card == cardHolder.CardModel)));
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x0015DBA4 File Offset: 0x0015BDA4
		private unsafe void InspectCard(NCardHolder cardHolder)
		{
			if (this._completionSource.Task.IsCompleted)
			{
				return;
			}
			NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
			int num = 1;
			List<CardModel> list = new List<CardModel>(num);
			CollectionsMarshal.SetCount<CardModel>(list, num);
			Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
			int num2 = 0;
			*span[num2] = cardHolder.CardNode.Model;
			inspectCardScreen.Open(list, 0, false);
		}

		/// <returns>The index of the selected option. If the index is greater than or equal to the number of cards, then
		/// it represents the index of an alternative option.</returns>
		// Token: 0x06002FD7 RID: 12247 RVA: 0x0015DC04 File Offset: 0x0015BE04
		public async Task<int?> OptionSelected()
		{
			this._completionSource = new TaskCompletionSource<int?>();
			return await this._completionSource.Task;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x0015DC48 File Offset: 0x0015BE48
		public void AfterOverlayOpened()
		{
			this.PowerCardFtueCheck();
			this._banner.AnimateIn();
			this._buttonTween = base.CreateTween();
			this._buttonTween.SetParallel(true);
			this._buttonTween.TweenProperty(this._rewardAlternativesContainer, "position", this._rewardAlternativesContainer.Position, 0.5).SetEase(1L).SetTrans(10L)
				.From(this._rewardAlternativesContainer.Position - NCardRewardSelectionScreen._bannerAnimPosOffset);
			TaskHelper.RunSafely(this.DisableCardsForShortTimeAfterOpening());
		}

		/// <summary>
		/// For a short time after opening the screen, set the cards to unselectable.
		/// Common mistake is for the player to click right after opening the screen, selecting a reward by mistake.
		/// Note that this only makes it so the player cannot select the cards. They can still navigate
		/// </summary>
		// Token: 0x06002FD9 RID: 12249 RVA: 0x0015DCF0 File Offset: 0x0015BEF0
		private async Task DisableCardsForShortTimeAfterOpening()
		{
			foreach (NGridCardHolder ngridCardHolder in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
			{
				ngridCardHolder.SetClickable(false);
			}
			await Cmd.Wait(0.35f, this._cts.Token, false);
			if (this._cardRow.IsValid())
			{
				foreach (NGridCardHolder ngridCardHolder2 in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
				{
					ngridCardHolder2.SetClickable(true);
				}
			}
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x0015DD34 File Offset: 0x0015BF34
		private void PowerCardFtueCheck()
		{
			if (SaveManager.Instance.SeenFtue("power_card_ftue"))
			{
				return;
			}
			IEnumerable<NGridCardHolder> enumerable = this._cardRow.GetChildren(false).OfType<NGridCardHolder>();
			NGridCardHolder ngridCardHolder = enumerable.FirstOrDefault<NGridCardHolder>((NGridCardHolder h) => h.CardModel.Type == CardType.Power);
			if (ngridCardHolder != null)
			{
				NModalContainer.Instance.Add(NPowerCardFtue.Create(ngridCardHolder), true);
				SaveManager.Instance.MarkFtueAsComplete("power_card_ftue");
			}
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x0015DDAE File Offset: 0x0015BFAE
		public void AfterOverlayClosed()
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x0015DDB6 File Offset: 0x0015BFB6
		public void AfterOverlayShown()
		{
			base.Visible = true;
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x0015DDBF File Offset: 0x0015BFBF
		public void AfterOverlayHidden()
		{
			base.Visible = false;
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x0015DDC8 File Offset: 0x0015BFC8
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0015DDCC File Offset: 0x0015BFCC
		public Control DefaultFocusedControl
		{
			get
			{
				if (this._lastFocusedControl != null)
				{
					return this._lastFocusedControl;
				}
				List<NGridCardHolder> list = this._cardRow.GetChildren(false).OfType<NGridCardHolder>().ToList<NGridCardHolder>();
				return list[list.Count / 2];
			}
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x0015DE10 File Offset: 0x0015C010
		private void UpdateControllerIcons()
		{
			this._inspectPrompt.Visible = NControllerManager.Instance.IsUsingController;
			this._inspectPrompt.GetNode<TextureRect>("ControllerIcon").Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.accept);
			this._inspectPrompt.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("gameplay_ui", "TO_INSPECT_PROMPT").GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FE1 RID: 12257 RVA: 0x0015DE90 File Offset: 0x0015C090
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NCardRewardSelectionScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.SelectCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.InspectCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardSelectionScreen.MethodName.UpdateControllerIcons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FE2 RID: 12258 RVA: 0x0015E160 File Offset: 0x0015C360
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRewardSelectionScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected && args.Count == 1)
			{
				this.OnAlternateRewardSelected(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.SelectCard && args.Count == 1)
			{
				this.SelectCard(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.InspectCard && args.Count == 1)
			{
				this.InspectCard(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck && args.Count == 0)
			{
				this.PowerCardFtueCheck();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardSelectionScreen.MethodName.UpdateControllerIcons && args.Count == 0)
			{
				this.UpdateControllerIcons();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002FE3 RID: 12259 RVA: 0x0015E35C File Offset: 0x0015C55C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardRewardSelectionScreen.MethodName._EnterTree || (ref method) == NCardRewardSelectionScreen.MethodName._Ready || (ref method) == NCardRewardSelectionScreen.MethodName._ExitTree || (ref method) == NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected || (ref method) == NCardRewardSelectionScreen.MethodName.SelectCard || (ref method) == NCardRewardSelectionScreen.MethodName.InspectCard || (ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayOpened || (ref method) == NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck || (ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayClosed || (ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayShown || (ref method) == NCardRewardSelectionScreen.MethodName.AfterOverlayHidden || (ref method) == NCardRewardSelectionScreen.MethodName.UpdateControllerIcons || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002FE4 RID: 12260 RVA: 0x0015E424 File Offset: 0x0015C624
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._ui)
			{
				this._ui = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._cardRow)
			{
				this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer)
			{
				this._rewardAlternativesContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._inspectPrompt)
			{
				this._inspectPrompt = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._buttonTween)
			{
				this._buttonTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._lastFocusedControl)
			{
				this._lastFocusedControl = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002FE5 RID: 12261 RVA: 0x0015E514 File Offset: 0x0015C714
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardRewardSelectionScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._ui)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ui);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._cardRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardAlternativesContainer);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._inspectPrompt)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inspectPrompt);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._buttonTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._buttonTween);
				return true;
			}
			if ((ref name) == NCardRewardSelectionScreen.PropertyName._lastFocusedControl)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lastFocusedControl);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FE6 RID: 12262 RVA: 0x0015E694 File Offset: 0x0015C894
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._ui, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._cardRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._inspectPrompt, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._buttonTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName._lastFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardRewardSelectionScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardRewardSelectionScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardSelectionScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FE7 RID: 12263 RVA: 0x0015E814 File Offset: 0x0015CA14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._ui, Variant.From<Control>(ref this._ui));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, Variant.From<Control>(ref this._rewardAlternativesContainer));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._inspectPrompt, Variant.From<Control>(ref this._inspectPrompt));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._buttonTween, Variant.From<Tween>(ref this._buttonTween));
			info.AddProperty(NCardRewardSelectionScreen.PropertyName._lastFocusedControl, Variant.From<Control>(ref this._lastFocusedControl));
		}

		/// <inheritdoc />
		// Token: 0x06002FE8 RID: 12264 RVA: 0x0015E8D8 File Offset: 0x0015CAD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._ui, ref variant))
			{
				this._ui = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._banner, ref variant2))
			{
				this._banner = variant2.As<NCommonBanner>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._cardRow, ref variant3))
			{
				this._cardRow = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, ref variant4))
			{
				this._rewardAlternativesContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._inspectPrompt, ref variant5))
			{
				this._inspectPrompt = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._cardTween, ref variant6))
			{
				this._cardTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._buttonTween, ref variant7))
			{
				this._buttonTween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._lastFocusedControl, ref variant8))
			{
				this._lastFocusedControl = variant8.As<Control>();
			}
		}

		// Token: 0x04001260 RID: 4704
		private const ulong _noSelectionTimeMsec = 350UL;

		// Token: 0x04001261 RID: 4705
		private Control _ui;

		// Token: 0x04001262 RID: 4706
		private NCommonBanner _banner;

		// Token: 0x04001263 RID: 4707
		private Control _cardRow;

		// Token: 0x04001264 RID: 4708
		private IReadOnlyList<CardCreationResult> _options;

		// Token: 0x04001265 RID: 4709
		private IReadOnlyList<CardRewardAlternative> _extraOptions;

		// Token: 0x04001266 RID: 4710
		private Control _rewardAlternativesContainer;

		// Token: 0x04001267 RID: 4711
		private Control _inspectPrompt;

		// Token: 0x04001268 RID: 4712
		[Nullable(2)]
		private TaskCompletionSource<int?> _completionSource;

		// Token: 0x04001269 RID: 4713
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x0400126A RID: 4714
		[Nullable(2)]
		private Tween _buttonTween;

		// Token: 0x0400126B RID: 4715
		private const float _cardXOffset = 350f;

		// Token: 0x0400126C RID: 4716
		private static readonly Vector2 _bannerAnimPosOffset = new Vector2(0f, 50f);

		// Token: 0x0400126D RID: 4717
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400126E RID: 4718
		[Nullable(2)]
		private Control _lastFocusedControl;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F8 RID: 5624
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005524 RID: 21796
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005525 RID: 21797
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005526 RID: 21798
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnAlternateRewardSelected' method.
			/// </summary>
			// Token: 0x04005527 RID: 21799
			public static readonly StringName OnAlternateRewardSelected = "OnAlternateRewardSelected";

			/// <summary>
			/// Cached name for the 'SelectCard' method.
			/// </summary>
			// Token: 0x04005528 RID: 21800
			public static readonly StringName SelectCard = "SelectCard";

			/// <summary>
			/// Cached name for the 'InspectCard' method.
			/// </summary>
			// Token: 0x04005529 RID: 21801
			public static readonly StringName InspectCard = "InspectCard";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x0400552A RID: 21802
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'PowerCardFtueCheck' method.
			/// </summary>
			// Token: 0x0400552B RID: 21803
			public static readonly StringName PowerCardFtueCheck = "PowerCardFtueCheck";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x0400552C RID: 21804
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x0400552D RID: 21805
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x0400552E RID: 21806
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";

			/// <summary>
			/// Cached name for the 'UpdateControllerIcons' method.
			/// </summary>
			// Token: 0x0400552F RID: 21807
			public static readonly StringName UpdateControllerIcons = "UpdateControllerIcons";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F9 RID: 5625
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005530 RID: 21808
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005531 RID: 21809
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005532 RID: 21810
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_ui' field.
			/// </summary>
			// Token: 0x04005533 RID: 21811
			public static readonly StringName _ui = "_ui";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x04005534 RID: 21812
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_cardRow' field.
			/// </summary>
			// Token: 0x04005535 RID: 21813
			public static readonly StringName _cardRow = "_cardRow";

			/// <summary>
			/// Cached name for the '_rewardAlternativesContainer' field.
			/// </summary>
			// Token: 0x04005536 RID: 21814
			public static readonly StringName _rewardAlternativesContainer = "_rewardAlternativesContainer";

			/// <summary>
			/// Cached name for the '_inspectPrompt' field.
			/// </summary>
			// Token: 0x04005537 RID: 21815
			public static readonly StringName _inspectPrompt = "_inspectPrompt";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x04005538 RID: 21816
			public static readonly StringName _cardTween = "_cardTween";

			/// <summary>
			/// Cached name for the '_buttonTween' field.
			/// </summary>
			// Token: 0x04005539 RID: 21817
			public static readonly StringName _buttonTween = "_buttonTween";

			/// <summary>
			/// Cached name for the '_lastFocusedControl' field.
			/// </summary>
			// Token: 0x0400553A RID: 21818
			public static readonly StringName _lastFocusedControl = "_lastFocusedControl";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015FA RID: 5626
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
