using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000618 RID: 1560
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WingCharm : RelicModel
	{
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x0024A267 File Offset: 0x00248467
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x0600563E RID: 22078 RVA: 0x0024A26A File Offset: 0x0024846A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("SwiftAmount", 1m));
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x0600563F RID: 22079 RVA: 0x0024A280 File Offset: 0x00248480
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Swift>(base.DynamicVars["SwiftAmount"].IntValue);
			}
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x0024A29C File Offset: 0x0024849C
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			Swift canonicalSwift = ModelDb.Enchantment<Swift>();
			List<CardCreationResult> list = cardRewards.Where<CardCreationResult>((CardCreationResult r) => canonicalSwift.CanEnchant(r.Card)).ToList<CardCreationResult>();
			if (list.Count == 0)
			{
				return false;
			}
			CardCreationResult cardCreationResult = base.Owner.RunState.Rng.Niche.NextItem<CardCreationResult>(list);
			if (cardCreationResult == null)
			{
				return false;
			}
			CardModel cardModel = base.Owner.RunState.CloneCard(cardCreationResult.Card);
			CardCmd.Enchant<Swift>(cardModel, base.DynamicVars["SwiftAmount"].BaseValue);
			cardCreationResult.ModifyCard(cardModel, this);
			return true;
		}

		// Token: 0x040024E5 RID: 9445
		private const string _swiftAmountKey = "SwiftAmount";
	}
}
