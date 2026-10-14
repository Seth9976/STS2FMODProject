using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B0 RID: 1456
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ParryingShield : RelicModel
	{
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06005394 RID: 21396 RVA: 0x00245079 File Offset: 0x00243279
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06005395 RID: 21397 RVA: 0x0024507C File Offset: 0x0024327C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(10m, ValueProp.Unpowered),
					new DamageVar(6m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x002450A8 File Offset: 0x002432A8
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (!(base.Owner.Creature.Block < base.DynamicVars.Block.BaseValue))
				{
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
