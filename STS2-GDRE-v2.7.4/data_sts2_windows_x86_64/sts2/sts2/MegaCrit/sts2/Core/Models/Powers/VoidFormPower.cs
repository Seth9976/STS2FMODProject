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
	// Token: 0x0200072B RID: 1835
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VoidFormPower : PowerModel
	{
		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x06005C8F RID: 23695 RVA: 0x00256AE8 File Offset: 0x00254CE8
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x06005C90 RID: 23696 RVA: 0x00256AEB File Offset: 0x00254CEB
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x00256AEE File Offset: 0x00254CEE
		protected override object InitInternalData()
		{
			return new VoidFormPower.Data();
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x00256AF5 File Offset: 0x00254CF5
		public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (power != this)
			{
				return Task.CompletedTask;
			}
			this.HideTemporaryZeroCostVisual();
			return Task.CompletedTask;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x00256B0C File Offset: 0x00254D0C
		public override Task BeforeApplied(Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			this.HideTemporaryZeroCostVisual();
			return Task.CompletedTask;
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x00256B19 File Offset: 0x00254D19
		public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (this.ShouldSkip(card))
			{
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x00256B35 File Offset: 0x00254D35
		public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (this.ShouldSkip(card))
			{
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x00256B54 File Offset: 0x00254D54
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner && cardPlay != null && !cardPlay.IsAutoPlay && cardPlay.IsLastInSeries)
			{
				base.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn++;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x00256BA4 File Offset: 0x00254DA4
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn = 0;
			return Task.CompletedTask;
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x00256BCC File Offset: 0x00254DCC
		private bool ShouldSkip(CardModel card)
		{
			bool flag = card.Owner.Creature != base.Owner;
			bool flag2 = flag;
			if (!flag2)
			{
				CardPile pile = card.Pile;
				PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : null);
				bool flag3;
				if (pileType != null)
				{
					PileType valueOrDefault = pileType.GetValueOrDefault();
					if (valueOrDefault == PileType.Hand || valueOrDefault == PileType.Play)
					{
						flag3 = true;
						goto IL_005E;
					}
				}
				flag3 = false;
				IL_005E:
				flag2 = !flag3;
			}
			return flag2 || base.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn >= base.Amount;
		}

		/// <summary>
		/// HACK: If Void Form is the first card played in a turn, there's a brief period before the turn is auto-ended
		/// where we show a zero energy cost on all cards.
		/// To avoid this, we max out cardsPlayedThisTurn, which doesn't matter anyways since the turn is ending.
		/// </summary>
		// Token: 0x06005C99 RID: 23705 RVA: 0x00256C58 File Offset: 0x00254E58
		private void HideTemporaryZeroCostVisual()
		{
			base.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn = 999999999;
		}

		// Token: 0x02001CB8 RID: 7352
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x040073AE RID: 29614
			public int cardsPlayedThisTurn;
		}
	}
}
