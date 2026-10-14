using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000720 RID: 1824
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToolsOfTheTradePower : PowerModel
	{
		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06005C4C RID: 23628 RVA: 0x00256093 File Offset: 0x00254293
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06005C4D RID: 23629 RVA: 0x00256096 File Offset: 0x00254296
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x00256099 File Offset: 0x00254299
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return count + base.Amount;
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002560BC File Offset: 0x002542BC
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, base.Amount), null, this);
				List<CardModel> list = enumerable.ToList<CardModel>();
				if (list.Count != 0)
				{
					await CardCmd.Discard(choiceContext, list);
				}
			}
		}
	}
}
