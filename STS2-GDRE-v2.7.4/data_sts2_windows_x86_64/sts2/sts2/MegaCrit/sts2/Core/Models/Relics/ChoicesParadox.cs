using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200051F RID: 1311
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChoicesParadox : RelicModel
	{
		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06004FEB RID: 20459 RVA: 0x0023E457 File Offset: 0x0023C657
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x0023E45A File Offset: 0x0023C65A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(5));
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06004FED RID: 20461 RVA: 0x0023E467 File Offset: 0x0023C667
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x0023E474 File Offset: 0x0023C674
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
					if (list.Count == 0)
					{
						string text = "ChoicesParadox generated no cards for selection. Returning early to prevent softlock.";
						Log.Error(text, 2);
						SentryService.CaptureException(new SoftlockException(text));
					}
					else
					{
						foreach (CardModel cardModel in list)
						{
							CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Retain });
						}
						IEnumerable<CardModel> enumerable = await CardSelectCmd.FromSimpleGrid(choiceContext, list, base.Owner, new CardSelectorPrefs(RelicModel.L10NLookup("CHOICES_PARADOX.selectionScreenPrompt"), 1));
						IEnumerable<CardModel> enumerable2 = enumerable;
						foreach (CardModel cardModel2 in enumerable2)
						{
							await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, base.Owner, CardPilePosition.Bottom);
						}
						IEnumerator<CardModel> enumerator2 = null;
					}
				}
			}
		}
	}
}
