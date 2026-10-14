using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This is just a marker power for <see cref="T:MegaCrit.Sts2.Core.Models.Powers.SurroundedPower" /> to check for.
	/// </summary>
	// Token: 0x02000632 RID: 1586
	public sealed class BackAttackRightPower : PowerModel
	{
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x060056CD RID: 22221 RVA: 0x0024BC4D File Offset: 0x00249E4D
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x060056CE RID: 22222 RVA: 0x0024BC50 File Offset: 0x00249E50
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}
	}
}
