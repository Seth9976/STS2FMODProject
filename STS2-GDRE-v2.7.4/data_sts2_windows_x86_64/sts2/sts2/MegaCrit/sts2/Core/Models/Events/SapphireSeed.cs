using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200084E RID: 2126
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SapphireSeed : EventModel
	{
		// Token: 0x060068AF RID: 26799 RVA: 0x00278C84 File Offset: 0x00276E84
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Eat), "SAPPHIRE_SEED.pages.INITIAL.options.EAT", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Plant), "SAPPHIRE_SEED.pages.INITIAL.options.PLANT", HoverTipFactory.FromEnchantment<Sown>(1))
			});
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x060068B0 RID: 26800 RVA: 0x00278CDB File Offset: 0x00276EDB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Enchantment", ModelDb.Enchantment<Sown>().Title.GetFormattedText()),
					new HealVar(9m)
				});
			}
		}

		// Token: 0x060068B1 RID: 26801 RVA: 0x00278D14 File Offset: 0x00276F14
		private async Task Eat()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue, true);
			CardModel cardModel = (await CardSelectCmd.FromDeckForUpgrade(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1))).FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			base.SetEventFinished(base.L10NLookup("SAPPHIRE_SEED.pages.EAT.description"));
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x00278D58 File Offset: 0x00276F58
		private async Task Plant()
		{
			EnchantmentModel sown = ModelDb.Enchantment<Sown>();
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => sown.CanEnchant(c)).ToList<CardModel>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(list, sown, 1, cardSelectorPrefs);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<Sown>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("SAPPHIRE_SEED.pages.PLANT.description"));
		}

		// Token: 0x040027A9 RID: 10153
		private const string _enchantmentKey = "Enchantment";
	}
}
