using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// Debug console command for testing Sentry integration.
	/// </summary>
	// Token: 0x02000CFF RID: 3327
	[NullableContext(1)]
	[Nullable(0)]
	public class SentryConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002507 RID: 9479
		// (get) Token: 0x0600866A RID: 34410 RVA: 0x002D1B86 File Offset: 0x002CFD86
		public override string CmdName
		{
			get
			{
				return "sentry";
			}
		}

		// Token: 0x17002508 RID: 9480
		// (get) Token: 0x0600866B RID: 34411 RVA: 0x002D1B8D File Offset: 0x002CFD8D
		public override string Args
		{
			get
			{
				return "<test|message|exception|crash|status> [text]";
			}
		}

		// Token: 0x17002509 RID: 9481
		// (get) Token: 0x0600866C RID: 34412 RVA: 0x002D1B94 File Offset: 0x002CFD94
		public override string Description
		{
			get
			{
				return "Test Sentry error reporting. 'test' sends a test message and exception, 'message <text>' sends a custom message, 'exception' throws a test exception, 'crash confirm' triggers a native crash (terminates game!), 'status' shows Sentry status.";
			}
		}

		// Token: 0x1700250A RID: 9482
		// (get) Token: 0x0600866D RID: 34413 RVA: 0x002D1B9B File Offset: 0x002CFD9B
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600866E RID: 34414 RVA: 0x002D1BA0 File Offset: 0x002CFDA0
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "Usage: sentry <test|message|exception|crash|status> [text]");
			}
			string text = args[0].ToLowerInvariant();
			CmdResult cmdResult;
			if (!(text == "status"))
			{
				if (!(text == "test"))
				{
					if (!(text == "message"))
					{
						if (!(text == "exception"))
						{
							if (!(text == "crash"))
							{
								cmdResult = new CmdResult(false, "Unknown subcommand: " + text + ". Use test, message, exception, crash, or status.");
							}
							else
							{
								cmdResult = SentryConsoleCmd.TriggerNativeCrash(args);
							}
						}
						else
						{
							cmdResult = SentryConsoleCmd.ThrowException();
						}
					}
					else
					{
						cmdResult = SentryConsoleCmd.SendMessage(args);
					}
				}
				else
				{
					cmdResult = SentryConsoleCmd.RunTest();
				}
			}
			else
			{
				cmdResult = SentryConsoleCmd.GetStatus();
			}
			return cmdResult;
		}

		// Token: 0x0600866F RID: 34415 RVA: 0x002D1C4C File Offset: 0x002CFE4C
		private static CmdResult GetStatus()
		{
			bool isEnabled = SentryService.IsEnabled;
			bool flag = ReleaseInfoManager.Instance.ReleaseInfo != null;
			bool uploadData = SaveManager.Instance.PrefsSave.UploadData;
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			string text = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "N/A";
			ReleaseInfo releaseInfo2 = ReleaseInfoManager.Instance.ReleaseInfo;
			string text2 = ((releaseInfo2 != null) ? releaseInfo2.Branch : null) ?? "N/A";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Sentry Status:\n  .NET SDK Enabled: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(isEnabled);
			defaultInterpolatedStringHandler.AppendLiteral("\n  Has release_info.json: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(flag);
			defaultInterpolatedStringHandler.AppendLiteral("\n  User consent (UploadData): ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(uploadData);
			defaultInterpolatedStringHandler.AppendLiteral("\n  Version: ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("\n  Branch: ");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			string text3 = defaultInterpolatedStringHandler.ToStringAndClear();
			return new CmdResult(true, text3);
		}

		// Token: 0x06008670 RID: 34416 RVA: 0x002D1D40 File Offset: 0x002CFF40
		private static CmdResult RunTest()
		{
			bool isEnabled = SentryService.IsEnabled;
			if (!isEnabled)
			{
				SentryConsoleCmd.ForceInitializeForTesting();
			}
			if (!SentryService.IsEnabled)
			{
				return new CmdResult(false, "Failed to initialize Sentry for testing. Check logs for details.");
			}
			SentryService.CaptureMessage("Test message from STS2 dev console", 1, null);
			try
			{
				throw new InvalidOperationException("Test exception from STS2 dev console");
			}
			catch (Exception ex)
			{
				SentryService.CaptureException(ex);
			}
			string text = "Sent test message and exception to Sentry. Check your Sentry dashboard.";
			if (!isEnabled)
			{
				text += "\n(Sentry was temporarily enabled for this test)";
			}
			return new CmdResult(true, text);
		}

		// Token: 0x06008671 RID: 34417 RVA: 0x002D1DBC File Offset: 0x002CFFBC
		private static CmdResult SendMessage(string[] args)
		{
			if (args.Length < 2)
			{
				return new CmdResult(false, "Usage: sentry message <text>");
			}
			string text = string.Join(" ", args, 1, args.Length - 1);
			if (!SentryService.IsEnabled)
			{
				SentryConsoleCmd.ForceInitializeForTesting();
			}
			if (!SentryService.IsEnabled)
			{
				return new CmdResult(false, "Failed to initialize Sentry for testing.");
			}
			SentryService.CaptureMessage("[DevConsole] " + text, 1, null);
			return new CmdResult(true, "Sent message to Sentry: " + text);
		}

		// Token: 0x06008672 RID: 34418 RVA: 0x002D1E30 File Offset: 0x002D0030
		private static CmdResult ThrowException()
		{
			if (!SentryService.IsEnabled)
			{
				SentryConsoleCmd.ForceInitializeForTesting();
			}
			if (!SentryService.IsEnabled)
			{
				return new CmdResult(false, "Failed to initialize Sentry for testing.");
			}
			CmdResult cmdResult;
			try
			{
				throw new InvalidOperationException("Test exception triggered via 'sentry exception' command");
			}
			catch (Exception ex)
			{
				SentryService.CaptureException(ex);
				cmdResult = new CmdResult(true, "Captured test exception and sent to Sentry.");
			}
			return cmdResult;
		}

		// Token: 0x06008673 RID: 34419 RVA: 0x002D1E90 File Offset: 0x002D0090
		private static CmdResult TriggerNativeCrash(string[] args)
		{
			if (OS.HasFeature("editor"))
			{
				return new CmdResult(false, "Native crash testing only works in builds, not the editor.");
			}
			if (args.Length < 2 || !args[1].Equals("confirm", StringComparison.OrdinalIgnoreCase))
			{
				return new CmdResult(false, "WARNING: This will crash the game!\nTo confirm, run: sentry crash confirm");
			}
			OS.Crash("Sentry test crash triggered via dev console");
			return new CmdResult(true, "Crash triggered");
		}

		/// <summary>
		/// Force initializes Sentry for testing purposes, bypassing normal checks.
		/// </summary>
		// Token: 0x06008674 RID: 34420 RVA: 0x002D1EEC File Offset: 0x002D00EC
		private static void ForceInitializeForTesting()
		{
			SentryService.InitializeForTesting();
		}
	}
}
