using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200083A RID: 2106
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeMerchant : EventModel
	{
		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x060067F3 RID: 26611 RVA: 0x0027504C File Offset: 0x0027324C
		public static MerchantDialogueSet Dialogue
		{
			get
			{
				if (FakeMerchant._dialogue != null)
				{
					return FakeMerchant._dialogue;
				}
				LocTable table = LocManager.Instance.GetTable("events");
				string text = StringHelper.Slugify("FakeMerchant") + ".talk.";
				IReadOnlyList<LocString> locStringsWithPrefix = table.GetLocStringsWithPrefix(text);
				FakeMerchant._dialogue = MerchantDialogueSet.CreateFromLocStrings(locStringsWithPrefix);
				return FakeMerchant._dialogue;
			}
		}

		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x060067F4 RID: 26612 RVA: 0x002750A3 File Offset: 0x002732A3
		public override EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Custom;
			}
		}

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x060067F5 RID: 26613 RVA: 0x002750A6 File Offset: 0x002732A6
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x060067F6 RID: 26614 RVA: 0x002750A9 File Offset: 0x002732A9
		// (set) Token: 0x060067F7 RID: 26615 RVA: 0x002750B1 File Offset: 0x002732B1
		public MerchantInventory Inventory
		{
			get
			{
				return this._inventory;
			}
			private set
			{
				base.AssertMutable();
				this._inventory = value;
			}
		}

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x060067F8 RID: 26616 RVA: 0x002750C0 File Offset: 0x002732C0
		// (set) Token: 0x060067F9 RID: 26617 RVA: 0x002750C8 File Offset: 0x002732C8
		public bool StartedFight
		{
			get
			{
				return this._startedFight;
			}
			private set
			{
				base.AssertMutable();
				this._startedFight = value;
			}
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x002750D7 File Offset: 0x002732D7
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return Array.Empty<EventOption>();
		}

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x060067FB RID: 26619 RVA: 0x002750DE File Offset: 0x002732DE
		public override IEnumerable<LocString> GameInfoOptions
		{
			get
			{
				return Array.Empty<LocString>();
			}
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x002750E8 File Offset: 0x002732E8
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex < 1)
			{
				return false;
			}
			if (runState.Players.Count > 1)
			{
				return false;
			}
			return runState.Players.All<Player>(delegate(Player player)
			{
				if (player.Gold < 100)
				{
					return player.Potions.Any<PotionModel>((PotionModel potion) => potion is FoulPotion);
				}
				return true;
			});
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x0027513C File Offset: 0x0027333C
		protected override Task BeforeEventStarted(bool isPreFinished)
		{
			this.Inventory = new MerchantInventory(base.Owner);
			List<RelicModel> list = FakeMerchant._inventoryRelics.ToList<RelicModel>().UnstableShuffle<RelicModel>(base.Rng).Take<RelicModel>(6)
				.ToList<RelicModel>();
			foreach (RelicModel relicModel in list)
			{
				MerchantRelicEntry merchantRelicEntry = new MerchantRelicEntry(relicModel.ToMutable(), base.Owner);
				this.Inventory.AddRelicEntry(merchantRelicEntry);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x002751D8 File Offset: 0x002733D8
		protected override void OnEventFinished()
		{
			if (this.StartedFight)
			{
				return;
			}
			MapPointHistoryEntry currentMapPointHistoryEntry = base.Owner.RunState.CurrentMapPointHistoryEntry;
			PlayerMapPointHistoryEntry playerMapPointHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.GetEntry(base.Owner.NetId) : null);
			if (playerMapPointHistoryEntry == null)
			{
				return;
			}
			foreach (MerchantRelicEntry merchantRelicEntry in this.Inventory.RelicEntries)
			{
				if (merchantRelicEntry.IsStocked)
				{
					playerMapPointHistoryEntry.RelicChoices.Add(new ModelChoiceHistoryEntry(merchantRelicEntry.Model.Id, false));
				}
			}
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x0027527C File Offset: 0x0027347C
		public async Task FoulPotionThrown(FoulPotion potion)
		{
			if (LocalContext.IsMine(this))
			{
				NFakeMerchant nfakeMerchant = base.Node as NFakeMerchant;
				if (nfakeMerchant != null)
				{
					await nfakeMerchant.FoulPotionThrown();
				}
			}
			this.StartedFight = true;
			List<Reward> list = new List<Reward>();
			list.Add(new RelicReward(ModelDb.Relic<FakeMerchantsRug>().ToMutable(), base.Owner));
			FakeMerchant fakeMerchant = (FakeMerchant)RunManager.Instance.EventSynchronizer.GetEventForPlayer(base.Owner);
			foreach (MerchantRelicEntry merchantRelicEntry in fakeMerchant.Inventory.RelicEntries)
			{
				if (merchantRelicEntry.IsStocked || base.Owner.RunState.Players.Count > 1)
				{
					list.Add(new RelicReward(merchantRelicEntry.Model, base.Owner));
				}
			}
			base.EnterCombatWithoutExitingEvent<FakeMerchantEventEncounter>(list, false);
		}

		// Token: 0x0400277F RID: 10111
		public const int relicCost = 50;

		/// <summary>
		/// Note: don't add <see cref="T:MegaCrit.Sts2.Core.Models.Relics.FakeMerchantsRug" /> to this list, it's a combat reward, not a relic you can buy.
		/// </summary>
		// Token: 0x04002780 RID: 10112
		private static readonly RelicModel[] _inventoryRelics = new RelicModel[]
		{
			ModelDb.Relic<FakeAnchor>(),
			ModelDb.Relic<FakeBloodVial>(),
			ModelDb.Relic<FakeHappyFlower>(),
			ModelDb.Relic<FakeLeesWaffle>(),
			ModelDb.Relic<FakeMango>(),
			ModelDb.Relic<FakeOrichalcum>(),
			ModelDb.Relic<FakeSneckoEye>(),
			ModelDb.Relic<FakeStrikeDummy>(),
			ModelDb.Relic<FakeVenerableTeaSet>()
		};

		// Token: 0x04002781 RID: 10113
		[Nullable(2)]
		private static MerchantDialogueSet _dialogue;

		// Token: 0x04002782 RID: 10114
		[Nullable(2)]
		private MerchantInventory _inventory;

		// Token: 0x04002783 RID: 10115
		private bool _startedFight;
	}
}
