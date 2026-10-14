using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000110 RID: 272
	[NullableContext(1)]
	[Nullable(0)]
	public class RunManager : IRunLobbyListener
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0004E687 File Offset: 0x0004C887
		public static RunManager Instance { get; } = new RunManager();

		/// <summary>
		/// The ascension manager for this run.
		/// </summary>
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0004E68E File Offset: 0x0004C88E
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x0004E696 File Offset: 0x0004C896
		public AscensionManager AscensionManager { get; private set; }

		/// <summary>
		/// Whether or not this run should be saved.
		/// This will always be true for normal players.
		/// For developers, it will be true when starting/continuing a run from the main menu, but false when starting
		/// from SceneBootstrapper or from tests.
		/// </summary>
		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0004E69F File Offset: 0x0004C89F
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x0004E6A7 File Offset: 0x0004C8A7
		public bool ShouldSave { get; private set; }

		/// <summary>
		/// If set to true, the final run score will be uploaded to the daily run leaderboard for today.
		/// </summary>
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0004E6B0 File Offset: 0x0004C8B0
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		public DateTimeOffset? DailyTime { get; private set; }

		/// <summary>
		/// Is there currently a run in progress?
		/// True when in a run, false when on the main menu or submenus.
		/// You shouldn't usually have to check this, because most run-dependent things are only executed within the
		/// context of a run, but this is a good escape valve if you need it.
		/// </summary>
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0004E6C1 File Offset: 0x0004C8C1
		public bool IsInProgress
		{
			get
			{
				return this.State != null;
			}
		}

		/// <summary>
		/// Is the run currently being cleaned up? Only true in the brief period of time after the player hits Save and Quit
		/// or abandon and IsInProgress is still true.
		/// IsInProgress maybe should also check this, but I'm scared because we're about to launch.
		/// </summary>
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0004E6CC File Offset: 0x0004C8CC
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0004E6D4 File Offset: 0x0004C8D4
		public bool IsCleaningUp { get; private set; }

		/// <summary>
		/// Discovery order modifications (e.g. boss order or enemy order) is usually disabled in multiplayer and tests.
		/// Set this to true to force them to be applied.
		/// </summary>
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0004E6DD File Offset: 0x0004C8DD
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0004E6E5 File Offset: 0x0004C8E5
		public bool ForceDiscoveryOrderModifications { get; set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0004E6EE File Offset: 0x0004C8EE
		public bool IsGameOver
		{
			get
			{
				return this.IsInProgress && this.State.IsGameOver;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0004E705 File Offset: 0x0004C905
		// (set) Token: 0x06000C50 RID: 3152 RVA: 0x0004E70D File Offset: 0x0004C90D
		public bool IsAbandoned { get; private set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x0004E716 File Offset: 0x0004C916
		// (set) Token: 0x06000C52 RID: 3154 RVA: 0x0004E720 File Offset: 0x0004C920
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				this._isPaused = value;
				RunState state = this.State;
				if (state != null)
				{
					IReadOnlyList<Player> players = state.Players;
					if (players != null && players.Count == 1)
					{
						if (this._isPaused)
						{
							this._activeRunTime += DateTimeOffset.UtcNow.ToUnixTimeSeconds() - this._startOfCurrentActiveRunTime;
							return;
						}
						this._startOfCurrentActiveRunTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
					}
				}
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x0004E78E File Offset: 0x0004C98E
		// (set) Token: 0x06000C54 RID: 3156 RVA: 0x0004E796 File Offset: 0x0004C996
		[Nullable(2)]
		public RunHistory History
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0004E79F File Offset: 0x0004C99F
		// (set) Token: 0x06000C56 RID: 3158 RVA: 0x0004E7A7 File Offset: 0x0004C9A7
		public INetGameService NetService { get; private set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0004E7B0 File Offset: 0x0004C9B0
		// (set) Token: 0x06000C58 RID: 3160 RVA: 0x0004E7B8 File Offset: 0x0004C9B8
		public ChecksumTracker ChecksumTracker { get; private set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0004E7C1 File Offset: 0x0004C9C1
		// (set) Token: 0x06000C5A RID: 3162 RVA: 0x0004E7C9 File Offset: 0x0004C9C9
		public RunLocationTargetedMessageBuffer RunLocationTargetedBuffer { get; private set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0004E7D2 File Offset: 0x0004C9D2
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x0004E7DA File Offset: 0x0004C9DA
		public CombatReplayWriter CombatReplayWriter { get; private set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0004E7E3 File Offset: 0x0004C9E3
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x0004E7EB File Offset: 0x0004C9EB
		[Nullable(2)]
		public RunLobby RunLobby
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0004E7F4 File Offset: 0x0004C9F4
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x0004E7FC File Offset: 0x0004C9FC
		public CombatStateSynchronizer CombatStateSynchronizer { get; private set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0004E805 File Offset: 0x0004CA05
		// (set) Token: 0x06000C62 RID: 3170 RVA: 0x0004E80D File Offset: 0x0004CA0D
		public MapSelectionSynchronizer MapSelectionSynchronizer { get; private set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0004E816 File Offset: 0x0004CA16
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x0004E81E File Offset: 0x0004CA1E
		public ActChangeSynchronizer ActChangeSynchronizer { get; private set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0004E827 File Offset: 0x0004CA27
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x0004E82F File Offset: 0x0004CA2F
		public PlayerChoiceSynchronizer PlayerChoiceSynchronizer { get; private set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0004E838 File Offset: 0x0004CA38
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x0004E840 File Offset: 0x0004CA40
		public EventSynchronizer EventSynchronizer { get; private set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0004E849 File Offset: 0x0004CA49
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x0004E851 File Offset: 0x0004CA51
		public RewardSynchronizer RewardSynchronizer { get; private set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0004E85A File Offset: 0x0004CA5A
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x0004E862 File Offset: 0x0004CA62
		public RewardsSetSynchronizer RewardsSetSynchronizer { get; private set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0004E86B File Offset: 0x0004CA6B
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0004E873 File Offset: 0x0004CA73
		public RestSiteSynchronizer RestSiteSynchronizer { get; private set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004E87C File Offset: 0x0004CA7C
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0004E884 File Offset: 0x0004CA84
		public OneOffSynchronizer OneOffSynchronizer { get; private set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0004E88D File Offset: 0x0004CA8D
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x0004E895 File Offset: 0x0004CA95
		public TreasureRoomRelicSynchronizer TreasureRoomRelicSynchronizer { get; private set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0004E89E File Offset: 0x0004CA9E
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0004E8A6 File Offset: 0x0004CAA6
		public FlavorSynchronizer FlavorSynchronizer { get; private set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0004E8AF File Offset: 0x0004CAAF
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x0004E8B7 File Offset: 0x0004CAB7
		public PeerInputSynchronizer InputSynchronizer { get; private set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0004E8C0 File Offset: 0x0004CAC0
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x0004E8C8 File Offset: 0x0004CAC8
		public HoveredModelTracker HoveredModelTracker { get; private set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0004E8D1 File Offset: 0x0004CAD1
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x0004E8D9 File Offset: 0x0004CAD9
		public ActionQueueSet ActionQueueSet { get; private set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0004E8E2 File Offset: 0x0004CAE2
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0004E8EA File Offset: 0x0004CAEA
		public ActionExecutor ActionExecutor { get; private set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0004E8F3 File Offset: 0x0004CAF3
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x0004E8FB File Offset: 0x0004CAFB
		public ActionQueueSynchronizer ActionQueueSynchronizer { get; private set; }

		/// <summary>
		/// The run time when you beat the Act 3 boss.
		/// </summary>
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0004E904 File Offset: 0x0004CB04
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x0004E90C File Offset: 0x0004CB0C
		public long WinTime { get; set; }

		/// <summary>
		/// The total play time of the current run. If the run has been won, will use the WinTime instead.
		/// </summary>
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0004E918 File Offset: 0x0004CB18
		public long RunTime
		{
			get
			{
				if (this.WinTime > 0L)
				{
					return this.WinTime;
				}
				if (this.IsPaused)
				{
					RunState state = this.State;
					if (state != null)
					{
						IReadOnlyList<Player> players = state.Players;
						if (players != null && players.Count == 1)
						{
							return this._activeRunTime + this._prevSessionRunTime;
						}
					}
				}
				return DateTimeOffset.UtcNow.ToUnixTimeSeconds() - this._startOfCurrentActiveRunTime + this._activeRunTime + this._prevSessionRunTime;
			}
		}

		/// <summary>
		/// This flag returns true if we are in a singleplayer session or we are faking a multiplayer session.
		/// Sometimes, for testing, we add dummy players through the bootstrapper. In those cases, we still want end turn
		/// and other functions not to wait for multiple players, because there's only one acting player.
		/// </summary>
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0004E98B File Offset: 0x0004CB8B
		public bool IsSingleplayerOrFakeMultiplayer
		{
			get
			{
				return this.IsInProgress && this.NetService.Type == NetGameType.Singleplayer;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0004E9A5 File Offset: 0x0004CBA5
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x0004E9AD File Offset: 0x0004CBAD
		[Nullable(2)]
		public SerializableMapDrawings MapDrawingsToLoad
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// Saved maps from a loaded run, indexed by act index.
		/// Used to restore exact map topology instead of regenerating from RNG.
		/// Cleared after use to allow relics to regenerate maps normally.
		/// </summary>
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0004E9B6 File Offset: 0x0004CBB6
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x0004E9BE File Offset: 0x0004CBBE
		[Nullable(new byte[] { 2, 1 })]
		public Dictionary<int, SerializableActMap> SavedMapsToLoad
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000C87 RID: 3207 RVA: 0x0004E9C8 File Offset: 0x0004CBC8
		// (remove) Token: 0x06000C88 RID: 3208 RVA: 0x0004EA00 File Offset: 0x0004CC00
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<RunState> RunStarted;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000C89 RID: 3209 RVA: 0x0004EA38 File Offset: 0x0004CC38
		// (remove) Token: 0x06000C8A RID: 3210 RVA: 0x0004EA70 File Offset: 0x0004CC70
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action RoomEntered;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000C8B RID: 3211 RVA: 0x0004EAA8 File Offset: 0x0004CCA8
		// (remove) Token: 0x06000C8C RID: 3212 RVA: 0x0004EAE0 File Offset: 0x0004CCE0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action RoomExited;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000C8D RID: 3213 RVA: 0x0004EB18 File Offset: 0x0004CD18
		// (remove) Token: 0x06000C8E RID: 3214 RVA: 0x0004EB50 File Offset: 0x0004CD50
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action ActEntered;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000C8F RID: 3215 RVA: 0x0004EB88 File Offset: 0x0004CD88
		// (remove) Token: 0x06000C90 RID: 3216 RVA: 0x0004EBC0 File Offset: 0x0004CDC0
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Func<Task> TestFadeOut;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000C91 RID: 3217 RVA: 0x0004EBF8 File Offset: 0x0004CDF8
		// (remove) Token: 0x06000C92 RID: 3218 RVA: 0x0004EC30 File Offset: 0x0004CE30
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Func<Task> TestFadeIn;

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0004EC65 File Offset: 0x0004CE65
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0004EC6D File Offset: 0x0004CE6D
		[Nullable(2)]
		private RunState State
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0004EC76 File Offset: 0x0004CE76
		private RunManager()
		{
		}

		/// <summary>
		/// Set up a brand-new singleplayer run.
		/// This includes running initialization code for things that should happen at the start of a run (obtaining the
		/// characters' starting deck and relic, setting an empty potion belt, etc.).
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="shouldSave">
		/// Whether or not the run should be saved to disk.
		/// True during normal gameplay, false during tests and bootstrap.
		/// </param>
		/// <param name="dailyTime">
		/// If non-null, then the final run score will be uploaded to the daily run leaderboard for the passed time.
		/// </param>
		// Token: 0x06000C96 RID: 3222 RVA: 0x0004EC80 File Offset: 0x0004CE80
		public void SetUpNewSingleplayer(RunState state, bool shouldSave, DateTimeOffset? dailyTime = null)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			INetGameService netGameService = new NetSingleplayerGameService();
			this.InitializeShared(netGameService, new PeerInputSynchronizer(netGameService), shouldSave, dailyTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
			this.InitializeRunLobby(netGameService, state);
			this.InitializeNewRun();
			this.GenerateRooms();
		}

		/// <summary>
		/// Set up a brand-new multiplayer run.
		/// This includes running initialization code for things that should happen at the start of a run (obtaining the
		/// characters' starting deck and relic, setting an empty potion belt, etc.).
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="lobby">The multiplayer lobby containing the players that will go on the run together.</param>
		/// <param name="shouldSave">
		/// Whether or not the run should be saved to disk.
		/// True during normal gameplay, false during tests and bootstrap.
		/// </param>
		/// <param name="dailyTime">
		/// If non-null, then the final run score will be uploaded to the daily run leaderboard for the passed time.
		/// </param>
		// Token: 0x06000C97 RID: 3223 RVA: 0x0004ECE4 File Offset: 0x0004CEE4
		public void SetUpNewMultiplayer(RunState state, StartRunLobby lobby, bool shouldSave, DateTimeOffset? dailyTime = null)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			this.InitializeShared(lobby.NetService, lobby.InputSynchronizer, shouldSave, dailyTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
			this.InitializeRunLobby(lobby.NetService, state);
			this.InitializeNewRun();
			this.GenerateRooms();
		}

		/// <summary>
		/// Set up a singleplayer run that's been loaded from a save file.
		/// No start-of-run initialization code will be run here, since we're loading an existing state.
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="save">
		/// The serialized version of the run. This may contain extra data that's not part of the deserialized RunState.
		/// </param>
		// Token: 0x06000C98 RID: 3224 RVA: 0x0004ED4C File Offset: 0x0004CF4C
		public async Task SetUpSavedSingleplayer(RunState state, SerializableRun save)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			await SaveManager.Instance.IncrementNumReloads(save, NetGameType.Singleplayer, false);
			INetGameService netGameService = new NetSingleplayerGameService();
			this.InitializeShared(netGameService, new PeerInputSynchronizer(netGameService), true, save.DailyTime, save.StartTime, save.RunTime, save.WinTime, save.NumReloads);
			this.InitializeRunLobby(netGameService, state);
			this.InitializeSavedRun(save);
		}

		/// <summary>
		/// Set up a multiplayer run that's been loaded from a save file.
		/// No start-of-run initialization code will be run here, since we're loading an existing state.
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="lobby">
		/// The multiplayer lobby containing the players that will go on the run together.
		/// The lobby also contains the serialized version of the run. This may contain extra data that's not part of the
		/// deserialized RunState.
		/// </param>
		// Token: 0x06000C99 RID: 3225 RVA: 0x0004EDA0 File Offset: 0x0004CFA0
		public async Task SetUpSavedMultiplayer(RunState state, LoadRunLobby lobby)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			SerializableRun save = lobby.Run;
			await SaveManager.Instance.IncrementNumReloads(save, lobby.NetService.Type, false);
			this.InitializeShared(lobby.NetService, lobby.InputSynchronizer, true, save.DailyTime, save.StartTime, save.RunTime, save.WinTime, save.NumReloads);
			this.InitializeRunLobby(lobby.NetService, state);
			this.InitializeSavedRun(save);
		}

		/// <summary>
		/// Set up a run that's been loaded from a CombatReplay file.
		/// No start-of-run initialization code will be run here, since we're loading an existing state.
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="replay">
		/// CombatReplay that is being replayed in this run.
		/// The replay also contains the serialized version of the run. This may contain extra data that's not part of the
		/// deserialized RunState.
		/// </param>
		/// <param name="playerIdToLoad">The player ID to pretend that we are. UI will be displayed for this player.</param>
		// Token: 0x06000C9A RID: 3226 RVA: 0x0004EDF4 File Offset: 0x0004CFF4
		public void SetUpReplay(RunState state, CombatReplay replay, ulong playerIdToLoad)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			SerializableRun serializableRun = replay.serializableRun;
			NetReplayGameService netReplayGameService = new NetReplayGameService(playerIdToLoad);
			this.InitializeShared(netReplayGameService, new PeerInputSynchronizer(netReplayGameService), true, serializableRun.DailyTime, serializableRun.StartTime, serializableRun.RunTime, serializableRun.WinTime, serializableRun.NumReloads);
			this.InitializeRunLobby(netReplayGameService, state);
			this.InitializeSavedRun(serializableRun);
		}

		/// <summary>
		/// Set up a brand-new run to be used in an automated test. Should only be used in the test project.
		/// This includes running initialization code for things that should happen at the start of a run (obtaining the
		/// characters' starting deck and relic, setting an empty potion belt, etc.).
		/// </summary>
		/// <param name="state">RunState that should be used for the run.</param>
		/// <param name="gameService">The mock INetGameService to use.</param>
		/// <param name="disableCombatStateSync">
		/// Whether or not combat state synchronization should be disabled. This is only relevant if you are testing
		/// multiplayer scenarios.
		/// </param>
		/// <param name="shouldSave">
		/// If true, saving will be performed. Remember to call <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.MockInstanceForTesting(MegaCrit.Sts2.Core.Saves.SaveManager)" />.
		/// </param>
		// Token: 0x06000C9B RID: 3227 RVA: 0x0004EE64 File Offset: 0x0004D064
		public void SetUpTest(RunState state, INetGameService gameService, bool disableCombatStateSync = true, bool shouldSave = false)
		{
			if (this.State != null)
			{
				throw new InvalidOperationException("State is already set.");
			}
			this.State = state;
			this.InitializeShared(gameService, new PeerInputSynchronizer(gameService), shouldSave, null, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
			this.InitializeRunLobby(gameService, state);
			this.CombatStateSynchronizer.IsDisabled = disableCombatStateSync;
			this.InitializeNewRun();
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0004EED0 File Offset: 0x0004D0D0
		private void InitializeShared(INetGameService netService, PeerInputSynchronizer inputSynchronizer, bool shouldSave, DateTimeOffset? dailyTime, long startTime, long runTime, long winTime, int numReloads)
		{
			if (this.State == null)
			{
				throw new InvalidOperationException("State is not set.");
			}
			this.NetService = netService;
			ulong netId = this.NetService.NetId;
			this.ChecksumTracker = new ChecksumTracker(this.NetService, this.State);
			this.ChecksumTracker.IsEnabled = !TestMode.IsOn;
			this.RunLocationTargetedBuffer = new RunLocationTargetedMessageBuffer(this.NetService);
			this.FlavorSynchronizer = new FlavorSynchronizer(this.NetService, this.State, netId);
			this.ActionQueueSet = new ActionQueueSet(this.State.Players);
			this.ActionExecutor = new ActionExecutor(this.ActionQueueSet);
			this.ActionQueueSynchronizer = new ActionQueueSynchronizer(this.State, this.ActionQueueSet, this.RunLocationTargetedBuffer, this.NetService);
			this.PlayerChoiceSynchronizer = new PlayerChoiceSynchronizer(this.NetService, this.State);
			this.MapSelectionSynchronizer = new MapSelectionSynchronizer(this.NetService, this.ActionQueueSynchronizer, this.State);
			this.ActChangeSynchronizer = new ActChangeSynchronizer(this.State);
			this.EventSynchronizer = new EventSynchronizer(this.RunLocationTargetedBuffer, this.NetService, this.State, this.State, netId, this.State.Rng.Seed);
			this.RewardSynchronizer = new RewardSynchronizer(this.RunLocationTargetedBuffer, this.NetService, this.State, netId);
			this.RewardsSetSynchronizer = new RewardsSetSynchronizer(this.RunLocationTargetedBuffer, this.NetService, this.State, netId);
			this.OneOffSynchronizer = new OneOffSynchronizer(this.RunLocationTargetedBuffer, this.NetService, this.State, netId);
			this.TreasureRoomRelicSynchronizer = new TreasureRoomRelicSynchronizer(this.State, netId, this.ActionQueueSynchronizer, this.State.SharedRelicGrabBag, this.State.Rng.TreasureRoomRelics);
			this.CombatReplayWriter = new CombatReplayWriter(this.PlayerChoiceSynchronizer, this.RewardsSetSynchronizer, this.ActionQueueSet, this.ActionQueueSynchronizer, this.ChecksumTracker);
			this.CombatReplayWriter.IsEnabled = !TestMode.IsOn;
			this.ActionExecutor.JustBeforeActionFinishedExecuting += this.SendPostActionChecksum;
			this.ChecksumTracker.StateDiverged += this.StateDiverged;
			this.ActionExecutor.Pause();
			this.IsAbandoned = false;
			this.AscensionManager = new AscensionManager(this.State.AscensionLevel);
			this.ShouldSave = shouldSave;
			this.DailyTime = dailyTime;
			this._startTime = startTime;
			this._prevSessionRunTime = runTime;
			this._activeRunTime = 0L;
			this.WinTime = winTime;
			this._numReloads = numReloads;
			this._startOfCurrentActiveRunTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			this.InputSynchronizer = inputSynchronizer;
			this.HoveredModelTracker = new HoveredModelTracker(this.InputSynchronizer, this.State);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0004F1A0 File Offset: 0x0004D3A0
		private void InitializeRunLobby(INetGameService netService, RunState state)
		{
			if (netService.Type.IsMultiplayer())
			{
				this.RunLobby = new RunLobby(state.GameMode, netService, this, state, state.Players.Select<Player, ulong>((Player p) => p.NetId));
				this.RunLobby.RemotePlayerDisconnected += this.RemotePlayerDisconnected;
			}
			this.CombatStateSynchronizer = new CombatStateSynchronizer(this.NetService, this.RunLobby, state);
			this.RestSiteSynchronizer = new RestSiteSynchronizer(this.RunLocationTargetedBuffer, this.NetService, state, this.NetService.NetId, this.RunLobby);
		}

		/// <summary>
		/// Call this when starting a brand new run, not when loading a saved run.
		/// </summary>
		// Token: 0x06000C9E RID: 3230 RVA: 0x0004F250 File Offset: 0x0004D450
		private void InitializeNewRun()
		{
			this.State.SharedRelicGrabBag.Populate(ModelDb.RelicPool<SharedRelicPool>().GetUnlockedRelics(this.State.UnlockState), this.State.Rng.UpFront);
			foreach (Player player in this.State.Players)
			{
				player.PopulateRelicGrabBagIfNecessary(this.State.Rng.UpFront);
			}
			this.SetStartedWithNeowFlag();
			foreach (ModifierModel modifierModel in this.State.Modifiers)
			{
				modifierModel.OnRunCreated(this.State);
			}
			foreach (Player player2 in this.State.Players)
			{
				this.ApplyAscensionEffects(player2);
			}
		}

		/// <summary>
		/// Call this when loading a saved run, not when starting a brand new run.
		/// </summary>
		// Token: 0x06000C9F RID: 3231 RVA: 0x0004F37C File Offset: 0x0004D57C
		private void InitializeSavedRun(SerializableRun save)
		{
			foreach (ActModel actModel in this.State.Acts)
			{
				actModel.ValidateRoomsAfterLoad(this.State.Rng.UpFront);
			}
			this.AfterMapLocationChanged();
			this.MapDrawingsToLoad = save.MapDrawings;
			this.SavedMapsToLoad = null;
			for (int i = 0; i < save.Acts.Count; i++)
			{
				SerializableActMap savedMap = save.Acts[i].SavedMap;
				if (savedMap != null)
				{
					if (this.SavedMapsToLoad == null)
					{
						this.SavedMapsToLoad = new Dictionary<int, SerializableActMap>();
					}
					this.SavedMapsToLoad[i] = savedMap;
				}
			}
			foreach (ModifierModel modifierModel in this.State.Modifiers)
			{
				modifierModel.OnRunLoaded(this.State);
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0004F494 File Offset: 0x0004D694
		private void SendPostActionChecksum(GameAction action)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			bool flag = action is EndPlayerTurnAction || action is ReadyToBeginEnemyTurnAction;
			if (flag)
			{
				return;
			}
			ChecksumTracker checksumTracker = this.ChecksumTracker;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("finished action execution ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
			checksumTracker.GenerateChecksum(defaultInterpolatedStringHandler.ToStringAndClear(), action);
		}

		/// <summary>
		/// Call this when we start a new run to set the StartedWithNeow extra field.
		/// </summary>
		// Token: 0x06000CA1 RID: 3233 RVA: 0x0004F4FA File Offset: 0x0004D6FA
		private void SetStartedWithNeowFlag()
		{
			this.State.ExtraFields.StartedWithNeow = this.State.UnlockState.IsEpochRevealed<NeowEpoch>();
		}

		/// <summary>
		/// Call this to validate a save can be loaded without actually creating a run.
		/// This performs deep validation by instantiating save components to ensure all data is valid.
		///
		/// This validation occurs AFTER JSON parsing and migration, so the save file has already been
		/// successfully deserialized. However, the content may still be invalid due to:
		/// - Removed modifiers that exist in the save
		/// - Invalid card/relic/potion IDs
		/// - Corrupted game state data
		///
		/// If the save has any deprecated content in it, the save will be mutated so that the content is replaced with its
		/// deprecated version. This is done so that it can be sent over the network correctly in multiplayer scenarios.
		///
		/// Unlike MigrationManager.LoadSave(), this method does not automatically handle corruption.
		/// Callers must handle exceptions and rename corrupt files as needed.
		/// </summary>
		/// <param name="save">The save to attempt to load.</param>
		/// <param name="localPlayerId">The player ID of the local (hosting) player. If it is not in the save file, an
		/// exception is thrown.</param>
		// Token: 0x06000CA2 RID: 3234 RVA: 0x0004F51C File Offset: 0x0004D71C
		public static SerializableRun CanonicalizeSave(SerializableRun save, ulong localPlayerId)
		{
			if (save.Players.FirstOrDefault<SerializablePlayer>((SerializablePlayer p) => p.NetId == localPlayerId) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(90, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Save is invalid! Players does not contain local player Id. IDs in save file: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<ulong>(",", save.Players.Select<SerializablePlayer, ulong>((SerializablePlayer p) => p.NetId)));
				defaultInterpolatedStringHandler.AppendLiteral(". Local ID: ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(localPlayerId);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RunState runState = RunState.FromSerializable(save);
			int latestSchemaVersion = SaveManager.Instance.GetLatestSchemaVersion<SerializableRun>();
			SerializableRun serializableRun = new SerializableRun();
			serializableRun.SchemaVersion = latestSchemaVersion;
			serializableRun.Acts = runState.Acts.Zip<ActModel, SerializableActModel, SerializableActModel>(save.Acts, delegate(ActModel act, SerializableActModel savedAct)
			{
				SerializableActModel serializableActModel = act.ToSave();
				serializableActModel.SavedMap = savedAct.SavedMap;
				return serializableActModel;
			}).ToList<SerializableActModel>();
			serializableRun.Modifiers = runState.Modifiers.Select<ModifierModel, SerializableModifier>((ModifierModel m) => m.ToSerializable()).ToList<SerializableModifier>();
			serializableRun.DailyTime = save.DailyTime;
			serializableRun.GameMode = runState.GameMode;
			serializableRun.CurrentActIndex = runState.CurrentActIndex;
			serializableRun.EventsSeen = runState.VisitedEventIds.ToList<ModelId>();
			serializableRun.SerializableOdds = runState.Odds.ToSerializable();
			serializableRun.SerializableSharedRelicGrabBag = runState.SharedRelicGrabBag.ToSerializable();
			serializableRun.Players = runState.Players.Select<Player, SerializablePlayer>((Player p) => p.ToSerializable()).ToList<SerializablePlayer>();
			serializableRun.SerializableRng = runState.Rng.ToSerializable();
			serializableRun.VisitedMapCoords = runState.VisitedMapCoords.ToList<MapCoord>();
			serializableRun.MapPointHistory = runState.MapPointHistory.Select<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((IReadOnlyList<MapPointHistoryEntry> l) => l.ToList<MapPointHistoryEntry>()).ToList<List<MapPointHistoryEntry>>();
			serializableRun.SaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			serializableRun.StartTime = save.StartTime;
			serializableRun.RunTime = save.RunTime;
			serializableRun.WinTime = save.WinTime;
			serializableRun.Ascension = runState.AscensionLevel;
			serializableRun.PlatformType = save.PlatformType;
			serializableRun.MapDrawings = save.MapDrawings;
			serializableRun.NumReloads = save.NumReloads;
			serializableRun.ExtraFields = runState.ExtraFields.ToSerializable();
			serializableRun.PreFinishedRoom = save.PreFinishedRoom;
			SerializableRun serializableRun2 = serializableRun;
			PacketWriter packetWriter = new PacketWriter();
			packetWriter.Write<SerializableRun>(serializableRun2);
			return serializableRun2;
		}

		/// <summary>
		/// Builds a blacklist of room types that should not be rolled for an Unknown map point, based on the connected map
		/// points.
		/// </summary>
		/// <param name="previousMapPointEntry">
		/// The history entry for the previous map point.
		/// Null before we've visited the second map point.
		/// </param>
		/// <param name="nextMapPoints">
		/// The next map points from the current one.
		/// Empty before we've visited the first map point, and after visiting the boss map point.
		/// </param>
		/// <returns>A set of room types that should be excluded when rolling.</returns>
		// Token: 0x06000CA3 RID: 3235 RVA: 0x0004F7DC File Offset: 0x0004D9DC
		public static HashSet<RoomType> BuildRoomTypeBlacklist([Nullable(2)] MapPointHistoryEntry previousMapPointEntry, IReadOnlyCollection<MapPoint> nextMapPoints)
		{
			HashSet<RoomType> hashSet = new HashSet<RoomType>();
			bool flag;
			if (previousMapPointEntry == null || !previousMapPointEntry.HasRoomOfType(RoomType.Shop))
			{
				if (nextMapPoints.Count > 0)
				{
					flag = nextMapPoints.All<MapPoint>((MapPoint p) => p.PointType == MapPointType.Shop);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				hashSet.Add(RoomType.Shop);
			}
			return hashSet;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0004F83C File Offset: 0x0004DA3C
		public SerializableRun ToSave([Nullable(2)] AbstractRoom preFinishedRoom)
		{
			int latestSchemaVersion = SaveManager.Instance.GetLatestSchemaVersion<SerializableRun>();
			List<SerializableActModel> list = new List<SerializableActModel>();
			for (int i = 0; i < this.State.Acts.Count; i++)
			{
				SerializableActModel serializableActModel = this.State.Acts[i].ToSave();
				if (i == this.State.CurrentActIndex && this.State.Map != null)
				{
					serializableActModel.SavedMap = SerializableActMap.FromActMap(this.State.Map);
				}
				list.Add(serializableActModel);
			}
			SerializableRun serializableRun = new SerializableRun();
			serializableRun.SchemaVersion = latestSchemaVersion;
			serializableRun.Acts = list;
			serializableRun.Modifiers = this.State.Modifiers.Select<ModifierModel, SerializableModifier>((ModifierModel m) => m.ToSerializable()).ToList<SerializableModifier>();
			serializableRun.DailyTime = this.DailyTime;
			serializableRun.CurrentActIndex = this.State.CurrentActIndex;
			serializableRun.EventsSeen = this.State.VisitedEventIds.ToList<ModelId>();
			serializableRun.GameMode = this.State.GameMode;
			serializableRun.SerializableOdds = this.State.Odds.ToSerializable();
			serializableRun.SerializableSharedRelicGrabBag = this.State.SharedRelicGrabBag.ToSerializable();
			serializableRun.Players = this.State.Players.Select<Player, SerializablePlayer>((Player p) => p.ToSerializable()).ToList<SerializablePlayer>();
			serializableRun.SerializableRng = this.State.Rng.ToSerializable();
			serializableRun.VisitedMapCoords = this.State.VisitedMapCoords.ToList<MapCoord>();
			serializableRun.MapPointHistory = this.State.MapPointHistory.Select<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((IReadOnlyList<MapPointHistoryEntry> l) => l.ToList<MapPointHistoryEntry>()).ToList<List<MapPointHistoryEntry>>();
			serializableRun.SaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			serializableRun.StartTime = this._startTime;
			serializableRun.RunTime = this.RunTime;
			serializableRun.WinTime = this.WinTime;
			serializableRun.NumReloads = this._numReloads;
			serializableRun.Ascension = this.State.AscensionLevel;
			serializableRun.PlatformType = this.NetService.Platform;
			NRun instance = NRun.Instance;
			serializableRun.MapDrawings = ((instance != null) ? instance.GlobalUi.MapScreen.Drawings.GetSerializableMapDrawings() : null);
			serializableRun.ExtraFields = this.State.ExtraFields.ToSerializable();
			serializableRun.PreFinishedRoom = ((preFinishedRoom != null) ? preFinishedRoom.ToSerializable() : null);
			return serializableRun;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0004FADC File Offset: 0x0004DCDC
		public RunState Launch()
		{
			LocalContext.NetId = new ulong?(this.NetService.NetId);
			this.NetService.SetBufferMessages(false);
			Action<RunState> runStarted = this.RunStarted;
			if (runStarted != null)
			{
				runStarted(this.State);
			}
			this.UpdateRichPresence();
			return this.State;
		}

		/// <summary>
		/// Finalize the relics at the start of a run.
		/// This is called when creating a new run, but not before loading a saved run, since
		/// <see cref="M:MegaCrit.Sts2.Core.Models.RelicModel.AfterObtained" /> is not idempotent.
		/// </summary>
		// Token: 0x06000CA6 RID: 3238 RVA: 0x0004FB30 File Offset: 0x0004DD30
		public async Task FinalizeStartingRelics()
		{
			if (this.State != null)
			{
				foreach (Player player in this.State.Players)
				{
					foreach (RelicModel relicModel in player.Relics)
					{
						await relicModel.AfterObtained();
					}
					IEnumerator<RelicModel> enumerator2 = null;
				}
				IEnumerator<Player> enumerator = null;
			}
		}

		/// <summary>
		/// Initialize the rooms for the run. Don't call this in Start so we can skip it in tests.
		/// </summary>
		// Token: 0x06000CA7 RID: 3239 RVA: 0x0004FB74 File Offset: 0x0004DD74
		public void GenerateRooms()
		{
			List<AncientEventModel> list = this.State.UnlockState.SharedAncients.ToList<AncientEventModel>().UnstableShuffle<AncientEventModel>(this.State.Rng.UpFront);
			foreach (ActModel actModel in this.State.Acts.Skip<ActModel>(1))
			{
				int num = this.State.Rng.UpFront.NextInt(list.Count + 1);
				List<AncientEventModel> list2 = list.Take<AncientEventModel>(num).ToList<AncientEventModel>();
				list = list.Except<AncientEventModel>(list2).ToList<AncientEventModel>();
				actModel.SetSharedAncientSubset(list2);
			}
			for (int i = 0; i < this.State.Acts.Count; i++)
			{
				ActModel act = this.State.Acts[i];
				act.GenerateRooms(this.State.Rng.UpFront, this.State.UnlockState, this.State.Players.Count > 1);
				if (this.ShouldApplyTutorialModifications())
				{
					act.ApplyDiscoveryOrderModifications(this.State.UnlockState);
				}
				bool flag = i == this.State.Acts.Count - 1;
				if (flag && this.AscensionManager.HasLevel(AscensionLevel.DoubleBoss))
				{
					EncounterModel encounterModel = this.State.Rng.UpFront.NextItem<EncounterModel>(act.AllBossEncounters.Where<EncounterModel>((EncounterModel e) => e.Id != act.BossEncounter.Id));
					act.SetSecondBossEncounter(encounterModel);
				}
			}
		}

		/// <summary>
		/// Returns true if tutorial modifications should be applied to the current run. False otherwise.
		/// Takes into account things like game mode, test mode, and singleplayer/multiplayer.
		/// </summary>
		// Token: 0x06000CA8 RID: 3240 RVA: 0x0004FD40 File Offset: 0x0004DF40
		public bool ShouldApplyTutorialModifications()
		{
			return this.ForceDiscoveryOrderModifications || (!TestMode.IsOn && this.State != null && this.State.GameMode == GameMode.Standard);
		}

		/// <summary>
		/// Generate a new map.
		/// This is called at the start of the run, between each act, and when relics create an entirely new map.
		/// Don't call this in Start so we can skip it in tests.
		/// </summary>
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0004FD70 File Offset: 0x0004DF70
		public async Task GenerateMap()
		{
			if (this.State == null)
			{
				throw new InvalidOperationException("State is not set.");
			}
			this.MapSelectionSynchronizer.BeforeMapGenerated();
			SerializableActMap serializableActMap;
			ActMap map;
			if (this.SavedMapsToLoad != null && this.SavedMapsToLoad.TryGetValue(this.State.CurrentActIndex, out serializableActMap))
			{
				map = new SavedActMap(serializableActMap);
				this.SavedMapsToLoad.Remove(this.State.CurrentActIndex);
				if (this.SavedMapsToLoad.Count == 0)
				{
					this.SavedMapsToLoad = null;
				}
				map = Hook.ModifyGeneratedMapLate(this.State, map, this.State.CurrentActIndex);
				await Hook.AfterMapGenerated(this.State, map, this.State.CurrentActIndex);
			}
			else
			{
				ActMap actMap = this.State.Act.CreateMap(this.State, false);
				map = Hook.ModifyGeneratedMap(this.State, actMap, this.State.CurrentActIndex);
				await Hook.AfterMapGenerated(this.State, map, this.State.CurrentActIndex);
				if (!this.State.ExtraFields.StartedWithNeow && this.State.CurrentActIndex == 0)
				{
					map.StartingMapPoint.PointType = MapPointType.Monster;
				}
			}
			this.State.Map = map;
			this.State.RemoveStaleVisitedMapCoords(map);
			NMapScreen instance = NMapScreen.Instance;
			if (instance != null)
			{
				instance.SetMap(map, this.State.Rng.Seed, true);
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0004FDB3 File Offset: 0x0004DFB3
		public Task EnterMapCoord(MapCoord coord)
		{
			if (this.State == null)
			{
				return Task.CompletedTask;
			}
			if (!this.State.AddVisitedMapCoord(coord))
			{
				return Task.CompletedTask;
			}
			return this.EnterMapCoordInternal(coord, null, true);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0004FDE0 File Offset: 0x0004DFE0
		public async Task LoadIntoLatestMapCoord([Nullable(2)] AbstractRoom preFinishedRoom)
		{
			if (this.State != null)
			{
				if (this.State.VisitedMapCoords.Count > 0)
				{
					IReadOnlyList<MapCoord> visitedMapCoords = this.State.VisitedMapCoords;
					await this.EnterMapCoordInternal(visitedMapCoords[visitedMapCoords.Count - 1], preFinishedRoom, false);
				}
				else
				{
					await this.EnterRoomInternal(new MapRoom(), false);
				}
			}
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0004FE2C File Offset: 0x0004E02C
		private Task EnterMapCoordInternal(MapCoord coord, [Nullable(2)] AbstractRoom preFinishedRoom, bool saveGame)
		{
			if (this.State == null)
			{
				return Task.CompletedTask;
			}
			MapPoint point = this.State.Map.GetPoint(coord);
			return this.EnterMapPointInternal(coord.row + 1, point.PointType, preFinishedRoom, saveGame);
		}

		/// <summary>
		/// WARNING: This should only be called by <see cref="T:MegaCrit.Sts2.Core.Runs.RunManager" /> and in tests.
		/// </summary>
		// Token: 0x06000CAD RID: 3245 RVA: 0x0004FE70 File Offset: 0x0004E070
		public async Task EnterMapPointInternal(int actFloor, MapPointType pointType, [Nullable(2)] AbstractRoom preFinishedRoom, bool saveGame)
		{
			if (this.State != null)
			{
				using (new NetLoadingHandle(this.NetService))
				{
					if (this.State.MapPointHistory.Count > 0)
					{
						this.UpdatePlayerStatsInMapPointHistory();
					}
					this.State.ActFloor = actFloor;
					await this.ExitCurrentRooms();
					if (preFinishedRoom == null)
					{
						this.CombatStateSynchronizer.StartSync();
					}
					this.ClearScreens();
					if (preFinishedRoom == null)
					{
						await this.CombatStateSynchronizer.WaitForSync();
					}
					if (saveGame)
					{
						await SaveManager.Instance.SaveRun(null, true);
					}
					if (this.CombatReplayWriter.IsEnabled)
					{
						this.CombatReplayWriter.RecordInitialState(this.ToSave(null));
					}
					RoomType roomType;
					if (pointType == MapPointType.Unknown && preFinishedRoom != null)
					{
						roomType = RoomType.Monster;
					}
					else
					{
						MapPointHistoryEntry currentMapPointHistoryEntry = this.State.CurrentMapPointHistoryEntry;
						MapPoint currentMapPoint = this.State.CurrentMapPoint;
						roomType = this.RollRoomTypeFor(pointType, RunManager.BuildRoomTypeBlacklist(currentMapPointHistoryEntry, ((currentMapPoint != null) ? currentMapPoint.Children : null) ?? new HashSet<MapPoint>()));
					}
					AbstractRoom abstractRoom;
					if (preFinishedRoom != null)
					{
						abstractRoom = preFinishedRoom;
					}
					else
					{
						abstractRoom = this.CreateRoom(roomType, pointType, null);
					}
					this.ActionExecutor.Pause();
					if (preFinishedRoom == null)
					{
						this.State.AppendToMapPointHistory(pointType, abstractRoom.RoomType, abstractRoom.ModelId);
					}
					CombatRoom combatRoom = abstractRoom as CombatRoom;
					if (combatRoom != null && combatRoom.IsPreFinished && combatRoom.ParentEventId != null)
					{
						await this.EnterRoomInternal(new EventRoom(ModelDb.GetById<EventModel>(combatRoom.ParentEventId)), true);
						await this.EnterRoomInternal(combatRoom, false);
					}
					else
					{
						await this.EnterRoom(abstractRoom);
					}
					if (NRun.Instance != null)
					{
						NRun.Instance.GlobalUi.MapScreen.IsTraveling = false;
					}
					this.AfterMapLocationChanged();
				}
			}
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0004FED4 File Offset: 0x0004E0D4
		private AbstractRoom CreateRoom(RoomType roomType, MapPointType mapPointType = MapPointType.Unassigned, [Nullable(2)] AbstractModel model = null)
		{
			if (this.State == null)
			{
				throw new InvalidOperationException("RunState is not set.");
			}
			AbstractRoom abstractRoom;
			switch (roomType)
			{
			case RoomType.Monster:
			case RoomType.Elite:
			case RoomType.Boss:
				abstractRoom = new CombatRoom((model as EncounterModel) ?? this.State.Act.PullNextEncounter(roomType).ToMutable(), this.State);
				break;
			case RoomType.Treasure:
				abstractRoom = new TreasureRoom(this.State.CurrentActIndex);
				break;
			case RoomType.Shop:
				abstractRoom = new MerchantRoom();
				break;
			case RoomType.Event:
				abstractRoom = new EventRoom((model as EventModel) ?? ((mapPointType == MapPointType.Ancient) ? this.State.Act.PullAncient() : this.State.Act.PullNextEvent(this.State)));
				break;
			case RoomType.RestSite:
				abstractRoom = new RestSiteRoom();
				break;
			case RoomType.Map:
				abstractRoom = new MapRoom();
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected RoomType: ");
				defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return abstractRoom;
		}

		/// <summary>
		/// Roll for a room type based on the specified map point type.
		/// Most map point type have an idempotent room type mapping, but unknown points need to do an RNG roll to determine
		/// room type, which is why we call this a "roll".
		/// </summary>
		/// <param name="pointType">MapPointType to roll a RoomType for.</param>
		/// <param name="blacklist">Room types that we shouldn't be able to roll for map point types with multiple options.</param>
		/// <returns>RoomType.</returns>
		// Token: 0x06000CAF RID: 3247 RVA: 0x0004FFEC File Offset: 0x0004E1EC
		private RoomType RollRoomTypeFor(MapPointType pointType, IEnumerable<RoomType> blacklist)
		{
			RoomType roomType;
			if (this.TryGetRoomTypeForTutorial(pointType, out roomType))
			{
				return roomType;
			}
			RoomType roomType2;
			switch (pointType)
			{
			case MapPointType.Unassigned:
				roomType2 = RoomType.Unassigned;
				break;
			case MapPointType.Unknown:
				roomType2 = this.State.Odds.UnknownMapPoint.Roll(blacklist, this.State);
				break;
			case MapPointType.Shop:
				roomType2 = RoomType.Shop;
				break;
			case MapPointType.Treasure:
				roomType2 = RoomType.Treasure;
				break;
			case MapPointType.RestSite:
				roomType2 = RoomType.RestSite;
				break;
			case MapPointType.Monster:
				roomType2 = RoomType.Monster;
				break;
			case MapPointType.Elite:
				roomType2 = RoomType.Elite;
				break;
			case MapPointType.Boss:
				roomType2 = RoomType.Boss;
				break;
			case MapPointType.Ancient:
				roomType2 = RoomType.Event;
				break;
			default:
				throw new ArgumentOutOfRangeException("pointType", pointType, null);
			}
			return roomType2;
		}

		/// <summary>
		/// Overrides question mark rooms for the player's very first run.
		/// </summary>
		// Token: 0x06000CB0 RID: 3248 RVA: 0x00050084 File Offset: 0x0004E284
		private bool TryGetRoomTypeForTutorial(MapPointType pointType, out RoomType roomType)
		{
			roomType = RoomType.Unassigned;
			if (!TestMode.IsOn)
			{
				RunState state = this.State;
				if (state == null || state.Players.Count <= 1)
				{
					if (pointType != MapPointType.Unassigned)
					{
						return false;
					}
					if (SaveManager.Instance.Progress.NumberOfRuns > 0)
					{
						return false;
					}
					RunState state2 = this.State;
					bool flag;
					if (state2 == null)
					{
						flag = false;
					}
					else
					{
						flag = state2.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry> l) => l).Any<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.MapPointType == MapPointType.Unassigned);
					}
					if (flag)
					{
						return false;
					}
					roomType = RoomType.Event;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Helper to call the universal "enter the room" fade in vfx.
		/// </summary>
		// Token: 0x06000CB1 RID: 3249 RVA: 0x00050134 File Offset: 0x0004E334
		public async Task FadeIn(bool showTransition = true)
		{
			if (TestMode.IsOn)
			{
				Func<Task> testFadeIn = this.TestFadeIn;
				Task task = ((testFadeIn != null) ? testFadeIn() : null);
				if (task != null)
				{
					await task;
				}
			}
			else
			{
				await NGame.Instance.Transition.RoomFadeIn(showTransition);
			}
		}

		/// <summary>
		/// Helper to call the universal "exit the room" fade out vfx.
		/// </summary>
		// Token: 0x06000CB2 RID: 3250 RVA: 0x00050180 File Offset: 0x0004E380
		public async Task FadeOut()
		{
			if (TestMode.IsOn)
			{
				Func<Task> testFadeOut = this.TestFadeOut;
				Task task = ((testFadeOut != null) ? testFadeOut() : null);
				if (task != null)
				{
					await task;
				}
			}
			else
			{
				await NGame.Instance.Transition.RoomFadeOut();
			}
		}

		/// <summary>
		/// Resets various UI elements before transitioning to the next room.
		/// </summary>
		// Token: 0x06000CB3 RID: 3251 RVA: 0x000501C3 File Offset: 0x0004E3C3
		private void ClearScreens()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			NOverlayStack.Instance.Clear();
			NCapstoneContainer.Instance.Close();
			NMapScreen.Instance.Close(false);
		}

		/// <summary>
		/// Should only be used in tests or dev commands, never in real flows.
		/// </summary>
		// Token: 0x06000CB4 RID: 3252 RVA: 0x000501EC File Offset: 0x0004E3EC
		public async Task EnterMapCoordDebug(MapCoord coord, RoomType roomType, MapPointType pointType = MapPointType.Unassigned, [Nullable(2)] AbstractModel model = null, bool showTransition = true)
		{
			this.State.AddVisitedMapCoord(coord);
			await this.EnterRoomDebug(roomType, pointType, model, showTransition);
		}

		/// <summary>
		/// Should only be used in tests or dev commands, never in real flows.
		/// </summary>
		// Token: 0x06000CB5 RID: 3253 RVA: 0x0005025C File Offset: 0x0004E45C
		public async Task<AbstractRoom> EnterRoomDebug(RoomType roomType, MapPointType pointType = MapPointType.Unassigned, [Nullable(2)] AbstractModel model = null, bool showTransition = true)
		{
			AbstractRoom abstractRoom;
			using (new NetLoadingHandle(this.NetService))
			{
				this.ClearScreens();
				await this.ExitCurrentRooms();
				this.CombatStateSynchronizer.StartSync();
				EncounterModel encounterModel = model as EncounterModel;
				if (encounterModel != null)
				{
					roomType = encounterModel.RoomType;
				}
				else if (model is EventModel)
				{
					roomType = RoomType.Event;
				}
				if (pointType == MapPointType.Unassigned)
				{
					MapPointType mapPointType;
					switch (roomType)
					{
					case RoomType.Unassigned:
						mapPointType = MapPointType.Unassigned;
						break;
					case RoomType.Monster:
						mapPointType = MapPointType.Monster;
						break;
					case RoomType.Elite:
						mapPointType = MapPointType.Elite;
						break;
					case RoomType.Boss:
						mapPointType = MapPointType.Boss;
						break;
					case RoomType.Treasure:
						mapPointType = MapPointType.Treasure;
						break;
					case RoomType.Shop:
						mapPointType = MapPointType.Shop;
						break;
					case RoomType.Event:
						mapPointType = MapPointType.Unknown;
						break;
					case RoomType.RestSite:
						mapPointType = MapPointType.RestSite;
						break;
					case RoomType.Map:
						mapPointType = MapPointType.Unassigned;
						break;
					default:
						<PrivateImplementationDetails>.ThrowSwitchExpressionException(roomType);
						break;
					}
					pointType = mapPointType;
				}
				if (this.CombatReplayWriter.IsEnabled)
				{
					this.CombatReplayWriter.RecordInitialState(this.ToSave(null));
				}
				this.State.AppendToMapPointHistory(pointType, roomType, (model != null) ? model.Id : null);
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.GlobalUi.TopBar.RoomIcon.DebugSetMapPointTypeOverride(pointType);
				}
				MockSinglePointActMap mockSinglePointActMap = this.State.Map as MockSinglePointActMap;
				if (mockSinglePointActMap != null)
				{
					mockSinglePointActMap.MockCurrentMapPointType(pointType);
				}
				await this.CombatStateSynchronizer.WaitForSync();
				AbstractRoom room = this.CreateRoom(roomType, MapPointType.Unassigned, model);
				await this.EnterRoom(room);
				await this.FadeIn(showTransition);
				abstractRoom = room;
			}
			return abstractRoom;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000502C0 File Offset: 0x0004E4C0
		private async Task ExitCurrentRooms()
		{
			if (this.State != null)
			{
				while (this.State.CurrentRoomCount > 0)
				{
					await this.ExitCurrentRoom();
				}
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.GlobalUi.TopBar.RoomIcon.DebugClearMapPointTypeOverride();
				}
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00050304 File Offset: 0x0004E504
		[return: Nullable(new byte[] { 1, 2 })]
		private async Task<AbstractRoom> ExitCurrentRoom()
		{
			AbstractRoom abstractRoom;
			if (this.State == null)
			{
				abstractRoom = null;
			}
			else
			{
				this.RewardsSetSynchronizer.BeforeLeavingRoom();
				AbstractRoom currentRoom = this.State.PopCurrentRoom();
				await currentRoom.Exit(this.State);
				Action roomExited = this.RoomExited;
				if (roomExited != null)
				{
					roomExited();
				}
				abstractRoom = currentRoom;
			}
			return abstractRoom;
		}

		/// <param name="room">Room to enter.</param>
		/// <param name="isRestoringRoomStackBase">
		/// If true, skip hooks and room visit tracking. Used when reconstructing the base of the room stack on load
		/// (e.g., pushing a parent EventRoom underneath a pre-finished CombatRoom).
		/// </param>
		// Token: 0x06000CB8 RID: 3256 RVA: 0x00050348 File Offset: 0x0004E548
		private async Task EnterRoomInternal(AbstractRoom room, bool isRestoringRoomStackBase = false)
		{
			if (this.State != null)
			{
				bool flag = isRestoringRoomStackBase;
				if (!flag)
				{
					CombatRoom combatRoom = room as CombatRoom;
					if (combatRoom != null)
					{
						if (!combatRoom.IsPreFinished)
						{
							goto IL_0077;
						}
					}
					else
					{
						EventRoom eventRoom = room as EventRoom;
						if (eventRoom == null)
						{
							goto IL_0077;
						}
						bool isPreFinished = eventRoom.IsPreFinished;
						if (!isPreFinished)
						{
							goto IL_0077;
						}
					}
					bool flag2 = true;
					goto IL_007A;
					IL_0077:
					flag2 = false;
					IL_007A:
					flag = flag2;
				}
				bool runExternalEffects = !flag;
				this.State.PushRoom(room);
				if (runExternalEffects && !(room is MapRoom))
				{
					await Hook.BeforeRoomEntered(this.State, room);
				}
				await room.Enter(this.State, isRestoringRoomStackBase);
				if (runExternalEffects)
				{
					NRunMusicController instance = NRunMusicController.Instance;
					if (instance != null)
					{
						instance.UpdateTrack();
					}
					if (this.State.CurrentRoomCount == 1)
					{
						this.State.Act.MarkRoomVisited(room.RoomType);
					}
				}
				this.RunLocationTargetedBuffer.OnLocationChanged(this.State.RunLocation);
				if (!(room is CombatRoom))
				{
					this.ActionExecutor.Unpause();
				}
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateAmbience();
				}
				Action roomEntered = this.RoomEntered;
				if (roomEntered != null)
				{
					roomEntered();
				}
			}
		}

		/// <summary>
		/// Exit all the rooms that the player is currently in and enter the specified room.
		/// NOTE: If you want to enter a room WITHOUT exiting the current rooms first, call
		/// <see cref="M:MegaCrit.Sts2.Core.Runs.RunManager.EnterRoomWithoutExitingCurrentRoom(MegaCrit.Sts2.Core.Rooms.AbstractRoom,System.Boolean)" /> instead.
		/// </summary>
		/// <param name="room">Room to enter</param>
		// Token: 0x06000CB9 RID: 3257 RVA: 0x0005039C File Offset: 0x0004E59C
		public async Task EnterRoom(AbstractRoom room)
		{
			await this.ExitCurrentRooms();
			await this.EnterRoomInternal(room, false);
		}

		/// <summary>
		/// Enter the specified room without exiting any current rooms you may be in.
		/// IMPORTANT NOTE: If you are already in a room and want to exit it first, you should call <see cref="M:MegaCrit.Sts2.Core.Runs.RunManager.EnterRoom(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" />
		/// instead.
		/// If you are already in a room, this will push the new room on top of the "room stack". This is to support effects
		/// where the player should enter a new sub-room, then return to the old one when they're done.
		/// For example, if the player enters an event and selects options that start a fight, they should enter a combat
		/// room. Then, when they're done with the combat, it should be "popped off" the room stack, and they should
		/// transition back to the event.
		/// </summary>
		/// <param name="room">Room that should be entered.</param>
		/// <param name="fadeToBlack">
		/// Whether we should fade to black before entering the room.
		/// Usually true, but false when the room-change is backend only (like when transitioning from a combat-style event
		/// to a real combat).
		/// </param>
		// Token: 0x06000CBA RID: 3258 RVA: 0x000503E8 File Offset: 0x0004E5E8
		public async Task EnterRoomWithoutExitingCurrentRoom(AbstractRoom room, bool fadeToBlack)
		{
			if (this.State != null)
			{
				this.ActionExecutor.Pause();
				this.CombatStateSynchronizer.StartSync();
				using (new NetLoadingHandle(this.NetService))
				{
					if (fadeToBlack)
					{
						await this.FadeOut();
						this.ClearScreens();
					}
					await this.CombatStateSynchronizer.WaitForSync();
					MapPointHistoryEntry currentMapPointHistoryEntry = this.State.CurrentMapPointHistoryEntry;
					if (currentMapPointHistoryEntry != null)
					{
						currentMapPointHistoryEntry.Rooms.Add(new MapPointRoomHistoryEntry
						{
							RoomType = room.RoomType,
							ModelId = room.ModelId
						});
					}
					await this.EnterRoomInternal(room, false);
					ActiveScreenContext.Instance.Update();
					if (fadeToBlack)
					{
						await this.FadeIn(true);
					}
				}
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0005043C File Offset: 0x0004E63C
		public async Task EnterNextAct()
		{
			if (this.State != null)
			{
				using (new NetLoadingHandle(this.NetService))
				{
					if (this.State.CurrentActIndex >= this.State.Acts.Count - 1)
					{
						AbstractRoom currentRoom = this.State.CurrentRoom;
						if (currentRoom != null && currentRoom.IsVictoryRoom)
						{
							await this.WinRun();
						}
						else
						{
							await this.FadeOut();
							this.ClearScreens();
							await this.EnterRoom(new EventRoom(ModelDb.Event<TheArchitect>()));
							await this.FadeIn(true);
						}
					}
					else
					{
						await this.EnterAct(this.State.CurrentActIndex + 1, true);
					}
				}
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00050480 File Offset: 0x0004E680
		private async Task WinRun()
		{
			if (this.State != null)
			{
				EventRoom eventRoom = (EventRoom)this.State.CurrentRoom;
				((TheArchitect)eventRoom.LocalMutableEvent).TriggerVictory();
				this.OnEnded(true);
				await this.GuaranteeKillAllPlayers();
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000504C4 File Offset: 0x0004E6C4
		public async Task EnterAct(int currentActIndex, bool doTransition = true)
		{
			if (this.State != null)
			{
				await this.FadeOut();
				using (new NetLoadingHandle(this.NetService))
				{
					this.ClearScreens();
					await this.ExitCurrentRooms();
					await this.SetActInternal(currentActIndex);
					if (currentActIndex == 0 && this.State.ExtraFields.StartedWithNeow)
					{
						if (NRun.Instance != null)
						{
							NMapScreen instance = NMapScreen.Instance;
							if (instance != null)
							{
								instance.InitMarker(this.State.Map.StartingMapPoint.coord);
							}
						}
						await this.EnterMapCoord(this.State.Map.StartingMapPoint.coord);
						NMapScreen instance2 = NMapScreen.Instance;
						if (instance2 != null)
						{
							instance2.RefreshAllMapPointVotes();
						}
					}
					else
					{
						await this.EnterRoomInternal(new MapRoom(), false);
						Action actEntered = this.ActEntered;
						if (actEntered != null)
						{
							actEntered();
						}
					}
					await this.FadeIn(doTransition);
					await Hook.AfterActEntered(this.State);
				}
			}
		}

		/// <summary>
		/// Should only be used in <see cref="T:MegaCrit.Sts2.Core.Runs.RunManager" /> and tests/debugging.
		/// </summary>
		/// <param name="actIndex">The act to set.</param>
		// Token: 0x06000CBE RID: 3262 RVA: 0x00050518 File Offset: 0x0004E718
		public async Task SetActInternal(int actIndex)
		{
			if (this.State != null)
			{
				this.State.CurrentActIndex = actIndex;
				this.State.ClearVisitedMapCoordsDebug();
				this.State.Odds.UnknownMapPoint.ResetToBase();
				this.AfterMapLocationChanged();
				await PreloadManager.LoadActAssets(this.State.Act);
				await this.GenerateMap();
				NMapScreen instance = NMapScreen.Instance;
				if (instance != null)
				{
					instance.SetTravelEnabled(false);
				}
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusic();
				}
				this.UpdateRichPresence();
			}
		}

		/// <summary>
		/// Update rich presence on platform for the current character/act/ascension.
		/// </summary>
		// Token: 0x06000CBF RID: 3263 RVA: 0x00050564 File Offset: 0x0004E764
		private void UpdateRichPresence()
		{
			if (TestMode.IsOn || this.State == null)
			{
				return;
			}
			PlatformUtil.SetRichPresence("IN_RUN", this.NetService.GetRawLobbyIdentifier(), new int?(this.State.Players.Count));
			PlatformUtil.SetRichPresenceValue("Character", LocalContext.GetMe(this.State).Character.Id.Entry);
			PlatformUtil.SetRichPresenceValue("Act", this.State.Act.Id.Entry);
			PlatformUtil.SetRichPresenceValue("Ascension", this.State.AscensionLevel.ToString());
		}

		/// <summary>
		/// This is called from NRewardsScreen when the screen is terminal.
		/// Runs the correct logic for proceeding depending on the state of the run.
		///
		/// A rewards screen is terminal for combat rewards and treasure rooms, where proceeding from it opens up the map so
		/// you can select the next room to travel to.
		///
		/// A rewards screen is non-terminal for things like the Calling Bell relic, which shows a rewards screen wherever
		/// you pick it up from and then lets you continue with whatever was happening in that room.
		/// </summary>
		// Token: 0x06000CC0 RID: 3264 RVA: 0x0005060C File Offset: 0x0004E80C
		public async Task ProceedFromTerminalRewardsScreen()
		{
			if (this.State != null)
			{
				if (this.State.CurrentRoomCount > 1)
				{
					CombatRoom combatRoom = this.State.CurrentRoom as CombatRoom;
					if (combatRoom != null && combatRoom.ShouldResumeParentEventAfterCombat)
					{
						await this.ResumePreviousRoom();
					}
					else
					{
						NMapScreen instance = NMapScreen.Instance;
						if (instance != null)
						{
							instance.SetTravelEnabled(true);
						}
						NMapScreen instance2 = NMapScreen.Instance;
						if (instance2 != null)
						{
							instance2.Open(false);
						}
					}
				}
				else
				{
					NMapScreen instance3 = NMapScreen.Instance;
					if (instance3 != null)
					{
						instance3.Open(false);
					}
				}
			}
		}

		/// <summary>
		/// Exits the current room and resumes the previous room in the stack.
		/// This is usually called if you're in an event combat and you're returning back to the event.
		/// </summary>
		// Token: 0x06000CC1 RID: 3265 RVA: 0x00050650 File Offset: 0x0004E850
		private async Task ResumePreviousRoom()
		{
			if (this.State != null)
			{
				this.ClearScreens();
				AbstractRoom abstractRoom = await this.ExitCurrentRoom();
				AbstractRoom abstractRoom2 = abstractRoom;
				if (abstractRoom2 != null)
				{
					await this.State.CurrentRoom.Resume(abstractRoom2, this.State);
					NRunMusicController instance = NRunMusicController.Instance;
					if (instance != null)
					{
						instance.UpdateTrack();
					}
					await this.FadeIn(true);
				}
				else
				{
					Log.Error("Current room returned null while exiting.", 2);
				}
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00050693 File Offset: 0x0004E893
		private void AfterMapLocationChanged()
		{
			this.MapSelectionSynchronizer.OnLocationChanged(this.State.MapLocation);
			this.RunLocationTargetedBuffer.OnLocationChanged(this.State.RunLocation);
		}

		/// <summary>
		/// Abandons the run.
		/// If in multiplayer, you should only call this on the host, and the run abandonment will be synchronized across
		/// all peers. If called on a multiplayer client, an exception will be thrown.
		/// </summary>
		// Token: 0x06000CC3 RID: 3267 RVA: 0x000506C1 File Offset: 0x0004E8C1
		public void Abandon()
		{
			Log.Info("Abandoning an in-progress run (player-initiated)", 2);
			if (this.NetService.Type == NetGameType.Singleplayer)
			{
				TaskHelper.RunSafely(this.AbandonInternal());
				return;
			}
			this.RunLobby.AbandonRun();
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000506F4 File Offset: 0x0004E8F4
		void IRunLobbyListener.RunAbandoned()
		{
			Log.Info("The host told us to abandon the run", 2);
			NMapScreen instance = NMapScreen.Instance;
			if (instance != null)
			{
				instance.Close(false);
			}
			NCapstoneContainer instance2 = NCapstoneContainer.Instance;
			if (instance2 != null)
			{
				instance2.Close();
			}
			TaskHelper.RunSafely(this.AbandonInternal());
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00050730 File Offset: 0x0004E930
		private async Task AbandonInternal()
		{
			try
			{
				NCapstoneContainer.Instance.Close();
				NMapScreen.Instance.Close(false);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception thrown while trying to abandon run: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			this.IsAbandoned = true;
			await this.GuaranteeKillAllPlayers();
			if (this.NetService.Type == NetGameType.Client)
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(new NetErrorInfo(NetError.HostAbandoned, false));
				if (nerrorPopup != null)
				{
					NModalContainer.Instance.Add(nerrorPopup, true);
				}
			}
		}

		/// <summary>
		/// When you Abandon a Run you are sentenced to death.
		/// </summary>
		// Token: 0x06000CC6 RID: 3270 RVA: 0x00050774 File Offset: 0x0004E974
		private async Task GuaranteeKillAllPlayers()
		{
			if (this.State != null)
			{
				foreach (Player player in this.State.Players)
				{
					await CreatureCmd.Kill(player.Creature, true);
					await Cmd.CustomScaledWait(0.25f, 0.5f, false, default(CancellationToken));
				}
				IEnumerator<Player> enumerator = null;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x000507B7 File Offset: 0x0004E9B7
		private void StateDiverged(NetFullCombatState state)
		{
			if (this.NetService.Type == NetGameType.Replay)
			{
				return;
			}
			Log.Info("Abandoning run and returning to main menu because our state diverged from host's", 2);
			this.WriteReplay(false);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x000507DC File Offset: 0x0004E9DC
		public void WriteReplay(bool stopRecording)
		{
			string profileScopedPath = SaveManager.Instance.GetProfileScopedPath("replays/latest.mcr");
			this.CombatReplayWriter.WriteReplay(profileScopedPath, stopRecording);
		}

		/// <summary>
		/// Cleans up the run and disconnects us from any multiplayer peers.
		/// </summary>
		/// <param name="graceful">If true, messages are allowed to be sent before closing the multiplayer connection. Pass
		/// false if the game window is being closed.</param>
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00050808 File Offset: 0x0004EA08
		public void CleanUp(bool graceful = true)
		{
			if (this.State == null)
			{
				return;
			}
			this.ShouldSave = false;
			this.IsCleaningUp = true;
			try
			{
				this._runHistoryWasUploaded = false;
				this.ActionQueueSet.Reset();
				CardSelectCmd.Reset();
				NAudioManager instance = NAudioManager.Instance;
				if (instance != null)
				{
					instance.StopAllLoops();
				}
				NOverlayStack instance2 = NOverlayStack.Instance;
				if (instance2 != null)
				{
					instance2.Clear();
				}
				NCapstoneContainer instance3 = NCapstoneContainer.Instance;
				if (instance3 != null)
				{
					instance3.CleanUp();
				}
				NMapScreen instance4 = NMapScreen.Instance;
				if (instance4 != null)
				{
					instance4.CleanUp();
				}
				NModalContainer instance5 = NModalContainer.Instance;
				if (instance5 != null)
				{
					instance5.Clear();
				}
				CombatManager.Instance.Reset(graceful);
				if (this.CombatReplayWriter.IsRecordingReplay)
				{
					this.WriteReplay(true);
				}
				this.ActionExecutor.JustBeforeActionFinishedExecuting -= this.SendPostActionChecksum;
				this.CombatReplayWriter.Dispose();
				this.ActionQueueSynchronizer.Dispose();
				this.PlayerChoiceSynchronizer.Dispose();
				this.RewardSynchronizer.Dispose();
				this.RewardsSetSynchronizer.Dispose();
				this.RestSiteSynchronizer.Dispose();
				this.FlavorSynchronizer.Dispose();
				this.ChecksumTracker.Dispose();
				if (this.RunLobby != null)
				{
					this.RunLobby.RemotePlayerDisconnected -= this.RemotePlayerDisconnected;
					this.RunLobby.Dispose();
				}
				this.NetService.Disconnect(NetError.Quit, !graceful);
			}
			finally
			{
				this.IsCleaningUp = false;
				LocalContext.NetId = null;
				this.State = null;
				this.DailyTime = null;
			}
		}

		/// <summary>
		/// Called when the run ends.
		/// </summary>
		/// <param name="isVictory">Whether or not the run ended in a victory.</param>
		/// <returns>The serialized version of the run that just ended.</returns>
		// Token: 0x06000CCA RID: 3274 RVA: 0x000509A8 File Offset: 0x0004EBA8
		public SerializableRun OnEnded(bool isVictory)
		{
			this.UpdatePlayerStatsInMapPointHistory();
			RunState state = this.State;
			Player me = LocalContext.GetMe(state);
			CombatRoom combatRoom = state.CurrentRoom as CombatRoom;
			if (combatRoom != null)
			{
				MapPointRoomHistoryEntry mapPointRoomHistoryEntry = state.CurrentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
				MapPointRoomHistoryEntry mapPointRoomHistoryEntry2 = mapPointRoomHistoryEntry;
				PlayerCombatState playerCombatState = me.PlayerCombatState;
				mapPointRoomHistoryEntry2.TurnsTaken = ((playerCombatState != null) ? playerCombatState.TurnNumber : combatRoom.CombatState.RoundNumber);
			}
			SerializableRun serializableRun = this.ToSave(null);
			SerializablePlayer me2 = LocalContext.GetMe(serializableRun);
			if (this._runHistoryWasUploaded)
			{
				return serializableRun;
			}
			this._runHistoryWasUploaded = true;
			if (!isVictory)
			{
				CombatRoom combatRoom2 = state.CurrentRoom as CombatRoom;
				if (combatRoom2 != null)
				{
					foreach (ValueTuple<MonsterModel, string> valueTuple in combatRoom2.Encounter.MonstersWithSlots)
					{
						MonsterModel item = valueTuple.Item1;
						RunManager.CheckUpdateEnemyDiscoveryAfterLoss(me, item.Id);
					}
				}
			}
			if (this.ShouldSave)
			{
				using (SaveManager.Instance.BeginSaveBatch())
				{
					SaveManager.Instance.UpdateProgressWithRunData(serializableRun, isVictory);
					foreach (string text in me2.DiscoveredEpochs)
					{
						if (!me.DiscoveredEpochs.Contains(text))
						{
							me.DiscoveredEpochs.Add(text);
						}
					}
					AchievementsHelper.AfterRunEnded(state, me, isVictory);
					RunHistoryUtilities.CreateRunHistoryEntry(serializableRun, isVictory, this.IsAbandoned, this.NetService.Platform);
					MetricUtilities.UploadRunMetrics(serializableRun, isVictory, this.NetService.NetId);
					if (SaveManager.Instance.Progress.NumberOfRuns == 5)
					{
						MetricUtilities.UploadSettingsMetric();
					}
					if (this.NetService.Type == NetGameType.Singleplayer)
					{
						SaveManager.Instance.DeleteCurrentRun();
					}
					else if (this.NetService.Type == NetGameType.Host)
					{
						SaveManager.Instance.DeleteCurrentMultiplayerRun();
					}
				}
				if (isVictory)
				{
					int num = ScoreUtility.CalculateScore(serializableRun, isVictory);
					StatsManager.IncrementArchitectDamage(num);
				}
			}
			if (this.DailyTime != null)
			{
				NetGameType type = this.NetService.Type;
				bool flag = type - NetGameType.Singleplayer <= 1;
				if (flag)
				{
					int num2 = ScoreUtility.CalculateDailyScore(serializableRun, me.NetId, isVictory);
					TaskHelper.RunSafely(DailyRunUtility.UploadScore(this.DailyTime.Value, num2, serializableRun.Players));
				}
				else if (this.NetService.Type == NetGameType.Client)
				{
					TaskHelper.RunSafely(DailyRunUtility.UploadScore(this.DailyTime.Value, -999999999, serializableRun.Players));
				}
			}
			return serializableRun;
		}

		/// <summary>
		/// Updates the player's DiscoveredEnemies stat when the player loses to a monster.
		/// This does _not_ update the progress save in any way! It only checks if the player has newly discovered a monster
		/// and adds it to DiscoveredEnemies if so.
		/// </summary>
		// Token: 0x06000CCB RID: 3275 RVA: 0x00050C5C File Offset: 0x0004EE5C
		private static void CheckUpdateEnemyDiscoveryAfterLoss(Player player, ModelId monster)
		{
			EnemyStats enemyStats;
			if ((SaveManager.Instance.Progress.EnemyStats.TryGetValue(monster, out enemyStats) ? enemyStats : null) == null)
			{
				player.DiscoveredEnemies.Add(monster);
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00050C98 File Offset: 0x0004EE98
		private void UpdatePlayerStatsInMapPointHistory()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (this.State == null)
			{
				return;
			}
			foreach (Player player in this.State.Players)
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = this.State.CurrentMapPointHistoryEntry;
				PlayerMapPointHistoryEntry playerMapPointHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.GetEntry(player.NetId) : null);
				if (playerMapPointHistoryEntry != null)
				{
					playerMapPointHistoryEntry.CurrentGold = player.Gold;
					playerMapPointHistoryEntry.CurrentHp = player.Creature.CurrentHp;
					playerMapPointHistoryEntry.MaxHp = player.Creature.MaxHp;
				}
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00050D44 File Offset: 0x0004EF44
		public bool HasAscension(AscensionLevel level)
		{
			return this.IsInProgress && this.AscensionManager.HasLevel(level);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00050D5C File Offset: 0x0004EF5C
		public void ApplyAscensionEffects(Player player)
		{
			this.AscensionManager.ApplyEffectsTo(player);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00050D6C File Offset: 0x0004EF6C
		public ClientRejoinResponseMessage GetRejoinMessage()
		{
			return new ClientRejoinResponseMessage
			{
				serializableRun = this.ToSave(null),
				combatState = NetFullCombatState.FromRun(this.State, null)
			};
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00050DA4 File Offset: 0x0004EFA4
		public void LocalPlayerDisconnected(NetErrorInfo info)
		{
			foreach (Player player in this.State.Players)
			{
				if (!LocalContext.IsMe(player))
				{
					this.InputSynchronizer.OnPlayerDisconnected(player.NetId);
				}
			}
			if (info.GetReason() == NetError.QuitGameOver)
			{
				return;
			}
			if (this.IsAbandoned)
			{
				return;
			}
			if (this.State.IsGameOver)
			{
				return;
			}
			TaskHelper.RunSafely(this.ReturnToMainMenuWithError(info));
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00050E38 File Offset: 0x0004F038
		private void RemotePlayerDisconnected(ulong playerId)
		{
			this.InputSynchronizer.OnPlayerDisconnected(playerId);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00050E48 File Offset: 0x0004F048
		private async Task ReturnToMainMenuWithError(NetErrorInfo info)
		{
			NCapstoneContainer instance = NCapstoneContainer.Instance;
			if (instance != null)
			{
				instance.Close();
			}
			NMapScreen instance2 = NMapScreen.Instance;
			if (instance2 != null)
			{
				instance2.Close(false);
			}
			if (TestMode.IsOff)
			{
				await NGame.Instance.ReturnToMainMenuAfterRun();
				NErrorPopup nerrorPopup = NErrorPopup.Create(info);
				if (nerrorPopup != null)
				{
					NModalContainer.Instance.Add(nerrorPopup, true);
				}
			}
		}

		/// <summary>
		/// Get the energy icon prefix for the local player's character. Null if there's no run in progress.
		/// An unfortunate necessity to make energy icons render properly.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000CD3 RID: 3283 RVA: 0x00050E8C File Offset: 0x0004F08C
		[NullableContext(2)]
		public string GetLocalCharacterEnergyIconPrefix()
		{
			Player me = LocalContext.GetMe(this.State);
			CardPoolModel cardPoolModel = ((me != null) ? me.Character.CardPool : null);
			if (cardPoolModel != null)
			{
				return EnergyIconHelper.GetPrefix(cardPoolModel);
			}
			return null;
		}

		/// <summary>
		/// THIS IS TEMPORARY AND SHOULD ONLY BE USED IN TESTS
		/// </summary>
		// Token: 0x06000CD4 RID: 3284 RVA: 0x00050EC1 File Offset: 0x0004F0C1
		[NullableContext(2)]
		public RunState DebugOnlyGetState()
		{
			return this.State;
		}

		// Token: 0x04000447 RID: 1095
		private long _startTime;

		/// <summary>
		/// previous session's run time before reload
		/// </summary>
		// Token: 0x04000448 RID: 1096
		private long _prevSessionRunTime;

		/// <summary>
		/// Accumulated run time not spent in the pause menu
		/// </summary>
		// Token: 0x04000449 RID: 1097
		private long _activeRunTime;

		/// <summary>
		/// When the current active run time began
		/// Which is either the start of the session OR the time from when you last closed the pause menu
		/// </summary>
		// Token: 0x0400044A RID: 1098
		private long _startOfCurrentActiveRunTime;

		// Token: 0x0400044B RID: 1099
		private bool _isPaused;

		// Token: 0x0400044C RID: 1100
		private bool _runHistoryWasUploaded;

		// Token: 0x0400044D RID: 1101
		private int _numReloads;

		// Token: 0x04000465 RID: 1125
		[Nullable(2)]
		public Action debugAfterCombatRewardsOverride;
	}
}
