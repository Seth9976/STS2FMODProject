using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000734 RID: 1844
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockExtraTurnPower : PowerModel
	{
		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x06005CC8 RID: 23752 RVA: 0x002570D6 File Offset: 0x002552D6
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x002570D9 File Offset: 0x002552D9
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06005CCA RID: 23754 RVA: 0x002570DC File Offset: 0x002552DC
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x002570DF File Offset: 0x002552DF
		public override bool ShouldTakeExtraTurn(Player player)
		{
			return player == base.Owner.Player;
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x002570F0 File Offset: 0x002552F0
		public override async Task AfterTakingExtraTurn(Player player)
		{
			if (player == base.Owner.Player)
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
