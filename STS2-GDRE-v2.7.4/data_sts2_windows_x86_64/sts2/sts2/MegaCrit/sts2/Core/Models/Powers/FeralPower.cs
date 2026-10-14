using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000675 RID: 1653
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FeralPower : PowerModel
	{
		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x0024E3E3 File Offset: 0x0024C5E3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x0600583D RID: 22589 RVA: 0x0024E3E6 File Offset: 0x0024C5E6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x0024E3E9 File Offset: 0x0024C5E9
		public override int DisplayAmount
		{
			get
			{
				return Math.Max(0, base.Amount - base.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed);
			}
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x0024E403 File Offset: 0x0024C603
		protected override object InitInternalData()
		{
			return new FeralPower.Data();
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x0024E40A File Offset: 0x0024C60A
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			this.SetZeroCostAttacksPlayed(CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().Count<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == base.Owner.Player && e.CardPlay.Resources.EnergyValue == 0 && e.HappenedThisTurn(base.CombatState)));
			return Task.CompletedTask;
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x0024E43C File Offset: 0x0024C63C
		public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation location)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return location;
			}
			if (card.Type != CardType.Attack)
			{
				return location;
			}
			if (resources.EnergyValue > 0)
			{
				return location;
			}
			if (card.IsDupe)
			{
				return location;
			}
			if (base.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed >= base.Amount)
			{
				return location;
			}
			location.pileType = PileType.Hand;
			location.position = CardPilePosition.Top;
			return location;
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x0024E4AB File Offset: 0x0024C6AB
		public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation location)
		{
			base.Flash();
			this.SetZeroCostAttacksPlayed(base.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed + 1);
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x0024E4D1 File Offset: 0x0024C6D1
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			this.SetZeroCostAttacksPlayed(0);
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x0024E4F9 File Offset: 0x0024C6F9
		private void SetZeroCostAttacksPlayed(int value)
		{
			base.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed = value;
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x02001BC6 RID: 7110
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006EAE RID: 28334
			public int zeroCostAttacksPlayed;
		}
	}
}
