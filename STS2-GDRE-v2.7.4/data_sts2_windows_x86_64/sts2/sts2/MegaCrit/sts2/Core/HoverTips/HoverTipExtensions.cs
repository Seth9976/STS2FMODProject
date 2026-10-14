using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.HoverTips
{
	// Token: 0x02000BFC RID: 3068
	public static class HoverTipExtensions
	{
		/// <summary>
		/// Tries to add the tip to the list if the list doesn't already contain a tip of the same type that is equal or
		/// smarter than it.
		/// </summary>
		/// <param name="tips">List we are trying to add to.</param>
		/// <param name="tip">Tip we are trying to add.</param>
		// Token: 0x06007EDB RID: 32475 RVA: 0x002AF0E0 File Offset: 0x002AD2E0
		[NullableContext(1)]
		public static void MegaTryAddingTip(this ICollection<IHoverTip> tips, IHoverTip tip)
		{
			IHoverTip hoverTip = tips.FirstOrDefault<IHoverTip>((IHoverTip t) => t.Id == tip.Id);
			if (hoverTip != null && !hoverTip.IsInstanced)
			{
				if (!hoverTip.IsSmart && tip.IsSmart)
				{
					tips.Remove(hoverTip);
					tips.Add(tip);
					return;
				}
			}
			else
			{
				tips.Add(tip);
			}
		}
	}
}
