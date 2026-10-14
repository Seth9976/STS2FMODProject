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
	// Token: 0x02000A7E RID: 2686
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Prepared : CardModel
	{
		// Token: 0x060075A9 RID: 30121 RVA: 0x00294137 File Offset: 0x00292337
		public Prepared()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001FA7 RID: 8103
		// (get) Token: 0x060075AA RID: 30122 RVA: 0x00294144 File Offset: 0x00292344
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x00294154 File Offset: 0x00292354
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int cardCount = base.DynamicVars.Cards.IntValue;
			await CardPileCmd.Draw(choiceContext, cardCount, base.Owner, false);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, cardCount), null, this);
			await CardCmd.Discard(choiceContext, enumerable);
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x0029419F File Offset: 0x0029239F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
