using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000741 RID: 1857
	public sealed class MockUnhittablePower : PowerModel
	{
		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x06005D26 RID: 23846 RVA: 0x0025762F File Offset: 0x0025582F
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x06005D27 RID: 23847 RVA: 0x00257632 File Offset: 0x00255832
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x06005D28 RID: 23848 RVA: 0x00257635 File Offset: 0x00255835
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x00257638 File Offset: 0x00255838
		[NullableContext(1)]
		public override bool ShouldAllowHitting(Creature creature)
		{
			return creature != base.Owner;
		}
	}
}
