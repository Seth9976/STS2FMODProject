using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	/// <summary>
	/// remove_card console cmd. Attempts to remove a card from both Hand and Deck at the same time.
	/// </summary>
	// Token: 0x02000CFD RID: 3325
	[NullableContext(1)]
	[Nullable(0)]
	public class RemoveCardConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024FF RID: 9471
		// (get) Token: 0x0600865C RID: 34396 RVA: 0x002D1714 File Offset: 0x002CF914
		public override string CmdName
		{
			get
			{
				return "remove_card";
			}
		}

		// Token: 0x17002500 RID: 9472
		// (get) Token: 0x0600865D RID: 34397 RVA: 0x002D171B File Offset: 0x002CF91B
		public override string Args
		{
			get
			{
				return "<id:string> [pileName:string]";
			}
		}

		// Token: 0x17002501 RID: 9473
		// (get) Token: 0x0600865E RID: 34398 RVA: 0x002D1722 File Offset: 0x002CF922
		public override string Description
		{
			get
			{
				return "Removes a card from your Hand or Deck. Screaming snake case ('BODY_SLAM', not 'Body Slam').";
			}
		}

		// Token: 0x17002502 RID: 9474
		// (get) Token: 0x0600865F RID: 34399 RVA: 0x002D1729 File Offset: 0x002CF929
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008660 RID: 34400 RVA: 0x002D172C File Offset: 0x002CF92C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "No card name specified.");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			string cardName = args[0].ToUpperInvariant();
			CardModel cardModel = ModelDb.AllCards.FirstOrDefault<CardModel>((CardModel c) => c.Id.Entry == cardName);
			PileType pileType = PileType.Hand;
			if (args.Length >= 2 && !AbstractConsoleCmd.TryParseEnum<PileType>(args[1], out pileType))
			{
				return new CmdResult(false, "Unknown pile '" + args[1] + "'. Valid piles: Hand, Deck");
			}
			if (cardModel == null)
			{
				return new CmdResult(false, "Card '" + cardName + "' not found");
			}
			ModelId id = cardModel.Id;
			Task task = Task.CompletedTask;
			if (pileType == PileType.Hand)
			{
				using (IEnumerator<CardModel> enumerator = PileType.Hand.GetPile(issuingPlayer).Cards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardModel cardModel2 = enumerator.Current;
						if (cardModel2.Id == id)
						{
							task = CardPileCmd.RemoveFromCombat(cardModel2, false);
							break;
						}
					}
					goto IL_0161;
				}
			}
			if (pileType == PileType.Deck)
			{
				using (IEnumerator<CardModel> enumerator2 = PileType.Deck.GetPile(issuingPlayer).Cards.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CardModel cardModel3 = enumerator2.Current;
						if (cardModel3.Id == id)
						{
							task = CardPileCmd.RemoveFromDeck(cardModel3, true);
							break;
						}
					}
					goto IL_0161;
				}
			}
			return new CmdResult(false, "Unsupported pile.");
			IL_0161:
			return new CmdResult(task, true, "Removed card '" + id.Entry + "'");
		}

		// Token: 0x06008661 RID: 34401 RVA: 0x002D18D4 File Offset: 0x002CFAD4
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = new List<string>();
				if (player != null && RunManager.Instance.IsInProgress)
				{
					IEnumerable<string> enumerable = PileType.Hand.GetPile(player).Cards.Select<CardModel, string>((CardModel c) => c.Id.Entry).Distinct<string>();
					IEnumerable<string> enumerable2 = PileType.Deck.GetPile(player).Cards.Select<CardModel, string>((CardModel c) => c.Id.Entry).Distinct<string>();
					list.AddRange(enumerable);
					list.AddRange(enumerable2);
					list = list.Distinct<string>().ToList<string>();
				}
				if (list.Count == 0)
				{
					list = ModelDb.AllCards.Select<CardModel, string>((CardModel card) => card.Id.Entry).ToList<string>();
				}
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			if (args.Length == 2)
			{
				List<string> list2 = new List<string> { "Hand", "Deck" };
				return base.CompleteArgument(list2, new string[] { args[0] }, args[1], CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
