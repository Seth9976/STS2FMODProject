using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x02000A19 RID: 2585
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JackOfAllTrades : CardModel
	{
		// Token: 0x06007386 RID: 29574 RVA: 0x0028F84B File Offset: 0x0028DA4B
		public JackOfAllTrades()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x06007387 RID: 29575 RVA: 0x0028F858 File Offset: 0x0028DA58
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x17001EC9 RID: 7881
		// (get) Token: 0x06007388 RID: 29576 RVA: 0x0028F86F File Offset: 0x0028DA6F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001ECA RID: 7882
		// (get) Token: 0x06007389 RID: 29577 RVA: 0x0028F877 File Offset: 0x0028DA77
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x0600738A RID: 29578 RVA: 0x0028F884 File Offset: 0x0028DA84
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(base.Owner, from c in ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where !(c is JackOfAllTrades)
				select c, base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
			foreach (CardModel cardModel in distinctForCombat.ToList<CardModel>())
			{
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}
	}
}
