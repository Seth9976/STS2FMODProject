using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CDB RID: 3291
	[NullableContext(1)]
	[Nullable(0)]
	public class DevConsole
	{
		// Token: 0x06008552 RID: 34130 RVA: 0x002CC2E4 File Offset: 0x002CA4E4
		public DevConsole(bool shouldAllowDebugCommands)
		{
			this._historyFilePath = UserDataPathProvider.GetAccountScopedBasePath("console_history.log", null, null);
			this.history = new FixedSizedQueue<string>(40);
			this.LoadCommandHistory();
			foreach (Type type in AbstractConsoleCmdSubtypes.All.Concat<Type>(ReflectionHelper.GetSubtypesInMods<AbstractConsoleCmd>()))
			{
				AbstractConsoleCmd abstractConsoleCmd = (AbstractConsoleCmd)Activator.CreateInstance(type);
				if (!abstractConsoleCmd.DebugOnly || shouldAllowDebugCommands)
				{
					this.RegisterCommand(abstractConsoleCmd);
				}
			}
		}

		// Token: 0x06008553 RID: 34131 RVA: 0x002CC39C File Offset: 0x002CA59C
		private void RegisterCommand(AbstractConsoleCmd consoleCmd)
		{
			this._commands[consoleCmd.CmdName] = consoleCmd;
		}

		// Token: 0x06008554 RID: 34132 RVA: 0x002CC3B0 File Offset: 0x002CA5B0
		private void LoadCommandHistory()
		{
			using (FileAccess fileAccess = FileAccess.Open(this._historyFilePath, 1L))
			{
				if (fileAccess != null)
				{
					while (fileAccess.GetPosition() < fileAccess.GetLength())
					{
						this.history.Add(fileAccess.GetLine());
					}
				}
			}
		}

		// Token: 0x06008555 RID: 34133 RVA: 0x002CC40C File Offset: 0x002CA60C
		private void SaveCommandHistory()
		{
			using (FileAccess fileAccess = FileAccess.Open(this._historyFilePath, 2L))
			{
				if (fileAccess != null)
				{
					foreach (string text in this.history)
					{
						fileAccess.StoreLine(text);
					}
				}
			}
		}

		/// <summary>
		/// Gets structured completion results for interactive selection
		/// </summary>
		// Token: 0x06008556 RID: 34134 RVA: 0x002CC48C File Offset: 0x002CA68C
		public unsafe CompletionResult GetCompletionResults(string inputBuffer)
		{
			string[] array = (inputBuffer.EndsWith(' ') ? inputBuffer.Trim().Split(' ', StringSplitOptions.None).Append(string.Empty)
				.ToArray<string>() : inputBuffer.Trim().Split(' ', StringSplitOptions.None));
			string text = array.First<string>();
			if (this._commands.ContainsKey(text) && array.Length == 1)
			{
				CompletionResult completionResult = new CompletionResult();
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = text;
				completionResult.Candidates = list;
				completionResult.CommonPrefix = text + " ";
				completionResult.Type = CompletionType.Command;
				completionResult.CommandPrefix = "";
				completionResult.ArgumentIndex = -1;
				completionResult.ArgumentContext = "";
				return completionResult;
			}
			AbstractConsoleCmd abstractConsoleCmd;
			if (this._commands.TryGetValue(text.Trim().ToLowerInvariant(), out abstractConsoleCmd))
			{
				string[] array2 = array.Skip<string>(1).ToArray<string>();
				string text2 = text + " ";
				if (array2.Length != 0)
				{
					string[] array3 = array2.Take<string>(array2.Length - 1).ToArray<string>();
					if (array3.Length != 0)
					{
						text2 = text2 + string.Join(" ", array3) + " ";
					}
				}
				CompletionType completionType = ((array2.Length <= 1) ? CompletionType.Subcommand : CompletionType.Argument);
				int num3 = array2.Length - 1;
				CompletionResult argumentCompletions = abstractConsoleCmd.GetArgumentCompletions(LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState()), array2);
				if (argumentCompletions.Candidates.Count > 0)
				{
					argumentCompletions.Type = completionType;
					argumentCompletions.CommandPrefix = text2;
					argumentCompletions.ArgumentIndex = num3;
					argumentCompletions.ArgumentContext = text;
					if (string.IsNullOrEmpty(argumentCompletions.CommonPrefix))
					{
						argumentCompletions.CommonPrefix = inputBuffer;
					}
				}
				else
				{
					argumentCompletions.Type = completionType;
					argumentCompletions.CommandPrefix = text2;
					argumentCompletions.ArgumentIndex = num3;
					argumentCompletions.ArgumentContext = text;
				}
				return argumentCompletions;
			}
			if (array.Length > 1)
			{
				return new CompletionResult
				{
					Type = CompletionType.Command,
					CommandPrefix = "",
					ArgumentIndex = -1,
					ArgumentContext = ""
				};
			}
			List<string> list2 = this._commands.Values.Select<AbstractConsoleCmd, string>((AbstractConsoleCmd s) => s.CmdName).ToList<string>();
			return DevConsole.GetCompletionResultsFromTokens(text, list2, inputBuffer);
		}

		/// <summary>
		/// Calculates the inline ghost-text suffix to display for the given input and completion result,
		/// or null when no ghost text should be shown.
		///
		/// Ghost text is a gray inline overlay rendered after the user's typed text, so it only makes
		/// sense when the completion extends the input (prefix match). Fuzzy completions legitimately
		/// replace the typed token instead of extending it (e.g. "relic sarig" -&gt; "relic KUSARIGAMA "),
		/// so no ghost text is shown for them. This is expected, not a bug.
		///
		/// IMPORTANT: The space padding requires the input buffer and ghost-text label to share the SAME
		/// MONOSPACE FONT (currently Source Code Pro). Since both start at x=0, we pad with spaces so the
		/// ghost text sits after the user's input instead of overlaying it. If the font is changed to a
		/// variable-width one, this alignment breaks.
		/// </summary>
		// Token: 0x06008557 RID: 34135 RVA: 0x002CC6D0 File Offset: 0x002CA8D0
		[return: Nullable(2)]
		public static string CalculateGhostText(string inputText, CompletionResult result)
		{
			if (result.Candidates.Count != 1 || string.IsNullOrEmpty(result.CommonPrefix))
			{
				return null;
			}
			string commonPrefix = result.CommonPrefix;
			if (!commonPrefix.StartsWith(inputText, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string text = commonPrefix.Substring(inputText.Length);
			string text2 = new string(' ', inputText.Length);
			return text2 + text;
		}

		// Token: 0x06008558 RID: 34136 RVA: 0x002CC730 File Offset: 0x002CA930
		private static CompletionResult GetCompletionResultsFromTokens(string partialToken, List<string> possibleTokens, string originalInput)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (string text in possibleTokens)
			{
				if (text.StartsWith(partialToken, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(text);
				}
				else if (text.Contains(partialToken, StringComparison.OrdinalIgnoreCase))
				{
					list2.Add(text);
				}
			}
			List<string> list3 = list;
			List<string> list4 = list2;
			int num = list3.Count + list4.Count;
			List<string> list5 = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list5, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list5);
			int num2 = 0;
			Span<string> span2 = CollectionsMarshal.AsSpan<string>(list3);
			span2.CopyTo(span.Slice(num2, span2.Length));
			num2 += span2.Length;
			Span<string> span3 = CollectionsMarshal.AsSpan<string>(list4);
			span3.CopyTo(span.Slice(num2, span3.Length));
			num2 += span3.Length;
			List<string> list6 = list5;
			if (list6.Count == 0)
			{
				return new CompletionResult
				{
					Type = CompletionType.Command,
					CommandPrefix = "",
					ArgumentIndex = -1,
					ArgumentContext = ""
				};
			}
			if (list6.Count == 1)
			{
				string text2 = ((list.Count == 1) ? (DevConsole.ReplaceLastToken(originalInput, list6.First<string>()) + " ") : originalInput);
				return new CompletionResult
				{
					Candidates = list6,
					CommonPrefix = text2,
					Type = CompletionType.Command,
					CommandPrefix = "",
					ArgumentIndex = -1,
					ArgumentContext = ""
				};
			}
			return new CompletionResult
			{
				Candidates = list6,
				CommonPrefix = DevConsole.ReplaceLastToken(originalInput, DevConsole.LongestCommonSubstring(list6)),
				Type = CompletionType.Command,
				CommandPrefix = "",
				ArgumentIndex = -1,
				ArgumentContext = ""
			};
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x002CC914 File Offset: 0x002CAB14
		private static string ReplaceLastToken(string text, string replacement)
		{
			if (string.IsNullOrWhiteSpace(replacement))
			{
				return text;
			}
			string[] array = text.Trim().Split(' ', StringSplitOptions.None);
			string[] array2 = ((array.Length != 0) ? array.Take<string>(array.Length - 1).ToArray<string>() : Array.Empty<string>());
			string text2 = ((array2.Length != 0) ? (string.Join(" ", array2) + " ") : string.Empty);
			return text2 + replacement;
		}

		// Token: 0x0600855A RID: 34138 RVA: 0x002CC980 File Offset: 0x002CAB80
		private static string LongestCommonSubstring(List<string> cmdCandidates)
		{
			int num = cmdCandidates.Select<string, int>((string cmd) => cmd.Length).Min();
			string text = cmdCandidates.First<string>();
			for (int i = 0; i < 2147483647; i++)
			{
				foreach (string text2 in cmdCandidates)
				{
					if (num == i || char.ToLowerInvariant(text2[i]) != char.ToLowerInvariant(text[i]))
					{
						return text2.Substring(0, i);
					}
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Call this to attempt to execute a command that is issued locally.
		/// If the command must be networked and a run is in progress, a GameAction will be enqueued to all peers before
		/// the command is executed.
		/// </summary>
		/// <param name="inputValue">The command input by the player.</param>
		// Token: 0x0600855B RID: 34139 RVA: 0x002CCA40 File Offset: 0x002CAC40
		public CmdResult ProcessCommand(string inputValue)
		{
			inputValue = inputValue.Trim();
			this.history.Enqueue(inputValue);
			this.historyIndex = 0;
			this.SaveCommandHistory();
			string[] array = inputValue.Split(' ', StringSplitOptions.None);
			Player me = LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState());
			AbstractConsoleCmd abstractConsoleCmd;
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && this._commands.TryGetValue(array[0].ToLowerInvariant(), out abstractConsoleCmd) && abstractConsoleCmd.IsNetworked && me != null)
			{
				RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new ConsoleCmdGameAction(me, inputValue, CombatManager.Instance.IsInProgress));
				bool flag = true;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Enqueued ");
				defaultInterpolatedStringHandler.AppendFormatted(array[0]);
				defaultInterpolatedStringHandler.AppendLiteral(" command: '");
				defaultInterpolatedStringHandler.AppendFormatted(inputValue);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			CmdResult cmdResult = this.ProcessCommandInternal(me, array);
			if (cmdResult.task != null)
			{
				TaskHelper.RunSafely(cmdResult.task);
			}
			return cmdResult;
		}

		/// <summary>
		/// Call this when a networked console command is received.
		/// You are expected to await CmdResult.task.
		/// </summary>
		/// <param name="player">The player that sent the command.</param>
		/// <param name="inputValue">The command input by the player.</param>
		// Token: 0x0600855C RID: 34140 RVA: 0x002CCB48 File Offset: 0x002CAD48
		public CmdResult ProcessNetCommand([Nullable(2)] Player player, string inputValue)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Executing DevConsole command (player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong?>((player != null) ? new ulong?(player.NetId) : null);
			defaultInterpolatedStringHandler.AppendLiteral("): `");
			defaultInterpolatedStringHandler.AppendFormatted(inputValue);
			defaultInterpolatedStringHandler.AppendLiteral("`");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			string[] array = inputValue.Split(' ', StringSplitOptions.None);
			return this.ProcessCommandInternal(player, array);
		}

		// Token: 0x0600855D RID: 34141 RVA: 0x002CCBCC File Offset: 0x002CADCC
		private CmdResult ProcessCommandInternal([Nullable(2)] Player player, string[] tokens)
		{
			string text = tokens[0];
			string[] array = tokens.Skip<string>(1).ToArray<string>();
			return this.ProcessCommand(player, text, array);
		}

		// Token: 0x0600855E RID: 34142 RVA: 0x002CCBF8 File Offset: 0x002CADF8
		private CmdResult ProcessCommand([Nullable(2)] Player player, string cmdName, string[] args)
		{
			args = args.Where<string>((string a) => !string.IsNullOrWhiteSpace(a)).ToArray<string>();
			if (cmdName.Equals("help"))
			{
				return this.HelpCmd(args);
			}
			AbstractConsoleCmd abstractConsoleCmd;
			if (this._commands.TryGetValue(cmdName.ToLowerInvariant(), out abstractConsoleCmd))
			{
				Log.Info("DevConsole: " + cmdName + " " + string.Join(" ", args), 2);
				return abstractConsoleCmd.Process(player, args);
			}
			return new CmdResult(false, "The command '" + cmdName + "' does not exist.\nYou can use the 'help' to get a list of all possible commands.\n");
		}

		// Token: 0x0600855F RID: 34143 RVA: 0x002CCC9C File Offset: 0x002CAE9C
		private CmdResult HelpCmd(string[] args)
		{
			if (args.Length == 0)
			{
				int num = this._commands.Values.Select<AbstractConsoleCmd, int>((AbstractConsoleCmd c) => c.CmdName.Length).Max();
				StringBuilder stringBuilder = new StringBuilder().Append("Usage:\n");
				foreach (KeyValuePair<string, AbstractConsoleCmd> keyValuePair in this._commands)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\t[gold]");
					appendInterpolatedStringHandler.AppendFormatted(keyValuePair.Value.CmdName.PadRight(num));
					appendInterpolatedStringHandler.AppendLiteral("[/gold] - ");
					appendInterpolatedStringHandler.AppendFormatted(keyValuePair.Value.Description);
					appendInterpolatedStringHandler.AppendLiteral("\n");
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				stringBuilder.Append("Use 'help <cmd>' to obtain help on a specific command.");
				return new CmdResult(true, stringBuilder.ToString());
			}
			AbstractConsoleCmd abstractConsoleCmd;
			if (!this._commands.TryGetValue(args[0], out abstractConsoleCmd))
			{
				return new CmdResult(false, "No command named " + args[0] + " found!");
			}
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 3);
			defaultInterpolatedStringHandler.AppendLiteral("[gold]");
			defaultInterpolatedStringHandler.AppendFormatted(abstractConsoleCmd.CmdName);
			defaultInterpolatedStringHandler.AppendLiteral("[/gold] ");
			defaultInterpolatedStringHandler.AppendFormatted(abstractConsoleCmd.Args);
			defaultInterpolatedStringHandler.AppendLiteral("\n\t");
			defaultInterpolatedStringHandler.AppendFormatted(abstractConsoleCmd.Description);
			return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04002E58 RID: 11864
		private readonly Dictionary<string, AbstractConsoleCmd> _commands = new Dictionary<string, AbstractConsoleCmd>();

		// Token: 0x04002E59 RID: 11865
		public readonly FixedSizedQueue<string> history;

		// Token: 0x04002E5A RID: 11866
		public int historyIndex;

		// Token: 0x04002E5B RID: 11867
		private readonly string _historyFilePath;
	}
}
