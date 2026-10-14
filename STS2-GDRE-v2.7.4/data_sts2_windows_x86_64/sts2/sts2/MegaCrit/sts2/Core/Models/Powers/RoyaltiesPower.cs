using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E0 RID: 1760
	public sealed class RoyaltiesPower : PowerModel
	{
		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x00252E56 File Offset: 0x00251056
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x00252E59 File Offset: 0x00251059
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00252E5C File Offset: 0x0025105C
		[NullableContext(1)]
		public override Task AfterCombatEnd(CombatRoom room)
		{
			room.AddExtraReward(base.Owner.Player, new GoldReward(base.Amount, base.Owner.Player, false));
			return Task.CompletedTask;
		}
	}
}
