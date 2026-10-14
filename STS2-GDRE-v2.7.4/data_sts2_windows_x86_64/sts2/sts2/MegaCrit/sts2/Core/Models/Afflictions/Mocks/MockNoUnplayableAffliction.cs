using System;

namespace MegaCrit.Sts2.Core.Models.Afflictions.Mocks
{
	// Token: 0x02000B7D RID: 2941
	public sealed class MockNoUnplayableAffliction : AfflictionModel
	{
		// Token: 0x170021FA RID: 8698
		// (get) Token: 0x06007B15 RID: 31509 RVA: 0x002A0332 File Offset: 0x0029E532
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021FB RID: 8699
		// (get) Token: 0x06007B16 RID: 31510 RVA: 0x002A0335 File Offset: 0x0029E535
		public override bool CanAfflictUnplayableCards
		{
			get
			{
				return false;
			}
		}
	}
}
