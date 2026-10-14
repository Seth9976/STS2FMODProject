using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000D04 RID: 3332
	[NullableContext(1)]
	[Nullable(0)]
	public class UpgradeCardConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700251B RID: 9499
		// (get) Token: 0x06008699 RID: 34457 RVA: 0x002D29AE File Offset: 0x002D0BAE
		public override string CmdName
		{
			get
			{
				return "upgrade";
			}
		}

		// Token: 0x1700251C RID: 9500
		// (get) Token: 0x0600869A RID: 34458 RVA: 0x002D29B5 File Offset: 0x002D0BB5
		public override string Args
		{
			get
			{
				return "<hand-index:int>";
			}
		}

		// Token: 0x1700251D RID: 9501
		// (get) Token: 0x0600869B RID: 34459 RVA: 0x002D29BC File Offset: 0x002D0BBC
		public override string Description
		{
			get
			{
				return "Upgrade the target card based on its hand position (0 is the left most).";
			}
		}

		// Token: 0x1700251E RID: 9502
		// (get) Token: 0x0600869C RID: 34460 RVA: 0x002D29C3 File Offset: 0x002D0BC3
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600869D RID: 34461 RVA: 0x002D29C8 File Offset: 0x002D0BC8
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			int num = 0;
			if (args.Length != 0 && !int.TryParse(args[0], out num))
			{
				return new CmdResult(false, "Arg 1 must be the hand index (int), got '" + args[0] + "'.");
			}
			CardPile pile = PileType.Hand.GetPile(issuingPlayer);
			IReadOnlyList<CardModel> cards = pile.Cards;
			int count = cards.Count;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num < 0 || num >= count)
			{
				bool flag = false;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid hand index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count - 1);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			CardModel cardModel = pile.Cards[num];
			if (cardModel.CurrentUpgradeLevel == cardModel.MaxUpgradeLevel)
			{
				bool flag2 = false;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The card at index=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" is already upgraded to ");
				defaultInterpolatedStringHandler.AppendLiteral("max_level=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(cardModel.MaxUpgradeLevel);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			bool flag3 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Upgraded '");
			defaultInterpolatedStringHandler.AppendFormatted(cardModel.Title);
			defaultInterpolatedStringHandler.AppendLiteral("' at index '");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("' in hand.");
			return new CmdResult(flag3, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x002D2B5C File Offset: 0x002D0D5C
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1 && RunManager.Instance.IsInProgress && CombatManager.Instance.IsInProgress && player != null)
			{
				CardPile pile = PileType.Hand.GetPile(player);
				int count = pile.Cards.Count;
				if (count > 0)
				{
					List<string> list = (from i in Enumerable.Range(0, count)
						select i.ToString()).ToList<string>();
					return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
				}
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
