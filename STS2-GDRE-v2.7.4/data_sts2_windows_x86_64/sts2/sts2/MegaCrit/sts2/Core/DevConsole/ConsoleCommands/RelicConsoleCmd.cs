using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CFC RID: 3324
	[NullableContext(1)]
	[Nullable(0)]
	public class RelicConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024FB RID: 9467
		// (get) Token: 0x06008654 RID: 34388 RVA: 0x002D13A0 File Offset: 0x002CF5A0
		public override string CmdName
		{
			get
			{
				return "relic";
			}
		}

		// Token: 0x170024FC RID: 9468
		// (get) Token: 0x06008655 RID: 34389 RVA: 0x002D13A7 File Offset: 0x002CF5A7
		public override string Args
		{
			get
			{
				return "[add|remove] <relic-id:string>";
			}
		}

		// Token: 0x170024FD RID: 9469
		// (get) Token: 0x06008656 RID: 34390 RVA: 0x002D13AE File Offset: 0x002CF5AE
		public override string Description
		{
			get
			{
				return "Adds/Removes relic from player (add by default)";
			}
		}

		// Token: 0x170024FE RID: 9470
		// (get) Token: 0x06008657 RID: 34391 RVA: 0x002D13B5 File Offset: 0x002CF5B5
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008658 RID: 34392 RVA: 0x002D13B8 File Offset: 0x002CF5B8
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, this.CmdName + " requires a relic name");
			}
			if (issuingPlayer == null)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			string text;
			if (args[0].ToLowerInvariant().Equals("add") || args[0].ToLowerInvariant().Equals("remove"))
			{
				if (args.Length < 2)
				{
					return new CmdResult(false, "You need to specify a relic!");
				}
				text = args[1];
			}
			else
			{
				text = args[0];
			}
			RelicModel relicById = RelicConsoleCmd.GetRelicById(text);
			if (relicById == null)
			{
				return new CmdResult(false, "Unable to create relic '" + text + "'.");
			}
			if (!args[0].ToLowerInvariant().Equals("remove"))
			{
				TaskHelper.RunSafely(RelicCmd.Obtain(relicById.ToMutable(), issuingPlayer, -1));
				return new CmdResult(true, "Added relic '" + text + "'");
			}
			RelicModel relicById2 = issuingPlayer.GetRelicById(relicById.Id);
			if (relicById2 != null)
			{
				TaskHelper.RunSafely(RelicCmd.Remove(relicById2));
				return new CmdResult(true, "Relic removed!");
			}
			return new CmdResult(false, "Unable to find relic in player!");
		}

		// Token: 0x06008659 RID: 34393 RVA: 0x002D14C8 File Offset: 0x002CF6C8
		[return: Nullable(2)]
		private static RelicModel GetRelicById(string id)
		{
			id = id.ToUpperInvariant();
			List<RelicModel> list = ModelDb.AllRelics.Where<RelicModel>((RelicModel r) => r.Id.Entry.Contains(id)).ToList<RelicModel>();
			RelicModel relicModel = list.FirstOrDefault<RelicModel>((RelicModel r) => r.Id.Entry == id);
			if (relicModel != null)
			{
				return relicModel;
			}
			RelicModel relicModel2 = list.FirstOrDefault<RelicModel>((RelicModel r) => r.Id.Entry.StartsWith(id));
			if (relicModel2 != null)
			{
				return relicModel2;
			}
			return list.FirstOrDefault<RelicModel>();
		}

		// Token: 0x0600865A RID: 34394 RVA: 0x002D1548 File Offset: 0x002CF748
		public unsafe override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length == 0 || (args.Length == 1 && string.IsNullOrWhiteSpace(args[0])))
			{
				CompletionResult completionResult = new CompletionResult();
				int num = 2;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = "add";
				num2++;
				*span[num2] = "remove";
				completionResult.Candidates = list;
				completionResult.Type = CompletionType.Subcommand;
				completionResult.ArgumentContext = this.CmdName;
				return completionResult;
			}
			if (args.Length == 1)
			{
				List<string> list2 = new List<string>();
				string text = args[0].ToLowerInvariant();
				if ("add".StartsWith(text))
				{
					list2.Add("add");
				}
				if ("remove".StartsWith(text))
				{
					list2.Add("remove");
				}
				if (list2.Count > 0)
				{
					return base.CompleteArgument(list2, Array.Empty<string>(), text, CompletionType.Subcommand, null);
				}
				List<string> list3 = ModelDb.AllRelics.Select<RelicModel, string>((RelicModel r) => r.Id.Entry).ToList<string>();
				return base.CompleteArgument(list3, Array.Empty<string>(), args[0], CompletionType.Argument, (string candidate, string partialArg) => candidate.Contains(partialArg, StringComparison.OrdinalIgnoreCase));
			}
			else
			{
				if (args.Length == 2)
				{
					List<string> list4 = ModelDb.AllRelics.Select<RelicModel, string>((RelicModel r) => r.Id.Entry).ToList<string>();
					return base.CompleteArgument(list4, new string[] { args[0] }, args[1], CompletionType.Argument, (string candidate, string partial) => candidate.Contains(partial, StringComparison.OrdinalIgnoreCase));
				}
				return new CompletionResult
				{
					Type = CompletionType.Argument,
					ArgumentContext = this.CmdName
				};
			}
		}
	}
}
