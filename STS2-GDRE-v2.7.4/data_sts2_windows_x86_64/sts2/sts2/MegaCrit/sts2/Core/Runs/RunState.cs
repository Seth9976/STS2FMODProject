using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000112 RID: 274
	[NullableContext(1)]
	[Nullable(0)]
	public class RunState : IRunState, ICardScope, IPlayerCollection
	{
		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IPlayerCollection.Players" />
		/// </summary>
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x000511FF File Offset: 0x0004F3FF
		public IReadOnlyList<Player> Players
		{
			get
			{
				return this._players;
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.Acts" />
		/// </summary>
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00051207 File Offset: 0x0004F407
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0005120F File Offset: 0x0004F40F
		public IReadOnlyList<ActModel> Acts { get; private set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.CurrentActIndex" />
		/// </summary>
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00051218 File Offset: 0x0004F418
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00051220 File Offset: 0x0004F420
		public int CurrentActIndex
		{
			get
			{
				return this._currentActIndex;
			}
			set
			{
				if (this._currentActIndex == value)
				{
					return;
				}
				this._visitedMapCoords.Clear();
				this.ActFloor = 0;
				this.NextRoomId = 0;
				this._currentActIndex = value;
			}
		}

		/// <summary>
		/// The index of the next ID which would be returned from <see cref="M:MegaCrit.Sts2.Core.Runs.RunState.GetAndIncrementNextRoomId" />.
		/// Reset when we enter a new map location.
		/// </summary>
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0005124C File Offset: 0x0004F44C
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00051254 File Offset: 0x0004F454
		public int NextRoomId { get; private set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.Act" />
		/// </summary>
		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0005125D File Offset: 0x0004F45D
		public ActModel Act
		{
			get
			{
				return this.Acts[this.CurrentActIndex];
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.Map" />
		/// </summary>
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00051270 File Offset: 0x0004F470
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x00051278 File Offset: 0x0004F478
		public ActMap Map { get; set; } = NullActMap.Instance;

		/// <summary>
		/// The MapCoords that have been visited within the current act.
		/// </summary>
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00051281 File Offset: 0x0004F481
		public IReadOnlyList<MapCoord> VisitedMapCoords
		{
			get
			{
				return this._visitedMapCoords;
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.CurrentMapCoord" />
		/// </summary>
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0005128C File Offset: 0x0004F48C
		public MapCoord? CurrentMapCoord
		{
			get
			{
				if (this._visitedMapCoords.Count != 0)
				{
					return new MapCoord?(this._visitedMapCoords.Last<MapCoord>());
				}
				return null;
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.CurrentMapPoint" />
		/// </summary>
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x000512C0 File Offset: 0x0004F4C0
		[Nullable(2)]
		public MapPoint CurrentMapPoint
		{
			[NullableContext(2)]
			get
			{
				if (this.CurrentMapCoord == null)
				{
					return null;
				}
				return this.Map.GetPoint(this.CurrentMapCoord.Value);
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.RunLocation" />
		/// </summary>
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000512F8 File Offset: 0x0004F4F8
		public RunLocation RunLocation
		{
			get
			{
				MapLocation mapLocation = this.MapLocation;
				AbstractRoom currentRoom = this.CurrentRoom;
				return new RunLocation(mapLocation, (currentRoom != null) ? currentRoom.Id : null);
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.MapLocation" />
		/// </summary>
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0005132A File Offset: 0x0004F52A
		public MapLocation MapLocation
		{
			get
			{
				return new MapLocation(this.CurrentMapCoord, this.CurrentActIndex);
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.ActFloor" />
		/// </summary>
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0005133D File Offset: 0x0004F53D
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x00051345 File Offset: 0x0004F545
		public int ActFloor { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.TotalFloor" />
		/// </summary>
		///  NOTE: If in game we end up with an ability to skip rows (ie sts1 winged boots) we will have to rethink this approach.
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0005134E File Offset: 0x0004F54E
		public int TotalFloor
		{
			get
			{
				return this.MapPointHistory.Sum<IReadOnlyList<MapPointHistoryEntry>>((IReadOnlyList<MapPointHistoryEntry> c) => c.Count);
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.MapPointHistory" />
		/// </summary>
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0005137A File Offset: 0x0004F57A
		public IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory
		{
			get
			{
				return this._mapPointHistory;
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.CurrentMapPointHistoryEntry" />
		/// </summary>
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00051382 File Offset: 0x0004F582
		[Nullable(2)]
		public MapPointHistoryEntry CurrentMapPointHistoryEntry
		{
			[NullableContext(2)]
			get
			{
				IReadOnlyList<MapPointHistoryEntry> readOnlyList = this.MapPointHistory.LastOrDefault<IReadOnlyList<MapPointHistoryEntry>>();
				if (readOnlyList == null)
				{
					return null;
				}
				return readOnlyList.LastOrDefault<MapPointHistoryEntry>();
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.CurrentRoomCount" />
		/// </summary>
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0005139A File Offset: 0x0004F59A
		public int CurrentRoomCount
		{
			get
			{
				return this._currentRooms.Count;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x000513A7 File Offset: 0x0004F5A7
		[Nullable(2)]
		public AbstractRoom CurrentRoom
		{
			[NullableContext(2)]
			get
			{
				return this._currentRooms.LastOrDefault<AbstractRoom>();
			}
		}

		/// <summary>
		/// The room at the base of the rooms stack.
		/// Usually the same as CurrentRoom because there's usually only one room in the rooms stack. However, when there
		/// are multiple rooms in the stack (like at an event where one of the options starts a combat), this will be the
		/// first room that was entered in the current map point.
		/// Usually safe to treat as non-null, but may be null during brief windows, like while we're in the middle of
		/// traveling to a new map point.
		/// </summary>
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x000513B4 File Offset: 0x0004F5B4
		[Nullable(2)]
		public AbstractRoom BaseRoom
		{
			[NullableContext(2)]
			get
			{
				return this._currentRooms.FirstOrDefault<AbstractRoom>();
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.IsGameOver" />
		/// </summary>
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x000513C1 File Offset: 0x0004F5C1
		public bool IsGameOver
		{
			get
			{
				if (this.Players.Count > 0)
				{
					return this.Players.All<Player>((Player p) => p.Creature.IsDead);
				}
				return false;
			}
		}

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.GameMode" />
		/// </summary>
		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x000513FD File Offset: 0x0004F5FD
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x00051405 File Offset: 0x0004F605
		public GameMode GameMode { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.AscensionLevel" />
		/// </summary>
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0005140E File Offset: 0x0004F60E
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00051416 File Offset: 0x0004F616
		public int AscensionLevel { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.Rng" />
		/// </summary>
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005141F File Offset: 0x0004F61F
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x00051427 File Offset: 0x0004F627
		public RunRngSet Rng { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.Odds" />
		/// </summary>
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00051430 File Offset: 0x0004F630
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00051438 File Offset: 0x0004F638
		public RunOddsSet Odds { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.SharedRelicGrabBag" />
		/// </summary>
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00051441 File Offset: 0x0004F641
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x00051449 File Offset: 0x0004F649
		public RelicGrabBag SharedRelicGrabBag { get; set; }

		/// <summary>
		/// The unlock state for the run.
		/// In multiplayer, this encompasses the unlocks for _all_ players in the run. Only use this for things that are
		/// shared among players, like the shared treasure relics, or ancients. For checking player-specific unlocks, e.g.
		/// for player-specific rewards, use the unlock state on the individual players.
		/// In singleplayer, this is equivalent to the player's unlock state.
		/// </summary>
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00051452 File Offset: 0x0004F652
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0005145A File Offset: 0x0004F65A
		public UnlockState UnlockState { get; set; }

		/// <summary>
		/// The IDs of all the events that have been visited during this run.
		/// This allows us to avoid visiting the same shared event twice across multiple acts.
		/// </summary>
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00051463 File Offset: 0x0004F663
		public IReadOnlySet<ModelId> VisitedEventIds
		{
			get
			{
				return this._visitedEventIds;
			}
		}

		/// <summary>
		/// List of custom modifiers applied to this combat, for daily or custom runs.
		/// </summary>
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0005146B File Offset: 0x0004F66B
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00051473 File Offset: 0x0004F673
		public IReadOnlyList<ModifierModel> Modifiers { get; private set; }

		/// <summary>
		/// List of badge models.
		/// </summary>
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0005147C File Offset: 0x0004F67C
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00051484 File Offset: 0x0004F684
		public IReadOnlyList<BadgeModel> BadgeModels { get; private set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.ExtraFields" />.
		/// </summary>
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0005148D File Offset: 0x0004F68D
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00051495 File Offset: 0x0004F695
		public ExtraRunFields ExtraFields { get; private set; } = new ExtraRunFields();

		/// <summary>
		/// The model used to scale various things (block, power application) in multiplayer.
		/// </summary>
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0005149E File Offset: 0x0004F69E
		// (set) Token: 0x06000D19 RID: 3353 RVA: 0x000514A6 File Offset: 0x0004F6A6
		public MultiplayerScalingModel MultiplayerScalingModel { get; private set; }

		/// <summary>
		/// Create a RunState for a brand new run.
		/// This will connect each player up with the RunState so their starting inventory works properly.
		/// </summary>
		/// <param name="players">The players that should be in the run.</param>
		/// <param name="acts">The mutable acts that should be in the run.</param>
		/// <param name="modifiers">The modifiers that are applied to the run.</param>
		/// <param name="gameMode">The game mode that we're playing in.</param>
		/// <param name="ascensionLevel">The ascension level that the run should be played at.</param>
		/// <param name="seed">The seed that the run's RNG should use.</param>
		// Token: 0x06000D1A RID: 3354 RVA: 0x000514B0 File Offset: 0x0004F6B0
		public static RunState CreateForNewRun(IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers, GameMode gameMode, int ascensionLevel, string seed)
		{
			RunRngSet runRngSet = new RunRngSet(seed);
			RunOddsSet runOddsSet = new RunOddsSet(runRngSet.UnknownMapPoint);
			RunState runState = RunState.CreateShared(players, acts, modifiers, gameMode, 0, runRngSet, runOddsSet, new RelicGrabBag(true), ascensionLevel);
			foreach (Player player in players)
			{
				player.InitializeSeed(seed);
				foreach (CardModel cardModel in player.Deck.Cards)
				{
					cardModel.AfterCreated();
				}
			}
			return runState;
		}

		/// <summary>
		/// Load a serialized RunState.
		/// </summary>
		// Token: 0x06000D1B RID: 3355 RVA: 0x00051570 File Offset: 0x0004F770
		public static RunState FromSerializable(SerializableRun save)
		{
			List<SerializablePlayer> players = save.Players;
			IEnumerable<SerializablePlayer> enumerable = players;
			Func<SerializablePlayer, Player> func;
			if ((func = RunState.<>O.<0>__FromSerializable) == null)
			{
				func = (RunState.<>O.<0>__FromSerializable = new Func<SerializablePlayer, Player>(Player.FromSerializable));
			}
			List<Player> list = enumerable.Select<SerializablePlayer, Player>(func).ToList<Player>();
			RunRngSet runRngSet = RunRngSet.FromSave(save.SerializableRng);
			IReadOnlyList<Player> readOnlyList = list;
			IEnumerable<SerializableActModel> acts = save.Acts;
			Func<SerializableActModel, ActModel> func2;
			if ((func2 = RunState.<>O.<1>__FromSave) == null)
			{
				func2 = (RunState.<>O.<1>__FromSave = new Func<SerializableActModel, ActModel>(ActModel.FromSave));
			}
			IReadOnlyList<ActModel> readOnlyList2 = acts.Select<SerializableActModel, ActModel>(func2).ToList<ActModel>();
			IEnumerable<SerializableModifier> modifiers = save.Modifiers;
			Func<SerializableModifier, ModifierModel> func3;
			if ((func3 = RunState.<>O.<2>__FromSerializable) == null)
			{
				func3 = (RunState.<>O.<2>__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable));
			}
			RunState runState = RunState.CreateShared(readOnlyList, readOnlyList2, modifiers.Select<SerializableModifier, ModifierModel>(func3).ToList<ModifierModel>(), save.GameMode, save.CurrentActIndex, runRngSet, RunOddsSet.FromSerializable(save.SerializableOdds, runRngSet.UnknownMapPoint), RelicGrabBag.FromSerializable(save.SerializableSharedRelicGrabBag), save.Ascension);
			runState._visitedMapCoords.AddRange(save.VisitedMapCoords);
			runState._visitedEventIds.UnionWith(save.EventsSeen);
			runState._mapPointHistory.AddRange(new <>z__ReadOnlyArray<List<MapPointHistoryEntry>>(save.MapPointHistory.ToArray()));
			runState.ExtraFields = ExtraRunFields.FromSerializable(save.ExtraFields);
			return runState;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00051694 File Offset: 0x0004F894
		public static RunState CreateForTest([Nullable(new byte[] { 2, 1 })] IReadOnlyList<Player> players = null, [Nullable(new byte[] { 2, 1 })] IReadOnlyList<ActModel> acts = null, [Nullable(new byte[] { 2, 1 })] IReadOnlyList<ModifierModel> modifiers = null, GameMode gameMode = GameMode.Standard, int ascensionLevel = 0, [Nullable(2)] string seed = null)
		{
			if (seed == null)
			{
				seed = SeedHelper.GetRandomSeed(null, 12);
			}
			RunRngSet runRngSet = new RunRngSet(seed);
			RunState runState = RunState.CreateShared(players ?? new <>z__ReadOnlySingleElementList<Player>(Player.CreateForNewRun<Deprived>(UnlockState.all, 1UL)), (acts ?? ActModel.GetDefaultList()).Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), modifiers ?? Array.Empty<ModifierModel>(), gameMode, 0, runRngSet, new RunOddsSet(runRngSet.UnknownMapPoint), new RelicGrabBag(true), ascensionLevel);
			foreach (Player player in runState.Players)
			{
				player.InitializeSeed(seed);
			}
			return runState;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00051768 File Offset: 0x0004F968
		private static RunState CreateShared(IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers, GameMode gameMode, int currentActIndex, RunRngSet rng, RunOddsSet odds, RelicGrabBag sharedRelicGrabBag, int ascensionLevel)
		{
			RunState runState = new RunState(players, acts, modifiers, gameMode, currentActIndex, rng, odds, sharedRelicGrabBag, ascensionLevel);
			foreach (Player player in players)
			{
				player.RunState = runState;
				foreach (CardModel cardModel in player.Deck.Cards)
				{
					runState.AddCard(cardModel, player);
				}
			}
			runState.MultiplayerScalingModel = (MultiplayerScalingModel)ModelDb.Singleton<MultiplayerScalingModel>().MutableClone();
			runState.MultiplayerScalingModel.Initialize(runState);
			runState.BadgeModels = ModelDb.BadgeModels.Select<BadgeModel, BadgeModel>((BadgeModel m) => (BadgeModel)m.MutableClone()).ToList<BadgeModel>();
			return runState;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00051860 File Offset: 0x0004FA60
		private RunState(IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers, GameMode gameMode, int currentActIndex, RunRngSet rng, RunOddsSet odds, RelicGrabBag sharedRelicGrabBag, int ascensionLevel)
		{
			foreach (ActModel actModel in acts)
			{
				actModel.AssertMutable();
			}
			this._players.AddRange(players);
			this.Acts = acts;
			this.Modifiers = modifiers;
			this.GameMode = gameMode;
			this.CurrentActIndex = currentActIndex;
			this.Rng = rng;
			this.Odds = odds;
			this.SharedRelicGrabBag = sharedRelicGrabBag;
			this.UnlockState = new UnlockState(players.Select<Player, UnlockState>((Player p) => p.UnlockState));
			this.AscensionLevel = ascensionLevel;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00051980 File Offset: 0x0004FB80
		public int GetPlayerSlotIndex(Player player)
		{
			return this.Players.IndexOf(player);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00051990 File Offset: 0x0004FB90
		public int GetPlayerSlotIndex(ulong netId)
		{
			return this.Players.FirstIndex<Player>((Player p) => p.NetId == netId);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IPlayerCollection.GetPlayer(System.UInt64)" />.
		/// </summary>
		// Token: 0x06000D21 RID: 3361 RVA: 0x000519C4 File Offset: 0x0004FBC4
		[NullableContext(2)]
		public Player GetPlayer(ulong netId)
		{
			return this.Players.FirstOrDefault<Player>((Player p) => p.NetId == netId);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard``1(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06000D22 RID: 3362 RVA: 0x000519F5 File Offset: 0x0004FBF5
		public T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel
		{
			return (T)((object)this.CreateCard(ModelDb.Card<T>(), owner));
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06000D23 RID: 3363 RVA: 0x00051A10 File Offset: 0x0004FC10
		public CardModel CreateCard(CardModel canonicalCard, Player owner)
		{
			CardModel cardModel = canonicalCard.ToMutable();
			this.AddCard(cardModel, owner);
			cardModel.AfterCreated();
			return cardModel;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CloneCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06000D24 RID: 3364 RVA: 0x00051A34 File Offset: 0x0004FC34
		public CardModel CloneCard(CardModel mutableCard)
		{
			CardModel cardModel = (CardModel)mutableCard.ClonePreservingMutability();
			this.AddCard(cardModel);
			return cardModel;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.AddCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06000D25 RID: 3365 RVA: 0x00051A55 File Offset: 0x0004FC55
		public void AddCard(CardModel card, Player owner)
		{
			if (!card.HasBeenRemovedFromState)
			{
				card.Owner = owner;
			}
			this.AddCard(card);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.RemoveCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06000D26 RID: 3366 RVA: 0x00051A6D File Offset: 0x0004FC6D
		public void RemoveCard(CardModel card)
		{
			this._allCards.Remove(card);
			card.Owner = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IRunState.ContainsCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06000D27 RID: 3367 RVA: 0x00051A83 File Offset: 0x0004FC83
		public bool ContainsCard(CardModel card)
		{
			return this._allCards.Contains(card);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IRunState.LoadCard(MegaCrit.Sts2.Core.Saves.Runs.SerializableCard,MegaCrit.Sts2.Core.Entities.Players.Player)" />
		/// </summary>
		// Token: 0x06000D28 RID: 3368 RVA: 0x00051A94 File Offset: 0x0004FC94
		public CardModel LoadCard(SerializableCard serializableCard, Player owner)
		{
			CardModel cardModel = CardModel.FromSerializable(serializableCard);
			this.AddCard(cardModel, owner);
			return cardModel;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00051AB4 File Offset: 0x0004FCB4
		private void AddCard(CardModel card)
		{
			card.AssertMutable();
			if (!card.HasBeenRemovedFromState)
			{
				this._allCards.Add(card);
				return;
			}
			if (!this.ContainsCard(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to add card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" to RunState that has HasBeenRemovedFromState set as true, but it does not belong to this state!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			card.HasBeenRemovedFromState = false;
		}

		/// <summary>
		/// Add the specified coord to the list of visited map coords.
		/// </summary>
		// Token: 0x06000D2A RID: 3370 RVA: 0x00051B23 File Offset: 0x0004FD23
		public bool AddVisitedMapCoord(MapCoord coord)
		{
			if (this._visitedMapCoords.Contains(coord))
			{
				return false;
			}
			this._visitedMapCoords.Add(coord);
			this.NextRoomId = 0;
			return true;
		}

		/// <summary>
		/// Pop the current room off the stack of rooms that we're in.
		/// </summary>
		/// <returns>The removed room.</returns>
		/// <exception cref="T:System.InvalidOperationException">If we aren't in any rooms.</exception>
		// Token: 0x06000D2B RID: 3371 RVA: 0x00051B4C File Offset: 0x0004FD4C
		public AbstractRoom PopCurrentRoom()
		{
			if (this._currentRooms.Count == 0)
			{
				throw new InvalidOperationException("Not in any rooms.");
			}
			AbstractRoom abstractRoom = this._currentRooms.Last<AbstractRoom>();
			this._currentRooms.RemoveAt(this._currentRooms.Count - 1);
			return abstractRoom;
		}

		/// <summary>
		/// Push the specified room onto the stack of rooms that we're in.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">If we're already in the specified room.</exception>
		// Token: 0x06000D2C RID: 3372 RVA: 0x00051B96 File Offset: 0x0004FD96
		public void PushRoom(AbstractRoom room)
		{
			if (this._currentRooms.Contains(room))
			{
				throw new InvalidOperationException("Already in this room.");
			}
			this._currentRooms.Add(room);
		}

		/// <summary>
		/// Add the specified event to the list of visited events.
		/// </summary>
		// Token: 0x06000D2D RID: 3373 RVA: 0x00051BBD File Offset: 0x0004FDBD
		public void AddVisitedEvent(EventModel eventModel)
		{
			this._visitedEventIds.Add(eventModel.Id);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IRunState.AppendToMapPointHistory(MegaCrit.Sts2.Core.Map.MapPointType,MegaCrit.Sts2.Core.Rooms.RoomType,MegaCrit.Sts2.Core.Models.ModelId)" />.
		/// </summary>
		// Token: 0x06000D2E RID: 3374 RVA: 0x00051BD4 File Offset: 0x0004FDD4
		[NullableContext(2)]
		public void AppendToMapPointHistory(MapPointType mapPointType, RoomType initialRoomType, ModelId roomModelId)
		{
			if (this._mapPointHistory.Count <= this.CurrentActIndex)
			{
				int num = this.CurrentActIndex + 1 - this._mapPointHistory.Count;
				for (int i = 0; i < num; i++)
				{
					this._mapPointHistory.Add(new List<MapPointHistoryEntry>());
				}
			}
			MapPointHistoryEntry mapPointHistoryEntry = new MapPointHistoryEntry(mapPointType, this);
			mapPointHistoryEntry.Rooms.Add(new MapPointRoomHistoryEntry
			{
				RoomType = initialRoomType,
				ModelId = roomModelId
			});
			this._mapPointHistory[this.CurrentActIndex].Add(mapPointHistoryEntry);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IRunState.GetHistoryEntryFor(MegaCrit.Sts2.Core.Runs.MapLocation)" />.
		/// </summary>
		// Token: 0x06000D2F RID: 3375 RVA: 0x00051C64 File Offset: 0x0004FE64
		[NullableContext(2)]
		public MapPointHistoryEntry GetHistoryEntryFor(MapLocation location)
		{
			if (location.actIndex < this._mapPointHistory.Count && location.coord != null)
			{
				int? num = ((location.coord != null) ? new int?(location.coord.GetValueOrDefault().row) : null);
				int count = this._mapPointHistory[location.actIndex].Count;
				if (!((num.GetValueOrDefault() >= count) & (num != null)))
				{
					return this._mapPointHistory[location.actIndex][location.coord.Value.row];
				}
			}
			return null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.IRunState.IterateHookListeners(MegaCrit.Sts2.Core.Combat.ICombatState)" />.
		/// </summary>
		// Token: 0x06000D30 RID: 3376 RVA: 0x00051D14 File Offset: 0x0004FF14
		public IEnumerable<AbstractModel> IterateHookListeners([Nullable(2)] ICombatState childCombatState)
		{
			RunState.<IterateHookListeners>d__118 <IterateHookListeners>d__ = new RunState.<IterateHookListeners>d__118(-2);
			<IterateHookListeners>d__.<>4__this = this;
			<IterateHookListeners>d__.<>3__childCombatState = childCombatState;
			return <IterateHookListeners>d__;
		}

		/// <summary>
		/// Adds a player to the players in the run.
		/// This should only ever be used in debug scenarios - typically players are passed to the run in the constructor.
		/// </summary>
		/// <param name="player">The player to add to the run.</param>
		/// <param name="index">The index at which to add them to the list. -1 to append.</param>
		// Token: 0x06000D31 RID: 3377 RVA: 0x00051D2C File Offset: 0x0004FF2C
		public void AddPlayerDebug(Player player, int index)
		{
			if (index >= 0)
			{
				this._players.Insert(index, player);
			}
			else
			{
				this._players.Add(player);
			}
			player.RunState = this;
			player.InitializeSeed(this.Rng.StringSeed);
			foreach (CardModel cardModel in player.Deck.Cards)
			{
				cardModel.AfterCreated();
			}
			foreach (CardModel cardModel2 in player.Deck.Cards)
			{
				this.AddCard(cardModel2, player);
			}
			MapPointHistoryEntry currentMapPointHistoryEntry = this.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.PlayerStats.Add(new PlayerMapPointHistoryEntry
				{
					PlayerId = player.NetId
				});
			}
			if (RunManager.Instance.IsInProgress)
			{
				player.PopulateRelicGrabBagIfNecessary(this.Rng.UpFront);
				RunManager.Instance.ApplyAscensionEffects(player);
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00051E48 File Offset: 0x00050048
		public void SetActDebug(ActModel act)
		{
			act.AssertMutable();
			List<ActModel> list = this.Acts.ToList<ActModel>();
			list[this.CurrentActIndex] = act;
			this.Acts = list;
		}

		/// <summary>
		/// Removes any visited map coords that don't exist in the given map.
		/// Defends against old multiplayer saves (pre-v0.99.1) that had their SavedMap stripped by a
		/// canonicalization bug, causing map regeneration with different topology.
		/// The root cause is fixed but damaged saves persist.
		/// </summary>
		// Token: 0x06000D33 RID: 3379 RVA: 0x00051E7C File Offset: 0x0005007C
		public void RemoveStaleVisitedMapCoords(ActMap map)
		{
			int num = this._visitedMapCoords.RemoveAll((MapCoord coord) => !map.HasPoint(coord));
			if (num > 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" stale visited map coord(s) that don't exist in the current map");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Clears the list of visited map coords for the current act.
		/// This should only be used in debug scenarios, like when using the dev console to re-enter an act.
		/// </summary>
		// Token: 0x06000D34 RID: 3380 RVA: 0x00051EE9 File Offset: 0x000500E9
		public void ClearVisitedMapCoordsDebug()
		{
			this._visitedMapCoords.Clear();
			this.ActFloor = 0;
		}

		/// <summary>
		/// Adds a modifier to the list of modifiers applied to this run.
		/// This should never be used in a real game run.
		/// </summary>
		// Token: 0x06000D35 RID: 3381 RVA: 0x00051F00 File Offset: 0x00050100
		public void AddModifierDebug(ModifierModel modifier)
		{
			IReadOnlyList<ModifierModel> modifiers = this.Modifiers;
			int num = 0;
			ModifierModel[] array = new ModifierModel[1 + modifiers.Count];
			foreach (ModifierModel modifierModel in modifiers)
			{
				array[num] = modifierModel;
				num++;
			}
			array[num] = modifier;
			this.Modifiers = new <>z__ReadOnlyArray<ModifierModel>(array);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00051F74 File Offset: 0x00050174
		public int GetAndIncrementNextRoomId()
		{
			int nextRoomId = this.NextRoomId;
			int nextRoomId2 = this.NextRoomId;
			this.NextRoomId = nextRoomId2 + 1;
			return nextRoomId;
		}

		/// <summary>
		/// Returns true if the RunState still contains a given model.
		/// Used in hook iteration to determine whether a model was removed by a different model's hook execution.
		/// </summary>
		// Token: 0x06000D37 RID: 3383 RVA: 0x00051F9C File Offset: 0x0005019C
		private static bool Contains(AbstractModel model)
		{
			RelicModel relicModel = model as RelicModel;
			bool flag;
			if (relicModel == null)
			{
				PotionModel potionModel = model as PotionModel;
				if (potionModel == null)
				{
					CardModel cardModel = model as CardModel;
					if (cardModel == null)
					{
						EnchantmentModel enchantmentModel = model as EnchantmentModel;
						if (enchantmentModel == null)
						{
							if (!(model is AchievementModel))
							{
								if (!(model is BadgeModel))
								{
									if (!(model is ModifierModel))
									{
										if (!(model is MultiplayerScalingModel))
										{
											string text = "model";
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
											defaultInterpolatedStringHandler.AppendLiteral("Invalid model type ");
											defaultInterpolatedStringHandler.AppendFormatted<Type>(model.GetType());
											defaultInterpolatedStringHandler.AppendLiteral(" (");
											defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model);
											defaultInterpolatedStringHandler.AppendLiteral(")");
											throw new ArgumentOutOfRangeException(text, model, defaultInterpolatedStringHandler.ToStringAndClear());
										}
										flag = true;
									}
									else
									{
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							flag = enchantmentModel.HasCard && !enchantmentModel.Card.HasBeenRemovedFromState && enchantmentModel.Card.Owner.IsActiveForHooks;
						}
					}
					else
					{
						flag = !cardModel.HasBeenRemovedFromState && cardModel.Owner.IsActiveForHooks;
					}
				}
				else
				{
					flag = !potionModel.HasBeenRemovedFromState && potionModel.Owner.IsActiveForHooks;
				}
			}
			else
			{
				flag = !relicModel.HasBeenRemovedFromState && relicModel.Owner.IsActiveForHooks;
			}
			return flag;
		}

		// Token: 0x04000473 RID: 1139
		private readonly List<Player> _players = new List<Player>();

		// Token: 0x04000475 RID: 1141
		private int _currentActIndex;

		// Token: 0x04000478 RID: 1144
		private readonly List<MapCoord> _visitedMapCoords = new List<MapCoord>();

		// Token: 0x0400047A RID: 1146
		private readonly List<List<MapPointHistoryEntry>> _mapPointHistory = new List<List<MapPointHistoryEntry>>();

		/// <summary>
		/// A stack of all the rooms the player is currently in.
		///
		/// There will usually be exactly one room in here, but here are the reasons it may be a different number:
		/// * There may be 2+ rooms in here when a player makes a choice that spawns a new room without traveling to a new
		///   map point. For example, in the Dense Vegetation event, when the player chooses the Rest option, a new
		///   <see cref="T:MegaCrit.Sts2.Core.Rooms.CombatRoom" /> is pushed to this stack. Then, when that combat is over, we pop that room off the
		///   stack, allowing us to return to the event.
		/// * There may be 0 rooms in here during brief windows, like in the middle of traveling to a new map point.
		///
		/// Whenever we travel to a new map point, this stack is cleared and the first room for that map point is pushed
		/// onto it.
		/// </summary>
		// Token: 0x0400047B RID: 1147
		private readonly List<AbstractRoom> _currentRooms = new List<AbstractRoom>();

		// Token: 0x04000482 RID: 1154
		private readonly HashSet<ModelId> _visitedEventIds = new HashSet<ModelId>();

		/// <summary>
		/// All cards that have been created within this state.
		/// This allows us to keep track of "floating" cards that have not been added to a deck (like card rewards or fake
		/// cards in upgrade previews).
		/// </summary>
		// Token: 0x04000486 RID: 1158
		private readonly List<CardModel> _allCards = new List<CardModel>();

		// Token: 0x02000ECD RID: 3789
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003A70 RID: 14960
			[Nullable(0)]
			public static Func<SerializablePlayer, Player> <0>__FromSerializable;

			// Token: 0x04003A71 RID: 14961
			[Nullable(0)]
			public static Func<SerializableActModel, ActModel> <1>__FromSave;

			// Token: 0x04003A72 RID: 14962
			[Nullable(0)]
			public static Func<SerializableModifier, ModifierModel> <2>__FromSerializable;
		}
	}
}
