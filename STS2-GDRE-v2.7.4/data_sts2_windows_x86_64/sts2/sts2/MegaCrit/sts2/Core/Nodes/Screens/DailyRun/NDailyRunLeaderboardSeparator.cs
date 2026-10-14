using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x0200032E RID: 814
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboardSeparator.cs")]
	public class NDailyRunLeaderboardSeparator : Control
	{
		// Token: 0x06002D78 RID: 11640 RVA: 0x001461D0 File Offset: 0x001443D0
		[NullableContext(2)]
		public static NDailyRunLeaderboardSeparator Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDailyRunLeaderboardSeparator._scenePath).Instantiate<NDailyRunLeaderboardSeparator>(0L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D79 RID: 11641 RVA: 0x00146200 File Offset: 0x00144400
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NDailyRunLeaderboardSeparator.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D7A RID: 11642 RVA: 0x00146250 File Offset: 0x00144450
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardSeparator.MethodName.Create && args.Count == 0)
			{
				NDailyRunLeaderboardSeparator ndailyRunLeaderboardSeparator = NDailyRunLeaderboardSeparator.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLeaderboardSeparator>(ref ndailyRunLeaderboardSeparator);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x00146294 File Offset: 0x00144494
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboardSeparator.MethodName.Create && args.Count == 0)
			{
				NDailyRunLeaderboardSeparator ndailyRunLeaderboardSeparator = NDailyRunLeaderboardSeparator.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLeaderboardSeparator>(ref ndailyRunLeaderboardSeparator);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002D7C RID: 11644 RVA: 0x001462D4 File Offset: 0x001444D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunLeaderboardSeparator.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D7D RID: 11645 RVA: 0x001462EC File Offset: 0x001444EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002D7E RID: 11646 RVA: 0x001462F5 File Offset: 0x001444F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x0400116A RID: 4458
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard_separator");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001599 RID: 5529
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040052E2 RID: 21218
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200159A RID: 5530
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200159B RID: 5531
		public class SignalName : Control.SignalName
		{
		}
	}
}
