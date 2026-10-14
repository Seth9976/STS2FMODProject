using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000849 RID: 2121
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RanwidTheElder : EventModel
	{
		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600687F RID: 26751 RVA: 0x00277F4B File Offset: 0x0027614B
		private LocString PotionChoiceTitle
		{
			get
			{
				return new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.POTION.title");
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x06006880 RID: 26752 RVA: 0x00277F5C File Offset: 0x0027615C
		private LocString PotionChoiceDescription
		{
			get
			{
				return new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.POTION.description");
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x06006881 RID: 26753 RVA: 0x00277F6D File Offset: 0x0027616D
		private LocString RelicChoiceTitle
		{
			get
			{
				return new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.RELIC.title");
			}
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x06006882 RID: 26754 RVA: 0x00277F7E File Offset: 0x0027617E
		private LocString RelicChoiceDescription
		{
			get
			{
				return new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.RELIC.description");
			}
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x00277F8F File Offset: 0x0027618F
		protected override Task BeforeEventStarted(bool isPreFinished)
		{
			base.Owner.CanUseOrRemovePotions = false;
			return Task.CompletedTask;
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x00277FA2 File Offset: 0x002761A2
		protected override void OnEventFinished()
		{
			base.Owner.CanUseOrRemovePotions = true;
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x00277FB0 File Offset: 0x002761B0
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex == 0)
			{
				return false;
			}
			if (runState.Players.Any<Player>((Player p) => !this.GetValidRelics(p).Any<RelicModel>()))
			{
				return false;
			}
			if (runState.Players.Any<Player>((Player p) => p.Gold < 100))
			{
				return false;
			}
			return !runState.Players.Any<Player>((Player p) => !p.Potions.Any<PotionModel>());
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x0027803F File Offset: 0x0027623F
		private IEnumerable<RelicModel> GetValidRelics(Player player)
		{
			return player.Relics.Where<RelicModel>((RelicModel r) => r.IsTradable);
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x06006887 RID: 26759 RVA: 0x0027806B File Offset: 0x0027626B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(100),
					new StringVar("Potion", "Potion"),
					new StringVar("Relic", "Relic")
				});
			}
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x002780A8 File Offset: 0x002762A8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			RanwidTheElder.<>c__DisplayClass18_0 CS$<>8__locals1 = new RanwidTheElder.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			List<EventOption> list = new List<EventOption>();
			CS$<>8__locals1.potion = base.Rng.NextItem<PotionModel>(base.Owner.Potions);
			if (CS$<>8__locals1.potion != null)
			{
				((StringVar)base.DynamicVars["Potion"]).StringValue = CS$<>8__locals1.potion.Title.GetFormattedText();
				list.Add(new EventOption(this, delegate
				{
					RanwidTheElder.<>c__DisplayClass18_0.<<GenerateInitialOptions>b__0>d <<GenerateInitialOptions>b__0>d;
					<<GenerateInitialOptions>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<GenerateInitialOptions>b__0>d.<>4__this = CS$<>8__locals1;
					<<GenerateInitialOptions>b__0>d.<>1__state = -1;
					<<GenerateInitialOptions>b__0>d.<>t__builder.Start<RanwidTheElder.<>c__DisplayClass18_0.<<GenerateInitialOptions>b__0>d>(ref <<GenerateInitialOptions>b__0>d);
					return <<GenerateInitialOptions>b__0>d.<>t__builder.Task;
				}, this.PotionChoiceTitle, this.PotionChoiceDescription, "RANWID_THE_ELDER.pages.INITIAL.options.POTION", CS$<>8__locals1.potion.HoverTips).ThatHasDynamicTitle());
			}
			else
			{
				list.Add(new EventOption(this, null, "RANWID_THE_ELDER.pages.INITIAL.options.POTION_LOCKED", Array.Empty<IHoverTip>()));
			}
			list.Add(new EventOption(this, new Func<Task>(this.GiveGold), "RANWID_THE_ELDER.pages.INITIAL.options.GOLD", Array.Empty<IHoverTip>()));
			CS$<>8__locals1.relic = base.Rng.NextItem<RelicModel>(base.Owner.Relics.Where<RelicModel>((RelicModel r) => r.IsTradable));
			if (CS$<>8__locals1.relic != null)
			{
				((StringVar)base.DynamicVars["Relic"]).StringValue = CS$<>8__locals1.relic.Title.GetFormattedText();
				list.Add(new EventOption(this, delegate
				{
					RanwidTheElder.<>c__DisplayClass18_0.<<GenerateInitialOptions>b__2>d <<GenerateInitialOptions>b__2>d;
					<<GenerateInitialOptions>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<GenerateInitialOptions>b__2>d.<>4__this = CS$<>8__locals1;
					<<GenerateInitialOptions>b__2>d.<>1__state = -1;
					<<GenerateInitialOptions>b__2>d.<>t__builder.Start<RanwidTheElder.<>c__DisplayClass18_0.<<GenerateInitialOptions>b__2>d>(ref <<GenerateInitialOptions>b__2>d);
					return <<GenerateInitialOptions>b__2>d.<>t__builder.Task;
				}, this.RelicChoiceTitle, this.RelicChoiceDescription, "RANWID_THE_ELDER.pages.INITIAL.options.RELIC", CS$<>8__locals1.relic.HoverTips).ThatHasDynamicTitle());
			}
			else
			{
				list.Add(new EventOption(this, null, "RANWID_THE_ELDER.pages.INITIAL.options.RELIC_LOCKED", Array.Empty<IHoverTip>()));
			}
			return list;
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x00278250 File Offset: 0x00276450
		private async Task GivePotion(PotionModel potion)
		{
			await PotionCmd.Discard(potion);
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("RANWID_THE_ELDER.pages.POTION.description"));
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x0027829C File Offset: 0x0027649C
		private async Task GiveGold()
		{
			await PlayerCmd.LoseGold(base.DynamicVars.Gold.IntValue, base.Owner, GoldLossType.Spent);
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("RANWID_THE_ELDER.pages.GOLD.description"));
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x002782E0 File Offset: 0x002764E0
		private async Task GiveRelic(RelicModel relic)
		{
			await RelicCmd.Remove(relic);
			for (int i = 0; i < 2; i++)
			{
				await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable(), base.Owner, -1);
			}
			base.SetEventFinished(base.L10NLookup("RANWID_THE_ELDER.pages.RELIC.description"));
		}

		// Token: 0x0400279D RID: 10141
		private const string _potionChoiceKey = "RANWID_THE_ELDER.pages.INITIAL.options.POTION";

		// Token: 0x0400279E RID: 10142
		private const string _potionKey = "Potion";

		// Token: 0x0400279F RID: 10143
		private const string _relicChoiceKey = "RANWID_THE_ELDER.pages.INITIAL.options.RELIC";

		// Token: 0x040027A0 RID: 10144
		private const string _relicKey = "Relic";
	}
}
