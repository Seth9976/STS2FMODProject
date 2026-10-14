using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000589 RID: 1417
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MercuryHourglass : RelicModel
	{
		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x0600527B RID: 21115 RVA: 0x00242EAA File Offset: 0x002410AA
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x0600527C RID: 21116 RVA: 0x00242EAD File Offset: 0x002410AD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Unpowered));
			}
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x00242EC0 File Offset: 0x002410C0
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				base.Flash();
				await CreatureCmd.Damage(choiceContext, player.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
			}
		}
	}
}
