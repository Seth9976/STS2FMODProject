using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000561 RID: 1377
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HeftyTablet : RelicModel
	{
		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x00240F0D File Offset: 0x0023F10D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x00240F10 File Offset: 0x0023F110
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x00240F13 File Offset: 0x0023F113
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x00240F20 File Offset: 0x0023F120
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Injury>(false);
			}
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x00240F28 File Offset: 0x0023F128
		public override async Task AfterObtained()
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoUpgradeRoll);
			List<CardModel> options = (from r in CardFactory.CreateForReward(base.Owner, base.DynamicVars.Cards.IntValue, cardCreationOptions)
				select r.Card).ToList<CardModel>();
			CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), options, base.Owner, true);
			CardModel chosenCard = cardModel;
			List<CardModel> list = new List<CardModel>(1) { base.Owner.RunState.CreateCard<Injury>(base.Owner) };
			if (chosenCard != null)
			{
				list.Insert(0, chosenCard);
			}
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.Add(list, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
			foreach (CardModel cardModel2 in options)
			{
				if (cardModel2 != chosenCard)
				{
					MapPointHistoryEntry currentMapPointHistoryEntry = base.Owner.RunState.CurrentMapPointHistoryEntry;
					if (currentMapPointHistoryEntry != null)
					{
						currentMapPointHistoryEntry.GetEntry(base.Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardModel2, false));
					}
				}
			}
		}
	}
}
