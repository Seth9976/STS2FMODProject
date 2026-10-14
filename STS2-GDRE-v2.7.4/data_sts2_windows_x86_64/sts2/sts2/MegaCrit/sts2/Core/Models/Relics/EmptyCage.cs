using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200053C RID: 1340
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmptyCage : RelicModel
	{
		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005079 RID: 20601 RVA: 0x0023F365 File Offset: 0x0023D565
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x0023F368 File Offset: 0x0023D568
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x0023F36B File Offset: 0x0023D56B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0023F378 File Offset: 0x0023D578
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
		}
	}
}
