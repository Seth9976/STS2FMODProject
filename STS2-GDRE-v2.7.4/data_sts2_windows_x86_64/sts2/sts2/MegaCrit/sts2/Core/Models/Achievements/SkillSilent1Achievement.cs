using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when a player plays a large number of Sly cards from a single card.
	/// </summary>
	// Token: 0x02000B8C RID: 2956
	[NullableContext(1)]
	[Nullable(0)]
	public class SkillSilent1Achievement : AchievementModel
	{
		// Token: 0x06007BA1 RID: 31649 RVA: 0x002A10E3 File Offset: 0x0029F2E3
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (!LocalContext.IsMine(cardPlay.Card))
			{
				return Task.CompletedTask;
			}
			if (this._firstCardOnStack == null)
			{
				this._firstCardOnStack = cardPlay.Card;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x002A1114 File Offset: 0x0029F314
		public override Task BeforeCardAutoPlayed(CardModel card, [Nullable(2)] Creature target, AutoPlayType type)
		{
			if (!LocalContext.IsMine(card))
			{
				return Task.CompletedTask;
			}
			if (type != AutoPlayType.SlyDiscard)
			{
				return Task.CompletedTask;
			}
			if (this._firstCardOnStack == null)
			{
				return Task.CompletedTask;
			}
			this._slyCardsPlayed++;
			if (this._slyCardsPlayed >= 5)
			{
				AchievementsUtil.Unlock(Achievement.CharacterSkillSilent1, card.Owner);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007BA3 RID: 31651 RVA: 0x002A1170 File Offset: 0x0029F370
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!LocalContext.IsMine(cardPlay.Card))
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card == this._firstCardOnStack)
			{
				this._firstCardOnStack = null;
				this._slyCardsPlayed = 0;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007BA4 RID: 31652 RVA: 0x002A11A6 File Offset: 0x0029F3A6
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			this._firstCardOnStack = null;
			this._slyCardsPlayed = 0;
			return Task.CompletedTask;
		}

		// Token: 0x04002910 RID: 10512
		[Nullable(2)]
		private CardModel _firstCardOnStack;

		// Token: 0x04002911 RID: 10513
		private int _slyCardsPlayed;
	}
}
