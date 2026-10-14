using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000855 RID: 2133
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SunkenTreasury : EventModel
	{
		// Token: 0x060068ED RID: 26861 RVA: 0x00279BC4 File Offset: 0x00277DC4
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.FirstChest), "SUNKEN_TREASURY.pages.INITIAL.options.FIRST_CHEST", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.SecondChest), "SUNKEN_TREASURY.pages.INITIAL.options.SECOND_CHEST", HoverTipFactory.FromCardWithCardHoverTips<Greed>(false))
			});
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x060068EE RID: 26862 RVA: 0x00279C1B File Offset: 0x00277E1B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("SmallChestGold", 60m),
					new DynamicVar("LargeChestGold", 333m)
				});
			}
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x00279C54 File Offset: 0x00277E54
		public override void CalculateVars()
		{
			base.DynamicVars["SmallChestGold"].BaseValue += base.Rng.NextInt(16) - 8;
			base.DynamicVars["LargeChestGold"].BaseValue += base.Rng.NextInt(61) - 30;
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x00279CCC File Offset: 0x00277ECC
		private async Task FirstChest()
		{
			await PlayerCmd.GainGold(base.DynamicVars["SmallChestGold"].BaseValue, base.Owner, false);
			base.SetEventFinished(base.L10NLookup("SUNKEN_TREASURY.pages.FIRST_CHEST.description"));
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x00279D10 File Offset: 0x00277F10
		private async Task SecondChest()
		{
			await PlayerCmd.GainGold(base.DynamicVars["LargeChestGold"].BaseValue, base.Owner, false);
			await CardPileCmd.AddCurseToDeck<Greed>(base.Owner);
			LocString locString = base.L10NLookup("SUNKEN_TREASURY.pages.SECOND_CHEST.description");
			locString.Add("Monologue", new LocString("characters", base.Owner.Character.Id.Entry + ".goldMonologue"));
			base.SetEventFinished(locString);
		}

		// Token: 0x040027C1 RID: 10177
		private const string _smallChestGoldKey = "SmallChestGold";

		// Token: 0x040027C2 RID: 10178
		private const string _largeChestGoldKey = "LargeChestGold";
	}
}
