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
	// Token: 0x0200054C RID: 1356
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForgottenSoul : RelicModel
	{
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x060050E1 RID: 20705 RVA: 0x0023FD6A File Offset: 0x0023DF6A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x060050E2 RID: 20706 RVA: 0x0023FD6D File Offset: 0x0023DF6D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(1m, ValueProp.Unpowered));
			}
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x0023FD80 File Offset: 0x0023DF80
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
		{
			if (card.Owner == base.Owner)
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
