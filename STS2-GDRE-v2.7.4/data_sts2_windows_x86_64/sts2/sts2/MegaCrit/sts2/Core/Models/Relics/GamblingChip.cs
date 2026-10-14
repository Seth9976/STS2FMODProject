using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000553 RID: 1363
	public sealed class GamblingChip : RelicModel
	{
		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x0024067F File Offset: 0x0023E87F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x00240684 File Offset: 0x0023E884
		[NullableContext(1)]
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 999999999), null, this);
					List<CardModel> list = enumerable.ToList<CardModel>();
					if (list.Count != 0)
					{
						await CardCmd.DiscardAndDraw(choiceContext, list, list.Count);
					}
				}
			}
		}
	}
}
