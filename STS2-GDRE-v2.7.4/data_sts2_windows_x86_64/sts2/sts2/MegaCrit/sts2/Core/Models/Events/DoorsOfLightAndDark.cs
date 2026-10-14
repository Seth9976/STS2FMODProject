using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000837 RID: 2103
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DoorsOfLightAndDark : EventModel
	{
		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x060067D2 RID: 26578 RVA: 0x002745AC File Offset: 0x002727AC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x002745BC File Offset: 0x002727BC
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Light), "DOORS_OF_LIGHT_AND_DARK.pages.INITIAL.options.LIGHT", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Dark), "DOORS_OF_LIGHT_AND_DARK.pages.INITIAL.options.DARK", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x00274614 File Offset: 0x00272814
		private Task Light()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Rng)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			base.SetEventFinished(base.L10NLookup("DOORS_OF_LIGHT_AND_DARK.pages.LIGHT.description"));
			return Task.CompletedTask;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x002746D0 File Offset: 0x002728D0
		private async Task Dark()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			base.SetEventFinished(base.L10NLookup("DOORS_OF_LIGHT_AND_DARK.pages.DARK.description"));
		}
	}
}
