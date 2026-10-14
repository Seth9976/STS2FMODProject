using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CBD RID: 3261
	public static class CardUpgradePreviewTypeExtensions
	{
		// Token: 0x060084DD RID: 34013 RVA: 0x002CB01F File Offset: 0x002C921F
		public static bool IsPreview(this CardUpgradePreviewType previewType)
		{
			return previewType > CardUpgradePreviewType.None;
		}
	}
}
