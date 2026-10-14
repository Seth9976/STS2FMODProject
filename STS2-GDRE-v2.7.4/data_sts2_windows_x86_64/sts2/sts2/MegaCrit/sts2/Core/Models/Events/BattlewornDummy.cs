using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082B RID: 2091
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattlewornDummy : EventModel
	{
		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x06006776 RID: 26486 RVA: 0x00272B5F File Offset: 0x00270D5F
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x06006777 RID: 26487 RVA: 0x00272B64 File Offset: 0x00270D64
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Setting1Hp", ModelDb.Monster<BattleFriendV1>().MinInitialHp),
					new DynamicVar("Setting2Hp", ModelDb.Monster<BattleFriendV2>().MinInitialHp),
					new DynamicVar("Setting3Hp", ModelDb.Monster<BattleFriendV3>().MinInitialHp)
				});
			}
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x00272BD0 File Offset: 0x00270DD0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			Player owner = base.Owner;
			int num = ((owner != null) ? owner.RunState.Players.Count : 1);
			Player owner2 = base.Owner;
			int num2 = ((owner2 != null) ? owner2.RunState.CurrentActIndex : 0);
			base.DynamicVars["Setting1Hp"].BaseValue = Creature.ScaleHpForMultiplayer(ModelDb.Monster<BattleFriendV1>().MinInitialHp, ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), num, num2);
			base.DynamicVars["Setting2Hp"].BaseValue = Creature.ScaleHpForMultiplayer(ModelDb.Monster<BattleFriendV2>().MinInitialHp, ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), num, num2);
			base.DynamicVars["Setting3Hp"].BaseValue = Creature.ScaleHpForMultiplayer(ModelDb.Monster<BattleFriendV3>().MinInitialHp, ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), num, num2);
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Setting1), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_1", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Setting2), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_2", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Setting3), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_3", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x00272D0A File Offset: 0x00270F0A
		private Task Setting1()
		{
			base.EnterCombatWithoutExitingEvent(ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), Array.Empty<Reward>(), true);
			return Task.CompletedTask;
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x00272D22 File Offset: 0x00270F22
		private Task Setting2()
		{
			base.EnterCombatWithoutExitingEvent(ModelDb.Encounter<BattlewornDummyEventV2Encounter>(), Array.Empty<Reward>(), true);
			return Task.CompletedTask;
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x00272D3A File Offset: 0x00270F3A
		private Task Setting3()
		{
			base.EnterCombatWithoutExitingEvent(ModelDb.Encounter<BattlewornDummyEventV3Encounter>(), Array.Empty<Reward>(), true);
			return Task.CompletedTask;
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x00272D54 File Offset: 0x00270F54
		public override async Task Resume(AbstractRoom room)
		{
			CombatRoom combatRoom = (CombatRoom)room;
			BattlewornDummyEventEncounter battlewornDummyEventEncounter = (BattlewornDummyEventEncounter)combatRoom.Encounter;
			if (battlewornDummyEventEncounter.RanOutOfTime)
			{
				base.SetEventFinished(base.L10NLookup("BATTLEWORN_DUMMY.pages.DEFEAT.description"));
			}
			else
			{
				base.SetEventFinished(base.L10NLookup("BATTLEWORN_DUMMY.pages.VICTORY.description"));
				List<Reward> list = new List<Reward>();
				if (!(battlewornDummyEventEncounter is BattlewornDummyEventV1Encounter))
				{
					if (!(battlewornDummyEventEncounter is BattlewornDummyEventV2Encounter))
					{
						if (!(battlewornDummyEventEncounter is BattlewornDummyEventV3Encounter))
						{
							goto IL_01A9;
						}
					}
					else
					{
						IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Rng)
							.Take<CardModel>(2);
						using (IEnumerator<CardModel> enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CardModel cardModel = enumerator.Current;
								CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
							}
							goto IL_01A9;
						}
					}
					RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
					list.Add(new RelicReward(relicModel, base.Owner));
				}
				else
				{
					IEnumerable<PotionModel> enumerable2 = base.Owner.Character.PotionPool.GetUnlockedPotions(base.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(base.Owner.UnlockState));
					PotionModel potionModel = base.Owner.PlayerRng.Rewards.NextItem<PotionModel>(enumerable2);
					if (potionModel != null)
					{
						list.Add(new PotionReward(potionModel.ToMutable(), base.Owner));
					}
				}
				IL_01A9:
				if (combatRoom.ExtraRewards.ContainsKey(base.Owner))
				{
					list = list.Concat<Reward>(combatRoom.ExtraRewards[base.Owner]).ToList<Reward>();
				}
				if (list.Count > 0)
				{
					await RewardsCmd.OfferCustom(base.Owner, list);
				}
			}
		}

		// Token: 0x04002756 RID: 10070
		private const string _setting1HpKey = "Setting1Hp";

		// Token: 0x04002757 RID: 10071
		private const string _setting2HpKey = "Setting2Hp";

		// Token: 0x04002758 RID: 10072
		private const string _setting3HpKey = "Setting3Hp";
	}
}
