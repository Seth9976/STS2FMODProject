using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000611 RID: 1553
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VitruvianMinion : RelicModel
	{
		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x0600561A RID: 22042 RVA: 0x00249D7F File Offset: 0x00247F7F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x0600561B RID: 22043 RVA: 0x00249D82 File Offset: 0x00247F82
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00249D94 File Offset: 0x00247F94
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (cardSource == null)
			{
				return 1m;
			}
			if (cardSource.Owner != base.Owner)
			{
				return 1m;
			}
			if (!cardSource.Tags.Contains(CardTag.Minion))
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x00249DD0 File Offset: 0x00247FD0
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (cardSource == null)
			{
				return 1m;
			}
			if (cardSource.Owner != base.Owner)
			{
				return 1m;
			}
			if (!cardSource.Tags.Contains(CardTag.Minion))
			{
				return 1m;
			}
			return 2m;
		}
	}
}
