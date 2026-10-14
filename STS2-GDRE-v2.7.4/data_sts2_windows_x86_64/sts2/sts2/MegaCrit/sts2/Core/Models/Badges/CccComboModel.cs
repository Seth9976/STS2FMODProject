using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Play 20 cards in a single turn.
	/// </summary>
	// Token: 0x02000B5E RID: 2910
	[NullableContext(1)]
	[Nullable(0)]
	public class CccComboModel : BadgeModel
	{
		// Token: 0x06007ABD RID: 31421 RVA: 0x0029F1EC File Offset: 0x0029D3EC
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!LocalContext.IsMine(cardPlay.Card))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn++;
			if (this._cardsPlayedThisTurn >= 20)
			{
				Player owner = cardPlay.Card.Owner;
				if (!owner.ExtraFields.CccomboBadgeUnlocked)
				{
					Log.Info("Player played 20 cards in a single turn, ccccombo unlocked", 2);
				}
				owner.ExtraFields.CccomboBadgeUnlocked = true;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x0029F259 File Offset: 0x0029D459
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side != CombatSide.Player)
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x040028FE RID: 10494
		private int _cardsPlayedThisTurn;
	}
}
