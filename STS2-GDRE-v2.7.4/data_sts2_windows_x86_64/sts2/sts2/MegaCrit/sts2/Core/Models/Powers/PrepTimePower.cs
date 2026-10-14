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
	// Token: 0x020006D0 RID: 1744
	public sealed class PrepTimePower : PowerModel
	{
		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06005A54 RID: 23124 RVA: 0x0025220B File Offset: 0x0025040B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06005A55 RID: 23125 RVA: 0x0025220E File Offset: 0x0025040E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x00252214 File Offset: 0x00250414
		[NullableContext(1)]
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
