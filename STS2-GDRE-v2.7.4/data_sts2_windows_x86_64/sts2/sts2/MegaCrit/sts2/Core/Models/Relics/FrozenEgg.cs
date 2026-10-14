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
	// Token: 0x0200054F RID: 1359
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FrozenEgg : RelicModel
	{
		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x060050F3 RID: 20723 RVA: 0x0023FF90 File Offset: 0x0023E190
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x0023FF93 File Offset: 0x0023E193
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x0023FF9B File Offset: 0x0023E19B
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
			EggRelicHelper.UpgradeValidCards(cardRewards, CardType.Power, this);
			return true;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x0023FFCB File Offset: 0x0023E1CB
		public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
		{
			if (player != base.Owner)
			{
				return;
			}
			EggRelicHelper.UpgradeValidCards(cards, CardType.Power, this);
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x0023FFE0 File Offset: 0x0023E1E0
		public override bool TryModifyCardBeingAddedToDeck(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			if (card.Owner != base.Owner)
			{
				return false;
			}
			if (card.Type != CardType.Power)
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
