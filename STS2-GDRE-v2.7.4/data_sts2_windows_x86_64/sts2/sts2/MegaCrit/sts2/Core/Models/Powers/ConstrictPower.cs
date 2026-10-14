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
	// Token: 0x02000649 RID: 1609
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConstrictPower : PowerModel
	{
		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x06005754 RID: 22356 RVA: 0x0024CAF3 File Offset: 0x0024ACF3
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06005755 RID: 22357 RVA: 0x0024CAF6 File Offset: 0x0024ACF6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x0024CAFC File Offset: 0x0024ACFC
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered, base.Owner);
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x0024CB50 File Offset: 0x0024AD50
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Applier)
				{
					await PowerCmd.Remove(this);
				}
			}
		}
	}
}
