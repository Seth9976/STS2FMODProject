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
	// Token: 0x02000663 RID: 1635
	public sealed class DisintegrationPower : PowerModel
	{
		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x060057E5 RID: 22501 RVA: 0x0024D9CE File Offset: 0x0024BBCE
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x060057E6 RID: 22502 RVA: 0x0024D9D1 File Offset: 0x0024BBD1
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x0024D9D4 File Offset: 0x0024BBD4
		[NullableContext(1)]
		public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered, base.Owner);
				VfxCmd.PlayOnCreatureCenter(base.Owner, "vfx/vfx_attack_blunt");
			}
		}
	}
}
