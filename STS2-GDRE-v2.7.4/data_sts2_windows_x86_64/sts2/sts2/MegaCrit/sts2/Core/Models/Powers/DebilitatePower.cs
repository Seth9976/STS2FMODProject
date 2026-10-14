using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200065C RID: 1628
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DebilitatePower : PowerModel
	{
		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x060057BC RID: 22460 RVA: 0x0024D641 File Offset: 0x0024B841
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x060057BD RID: 22461 RVA: 0x0024D644 File Offset: 0x0024B844
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x060057BE RID: 22462 RVA: 0x0024D648 File Offset: 0x0024B848
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<VulnerablePower>(null),
					HoverTipFactory.FromPower<WeakPower>(null)
				});
			}
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x0024D682 File Offset: 0x0024B882
		[NullableContext(2)]
		public decimal ModifyVulnerableMultiplier([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			return amount + (amount - 1m);
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x0024D6AA File Offset: 0x0024B8AA
		[NullableContext(2)]
		public decimal ModifyWeakMultiplier([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (dealer != base.Owner)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			return amount - (1m - amount);
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x0024D6D4 File Offset: 0x0024B8D4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
