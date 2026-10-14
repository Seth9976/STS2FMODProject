using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This power doesn't actually do anything on its own. Instead, Sovereign blade cards checks for this power and
	/// and changes its behavior based off of that
	/// </summary>
	// Token: 0x020006E3 RID: 1763
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SeekingEdgePower : PowerModel
	{
		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x002532DC File Offset: 0x002514DC
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06005AD2 RID: 23250 RVA: 0x002532DF File Offset: 0x002514DF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x002532E2 File Offset: 0x002514E2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}
	}
}
