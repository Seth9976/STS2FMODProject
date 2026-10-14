using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Container for run-level global UI elements.
	/// Also adjusts the Window size to meet aspect ratio requirements
	/// </summary>
	// Token: 0x020003DE RID: 990
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NGlobalUi.cs")]
	public class NGlobalUi : Control
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x001CCBE1 File Offset: 0x001CADE1
		// (set) Token: 0x06003C2C RID: 15404 RVA: 0x001CCBE9 File Offset: 0x001CADE9
		public NTopBar TopBar { get; private set; }

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x001CCBF2 File Offset: 0x001CADF2
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x001CCBFA File Offset: 0x001CADFA
		public NOverlayStack Overlays { get; private set; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x001CCC03 File Offset: 0x001CAE03
		// (set) Token: 0x06003C30 RID: 15408 RVA: 0x001CCC0B File Offset: 0x001CAE0B
		public NCapstoneContainer CapstoneContainer { get; private set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x001CCC14 File Offset: 0x001CAE14
		// (set) Token: 0x06003C32 RID: 15410 RVA: 0x001CCC1C File Offset: 0x001CAE1C
		public NRelicInventory RelicInventory { get; private set; }

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003C33 RID: 15411 RVA: 0x001CCC25 File Offset: 0x001CAE25
		// (set) Token: 0x06003C34 RID: 15412 RVA: 0x001CCC2D File Offset: 0x001CAE2D
		public Control EventCardPreviewContainer { get; private set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x001CCC36 File Offset: 0x001CAE36
		// (set) Token: 0x06003C36 RID: 15414 RVA: 0x001CCC3E File Offset: 0x001CAE3E
		public Control CardPreviewContainer { get; private set; }

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06003C37 RID: 15415 RVA: 0x001CCC47 File Offset: 0x001CAE47
		// (set) Token: 0x06003C38 RID: 15416 RVA: 0x001CCC4F File Offset: 0x001CAE4F
		public NMessyCardPreviewContainer MessyCardPreviewContainer { get; private set; }

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003C39 RID: 15417 RVA: 0x001CCC58 File Offset: 0x001CAE58
		// (set) Token: 0x06003C3A RID: 15418 RVA: 0x001CCC60 File Offset: 0x001CAE60
		public NGridCardPreviewContainer GridCardPreviewContainer { get; private set; }

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x001CCC69 File Offset: 0x001CAE69
		// (set) Token: 0x06003C3C RID: 15420 RVA: 0x001CCC71 File Offset: 0x001CAE71
		public Control AboveTopBarVfxContainer { get; private set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x001CCC7A File Offset: 0x001CAE7A
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x001CCC82 File Offset: 0x001CAE82
		public NMapScreen MapScreen { get; private set; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x001CCC8B File Offset: 0x001CAE8B
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x001CCC93 File Offset: 0x001CAE93
		public NMultiplayerPlayerStateContainer MultiplayerPlayerContainer { get; private set; }

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x001CCC9C File Offset: 0x001CAE9C
		// (set) Token: 0x06003C42 RID: 15426 RVA: 0x001CCCA4 File Offset: 0x001CAEA4
		public NMultiplayerTimeoutOverlay TimeoutOverlay { get; private set; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x001CCCAD File Offset: 0x001CAEAD
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x001CCCB5 File Offset: 0x001CAEB5
		public NCapstoneSubmenuStack SubmenuStack { get; private set; }

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x001CCCBE File Offset: 0x001CAEBE
		// (set) Token: 0x06003C46 RID: 15430 RVA: 0x001CCCC6 File Offset: 0x001CAEC6
		public NTargetManager TargetManager { get; private set; }

		// Token: 0x06003C47 RID: 15431 RVA: 0x001CCCD0 File Offset: 0x001CAED0
		public override void _Ready()
		{
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.EventCardPreviewContainer = base.GetNode<Control>("%EventCardPreviewContainer");
			this.CardPreviewContainer = base.GetNode<Control>("%CardPreviewContainer");
			this.MessyCardPreviewContainer = base.GetNode<NMessyCardPreviewContainer>("%MessyCardPreviewContainer");
			this.GridCardPreviewContainer = base.GetNode<NGridCardPreviewContainer>("%GridCardPreviewContainer");
			this.TopBar = base.GetNode<NTopBar>("%TopBar");
			this.Overlays = base.GetNode<NOverlayStack>("%OverlayScreensContainer");
			this.CapstoneContainer = base.GetNode<NCapstoneContainer>("%CapstoneScreenContainer");
			this.MapScreen = base.GetNode<NMapScreen>("%MapScreen");
			this.SubmenuStack = base.GetNode<NCapstoneSubmenuStack>("%CapstoneSubmenuStack");
			this.RelicInventory = base.GetNode<NRelicInventory>("%RelicInventory");
			this.MultiplayerPlayerContainer = base.GetNode<NMultiplayerPlayerStateContainer>("%MultiplayerPlayerContainer");
			this.TargetManager = base.GetNode<NTargetManager>("TargetManager");
			this.TimeoutOverlay = base.GetNode<NMultiplayerTimeoutOverlay>("%MultiplayerTimeoutOverlay");
			this.AboveTopBarVfxContainer = base.GetNode<Control>("%AboveTopBarVfxContainer");
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x001CCE48 File Offset: 0x001CB048
		private void OnWindowChange()
		{
			if (SaveManager.Instance.SettingsSave.AspectRatioSetting != AspectRatioSetting.Auto)
			{
				return;
			}
			float num = (float)this._window.Size.X / (float)this._window.Size.Y;
			if (num > 2.3888888f)
			{
				this._window.ContentScaleAspect = 2L;
				this._window.ContentScaleSize = new Vector2I(2580, 1080);
				return;
			}
			if (num < 1.3333334f)
			{
				this._window.ContentScaleAspect = 3L;
				this._window.ContentScaleSize = new Vector2I(1680, 1260);
				return;
			}
			this._window.ContentScaleAspect = 4L;
			this._window.ContentScaleSize = new Vector2I(1680, 1080);
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x001CCF14 File Offset: 0x001CB114
		public void ReparentCard(NCard card)
		{
			Vector2 globalPosition = card.GlobalPosition;
			Node parent = card.GetParent();
			if (parent != null)
			{
				parent.RemoveChildSafely(card);
			}
			this.TopBar.TrailContainer.AddChildSafely(card);
			card.GlobalPosition = globalPosition;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x001CCF54 File Offset: 0x001CB154
		public void Initialize(RunState runState)
		{
			this.TopBar.Initialize(runState);
			this.MultiplayerPlayerContainer.Initialize(runState);
			this.RelicInventory.Initialize(runState);
			this.MapScreen.Initialize(runState);
			this.TimeoutOverlay.Initialize(RunManager.Instance.NetService, false);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C4B RID: 15435 RVA: 0x001CCFA8 File Offset: 0x001CB1A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NGlobalUi.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalUi.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlobalUi.MethodName.ReparentCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C4C RID: 15436 RVA: 0x001CD07C File Offset: 0x001CB27C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGlobalUi.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalUi.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlobalUi.MethodName.ReparentCard && args.Count == 1)
			{
				this.ReparentCard(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C4D RID: 15437 RVA: 0x001CD10F File Offset: 0x001CB30F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGlobalUi.MethodName._Ready || (ref method) == NGlobalUi.MethodName.OnWindowChange || (ref method) == NGlobalUi.MethodName.ReparentCard || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003C4E RID: 15438 RVA: 0x001CD148 File Offset: 0x001CB348
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGlobalUi.PropertyName.TopBar)
			{
				this.TopBar = VariantUtils.ConvertTo<NTopBar>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.Overlays)
			{
				this.Overlays = VariantUtils.ConvertTo<NOverlayStack>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.CapstoneContainer)
			{
				this.CapstoneContainer = VariantUtils.ConvertTo<NCapstoneContainer>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.RelicInventory)
			{
				this.RelicInventory = VariantUtils.ConvertTo<NRelicInventory>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.EventCardPreviewContainer)
			{
				this.EventCardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.CardPreviewContainer)
			{
				this.CardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MessyCardPreviewContainer)
			{
				this.MessyCardPreviewContainer = VariantUtils.ConvertTo<NMessyCardPreviewContainer>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.GridCardPreviewContainer)
			{
				this.GridCardPreviewContainer = VariantUtils.ConvertTo<NGridCardPreviewContainer>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.AboveTopBarVfxContainer)
			{
				this.AboveTopBarVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MapScreen)
			{
				this.MapScreen = VariantUtils.ConvertTo<NMapScreen>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MultiplayerPlayerContainer)
			{
				this.MultiplayerPlayerContainer = VariantUtils.ConvertTo<NMultiplayerPlayerStateContainer>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.TimeoutOverlay)
			{
				this.TimeoutOverlay = VariantUtils.ConvertTo<NMultiplayerTimeoutOverlay>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.SubmenuStack)
			{
				this.SubmenuStack = VariantUtils.ConvertTo<NCapstoneSubmenuStack>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.TargetManager)
			{
				this.TargetManager = VariantUtils.ConvertTo<NTargetManager>(ref value);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003C4F RID: 15439 RVA: 0x001CD2F4 File Offset: 0x001CB4F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGlobalUi.PropertyName.TopBar)
			{
				NTopBar topBar = this.TopBar;
				value = VariantUtils.CreateFrom<NTopBar>(ref topBar);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.Overlays)
			{
				NOverlayStack overlays = this.Overlays;
				value = VariantUtils.CreateFrom<NOverlayStack>(ref overlays);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.CapstoneContainer)
			{
				NCapstoneContainer capstoneContainer = this.CapstoneContainer;
				value = VariantUtils.CreateFrom<NCapstoneContainer>(ref capstoneContainer);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.RelicInventory)
			{
				NRelicInventory relicInventory = this.RelicInventory;
				value = VariantUtils.CreateFrom<NRelicInventory>(ref relicInventory);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.EventCardPreviewContainer)
			{
				Control control = this.EventCardPreviewContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.CardPreviewContainer)
			{
				Control control = this.CardPreviewContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MessyCardPreviewContainer)
			{
				NMessyCardPreviewContainer messyCardPreviewContainer = this.MessyCardPreviewContainer;
				value = VariantUtils.CreateFrom<NMessyCardPreviewContainer>(ref messyCardPreviewContainer);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.GridCardPreviewContainer)
			{
				NGridCardPreviewContainer gridCardPreviewContainer = this.GridCardPreviewContainer;
				value = VariantUtils.CreateFrom<NGridCardPreviewContainer>(ref gridCardPreviewContainer);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.AboveTopBarVfxContainer)
			{
				Control control = this.AboveTopBarVfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MapScreen)
			{
				NMapScreen mapScreen = this.MapScreen;
				value = VariantUtils.CreateFrom<NMapScreen>(ref mapScreen);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.MultiplayerPlayerContainer)
			{
				NMultiplayerPlayerStateContainer multiplayerPlayerContainer = this.MultiplayerPlayerContainer;
				value = VariantUtils.CreateFrom<NMultiplayerPlayerStateContainer>(ref multiplayerPlayerContainer);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.TimeoutOverlay)
			{
				NMultiplayerTimeoutOverlay timeoutOverlay = this.TimeoutOverlay;
				value = VariantUtils.CreateFrom<NMultiplayerTimeoutOverlay>(ref timeoutOverlay);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.SubmenuStack)
			{
				NCapstoneSubmenuStack submenuStack = this.SubmenuStack;
				value = VariantUtils.CreateFrom<NCapstoneSubmenuStack>(ref submenuStack);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName.TargetManager)
			{
				NTargetManager targetManager = this.TargetManager;
				value = VariantUtils.CreateFrom<NTargetManager>(ref targetManager);
				return true;
			}
			if ((ref name) == NGlobalUi.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C50 RID: 15440 RVA: 0x001CD520 File Offset: 0x001CB720
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGlobalUi.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.TopBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.Overlays, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.CapstoneContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.RelicInventory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.EventCardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.CardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.MessyCardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.GridCardPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.AboveTopBarVfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.MapScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.MultiplayerPlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.TimeoutOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.SubmenuStack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlobalUi.PropertyName.TargetManager, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C51 RID: 15441 RVA: 0x001CD724 File Offset: 0x001CB924
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName topBar = NGlobalUi.PropertyName.TopBar;
			NTopBar topBar2 = this.TopBar;
			info.AddProperty(topBar, Variant.From<NTopBar>(ref topBar2));
			StringName overlays = NGlobalUi.PropertyName.Overlays;
			NOverlayStack overlays2 = this.Overlays;
			info.AddProperty(overlays, Variant.From<NOverlayStack>(ref overlays2));
			StringName capstoneContainer = NGlobalUi.PropertyName.CapstoneContainer;
			NCapstoneContainer capstoneContainer2 = this.CapstoneContainer;
			info.AddProperty(capstoneContainer, Variant.From<NCapstoneContainer>(ref capstoneContainer2));
			StringName relicInventory = NGlobalUi.PropertyName.RelicInventory;
			NRelicInventory relicInventory2 = this.RelicInventory;
			info.AddProperty(relicInventory, Variant.From<NRelicInventory>(ref relicInventory2));
			StringName eventCardPreviewContainer = NGlobalUi.PropertyName.EventCardPreviewContainer;
			Control control = this.EventCardPreviewContainer;
			info.AddProperty(eventCardPreviewContainer, Variant.From<Control>(ref control));
			StringName cardPreviewContainer = NGlobalUi.PropertyName.CardPreviewContainer;
			control = this.CardPreviewContainer;
			info.AddProperty(cardPreviewContainer, Variant.From<Control>(ref control));
			StringName messyCardPreviewContainer = NGlobalUi.PropertyName.MessyCardPreviewContainer;
			NMessyCardPreviewContainer messyCardPreviewContainer2 = this.MessyCardPreviewContainer;
			info.AddProperty(messyCardPreviewContainer, Variant.From<NMessyCardPreviewContainer>(ref messyCardPreviewContainer2));
			StringName gridCardPreviewContainer = NGlobalUi.PropertyName.GridCardPreviewContainer;
			NGridCardPreviewContainer gridCardPreviewContainer2 = this.GridCardPreviewContainer;
			info.AddProperty(gridCardPreviewContainer, Variant.From<NGridCardPreviewContainer>(ref gridCardPreviewContainer2));
			StringName aboveTopBarVfxContainer = NGlobalUi.PropertyName.AboveTopBarVfxContainer;
			control = this.AboveTopBarVfxContainer;
			info.AddProperty(aboveTopBarVfxContainer, Variant.From<Control>(ref control));
			StringName mapScreen = NGlobalUi.PropertyName.MapScreen;
			NMapScreen mapScreen2 = this.MapScreen;
			info.AddProperty(mapScreen, Variant.From<NMapScreen>(ref mapScreen2));
			StringName multiplayerPlayerContainer = NGlobalUi.PropertyName.MultiplayerPlayerContainer;
			NMultiplayerPlayerStateContainer multiplayerPlayerContainer2 = this.MultiplayerPlayerContainer;
			info.AddProperty(multiplayerPlayerContainer, Variant.From<NMultiplayerPlayerStateContainer>(ref multiplayerPlayerContainer2));
			StringName timeoutOverlay = NGlobalUi.PropertyName.TimeoutOverlay;
			NMultiplayerTimeoutOverlay timeoutOverlay2 = this.TimeoutOverlay;
			info.AddProperty(timeoutOverlay, Variant.From<NMultiplayerTimeoutOverlay>(ref timeoutOverlay2));
			StringName submenuStack = NGlobalUi.PropertyName.SubmenuStack;
			NCapstoneSubmenuStack submenuStack2 = this.SubmenuStack;
			info.AddProperty(submenuStack, Variant.From<NCapstoneSubmenuStack>(ref submenuStack2));
			StringName targetManager = NGlobalUi.PropertyName.TargetManager;
			NTargetManager targetManager2 = this.TargetManager;
			info.AddProperty(targetManager, Variant.From<NTargetManager>(ref targetManager2));
			info.AddProperty(NGlobalUi.PropertyName._window, Variant.From<Window>(ref this._window));
		}

		/// <inheritdoc />
		// Token: 0x06003C52 RID: 15442 RVA: 0x001CD8B8 File Offset: 0x001CBAB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGlobalUi.PropertyName.TopBar, ref variant))
			{
				this.TopBar = variant.As<NTopBar>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGlobalUi.PropertyName.Overlays, ref variant2))
			{
				this.Overlays = variant2.As<NOverlayStack>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGlobalUi.PropertyName.CapstoneContainer, ref variant3))
			{
				this.CapstoneContainer = variant3.As<NCapstoneContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGlobalUi.PropertyName.RelicInventory, ref variant4))
			{
				this.RelicInventory = variant4.As<NRelicInventory>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGlobalUi.PropertyName.EventCardPreviewContainer, ref variant5))
			{
				this.EventCardPreviewContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGlobalUi.PropertyName.CardPreviewContainer, ref variant6))
			{
				this.CardPreviewContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NGlobalUi.PropertyName.MessyCardPreviewContainer, ref variant7))
			{
				this.MessyCardPreviewContainer = variant7.As<NMessyCardPreviewContainer>();
			}
			Variant variant8;
			if (info.TryGetProperty(NGlobalUi.PropertyName.GridCardPreviewContainer, ref variant8))
			{
				this.GridCardPreviewContainer = variant8.As<NGridCardPreviewContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NGlobalUi.PropertyName.AboveTopBarVfxContainer, ref variant9))
			{
				this.AboveTopBarVfxContainer = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NGlobalUi.PropertyName.MapScreen, ref variant10))
			{
				this.MapScreen = variant10.As<NMapScreen>();
			}
			Variant variant11;
			if (info.TryGetProperty(NGlobalUi.PropertyName.MultiplayerPlayerContainer, ref variant11))
			{
				this.MultiplayerPlayerContainer = variant11.As<NMultiplayerPlayerStateContainer>();
			}
			Variant variant12;
			if (info.TryGetProperty(NGlobalUi.PropertyName.TimeoutOverlay, ref variant12))
			{
				this.TimeoutOverlay = variant12.As<NMultiplayerTimeoutOverlay>();
			}
			Variant variant13;
			if (info.TryGetProperty(NGlobalUi.PropertyName.SubmenuStack, ref variant13))
			{
				this.SubmenuStack = variant13.As<NCapstoneSubmenuStack>();
			}
			Variant variant14;
			if (info.TryGetProperty(NGlobalUi.PropertyName.TargetManager, ref variant14))
			{
				this.TargetManager = variant14.As<NTargetManager>();
			}
			Variant variant15;
			if (info.TryGetProperty(NGlobalUi.PropertyName._window, ref variant15))
			{
				this._window = variant15.As<Window>();
			}
		}

		// Token: 0x04001780 RID: 6016
		private const float _maxNarrowRatio = 1.3333334f;

		// Token: 0x04001781 RID: 6017
		private const float _maxWideRatio = 2.3888888f;

		// Token: 0x04001782 RID: 6018
		private Window _window;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184A RID: 6218
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F1D RID: 24349
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005F1E RID: 24350
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'ReparentCard' method.
			/// </summary>
			// Token: 0x04005F1F RID: 24351
			public static readonly StringName ReparentCard = "ReparentCard";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184B RID: 6219
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TopBar' property.
			/// </summary>
			// Token: 0x04005F20 RID: 24352
			public static readonly StringName TopBar = "TopBar";

			/// <summary>
			/// Cached name for the 'Overlays' property.
			/// </summary>
			// Token: 0x04005F21 RID: 24353
			public static readonly StringName Overlays = "Overlays";

			/// <summary>
			/// Cached name for the 'CapstoneContainer' property.
			/// </summary>
			// Token: 0x04005F22 RID: 24354
			public static readonly StringName CapstoneContainer = "CapstoneContainer";

			/// <summary>
			/// Cached name for the 'RelicInventory' property.
			/// </summary>
			// Token: 0x04005F23 RID: 24355
			public static readonly StringName RelicInventory = "RelicInventory";

			/// <summary>
			/// Cached name for the 'EventCardPreviewContainer' property.
			/// </summary>
			// Token: 0x04005F24 RID: 24356
			public static readonly StringName EventCardPreviewContainer = "EventCardPreviewContainer";

			/// <summary>
			/// Cached name for the 'CardPreviewContainer' property.
			/// </summary>
			// Token: 0x04005F25 RID: 24357
			public static readonly StringName CardPreviewContainer = "CardPreviewContainer";

			/// <summary>
			/// Cached name for the 'MessyCardPreviewContainer' property.
			/// </summary>
			// Token: 0x04005F26 RID: 24358
			public static readonly StringName MessyCardPreviewContainer = "MessyCardPreviewContainer";

			/// <summary>
			/// Cached name for the 'GridCardPreviewContainer' property.
			/// </summary>
			// Token: 0x04005F27 RID: 24359
			public static readonly StringName GridCardPreviewContainer = "GridCardPreviewContainer";

			/// <summary>
			/// Cached name for the 'AboveTopBarVfxContainer' property.
			/// </summary>
			// Token: 0x04005F28 RID: 24360
			public static readonly StringName AboveTopBarVfxContainer = "AboveTopBarVfxContainer";

			/// <summary>
			/// Cached name for the 'MapScreen' property.
			/// </summary>
			// Token: 0x04005F29 RID: 24361
			public static readonly StringName MapScreen = "MapScreen";

			/// <summary>
			/// Cached name for the 'MultiplayerPlayerContainer' property.
			/// </summary>
			// Token: 0x04005F2A RID: 24362
			public static readonly StringName MultiplayerPlayerContainer = "MultiplayerPlayerContainer";

			/// <summary>
			/// Cached name for the 'TimeoutOverlay' property.
			/// </summary>
			// Token: 0x04005F2B RID: 24363
			public static readonly StringName TimeoutOverlay = "TimeoutOverlay";

			/// <summary>
			/// Cached name for the 'SubmenuStack' property.
			/// </summary>
			// Token: 0x04005F2C RID: 24364
			public static readonly StringName SubmenuStack = "SubmenuStack";

			/// <summary>
			/// Cached name for the 'TargetManager' property.
			/// </summary>
			// Token: 0x04005F2D RID: 24365
			public static readonly StringName TargetManager = "TargetManager";

			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04005F2E RID: 24366
			public static readonly StringName _window = "_window";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184C RID: 6220
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
