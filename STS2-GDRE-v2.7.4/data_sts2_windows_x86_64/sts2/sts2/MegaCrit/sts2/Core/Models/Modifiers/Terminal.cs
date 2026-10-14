using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000822 RID: 2082
	public class Terminal : ModifierModel
	{
		// Token: 0x06006757 RID: 26455 RVA: 0x00272528 File Offset: 0x00270728
		[NullableContext(1)]
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (base.RunState.BaseRoom == room)
			{
				foreach (Player player in base.RunState.Players)
				{
					Creature creature = player.Creature;
					if (creature == null || creature.CurrentHp > 1 || creature.MaxHp > 1)
					{
						await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), player.Creature, 1m, false);
					}
				}
				IEnumerator<Player> enumerator = null;
			}
			CombatRoom combatRoom = room as CombatRoom;
			if (combatRoom != null)
			{
				foreach (Creature creature2 in combatRoom.CombatState.PlayerCreatures)
				{
					await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), creature2, 5m, null, null, false);
				}
				IEnumerator<Creature> enumerator2 = null;
			}
		}
	}
}
