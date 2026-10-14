using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CE RID: 1486
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RingingTriangle : RelicModel
	{
		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06005466 RID: 21606 RVA: 0x00246827 File Offset: 0x00244A27
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06005467 RID: 21607 RVA: 0x0024682A File Offset: 0x00244A2A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x00246837 File Offset: 0x00244A37
		public override bool ShouldFlush(Player player)
		{
			return player != base.Owner || base.Owner.PlayerCombatState.TurnNumber > 1;
		}
	}
}
