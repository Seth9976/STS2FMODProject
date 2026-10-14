using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x0200003C RID: 60
	[ScriptPath("res://src/Core/Timeline/TimelineInfoDumper.cs")]
	public class TimelineInfoDumper : Node
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00012960 File Offset: 0x00010B60
		public static void Dump()
		{
			List<EpochModel> allEpochs = TimelineInfoDumper.GetAllEpochs();
			Console.Out.WriteLine("START TIMELINE INFO DUMPER");
			Console.Out.WriteLine("START TIMELINE INFO DUMPER");
			Console.Out.WriteLine("START TIMELINE INFO DUMPER");
			foreach (EpochModel epochModel in allEpochs)
			{
				TextWriter @out = Console.Out;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 8);
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted(epochModel.Id);
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted<EpochEra>(epochModel.Era);
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)epochModel.Era);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<int>(epochModel.EraPosition);
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted<LocString>(epochModel.Title);
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted(epochModel.Description.Replace("\r", "").Replace("\n", ""));
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted(epochModel.UnlockText);
				defaultInterpolatedStringHandler.AppendLiteral("\", ");
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				defaultInterpolatedStringHandler.AppendFormatted(epochModel.ResolvedPortraitPath);
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				@out.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Console.Out.WriteLine("END TIMELINE INFO DUMPER");
			Console.Out.WriteLine("END TIMELINE INFO DUMPER");
			Console.Out.WriteLine("END TIMELINE INFO DUMPER");
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00012B70 File Offset: 0x00010D70
		[NullableContext(1)]
		public static List<EpochModel> GetAllEpochs()
		{
			IEnumerable<string> allEpochIds = EpochModel.AllEpochIds;
			Func<string, EpochModel> func;
			if ((func = TimelineInfoDumper.<>O.<0>__Get) == null)
			{
				func = (TimelineInfoDumper.<>O.<0>__Get = new Func<string, EpochModel>(EpochModel.Get));
			}
			return allEpochIds.Select<string, EpochModel>(func).ToList<EpochModel>();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000272 RID: 626 RVA: 0x00012B9C File Offset: 0x00010D9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(TimelineInfoDumper.MethodName.Dump, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000273 RID: 627 RVA: 0x00012BE1 File Offset: 0x00010DE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == TimelineInfoDumper.MethodName.Dump && args.Count == 0)
			{
				TimelineInfoDumper.Dump();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00012C10 File Offset: 0x00010E10
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == TimelineInfoDumper.MethodName.Dump && args.Count == 0)
			{
				TimelineInfoDumper.Dump();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06000275 RID: 629 RVA: 0x00012C3E File Offset: 0x00010E3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == TimelineInfoDumper.MethodName.Dump || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06000276 RID: 630 RVA: 0x00012C56 File Offset: 0x00010E56
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06000277 RID: 631 RVA: 0x00012C5F File Offset: 0x00010E5F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E09 RID: 3593
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the 'Dump' method.
			/// </summary>
			// Token: 0x040032F7 RID: 13047
			public static readonly StringName Dump = "Dump";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E0A RID: 3594
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E0B RID: 3595
		public class SignalName : Node.SignalName
		{
		}

		// Token: 0x02000E0C RID: 3596
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040032F8 RID: 13048
			public static Func<string, EpochModel> <0>__Get;
		}
	}
}
