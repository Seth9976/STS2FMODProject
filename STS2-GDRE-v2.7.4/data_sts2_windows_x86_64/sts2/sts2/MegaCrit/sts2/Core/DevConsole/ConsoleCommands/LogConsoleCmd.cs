using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF8 RID: 3320
	[NullableContext(1)]
	[Nullable(0)]
	public class LogConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024E9 RID: 9449
		// (get) Token: 0x06008636 RID: 34358 RVA: 0x002D0DDE File Offset: 0x002CEFDE
		public override string CmdName
		{
			get
			{
				return "log";
			}
		}

		// Token: 0x170024EA RID: 9450
		// (get) Token: 0x06008637 RID: 34359 RVA: 0x002D0DE5 File Offset: 0x002CEFE5
		public override string Args
		{
			get
			{
				return "[type:string] <level:string>";
			}
		}

		// Token: 0x170024EB RID: 9451
		// (get) Token: 0x06008638 RID: 34360 RVA: 0x002D0DEC File Offset: 0x002CEFEC
		public override string Description
		{
			get
			{
				return "Set log level for specific log types. Type can be: " + string.Join(",", Enum.GetNames<LogType>()) + ". Levels can be: " + string.Join(",", Enum.GetNames<LogLevel>());
			}
		}

		// Token: 0x170024EC RID: 9452
		// (get) Token: 0x06008639 RID: 34361 RVA: 0x002D0E1B File Offset: 0x002CF01B
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024ED RID: 9453
		// (get) Token: 0x0600863A RID: 34362 RVA: 0x002D0E1E File Offset: 0x002CF01E
		public override bool DebugOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x002D0E24 File Offset: 0x002CF024
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "At least one arg must be supplied!");
			}
			LogLevel? logLevel;
			LogType? logType;
			if (LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(args[0], out logLevel))
			{
				logType = new LogType?(LogType.Generic);
			}
			else
			{
				if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogType>(args[0], out logType))
				{
					return new CmdResult(false, "First argument '" + args[0] + "' could not be parsed as either a log level or type!");
				}
				if (args.Length <= 1)
				{
					return new CmdResult(false, "Must supply a log level as the second argument!");
				}
				if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(args[1], out logLevel))
				{
					return new CmdResult(false, "Second argument '" + args[1] + "' could not be parsed as a log level!");
				}
			}
			Logger.logLevelTypeMap[logType.Value] = logLevel.Value;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Logging level for ");
			defaultInterpolatedStringHandler.AppendFormatted<LogType?>(logType);
			defaultInterpolatedStringHandler.AppendLiteral(" set to ");
			defaultInterpolatedStringHandler.AppendFormatted<LogLevel?>(logLevel);
			return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600863C RID: 34364 RVA: 0x002D0F0C File Offset: 0x002CF10C
		[NullableContext(0)]
		public static bool TryParseEnumCaseInsensitive<T>([Nullable(1)] string str, out T? enumVal) where T : struct, Enum
		{
			foreach (T t in Enum.GetValues<T>())
			{
				if (str.Equals(t.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					enumVal = new T?(t);
					return true;
				}
			}
			enumVal = null;
			return false;
		}
	}
}
