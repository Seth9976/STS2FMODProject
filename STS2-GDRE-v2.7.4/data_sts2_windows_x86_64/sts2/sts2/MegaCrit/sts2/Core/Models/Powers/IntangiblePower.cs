using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069F RID: 1695
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IntangiblePower : PowerModel
	{
		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06005931 RID: 22833 RVA: 0x0025009F File Offset: 0x0024E29F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x002500A2 File Offset: 0x0024E2A2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x002500A5 File Offset: 0x0024E2A5
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return amount;
			}
			if (target != base.Owner)
			{
				return amount;
			}
			if (amount < 1m)
			{
				return amount;
			}
			return 1m;
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x002500D4 File Offset: 0x0024E2D4
		public override Task AfterModifyingHpLostAfterOsty()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Caps damage received at 1.
		/// Note: the HP loss logic is already handled by <see cref="M:MegaCrit.Sts2.Core.Models.Powers.IntangiblePower.ModifyHpLostAfterOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />, the duplicated logic here is
		/// for block loss and preview logic on targeted attacks.
		/// </summary>
		// Token: 0x06005935 RID: 22837 RVA: 0x002500E1 File Offset: 0x0024E2E1
		[NullableContext(2)]
		public override decimal ModifyDamageCap(Creature target, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return decimal.MaxValue;
			}
			return 1m;
		}

		/// <summary>
		/// Note: the HP loss logic is already handled by <see cref="M:MegaCrit.Sts2.Core.Models.Powers.IntangiblePower.AfterModifyingHpLostAfterOsty" />, the duplicated logic
		/// here is for block loss and preview logic on targeted attacks.
		/// </summary>
		// Token: 0x06005936 RID: 22838 RVA: 0x002500FC File Offset: 0x0024E2FC
		public override Task AfterModifyingDamageAmount([Nullable(2)] CardModel cardSource)
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x0025010C File Offset: 0x0024E30C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
