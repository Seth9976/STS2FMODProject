using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002D0 RID: 720
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NMapPointHistory.cs")]
	public class NMapPointHistory : Control
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x00100FBE File Offset: 0x000FF1BE
		private List<NMapPointHistoryEntry> MapHistories
		{
			get
			{
				return this._actHistories.SelectMany<NActHistoryEntry, NMapPointHistoryEntry>((NActHistoryEntry a) => a.Entries).ToList<NMapPointHistoryEntry>();
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x00100FEF File Offset: 0x000FF1EF
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>();
			}
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x00100FFC File Offset: 0x000FF1FC
		public override void _Ready()
		{
			this._actContainer = base.GetNode<Control>("%Acts");
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x00101014 File Offset: 0x000FF214
		public void LoadHistory(RunHistory history)
		{
			foreach (Node node in this._actContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			this._actHistories.Clear();
			int num = 1;
			for (int i = 0; i < history.MapPointHistory.Count; i++)
			{
				LocString title = SaveUtil.ActOrDeprecated(history.Acts[i]).Title;
				NActHistoryEntry nactHistoryEntry = NActHistoryEntry.Create(title, history, history.MapPointHistory[i], num);
				this._actContainer.AddChildSafely(nactHistoryEntry);
				this._actHistories.Add(nactHistoryEntry);
				num += history.MapPointHistory[i].Count;
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x001010E8 File Offset: 0x000FF2E8
		public void SetPlayer(RunHistoryPlayer player)
		{
			foreach (NActHistoryEntry nactHistoryEntry in this._actHistories)
			{
				nactHistoryEntry.SetPlayer(player);
			}
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0010113C File Offset: 0x000FF33C
		public void SetDeckHistory(NDeckHistory deckHistory)
		{
			deckHistory.Connect(NDeckHistory.SignalName.Hovered, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.HighlightRelevantEntries)), 0U);
			deckHistory.Connect(NDeckHistory.SignalName.Unhovered, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.UnHighlightEntries)), 0U);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0010117A File Offset: 0x000FF37A
		public void SetRelicHistory(NRelicHistory relicHistory)
		{
			relicHistory.Connect(NRelicHistory.SignalName.Hovered, Callable.From<NRelicBasicHolder>(new Action<NRelicBasicHolder>(this.HighlightRelevantEntries)), 0U);
			relicHistory.Connect(NRelicHistory.SignalName.Unhovered, Callable.From<NRelicBasicHolder>(new Action<NRelicBasicHolder>(this.UnHighlightEntries)), 0U);
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x001011B8 File Offset: 0x000FF3B8
		private void HighlightRelevantEntries(NDeckHistoryEntry historyEntry)
		{
			using (IEnumerator<int> enumerator = historyEntry.FloorsAddedToDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int floorNumber = enumerator.Current;
					NMapPointHistoryEntry nmapPointHistoryEntry = this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>((NMapPointHistoryEntry e) => e.FloorNum == floorNumber);
					if (nmapPointHistoryEntry != null)
					{
						nmapPointHistoryEntry.Highlight();
					}
				}
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0010122C File Offset: 0x000FF42C
		private void HighlightRelevantEntries(NRelicBasicHolder holder)
		{
			if (holder.Relic.Model.FloorAddedToDeck > 0)
			{
				NMapPointHistoryEntry nmapPointHistoryEntry = this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>((NMapPointHistoryEntry e) => e.FloorNum == holder.Relic.Model.FloorAddedToDeck);
				if (nmapPointHistoryEntry != null)
				{
					nmapPointHistoryEntry.Highlight();
				}
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0010127F File Offset: 0x000FF47F
		private void UnHighlightEntries(NRelicBasicHolder _)
		{
			this.UnHighlightEntries();
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00101287 File Offset: 0x000FF487
		private void UnHighlightEntries(NDeckHistoryEntry _)
		{
			this.UnHighlightEntries();
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x00101290 File Offset: 0x000FF490
		private void UnHighlightEntries()
		{
			foreach (NMapPointHistoryEntry nmapPointHistoryEntry in this.MapHistories)
			{
				nmapPointHistoryEntry.Unhighlight();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600261C RID: 9756 RVA: 0x001012E4 File Offset: 0x000FF4E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NMapPointHistory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistory.MethodName.SetDeckHistory, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "deckHistory", 0L, "", 6L, new StringName("VBoxContainer"), false)
				}, null),
				new MethodInfo(NMapPointHistory.MethodName.SetRelicHistory, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relicHistory", 0L, "", 6L, new StringName("VBoxContainer"), false)
				}, null),
				new MethodInfo(NMapPointHistory.MethodName.HighlightRelevantEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "historyEntry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapPointHistory.MethodName.UnHighlightEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapPointHistory.MethodName.UnHighlightEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600261D RID: 9757 RVA: 0x001014D4 File Offset: 0x000FF6D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPointHistory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistory.MethodName.SetDeckHistory && args.Count == 1)
			{
				this.SetDeckHistory(VariantUtils.ConvertTo<NDeckHistory>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistory.MethodName.SetRelicHistory && args.Count == 1)
			{
				this.SetRelicHistory(VariantUtils.ConvertTo<NRelicHistory>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistory.MethodName.HighlightRelevantEntries && args.Count == 1)
			{
				this.HighlightRelevantEntries(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistory.MethodName.UnHighlightEntries && args.Count == 1)
			{
				this.UnHighlightEntries(VariantUtils.ConvertTo<NRelicBasicHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistory.MethodName.UnHighlightEntries && args.Count == 0)
			{
				this.UnHighlightEntries();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600261E RID: 9758 RVA: 0x00101600 File Offset: 0x000FF800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapPointHistory.MethodName._Ready || (ref method) == NMapPointHistory.MethodName.SetDeckHistory || (ref method) == NMapPointHistory.MethodName.SetRelicHistory || (ref method) == NMapPointHistory.MethodName.HighlightRelevantEntries || (ref method) == NMapPointHistory.MethodName.UnHighlightEntries || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600261F RID: 9759 RVA: 0x0010165F File Offset: 0x000FF85F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapPointHistory.PropertyName._actContainer)
			{
				this._actContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002620 RID: 9760 RVA: 0x00101684 File Offset: 0x000FF884
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapPointHistory.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMapPointHistory.PropertyName._actContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._actContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002621 RID: 9761 RVA: 0x001016DC File Offset: 0x000FF8DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapPointHistory.PropertyName._actContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistory.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002622 RID: 9762 RVA: 0x00101732 File Offset: 0x000FF932
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapPointHistory.PropertyName._actContainer, Variant.From<Control>(ref this._actContainer));
		}

		/// <inheritdoc />
		// Token: 0x06002623 RID: 9763 RVA: 0x00101754 File Offset: 0x000FF954
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapPointHistory.PropertyName._actContainer, ref variant))
			{
				this._actContainer = variant.As<Control>();
			}
		}

		// Token: 0x04000E16 RID: 3606
		private Control _actContainer;

		// Token: 0x04000E17 RID: 3607
		private readonly List<NActHistoryEntry> _actHistories = new List<NActHistoryEntry>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001427 RID: 5159
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C82 RID: 19586
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetDeckHistory' method.
			/// </summary>
			// Token: 0x04004C83 RID: 19587
			public static readonly StringName SetDeckHistory = "SetDeckHistory";

			/// <summary>
			/// Cached name for the 'SetRelicHistory' method.
			/// </summary>
			// Token: 0x04004C84 RID: 19588
			public static readonly StringName SetRelicHistory = "SetRelicHistory";

			/// <summary>
			/// Cached name for the 'HighlightRelevantEntries' method.
			/// </summary>
			// Token: 0x04004C85 RID: 19589
			public static readonly StringName HighlightRelevantEntries = "HighlightRelevantEntries";

			/// <summary>
			/// Cached name for the 'UnHighlightEntries' method.
			/// </summary>
			// Token: 0x04004C86 RID: 19590
			public static readonly StringName UnHighlightEntries = "UnHighlightEntries";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001428 RID: 5160
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004C87 RID: 19591
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_actContainer' field.
			/// </summary>
			// Token: 0x04004C88 RID: 19592
			public static readonly StringName _actContainer = "_actContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001429 RID: 5161
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
