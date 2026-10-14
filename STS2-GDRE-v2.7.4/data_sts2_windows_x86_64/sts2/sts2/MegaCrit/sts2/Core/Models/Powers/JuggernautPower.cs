using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A2 RID: 1698
	public sealed class JuggernautPower : PowerModel
	{
		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x00250381 File Offset: 0x0024E581
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06005947 RID: 22855 RVA: 0x00250384 File Offset: 0x0024E584
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x00250388 File Offset: 0x0024E588
		[NullableContext(1)]
		public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount <= 0m))
			{
				if (creature == base.Owner)
				{
					IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
					if (hittableEnemies.Count != 0)
					{
						Creature creature2 = base.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>(hittableEnemies);
						base.Flash();
						await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature2, base.Amount, ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}
	}
}
