using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F3 RID: 1779
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlothPower : PowerModel
	{
		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06005B2F RID: 23343 RVA: 0x00253C7F File Offset: 0x00251E7F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06005B30 RID: 23344 RVA: 0x00253C82 File Offset: 0x00251E82
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06005B31 RID: 23345 RVA: 0x00253C85 File Offset: 0x00251E85
		public override int DisplayAmount
		{
			get
			{
				return this._cardsPlayedThisTurn;
			}
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x00253C8D File Offset: 0x00251E8D
		public override bool ShouldPlay(CardModel card, AutoPlayType _)
		{
			return card.Owner.Creature != base.Owner || this._cardsPlayedThisTurn < base.Amount;
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x00253CB2 File Offset: 0x00251EB2
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn++;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x00253CEB File Offset: 0x00251EEB
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			this._cardsPlayedThisTurn = 0;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0400251A RID: 9498
		private int _cardsPlayedThisTurn;
	}
}
