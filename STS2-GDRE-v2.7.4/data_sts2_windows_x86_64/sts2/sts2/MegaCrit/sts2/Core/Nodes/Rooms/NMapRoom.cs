using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	/// <summary>
	/// The map room is what we enter at the start of each Act.
	/// </summary>
	// Token: 0x0200036C RID: 876
	[ScriptPath("res://src/Core/Nodes/Rooms/NMapRoom.cs")]
	public class NMapRoom : Control, IScreenContext
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x0017DAF1 File Offset: 0x0017BCF1
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				List<string> list = new List<string>();
				list.Add("res://scenes/rooms/map_room.tscn");
				list.AddRange(NMapScreen.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x0017DB14 File Offset: 0x0017BD14
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NMapRoom Create(ActModel act, int actIndex)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMapRoom nmapRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/map_room.tscn").Instantiate<NMapRoom>(0L);
			nmapRoom._act = act;
			nmapRoom._actIndex = actIndex;
			return nmapRoom;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x0017DB50 File Offset: 0x0017BD50
		public override void _Ready()
		{
			NMapScreen nmapScreen = NMapScreen.Instance.Open(false);
			NRun.Instance.GlobalUi.TopBar.Map.Disable();
			nmapScreen.SetTravelEnabled(true);
			NCapstoneContainer.Instance.Connect(NCapstoneContainer.SignalName.CapstoneClosed, Callable.From(new Action(this.ReopenMap)), 0U);
			NActBanner nactBanner = NActBanner.Create(this._act, this._actIndex);
			NRun.Instance.GlobalUi.MapScreen.AddChildSafely(nactBanner);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x0017DBD2 File Offset: 0x0017BDD2
		private void ReopenMap()
		{
			NMapScreen.Instance.Open(false);
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x0017DBE0 File Offset: 0x0017BDE0
		public override void _ExitTree()
		{
			NCapstoneContainer.Instance.Disconnect(NCapstoneContainer.SignalName.CapstoneClosed, Callable.From(new Action(this.ReopenMap)));
			NCapstoneContainer.Instance.Close();
			NRun instance = NRun.Instance;
			if (instance == null)
			{
				return;
			}
			instance.GlobalUi.TopBar.Map.Enable();
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x0017DC35 File Offset: 0x0017BE35
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
		// Token: 0x0600333D RID: 13117 RVA: 0x0017DC38 File Offset: 0x0017BE38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapRoom.MethodName.ReopenMap, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600333E RID: 13118 RVA: 0x0017DCDC File Offset: 0x0017BEDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapRoom.MethodName.ReopenMap && args.Count == 0)
			{
				this.ReopenMap();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600333F RID: 13119 RVA: 0x0017DD61 File Offset: 0x0017BF61
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapRoom.MethodName._Ready || (ref method) == NMapRoom.MethodName.ReopenMap || (ref method) == NMapRoom.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003340 RID: 13120 RVA: 0x0017DD97 File Offset: 0x0017BF97
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapRoom.PropertyName._actIndex)
			{
				this._actIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003341 RID: 13121 RVA: 0x0017DDBC File Offset: 0x0017BFBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapRoom.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMapRoom.PropertyName._actIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._actIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003342 RID: 13122 RVA: 0x0017DE14 File Offset: 0x0017C014
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMapRoom.PropertyName._actIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003343 RID: 13123 RVA: 0x0017DE69 File Offset: 0x0017C069
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapRoom.PropertyName._actIndex, Variant.From<int>(ref this._actIndex));
		}

		/// <inheritdoc />
		// Token: 0x06003344 RID: 13124 RVA: 0x0017DE88 File Offset: 0x0017C088
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapRoom.PropertyName._actIndex, ref variant))
			{
				this._actIndex = variant.As<int>();
			}
		}

		// Token: 0x040013E0 RID: 5088
		[Nullable(1)]
		private const string _scenePath = "res://scenes/rooms/map_room.tscn";

		// Token: 0x040013E1 RID: 5089
		[Nullable(1)]
		private ActModel _act;

		// Token: 0x040013E2 RID: 5090
		private int _actIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001690 RID: 5776
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040057E4 RID: 22500
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ReopenMap' method.
			/// </summary>
			// Token: 0x040057E5 RID: 22501
			public static readonly StringName ReopenMap = "ReopenMap";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040057E6 RID: 22502
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001691 RID: 5777
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040057E7 RID: 22503
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_actIndex' field.
			/// </summary>
			// Token: 0x040057E8 RID: 22504
			public static readonly StringName _actIndex = "_actIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001692 RID: 5778
		public class SignalName : Control.SignalName
		{
		}
	}
}
