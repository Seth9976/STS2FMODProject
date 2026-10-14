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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200086A RID: 2154
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WoodCarvings : EventModel
	{
		// Token: 0x060069CD RID: 27085 RVA: 0x0027E48B File Offset: 0x0027C68B
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => CardPile.Get(PileType.Deck, p).Cards.Any<CardModel>((CardModel c) => c != null && c.Rarity == CardRarity.Basic && c.IsRemovable));
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x0027E4B8 File Offset: 0x0027C6B8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			IReadOnlyList<CardModel> cards = PileType.Deck.GetPile(base.Owner).Cards;
			EventOption eventOption;
			if (cards.Any<CardModel>((CardModel c) => ModelDb.Enchantment<Slither>().CanEnchant(c)))
			{
				eventOption = new EventOption(this, new Func<Task>(this.Snake), "WOOD_CARVINGS.pages.INITIAL.options.SNAKE", HoverTipFactory.FromEnchantment<Slither>(1));
			}
			else
			{
				eventOption = new EventOption(this, null, "WOOD_CARVINGS.pages.INITIAL.options.SNAKE_LOCKED", Array.Empty<IHoverTip>());
			}
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Bird), "WOOD_CARVINGS.pages.INITIAL.options.BIRD", HoverTipFactory.FromCardWithCardHoverTips<Peck>(false)),
				eventOption,
				new EventOption(this, new Func<Task>(this.Torus), "WOOD_CARVINGS.pages.INITIAL.options.TORUS", HoverTipFactory.FromCardWithCardHoverTips<ToricToughness>(false))
			});
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x060069CF RID: 27087 RVA: 0x0027E580 File Offset: 0x0027C780
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("BirdCard", ModelDb.Card<Peck>().Title),
					new StringVar("SnakeEnchantment", ModelDb.Enchantment<Slither>().Title.GetFormattedText()),
					new StringVar("ToricCard", ModelDb.Card<ToricToughness>().Title)
				});
			}
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x0027E5E4 File Offset: 0x0027C7E4
		private async Task Bird()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckGeneric(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), (CardModel c) => c.IsTransformable && c.Rarity == CardRarity.Basic, null);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.TransformTo<Peck>(cardModel, CardPreviewStyle.EventLayout);
			}
			base.SetEventFinished(base.L10NLookup("WOOD_CARVINGS.pages.BIRD.description"));
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x0027E628 File Offset: 0x0027C828
		private async Task Snake()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Slither>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<Slither>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("WOOD_CARVINGS.pages.SNAKE.description"));
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x0027E66C File Offset: 0x0027C86C
		private async Task Torus()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckGeneric(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), (CardModel c) => c != null && c.IsTransformable && c.Rarity == CardRarity.Basic, null);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.TransformTo<ToricToughness>(cardModel, CardPreviewStyle.EventLayout);
			}
			base.SetEventFinished(base.L10NLookup("WOOD_CARVINGS.pages.TORUS.description"));
		}

		// Token: 0x040027F7 RID: 10231
		private const string _birdCardKey = "BirdCard";

		// Token: 0x040027F8 RID: 10232
		private const string _snakeEnchantmentKey = "SnakeEnchantment";

		// Token: 0x040027F9 RID: 10233
		private const string _toricCardKey = "ToricCard";
	}
}
