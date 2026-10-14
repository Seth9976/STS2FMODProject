using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000601 RID: 1537
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToxicEgg : RelicModel
	{
		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x0600559F RID: 21919 RVA: 0x00248F73 File Offset: 0x00247173
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x00248F76 File Offset: 0x00247176
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x00248F7E File Offset: 0x0024717E
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (options.Flags.HasFlag(CardCreationFlags.NoHookUpgrades))
			{
				return false;
			}
			EggRelicHelper.UpgradeValidCards(cardRewards, CardType.Skill, this);
			return true;
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x00248FAE File Offset: 0x002471AE
		public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
		{
			if (player != base.Owner)
			{
				return;
			}
			EggRelicHelper.UpgradeValidCards(cards, CardType.Skill, this);
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x00248FC4 File Offset: 0x002471C4
		public override bool TryModifyCardBeingAddedToDeck(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			if (card.Owner != base.Owner)
			{
				return false;
			}
			if (card.Type != CardType.Skill)
			{
				return false;
			}
			if (!card.IsUpgradable)
			{
				return false;
			}
			newCard = base.Owner.RunState.CloneCard(card);
			CardCmd.Upgrade(newCard, CardPreviewStyle.None);
			return true;
		}
	}
}
