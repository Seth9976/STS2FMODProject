using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000847 RID: 2119
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionCourier : EventModel
	{
		// Token: 0x0600686B RID: 26731 RVA: 0x00277BEC File Offset: 0x00275DEC
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.GrabPotions), "POTION_COURIER.pages.INITIAL.options.GRAB_POTIONS", new IHoverTip[] { HoverTipFactory.FromPotion<FoulPotion>() }),
				new EventOption(this, new Func<Task>(this.Ransack), "POTION_COURIER.pages.INITIAL.options.RANSACK", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600686C RID: 26732 RVA: 0x00277C4B File Offset: 0x00275E4B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("FoulPotions", 3m));
			}
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x00277C62 File Offset: 0x00275E62
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex > 0;
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x00277C70 File Offset: 0x00275E70
		private async Task GrabPotions()
		{
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < base.DynamicVars["FoulPotions"].IntValue; i++)
			{
				list.Add(new PotionReward(ModelDb.Potion<FoulPotion>().ToMutable(), base.Owner));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
			base.SetEventFinished(base.L10NLookup("POTION_COURIER.pages.GRAB_POTIONS.description"));
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x00277CB4 File Offset: 0x00275EB4
		private async Task Ransack()
		{
			IEnumerable<PotionModel> enumerable = from p in base.Owner.Character.PotionPool.GetUnlockedPotions(base.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(base.Owner.UnlockState))
				where p.Rarity == PotionRarity.Uncommon
				select p;
			PotionModel potionModel = base.Owner.PlayerRng.Rewards.NextItem<PotionModel>(enumerable);
			if (potionModel != null)
			{
				await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
				{
					new PotionReward(potionModel.ToMutable(), base.Owner)
				});
			}
			base.SetEventFinished(base.L10NLookup("POTION_COURIER.pages.RANSACK.description"));
		}

		// Token: 0x0400279B RID: 10139
		private const string _foulPotionsKey = "FoulPotions";
	}
}
