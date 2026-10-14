using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This is just a marker power for <see cref="T:MegaCrit.Sts2.Core.Models.Powers.SurroundedPower" /> to check for.
	/// </summary>
	// Token: 0x02000631 RID: 1585
	public sealed class BackAttackLeftPower : PowerModel
	{
		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060056CA RID: 22218 RVA: 0x0024BC3F File Offset: 0x00249E3F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x060056CB RID: 22219 RVA: 0x0024BC42 File Offset: 0x00249E42
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}
	}
}
