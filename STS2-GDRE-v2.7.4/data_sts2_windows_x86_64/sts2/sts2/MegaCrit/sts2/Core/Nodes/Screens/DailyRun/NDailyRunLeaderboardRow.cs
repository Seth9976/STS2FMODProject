using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x0200032D RID: 813
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboardRow.cs")]
	public class NDailyRunLeaderboardRow : Control
	{
		// Token: 0x06002D6A RID: 11626 RVA: 0x00145A1C File Offset: 0x00143C1C
		[return: Nullable(2)]
		public static NDailyRunLeaderboardRow Create(LeaderboardEntry entry, bool isYou)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDailyRunLeaderboardRow ndailyRunLeaderboardRow = PreloadManager.Cache.GetScene(NDailyRunLeaderboardRow._scenePath).Instantiate<NDailyRunLeaderboardRow>(0L);
			ndailyRunLeaderboardRow._entry = entry;
			ndailyRunLeaderboardRow._isYou = isYou;
			return ndailyRunLeaderboardRow;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x00145A58 File Offset: 0x00143C58
		public override void _Ready()
		{
			this._rank = base.GetNode<MegaLabel>("%Rank");
			this._floor = base.GetNode<MegaLabel>("%Floor");
			this._name = base.GetNode<MegaLabel>("%Name");
			this._badges = base.GetNode<MegaLabel>("%Badges");
			this._time = base.GetNode<MegaLabel>("%Time");
			IEnumerable<string> enumerable = this._entry.userIds.Select<ulong, string>((ulong id) => PlatformUtil.GetPlayerNameRaw(LeaderboardManager.CurrentPlatform, id));
			DecodedDailyScore decodedDailyScore = ScoreUtility.DecodeDailyScore(this._entry.score);
			if (!decodedDailyScore.isValid)
			{
				this.QueueFreeSafely();
				return;
			}
			MegaLabel rank = this._rank;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._entry.rank + 1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			rank.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			this._name.SetTextAutoSize(string.Join(",", enumerable));
			if (this._isYou)
			{
				this._name.Modulate = StsColors.blue;
			}
			MegaLabel floor = this._floor;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(decodedDailyScore.floors);
			floor.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			if (decodedDailyScore.victory == 2)
			{
				base.GetNode<Control>("%Tick").Visible = true;
			}
			MegaLabel badges = this._badges;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(decodedDailyScore.badges);
			badges.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			this._time.SetTextAutoSize(NDailyRunLeaderboardRow.FormatHoursAndMinutes(decodedDailyScore.runTime));
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x00145C14 File Offset: 0x00143E14
		private static string FormatHoursAndMinutes(int value)
		{
			if (value >= 9999)
			{
				return "--:--";
			}
			int num = value / 60;
			int num2 = value % 60;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2, "D2");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D6D RID: 11629 RVA: 0x00145C6C File Offset: 0x00143E6C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDailyRunLeaderboardRow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D6E RID: 11630 RVA: 0x00145D08 File Offset: 0x00143F08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardRow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes && args.Count == 1)
			{
				string text = NDailyRunLeaderboardRow.FormatHoursAndMinutes(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x00145D7C File Offset: 0x00143F7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes && args.Count == 1)
			{
				string text = NDailyRunLeaderboardRow.FormatHoursAndMinutes(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002D70 RID: 11632 RVA: 0x00145DCA File Offset: 0x00143FCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunLeaderboardRow.MethodName._Ready || (ref method) == NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D71 RID: 11633 RVA: 0x00145DF4 File Offset: 0x00143FF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._rank)
			{
				this._rank = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._name)
			{
				this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._floor)
			{
				this._floor = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._badges)
			{
				this._badges = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._isYou)
			{
				this._isYou = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D72 RID: 11634 RVA: 0x00145EAC File Offset: 0x001440AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._rank)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._rank);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._name)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._floor)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._floor);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._badges)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._badges);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._time);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardRow.PropertyName._isYou)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isYou);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D73 RID: 11635 RVA: 0x00145F84 File Offset: 0x00144184
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunLeaderboardRow.PropertyName._rank, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardRow.PropertyName._name, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardRow.PropertyName._floor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardRow.PropertyName._badges, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardRow.PropertyName._time, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDailyRunLeaderboardRow.PropertyName._isYou, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D74 RID: 11636 RVA: 0x00146060 File Offset: 0x00144260
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._rank, Variant.From<MegaLabel>(ref this._rank));
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._floor, Variant.From<MegaLabel>(ref this._floor));
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._badges, Variant.From<MegaLabel>(ref this._badges));
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._time, Variant.From<MegaLabel>(ref this._time));
			info.AddProperty(NDailyRunLeaderboardRow.PropertyName._isYou, Variant.From<bool>(ref this._isYou));
		}

		/// <inheritdoc />
		// Token: 0x06002D75 RID: 11637 RVA: 0x001460F8 File Offset: 0x001442F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._rank, ref variant))
			{
				this._rank = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._name, ref variant2))
			{
				this._name = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._floor, ref variant3))
			{
				this._floor = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._badges, ref variant4))
			{
				this._badges = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._time, ref variant5))
			{
				this._time = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._isYou, ref variant6))
			{
				this._isYou = variant6.As<bool>();
			}
		}

		// Token: 0x04001162 RID: 4450
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard_row");

		// Token: 0x04001163 RID: 4451
		private MegaLabel _rank;

		// Token: 0x04001164 RID: 4452
		private MegaLabel _name;

		// Token: 0x04001165 RID: 4453
		private MegaLabel _floor;

		// Token: 0x04001166 RID: 4454
		private MegaLabel _badges;

		// Token: 0x04001167 RID: 4455
		private MegaLabel _time;

		// Token: 0x04001168 RID: 4456
		private LeaderboardEntry _entry;

		// Token: 0x04001169 RID: 4457
		private bool _isYou;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001595 RID: 5525
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040052D8 RID: 21208
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'FormatHoursAndMinutes' method.
			/// </summary>
			// Token: 0x040052D9 RID: 21209
			public static readonly StringName FormatHoursAndMinutes = "FormatHoursAndMinutes";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001596 RID: 5526
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_rank' field.
			/// </summary>
			// Token: 0x040052DA RID: 21210
			public static readonly StringName _rank = "_rank";

			/// <summary>
			/// Cached name for the '_name' field.
			/// </summary>
			// Token: 0x040052DB RID: 21211
			public static readonly StringName _name = "_name";

			/// <summary>
			/// Cached name for the '_floor' field.
			/// </summary>
			// Token: 0x040052DC RID: 21212
			public static readonly StringName _floor = "_floor";

			/// <summary>
			/// Cached name for the '_badges' field.
			/// </summary>
			// Token: 0x040052DD RID: 21213
			public static readonly StringName _badges = "_badges";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x040052DE RID: 21214
			public static readonly StringName _time = "_time";

			/// <summary>
			/// Cached name for the '_isYou' field.
			/// </summary>
			// Token: 0x040052DF RID: 21215
			public static readonly StringName _isYou = "_isYou";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001597 RID: 5527
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
