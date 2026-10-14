using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rewards
{
	/// <summary>
	/// Represents a set of rewards that can be presented to the player.
	/// One of these is always generated at the end of combat, but events and relics can also present rewards.
	/// This class will always apply reward modifications from relics and other hooks.
	/// There are currently no cases in which you need to use this class directly. In the future, if there are complicated
	/// reward cases that are hard to build into RewardsCmd, change this class to support them.
	/// </summary>
	// Token: 0x0200014B RID: 331
	[NullableContext(1)]
	[Nullable(0)]
	public class RewardsSet
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x00060931 File Offset: 0x0005EB31
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x00060939 File Offset: 0x0005EB39
		[Nullable(2)]
		public AbstractRoom Room
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00060942 File Offset: 0x0005EB42
		public Player Player { get; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x0006094A File Offset: 0x0005EB4A
		public List<Reward> Rewards { get; } = new List<Reward>();

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x00060952 File Offset: 0x0005EB52
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x0006095A File Offset: 0x0005EB5A
		public bool ThrowInTestIfRewardsNotTaken { get; set; } = true;

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x00060963 File Offset: 0x0005EB63
		public bool DisallowSkipping
		{
			get
			{
				return this._disallowSkipping;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x0006096B File Offset: 0x0005EB6B
		public bool AllRewardsSuccessfullySelected
		{
			get
			{
				return this.Rewards.All<Reward>((Reward r) => r.SuccessfullySelected);
			}
		}

		/// <summary>
		/// Set by RewardsSetSynchronizer.
		/// </summary>
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x00060997 File Offset: 0x0005EB97
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x0006099F File Offset: 0x0005EB9F
		public int Id { get; set; } = -1;

		// Token: 0x0600100C RID: 4108 RVA: 0x000609A8 File Offset: 0x0005EBA8
		public RewardsSet(Player player, [Nullable(2)] RewardsSetSynchronizer synchronizer = null)
		{
			this.Player = player;
			this._synchronizer = synchronizer ?? RunManager.Instance.RewardsSetSynchronizer;
		}

		/// <summary>
		/// Associates this rewards set with a room without generating rewards for it, and allows the rewards screen
		/// to be shown even with no rewards. This determines whether the rewards screen is terminal (has a proceed
		/// button). Use <see cref="M:MegaCrit.Sts2.Core.Rewards.RewardsSet.WithRewardsFromRoom(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" /> to both associate a room and generate its rewards.
		/// </summary>
		// Token: 0x0600100D RID: 4109 RVA: 0x000609E5 File Offset: 0x0005EBE5
		public RewardsSet EmptyForRoom(AbstractRoom room)
		{
			this.Room = room;
			this._allowEmptyRewards = true;
			return this;
		}

		/// <summary>
		/// Generate a list of rewards to offer for the specified room.
		/// Should only be used with combat or treasure rooms.
		/// </summary>
		/// <param name="room">Room to generate rewards for.</param>
		// Token: 0x0600100E RID: 4110 RVA: 0x000609F8 File Offset: 0x0005EBF8
		public RewardsSet WithRewardsFromRoom(AbstractRoom room)
		{
			this.Room = room;
			if (room.RoomType == RoomType.Boss && this.Player.RunState.CurrentActIndex >= this.Player.RunState.Acts.Count - 1)
			{
				return this;
			}
			if (!this.TryGenerateTutorialRewards(this.Player, room))
			{
				this.Rewards.AddRange(this.GenerateRewardsFor(this.Player, room));
			}
			CombatRoom combatRoom = this.Room as CombatRoom;
			List<Reward> list;
			if (combatRoom != null && combatRoom.ExtraRewards.TryGetValue(this.Player, out list))
			{
				this.Rewards.AddRange(list);
			}
			return this;
		}

		/// <summary>
		/// Used when custom rewards are offered, like in an event or by a relic.
		/// </summary>
		// Token: 0x0600100F RID: 4111 RVA: 0x00060A9A File Offset: 0x0005EC9A
		public RewardsSet WithCustomRewards(List<Reward> rewards)
		{
			this.Rewards.AddRange(rewards);
			return this;
		}

		/// <summary>
		/// Call this to disallow skipping the rewards. The player must take all the rewards before the screen is dismissed.
		/// </summary>
		// Token: 0x06001010 RID: 4112 RVA: 0x00060AA9 File Offset: 0x0005ECA9
		public RewardsSet WithSkippingDisallowed()
		{
			this._disallowSkipping = true;
			return this;
		}

		/// <summary>
		/// Populates the reward instances in Rewards, calls modify hooks on them, and returns the result.
		/// Usually you want to use Offer instead. This is mostly for tests where we inspect the rewards that are generated.
		/// </summary>
		// Token: 0x06001011 RID: 4113 RVA: 0x00060AB4 File Offset: 0x0005ECB4
		public async Task GenerateWithoutOffering()
		{
			if (!this._isGenerated)
			{
				List<Reward> list = this.Rewards.ToList<Reward>();
				foreach (Reward reward in this.Rewards)
				{
					reward.Populate();
				}
				IEnumerable<AbstractModel> enumerable = Hook.ModifyRewards(this.Player.RunState, this.Player, this.Rewards, this.Room);
				foreach (Reward reward2 in this.Rewards.Except<Reward>(list))
				{
					if (!reward2.IsPopulated)
					{
						reward2.Populate();
					}
				}
				await Hook.AfterModifyingRewards(this.Player.RunState, enumerable);
				this.Rewards.Sort((Reward x, Reward y) => x.RewardsSetIndex.CompareTo(y.RewardsSetIndex));
				this._isGenerated = true;
			}
		}

		/// <summary>
		/// Populates the reward instances in Rewards, calls modify hooks on them, and offers the rewards to the player.
		/// If the player is not the local player, then rewards are only generated on the backend and not shown to the player.
		/// </summary>
		// Token: 0x06001012 RID: 4114 RVA: 0x00060AF8 File Offset: 0x0005ECF8
		public async Task Offer()
		{
			if (!this.Player.Creature.IsDead)
			{
				await this.GenerateWithoutOffering();
				bool flag = this.Room is CombatRoom;
				Task task = this._synchronizer.BeginRewardsSet(this);
				if (this.Rewards.Count > 0 || flag || this._allowEmptyRewards)
				{
					if (!this.Rewards.All<Reward>((Reward r) => r.IsPopulated))
					{
						if (this.Rewards.Any<Reward>((Reward r) => r.IsPopulated))
						{
							Log.Warn("Some rewards are populated and others are not when calling RewardsCmd.Offer! This might lead to hooks getting called twice", 2);
						}
					}
					if (LocalContext.IsMe(this.Player))
					{
						if (TestMode.IsOn)
						{
							if (RewardsSet.testSelector != null)
							{
								await RewardsSet.testSelector(this);
							}
							else
							{
								foreach (Reward reward in this.Rewards)
								{
									await this._synchronizer.SelectLocalReward(reward);
								}
								List<Reward>.Enumerator enumerator = default(List<Reward>.Enumerator);
							}
							if (!this._synchronizer.IsRewardsSetCompleted(this) && this.ThrowInTestIfRewardsNotTaken)
							{
								throw new InvalidOperationException("The RewardsSet is not complete after rewards were selected!");
							}
						}
						else
						{
							NRewardsScreen.ShowScreen(this, flag, this.Player.RunState);
						}
					}
					await task;
				}
			}
		}

		/// <summary>
		/// Generates a list of default rewards to offer at the end of the specified room.
		/// This does _not_ include hook modifications, like from Prayer Wheel.
		/// Calling this may increment RNG counters and make other run state changes.
		/// </summary>
		/// <param name="player">Player to generate rewards for.</param>
		/// <param name="room">Room to generate rewards for.</param>
		/// <returns>List of rewards.</returns>
		// Token: 0x06001013 RID: 4115 RVA: 0x00060B3C File Offset: 0x0005ED3C
		private List<Reward> GenerateRewardsFor(Player player, AbstractRoom room)
		{
			if (RunManager.Instance == null)
			{
				throw new InvalidOperationException("Only valid during a run.");
			}
			List<Reward> list = new List<Reward>();
			CombatRoom combatRoom = room as CombatRoom;
			if (combatRoom == null)
			{
				if (!(room is TreasureRoom))
				{
					throw new InvalidOperationException("Tried to generate a reward for invalid room type: " + room.GetType().Name);
				}
			}
			else
			{
				switch (room.RoomType)
				{
				case RoomType.Monster:
					if (combatRoom.GoldProportion > 0f)
					{
						list.Add(new GoldReward((int)Math.Round((double)((float)combatRoom.Encounter.MinGoldReward * combatRoom.GoldProportion)), (int)Math.Round((double)((float)combatRoom.Encounter.MaxGoldReward * combatRoom.GoldProportion)), player, false));
					}
					this.RollForPotionAndAddTo(list, player, room.RoomType);
					list.Add(new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player, null));
					break;
				case RoomType.Elite:
					list.Add(new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player, false));
					this.RollForPotionAndAddTo(list, player, room.RoomType);
					list.Add(new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player, null));
					list.Add(new RelicReward(player));
					break;
				case RoomType.Boss:
					list.Add(new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player, false));
					this.RollForPotionAndAddTo(list, player, room.RoomType);
					list.Add(new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player, null));
					break;
				}
			}
			return list;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00060CF8 File Offset: 0x0005EEF8
		private void RollForPotionAndAddTo(ICollection<Reward> rewards, Player player, RoomType roomType)
		{
			PotionRewardOdds potionReward = player.PlayerOdds.PotionReward;
			AscensionManager ascensionManager = RunManager.Instance.AscensionManager;
			if (!potionReward.Roll(player, roomType))
			{
				return;
			}
			rewards.Add(new PotionReward(player));
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00060D34 File Offset: 0x0005EF34
		private bool TryGenerateTutorialRewards(Player player, AbstractRoom room)
		{
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForRoom(player, room.RoomType);
			if (player.UnlockState.NumberOfRuns == 0 && player.UnlockState.EpochUnlockCount() == 0 && player.Character is Ironclad)
			{
				CombatRoom combatRoom = room as CombatRoom;
				if (combatRoom != null)
				{
					int num = player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> p) => p).Count<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.Rooms.FindIndex((MapPointRoomHistoryEntry r) => r.RoomType == RoomType.Monster) >= 0);
					if (room.RoomType == RoomType.Monster && num <= 7)
					{
						ValueTuple<CardModel[], PotionModel>? tutorialMonsterRewards = RewardsSet.GetTutorialMonsterRewards(player, num - 1);
						if (tutorialMonsterRewards != null)
						{
							ValueTuple<CardModel[], PotionModel> valueOrDefault = tutorialMonsterRewards.GetValueOrDefault();
							this.Rewards.Add(new GoldReward(10, 20, player, false));
							if (valueOrDefault.Item2 != null)
							{
								this.Rewards.Add(new PotionReward(valueOrDefault.Item2, player));
							}
							this.Rewards.Add(new CardReward(valueOrDefault.Item1, CardCreationSource.Encounter, player, cardCreationOptions, null));
							return true;
						}
						return false;
					}
					else if (room.RoomType == RoomType.Elite)
					{
						int num2 = player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> l) => l).Count<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.MapPointType == MapPointType.Elite);
						if (num2 == 1)
						{
							CardModel[] array = new CardModel[]
							{
								player.RunState.CreateCard<Bludgeon>(player),
								player.RunState.CreateCard<Pyre>(player),
								player.RunState.CreateCard<EvilEye>(player)
							};
							this.Rewards.Add(new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player, false));
							this.Rewards.Add(new PotionReward(ModelDb.Potion<BlockPotion>().ToMutable(), player));
							this.Rewards.Add(RewardsSet.GetTutorialRelicReward<Vajra>(player));
							this.Rewards.Add(new CardReward(array, CardCreationSource.Encounter, player, cardCreationOptions, null));
							return true;
						}
						if (num2 == 2)
						{
							CardModel[] array2 = new CardModel[]
							{
								player.RunState.CreateCard<Pillage>(player),
								player.RunState.CreateCard<Rampage>(player),
								player.RunState.CreateCard<FlameBarrier>(player)
							};
							this.Rewards.Add(new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player, false));
							this.Rewards.Add(RewardsSet.GetTutorialRelicReward<OrnamentalFan>(player));
							this.Rewards.Add(new CardReward(array2, CardCreationSource.Encounter, player, cardCreationOptions, null));
							return true;
						}
					}
					else if (room.RoomType == RoomType.Boss)
					{
						if (player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> l) => l).Count<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.MapPointType == MapPointType.Boss) == 1)
						{
							CardModel[] array3 = new CardModel[]
							{
								player.RunState.CreateCard<PrimalForce>(player),
								player.RunState.CreateCard<DemonForm>(player),
								player.RunState.CreateCard<Thrash>(player)
							};
							this.Rewards.Add(new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player, false));
							this.Rewards.Add(new CardReward(array3, CardCreationSource.Encounter, player, cardCreationOptions, null));
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000610D0 File Offset: 0x0005F2D0
		private static RelicReward GetTutorialRelicReward<[Nullable(0)] T>(Player player) where T : RelicModel
		{
			if (player.RelicGrabBag.Contains(ModelDb.Relic<T>()))
			{
				return new RelicReward(ModelDb.Relic<T>().ToMutable(), player);
			}
			return new RelicReward(player);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00061108 File Offset: 0x0005F308
		[return: TupleElementNames(new string[] { "Cards", "Potion" })]
		[return: Nullable(new byte[] { 0, 1, 1, 2 })]
		public static ValueTuple<CardModel[], PotionModel>? GetTutorialMonsterRewards(Player player, int index)
		{
			ValueTuple<CardModel[], PotionModel>? valueTuple;
			switch (index)
			{
			case 0:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<SetupStrike>(player),
					player.RunState.CreateCard<Tremble>(player),
					player.RunState.CreateCard<BloodWall>(player)
				}, null));
				break;
			case 1:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<Breakthrough>(player),
					player.RunState.CreateCard<Inflame>(player),
					player.RunState.CreateCard<Anger>(player)
				}, null));
				break;
			case 2:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<IronWave>(player),
					player.RunState.CreateCard<Dismantle>(player),
					player.RunState.CreateCard<Cinder>(player)
				}, ModelDb.Potion<FirePotion>().ToMutable()));
				break;
			case 3:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<Stomp>(player),
					player.RunState.CreateCard<ShrugItOff>(player),
					player.RunState.CreateCard<Armaments>(player)
				}, null));
				break;
			case 4:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<Thunderclap>(player),
					player.RunState.CreateCard<SetupStrike>(player),
					player.RunState.CreateCard<Rage>(player)
				}, ModelDb.Potion<StrengthPotion>().ToMutable()));
				break;
			case 5:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<BattleTrance>(player),
					player.RunState.CreateCard<TrueGrit>(player),
					player.RunState.CreateCard<Uppercut>(player)
				}, null));
				break;
			case 6:
				valueTuple = new ValueTuple<CardModel[], PotionModel>?(new ValueTuple<CardModel[], PotionModel>(new CardModel[]
				{
					player.RunState.CreateCard<Bloodletting>(player),
					player.RunState.CreateCard<Whirlwind>(player),
					player.RunState.CreateCard<Tremble>(player)
				}, ModelDb.Potion<EnergyPotion>().ToMutable()));
				break;
			default:
				valueTuple = null;
				break;
			}
			return valueTuple;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00061338 File Offset: 0x0005F538
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Id: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" Owner: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" Rewards: ");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join<Reward>(",", this.Rewards));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040005A3 RID: 1443
		[Nullable(new byte[] { 2, 1, 1 })]
		public static Func<RewardsSet, Task> testSelector;

		// Token: 0x040005A8 RID: 1448
		private bool _allowEmptyRewards;

		// Token: 0x040005A9 RID: 1449
		private bool _disallowSkipping;

		// Token: 0x040005AA RID: 1450
		private bool _isGenerated;

		// Token: 0x040005AB RID: 1451
		private readonly RewardsSetSynchronizer _synchronizer;
	}
}
