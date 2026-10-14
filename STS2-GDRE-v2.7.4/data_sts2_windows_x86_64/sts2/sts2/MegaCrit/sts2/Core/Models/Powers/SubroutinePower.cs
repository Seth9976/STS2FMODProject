using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000708 RID: 1800
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SubroutinePower : PowerModel
	{
		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x002548CB File Offset: 0x00252ACB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x002548CE File Offset: 0x00252ACE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x002548D1 File Offset: 0x00252AD1
		protected override object InitInternalData()
		{
			return new SubroutinePower.Data();
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x002548D8 File Offset: 0x00252AD8
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Power)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<SubroutinePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x00254938 File Offset: 0x00252B38
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				int energy;
				if (base.GetInternalData<SubroutinePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out energy))
				{
					if (energy > 0)
					{
						base.Flash();
						for (int i = 0; i < energy; i++)
						{
							await PlayerCmd.GainEnergy(1m, base.Owner.Player);
						}
					}
				}
			}
		}

		// Token: 0x02001C80 RID: 7296
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the Power cards we've seen played and the power amount at the time they were played.
			/// This lets SubroutinePower avoid triggering on cards that started play before it was applied, and avoid
			/// granting extra energy on multiple plays of Subroutine.
			/// </summary>
			// Token: 0x04007264 RID: 29284
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
