using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000590 RID: 1424
	public sealed class MummifiedHand : RelicModel
	{
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x060052A6 RID: 21158 RVA: 0x002433A3 File Offset: 0x002415A3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x002433A8 File Offset: 0x002415A8
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Power)
			{
				return Task.CompletedTask;
			}
			Rng combatCardSelection = base.Owner.RunState.Rng.CombatCardSelection;
			IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards;
			List<CardModel> list = cards.Where<CardModel>((CardModel c) => c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || c.BaseStarCost > 0).ToList<CardModel>();
			CardModel cardModel = combatCardSelection.NextItem<CardModel>(list.Where<CardModel>((CardModel c) => c.CostsEnergyOrStars(true)));
			if (cardModel == null)
			{
				cardModel = combatCardSelection.NextItem<CardModel>(cards.Where<CardModel>((CardModel c) => c.CostsEnergyOrStars(true)));
			}
			if (cardModel == null)
			{
				cardModel = combatCardSelection.NextItem<CardModel>(list);
			}
			if (cardModel == null)
			{
				cardModel = combatCardSelection.NextItem<CardModel>(cards);
			}
			if (cardModel != null)
			{
				cardModel.SetToFreeThisTurn();
			}
			return Task.CompletedTask;
		}
	}
}
