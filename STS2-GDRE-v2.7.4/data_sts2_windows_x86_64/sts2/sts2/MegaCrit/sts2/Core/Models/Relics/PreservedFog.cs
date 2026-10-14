using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C2 RID: 1474
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PreservedFog : RelicModel
	{
		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06005412 RID: 21522 RVA: 0x00245EC7 File Offset: 0x002440C7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06005413 RID: 21523 RVA: 0x00245ECA File Offset: 0x002440CA
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Folly>(false);
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06005414 RID: 21524 RVA: 0x00245ED2 File Offset: 0x002440D2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x00245EE0 File Offset: 0x002440E0
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				await CardPileCmd.RemoveFromDeck(cardModel, true);
			}
			IEnumerator<CardModel> enumerator = null;
			await CardPileCmd.AddCurseToDeck<Folly>(base.Owner);
		}
	}
}
