using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000563 RID: 1379
	public sealed class HistoryCourse : RelicModel
	{
		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x0600517A RID: 20858 RVA: 0x0024100F File Offset: 0x0023F20F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x00241014 File Offset: 0x0023F214
		[NullableContext(1)]
		public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber != 1)
				{
					CardPlayFinishedEntry cardPlayFinishedEntry = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.CardPlay.Player == base.Owner && e.HappenedLastPlayerTurn(base.Owner) && e.CardPlay.Card.Type == CardType.Attack && !e.CardPlay.Card.IsDupe);
					CardModel cardModel = ((cardPlayFinishedEntry != null) ? cardPlayFinishedEntry.CardPlay.Card : null);
					if (cardModel != null)
					{
						base.Flash();
						await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(player), null, AutoPlayType.Default, false, false);
					}
				}
			}
		}
	}
}
