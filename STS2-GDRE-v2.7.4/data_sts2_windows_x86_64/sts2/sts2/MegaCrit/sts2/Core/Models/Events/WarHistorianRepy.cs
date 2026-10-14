using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000865 RID: 2149
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WarHistorianRepy : EventModel
	{
		// Token: 0x060069A0 RID: 27040 RVA: 0x0027D7F5 File Offset: 0x0027B9F5
		public override bool IsAllowed(IRunState runState)
		{
			return false;
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x060069A1 RID: 27041 RVA: 0x0027D7F8 File Offset: 0x0027B9F8
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x0027D7FC File Offset: 0x0027B9FC
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.InitialUnlockCage), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CAGE", HoverTipFactory.FromRelic<HistoryCourse>().Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<LanternKey>(false))),
				new EventOption(this, new Func<Task>(this.InitialUnlockChest), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CHEST", HoverTipFactory.FromCardWithCardHoverTips<LanternKey>(false))
			});
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x060069A3 RID: 27043 RVA: 0x0027D860 File Offset: 0x0027BA60
		private bool ShouldGetSecondReward
		{
			get
			{
				return base.Owner.Deck.Cards.Any<CardModel>((CardModel c) => c is LanternKey) && base.Owner.RunState.Players.Count <= 1;
			}
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x0027D8C0 File Offset: 0x0027BAC0
		private async Task InitialUnlockCage()
		{
			await this.RemoveLanternKeysForInitialChoice();
			await this.UnlockCage();
			if (this.ShouldGetSecondReward)
			{
				this.SetEventState(base.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CAGE.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.SecondUnlockChest), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CHEST", HoverTipFactory.FromCardWithCardHoverTips<LanternKey>(false))));
			}
			else
			{
				base.SetEventFinished(base.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CAGE.description"));
			}
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x0027D904 File Offset: 0x0027BB04
		private async Task InitialUnlockChest()
		{
			await this.RemoveLanternKeysForInitialChoice();
			await this.UnlockChest();
			if (this.ShouldGetSecondReward)
			{
				this.SetEventState(base.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CHEST.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.SecondUnlockCage), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CAGE", HoverTipFactory.FromRelic<HistoryCourse>().Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<LanternKey>(false)))));
			}
			else
			{
				base.SetEventFinished(base.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CHEST.description"));
			}
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x0027D948 File Offset: 0x0027BB48
		private async Task SecondUnlockCage()
		{
			base.SetEventFinished(base.L10NLookup("WAR_HISTORIAN_REPY.pages.EXTRA_UNLOCK_CAGE.description"));
			await this.RemoveLanternKeysForSecondChoice();
			await this.UnlockCage();
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x0027D98C File Offset: 0x0027BB8C
		private async Task SecondUnlockChest()
		{
			base.SetEventFinished(base.L10NLookup("WAR_HISTORIAN_REPY.pages.EXTRA_UNLOCK_CHEST.description"));
			await this.RemoveLanternKeysForSecondChoice();
			await this.UnlockChest();
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x0027D9D0 File Offset: 0x0027BBD0
		private async Task UnlockChest()
		{
			List<Reward> list = new List<Reward>();
			list.Add(new PotionReward(base.Owner));
			list.Add(new PotionReward(base.Owner));
			list.Add(new RelicReward(base.Owner));
			list.Add(new RelicReward(base.Owner));
			await RewardsCmd.OfferCustom(base.Owner, list);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x0027DA14 File Offset: 0x0027BC14
		private async Task UnlockCage()
		{
			base.Owner.RunState.ExtraFields.FreedRepy = true;
			await RelicCmd.Obtain<HistoryCourse>(base.Owner);
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x0027DA58 File Offset: 0x0027BC58
		private async Task RemoveLanternKeysForInitialChoice()
		{
			bool flag = base.Owner.RunState.Players.Count > 1;
			if (flag)
			{
				await this.RemoveLanternKeysForSecondChoice();
			}
			else
			{
				await this.RemoveFirstLanternKey();
			}
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x0027DA9C File Offset: 0x0027BC9C
		private async Task RemoveFirstLanternKey()
		{
			CardModel cardModel = base.Owner.Deck.Cards.FirstOrDefault<CardModel>((CardModel c) => c is LanternKey);
			if (cardModel != null)
			{
				PlayerCmd.CompleteQuest(cardModel);
				await CardPileCmd.RemoveFromDeck(cardModel, true);
			}
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x0027DAE0 File Offset: 0x0027BCE0
		private async Task RemoveLanternKeysForSecondChoice()
		{
			List<CardModel> list = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c is LanternKey).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				PlayerCmd.CompleteQuest(cardModel);
				await CardPileCmd.RemoveFromDeck(cardModel, true);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}
	}
}
