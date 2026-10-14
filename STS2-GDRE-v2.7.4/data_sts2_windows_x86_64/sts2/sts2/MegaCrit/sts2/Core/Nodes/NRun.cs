using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes
{
	/// <summary>
	/// The root Node which contains reference to all other game system Nodes in a given run.
	/// For example: Run.Node.TopBar.Settings can get you access to the TopBarSettingsButton node.
	/// </summary>
	// Token: 0x0200017A RID: 378
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/NRun.cs")]
	public class NRun : Control
	{
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x0006AF74 File Offset: 0x00069174
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/run.tscn");
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x0006AF80 File Offset: 0x00069180
		[Nullable(2)]
		public static NRun Instance
		{
			[NullableContext(2)]
			get
			{
				NGame instance = NGame.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.CurrentRunNode;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x0006AF94 File Offset: 0x00069194
		[Nullable(2)]
		public NCombatRoom CombatRoom
		{
			[NullableContext(2)]
			get
			{
				Control currentScene = this._roomContainer.CurrentScene;
				NCombatRoom ncombatRoom = currentScene as NCombatRoom;
				NCombatRoom ncombatRoom2;
				if (ncombatRoom == null)
				{
					NEventRoom neventRoom = currentScene as NEventRoom;
					if (neventRoom == null)
					{
						ncombatRoom2 = null;
					}
					else
					{
						ncombatRoom2 = neventRoom.EmbeddedCombatRoom;
					}
				}
				else
				{
					ncombatRoom2 = ncombatRoom;
				}
				return ncombatRoom2;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0006AFD3 File Offset: 0x000691D3
		[Nullable(2)]
		public NTreasureRoom TreasureRoom
		{
			[NullableContext(2)]
			get
			{
				return this._roomContainer.CurrentScene as NTreasureRoom;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x0006AFE5 File Offset: 0x000691E5
		[Nullable(2)]
		public NEventRoom EventRoom
		{
			[NullableContext(2)]
			get
			{
				return this._roomContainer.CurrentScene as NEventRoom;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x0006AFF7 File Offset: 0x000691F7
		[Nullable(2)]
		public NRestSiteRoom RestSiteRoom
		{
			[NullableContext(2)]
			get
			{
				return this._roomContainer.CurrentScene as NRestSiteRoom;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x0006B009 File Offset: 0x00069209
		[Nullable(2)]
		public NMapRoom MapRoom
		{
			[NullableContext(2)]
			get
			{
				return this._roomContainer.CurrentScene as NMapRoom;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x0006B01B File Offset: 0x0006921B
		[Nullable(2)]
		public NMerchantRoom MerchantRoom
		{
			[NullableContext(2)]
			get
			{
				return this._roomContainer.CurrentScene as NMerchantRoom;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0006B02D File Offset: 0x0006922D
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x0006B035 File Offset: 0x00069235
		public NGlobalUi GlobalUi { get; private set; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0006B03E File Offset: 0x0006923E
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x0006B046 File Offset: 0x00069246
		public NRunMusicController RunMusicController { get; private set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x0006B04F File Offset: 0x0006924F
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x0006B057 File Offset: 0x00069257
		public ScreenStateTracker ScreenStateTracker { get; private set; }

		// Token: 0x0600122E RID: 4654 RVA: 0x0006B060 File Offset: 0x00069260
		public static NRun Create(RunState state)
		{
			NRun nrun = PreloadManager.Cache.GetScene("res://scenes/run.tscn").Instantiate<NRun>(0L);
			nrun._state = state;
			return nrun;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0006B08C File Offset: 0x0006928C
		public override void _Ready()
		{
			this._roomContainer = base.GetNode<NSceneContainer>("%RoomContainer");
			this.GlobalUi = base.GetNode<NGlobalUi>("%GlobalUi");
			this.GlobalUi.Initialize(this._state);
			this.ScreenStateTracker = new ScreenStateTracker(this.GlobalUi.MapScreen, this.GlobalUi.CapstoneContainer, this.GlobalUi.Overlays);
			this.RunMusicController = base.GetNode<NRunMusicController>("%RunMusicController");
			this.RunMusicController.SetRunState(this._state);
			this.RunMusicController.UpdateMusic();
			this._seedLabel = base.GetNode<MegaLabel>("%DebugSeed");
			this._seedLabel.SetTextAutoSize(this._state.Rng.StringSeed);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0006B165 File Offset: 0x00069365
		public override void _Process(double delta)
		{
			RunManager.Instance.NetService.Update();
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0006B176 File Offset: 0x00069376
		public override void _Notification(int what)
		{
			if ((long)what == 1006L)
			{
				RunManager.Instance.CleanUp(false);
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0006B18D File Offset: 0x0006938D
		[NullableContext(2)]
		public void SetCurrentRoom(Control node)
		{
			if (node == null)
			{
				return;
			}
			this._roomContainer.SetCurrentScene(node);
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0006B1A9 File Offset: 0x000693A9
		public void ShowGameOverScreen(SerializableRun serializableRun)
		{
			NCapstoneContainer.Instance.Close();
			NMapScreen.Instance.Close(true);
			NOverlayStack.Instance.Push(NGameOverScreen.Create(this._state, serializableRun));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001234 RID: 4660 RVA: 0x0006B1D8 File Offset: 0x000693D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NRun.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRun.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRun.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRun.MethodName.SetCurrentRoom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001235 RID: 4661 RVA: 0x0006B324 File Offset: 0x00069524
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRun.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRun.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRun.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRun.MethodName.SetCurrentRoom && args.Count == 1)
			{
				this.SetCurrentRoom(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001236 RID: 4662 RVA: 0x0006B3F8 File Offset: 0x000695F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRun.MethodName._Ready || (ref method) == NRun.MethodName._Process || (ref method) == NRun.MethodName._Notification || (ref method) == NRun.MethodName.SetCurrentRoom || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001237 RID: 4663 RVA: 0x0006B448 File Offset: 0x00069648
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRun.PropertyName.GlobalUi)
			{
				this.GlobalUi = VariantUtils.ConvertTo<NGlobalUi>(ref value);
				return true;
			}
			if ((ref name) == NRun.PropertyName.RunMusicController)
			{
				this.RunMusicController = VariantUtils.ConvertTo<NRunMusicController>(ref value);
				return true;
			}
			if ((ref name) == NRun.PropertyName._cardScene)
			{
				this._cardScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NRun.PropertyName._roomContainer)
			{
				this._roomContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
				return true;
			}
			if ((ref name) == NRun.PropertyName._testButton)
			{
				this._testButton = VariantUtils.ConvertTo<Button>(ref value);
				return true;
			}
			if ((ref name) == NRun.PropertyName._seedLabel)
			{
				this._seedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001238 RID: 4664 RVA: 0x0006B500 File Offset: 0x00069700
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRun.PropertyName.CombatRoom)
			{
				NCombatRoom combatRoom = this.CombatRoom;
				value = VariantUtils.CreateFrom<NCombatRoom>(ref combatRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.TreasureRoom)
			{
				NTreasureRoom treasureRoom = this.TreasureRoom;
				value = VariantUtils.CreateFrom<NTreasureRoom>(ref treasureRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.EventRoom)
			{
				NEventRoom eventRoom = this.EventRoom;
				value = VariantUtils.CreateFrom<NEventRoom>(ref eventRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.RestSiteRoom)
			{
				NRestSiteRoom restSiteRoom = this.RestSiteRoom;
				value = VariantUtils.CreateFrom<NRestSiteRoom>(ref restSiteRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.MapRoom)
			{
				NMapRoom mapRoom = this.MapRoom;
				value = VariantUtils.CreateFrom<NMapRoom>(ref mapRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.MerchantRoom)
			{
				NMerchantRoom merchantRoom = this.MerchantRoom;
				value = VariantUtils.CreateFrom<NMerchantRoom>(ref merchantRoom);
				return true;
			}
			if ((ref name) == NRun.PropertyName.GlobalUi)
			{
				NGlobalUi globalUi = this.GlobalUi;
				value = VariantUtils.CreateFrom<NGlobalUi>(ref globalUi);
				return true;
			}
			if ((ref name) == NRun.PropertyName.RunMusicController)
			{
				NRunMusicController runMusicController = this.RunMusicController;
				value = VariantUtils.CreateFrom<NRunMusicController>(ref runMusicController);
				return true;
			}
			if ((ref name) == NRun.PropertyName._cardScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._cardScene);
				return true;
			}
			if ((ref name) == NRun.PropertyName._roomContainer)
			{
				value = VariantUtils.CreateFrom<NSceneContainer>(ref this._roomContainer);
				return true;
			}
			if ((ref name) == NRun.PropertyName._testButton)
			{
				value = VariantUtils.CreateFrom<Button>(ref this._testButton);
				return true;
			}
			if ((ref name) == NRun.PropertyName._seedLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._seedLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001239 RID: 4665 RVA: 0x0006B6B4 File Offset: 0x000698B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRun.PropertyName._cardScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NRun.PropertyName._roomContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName._testButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName._seedLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.CombatRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.TreasureRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.EventRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.RestSiteRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.MapRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.MerchantRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.GlobalUi, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRun.PropertyName.RunMusicController, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600123A RID: 4666 RVA: 0x0006B858 File Offset: 0x00069A58
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName globalUi = NRun.PropertyName.GlobalUi;
			NGlobalUi globalUi2 = this.GlobalUi;
			info.AddProperty(globalUi, Variant.From<NGlobalUi>(ref globalUi2));
			StringName runMusicController = NRun.PropertyName.RunMusicController;
			NRunMusicController runMusicController2 = this.RunMusicController;
			info.AddProperty(runMusicController, Variant.From<NRunMusicController>(ref runMusicController2));
			info.AddProperty(NRun.PropertyName._cardScene, Variant.From<PackedScene>(ref this._cardScene));
			info.AddProperty(NRun.PropertyName._roomContainer, Variant.From<NSceneContainer>(ref this._roomContainer));
			info.AddProperty(NRun.PropertyName._testButton, Variant.From<Button>(ref this._testButton));
			info.AddProperty(NRun.PropertyName._seedLabel, Variant.From<MegaLabel>(ref this._seedLabel));
		}

		/// <inheritdoc />
		// Token: 0x0600123B RID: 4667 RVA: 0x0006B8F8 File Offset: 0x00069AF8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRun.PropertyName.GlobalUi, ref variant))
			{
				this.GlobalUi = variant.As<NGlobalUi>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRun.PropertyName.RunMusicController, ref variant2))
			{
				this.RunMusicController = variant2.As<NRunMusicController>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRun.PropertyName._cardScene, ref variant3))
			{
				this._cardScene = variant3.As<PackedScene>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRun.PropertyName._roomContainer, ref variant4))
			{
				this._roomContainer = variant4.As<NSceneContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRun.PropertyName._testButton, ref variant5))
			{
				this._testButton = variant5.As<Button>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRun.PropertyName._seedLabel, ref variant6))
			{
				this._seedLabel = variant6.As<MegaLabel>();
			}
		}

		// Token: 0x04000665 RID: 1637
		private const string _scenePath = "res://scenes/run.tscn";

		// Token: 0x04000666 RID: 1638
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _cardScene;

		// Token: 0x04000667 RID: 1639
		private RunState _state;

		// Token: 0x04000668 RID: 1640
		private NSceneContainer _roomContainer;

		// Token: 0x04000669 RID: 1641
		private Button _testButton;

		// Token: 0x0400066A RID: 1642
		private MegaLabel _seedLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F7B RID: 3963
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003DC6 RID: 15814
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003DC7 RID: 15815
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04003DC8 RID: 15816
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'SetCurrentRoom' method.
			/// </summary>
			// Token: 0x04003DC9 RID: 15817
			public static readonly StringName SetCurrentRoom = "SetCurrentRoom";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F7C RID: 3964
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CombatRoom' property.
			/// </summary>
			// Token: 0x04003DCA RID: 15818
			public static readonly StringName CombatRoom = "CombatRoom";

			/// <summary>
			/// Cached name for the 'TreasureRoom' property.
			/// </summary>
			// Token: 0x04003DCB RID: 15819
			public static readonly StringName TreasureRoom = "TreasureRoom";

			/// <summary>
			/// Cached name for the 'EventRoom' property.
			/// </summary>
			// Token: 0x04003DCC RID: 15820
			public static readonly StringName EventRoom = "EventRoom";

			/// <summary>
			/// Cached name for the 'RestSiteRoom' property.
			/// </summary>
			// Token: 0x04003DCD RID: 15821
			public static readonly StringName RestSiteRoom = "RestSiteRoom";

			/// <summary>
			/// Cached name for the 'MapRoom' property.
			/// </summary>
			// Token: 0x04003DCE RID: 15822
			public static readonly StringName MapRoom = "MapRoom";

			/// <summary>
			/// Cached name for the 'MerchantRoom' property.
			/// </summary>
			// Token: 0x04003DCF RID: 15823
			public static readonly StringName MerchantRoom = "MerchantRoom";

			/// <summary>
			/// Cached name for the 'GlobalUi' property.
			/// </summary>
			// Token: 0x04003DD0 RID: 15824
			public static readonly StringName GlobalUi = "GlobalUi";

			/// <summary>
			/// Cached name for the 'RunMusicController' property.
			/// </summary>
			// Token: 0x04003DD1 RID: 15825
			public static readonly StringName RunMusicController = "RunMusicController";

			/// <summary>
			/// Cached name for the '_cardScene' field.
			/// </summary>
			// Token: 0x04003DD2 RID: 15826
			public static readonly StringName _cardScene = "_cardScene";

			/// <summary>
			/// Cached name for the '_roomContainer' field.
			/// </summary>
			// Token: 0x04003DD3 RID: 15827
			public static readonly StringName _roomContainer = "_roomContainer";

			/// <summary>
			/// Cached name for the '_testButton' field.
			/// </summary>
			// Token: 0x04003DD4 RID: 15828
			public static readonly StringName _testButton = "_testButton";

			/// <summary>
			/// Cached name for the '_seedLabel' field.
			/// </summary>
			// Token: 0x04003DD5 RID: 15829
			public static readonly StringName _seedLabel = "_seedLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F7D RID: 3965
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
