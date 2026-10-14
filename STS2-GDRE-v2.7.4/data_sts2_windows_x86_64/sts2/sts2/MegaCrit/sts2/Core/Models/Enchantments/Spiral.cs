using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E2 RID: 2274
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Spiral : EnchantmentModel
	{
		// Token: 0x06006C96 RID: 27798 RVA: 0x002821B3 File Offset: 0x002803B3
		public override bool CanEnchant(CardModel c)
		{
			return base.CanEnchant(c) && c.Rarity == CardRarity.Basic && (c.Tags.Contains(CardTag.Strike) || c.Tags.Contains(CardTag.Defend));
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x06006C97 RID: 27799 RVA: 0x002821E5 File Offset: 0x002803E5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new IntVar("Times", 1m));
			}
		}

		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x06006C98 RID: 27800 RVA: 0x002821FB File Offset: 0x002803FB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, new DynamicVar[] { base.DynamicVars["Times"] }));
			}
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x00282222 File Offset: 0x00280422
		public override int EnchantPlayCount(int originalPlayCount)
		{
			return originalPlayCount + base.DynamicVars["Times"].IntValue;
		}

		// Token: 0x04002837 RID: 10295
		private const string _timesKey = "Times";
	}
}
