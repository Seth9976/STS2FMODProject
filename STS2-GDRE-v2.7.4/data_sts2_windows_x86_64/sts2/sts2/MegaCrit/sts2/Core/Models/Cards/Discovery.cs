using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000992 RID: 2450
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Discovery : CardModel
	{
		// Token: 0x060070A6 RID: 28838 RVA: 0x00289C62 File Offset: 0x00287E62
		public Discovery()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D89 RID: 7561
		// (get) Token: 0x060070A7 RID: 28839 RVA: 0x00289C6F File Offset: 0x00287E6F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x00289C78 File Offset: 0x00287E78
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardModel cardModel2;
			if (this._mockSelectedCard == null)
			{
				List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
				CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, base.Owner, true);
				cardModel2 = cardModel;
			}
			else
			{
				cardModel2 = this._mockSelectedCard;
			}
			if (cardModel2 != null)
			{
				cardModel2.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x00289CC3 File Offset: 0x00287EC3
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}

		/// <summary>
		/// ONLY USE THIS IN TESTS!
		/// </summary>
		// Token: 0x060070AA RID: 28842 RVA: 0x00289CCC File Offset: 0x00287ECC
		public void MockSelectedCard(CardModel card)
		{
			base.AssertMutable();
			this._mockSelectedCard = card;
		}

		// Token: 0x0400285D RID: 10333
		[Nullable(2)]
		private CardModel _mockSelectedCard;
	}
}
