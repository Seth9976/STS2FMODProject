using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002CD RID: 717
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NActHistoryEntry.cs")]
	public class NActHistoryEntry : HBoxContainer
	{
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060025CE RID: 9678 RVA: 0x000FF7D1 File Offset: 0x000FD9D1
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/run_history_screen/act_history_entry");
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060025CF RID: 9679 RVA: 0x000FF7DD File Offset: 0x000FD9DD
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NActHistoryEntry.ScenePath);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x000FF7E9 File Offset: 0x000FD9E9
		// (set) Token: 0x060025D1 RID: 9681 RVA: 0x000FF7F1 File Offset: 0x000FD9F1
		public List<NMapPointHistoryEntry> Entries { get; private set; } = new List<NMapPointHistoryEntry>();

		// Token: 0x060025D2 RID: 9682 RVA: 0x000FF7FC File Offset: 0x000FD9FC
		public override void _Ready()
		{
			this._actLabel = base.GetNode<MegaLabel>("%Title");
			this._actLabel.SetTextAutoSize(this._actName.GetFormattedText());
			for (int i = 0; i < this._entries.Count; i++)
			{
				NMapPointHistoryEntry nmapPointHistoryEntry = NMapPointHistoryEntry.Create(this._runHistory, this._entries[i], i + this._baseFloorNum);
				this.AddChildSafely(nmapPointHistoryEntry);
				this.Entries.Add(nmapPointHistoryEntry);
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000FF880 File Offset: 0x000FDA80
		public void SetPlayer(RunHistoryPlayer player)
		{
			foreach (NMapPointHistoryEntry nmapPointHistoryEntry in this.Entries)
			{
				nmapPointHistoryEntry.SetPlayer(player);
			}
		}

		/// <summary>
		/// Create an instance of this node.
		/// Null if we're in test mode.
		/// </summary>
		// Token: 0x060025D4 RID: 9684 RVA: 0x000FF8D4 File Offset: 0x000FDAD4
		[return: Nullable(2)]
		public static NActHistoryEntry Create(LocString actName, RunHistory runHistory, IReadOnlyList<MapPointHistoryEntry> logs, int baseFloorNum)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NActHistoryEntry nactHistoryEntry = PreloadManager.Cache.GetScene(NActHistoryEntry.ScenePath).Instantiate<NActHistoryEntry>(0L);
			nactHistoryEntry._actName = actName;
			nactHistoryEntry._runHistory = runHistory;
			nactHistoryEntry._entries = logs;
			nactHistoryEntry._baseFloorNum = baseFloorNum;
			return nactHistoryEntry;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025D5 RID: 9685 RVA: 0x000FF920 File Offset: 0x000FDB20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NActHistoryEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025D6 RID: 9686 RVA: 0x000FF964 File Offset: 0x000FDB64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NActHistoryEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060025D7 RID: 9687 RVA: 0x000FF994 File Offset: 0x000FDB94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NActHistoryEntry.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060025D8 RID: 9688 RVA: 0x000FF9AC File Offset: 0x000FDBAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NActHistoryEntry.PropertyName._actLabel)
			{
				this._actLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NActHistoryEntry.PropertyName._baseFloorNum)
			{
				this._baseFloorNum = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060025D9 RID: 9689 RVA: 0x000FF9EC File Offset: 0x000FDBEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NActHistoryEntry.PropertyName._actLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._actLabel);
				return true;
			}
			if ((ref name) == NActHistoryEntry.PropertyName._baseFloorNum)
			{
				value = VariantUtils.CreateFrom<int>(ref this._baseFloorNum);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025DA RID: 9690 RVA: 0x000FFA44 File Offset: 0x000FDC44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NActHistoryEntry.PropertyName._actLabel, 0L, "", 4096L, false),
				new PropertyInfo(2L, NActHistoryEntry.PropertyName._baseFloorNum, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025DB RID: 9691 RVA: 0x000FFA99 File Offset: 0x000FDC99
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NActHistoryEntry.PropertyName._actLabel, Variant.From<MegaLabel>(ref this._actLabel));
			info.AddProperty(NActHistoryEntry.PropertyName._baseFloorNum, Variant.From<int>(ref this._baseFloorNum));
		}

		/// <inheritdoc />
		// Token: 0x060025DC RID: 9692 RVA: 0x000FFAD0 File Offset: 0x000FDCD0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NActHistoryEntry.PropertyName._actLabel, ref variant))
			{
				this._actLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NActHistoryEntry.PropertyName._baseFloorNum, ref variant2))
			{
				this._baseFloorNum = variant2.As<int>();
			}
		}

		// Token: 0x04000E00 RID: 3584
		private MegaLabel _actLabel;

		// Token: 0x04000E01 RID: 3585
		private LocString _actName;

		// Token: 0x04000E02 RID: 3586
		private RunHistory _runHistory;

		// Token: 0x04000E03 RID: 3587
		private IReadOnlyList<MapPointHistoryEntry> _entries;

		/// <summary>
		/// Sum of floors at the start of this act
		/// </summary>
		// Token: 0x04000E05 RID: 3589
		private int _baseFloorNum;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001419 RID: 5145
		[NullableContext(0)]
		public class MethodName : HBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C67 RID: 19559
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200141A RID: 5146
		[NullableContext(0)]
		public class PropertyName : HBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_actLabel' field.
			/// </summary>
			// Token: 0x04004C68 RID: 19560
			public static readonly StringName _actLabel = "_actLabel";

			/// <summary>
			/// Cached name for the '_baseFloorNum' field.
			/// </summary>
			// Token: 0x04004C69 RID: 19561
			public static readonly StringName _baseFloorNum = "_baseFloorNum";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200141B RID: 5147
		[NullableContext(0)]
		public class SignalName : HBoxContainer.SignalName
		{
		}
	}
}
