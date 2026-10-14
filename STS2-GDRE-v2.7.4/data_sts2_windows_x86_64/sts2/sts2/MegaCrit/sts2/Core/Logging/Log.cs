using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BB2 RID: 2994
	[NullableContext(1)]
	[Nullable(0)]
	public static class Log
	{
		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06007CD0 RID: 31952 RVA: 0x002A8AAC File Offset: 0x002A6CAC
		// (remove) Token: 0x06007CD1 RID: 31953 RVA: 0x002A8AE0 File Offset: 0x002A6CE0
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public static event Action<LogLevel, string, int> LogCallback;

		// Token: 0x17002288 RID: 8840
		// (get) Token: 0x06007CD2 RID: 31954 RVA: 0x002A8B14 File Offset: 0x002A6D14
		public static string Timestamp
		{
			get
			{
				return DateTime.UtcNow.ToString("HH:mm:ss");
			}
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x002A8B33 File Offset: 0x002A6D33
		public static void InvokeGlobalLogCallback(LogLevel logLevel, string log, int skipFrames)
		{
			Action<LogLevel, string, int> logCallback = Log.LogCallback;
			if (logCallback == null)
			{
				return;
			}
			logCallback(logLevel, log, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. It should be used when loading operations are happening.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD4 RID: 31956 RVA: 0x002A8B47 File Offset: 0x002A6D47
		public static void Load(string text, int skipFrames = 2)
		{
			Log._logger.Load(text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD5 RID: 31957 RVA: 0x002A8B55 File Offset: 0x002A6D55
		public static void Debug(string text, int skipFrames = 2)
		{
			Log._logger.Debug(text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD6 RID: 31958 RVA: 0x002A8B63 File Offset: 0x002A6D63
		public static void VeryDebug(string text, int skipFrames = 2)
		{
			Log._logger.VeryDebug(text, skipFrames);
		}

		/// <summary>
		/// Prints to stdout. It should be used for general information which is useful for debugging.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD7 RID: 31959 RVA: 0x002A8B71 File Offset: 0x002A6D71
		public static void Info(string text, int skipFrames = 2)
		{
			Log._logger.Info(text, skipFrames);
		}

		/// <summary>
		/// Prints to stderr without a stacktrace. It should be used for non-critical issues which we should be aware of
		/// or could indicate an issue.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD8 RID: 31960 RVA: 0x002A8B7F File Offset: 0x002A6D7F
		public static void Warn(string text, int skipFrames = 2)
		{
			Log._logger.Warn(text, skipFrames);
		}

		/// <summary>
		/// Prints a stacktrace to stderr. It should be used for critical issue which should also not block
		/// the continuation of the game.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="skipFrames"></param>
		// Token: 0x06007CD9 RID: 31961 RVA: 0x002A8B8D File Offset: 0x002A6D8D
		public static void Error(string text, int skipFrames = 2)
		{
			Log._logger.Error(text, skipFrames);
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x002A8B9B File Offset: 0x002A6D9B
		public static void LogMessage(LogLevel level, LogType type, string text, int skipFrames = 1)
		{
			Log._logger.LogMessage(level, type, text, skipFrames);
		}

		// Token: 0x040029A1 RID: 10657
		private static readonly Logger _logger = new Logger(null, LogType.Generic);
	}
}
