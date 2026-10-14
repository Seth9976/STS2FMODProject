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
	// Token: 0x020006AA RID: 1706
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MagicBombPower : PowerModel
	{
		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06005970 RID: 22896 RVA: 0x00250864 File Offset: 0x0024EA64
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06005971 RID: 22897 RVA: 0x00250867 File Offset: 0x0024EA67
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06005972 RID: 22898 RVA: 0x0025086A File Offset: 0x0024EA6A
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x00250870 File Offset: 0x0024EA70
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Applier != null && !base.Applier.IsDead)
				{
					base.Flash();
					await Cmd.Wait(0.25f, false);
					await CreatureCmd.Damage(choiceContext, base.Owner, base.Amount, ValueProp.Unpowered, base.Owner);
					await PowerCmd.Remove(this);
				}
			}
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x002508C4 File Offset: 0x0024EAC4
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
