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
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x0200032C RID: 812
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboardHeader.cs")]
	public class NDailyRunLeaderboardHeader : Control
	{
		// Token: 0x06002D5D RID: 11613 RVA: 0x00145630 File Offset: 0x00143830
		[NullableContext(2)]
		public static NDailyRunLeaderboardHeader Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDailyRunLeaderboardHeader._scenePath).Instantiate<NDailyRunLeaderboardHeader>(0L);
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x0014565E File Offset: 0x0014385E
		public override void _Ready()
		{
			this._name = base.GetNode<MegaLabel>("%Name");
			this._name.SetTextAutoSize(new LocString("main_menu_ui", "LEADERBOARDS.nameHeader").GetRawText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D5F RID: 11615 RVA: 0x00145698 File Offset: 0x00143898
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDailyRunLeaderboardHeader.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NDailyRunLeaderboardHeader.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D60 RID: 11616 RVA: 0x00145718 File Offset: 0x00143918
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardHeader.MethodName.Create && args.Count == 0)
			{
				NDailyRunLeaderboardHeader ndailyRunLeaderboardHeader = NDailyRunLeaderboardHeader.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLeaderboardHeader>(ref ndailyRunLeaderboardHeader);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboardHeader.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x00145780 File Offset: 0x00143980
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardHeader.MethodName.Create && args.Count == 0)
			{
				NDailyRunLeaderboardHeader ndailyRunLeaderboardHeader = NDailyRunLeaderboardHeader.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLeaderboardHeader>(ref ndailyRunLeaderboardHeader);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002D62 RID: 11618 RVA: 0x001457C0 File Offset: 0x001439C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunLeaderboardHeader.MethodName.Create || (ref method) == NDailyRunLeaderboardHeader.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D63 RID: 11619 RVA: 0x001457E8 File Offset: 0x001439E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._rank)
			{
				this._rank = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._name)
			{
				this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._score)
			{
				this._score = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D64 RID: 11620 RVA: 0x00145850 File Offset: 0x00143A50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._rank)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._rank);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._name)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboardHeader.PropertyName._score)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._score);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D65 RID: 11621 RVA: 0x001458C8 File Offset: 0x00143AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunLeaderboardHeader.PropertyName._rank, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardHeader.PropertyName._name, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboardHeader.PropertyName._score, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D66 RID: 11622 RVA: 0x00145940 File Offset: 0x00143B40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._rank, Variant.From<MegaLabel>(ref this._rank));
			info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
			info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._score, Variant.From<MegaLabel>(ref this._score));
		}

		/// <inheritdoc />
		// Token: 0x06002D67 RID: 11623 RVA: 0x00145998 File Offset: 0x00143B98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._rank, ref variant))
			{
				this._rank = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._name, ref variant2))
			{
				this._name = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._score, ref variant3))
			{
				this._score = variant3.As<MegaLabel>();
			}
		}

		// Token: 0x0400115E RID: 4446
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard_header");

		// Token: 0x0400115F RID: 4447
		[Nullable(1)]
		private MegaLabel _rank;

		// Token: 0x04001160 RID: 4448
		[Nullable(1)]
		private MegaLabel _name;

		// Token: 0x04001161 RID: 4449
		[Nullable(1)]
		private MegaLabel _score;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001592 RID: 5522
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040052D3 RID: 21203
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040052D4 RID: 21204
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001593 RID: 5523
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_rank' field.
			/// </summary>
			// Token: 0x040052D5 RID: 21205
			public static readonly StringName _rank = "_rank";

			/// <summary>
			/// Cached name for the '_name' field.
			/// </summary>
			// Token: 0x040052D6 RID: 21206
			public static readonly StringName _name = "_name";

			/// <summary>
			/// Cached name for the '_score' field.
			/// </summary>
			// Token: 0x040052D7 RID: 21207
			public static readonly StringName _score = "_score";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001594 RID: 5524
		public class SignalName : Control.SignalName
		{
		}
	}
}
