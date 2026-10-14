using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A3 RID: 1699
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JugglingPower : PowerModel
	{
		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x0600594A RID: 22858 RVA: 0x002503E3 File Offset: 0x0024E5E3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x0600594B RID: 22859 RVA: 0x002503E6 File Offset: 0x0024E5E6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x002503E9 File Offset: 0x0024E5E9
		protected override object InitInternalData()
		{
			return new JugglingPower.Data();
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x002503F0 File Offset: 0x0024E5F0
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			base.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == base.Owner.Player && e.HappenedThisTurn(base.CombatState));
			return Task.CompletedTask;
		}

		// Token: 0x0600594E RID: 22862 RVA: 0x00250424 File Offset: 0x0024E624
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				if (cardPlay.Card.Type == CardType.Attack)
				{
					base.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn++;
					if (base.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn == 3)
					{
						base.Flash();
						for (int i = 0; i < base.Amount; i++)
						{
							CardModel cardModel = cardPlay.Card.CreateClone();
							await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
						}
					}
				}
			}
		}

		// Token: 0x0600594F RID: 22863 RVA: 0x0025046F File Offset: 0x0024E66F
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x02001C00 RID: 7168
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006FC1 RID: 28609
			public int attacksPlayedThisTurn;
		}
	}
}
