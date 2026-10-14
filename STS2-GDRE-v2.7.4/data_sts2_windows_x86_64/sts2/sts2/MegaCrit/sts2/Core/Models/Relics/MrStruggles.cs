using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200058F RID: 1423
	public sealed class MrStruggles : RelicModel
	{
		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x060052A3 RID: 21155 RVA: 0x00243344 File Offset: 0x00241544
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x00243348 File Offset: 0x00241548
		[NullableContext(1)]
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				base.Flash();
				ICombatState combatState = player.Creature.CombatState;
				await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, base.Owner.PlayerCombatState.TurnNumber, ValueProp.Unpowered, base.Owner.Creature);
			}
		}
	}
}
