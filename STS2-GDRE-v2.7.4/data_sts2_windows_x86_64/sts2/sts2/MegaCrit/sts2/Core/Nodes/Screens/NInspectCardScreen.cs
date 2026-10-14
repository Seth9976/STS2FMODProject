using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// Shows a very large version of a single card so players can view the portrait and its upgrade.
	/// Accessed from various card pile screens, shop, run history screen, and card library.
	/// We have to do this, rather than making it its own Overlay Screen, because we end up using
	/// it both for other overlay screens and capstone screens (deck view screen).
	/// </summary>
	// Token: 0x0200024E RID: 590
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NInspectCardScreen.cs")]
	public class NInspectCardScreen : Control, IScreenContext
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x000C6541 File Offset: 0x000C4741
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NInspectCardScreen._scenePath };
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x000C6551 File Offset: 0x000C4751
		private bool IsShowingUpgradedCard
		{
			get
			{
				return this._upgradeTickbox.IsTicked;
			}
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000C655E File Offset: 0x000C475E
		[NullableContext(2)]
		public static NInspectCardScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NInspectCardScreen._scenePath).Instantiate<NInspectCardScreen>(0L);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000C6580 File Offset: 0x000C4780
		public override void _Ready()
		{
			this._card = base.GetNode<NCard>("Card");
			this._cardPosition = this._card.Position;
			this._hoverTipRect = base.GetNode<Control>("HoverTipRect");
			this._backstop = base.GetNode<NButton>("Backstop");
			this._backstop.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackstopPressed)), 0U);
			this._leftButton = base.GetNode<NButton>("LeftArrow");
			this._leftButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.OnLeftButtonReleased();
			}), 0U);
			this._rightButton = base.GetNode<NButton>("RightArrow");
			this._rightButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.OnRightButtonReleased();
			}), 0U);
			this._leftButtonX = this._leftButton.Position.X;
			this._rightButtonX = this._rightButton.Position.X;
			this._upgradeTickbox = base.GetNode<NTickbox>("%Upgrade");
			this._upgradeTickbox.IsTicked = false;
			this._upgradeTickbox.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrade)), 0U);
			base.GetNode<MegaLabel>("%ShowUpgradeLabel").SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
			this._rightButton.Disable();
			this._leftButton.Disable();
			this._upgradeTickbox.Disable();
			this.Close();
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000C6738 File Offset: 0x000C4938
		public void Open(List<CardModel> cards, int index, bool viewAllUpgraded = false)
		{
			this._cards = cards;
			base.Visible = true;
			base.MouseFilter = 0L;
			this._viewAllUpgraded = viewAllUpgraded;
			this.SetCard(index);
			this._card.Scale = Vector2.One * 1.75f;
			this._card.Modulate = StsColors.transparentBlack;
			this._leftButton.Modulate = StsColors.transparentBlack;
			this._rightButton.Modulate = StsColors.transparentBlack;
			Tween openTween = this._openTween;
			if (openTween != null)
			{
				openTween.Kill();
			}
			this._openTween = base.CreateTween().SetParallel(true);
			this._openTween.TweenProperty(this._backstop, "modulate:a", 0.9f, 0.25);
			this._openTween.TweenProperty(this, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(5L)
				.From(0f);
			this._openTween.TweenProperty(this._leftButton, "position:x", this._leftButtonX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._leftButtonX + 100f)
				.SetDelay(0.1);
			this._openTween.TweenProperty(this._leftButton, "modulate", Colors.White, 0.25).SetDelay(0.1);
			this._openTween.TweenProperty(this._rightButton, "position:x", this._rightButtonX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._rightButtonX - 100f)
				.SetDelay(0.1);
			this._openTween.TweenProperty(this._rightButton, "modulate", Colors.White, 0.25).SetDelay(0.1);
			Tween cardTween = this._cardTween;
			if (cardTween != null)
			{
				cardTween.Kill();
			}
			this._cardTween = base.CreateTween().SetParallel(true);
			this._cardTween.TweenProperty(this._card, "modulate", Colors.White, 0.25);
			this._cardTween.TweenProperty(this._card, "scale", Vector2.One * 2f, 0.15).SetEase(1L).SetTrans(11L)
				.SetDelay(0.1);
			ActiveScreenContext.Instance.Update();
			NHotkeyManager.Instance.AddBlockingScreen(this);
			this._rightButton.Enable();
			this._leftButton.Enable();
			this._upgradeTickbox.Enable();
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.cancel, new Action(this.Close));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.pauseAndBack, new Action(this.Close));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.left, new Action(this.OnLeftButtonReleased));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.right, new Action(this.OnRightButtonReleased));
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000C6AEC File Offset: 0x000C4CEC
		public void Close()
		{
			if (!base.Visible)
			{
				return;
			}
			base.MouseFilter = 2L;
			this._leftButton.MouseFilter = 2L;
			this._rightButton.MouseFilter = 2L;
			this._rightButton.Disable();
			this._leftButton.Disable();
			this._upgradeTickbox.Disable();
			NHoverTipSet.Clear();
			base.SetProcessUnhandledInput(false);
			Tween openTween = this._openTween;
			if (openTween != null)
			{
				openTween.Kill();
			}
			this._openTween = base.CreateTween().SetParallel(true);
			this._openTween.TweenProperty(this._backstop, "modulate:a", 0f, 0.25);
			this._openTween.TweenProperty(this._leftButton, "modulate:a", 0f, 0.1);
			this._openTween.TweenProperty(this._rightButton, "modulate:a", 0f, 0.1);
			this._openTween.TweenProperty(this._card, "modulate", StsColors.transparentWhite, 0.1);
			this._openTween.Chain().TweenCallback(Callable.From(delegate
			{
				base.Visible = false;
				ActiveScreenContext.Instance.Update();
			}));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.cancel, new Action(this.Close));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.pauseAndBack, new Action(this.Close));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.left, new Action(this.OnLeftButtonReleased));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.right, new Action(this.OnRightButtonReleased));
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000C6CE0 File Offset: 0x000C4EE0
		private void OnRightButtonReleased()
		{
			if (!this._rightButton.Visible)
			{
				return;
			}
			this.SetCard(this._index + 1);
			this._card.Modulate = Colors.White;
			Tween openTween = this._openTween;
			if (openTween != null)
			{
				openTween.Kill();
			}
			this._openTween = base.CreateTween().SetParallel(true);
			this._openTween.TweenProperty(this._card, "position", this._cardPosition, 0.25).SetEase(1L).SetTrans(5L)
				.From(this._cardPosition + new Vector2(100f, 0f));
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000C6DA0 File Offset: 0x000C4FA0
		private void OnLeftButtonReleased()
		{
			if (!this._leftButton.Visible)
			{
				return;
			}
			this.SetCard(this._index - 1);
			this._card.Modulate = Colors.White;
			Tween openTween = this._openTween;
			if (openTween != null)
			{
				openTween.Kill();
			}
			this._openTween = base.CreateTween().SetParallel(true);
			this._openTween.TweenProperty(this._card, "position", this._cardPosition, 0.25).SetEase(1L).SetTrans(5L)
				.From(this._cardPosition + new Vector2(-100f, 0f));
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000C6E5E File Offset: 0x000C505E
		private void ToggleShowUpgrade(NTickbox _)
		{
			this._viewAllUpgraded = false;
			this.UpdateCardDisplay();
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000C6E70 File Offset: 0x000C5070
		private void UpdateCardDisplay()
		{
			CardModel cardModel = this._cards[this._index];
			CardModel cardModel2 = (CardModel)this._cards[this._index].MutableClone();
			if (this.IsShowingUpgradedCard)
			{
				if (!cardModel.IsUpgraded && cardModel.IsUpgradable)
				{
					cardModel2.UpgradePreviewType = CardUpgradePreviewType.Deck;
					cardModel2.UpgradeInternal();
				}
				this._card.Model = cardModel2;
				this._card.ShowUpgradePreview();
			}
			else
			{
				if (cardModel2.IsUpgraded)
				{
					CardCmd.Downgrade(cardModel2);
				}
				this._card.Model = cardModel2;
				this._card.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
			}
			NHoverTipSet.Clear();
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, cardModel2.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignment(this._hoverTipRect, HoverTip.GetHoverTipAlignment(this, 0.75f));
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000C6F40 File Offset: 0x000C5140
		private void SetCard(int index)
		{
			this._index = Math.Clamp(index, 0, this._cards.Count - 1);
			this._leftButton.Visible = this._index > 0;
			this._leftButton.MouseFilter = (this._leftButton.Visible ? 0L : 2L);
			this._rightButton.Visible = this._index < this._cards.Count - 1;
			this._rightButton.MouseFilter = (this._rightButton.Visible ? 0L : 2L);
			this._upgradeTickbox.Visible = this._cards[this._index].MaxUpgradeLevel > 0;
			this._upgradeTickbox.MouseFilter = ((this._cards[this._index].MaxUpgradeLevel > 0) ? 0L : 2L);
			if (this._cards[this._index].IsUpgraded || this._viewAllUpgraded)
			{
				this._upgradeTickbox.IsTicked = true;
			}
			else
			{
				this._upgradeTickbox.IsTicked = false;
			}
			this.UpdateCardDisplay();
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000C7065 File Offset: 0x000C5265
		private void OnBackstopPressed(NButton _)
		{
			this.Close();
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x000C706D File Offset: 0x000C526D
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E63 RID: 7779 RVA: 0x000C7070 File Offset: 0x000C5270
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NInspectCardScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName.OnRightButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName.OnLeftButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName.ToggleShowUpgrade, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NInspectCardScreen.MethodName.UpdateCardDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectCardScreen.MethodName.SetCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInspectCardScreen.MethodName.OnBackstopPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E64 RID: 7780 RVA: 0x000C72C0 File Offset: 0x000C54C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInspectCardScreen.MethodName.Create && args.Count == 0)
			{
				NInspectCardScreen ninspectCardScreen = NInspectCardScreen.Create();
				ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref ninspectCardScreen);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.OnRightButtonReleased && args.Count == 0)
			{
				this.OnRightButtonReleased();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.OnLeftButtonReleased && args.Count == 0)
			{
				this.OnLeftButtonReleased();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.ToggleShowUpgrade && args.Count == 1)
			{
				this.ToggleShowUpgrade(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.UpdateCardDisplay && args.Count == 0)
			{
				this.UpdateCardDisplay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.SetCard && args.Count == 1)
			{
				this.SetCard(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectCardScreen.MethodName.OnBackstopPressed && args.Count == 1)
			{
				this.OnBackstopPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000C7454 File Offset: 0x000C5654
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInspectCardScreen.MethodName.Create && args.Count == 0)
			{
				NInspectCardScreen ninspectCardScreen = NInspectCardScreen.Create();
				ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref ninspectCardScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001E66 RID: 7782 RVA: 0x000C7494 File Offset: 0x000C5694
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInspectCardScreen.MethodName.Create || (ref method) == NInspectCardScreen.MethodName._Ready || (ref method) == NInspectCardScreen.MethodName.Close || (ref method) == NInspectCardScreen.MethodName.OnRightButtonReleased || (ref method) == NInspectCardScreen.MethodName.OnLeftButtonReleased || (ref method) == NInspectCardScreen.MethodName.ToggleShowUpgrade || (ref method) == NInspectCardScreen.MethodName.UpdateCardDisplay || (ref method) == NInspectCardScreen.MethodName.SetCard || (ref method) == NInspectCardScreen.MethodName.OnBackstopPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001E67 RID: 7783 RVA: 0x000C7530 File Offset: 0x000C5730
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInspectCardScreen.PropertyName._card)
			{
				this._card = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._upgradeTickbox)
			{
				this._upgradeTickbox = VariantUtils.ConvertTo<NTickbox>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._leftButton)
			{
				this._leftButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._rightButton)
			{
				this._rightButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._hoverTipRect)
			{
				this._hoverTipRect = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._index)
			{
				this._index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._openTween)
			{
				this._openTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._cardPosition)
			{
				this._cardPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._leftButtonX)
			{
				this._leftButtonX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._rightButtonX)
			{
				this._rightButtonX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._viewAllUpgraded)
			{
				this._viewAllUpgraded = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001E68 RID: 7784 RVA: 0x000C76A4 File Offset: 0x000C58A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInspectCardScreen.PropertyName.IsShowingUpgradedCard)
			{
				bool isShowingUpgradedCard = this.IsShowingUpgradedCard;
				value = VariantUtils.CreateFrom<bool>(ref isShowingUpgradedCard);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._card)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._card);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._backstop);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._upgradeTickbox)
			{
				value = VariantUtils.CreateFrom<NTickbox>(ref this._upgradeTickbox);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._leftButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._leftButton);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._rightButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._rightButton);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._hoverTipRect)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hoverTipRect);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._index)
			{
				value = VariantUtils.CreateFrom<int>(ref this._index);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._openTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._openTween);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._cardPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._cardPosition);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._leftButtonX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._leftButtonX);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._rightButtonX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rightButtonX);
				return true;
			}
			if ((ref name) == NInspectCardScreen.PropertyName._viewAllUpgraded)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._viewAllUpgraded);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E69 RID: 7785 RVA: 0x000C78A0 File Offset: 0x000C5AA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._card, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._upgradeTickbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._leftButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._rightButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._hoverTipRect, 0L, "", 4096L, false),
				new PropertyInfo(2L, NInspectCardScreen.PropertyName._index, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._openTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NInspectCardScreen.PropertyName._cardPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NInspectCardScreen.PropertyName._leftButtonX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NInspectCardScreen.PropertyName._rightButtonX, 0L, "", 4096L, false),
				new PropertyInfo(1L, NInspectCardScreen.PropertyName._viewAllUpgraded, 0L, "", 4096L, false),
				new PropertyInfo(1L, NInspectCardScreen.PropertyName.IsShowingUpgradedCard, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectCardScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E6A RID: 7786 RVA: 0x000C7AA0 File Offset: 0x000C5CA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NInspectCardScreen.PropertyName._card, Variant.From<NCard>(ref this._card));
			info.AddProperty(NInspectCardScreen.PropertyName._backstop, Variant.From<NButton>(ref this._backstop));
			info.AddProperty(NInspectCardScreen.PropertyName._upgradeTickbox, Variant.From<NTickbox>(ref this._upgradeTickbox));
			info.AddProperty(NInspectCardScreen.PropertyName._leftButton, Variant.From<NButton>(ref this._leftButton));
			info.AddProperty(NInspectCardScreen.PropertyName._rightButton, Variant.From<NButton>(ref this._rightButton));
			info.AddProperty(NInspectCardScreen.PropertyName._hoverTipRect, Variant.From<Control>(ref this._hoverTipRect));
			info.AddProperty(NInspectCardScreen.PropertyName._index, Variant.From<int>(ref this._index));
			info.AddProperty(NInspectCardScreen.PropertyName._openTween, Variant.From<Tween>(ref this._openTween));
			info.AddProperty(NInspectCardScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
			info.AddProperty(NInspectCardScreen.PropertyName._cardPosition, Variant.From<Vector2>(ref this._cardPosition));
			info.AddProperty(NInspectCardScreen.PropertyName._leftButtonX, Variant.From<float>(ref this._leftButtonX));
			info.AddProperty(NInspectCardScreen.PropertyName._rightButtonX, Variant.From<float>(ref this._rightButtonX));
			info.AddProperty(NInspectCardScreen.PropertyName._viewAllUpgraded, Variant.From<bool>(ref this._viewAllUpgraded));
		}

		/// <inheritdoc />
		// Token: 0x06001E6B RID: 7787 RVA: 0x000C7BD4 File Offset: 0x000C5DD4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._card, ref variant))
			{
				this._card = variant.As<NCard>();
			}
			Variant variant2;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._backstop, ref variant2))
			{
				this._backstop = variant2.As<NButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._upgradeTickbox, ref variant3))
			{
				this._upgradeTickbox = variant3.As<NTickbox>();
			}
			Variant variant4;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._leftButton, ref variant4))
			{
				this._leftButton = variant4.As<NButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._rightButton, ref variant5))
			{
				this._rightButton = variant5.As<NButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._hoverTipRect, ref variant6))
			{
				this._hoverTipRect = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._index, ref variant7))
			{
				this._index = variant7.As<int>();
			}
			Variant variant8;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._openTween, ref variant8))
			{
				this._openTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._cardTween, ref variant9))
			{
				this._cardTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._cardPosition, ref variant10))
			{
				this._cardPosition = variant10.As<Vector2>();
			}
			Variant variant11;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._leftButtonX, ref variant11))
			{
				this._leftButtonX = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._rightButtonX, ref variant12))
			{
				this._rightButtonX = variant12.As<float>();
			}
			Variant variant13;
			if (info.TryGetProperty(NInspectCardScreen.PropertyName._viewAllUpgraded, ref variant13))
			{
				this._viewAllUpgraded = variant13.As<bool>();
			}
		}

		// Token: 0x04000BAA RID: 2986
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/inspect_card_screen");

		// Token: 0x04000BAB RID: 2987
		private NCard _card;

		// Token: 0x04000BAC RID: 2988
		private NButton _backstop;

		// Token: 0x04000BAD RID: 2989
		private NTickbox _upgradeTickbox;

		// Token: 0x04000BAE RID: 2990
		private NButton _leftButton;

		// Token: 0x04000BAF RID: 2991
		private NButton _rightButton;

		// Token: 0x04000BB0 RID: 2992
		private Control _hoverTipRect;

		// Token: 0x04000BB1 RID: 2993
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _cards;

		// Token: 0x04000BB2 RID: 2994
		private int _index;

		// Token: 0x04000BB3 RID: 2995
		[Nullable(2)]
		private Tween _openTween;

		// Token: 0x04000BB4 RID: 2996
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x04000BB5 RID: 2997
		private Vector2 _cardPosition;

		// Token: 0x04000BB6 RID: 2998
		private float _leftButtonX;

		// Token: 0x04000BB7 RID: 2999
		private float _rightButtonX;

		// Token: 0x04000BB8 RID: 3000
		private const double _arrowButtonDelay = 0.1;

		// Token: 0x04000BB9 RID: 3001
		private bool _viewAllUpgraded;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001266 RID: 4710
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004746 RID: 18246
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004747 RID: 18247
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04004748 RID: 18248
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'OnRightButtonReleased' method.
			/// </summary>
			// Token: 0x04004749 RID: 18249
			public static readonly StringName OnRightButtonReleased = "OnRightButtonReleased";

			/// <summary>
			/// Cached name for the 'OnLeftButtonReleased' method.
			/// </summary>
			// Token: 0x0400474A RID: 18250
			public static readonly StringName OnLeftButtonReleased = "OnLeftButtonReleased";

			/// <summary>
			/// Cached name for the 'ToggleShowUpgrade' method.
			/// </summary>
			// Token: 0x0400474B RID: 18251
			public static readonly StringName ToggleShowUpgrade = "ToggleShowUpgrade";

			/// <summary>
			/// Cached name for the 'UpdateCardDisplay' method.
			/// </summary>
			// Token: 0x0400474C RID: 18252
			public static readonly StringName UpdateCardDisplay = "UpdateCardDisplay";

			/// <summary>
			/// Cached name for the 'SetCard' method.
			/// </summary>
			// Token: 0x0400474D RID: 18253
			public static readonly StringName SetCard = "SetCard";

			/// <summary>
			/// Cached name for the 'OnBackstopPressed' method.
			/// </summary>
			// Token: 0x0400474E RID: 18254
			public static readonly StringName OnBackstopPressed = "OnBackstopPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001267 RID: 4711
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsShowingUpgradedCard' property.
			/// </summary>
			// Token: 0x0400474F RID: 18255
			public static readonly StringName IsShowingUpgradedCard = "IsShowingUpgradedCard";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004750 RID: 18256
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_card' field.
			/// </summary>
			// Token: 0x04004751 RID: 18257
			public static readonly StringName _card = "_card";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04004752 RID: 18258
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_upgradeTickbox' field.
			/// </summary>
			// Token: 0x04004753 RID: 18259
			public static readonly StringName _upgradeTickbox = "_upgradeTickbox";

			/// <summary>
			/// Cached name for the '_leftButton' field.
			/// </summary>
			// Token: 0x04004754 RID: 18260
			public static readonly StringName _leftButton = "_leftButton";

			/// <summary>
			/// Cached name for the '_rightButton' field.
			/// </summary>
			// Token: 0x04004755 RID: 18261
			public static readonly StringName _rightButton = "_rightButton";

			/// <summary>
			/// Cached name for the '_hoverTipRect' field.
			/// </summary>
			// Token: 0x04004756 RID: 18262
			public static readonly StringName _hoverTipRect = "_hoverTipRect";

			/// <summary>
			/// Cached name for the '_index' field.
			/// </summary>
			// Token: 0x04004757 RID: 18263
			public static readonly StringName _index = "_index";

			/// <summary>
			/// Cached name for the '_openTween' field.
			/// </summary>
			// Token: 0x04004758 RID: 18264
			public static readonly StringName _openTween = "_openTween";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x04004759 RID: 18265
			public static readonly StringName _cardTween = "_cardTween";

			/// <summary>
			/// Cached name for the '_cardPosition' field.
			/// </summary>
			// Token: 0x0400475A RID: 18266
			public static readonly StringName _cardPosition = "_cardPosition";

			/// <summary>
			/// Cached name for the '_leftButtonX' field.
			/// </summary>
			// Token: 0x0400475B RID: 18267
			public static readonly StringName _leftButtonX = "_leftButtonX";

			/// <summary>
			/// Cached name for the '_rightButtonX' field.
			/// </summary>
			// Token: 0x0400475C RID: 18268
			public static readonly StringName _rightButtonX = "_rightButtonX";

			/// <summary>
			/// Cached name for the '_viewAllUpgraded' field.
			/// </summary>
			// Token: 0x0400475D RID: 18269
			public static readonly StringName _viewAllUpgraded = "_viewAllUpgraded";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001268 RID: 4712
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
