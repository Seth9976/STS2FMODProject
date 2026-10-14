using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CFA RID: 3322
	[NullableContext(1)]
	[Nullable(0)]
	public class OpenConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024F2 RID: 9458
		// (get) Token: 0x06008644 RID: 34372 RVA: 0x002D1016 File Offset: 0x002CF216
		public override string CmdName
		{
			get
			{
				return "open";
			}
		}

		// Token: 0x170024F3 RID: 9459
		// (get) Token: 0x06008645 RID: 34373 RVA: 0x002D101D File Offset: 0x002CF21D
		public override string Args
		{
			get
			{
				return "logs|saves|root|build-logs|loc-override";
			}
		}

		// Token: 0x170024F4 RID: 9460
		// (get) Token: 0x06008646 RID: 34374 RVA: 0x002D1024 File Offset: 0x002CF224
		public override string Description
		{
			get
			{
				return "Opens a common path in the local OS file browser.";
			}
		}

		// Token: 0x170024F5 RID: 9461
		// (get) Token: 0x06008647 RID: 34375 RVA: 0x002D102B File Offset: 0x002CF22B
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024F6 RID: 9462
		// (get) Token: 0x06008648 RID: 34376 RVA: 0x002D102E File Offset: 0x002CF22E
		public override bool DebugOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008649 RID: 34377 RVA: 0x002D1034 File Offset: 0x002CF234
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, "No argument specified.\n" + this.Args);
			}
			if (!OpenConsoleCmd._options.Contains(args[0]))
			{
				return new CmdResult(false, "Argument '" + args[0] + "' unrecognized.\n" + this.Args);
			}
			string userDataDir = OS.GetUserDataDir();
			if (userDataDir == null)
			{
				return new CmdResult(false, "Unable to open the user data directory.");
			}
			string dataDir = OS.GetDataDir();
			string text = args[0];
			string text2;
			if (!(text == "logs"))
			{
				if (!(text == "saves"))
				{
					if (!(text == "root"))
					{
						if (!(text == "build-logs"))
						{
							if (!(text == "loc-override"))
							{
								throw new ArgumentOutOfRangeException();
							}
							text2 = ProjectSettings.GlobalizePath("user://localization_override");
						}
						else
						{
							text2 = Path.Combine(dataDir, "Godot", "mono", "build_logs");
						}
					}
					else
					{
						text2 = userDataDir;
					}
				}
				else
				{
					text2 = ProjectSettings.GlobalizePath(SaveManager.Instance.GetProfileScopedPath("saves"));
				}
			}
			else
			{
				text2 = Path.Combine(userDataDir, "logs");
			}
			string text3 = text2;
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				text3 = text3.Replace('/', '\\');
			}
			Error error = OS.ShellShowInFileManager(text3, true);
			if (error != null)
			{
				bool flag = false;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
				defaultInterpolatedStringHandler.AppendLiteral(": Cannot open OS file manager.");
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CmdResult(true, "Opened '" + text3 + "'");
		}

		// Token: 0x0600864A RID: 34378 RVA: 0x002D11C0 File Offset: 0x002CF3C0
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				return base.CompleteArgument(OpenConsoleCmd._options, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x04002E6C RID: 11884
		private static readonly IReadOnlyCollection<string> _options = new <>z__ReadOnlyArray<string>(new string[] { "logs", "saves", "root", "build-logs", "loc-override" });
	}
}
