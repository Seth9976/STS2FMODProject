using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A8 RID: 1704
	public sealed class LoopPower : PowerModel
	{
		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x002507CF File Offset: 0x0024E9CF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06005969 RID: 22889 RVA: 0x002507D2 File Offset: 0x0024E9D2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x002507D8 File Offset: 0x0024E9D8
		[NullableContext(1)]
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				if (player.PlayerCombatState.OrbQueue.Orbs.Count != 0)
				{
					for (int i = 0; i < base.Amount; i++)
					{
						await OrbCmd.Passive(choiceContext, player.PlayerCombatState.OrbQueue.Orbs[0], null, false);
						await Cmd.Wait(0.25f, false);
					}
				}
			}
		}
	}
}
