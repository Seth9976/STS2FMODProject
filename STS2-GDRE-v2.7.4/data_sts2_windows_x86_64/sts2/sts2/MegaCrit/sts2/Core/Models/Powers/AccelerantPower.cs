using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This power doesn't actually do anything on its own. Instead, the Poison power checks for this power and
	/// re-triggers itself if it's there.
	/// </summary>
	// Token: 0x02000626 RID: 1574
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AccelerantPower : PowerModel
	{
		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x0600568A RID: 22154 RVA: 0x0024B69A File Offset: 0x0024989A
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x0024B69D File Offset: 0x0024989D
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600568C RID: 22156 RVA: 0x0024B6A0 File Offset: 0x002498A0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}
	}
}
