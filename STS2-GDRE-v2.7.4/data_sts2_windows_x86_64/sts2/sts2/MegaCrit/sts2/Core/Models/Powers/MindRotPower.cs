using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006AE RID: 1710
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MindRotPower : PowerModel
	{
		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06005981 RID: 22913 RVA: 0x00250A03 File Offset: 0x0024EC03
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x00250A06 File Offset: 0x0024EC06
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x00250A09 File Offset: 0x0024EC09
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return Math.Max(0m, count - base.Amount);
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x00250A36 File Offset: 0x0024EC36
		public override Task AfterModifyingHandDraw()
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
