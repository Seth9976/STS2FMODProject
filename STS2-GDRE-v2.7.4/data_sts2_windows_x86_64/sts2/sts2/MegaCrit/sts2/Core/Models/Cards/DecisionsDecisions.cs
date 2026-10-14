using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000980 RID: 2432
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DecisionsDecisions : CardModel
	{
		// Token: 0x0600703F RID: 28735 RVA: 0x002890E7 File Offset: 0x002872E7
		public DecisionsDecisions()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D57 RID: 7511
		// (get) Token: 0x06007040 RID: 28736 RVA: 0x002890F4 File Offset: 0x002872F4
		public override int CanonicalStarCost
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001D58 RID: 7512
		// (get) Token: 0x06007041 RID: 28737 RVA: 0x002890F7 File Offset: 0x002872F7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new RepeatVar(3)
				});
			}
		}

		// Token: 0x17001D59 RID: 7513
		// (get) Token: 0x06007042 RID: 28738 RVA: 0x00289116 File Offset: 0x00287316
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x00289120 File Offset: 0x00287320
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1)
			{
				PretendCardsCanBePlayed = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, (CardModel c) => c.Type == CardType.Skill && !c.Keywords.Contains(CardKeyword.Unplayable), this);
			CardModel card = enumerable.FirstOrDefault<CardModel>();
			if (card != null)
			{
				for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
				{
					await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.Default, false, false);
				}
			}
		}

		// Token: 0x06007044 RID: 28740 RVA: 0x0028916B File Offset: 0x0028736B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(2m);
		}
	}
}
