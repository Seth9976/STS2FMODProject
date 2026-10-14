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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D9 RID: 1753
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReflectPower : PowerModel
	{
		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x00252977 File Offset: 0x00250B77
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x0025297A File Offset: 0x00250B7A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x00252980 File Offset: 0x00250B80
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.BlockedDamage > 0)
				{
					if (props.IsPoweredAttack())
					{
						if (dealer != null)
						{
							await CreatureCmd.Damage(choiceContext, dealer, result.BlockedDamage, ValueProp.Unpowered, base.Owner);
						}
					}
				}
			}
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x002529F0 File Offset: 0x00250BF0
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
