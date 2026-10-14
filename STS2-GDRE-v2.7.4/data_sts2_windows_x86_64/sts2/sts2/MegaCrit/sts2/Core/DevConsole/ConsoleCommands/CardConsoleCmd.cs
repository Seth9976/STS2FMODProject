using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE7 RID: 3303
	[NullableContext(1)]
	[Nullable(0)]
	public class CardConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024A2 RID: 9378
		// (get) Token: 0x060085B3 RID: 34227 RVA: 0x002CE68F File Offset: 0x002CC88F
		public override string CmdName
		{
			get
			{
				return "card";
			}
		}

		// Token: 0x170024A3 RID: 9379
		// (get) Token: 0x060085B4 RID: 34228 RVA: 0x002CE696 File Offset: 0x002CC896
		public override string Args
		{
			get
			{
				return "<card-id:string> [pileName:string]";
			}
		}

		// Token: 0x170024A4 RID: 9380
		// (get) Token: 0x060085B5 RID: 34229 RVA: 0x002CE69D File Offset: 0x002CC89D
		public override string Description
		{
			get
			{
				return "Spawns a card into a pile (hand by default). Screaming snake case ('BODY_SLAM', not 'Body Slam').";
			}
		}

		// Token: 0x170024A5 RID: 9381
		// (get) Token: 0x060085B6 RID: 34230 RVA: 0x002CE6A4 File Offset: 0x002CC8A4
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085B7 RID: 34231 RVA: 0x002CE6A8 File Offset: 0x002CC8A8
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
			PileType pileType = PileType.Hand;
			if (args.Length >= 2 && !AbstractConsoleCmd.TryParseEnum<PileType>(args[1], out pileType))
			{
				return new CmdResult(false, "Unknown pile '" + args[1] + "'. Valid piles: " + string.Join(", ", Enum.GetNames<PileType>()));
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (pileType == PileType.Hand)
			{
				CardPile pile = PileType.Hand.GetPile(issuingPlayer);
				int count = pile.Cards.Count;
				if (count >= CardPile.MaxCardsInHand)
				{
					bool flag = false;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The hand is full (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(count);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(CardPile.MaxCardsInHand);
					defaultInterpolatedStringHandler.AppendLiteral(").");
					return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			string cardName = args[0].ToUpperInvariant();
			CardModel cardModel = ModelDb.AllCards.FirstOrDefault<CardModel>((CardModel c) => c.Id.Entry == cardName);
			if (cardModel == null)
			{
				return new CmdResult(false, "Card '" + cardName + "' not found");
			}
			ICardScope cardScope2;
			if (!pileType.IsCombatPile())
			{
				ICardScope cardScope = RunManager.Instance.DebugOnlyGetState();
				cardScope2 = cardScope;
			}
			else
			{
				ICardScope cardScope = CombatManager.Instance.DebugOnlyGetState();
				cardScope2 = cardScope;
			}
			ICardScope cardScope3 = cardScope2;
			CardModel cardModel2 = cardScope3.CreateCard(cardModel, issuingPlayer);
			Task task = CardPileCmd.Add(cardModel2, pileType, CardPilePosition.Bottom, null, false);
			Task task2 = task;
			bool flag2 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Added card '");
			defaultInterpolatedStringHandler.AppendFormatted(cardModel.Id.Entry);
			defaultInterpolatedStringHandler.AppendLiteral("' to '");
			defaultInterpolatedStringHandler.AppendFormatted<PileType>(pileType);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			return new CmdResult(task2, flag2, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060085B8 RID: 34232 RVA: 0x002CE870 File Offset: 0x002CCA70
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.AllCards.Select<CardModel, string>((CardModel card) => card.Id.Entry).ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			if (args.Length == 2)
			{
				List<string> list2 = Enum.GetNames<PileType>().ToList<string>();
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
