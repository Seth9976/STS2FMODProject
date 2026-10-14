using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	// Token: 0x02000348 RID: 840
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChooseABundleSelectionScreen.cs")]
	public class NChooseABundleSelectionScreen : Control, IOverlayScreen, IScreenContext
	{
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0015F6CA File Offset: 0x0015D8CA
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/screens/card_selection/choose_a_bundle_selection_screen");
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x0015F6D6 File Offset: 0x0015D8D6
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NChooseABundleSelectionScreen.ScenePath);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x0015F6E2 File Offset: 0x0015D8E2
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.CardSelection;
			}
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x0015F6E8 File Offset: 0x0015D8E8
		public override void _Ready()
		{
			this._bundleRow = base.GetNode<Control>("%BundleRow");
			this._bundlePreviewContainer = base.GetNode<Control>("%BundlePreviewContainer");
			this._bundlePreviewCards = base.GetNode<Control>("%Cards");
			this._previewCancelButton = base.GetNode<NBackButton>("%Cancel");
			this._previewConfirmButton = base.GetNode<NConfirmButton>("%Confirm");
			this._banner = base.GetNode<NCommonBanner>("Banner");
			this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_A_PACK").GetRawText());
			this._banner.AnimateIn();
			this._previewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._previewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			Vector2 vector = Vector2.Left * (float)(this._bundles.Count - 1) * 400f * 0.5f;
			for (int i = 0; i < this._bundles.Count; i++)
			{
				NCardBundle ncardBundle = NCardBundle.Create(this._bundles[i]);
				this._bundleRow.AddChildSafely(ncardBundle);
				ncardBundle.Connect(NCardBundle.SignalName.Clicked, Callable.From<NCardBundle>(new Action<NCardBundle>(this.OnBundleClicked)), 0U);
				ncardBundle.Scale = ncardBundle.smallScale;
				ncardBundle.Position += vector + Vector2.Right * 400f * (float)i;
			}
			for (int j = 0; j < this._bundleRow.GetChildCount(false); j++)
			{
				NCardBundle child = this._bundleRow.GetChild<NCardBundle>(j, false);
				child.Hitbox.FocusNeighborLeft = ((j > 0) ? this._bundleRow.GetChild<NCardBundle>(j - 1, false).Hitbox.GetPath() : this._bundleRow.GetChild<NCardBundle>(this._bundleRow.GetChildCount(false) - 1, false).Hitbox.GetPath());
				child.Hitbox.FocusNeighborRight = ((j < this._bundleRow.GetChildCount(false) - 1) ? this._bundleRow.GetChild<NCardBundle>(j + 1, false).Hitbox.GetPath() : this._bundleRow.GetChild<NCardBundle>(0, false).Hitbox.GetPath());
				child.Hitbox.FocusNeighborTop = child.Hitbox.GetPath();
				child.Hitbox.FocusNeighborBottom = child.Hitbox.GetPath();
			}
			this._bundlePreviewContainer.Visible = false;
			this._bundlePreviewContainer.MouseFilter = 2L;
			this._peekButton = base.GetNode<NPeekButton>("%PeekButton");
			this._peekButton.AddTargets(new Control[] { this._banner, this._bundleRow, this._bundlePreviewContainer });
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x0015FA14 File Offset: 0x0015DC14
		public override void _ExitTree()
		{
			base._ExitTree();
			if (!this._completionSource.Task.IsCompleted)
			{
				this._completionSource.SetCanceled();
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x0015FA3C File Offset: 0x0015DC3C
		public static NChooseABundleSelectionScreen ShowScreen(IReadOnlyList<IReadOnlyList<CardModel>> bundles)
		{
			NChooseABundleSelectionScreen nchooseABundleSelectionScreen = PreloadManager.Cache.GetScene(NChooseABundleSelectionScreen.ScenePath).Instantiate<NChooseABundleSelectionScreen>(0L);
			nchooseABundleSelectionScreen.Name = "NChooseABundleSelectionScreen";
			nchooseABundleSelectionScreen._bundles = bundles;
			NOverlayStack instance = NOverlayStack.Instance;
			if (instance != null)
			{
				instance.Push(nchooseABundleSelectionScreen);
			}
			return nchooseABundleSelectionScreen;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x0015FA8C File Offset: 0x0015DC8C
		private void OnBundleClicked(NCardBundle bundleNode)
		{
			this._banner.AnimateOut();
			this._selectedBundle = bundleNode;
			this._bundlePreviewContainer.Visible = true;
			this._bundlePreviewContainer.MouseFilter = 0L;
			this._bundleRow.Visible = false;
			this._previewCancelButton.Enable();
			this._previewConfirmButton.Enable();
			Vector2 vector = Vector2.Right * (float)(bundleNode.Bundle.Count - 1) * 400f * 0.5f;
			IReadOnlyList<NCard> readOnlyList = bundleNode.RemoveCardNodes();
			Tween cardTween = this._cardTween;
			if (cardTween != null)
			{
				cardTween.Kill();
			}
			this._cardTween = base.CreateTween().SetParallel(true);
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				Vector2 globalPosition = readOnlyList[i].GlobalPosition;
				NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(readOnlyList[i], true, true);
				this._bundlePreviewCards.AddChildSafely(npreviewCardHolder);
				npreviewCardHolder.GlobalPosition = globalPosition;
				npreviewCardHolder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OpenPreviewScreen)), 0U);
				readOnlyList[i].UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				this._cardTween.TweenProperty(npreviewCardHolder, "position", vector + Vector2.Left * 400f * (float)i, 0.5).SetEase(1L).SetTrans(5L);
			}
			for (int j = 0; j < this._bundlePreviewCards.GetChildCount(false); j++)
			{
				NPreviewCardHolder child = this._bundlePreviewCards.GetChild<NPreviewCardHolder>(j, false);
				child.FocusNeighborLeft = ((j < this._bundlePreviewCards.GetChildCount(false) - 1) ? this._bundlePreviewCards.GetChild(j + 1, false).GetPath() : this._bundlePreviewCards.GetChild(0, false).GetPath());
				child.FocusNeighborRight = ((j > 0) ? this._bundlePreviewCards.GetChild(j - 1, false).GetPath() : this._bundlePreviewCards.GetChild(this._bundlePreviewCards.GetChildCount(false) - 1, false).GetPath());
				child.FocusNeighborTop = child.Hitbox.GetPath();
				child.FocusNeighborBottom = child.Hitbox.GetPath();
			}
			this._bundlePreviewCards.GetChild<Control>(this._bundlePreviewCards.GetChildCount(false) - 1, false).TryGrabFocus();
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x0015FD00 File Offset: 0x0015DF00
		private unsafe void OpenPreviewScreen(NCardHolder cardHolder)
		{
			NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
			int num = 1;
			List<CardModel> list = new List<CardModel>(num);
			CollectionsMarshal.SetCount<CardModel>(list, num);
			Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
			int num2 = 0;
			*span[num2] = cardHolder.CardNode.Model;
			inspectCardScreen.Open(list, 0, false);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x0015FD4C File Offset: 0x0015DF4C
		private void CancelSelection(NButton _)
		{
			this._banner.AnimateIn();
			this._bundlePreviewContainer.Visible = false;
			this._bundlePreviewContainer.MouseFilter = 2L;
			Tween cardTween = this._cardTween;
			if (cardTween != null)
			{
				cardTween.Kill();
			}
			NCardBundle selectedBundle = this._selectedBundle;
			if (selectedBundle != null)
			{
				selectedBundle.ReAddCardNodes();
			}
			NCardBundle selectedBundle2 = this._selectedBundle;
			if (selectedBundle2 != null)
			{
				selectedBundle2.Hitbox.TryGrabFocus();
			}
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			this._selectedBundle = null;
			this._bundleRow.Visible = true;
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x0015FDE0 File Offset: 0x0015DFE0
		private void ConfirmSelection(NButton _)
		{
			IReadOnlyList<NCard> cardNodes = this._selectedBundle.CardNodes;
			foreach (NCard ncard in cardNodes)
			{
				NRun.Instance.GlobalUi.ReparentCard(ncard);
				NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(ncard, PileType.Deck, true, ncard.Model.Owner.Character.TrailPath);
				NRun.Instance.GlobalUi.TopBar.TrailContainer.AddChildSafely(ncardFlyVfx);
			}
			this._completionSource.SetResult(new <>z__ReadOnlySingleElementList<IReadOnlyList<CardModel>>(this._selectedBundle.Bundle));
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x0015FE90 File Offset: 0x0015E090
		public async Task<IEnumerable<IReadOnlyList<CardModel>>> CardsSelected()
		{
			IEnumerable<IReadOnlyList<CardModel>> enumerable = await this._completionSource.Task;
			IEnumerable<IReadOnlyList<CardModel>> enumerable2 = enumerable;
			NOverlayStack.Instance.Remove(this);
			return enumerable2;
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x0015FED4 File Offset: 0x0015E0D4
		public void AfterOverlayOpened()
		{
			base.Modulate = Colors.Transparent;
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this._fadeTween = base.CreateTween();
			this._fadeTween.TweenProperty(this, "modulate:a", 1f, 0.4);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x0015FF33 File Offset: 0x0015E133
		public void AfterOverlayClosed()
		{
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this.QueueFreeSafely();
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x0015FF4C File Offset: 0x0015E14C
		public void AfterOverlayShown()
		{
			base.Visible = true;
			if (this._bundlePreviewContainer.Visible)
			{
				this._previewCancelButton.Enable();
				this._previewConfirmButton.Enable();
			}
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x0015FF78 File Offset: 0x0015E178
		public void AfterOverlayHidden()
		{
			base.Visible = false;
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x0015FF97 File Offset: 0x0015E197
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x0015FF9A File Offset: 0x0015E19A
		public Control DefaultFocusedControl
		{
			get
			{
				if (!this._bundlePreviewContainer.Visible)
				{
					return this._bundleRow.GetChild<NCardBundle>(0, false).Hitbox;
				}
				return this._bundlePreviewCards.GetChild<Control>(this._bundlePreviewCards.GetChildCount(false) - 1, false);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600300D RID: 12301 RVA: 0x0015FFD8 File Offset: 0x0015E1D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NChooseABundleSelectionScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.OnBundleClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "bundleNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.CancelSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.ConfirmSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600300E RID: 12302 RVA: 0x00160284 File Offset: 0x0015E484
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NChooseABundleSelectionScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.OnBundleClicked && args.Count == 1)
			{
				this.OnBundleClicked(VariantUtils.ConvertTo<NCardBundle>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen && args.Count == 1)
			{
				this.OpenPreviewScreen(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.CancelSelection && args.Count == 1)
			{
				this.CancelSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.ConfirmSelection && args.Count == 1)
			{
				this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600300F RID: 12303 RVA: 0x00160444 File Offset: 0x0015E644
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NChooseABundleSelectionScreen.MethodName._Ready || (ref method) == NChooseABundleSelectionScreen.MethodName._ExitTree || (ref method) == NChooseABundleSelectionScreen.MethodName.OnBundleClicked || (ref method) == NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen || (ref method) == NChooseABundleSelectionScreen.MethodName.CancelSelection || (ref method) == NChooseABundleSelectionScreen.MethodName.ConfirmSelection || (ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened || (ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed || (ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayShown || (ref method) == NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003010 RID: 12304 RVA: 0x001604F0 File Offset: 0x0015E6F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundleRow)
			{
				this._bundleRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer)
			{
				this._bundlePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards)
			{
				this._bundlePreviewCards = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._previewCancelButton)
			{
				this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._previewConfirmButton)
			{
				this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._selectedBundle)
			{
				this._selectedBundle = VariantUtils.ConvertTo<NCardBundle>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._peekButton)
			{
				this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003011 RID: 12305 RVA: 0x00160614 File Offset: 0x0015E814
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundleRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bundleRow);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bundlePreviewContainer);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bundlePreviewCards);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._previewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._previewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._selectedBundle)
			{
				value = VariantUtils.CreateFrom<NCardBundle>(ref this._selectedBundle);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._peekButton)
			{
				value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			if ((ref name) == NChooseABundleSelectionScreen.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003012 RID: 12306 RVA: 0x001607D4 File Offset: 0x0015E9D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._bundleRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._previewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._selectedBundle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._peekButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._fadeTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NChooseABundleSelectionScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseABundleSelectionScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseABundleSelectionScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003013 RID: 12307 RVA: 0x00160994 File Offset: 0x0015EB94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundleRow, Variant.From<Control>(ref this._bundleRow));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, Variant.From<Control>(ref this._bundlePreviewContainer));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, Variant.From<Control>(ref this._bundlePreviewCards));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._selectedBundle, Variant.From<NCardBundle>(ref this._selectedBundle));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
			info.AddProperty(NChooseABundleSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
		}

		/// <inheritdoc />
		// Token: 0x06003014 RID: 12308 RVA: 0x00160A84 File Offset: 0x0015EC84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundleRow, ref variant))
			{
				this._bundleRow = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, ref variant2))
			{
				this._bundlePreviewContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, ref variant3))
			{
				this._bundlePreviewCards = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._previewCancelButton, ref variant4))
			{
				this._previewCancelButton = variant4.As<NBackButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, ref variant5))
			{
				this._previewConfirmButton = variant5.As<NConfirmButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._selectedBundle, ref variant6))
			{
				this._selectedBundle = variant6.As<NCardBundle>();
			}
			Variant variant7;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._banner, ref variant7))
			{
				this._banner = variant7.As<NCommonBanner>();
			}
			Variant variant8;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._peekButton, ref variant8))
			{
				this._peekButton = variant8.As<NPeekButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._fadeTween, ref variant9))
			{
				this._fadeTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._cardTween, ref variant10))
			{
				this._cardTween = variant10.As<Tween>();
			}
		}

		// Token: 0x0400127E RID: 4734
		private Control _bundleRow;

		// Token: 0x0400127F RID: 4735
		private IReadOnlyList<IReadOnlyList<CardModel>> _bundles;

		// Token: 0x04001280 RID: 4736
		private Control _bundlePreviewContainer;

		// Token: 0x04001281 RID: 4737
		private Control _bundlePreviewCards;

		// Token: 0x04001282 RID: 4738
		private NBackButton _previewCancelButton;

		// Token: 0x04001283 RID: 4739
		private NConfirmButton _previewConfirmButton;

		// Token: 0x04001284 RID: 4740
		[Nullable(2)]
		private NCardBundle _selectedBundle;

		// Token: 0x04001285 RID: 4741
		private NCommonBanner _banner;

		// Token: 0x04001286 RID: 4742
		private readonly TaskCompletionSource<IEnumerable<IReadOnlyList<CardModel>>> _completionSource = new TaskCompletionSource<IEnumerable<IReadOnlyList<CardModel>>>();

		// Token: 0x04001287 RID: 4743
		private NPeekButton _peekButton;

		// Token: 0x04001288 RID: 4744
		[Nullable(2)]
		private Tween _fadeTween;

		// Token: 0x04001289 RID: 4745
		private const float _cardXSpacing = 400f;

		// Token: 0x0400128A RID: 4746
		[Nullable(2)]
		private Tween _cardTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001605 RID: 5637
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400555C RID: 21852
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400555D RID: 21853
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnBundleClicked' method.
			/// </summary>
			// Token: 0x0400555E RID: 21854
			public static readonly StringName OnBundleClicked = "OnBundleClicked";

			/// <summary>
			/// Cached name for the 'OpenPreviewScreen' method.
			/// </summary>
			// Token: 0x0400555F RID: 21855
			public static readonly StringName OpenPreviewScreen = "OpenPreviewScreen";

			/// <summary>
			/// Cached name for the 'CancelSelection' method.
			/// </summary>
			// Token: 0x04005560 RID: 21856
			public static readonly StringName CancelSelection = "CancelSelection";

			/// <summary>
			/// Cached name for the 'ConfirmSelection' method.
			/// </summary>
			// Token: 0x04005561 RID: 21857
			public static readonly StringName ConfirmSelection = "ConfirmSelection";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005562 RID: 21858
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04005563 RID: 21859
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04005564 RID: 21860
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04005565 RID: 21861
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001606 RID: 5638
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005566 RID: 21862
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005567 RID: 21863
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005568 RID: 21864
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_bundleRow' field.
			/// </summary>
			// Token: 0x04005569 RID: 21865
			public static readonly StringName _bundleRow = "_bundleRow";

			/// <summary>
			/// Cached name for the '_bundlePreviewContainer' field.
			/// </summary>
			// Token: 0x0400556A RID: 21866
			public static readonly StringName _bundlePreviewContainer = "_bundlePreviewContainer";

			/// <summary>
			/// Cached name for the '_bundlePreviewCards' field.
			/// </summary>
			// Token: 0x0400556B RID: 21867
			public static readonly StringName _bundlePreviewCards = "_bundlePreviewCards";

			/// <summary>
			/// Cached name for the '_previewCancelButton' field.
			/// </summary>
			// Token: 0x0400556C RID: 21868
			public static readonly StringName _previewCancelButton = "_previewCancelButton";

			/// <summary>
			/// Cached name for the '_previewConfirmButton' field.
			/// </summary>
			// Token: 0x0400556D RID: 21869
			public static readonly StringName _previewConfirmButton = "_previewConfirmButton";

			/// <summary>
			/// Cached name for the '_selectedBundle' field.
			/// </summary>
			// Token: 0x0400556E RID: 21870
			public static readonly StringName _selectedBundle = "_selectedBundle";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x0400556F RID: 21871
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_peekButton' field.
			/// </summary>
			// Token: 0x04005570 RID: 21872
			public static readonly StringName _peekButton = "_peekButton";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x04005571 RID: 21873
			public static readonly StringName _fadeTween = "_fadeTween";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x04005572 RID: 21874
			public static readonly StringName _cardTween = "_cardTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001607 RID: 5639
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
