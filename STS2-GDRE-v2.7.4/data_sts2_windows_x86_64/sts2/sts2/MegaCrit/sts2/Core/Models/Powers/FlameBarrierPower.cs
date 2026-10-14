using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000676 RID: 1654
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlameBarrierPower : PowerModel
	{
		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06005847 RID: 22599 RVA: 0x0024E573 File Offset: 0x0024C773
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06005848 RID: 22600 RVA: 0x0024E576 File Offset: 0x0024C776
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x0024E57C File Offset: 0x0024C77C
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult _, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel __)
		{
			if (target == base.Owner)
			{
				if (dealer != null)
				{
					if (props.IsPoweredAttack())
					{
						await CreatureCmd.Damage(choiceContext, dealer, base.Amount, ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x0024E5E4 File Offset: 0x0024C7E4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (base.Owner.Side != side)
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
