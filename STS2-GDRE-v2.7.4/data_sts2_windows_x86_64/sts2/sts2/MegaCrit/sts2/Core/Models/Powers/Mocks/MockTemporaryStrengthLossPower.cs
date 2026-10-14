using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards.Mocks;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000740 RID: 1856
	[NullableContext(1)]
	[Nullable(0)]
	public class MockTemporaryStrengthLossPower : TemporaryStrengthPower
	{
		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x06005D22 RID: 23842 RVA: 0x0025761A File Offset: 0x0025581A
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x06005D23 RID: 23843 RVA: 0x0025761D File Offset: 0x0025581D
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<MockSkillCard>();
			}
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x06005D24 RID: 23844 RVA: 0x00257624 File Offset: 0x00255824
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
