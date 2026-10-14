using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200066A RID: 1642
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EnergyNextTurnPower : PowerModel
	{
		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06005810 RID: 22544 RVA: 0x0024E089 File Offset: 0x0024C289
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06005811 RID: 22545 RVA: 0x0024E08C File Offset: 0x0024C28C
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06005812 RID: 22546 RVA: 0x0024E08F File Offset: 0x0024C28F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x0024E09C File Offset: 0x0024C29C
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				await PlayerCmd.GainEnergy(base.Amount, player);
				await PowerCmd.Remove(this);
			}
		}
	}
}
