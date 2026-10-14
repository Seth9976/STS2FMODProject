using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Logging
{
	// Token: 0x02000BAF RID: 2991
	public class EditorLogPrinter : ILogPrinter
	{
		// Token: 0x06007CC9 RID: 31945 RVA: 0x002A87CC File Offset: 0x002A69CC
		[NullableContext(1)]
		private static string GetColorCode(LogLevel level)
		{
			string text;
			switch (level)
			{
			case LogLevel.VeryDebug:
				text = "#B180D3";
				break;
			case LogLevel.Load:
				text = "#87CEEB";
				break;
			case LogLevel.Debug:
				text = "#B180D3";
				break;
			case LogLevel.Info:
				text = "#76FF56";
				break;
			case LogLevel.Warn:
				text = "#FFCB3D";
				break;
			case LogLevel.Error:
				text = "#FF4747";
				break;
			default:
				throw new ArgumentOutOfRangeException("level", level, null);
			}
			return text;
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x002A883C File Offset: 0x002A6A3C
		private static bool LogFullStack(LogLevel level)
		{
			bool flag;
			switch (level)
			{
			case LogLevel.VeryDebug:
				flag = false;
				break;
			case LogLevel.Load:
				flag = false;
				break;
			case LogLevel.Debug:
				flag = false;
				break;
			case LogLevel.Info:
				flag = false;
				break;
			case LogLevel.Warn:
				flag = false;
				break;
			case LogLevel.Error:
				flag = true;
				break;
			default:
				throw new ArgumentOutOfRangeException("level", level, null);
			}
			return flag;
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x002A8894 File Offset: 0x002A6A94
		[NullableContext(1)]
		public void Print(LogLevel level, string text, int skipFrames)
		{
			skipFrames++;
			string text2 = level.ToString().ToUpperInvariant();
			string colorCode = EditorLogPrinter.GetColorCode(level);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
			defaultInterpolatedStringHandler.AppendLiteral("[b][color=");
			defaultInterpolatedStringHandler.AppendFormatted(colorCode);
			defaultInterpolatedStringHandler.AppendLiteral("][");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			defaultInterpolatedStringHandler.AppendLiteral("][/color][/b] ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			string text3 = defaultInterpolatedStringHandler.ToStringAndClear();
			string text4 = "";
			if (level != LogLevel.Warn)
			{
				if (EditorLogPrinter.LogFullStack(level))
				{
					StackTrace stackTrace = new StackTrace(skipFrames, true);
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler.AppendLiteral("\n");
					defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(stackTrace);
					text4 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					StackFrame stackFrame = new StackFrame(skipFrames, true);
					DiagnosticMethodInfo diagnosticMethodInfo = DiagnosticMethodInfo.Create(stackFrame);
					int fileLineNumber = stackFrame.GetFileLineNumber();
					string fileName = stackFrame.GetFileName();
					string fileName2 = Path.GetFileName(fileName);
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
					defaultInterpolatedStringHandler.AppendLiteral(" line ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(fileLineNumber);
					defaultInterpolatedStringHandler.AppendLiteral(":");
					defaultInterpolatedStringHandler.AppendFormatted((diagnosticMethodInfo != null) ? diagnosticMethodInfo.Name : null);
					defaultInterpolatedStringHandler.AppendLiteral("() in ");
					defaultInterpolatedStringHandler.AppendFormatted(fileName2);
					text4 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			if (level == LogLevel.Error)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("] ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendFormatted(text4);
				string text5 = defaultInterpolatedStringHandler.ToStringAndClear();
				GD.PrintErr(text5);
				GD.PushError(text5);
			}
			if (level == LogLevel.Warn)
			{
				GD.PrintRich(text3 ?? "");
				return;
			}
			GD.PrintRich(text3 + "[color=#606060]" + text4 + "[/color]");
		}
	}
}
