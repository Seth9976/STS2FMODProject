using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200067C RID: 1660
	public sealed class ForbiddenGrimoirePower : PowerModel
	{
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06005865 RID: 22629 RVA: 0x0024E865 File Offset: 0x0024CA65
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x0024E868 File Offset: 0x0024CA68
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x0024E86C File Offset: 0x0024CA6C
		[NullableContext(1)]
		public override Task AfterCombatEnd(CombatRoom room)
		{
			for (int i = 0; i < base.Amount; i++)
			{
				room.AddExtraReward(base.Owner.Player, new CardRemovalReward(base.Owner.Player));
			}
			return Task.CompletedTask;
		}
	}
}
