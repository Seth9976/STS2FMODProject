using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000733 RID: 1843
	public class MockDoNotScaleInMultiplayerPower : PowerModel
	{
		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06005CC2 RID: 23746 RVA: 0x002570BF File Offset: 0x002552BF
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x06005CC3 RID: 23747 RVA: 0x002570C2 File Offset: 0x002552C2
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x06005CC4 RID: 23748 RVA: 0x002570C5 File Offset: 0x002552C5
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x06005CC5 RID: 23749 RVA: 0x002570C8 File Offset: 0x002552C8
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x06005CC6 RID: 23750 RVA: 0x002570CB File Offset: 0x002552CB
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return false;
			}
		}
	}
}
