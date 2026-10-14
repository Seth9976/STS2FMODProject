using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000838 RID: 2104
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DrowningBeacon : EventModel
	{
		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x060067D7 RID: 26583 RVA: 0x0027471C File Offset: 0x0027291C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(13m),
					new StringVar("Potion", ModelDb.Potion<GlowwaterPotion>().Title.GetFormattedText()),
					new StringVar("Relic", ModelDb.Relic<FresnelLens>().Title.GetFormattedText())
				});
			}
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x0027477C File Offset: 0x0027297C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.BottleOption), "DROWNING_BEACON.pages.INITIAL.options.BOTTLE", new IHoverTip[] { HoverTipFactory.FromPotion(ModelDb.Potion<GlowwaterPotion>()) }),
				new EventOption(this, new Func<Task>(this.ClimbOption), "DROWNING_BEACON.pages.INITIAL.options.CLIMB", HoverTipFactory.FromRelic<FresnelLens>()).ThatDecreasesMaxHp(base.DynamicVars.HpLoss.BaseValue)
			});
		}

		/// <summary>
		/// Player chose to bottle the water and get a Glowwater Potion.
		/// </summary>
		// Token: 0x060067D9 RID: 26585 RVA: 0x002747F8 File Offset: 0x002729F8
		private async Task BottleOption()
		{
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
			{
				new PotionReward(ModelDb.Potion<GlowwaterPotion>().ToMutable(), base.Owner)
			});
			base.SetEventFinished(base.L10NLookup("DROWNING_BEACON.pages.BOTTLE.description"));
		}

		/// <summary>
		/// Player chose to climb the Lighthouse. They lose Max HP and get the Fresnel Lens relic.
		/// </summary>
		// Token: 0x060067DA RID: 26586 RVA: 0x0027483C File Offset: 0x00272A3C
		private async Task ClimbOption()
		{
			await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, false);
			await RelicCmd.Obtain(ModelDb.Relic<FresnelLens>().ToMutable(), base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("DROWNING_BEACON.pages.CLIMB.description"));
		}

		// Token: 0x04002775 RID: 10101
		private const string _potionKey = "Potion";

		// Token: 0x04002776 RID: 10102
		private const string _relicKey = "Relic";
	}
}
