using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200083B RID: 2107
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FieldOfManSizedHoles : EventModel
	{
		// Token: 0x06006802 RID: 26626 RVA: 0x00275329 File Offset: 0x00273529
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => CardPile.Get(PileType.Deck, p).Cards.Any<CardModel>(new Func<CardModel, bool>(ModelDb.Enchantment<PerfectFit>().CanEnchant)));
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x06006803 RID: 26627 RVA: 0x00275358 File Offset: 0x00273558
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(75),
					new CardsVar(2),
					new StringVar("ResistCurse", ModelDb.Card<Normality>().Title),
					new StringVar("Enchantment", ModelDb.Enchantment<PerfectFit>().Title.GetFormattedText())
				});
			}
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x002753B8 File Offset: 0x002735B8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Resist), "FIELD_OF_MAN_SIZED_HOLES.pages.INITIAL.options.RESIST", HoverTipFactory.FromCardWithCardHoverTips<Normality>(false)),
				new EventOption(this, new Func<Task>(this.EnterYourHole), "FIELD_OF_MAN_SIZED_HOLES.pages.INITIAL.options.ENTER_YOUR_HOLE", HoverTipFactory.FromEnchantment<PerfectFit>(1))
			});
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00275410 File Offset: 0x00273610
		private async Task Resist()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			await CardPileCmd.AddCursesToDeck(new <>z__ReadOnlySingleElementList<CardModel>(ModelDb.Card<Normality>()), base.Owner);
			base.SetEventFinished(base.L10NLookup("FIELD_OF_MAN_SIZED_HOLES.pages.RESIST.description"));
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x00275454 File Offset: 0x00273654
		private async Task EnterYourHole()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<PerfectFit>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<PerfectFit>(cardModel, 1m);
				NCardEnchantVfx ncardEnchantVfx = NCardEnchantVfx.Create(cardModel);
				if (ncardEnchantVfx != null)
				{
					NRun instance = NRun.Instance;
					if (instance != null)
					{
						instance.GlobalUi.CardPreviewContainer.AddChildSafely(ncardEnchantVfx);
					}
				}
			}
			base.SetEventFinished(base.L10NLookup("FIELD_OF_MAN_SIZED_HOLES.pages.ENTER_YOUR_HOLE.description"));
		}
	}
}
