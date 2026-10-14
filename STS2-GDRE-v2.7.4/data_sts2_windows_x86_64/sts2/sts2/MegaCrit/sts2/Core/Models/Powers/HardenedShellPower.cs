using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200068C RID: 1676
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HardenedShellPower : PowerModel
	{
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x060058BE RID: 22718 RVA: 0x0024F315 File Offset: 0x0024D515
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x060058BF RID: 22719 RVA: 0x0024F318 File Offset: 0x0024D518
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x060058C0 RID: 22720 RVA: 0x0024F31B File Offset: 0x0024D51B
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x060058C1 RID: 22721 RVA: 0x0024F31E File Offset: 0x0024D51E
		public override int DisplayAmount
		{
			get
			{
				return (int)Math.Max(0m, base.Amount - base.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn);
			}
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x0024F34A File Offset: 0x0024D54A
		protected override object InitInternalData()
		{
			return new HardenedShellPower.Data();
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x0024F351 File Offset: 0x0024D551
		[NullableContext(2)]
		public override decimal ModifyHpLostBeforeOstyLate([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return amount;
			}
			if (amount == 0m)
			{
				return amount;
			}
			return Math.Min(amount, base.Amount - base.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn);
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x0024F38E File Offset: 0x0024D58E
		public override Task AfterModifyingHpLostBeforeOsty()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x0024F39C File Offset: 0x0024D59C
		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (result.WasFullyBlocked)
			{
				return Task.CompletedTask;
			}
			HardenedShellPower.Data internalData = base.GetInternalData<HardenedShellPower.Data>();
			internalData.damageReceivedThisTurn += result.UnblockedDamage;
			base.InvokeDisplayAmountChanged();
			if (internalData.damageReceivedThisTurn >= base.Amount)
			{
				base.Owner.HpDisplay = HpDisplay.InfiniteWithNumbers;
			}
			return Task.CompletedTask;
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x0024F418 File Offset: 0x0024D618
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			base.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn = 0m;
			base.InvokeDisplayAmountChanged();
			base.Owner.HpDisplay = HpDisplay.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x02001BE0 RID: 7136
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006F2A RID: 28458
			public decimal damageReceivedThisTurn;
		}
	}
}
