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
	// Token: 0x020006EA RID: 1770
	public sealed class ShadowStepPower : PowerModel
	{
		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06005AF0 RID: 23280 RVA: 0x0025353B File Offset: 0x0025173B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x0025353E File Offset: 0x0025173E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x00253544 File Offset: 0x00251744
		[NullableContext(1)]
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Apply<DoubleDamagePower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null, false);
				await PowerCmd.Remove(this);
			}
		}
	}
}
