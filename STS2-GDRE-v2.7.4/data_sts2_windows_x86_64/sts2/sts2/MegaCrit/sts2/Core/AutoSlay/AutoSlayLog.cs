using System;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay
{
	/// <summary>
	/// Structured logging for AutoSlay with consistent prefixes for easy filtering.
	/// Writes to both the standard Godot log and a dedicated autoslay.log file.
	/// </summary>
	// Token: 0x02000D6B RID: 3435
	[NullableContext(1)]
	[Nullable(0)]
	public static class AutoSlayLog
	{
		/// <summary>Opens the log file for writing. Call at start of autoslay run.</summary>
		// Token: 0x06008ADF RID: 35551 RVA: 0x002E5C90 File Offset: 0x002E3E90
		public static void OpenLogFile(string path)
		{
			object @lock = AutoSlayLog._lock;
			lock (@lock)
			{
				StreamWriter fileWriter = AutoSlayLog._fileWriter;
				if (fileWriter != null)
				{
					fileWriter.Dispose();
				}
				AutoSlayLog._fileWriter = new StreamWriter(path, false)
				{
					AutoFlush = true
				};
			}
		}

		/// <summary>Closes the log file. Call at end of autoslay run.</summary>
		// Token: 0x06008AE0 RID: 35552 RVA: 0x002E5CEC File Offset: 0x002E3EEC
		public static void CloseLogFile()
		{
			object @lock = AutoSlayLog._lock;
			lock (@lock)
			{
				StreamWriter fileWriter = AutoSlayLog._fileWriter;
				if (fileWriter != null)
				{
					fileWriter.Dispose();
				}
				AutoSlayLog._fileWriter = null;
			}
		}

		// Token: 0x06008AE1 RID: 35553 RVA: 0x002E5D3C File Offset: 0x002E3F3C
		public static void Info(string message)
		{
			string text = "[AutoSlay] " + message;
			Log.Info(text, 2);
			AutoSlayLog.WriteToFile("INFO", text);
		}

		// Token: 0x06008AE2 RID: 35554 RVA: 0x002E5D68 File Offset: 0x002E3F68
		public static void Warn(string message)
		{
			string text = "[AutoSlay] " + message;
			Log.Warn(text, 2);
			AutoSlayLog.WriteToFile("WARN", text);
		}

		// Token: 0x06008AE3 RID: 35555 RVA: 0x002E5D94 File Offset: 0x002E3F94
		public static void Error(string message)
		{
			string text = "[AutoSlay] " + message;
			Log.Error(text, 2);
			AutoSlayLog.WriteToFile("ERROR", text);
		}

		// Token: 0x06008AE4 RID: 35556 RVA: 0x002E5DC0 File Offset: 0x002E3FC0
		public static void Error(string message, Exception ex)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 4);
			defaultInterpolatedStringHandler.AppendFormatted("[AutoSlay]");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(message);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendFormatted(ex.StackTrace);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			Log.Error(text, 2);
			AutoSlayLog.WriteToFile("ERROR", text);
		}

		// Token: 0x06008AE5 RID: 35557 RVA: 0x002E5E44 File Offset: 0x002E4044
		private static void WriteToFile(string level, string message)
		{
			object @lock = AutoSlayLog._lock;
			lock (@lock)
			{
				StreamWriter fileWriter = AutoSlayLog._fileWriter;
				if (fileWriter != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
					defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "HH:mm:ss.fff");
					defaultInterpolatedStringHandler.AppendLiteral(" [");
					defaultInterpolatedStringHandler.AppendFormatted(level);
					defaultInterpolatedStringHandler.AppendLiteral("] ");
					defaultInterpolatedStringHandler.AppendFormatted(message);
					fileWriter.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06008AE6 RID: 35558 RVA: 0x002E5ED8 File Offset: 0x002E40D8
		public static void RunStarted(string seed)
		{
			string text = Path.Combine(OS.GetUserDataDir(), "logs", "godot.log");
			AutoSlayLog.Info("Starting run with seed=" + seed);
			AutoSlayLog.Info("Godot log: " + text);
		}

		// Token: 0x06008AE7 RID: 35559 RVA: 0x002E5F1A File Offset: 0x002E411A
		public static void RunCompleted(string seed)
		{
			AutoSlayLog.Info("Run completed successfully with seed=" + seed);
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x002E5F2C File Offset: 0x002E412C
		public static void RunFailed(string seed, Exception ex)
		{
			AutoSlayLog.Error("Run failed with seed=" + seed, ex);
		}

		// Token: 0x06008AE9 RID: 35561 RVA: 0x002E5F40 File Offset: 0x002E4140
		public static void EnterRoom(RoomType type, int act, int floor)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Entering ");
			defaultInterpolatedStringHandler.AppendFormatted<RoomType>(type);
			defaultInterpolatedStringHandler.AppendLiteral(" room (Act ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(act + 1);
			defaultInterpolatedStringHandler.AppendLiteral(", Floor ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(floor);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06008AEA RID: 35562 RVA: 0x002E5FB0 File Offset: 0x002E41B0
		public static void ExitRoom(RoomType type)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Finished ");
			defaultInterpolatedStringHandler.AppendFormatted<RoomType>(type);
			defaultInterpolatedStringHandler.AppendLiteral(" room");
			AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06008AEB RID: 35563 RVA: 0x002E5FF3 File Offset: 0x002E41F3
		public static void EnterScreen(string screenName)
		{
			AutoSlayLog.Info("Handling screen: " + screenName);
		}

		// Token: 0x06008AEC RID: 35564 RVA: 0x002E6005 File Offset: 0x002E4205
		public static void ExitScreen(string screenName)
		{
			AutoSlayLog.Info("Finished screen: " + screenName);
		}

		// Token: 0x06008AED RID: 35565 RVA: 0x002E6017 File Offset: 0x002E4217
		public static void Action(string action)
		{
			AutoSlayLog.Info("Action: " + action);
		}

		// Token: 0x06008AEE RID: 35566 RVA: 0x002E602C File Offset: 0x002E422C
		[NullableContext(2)]
		public static void StateSnapshot(RunState runState)
		{
			if (runState == null)
			{
				AutoSlayLog.Info("State: RunState is null");
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
			defaultInterpolatedStringHandler.AppendLiteral("State: Floor=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(runState.TotalFloor);
			defaultInterpolatedStringHandler.AppendLiteral(", Room=");
			AbstractRoom currentRoom = runState.CurrentRoom;
			defaultInterpolatedStringHandler.AppendFormatted<RoomType?>((currentRoom != null) ? new RoomType?(currentRoom.RoomType) : null);
			defaultInterpolatedStringHandler.AppendLiteral(", Act=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(runState.CurrentActIndex + 1);
			AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04003037 RID: 12343
		private const string _prefix = "[AutoSlay]";

		// Token: 0x04003038 RID: 12344
		[Nullable(2)]
		private static StreamWriter _fileWriter;

		// Token: 0x04003039 RID: 12345
		private static readonly object _lock = new object();
	}
}
