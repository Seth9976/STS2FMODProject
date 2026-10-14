using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
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
	// Token: 0x02000851 RID: 2129
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpiralingWhirlpool : EventModel
	{
		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x060068D1 RID: 26833 RVA: 0x002794E0 File Offset: 0x002776E0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(0m));
			}
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x002794F4 File Offset: 0x002776F4
		public override void CalculateVars()
		{
			base.DynamicVars.Heal.BaseValue = ((base.Owner != null) ? (base.Owner.Creature.MaxHp * 0.33m) : 0m);
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x00279545 File Offset: 0x00277745
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Deck.Cards.Any<CardModel>(new Func<CardModel, bool>(ModelDb.Enchantment<Spiral>().CanEnchant)));
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x00279574 File Offset: 0x00277774
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.ObserveTheSpiral), "SPIRALING_WHIRLPOOL.pages.INITIAL.options.OBSERVE", HoverTipFactory.FromEnchantment<Spiral>(1)),
				new EventOption(this, new Func<Task>(this.Drink), "SPIRALING_WHIRLPOOL.pages.INITIAL.options.DRINK", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x002795CC File Offset: 0x002777CC
		private async Task ObserveTheSpiral()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Spiral>(), 1, (CardModel c) => ModelDb.Enchantment<Spiral>().CanEnchant(c), new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<Spiral>(cardModel, 1m);
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
			base.SetEventFinished(base.L10NLookup("SPIRALING_WHIRLPOOL.pages.OBSERVE.description"));
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x00279610 File Offset: 0x00277810
		private async Task Drink()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue, true);
			base.SetEventFinished(base.L10NLookup("SPIRALING_WHIRLPOOL.pages.DRINK.description"));
		}
	}
}
