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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200083C RID: 2108
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GraveOfTheForgotten : EventModel
	{
		// Token: 0x06006808 RID: 26632 RVA: 0x0027549F File Offset: 0x0027369F
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>(new Func<Player, bool>(this.HasEnchantableCards));
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x002754B8 File Offset: 0x002736B8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			EventOption eventOption;
			if (this.HasEnchantableCards(base.Owner))
			{
				eventOption = new EventOption(this, new Func<Task>(this.Confront), "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.CONFRONT", HoverTipFactory.FromEnchantment<SoulsPower>(1).Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<Decay>(false)));
			}
			else
			{
				eventOption = new EventOption(this, null, "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.CONFRONT_LOCKED", Array.Empty<IHoverTip>());
			}
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				eventOption,
				new EventOption(this, new Func<Task>(this.Accept), "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.ACCEPT", HoverTipFactory.FromRelic<ForgottenSoul>())
			});
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x0600680A RID: 26634 RVA: 0x00275540 File Offset: 0x00273740
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Relic", ModelDb.Relic<ForgottenSoul>().Title.GetFormattedText()),
					new StringVar("Enchantment", ModelDb.Enchantment<SoulsPower>().Title.GetFormattedText()),
					new StringVar("Curse", ModelDb.Card<Decay>().Title)
				});
			}
		}

		// Token: 0x0600680B RID: 26635 RVA: 0x002755A8 File Offset: 0x002737A8
		private async Task Confront()
		{
			await CardPileCmd.AddCurseToDeck<Decay>(base.Owner);
			CardModel cardModel = (await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<SoulsPower>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<SoulsPower>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("GRAVE_OF_THE_FORGOTTEN.pages.CONFRONT.description"));
		}

		// Token: 0x0600680C RID: 26636 RVA: 0x002755EC File Offset: 0x002737EC
		private async Task Accept()
		{
			await RelicCmd.Obtain<ForgottenSoul>(base.Owner);
			base.SetEventFinished(base.L10NLookup("GRAVE_OF_THE_FORGOTTEN.pages.ACCEPT.description"));
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x00275630 File Offset: 0x00273830
		private bool HasEnchantableCards(Player player)
		{
			IReadOnlyList<CardModel> cards = PileType.Deck.GetPile(player).Cards;
			return cards.Any<CardModel>((CardModel c) => ModelDb.Enchantment<SoulsPower>().CanEnchant(c));
		}

		// Token: 0x04002784 RID: 10116
		private const string _enchantmentKey = "Enchantment";

		// Token: 0x04002785 RID: 10117
		private const string _relicKey = "Relic";

		// Token: 0x04002786 RID: 10118
		private const string _curseKey = "Curse";
	}
}
