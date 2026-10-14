using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x0200081E RID: 2078
	[NullableContext(1)]
	[Nullable(0)]
	public class Murderous : ModifierModel
	{
		// Token: 0x06006748 RID: 26440 RVA: 0x00272270 File Offset: 0x00270470
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			CombatRoom combatRoom = room as CombatRoom;
			if (combatRoom != null)
			{
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), combatRoom.CombatState.Creatures, 3m, null, null, false);
			}
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x002722B3 File Offset: 0x002704B3
		public override Task AfterCreatureAddedToCombat(Creature creature)
		{
			if (creature.Side == CombatSide.Player)
			{
				return Task.CompletedTask;
			}
			return PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, 3m, null, null, false);
		}

		// Token: 0x04002751 RID: 10065
		private const int _strengthAmount = 3;
	}
}
