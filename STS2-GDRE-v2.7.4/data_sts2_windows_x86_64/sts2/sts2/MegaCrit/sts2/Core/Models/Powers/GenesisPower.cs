using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000685 RID: 1669
	public sealed class GenesisPower : PowerModel
	{
		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06005894 RID: 22676 RVA: 0x0024EE07 File Offset: 0x0024D007
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06005895 RID: 22677 RVA: 0x0024EE0A File Offset: 0x0024D00A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x0024EE10 File Offset: 0x0024D010
		[NullableContext(1)]
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				base.Flash();
				await PlayerCmd.GainStars(base.Amount, base.Owner.Player);
			}
		}
	}
}
