using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	// Token: 0x02000B85 RID: 2949
	[NullableContext(1)]
	[Nullable(0)]
	public class Play20CardsSingleTurnAchievement : AchievementModel
	{
		// Token: 0x06007B91 RID: 31633 RVA: 0x002A0E60 File Offset: 0x0029F060
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!LocalContext.IsMine(cardPlay.Card))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn++;
			if (this._cardsPlayedThisTurn >= 20)
			{
				AchievementsUtil.Unlock(Achievement.Play20CardsSingleTurn, cardPlay.Card.Owner);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007B92 RID: 31634 RVA: 0x002A0EAF File Offset: 0x0029F0AF
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side != CombatSide.Player)
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x04002908 RID: 10504
		private int _cardsPlayedThisTurn;
	}
}
