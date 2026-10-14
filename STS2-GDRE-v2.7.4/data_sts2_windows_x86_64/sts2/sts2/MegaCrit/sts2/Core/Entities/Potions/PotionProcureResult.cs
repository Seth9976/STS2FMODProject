using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Potions
{
	// Token: 0x02000C78 RID: 3192
	public class PotionProcureResult
	{
		// Token: 0x04002C24 RID: 11300
		public bool success;

		// Token: 0x04002C25 RID: 11301
		[Nullable(1)]
		public PotionModel potion;

		// Token: 0x04002C26 RID: 11302
		public PotionProcureFailureReason failureReason;
	}
}
