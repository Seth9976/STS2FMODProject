using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000938 RID: 2360
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BundleOfJoy : CardModel
	{
		// Token: 0x06006ED5 RID: 28373 RVA: 0x0028640D File Offset: 0x0028460D
		public BundleOfJoy()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x0028641A File Offset: 0x0028461A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x06006ED7 RID: 28375 RVA: 0x00286427 File Offset: 0x00284627
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006ED8 RID: 28376 RVA: 0x00286430 File Offset: 0x00284630
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(base.Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.RunState.CardMultiplayerConstraint), base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
			foreach (CardModel cardModel in distinctForCombat)
			{
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x00286473 File Offset: 0x00284673
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
