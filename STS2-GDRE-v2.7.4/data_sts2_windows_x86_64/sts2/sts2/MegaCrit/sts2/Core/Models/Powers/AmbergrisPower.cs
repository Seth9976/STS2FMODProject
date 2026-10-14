using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062B RID: 1579
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AmbergrisPower : PowerModel
	{
		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060056A8 RID: 22184 RVA: 0x0024B8F7 File Offset: 0x00249AF7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x0024B8FA File Offset: 0x00249AFA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x0024B8FD File Offset: 0x00249AFD
		protected override bool IsVisibleInternal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x0024B900 File Offset: 0x00249B00
		public override bool ShouldTakeExtraTurn(Player player)
		{
			return base.Amount > 0 && player == base.Owner.Player;
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x0024B91C File Offset: 0x00249B1C
		public override async Task AfterTakingExtraTurn(Player player)
		{
			if (player == base.Owner.Player)
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
