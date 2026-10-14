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
	// Token: 0x020009E1 RID: 2529
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glimmer : CardModel
	{
		// Token: 0x06007262 RID: 29282 RVA: 0x0028D387 File Offset: 0x0028B587
		public Glimmer()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E4C RID: 7756
		// (get) Token: 0x06007263 RID: 29283 RVA: 0x0028D394 File Offset: 0x0028B594
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new DynamicVar("PutBack", 1m)
				});
			}
		}

		// Token: 0x06007264 RID: 29284 RVA: 0x0028D3BC File Offset: 0x0028B5BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, base.DynamicVars["PutBack"].IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			CardModel[] array = enumerable.ToArray<CardModel>();
			if (array.Length != 0)
			{
				await CardPileCmd.Add(array, PileType.Draw, CardPilePosition.Top, null, false);
			}
		}

		// Token: 0x06007265 RID: 29285 RVA: 0x0028D407 File Offset: 0x0028B607
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x04002877 RID: 10359
		private const string _putBackKey = "PutBack";
	}
}
