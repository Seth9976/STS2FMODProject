using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A Node script which contains references to all systems which reside in the Top Bar, the bar at the top
	/// of the screen which displays gold, health, potions, and various buttons in the top right corner during a run.
	/// </summary>
	// Token: 0x020003EF RID: 1007
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NTopBar.cs")]
	public class NTopBar : Control
	{
		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003D87 RID: 15751 RVA: 0x001D755F File Offset: 0x001D575F
		// (set) Token: 0x06003D88 RID: 15752 RVA: 0x001D7567 File Offset: 0x001D5767
		public NTopBarMapButton Map { get; private set; }

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x001D7570 File Offset: 0x001D5770
		// (set) Token: 0x06003D8A RID: 15754 RVA: 0x001D7578 File Offset: 0x001D5778
		public NTopBarDeckButton Deck { get; private set; }

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003D8B RID: 15755 RVA: 0x001D7581 File Offset: 0x001D5781
		// (set) Token: 0x06003D8C RID: 15756 RVA: 0x001D7589 File Offset: 0x001D5789
		public NTopBarPauseButton Pause { get; private set; }

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x001D7592 File Offset: 0x001D5792
		// (set) Token: 0x06003D8E RID: 15758 RVA: 0x001D759A File Offset: 0x001D579A
		public NPotionContainer PotionContainer { get; private set; }

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x001D75A3 File Offset: 0x001D57A3
		// (set) Token: 0x06003D90 RID: 15760 RVA: 0x001D75AB File Offset: 0x001D57AB
		public NTopBarRoomIcon RoomIcon { get; private set; }

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x001D75B4 File Offset: 0x001D57B4
		// (set) Token: 0x06003D92 RID: 15762 RVA: 0x001D75BC File Offset: 0x001D57BC
		public NTopBarFloorIcon FloorIcon { get; private set; }

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x001D75C5 File Offset: 0x001D57C5
		// (set) Token: 0x06003D94 RID: 15764 RVA: 0x001D75CD File Offset: 0x001D57CD
		public NTopBarBossIcon BossIcon { get; private set; }

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003D95 RID: 15765 RVA: 0x001D75D6 File Offset: 0x001D57D6
		// (set) Token: 0x06003D96 RID: 15766 RVA: 0x001D75DE File Offset: 0x001D57DE
		public NTopBarGold Gold { get; private set; }

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003D97 RID: 15767 RVA: 0x001D75E7 File Offset: 0x001D57E7
		// (set) Token: 0x06003D98 RID: 15768 RVA: 0x001D75EF File Offset: 0x001D57EF
		public NTopBarHp Hp { get; private set; }

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x001D75F8 File Offset: 0x001D57F8
		// (set) Token: 0x06003D9A RID: 15770 RVA: 0x001D7600 File Offset: 0x001D5800
		public NTopBarPortrait Portrait { get; private set; }

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x001D7609 File Offset: 0x001D5809
		// (set) Token: 0x06003D9C RID: 15772 RVA: 0x001D7611 File Offset: 0x001D5811
		public NTopBarPortraitTip PortraitTip { get; private set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003D9D RID: 15773 RVA: 0x001D761A File Offset: 0x001D581A
		// (set) Token: 0x06003D9E RID: 15774 RVA: 0x001D7622 File Offset: 0x001D5822
		public NRunTimer Timer { get; private set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x001D762B File Offset: 0x001D582B
		// (set) Token: 0x06003DA0 RID: 15776 RVA: 0x001D7633 File Offset: 0x001D5833
		public Node TrailContainer { get; private set; }

		/// <summary>
		/// A control node that will automatically try to refocus on the current ScreenContext's FocusedControlFromTopBar.
		/// Used by things like the nodes in see <see cref="T:MegaCrit.Sts2.Core.Nodes.Relics.NRelicInventory" /> to dynamically controller navigate from the relic
		/// nodes back to the current active screen. Doing it this way circumvents us needing to call ActiveScreenContext.Update
		/// whenever we need update the top bar elements navigation back to the active screen.
		/// </summary>
		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x001D763C File Offset: 0x001D583C
		// (set) Token: 0x06003DA2 RID: 15778 RVA: 0x001D7644 File Offset: 0x001D5844
		public Control ActiveScreenProxy { get; private set; }

		// Token: 0x06003DA3 RID: 15779 RVA: 0x001D7650 File Offset: 0x001D5850
		public override void _Ready()
		{
			this.TrailContainer = base.GetNode<Node>("%TrailContainer");
			this.Map = base.GetNode<NTopBarMapButton>("%Map");
			this.Deck = base.GetNode<NTopBarDeckButton>("%Deck");
			this.Pause = base.GetNode<NTopBarPauseButton>("%PauseButton");
			this.PotionContainer = base.GetNode<NPotionContainer>("%PotionContainer");
			this.RoomIcon = base.GetNode<NTopBarRoomIcon>("%RoomIcon");
			this.FloorIcon = base.GetNode<NTopBarFloorIcon>("%FloorIcon");
			this.BossIcon = base.GetNode<NTopBarBossIcon>("%BossIcon");
			this.Gold = base.GetNode<NTopBarGold>("%TopBarGold");
			this.Hp = base.GetNode<NTopBarHp>("%TopBarHp");
			this.Portrait = base.GetNode<NTopBarPortrait>("%TopBarPortrait");
			this.PortraitTip = base.GetNode<NTopBarPortraitTip>("%TopBarPortraitTip");
			this.Timer = base.GetNode<NRunTimer>("%TimerContainer");
			this.ActiveScreenProxy = base.GetNode<Control>("%ActiveScreenProxy");
			this._achievementLock = base.GetNode<Control>("%AchievementLock");
			this._ascensionIcon = base.GetNode<Control>("%AscensionIcon");
			this._ascensionLabel = base.GetNode<MegaLabel>("%AscensionLabel");
			this._ascensionHsv = (ShaderMaterial)this._ascensionIcon.Material;
			this._modifiersContainer = base.GetNode<Control>("%Modifiers");
			this.ActiveScreenProxy.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				IScreenContext currentScreen = ActiveScreenContext.Instance.GetCurrentScreen();
				if (currentScreen == null)
				{
					return;
				}
				Control focusedControlFromTopBar = currentScreen.FocusedControlFromTopBar;
				if (focusedControlFromTopBar == null)
				{
					return;
				}
				focusedControlFromTopBar.TryGrabFocus();
			}), 0U);
			this._capstoneContainer = base.GetParent().GetNode<NCapstoneContainer>("%CapstoneScreenContainer");
			this._capstoneContainer.Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.ToggleAnimState)), 0U);
			this._capstoneContainer.Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.ToggleAnimState)), 0U);
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x001D7898 File Offset: 0x001D5A98
		public void Initialize(IRunState runState)
		{
			if (runState.AscensionLevel > 0)
			{
				if (runState.Players.Count > 1)
				{
					this._ascensionHsv.SetShaderParameter(NTopBar._h, 0.52f);
					this._ascensionHsv.SetShaderParameter(NTopBar._v, 1.2f);
					this._ascensionLabel.AddThemeColorOverride(NTopBar._fontOutlineTheme, NTopBar._blueLabelOutline);
				}
				else
				{
					this._ascensionHsv.SetShaderParameter(NTopBar._h, 1f);
					this._ascensionHsv.SetShaderParameter(NTopBar._v, 1f);
					this._ascensionLabel.AddThemeColorOverride(NTopBar._fontOutlineTheme, NTopBar._redLabelOutline);
				}
				this._ascensionIcon.Visible = true;
				this._ascensionLabel.SetTextAutoSize(runState.AscensionLevel.ToString());
			}
			this._achievementLock.Visible = runState.GameMode.AreAchievementsAndEpochsLocked();
			this._modifiersContainer.Visible = runState.Modifiers.Count > 0;
			foreach (ModifierModel modifierModel in runState.Modifiers)
			{
				NTopBarModifier ntopBarModifier = NTopBarModifier.Create(modifierModel);
				this._modifiersContainer.AddChildSafely(ntopBarModifier);
			}
			this._player = LocalContext.GetMe(runState);
			this.Deck.Initialize(this._player);
			this.RoomIcon.Initialize(runState);
			this.FloorIcon.Initialize(runState);
			this.BossIcon.Initialize(runState);
			this.Gold.Initialize(this._player);
			this.Hp.Initialize(this._player);
			this.Pause.Initialize(runState);
			this.Portrait.Initialize(this._player);
			this.PortraitTip.Initialize(runState);
			this.PotionContainer.Initialize(runState);
			this._player.RelicObtained += this.OnRelicsUpdated;
			this._player.RelicRemoved += this.OnRelicsUpdated;
			this._player.MaxPotionCountChanged += this.MaxPotionsChanged;
			Callable.From(new Action(this.UpdateNavigation)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x001D7AF0 File Offset: 0x001D5CF0
		public override void _EnterTree()
		{
			base._EnterTree();
			ActiveScreenContext.Instance.Updated += this.UpdateNavigation;
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x001D7B10 File Offset: 0x001D5D10
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.UpdateNavigation;
			if (this._player != null)
			{
				this._player.RelicObtained -= this.OnRelicsUpdated;
				this._player.RelicRemoved -= this.OnRelicsUpdated;
				this._player.MaxPotionCountChanged -= this.MaxPotionsChanged;
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x001D7B80 File Offset: 0x001D5D80
		private void ToggleAnimState(Node _)
		{
			this.Pause.ToggleAnimState();
			this.Deck.ToggleAnimState();
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x001D7B98 File Offset: 0x001D5D98
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideTopBar, false))
			{
				this.DebugHideTopBar();
			}
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x001D7BB0 File Offset: 0x001D5DB0
		private void DebugHideTopBar()
		{
			if (!this._isDebugHidden)
			{
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create("Hide Top Bar"));
				this.AnimHide();
			}
			else
			{
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create("Show Top Bar"));
				this.AnimShow();
			}
			this._isDebugHidden = !this._isDebugHidden;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x001D7C0C File Offset: 0x001D5E0C
		public void AnimHide()
		{
			base.FocusBehaviorRecursive = 1L;
			Tween hideTween = this._hideTween;
			if (hideTween != null)
			{
				hideTween.Kill();
			}
			this._hideTween = base.CreateTween();
			this._hideTween.TweenProperty(this, "position:y", -100f, 0.25).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x001D7C78 File Offset: 0x001D5E78
		public void AnimShow()
		{
			base.FocusBehaviorRecursive = 2L;
			Tween hideTween = this._hideTween;
			if (hideTween != null)
			{
				hideTween.Kill();
			}
			this._hideTween = base.CreateTween();
			this._hideTween.TweenProperty(this, "position:y", 0f, 0.25).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x001D7CE4 File Offset: 0x001D5EE4
		private void OnRelicsUpdated(RelicModel _)
		{
			Callable.From(new Action(this.UpdateNavigation)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x001D7D10 File Offset: 0x001D5F10
		private void MaxPotionsChanged(int _)
		{
			Callable.From(new Action(this.UpdateNavigation)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x001D7D3C File Offset: 0x001D5F3C
		private void UpdateNavigation()
		{
			Control control = NRun.Instance.GlobalUi.RelicInventory.RelicNodes.FirstOrDefault<NRelicInventoryHolder>();
			if (control == null)
			{
				return;
			}
			this.Gold.FocusNeighborBottom = control.GetPath();
			this.Hp.FocusNeighborBottom = control.GetPath();
			this.FloorIcon.FocusNeighborBottom = control.GetPath();
			this.RoomIcon.FocusNeighborBottom = control.GetPath();
			this.BossIcon.FocusNeighborBottom = control.GetPath();
			this.Gold.FocusNeighborTop = this.Gold.GetPath();
			this.Hp.FocusNeighborTop = this.Hp.GetPath();
			this.FloorIcon.FocusNeighborTop = this.FloorIcon.GetPath();
			this.RoomIcon.FocusNeighborTop = this.RoomIcon.GetPath();
			this.BossIcon.FocusNeighborTop = this.BossIcon.GetPath();
			this.Hp.FocusNeighborLeft = this.Hp.GetPath();
			this.Hp.FocusNeighborRight = this.Gold.GetPath();
			this.Gold.FocusNeighborLeft = this.Hp.GetPath();
			Control gold = this.Gold;
			Control firstPotionControl = this.PotionContainer.FirstPotionControl;
			gold.FocusNeighborRight = ((firstPotionControl != null) ? firstPotionControl.GetPath() : null);
			Control roomIcon = this.RoomIcon;
			Control lastPotionControl = this.PotionContainer.LastPotionControl;
			roomIcon.FocusNeighborLeft = ((lastPotionControl != null) ? lastPotionControl.GetPath() : null);
			this.RoomIcon.FocusNeighborRight = this.FloorIcon.GetPath();
			this.FloorIcon.FocusNeighborLeft = this.RoomIcon.GetPath();
			this.FloorIcon.FocusNeighborRight = this.BossIcon.GetPath();
			this.BossIcon.FocusNeighborLeft = this.FloorIcon.GetPath();
			this.BossIcon.FocusNeighborRight = this.BossIcon.GetPath();
			if (this.PortraitTip.ShowTip)
			{
				this.PortraitTip.FocusNeighborRight = this.Hp.GetPath();
				this.PortraitTip.FocusNeighborLeft = this.PortraitTip.GetPath();
				this.PortraitTip.FocusNeighborTop = this.PortraitTip.GetPath();
				this.PortraitTip.FocusNeighborBottom = this.PortraitTip.GetPath();
				this.Hp.FocusNeighborLeft = this.PortraitTip.GetPath();
			}
			Viewport viewport = base.GetViewport();
			if (viewport != null && viewport.GuiGetFocusOwner() == this.ActiveScreenProxy)
			{
				ActiveScreenContext.Instance.FocusOnDefaultControl();
			}
			if (this._modifiersContainer.Visible)
			{
				Control[] array = this._modifiersContainer.GetChildren(false).OfType<Control>().ToArray<Control>();
				this.BossIcon.FocusNeighborRight = array.First<Control>().GetPath();
				if (!this.BossIcon.IsVisible())
				{
					this.FloorIcon.FocusNeighborRight = array.First<Control>().GetPath();
				}
				for (int i = 0; i < array.Length; i++)
				{
					Control control2 = array[i];
					control2.FocusNeighborTop = control2.GetPath();
					control2.FocusNeighborBottom = control.GetPath();
					control2.FocusNeighborRight = ((i < array.Length - 1) ? array[i + 1].GetPath() : control2.GetPath());
					if (i > 0)
					{
						control2.FocusNeighborLeft = array[i - 1].GetPath();
					}
					else
					{
						control2.FocusNeighborLeft = (this.BossIcon.IsVisible() ? this.BossIcon.GetPath() : this.FloorIcon.GetPath());
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DAF RID: 15791 RVA: 0x001D80B4 File Offset: 0x001D62B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NTopBar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName.ToggleAnimState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NTopBar.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NTopBar.MethodName.DebugHideTopBar, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName.AnimHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName.AnimShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBar.MethodName.MaxPotionsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTopBar.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DB0 RID: 15792 RVA: 0x001D8328 File Offset: 0x001D6528
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.ToggleAnimState && args.Count == 1)
			{
				this.ToggleAnimState(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.DebugHideTopBar && args.Count == 0)
			{
				this.DebugHideTopBar();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.AnimHide && args.Count == 0)
			{
				this.AnimHide();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.AnimShow && args.Count == 0)
			{
				this.AnimShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.MaxPotionsChanged && args.Count == 1)
			{
				this.MaxPotionsChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBar.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003DB1 RID: 15793 RVA: 0x001D84DC File Offset: 0x001D66DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBar.MethodName._Ready || (ref method) == NTopBar.MethodName._EnterTree || (ref method) == NTopBar.MethodName._ExitTree || (ref method) == NTopBar.MethodName.ToggleAnimState || (ref method) == NTopBar.MethodName._Input || (ref method) == NTopBar.MethodName.DebugHideTopBar || (ref method) == NTopBar.MethodName.AnimHide || (ref method) == NTopBar.MethodName.AnimShow || (ref method) == NTopBar.MethodName.MaxPotionsChanged || (ref method) == NTopBar.MethodName.UpdateNavigation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003DB2 RID: 15794 RVA: 0x001D8588 File Offset: 0x001D6788
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBar.PropertyName.Map)
			{
				this.Map = VariantUtils.ConvertTo<NTopBarMapButton>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Deck)
			{
				this.Deck = VariantUtils.ConvertTo<NTopBarDeckButton>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Pause)
			{
				this.Pause = VariantUtils.ConvertTo<NTopBarPauseButton>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.PotionContainer)
			{
				this.PotionContainer = VariantUtils.ConvertTo<NPotionContainer>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.RoomIcon)
			{
				this.RoomIcon = VariantUtils.ConvertTo<NTopBarRoomIcon>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.FloorIcon)
			{
				this.FloorIcon = VariantUtils.ConvertTo<NTopBarFloorIcon>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.BossIcon)
			{
				this.BossIcon = VariantUtils.ConvertTo<NTopBarBossIcon>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Gold)
			{
				this.Gold = VariantUtils.ConvertTo<NTopBarGold>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Hp)
			{
				this.Hp = VariantUtils.ConvertTo<NTopBarHp>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Portrait)
			{
				this.Portrait = VariantUtils.ConvertTo<NTopBarPortrait>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.PortraitTip)
			{
				this.PortraitTip = VariantUtils.ConvertTo<NTopBarPortraitTip>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Timer)
			{
				this.Timer = VariantUtils.ConvertTo<NRunTimer>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.TrailContainer)
			{
				this.TrailContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.ActiveScreenProxy)
			{
				this.ActiveScreenProxy = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._capstoneContainer)
			{
				this._capstoneContainer = VariantUtils.ConvertTo<NCapstoneContainer>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._modifiersContainer)
			{
				this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._achievementLock)
			{
				this._achievementLock = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionIcon)
			{
				this._ascensionIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionLabel)
			{
				this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionHsv)
			{
				this._ascensionHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._hideTween)
			{
				this._hideTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._isDebugHidden)
			{
				this._isDebugHidden = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003DB3 RID: 15795 RVA: 0x001D87F0 File Offset: 0x001D69F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBar.PropertyName.Map)
			{
				NTopBarMapButton map = this.Map;
				value = VariantUtils.CreateFrom<NTopBarMapButton>(ref map);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Deck)
			{
				NTopBarDeckButton deck = this.Deck;
				value = VariantUtils.CreateFrom<NTopBarDeckButton>(ref deck);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Pause)
			{
				NTopBarPauseButton pause = this.Pause;
				value = VariantUtils.CreateFrom<NTopBarPauseButton>(ref pause);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.PotionContainer)
			{
				NPotionContainer potionContainer = this.PotionContainer;
				value = VariantUtils.CreateFrom<NPotionContainer>(ref potionContainer);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.RoomIcon)
			{
				NTopBarRoomIcon roomIcon = this.RoomIcon;
				value = VariantUtils.CreateFrom<NTopBarRoomIcon>(ref roomIcon);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.FloorIcon)
			{
				NTopBarFloorIcon floorIcon = this.FloorIcon;
				value = VariantUtils.CreateFrom<NTopBarFloorIcon>(ref floorIcon);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.BossIcon)
			{
				NTopBarBossIcon bossIcon = this.BossIcon;
				value = VariantUtils.CreateFrom<NTopBarBossIcon>(ref bossIcon);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Gold)
			{
				NTopBarGold gold = this.Gold;
				value = VariantUtils.CreateFrom<NTopBarGold>(ref gold);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Hp)
			{
				NTopBarHp hp = this.Hp;
				value = VariantUtils.CreateFrom<NTopBarHp>(ref hp);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Portrait)
			{
				NTopBarPortrait portrait = this.Portrait;
				value = VariantUtils.CreateFrom<NTopBarPortrait>(ref portrait);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.PortraitTip)
			{
				NTopBarPortraitTip portraitTip = this.PortraitTip;
				value = VariantUtils.CreateFrom<NTopBarPortraitTip>(ref portraitTip);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.Timer)
			{
				NRunTimer timer = this.Timer;
				value = VariantUtils.CreateFrom<NRunTimer>(ref timer);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.TrailContainer)
			{
				Node trailContainer = this.TrailContainer;
				value = VariantUtils.CreateFrom<Node>(ref trailContainer);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName.ActiveScreenProxy)
			{
				Control activeScreenProxy = this.ActiveScreenProxy;
				value = VariantUtils.CreateFrom<Control>(ref activeScreenProxy);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._capstoneContainer)
			{
				value = VariantUtils.CreateFrom<NCapstoneContainer>(ref this._capstoneContainer);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._modifiersContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._achievementLock)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._achievementLock);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ascensionIcon);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._ascensionHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._ascensionHsv);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._hideTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hideTween);
				return true;
			}
			if ((ref name) == NTopBar.PropertyName._isDebugHidden)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDebugHidden);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DB4 RID: 15796 RVA: 0x001D8AFC File Offset: 0x001D6CFC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBar.PropertyName._capstoneContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Map, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Deck, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Pause, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.PotionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.RoomIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.FloorIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.BossIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Gold, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Hp, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.PortraitTip, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.Timer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.TrailContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName.ActiveScreenProxy, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._modifiersContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._achievementLock, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._ascensionIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._ascensionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._ascensionHsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBar.PropertyName._hideTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTopBar.PropertyName._isDebugHidden, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DB5 RID: 15797 RVA: 0x001D8DE8 File Offset: 0x001D6FE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName map = NTopBar.PropertyName.Map;
			NTopBarMapButton map2 = this.Map;
			info.AddProperty(map, Variant.From<NTopBarMapButton>(ref map2));
			StringName deck = NTopBar.PropertyName.Deck;
			NTopBarDeckButton deck2 = this.Deck;
			info.AddProperty(deck, Variant.From<NTopBarDeckButton>(ref deck2));
			StringName pause = NTopBar.PropertyName.Pause;
			NTopBarPauseButton pause2 = this.Pause;
			info.AddProperty(pause, Variant.From<NTopBarPauseButton>(ref pause2));
			StringName potionContainer = NTopBar.PropertyName.PotionContainer;
			NPotionContainer potionContainer2 = this.PotionContainer;
			info.AddProperty(potionContainer, Variant.From<NPotionContainer>(ref potionContainer2));
			StringName roomIcon = NTopBar.PropertyName.RoomIcon;
			NTopBarRoomIcon roomIcon2 = this.RoomIcon;
			info.AddProperty(roomIcon, Variant.From<NTopBarRoomIcon>(ref roomIcon2));
			StringName floorIcon = NTopBar.PropertyName.FloorIcon;
			NTopBarFloorIcon floorIcon2 = this.FloorIcon;
			info.AddProperty(floorIcon, Variant.From<NTopBarFloorIcon>(ref floorIcon2));
			StringName bossIcon = NTopBar.PropertyName.BossIcon;
			NTopBarBossIcon bossIcon2 = this.BossIcon;
			info.AddProperty(bossIcon, Variant.From<NTopBarBossIcon>(ref bossIcon2));
			StringName gold = NTopBar.PropertyName.Gold;
			NTopBarGold gold2 = this.Gold;
			info.AddProperty(gold, Variant.From<NTopBarGold>(ref gold2));
			StringName hp = NTopBar.PropertyName.Hp;
			NTopBarHp hp2 = this.Hp;
			info.AddProperty(hp, Variant.From<NTopBarHp>(ref hp2));
			StringName portrait = NTopBar.PropertyName.Portrait;
			NTopBarPortrait portrait2 = this.Portrait;
			info.AddProperty(portrait, Variant.From<NTopBarPortrait>(ref portrait2));
			StringName portraitTip = NTopBar.PropertyName.PortraitTip;
			NTopBarPortraitTip portraitTip2 = this.PortraitTip;
			info.AddProperty(portraitTip, Variant.From<NTopBarPortraitTip>(ref portraitTip2));
			StringName timer = NTopBar.PropertyName.Timer;
			NRunTimer timer2 = this.Timer;
			info.AddProperty(timer, Variant.From<NRunTimer>(ref timer2));
			StringName trailContainer = NTopBar.PropertyName.TrailContainer;
			Node trailContainer2 = this.TrailContainer;
			info.AddProperty(trailContainer, Variant.From<Node>(ref trailContainer2));
			StringName activeScreenProxy = NTopBar.PropertyName.ActiveScreenProxy;
			Control activeScreenProxy2 = this.ActiveScreenProxy;
			info.AddProperty(activeScreenProxy, Variant.From<Control>(ref activeScreenProxy2));
			info.AddProperty(NTopBar.PropertyName._capstoneContainer, Variant.From<NCapstoneContainer>(ref this._capstoneContainer));
			info.AddProperty(NTopBar.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
			info.AddProperty(NTopBar.PropertyName._achievementLock, Variant.From<Control>(ref this._achievementLock));
			info.AddProperty(NTopBar.PropertyName._ascensionIcon, Variant.From<Control>(ref this._ascensionIcon));
			info.AddProperty(NTopBar.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
			info.AddProperty(NTopBar.PropertyName._ascensionHsv, Variant.From<ShaderMaterial>(ref this._ascensionHsv));
			info.AddProperty(NTopBar.PropertyName._hideTween, Variant.From<Tween>(ref this._hideTween));
			info.AddProperty(NTopBar.PropertyName._isDebugHidden, Variant.From<bool>(ref this._isDebugHidden));
		}

		/// <inheritdoc />
		// Token: 0x06003DB6 RID: 15798 RVA: 0x001D9014 File Offset: 0x001D7214
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBar.PropertyName.Map, ref variant))
			{
				this.Map = variant.As<NTopBarMapButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBar.PropertyName.Deck, ref variant2))
			{
				this.Deck = variant2.As<NTopBarDeckButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBar.PropertyName.Pause, ref variant3))
			{
				this.Pause = variant3.As<NTopBarPauseButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTopBar.PropertyName.PotionContainer, ref variant4))
			{
				this.PotionContainer = variant4.As<NPotionContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTopBar.PropertyName.RoomIcon, ref variant5))
			{
				this.RoomIcon = variant5.As<NTopBarRoomIcon>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTopBar.PropertyName.FloorIcon, ref variant6))
			{
				this.FloorIcon = variant6.As<NTopBarFloorIcon>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTopBar.PropertyName.BossIcon, ref variant7))
			{
				this.BossIcon = variant7.As<NTopBarBossIcon>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTopBar.PropertyName.Gold, ref variant8))
			{
				this.Gold = variant8.As<NTopBarGold>();
			}
			Variant variant9;
			if (info.TryGetProperty(NTopBar.PropertyName.Hp, ref variant9))
			{
				this.Hp = variant9.As<NTopBarHp>();
			}
			Variant variant10;
			if (info.TryGetProperty(NTopBar.PropertyName.Portrait, ref variant10))
			{
				this.Portrait = variant10.As<NTopBarPortrait>();
			}
			Variant variant11;
			if (info.TryGetProperty(NTopBar.PropertyName.PortraitTip, ref variant11))
			{
				this.PortraitTip = variant11.As<NTopBarPortraitTip>();
			}
			Variant variant12;
			if (info.TryGetProperty(NTopBar.PropertyName.Timer, ref variant12))
			{
				this.Timer = variant12.As<NRunTimer>();
			}
			Variant variant13;
			if (info.TryGetProperty(NTopBar.PropertyName.TrailContainer, ref variant13))
			{
				this.TrailContainer = variant13.As<Node>();
			}
			Variant variant14;
			if (info.TryGetProperty(NTopBar.PropertyName.ActiveScreenProxy, ref variant14))
			{
				this.ActiveScreenProxy = variant14.As<Control>();
			}
			Variant variant15;
			if (info.TryGetProperty(NTopBar.PropertyName._capstoneContainer, ref variant15))
			{
				this._capstoneContainer = variant15.As<NCapstoneContainer>();
			}
			Variant variant16;
			if (info.TryGetProperty(NTopBar.PropertyName._modifiersContainer, ref variant16))
			{
				this._modifiersContainer = variant16.As<Control>();
			}
			Variant variant17;
			if (info.TryGetProperty(NTopBar.PropertyName._achievementLock, ref variant17))
			{
				this._achievementLock = variant17.As<Control>();
			}
			Variant variant18;
			if (info.TryGetProperty(NTopBar.PropertyName._ascensionIcon, ref variant18))
			{
				this._ascensionIcon = variant18.As<Control>();
			}
			Variant variant19;
			if (info.TryGetProperty(NTopBar.PropertyName._ascensionLabel, ref variant19))
			{
				this._ascensionLabel = variant19.As<MegaLabel>();
			}
			Variant variant20;
			if (info.TryGetProperty(NTopBar.PropertyName._ascensionHsv, ref variant20))
			{
				this._ascensionHsv = variant20.As<ShaderMaterial>();
			}
			Variant variant21;
			if (info.TryGetProperty(NTopBar.PropertyName._hideTween, ref variant21))
			{
				this._hideTween = variant21.As<Tween>();
			}
			Variant variant22;
			if (info.TryGetProperty(NTopBar.PropertyName._isDebugHidden, ref variant22))
			{
				this._isDebugHidden = variant22.As<bool>();
			}
		}

		// Token: 0x040017F7 RID: 6135
		private NCapstoneContainer _capstoneContainer;

		// Token: 0x04001806 RID: 6150
		private static readonly StringName _fontOutlineTheme = "font_outline_color";

		// Token: 0x04001807 RID: 6151
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04001808 RID: 6152
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001809 RID: 6153
		private static readonly Color _redLabelOutline = new Color("593400");

		// Token: 0x0400180A RID: 6154
		private static readonly Color _blueLabelOutline = new Color("004759");

		// Token: 0x0400180B RID: 6155
		private Control _modifiersContainer;

		// Token: 0x0400180C RID: 6156
		private Control _achievementLock;

		// Token: 0x0400180D RID: 6157
		private Control _ascensionIcon;

		// Token: 0x0400180E RID: 6158
		private MegaLabel _ascensionLabel;

		// Token: 0x0400180F RID: 6159
		private ShaderMaterial _ascensionHsv;

		// Token: 0x04001810 RID: 6160
		[Nullable(2)]
		private Tween _hideTween;

		// Token: 0x04001811 RID: 6161
		private bool _isDebugHidden;

		// Token: 0x04001812 RID: 6162
		[Nullable(2)]
		private Player _player;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001891 RID: 6289
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006001 RID: 24577
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006002 RID: 24578
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006003 RID: 24579
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ToggleAnimState' method.
			/// </summary>
			// Token: 0x04006004 RID: 24580
			public static readonly StringName ToggleAnimState = "ToggleAnimState";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04006005 RID: 24581
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'DebugHideTopBar' method.
			/// </summary>
			// Token: 0x04006006 RID: 24582
			public static readonly StringName DebugHideTopBar = "DebugHideTopBar";

			/// <summary>
			/// Cached name for the 'AnimHide' method.
			/// </summary>
			// Token: 0x04006007 RID: 24583
			public static readonly StringName AnimHide = "AnimHide";

			/// <summary>
			/// Cached name for the 'AnimShow' method.
			/// </summary>
			// Token: 0x04006008 RID: 24584
			public static readonly StringName AnimShow = "AnimShow";

			/// <summary>
			/// Cached name for the 'MaxPotionsChanged' method.
			/// </summary>
			// Token: 0x04006009 RID: 24585
			public static readonly StringName MaxPotionsChanged = "MaxPotionsChanged";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x0400600A RID: 24586
			public static readonly StringName UpdateNavigation = "UpdateNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001892 RID: 6290
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Map' property.
			/// </summary>
			// Token: 0x0400600B RID: 24587
			public static readonly StringName Map = "Map";

			/// <summary>
			/// Cached name for the 'Deck' property.
			/// </summary>
			// Token: 0x0400600C RID: 24588
			public static readonly StringName Deck = "Deck";

			/// <summary>
			/// Cached name for the 'Pause' property.
			/// </summary>
			// Token: 0x0400600D RID: 24589
			public static readonly StringName Pause = "Pause";

			/// <summary>
			/// Cached name for the 'PotionContainer' property.
			/// </summary>
			// Token: 0x0400600E RID: 24590
			public static readonly StringName PotionContainer = "PotionContainer";

			/// <summary>
			/// Cached name for the 'RoomIcon' property.
			/// </summary>
			// Token: 0x0400600F RID: 24591
			public static readonly StringName RoomIcon = "RoomIcon";

			/// <summary>
			/// Cached name for the 'FloorIcon' property.
			/// </summary>
			// Token: 0x04006010 RID: 24592
			public static readonly StringName FloorIcon = "FloorIcon";

			/// <summary>
			/// Cached name for the 'BossIcon' property.
			/// </summary>
			// Token: 0x04006011 RID: 24593
			public static readonly StringName BossIcon = "BossIcon";

			/// <summary>
			/// Cached name for the 'Gold' property.
			/// </summary>
			// Token: 0x04006012 RID: 24594
			public static readonly StringName Gold = "Gold";

			/// <summary>
			/// Cached name for the 'Hp' property.
			/// </summary>
			// Token: 0x04006013 RID: 24595
			public static readonly StringName Hp = "Hp";

			/// <summary>
			/// Cached name for the 'Portrait' property.
			/// </summary>
			// Token: 0x04006014 RID: 24596
			public static readonly StringName Portrait = "Portrait";

			/// <summary>
			/// Cached name for the 'PortraitTip' property.
			/// </summary>
			// Token: 0x04006015 RID: 24597
			public static readonly StringName PortraitTip = "PortraitTip";

			/// <summary>
			/// Cached name for the 'Timer' property.
			/// </summary>
			// Token: 0x04006016 RID: 24598
			public static readonly StringName Timer = "Timer";

			/// <summary>
			/// Cached name for the 'TrailContainer' property.
			/// </summary>
			// Token: 0x04006017 RID: 24599
			public static readonly StringName TrailContainer = "TrailContainer";

			/// <summary>
			/// Cached name for the 'ActiveScreenProxy' property.
			/// </summary>
			// Token: 0x04006018 RID: 24600
			public static readonly StringName ActiveScreenProxy = "ActiveScreenProxy";

			/// <summary>
			/// Cached name for the '_capstoneContainer' field.
			/// </summary>
			// Token: 0x04006019 RID: 24601
			public static readonly StringName _capstoneContainer = "_capstoneContainer";

			/// <summary>
			/// Cached name for the '_modifiersContainer' field.
			/// </summary>
			// Token: 0x0400601A RID: 24602
			public static readonly StringName _modifiersContainer = "_modifiersContainer";

			/// <summary>
			/// Cached name for the '_achievementLock' field.
			/// </summary>
			// Token: 0x0400601B RID: 24603
			public static readonly StringName _achievementLock = "_achievementLock";

			/// <summary>
			/// Cached name for the '_ascensionIcon' field.
			/// </summary>
			// Token: 0x0400601C RID: 24604
			public static readonly StringName _ascensionIcon = "_ascensionIcon";

			/// <summary>
			/// Cached name for the '_ascensionLabel' field.
			/// </summary>
			// Token: 0x0400601D RID: 24605
			public static readonly StringName _ascensionLabel = "_ascensionLabel";

			/// <summary>
			/// Cached name for the '_ascensionHsv' field.
			/// </summary>
			// Token: 0x0400601E RID: 24606
			public static readonly StringName _ascensionHsv = "_ascensionHsv";

			/// <summary>
			/// Cached name for the '_hideTween' field.
			/// </summary>
			// Token: 0x0400601F RID: 24607
			public static readonly StringName _hideTween = "_hideTween";

			/// <summary>
			/// Cached name for the '_isDebugHidden' field.
			/// </summary>
			// Token: 0x04006020 RID: 24608
			public static readonly StringName _isDebugHidden = "_isDebugHidden";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001893 RID: 6291
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
