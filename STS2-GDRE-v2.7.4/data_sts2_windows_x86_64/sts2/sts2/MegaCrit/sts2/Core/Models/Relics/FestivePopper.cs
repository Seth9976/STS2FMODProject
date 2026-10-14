using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000549 RID: 1353
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FestivePopper : RelicModel
	{
		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0023FB5F File Offset: 0x0023DD5F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0023FB62 File Offset: 0x0023DD62
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(9m, ValueProp.Unpowered));
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x0023FB78 File Offset: 0x0023DD78
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				ICombatState combatState = player.Creature.CombatState;
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					VfxCmd.PlayOnCreatureCenters(combatState.HittableEnemies, "vfx/vfx_attack_slash");
					await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
				}
			}
		}
	}
}
