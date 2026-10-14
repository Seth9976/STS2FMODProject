using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FB RID: 1531
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tingsha : RelicModel
	{
		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600557A RID: 21882 RVA: 0x002489D2 File Offset: 0x00246BD2
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x0600557B RID: 21883 RVA: 0x002489D5 File Offset: 0x00246BD5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Unpowered));
			}
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x002489E8 File Offset: 0x00246BE8
		public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
		{
			if (card.Owner == base.Owner)
			{
				if (base.Owner.Creature.Side == base.Owner.Creature.CombatState.CurrentSide)
				{
					base.Flash();
					Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(base.Owner.Creature.CombatState.HittableEnemies);
					if (creature != null)
					{
						VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_blunt");
						await CreatureCmd.Damage(choiceContext, creature, base.DynamicVars.Damage, base.Owner.Creature);
					}
				}
			}
		}
	}
}
