using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000856 RID: 2134
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Symbiote : EventModel
	{
		// Token: 0x060068F3 RID: 26867 RVA: 0x00279D5B File Offset: 0x00277F5B
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex > 0;
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x00279D68 File Offset: 0x00277F68
		protected unsafe override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			CardPile pile = PileType.Deck.GetPile(base.Owner);
			IEnumerable<CardModel> cards = pile.Cards;
			Func<CardModel, bool> func;
			if ((func = Symbiote.<>O.<0>__CanEnchant) == null)
			{
				func = (Symbiote.<>O.<0>__CanEnchant = new Func<CardModel, bool>(Symbiote.CanEnchant));
			}
			EventOption eventOption;
			if (cards.Any<CardModel>(func))
			{
				eventOption = new EventOption(this, new Func<Task>(this.Approach), "SYMBIOTE.pages.INITIAL.options.APPROACH", HoverTipFactory.FromEnchantment<Corrupted>(1));
			}
			else
			{
				eventOption = new EventOption(this, null, "SYMBIOTE.pages.INITIAL.options.APPROACH_LOCKED", Array.Empty<IHoverTip>());
			}
			EventOption eventOption2 = new EventOption(this, new Func<Task>(this.KillWithFire), "SYMBIOTE.pages.INITIAL.options.KILL_WITH_FIRE", new IHoverTip[] { HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()) });
			int num = 2;
			List<EventOption> list = new List<EventOption>(num);
			CollectionsMarshal.SetCount<EventOption>(list, num);
			Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
			int num2 = 0;
			*span[num2] = eventOption;
			num2++;
			*span[num2] = eventOption2;
			return list;
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x060068F5 RID: 26869 RVA: 0x00279E43 File Offset: 0x00278043
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Enchantment", ModelDb.Enchantment<Corrupted>().Title.GetFormattedText()),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x00279E78 File Offset: 0x00278078
		private async Task Approach()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Corrupted>(), 1, cardSelectorPrefs);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<Corrupted>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("SYMBIOTE.pages.APPROACH.description"));
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x00279EBC File Offset: 0x002780BC
		private async Task KillWithFire()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			base.SetEventFinished(base.L10NLookup("SYMBIOTE.pages.KILL_WITH_FIRE.description"));
		}

		// Token: 0x060068F8 RID: 26872 RVA: 0x00279EFF File Offset: 0x002780FF
		private static bool CanEnchant(CardModel card)
		{
			return ModelDb.Enchantment<Corrupted>().CanEnchant(card);
		}

		// Token: 0x040027C3 RID: 10179
		private const string _enchantmentKey = "Enchantment";

		// Token: 0x02001F62 RID: 8034
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400802C RID: 32812
			[Nullable(0)]
			public static Func<CardModel, bool> <0>__CanEnchant;
		}
	}
}
