using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000701 RID: 1793
	public sealed class StarNextTurnPower : PowerModel
	{
		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x06005B7A RID: 23418 RVA: 0x0025448F File Offset: 0x0025268F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x00254492 File Offset: 0x00252692
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B7C RID: 23420 RVA: 0x00254498 File Offset: 0x00252698
		[NullableContext(1)]
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				await PlayerCmd.GainStars(base.Amount, player);
				await PowerCmd.Remove(this);
			}
		}
	}
}
