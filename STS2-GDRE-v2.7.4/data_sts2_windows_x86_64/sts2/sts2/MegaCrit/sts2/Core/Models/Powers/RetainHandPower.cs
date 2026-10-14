using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006DC RID: 1756
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RetainHandPower : PowerModel
	{
		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x06005A9B RID: 23195 RVA: 0x00252AAE File Offset: 0x00250CAE
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06005A9C RID: 23196 RVA: 0x00252AB1 File Offset: 0x00250CB1
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x00252AB4 File Offset: 0x00250CB4
		public override bool ShouldFlush(Player player)
		{
			return player != base.Owner.Player;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x00252AC8 File Offset: 0x00250CC8
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
