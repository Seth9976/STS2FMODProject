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
	// Token: 0x0200058E RID: 1422
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MoltenEgg : RelicModel
	{
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x0600529D RID: 21149 RVA: 0x00243290 File Offset: 0x00241490
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x00243293 File Offset: 0x00241493
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x0024329B File Offset: 0x0024149B
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
			EggRelicHelper.UpgradeValidCards(cardRewards, CardType.Attack, this);
			return true;
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x002432CB File Offset: 0x002414CB
		public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
		{
			if (player != base.Owner)
			{
				return;
			}
			EggRelicHelper.UpgradeValidCards(cards, CardType.Attack, this);
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x002432E0 File Offset: 0x002414E0
		public override bool TryModifyCardBeingAddedToDeck(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			if (card.Owner != base.Owner)
			{
				return false;
			}
			if (card.Type != CardType.Attack)
			{
				return false;
			}
			if (!card.IsUpgradable)
			{
				return false;
			}
			if (card.CurrentUpgradeLevel >= 1)
			{
				return false;
			}
			newCard = base.Owner.RunState.CloneCard(card);
			CardCmd.Upgrade(newCard, CardPreviewStyle.None);
			return true;
		}
	}
}
