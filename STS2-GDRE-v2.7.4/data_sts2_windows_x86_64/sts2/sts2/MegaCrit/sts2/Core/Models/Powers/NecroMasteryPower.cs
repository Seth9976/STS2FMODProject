using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B3 RID: 1715
	public sealed class NecroMasteryPower : PowerModel
	{
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x00250C7F File Offset: 0x0024EE7F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x060059A0 RID: 22944 RVA: 0x00250C82 File Offset: 0x0024EE82
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x00250C88 File Offset: 0x0024EE88
		[NullableContext(1)]
		public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
		{
			if (!(delta >= 0m))
			{
				if (creature.Monster is Osty)
				{
					if (creature.PetOwner == base.Owner.Player)
					{
						await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature.CombatState.HittableEnemies, -delta * base.Amount, ValueProp.Unblockable | ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}
	}
}
