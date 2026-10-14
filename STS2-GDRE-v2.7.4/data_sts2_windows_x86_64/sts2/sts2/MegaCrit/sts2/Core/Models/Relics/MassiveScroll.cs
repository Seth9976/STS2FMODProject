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
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000583 RID: 1411
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MassiveScroll : RelicModel
	{
		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x0600525A RID: 21082 RVA: 0x00242B2B File Offset: 0x00240D2B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x00242B2E File Offset: 0x00240D2E
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.Count > 1;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x00242B40 File Offset: 0x00240D40
		public override async Task AfterObtained()
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlyArray<CardPoolModel>(new CardPoolModel[]
			{
				base.Owner.Character.CardPool,
				ModelDb.CardPool<ColorlessCardPool>()
			}), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, (CardModel c) => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly);
			List<CardModel> options = (from r in CardFactory.CreateForReward(base.Owner, 3, cardCreationOptions)
				select r.Card).ToList<CardModel>();
			CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), options, base.Owner, true);
			CardModel chosenCard = cardModel;
			if (chosenCard != null)
			{
				CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(chosenCard, PileType.Deck, CardPilePosition.Bottom, null, false), 1.2f, CardPreviewStyle.HorizontalLayout);
			}
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
