using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CDE RID: 3294
	[NullableContext(1)]
	[Nullable(0)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public abstract class AbstractConsoleCmd
	{
		// Token: 0x1700247B RID: 9339
		// (get) Token: 0x0600856D RID: 34157
		public abstract string CmdName { get; }

		// Token: 0x1700247C RID: 9340
		// (get) Token: 0x0600856E RID: 34158
		public abstract string Args { get; }

		// Token: 0x1700247D RID: 9341
		// (get) Token: 0x0600856F RID: 34159
		public abstract string Description { get; }

		// Token: 0x1700247E RID: 9342
		// (get) Token: 0x06008570 RID: 34160
		public abstract bool IsNetworked { get; }

		// Token: 0x1700247F RID: 9343
		// (get) Token: 0x06008571 RID: 34161 RVA: 0x002CCF05 File Offset: 0x002CB105
		public virtual bool DebugOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008572 RID: 34162
		public abstract CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args);

		/// <summary>
		/// Provides argument completion candidates for console commands.
		/// Commands should override this method to provide context-aware completion suggestions.
		/// Returns an empty result by default for commands that don't require argument completion.
		/// </summary>
		/// <param name="player">The player invoking the command</param>
		/// <param name="args">Current command arguments being typed</param>
		/// <returns>Completion result containing candidates and context information</returns>
		// Token: 0x06008573 RID: 34163 RVA: 0x002CCF08 File Offset: 0x002CB108
		public virtual CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName,
				ArgumentIndex = args.Length - 1,
				CommandPrefix = this.BuildPrefix(args)
			};
		}

		/// <summary>
		/// Creates a completion result for command arguments with explicit separation of completed vs partial arguments.
		/// This makes the completion logic clear and prevents bugs from implicit argument slicing.
		/// </summary>
		/// <param name="candidates">All possible completion candidates</param>
		/// <param name="completedArgs">Arguments that are already fully typed (e.g., ["OFFERING"] when completing second arg)</param>
		/// <param name="partialArg">The argument currently being typed (e.g., "de" when typing "deck")</param>
		/// <param name="type">The type of completion being performed</param>
		/// <param name="matchPredicate">Optional custom matching predicate (defaults to StartsWith)</param>
		/// <returns>CompletionResult with filtered candidates and completion prefix</returns>
		// Token: 0x06008574 RID: 34164 RVA: 0x002CCF3C File Offset: 0x002CB13C
		protected CompletionResult CompleteArgument(IEnumerable<string> candidates, string[] completedArgs, string partialArg, CompletionType type = CompletionType.Argument, [Nullable(new byte[] { 2, 1, 1 })] Func<string, string, bool> matchPredicate = null)
		{
			List<string> list = candidates.ToList<string>();
			if (matchPredicate == null)
			{
				matchPredicate = (string candidate, string partial) => candidate.StartsWith(partial, StringComparison.OrdinalIgnoreCase);
			}
			List<string> list2;
			if (string.IsNullOrWhiteSpace(partialArg))
			{
				list2 = list;
			}
			else
			{
				list2 = list.Where<string>((string c) => matchPredicate(c, partialArg)).ToList<string>();
			}
			string text = this.BuildPrefix(completedArgs);
			string text2 = this.CalculateCommonCompletion(list2, text);
			return new CompletionResult
			{
				Candidates = list2,
				CommonPrefix = text2,
				Type = type,
				ArgumentContext = this.CmdName,
				ArgumentIndex = completedArgs.Length,
				CommandPrefix = text
			};
		}

		/// <summary>
		/// Builds the command prefix from completed arguments (not including the partial arg being typed).
		/// Example: BuildPrefix(["OFFERING"]) returns "card OFFERING "
		/// </summary>
		// Token: 0x06008575 RID: 34165 RVA: 0x002CD005 File Offset: 0x002CB205
		protected string BuildPrefix(string[] completedArgs)
		{
			if (completedArgs.Length == 0)
			{
				return this.CmdName + " ";
			}
			return this.CmdName + " " + string.Join(" ", completedArgs) + " ";
		}

		/// <summary>
		/// Parses a string into an enum value, rejecting numeric strings and undefined values.
		/// Use this instead of Enum.TryParse to avoid accepting raw integers as valid enum values.
		/// </summary>
		// Token: 0x06008576 RID: 34166 RVA: 0x002CD03C File Offset: 0x002CB23C
		[NullableContext(0)]
		protected static bool TryParseEnum<T>([Nullable(1)] string input, out T result) where T : struct, Enum
		{
			return Enum.TryParse<T>(input, true, out result) && Enum.IsDefined<T>(result);
		}

		/// <summary>
		/// Calculates the common completion string from filtered candidates.
		/// Single match gets trailing space, multiple matches get longest common prefix.
		/// </summary>
		// Token: 0x06008577 RID: 34167 RVA: 0x002CD058 File Offset: 0x002CB258
		private string CalculateCommonCompletion(List<string> filtered, string prefix)
		{
			if (filtered.Count == 0)
			{
				return "";
			}
			if (filtered.Count == 1)
			{
				return prefix + filtered[0] + " ";
			}
			int num = filtered.Min<string>((string s) => s.Length);
			string text = filtered[0];
			int num2 = 0;
			int i;
			int j;
			for (i = 0; i < num; i = j + 1)
			{
				char c = text[i];
				if (!filtered.All<string>((string s) => char.ToLowerInvariant(s[i]) == char.ToLowerInvariant(c)))
				{
					break;
				}
				num2 = i + 1;
				j = i;
			}
			if (num2 > 0)
			{
				return prefix + text.Substring(0, num2);
			}
			return "";
		}
	}
}
