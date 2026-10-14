using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000866 RID: 2150
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WaterloggedScriptorium : EventModel
	{
		// Token: 0x060069AE RID: 27054 RVA: 0x0027DB2B File Offset: 0x0027BD2B
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= 55);
		}

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x060069AF RID: 27055 RVA: 0x0027DB57 File Offset: 0x0027BD57
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new MaxHpVar(6m),
					new GoldVar(55),
					new GoldVar("PricklySpongeGold", 99),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x0027DB94 File Offset: 0x0027BD94
		protected unsafe override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			int num = 1;
			List<EventOption> list = new List<EventOption>(num);
			CollectionsMarshal.SetCount<EventOption>(list, num);
			Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
			int num2 = 0;
			*span[num2] = new EventOption(this, new Func<Task>(this.BloodyInk), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.BLOODY_INK", Array.Empty<IHoverTip>());
			List<EventOption> list2 = list;
			if (base.Owner.Gold >= base.DynamicVars.Gold.IntValue)
			{
				list2.Add(new EventOption(this, new Func<Task>(this.TentacleQuill), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.TENTACLE_QUILL", HoverTipFactory.FromEnchantment<Steady>(1)));
			}
			else
			{
				list2.Add(new EventOption(this, null, "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.TENTACLE_QUILL_LOCKED", Array.Empty<IHoverTip>()));
			}
			if (base.Owner.Gold >= base.DynamicVars["PricklySpongeGold"].IntValue)
			{
				list2.Add(new EventOption(this, new Func<Task>(this.PricklySponge), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.PRICKLY_SPONGE", HoverTipFactory.FromEnchantment<Steady>(1)));
			}
			else
			{
				list2.Add(new EventOption(this, null, "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.PRICKLY_SPONGE_LOCKED", Array.Empty<IHoverTip>()));
			}
			return list2;
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x0027DC98 File Offset: 0x0027BE98
		private async Task PricklySponge()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["PricklySpongeGold"].BaseValue, base.Owner, GoldLossType.Spent);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, base.DynamicVars.Cards.IntValue);
			Steady steady = ModelDb.Enchantment<Steady>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, steady, 1, cardSelectorPrefs);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Enchant<Steady>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.PRICKLY_SPONGE.description"));
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x0027DCDC File Offset: 0x0027BEDC
		private async Task TentacleQuill()
		{
			await PlayerCmd.LoseGold(base.DynamicVars.Gold.BaseValue, base.Owner, GoldLossType.Spent);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			CardModel cardModel = (await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Steady>(), 1, cardSelectorPrefs)).FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<Steady>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.TENTACLE_QUILL.description"));
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x0027DD20 File Offset: 0x0027BF20
		private async Task BloodyInk()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			base.SetEventFinished(base.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.BLOODY_INK.description"));
		}

		// Token: 0x040027E7 RID: 10215
		private const int _spawnGoldRequirement = 55;

		// Token: 0x040027E8 RID: 10216
		private const string _pricklySpongeGoldKey = "PricklySpongeGold";
	}
}
