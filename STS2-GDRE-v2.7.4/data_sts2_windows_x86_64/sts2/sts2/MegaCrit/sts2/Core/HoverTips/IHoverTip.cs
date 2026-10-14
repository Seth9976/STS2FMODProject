using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.HoverTips
{
	// Token: 0x02000BFB RID: 3067
	[NullableContext(1)]
	public interface IHoverTip
	{
		// Token: 0x170022FF RID: 8959
		// (get) Token: 0x06007ED4 RID: 32468
		string Id { get; }

		// Token: 0x17002300 RID: 8960
		// (get) Token: 0x06007ED5 RID: 32469
		bool IsSmart { get; }

		// Token: 0x17002301 RID: 8961
		// (get) Token: 0x06007ED6 RID: 32470
		bool IsDebuff { get; }

		// Token: 0x17002302 RID: 8962
		// (get) Token: 0x06007ED7 RID: 32471
		bool IsInstanced { get; }

		/// <summary>
		/// The canonical model that this HoverTip is for.
		/// Null for HoverTips that are unrelated to a model (static, card keyword, etc.).
		/// </summary>
		// Token: 0x17002303 RID: 8963
		// (get) Token: 0x06007ED8 RID: 32472
		[Nullable(2)]
		AbstractModel CanonicalModel
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x002AEF94 File Offset: 0x002AD194
		public static IEnumerable<IHoverTip> RemoveDupes(IEnumerable<IHoverTip> tips)
		{
			List<IHoverTip> list = new List<IHoverTip>();
			using (IEnumerator<IHoverTip> enumerator = tips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IHoverTip hoverTip = enumerator.Current;
					if (string.IsNullOrEmpty(hoverTip.Id))
					{
						list.Add(hoverTip);
					}
					else
					{
						IHoverTip hoverTip2 = list.FirstOrDefault<IHoverTip>((IHoverTip tip) => tip.Id == hoverTip.Id && !tip.IsInstanced);
						if (hoverTip2 == null)
						{
							list.Add(hoverTip);
						}
						else if (!hoverTip2.IsSmart || hoverTip.IsSmart)
						{
							list.Remove(hoverTip2);
							list.Add(hoverTip);
						}
					}
				}
			}
			if (list.Any<IHoverTip>((IHoverTip tip) => tip.Id == IHoverTip._summonStaticId))
			{
				list.RemoveAll((IHoverTip tip) => tip.Id == IHoverTip._summonDynamicId);
			}
			return list;
		}

		// Token: 0x04002A60 RID: 10848
		private static readonly string _summonStaticId = HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>()).Id;

		// Token: 0x04002A61 RID: 10849
		private static readonly string _summonDynamicId = HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[]
		{
			new SummonVar(0m)
		}).Id;
	}
}
