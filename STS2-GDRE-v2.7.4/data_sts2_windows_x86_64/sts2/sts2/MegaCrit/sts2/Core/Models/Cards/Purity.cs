using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A88 RID: 2696
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Purity : CardModel
	{
		// Token: 0x060075DA RID: 30170 RVA: 0x00294817 File Offset: 0x00292A17
		public Purity()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FBB RID: 8123
		// (get) Token: 0x060075DB RID: 30171 RVA: 0x00294824 File Offset: 0x00292A24
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Retain,
					CardKeyword.Exhaust
				});
			}
		}

		// Token: 0x17001FBC RID: 8124
		// (get) Token: 0x060075DC RID: 30172 RVA: 0x00294839 File Offset: 0x00292A39
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x00294848 File Offset: 0x00292A48
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x00294893 File Offset: 0x00292A93
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(2m);
		}
	}
}
