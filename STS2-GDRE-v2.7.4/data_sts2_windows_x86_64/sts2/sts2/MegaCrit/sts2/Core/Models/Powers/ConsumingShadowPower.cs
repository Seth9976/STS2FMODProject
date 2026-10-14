using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064A RID: 1610
	public sealed class ConsumingShadowPower : PowerModel
	{
		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06005759 RID: 22361 RVA: 0x0024CBAB File Offset: 0x0024ADAB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x0600575A RID: 22362 RVA: 0x0024CBAE File Offset: 0x0024ADAE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x0024CBB4 File Offset: 0x0024ADB4
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Owner.Player.PlayerCombatState.OrbQueue.Orbs.Count != 0)
				{
					for (int i = 0; i < base.Amount; i++)
					{
						await OrbCmd.EvokeLast(choiceContext, base.Owner.Player, true);
						await Cmd.Wait(0.25f, false);
					}
				}
			}
		}
	}
}
