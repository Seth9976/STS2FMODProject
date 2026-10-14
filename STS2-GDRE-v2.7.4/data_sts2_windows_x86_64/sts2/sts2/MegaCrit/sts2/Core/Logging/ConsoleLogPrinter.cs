using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BAE RID: 2990
	public class ConsoleLogPrinter : ILogPrinter
	{
		// Token: 0x06007CC7 RID: 31943 RVA: 0x002A86F8 File Offset: 0x002A68F8
		[NullableContext(1)]
		public void Print(LogLevel logLevel, string text, int skipFrames)
		{
			skipFrames++;
			string text2 = logLevel.ToString().ToUpperInvariant();
			switch (logLevel)
			{
			case LogLevel.Warn:
				GD.Print("[" + text2 + "] " + text);
				return;
			case LogLevel.Error:
			{
				StackTrace stackTrace = new StackTrace(skipFrames, true);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("] ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(stackTrace);
				GD.PrintErr(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			}
			GD.Print("[" + text2 + "] " + text);
		}
	}
}
