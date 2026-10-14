using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BB3 RID: 2995
	[NullableContext(1)]
	[Nullable(0)]
	public class Logger
	{
		// Token: 0x17002289 RID: 8841
		// (get) Token: 0x06007CDC RID: 31964 RVA: 0x002A8BB9 File Offset: 0x002A6DB9
		// (set) Token: 0x06007CDD RID: 31965 RVA: 0x002A8BC0 File Offset: 0x002A6DC0
		public static LogLevel GlobalLogLevel { get; set; } = LogLevel.Info;

		// Token: 0x06007CDE RID: 31966 RVA: 0x002A8BC8 File Offset: 0x002A6DC8
		private static bool GetIsRunningFromGodotEditor()
		{
			string[] cmdlineArgs = OS.GetCmdlineArgs();
			bool flag = cmdlineArgs.Any<string>((string arg) => arg == "--headless");
			bool flag2 = cmdlineArgs.Any<string>((string arg) => arg.Contains("CiCoreRunner.tscn"));
			bool flag3 = OS.HasFeature("editor");
			return !flag && !flag2 && !TestMode.IsOn && flag3;
		}

		// Token: 0x1700228A RID: 8842
		// (get) Token: 0x06007CDF RID: 31967 RVA: 0x002A8C41 File Offset: 0x002A6E41
		// (set) Token: 0x06007CE0 RID: 31968 RVA: 0x002A8C49 File Offset: 0x002A6E49
		[Nullable(2)]
		public string Context
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06007CE1 RID: 31969 RVA: 0x002A8C54 File Offset: 0x002A6E54
		// (remove) Token: 0x06007CE2 RID: 31970 RVA: 0x002A8C8C File Offset: 0x002A6E8C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<LogLevel, string, int> LogCallback;

		// Token: 0x06007CE3 RID: 31971 RVA: 0x002A8CC4 File Offset: 0x002A6EC4
		static Logger()
		{
			ILogPrinter logPrinter2;
			if (!Logger._isRunningFromGodotEditor)
			{
				ILogPrinter logPrinter = new ConsoleLogPrinter();
				logPrinter2 = logPrinter;
			}
			else
			{
				ILogPrinter logPrinter = new EditorLogPrinter();
				logPrinter2 = logPrinter;
			}
			Logger._logPrinter = logPrinter2;
			Logger.logLevelTypeMap = new Dictionary<LogType, LogLevel>
			{
				{
					LogType.Network,
					LogLevel.Info
				},
				{
					LogType.Actions,
					LogLevel.Info
				},
				{
					LogType.Generic,
					LogLevel.Info
				},
				{
					LogType.GameSync,
					LogLevel.Info
				}
			};
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (!(commandLineArgs[i] != "-log"))
				{
					LogType? logType;
					if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogType>(commandLineArgs[i + 1], out logType))
					{
						Logger._logPrinter.Print(LogLevel.Error, "Invalid log command line argument! Could not parse " + commandLineArgs[i + 1] + " as LogType", 1);
					}
					LogLevel? logLevel;
					if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(commandLineArgs[i + 2], out logLevel))
					{
						Logger._logPrinter.Print(LogLevel.Error, "Invalid log command line argument! Could not parse " + commandLineArgs[i + 2] + " as LogLevel", 1);
					}
					Logger.logLevelTypeMap[logType.Value] = logLevel.Value;
					ILogPrinter logPrinter3 = Logger._logPrinter;
					LogLevel logLevel2 = LogLevel.Info;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Log level for ");
					defaultInterpolatedStringHandler.AppendFormatted<LogType?>(logType);
					defaultInterpolatedStringHandler.AppendLiteral(" set to ");
					defaultInterpolatedStringHandler.AppendFormatted<LogLevel?>(logLevel);
					logPrinter3.Print(logLevel2, defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x002A8E17 File Offset: 0x002A7017
		[NullableContext(2)]
		public Logger(string context, LogType logType)
		{
			this.Context = context;
			this._logType = logType;
			Action<LogLevel, string, int> action;
			if ((action = Logger.<>O.<0>__InvokeGlobalLogCallback) == null)
			{
				action = (Logger.<>O.<0>__InvokeGlobalLogCallback = new Action<LogLevel, string, int>(Log.InvokeGlobalLogCallback));
			}
			this.LogCallback += action;
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x002A8E50 File Offset: 0x002A7050
		public bool WillLog(LogLevel level)
		{
			LogLevel logLevel;
			return level >= ((Logger.logLevelTypeMap.TryGetValue(this._logType, out logLevel) ? new LogLevel?(logLevel) : null) ?? Logger.GlobalLogLevel);
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x002A8EA4 File Offset: 0x002A70A4
		public void LogMessage(LogLevel level, string text, int skipFrames)
		{
			skipFrames++;
			string text2 = ((this.Context != null) ? ("[" + this.Context + "] " + text) : text);
			this.LogMessage(level, this._logType, text2, skipFrames);
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x002A8EE8 File Offset: 0x002A70E8
		public void LogMessage(LogLevel level, LogType type, string text, int skipFrames)
		{
			skipFrames++;
			if (!this.WillLog(level))
			{
				return;
			}
			object lockObj = Logger._lockObj;
			lock (lockObj)
			{
				Logger._logPrinter.Print(level, text, skipFrames);
				Action<LogLevel, string, int> logCallback = this.LogCallback;
				if (logCallback != null)
				{
					logCallback(level, text, skipFrames);
				}
			}
		}

		/// <summary>
		/// Prints to stdout. It should be used when loading operations are happening.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CE8 RID: 31976 RVA: 0x002A8F54 File Offset: 0x002A7154
		public void Load(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.Load, text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CE9 RID: 31977 RVA: 0x002A8F5F File Offset: 0x002A715F
		public void Debug(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.Debug, text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CEA RID: 31978 RVA: 0x002A8F6A File Offset: 0x002A716A
		public void VeryDebug(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.VeryDebug, text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. It should be used for general information which is useful for debugging.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CEB RID: 31979 RVA: 0x002A8F75 File Offset: 0x002A7175
		public void Info(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.Info, text, skipFrames);
		}

		/// <summary>
		/// Prints to stderr without a stacktrace. It should be used for non-critical issues which we should be aware of
		/// or could indicate an issue.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CEC RID: 31980 RVA: 0x002A8F80 File Offset: 0x002A7180
		public void Warn(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.Warn, text, skipFrames);
		}

		/// <summary>
		/// Prints a stacktrace to stderr. It should be used for critical issue which should also not block
		/// the continuation of the game.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CED RID: 31981 RVA: 0x002A8F8B File Offset: 0x002A718B
		public void Error(string text, int skipFrames = 1)
		{
			this.LogMessage(LogLevel.Error, text, skipFrames);
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x002A8F96 File Offset: 0x002A7196
		public static void SetLogLevelForType(LogType type, LogLevel? logLevel)
		{
			if (logLevel != null)
			{
				Logger.logLevelTypeMap[type] = logLevel.Value;
				return;
			}
			Logger.logLevelTypeMap.Remove(type);
		}

		// Token: 0x040029A4 RID: 10660
		private static readonly object _lockObj = new object();

		// Token: 0x040029A5 RID: 10661
		private static readonly bool _isRunningFromGodotEditor = Logger.GetIsRunningFromGodotEditor();

		// Token: 0x040029A6 RID: 10662
		private static readonly ILogPrinter _logPrinter;

		// Token: 0x040029A7 RID: 10663
		public static readonly Dictionary<LogType, LogLevel> logLevelTypeMap;

		// Token: 0x040029A8 RID: 10664
		private readonly LogType _logType;

		// Token: 0x020022EA RID: 8938
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040092B5 RID: 37557
			[Nullable(new byte[] { 0, 1 })]
			public static Action<LogLevel, string, int> <0>__InvokeGlobalLogCallback;
		}
	}
}
