using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatRoom : AbstractRoom, ICombatRoomVisuals
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0005B9BC File Offset: 0x00059BBC
		public override RoomType RoomType
		{
			get
			{
				return this.Encounter.RoomType;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0005B9C9 File Offset: 0x00059BC9
		public override ModelId ModelId
		{
			get
			{
				return this.Encounter.Id;
			}
		}

		/// <summary>
		/// The mutable encounter that the player is facing in this room.
		/// </summary>
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0005B9D6 File Offset: 0x00059BD6
		public EncounterModel Encounter
		{
			get
			{
				return this.CombatState.Encounter;
			}
		}

		/// <summary>
		/// The state of the combat that is currently in progress in this room.
		/// </summary>
		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0005B9E3 File Offset: 0x00059BE3
		public CombatState CombatState { get; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0005B9EB File Offset: 0x00059BEB
		public IEnumerable<Creature> Allies
		{
			get
			{
				return this.CombatState.Allies;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0005B9F8 File Offset: 0x00059BF8
		public IEnumerable<Creature> Enemies
		{
			get
			{
				return this.CombatState.Enemies;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0005BA05 File Offset: 0x00059C05
		public ActModel Act
		{
			get
			{
				return this.CombatState.RunState.Act;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0005BA17 File Offset: 0x00059C17
		public override bool IsPreFinished
		{
			get
			{
				return this._isPreFinished;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0005BA1F File Offset: 0x00059C1F
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0005BA27 File Offset: 0x00059C27
		public float GoldProportion { get; private set; } = 1f;

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0005BA30 File Offset: 0x00059C30
		public IReadOnlyDictionary<Player, List<Reward>> ExtraRewards
		{
			get
			{
				return this._extraRewards;
			}
		}

		/// <summary>
		/// Whether to create a combat room node for this combat room.
		/// Usually true, but false for "delayed-start" combats, like events with combat-style layouts that can transition
		/// to combats if certain options are selected.
		/// </summary>
		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0005BA38 File Offset: 0x00059C38
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x0005BA40 File Offset: 0x00059C40
		public bool ShouldCreateCombat { get; set; } = true;

		/// <summary>
		/// If this combat room is nested within an event room, should we resume the parent event after combat ends?
		/// Usually true, but false for some combat-style events. In many of these cases, you transition from a visual-only
		/// combat room into a real combat, and then you just proceed to the next map point after combat ends.
		/// </summary>
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0005BA49 File Offset: 0x00059C49
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x0005BA51 File Offset: 0x00059C51
		public bool ShouldResumeParentEventAfterCombat { get; set; } = true;

		/// <summary>
		/// If this combat room is nested within an event room that should resume after combat,
		/// this stores the parent event's ID so the event room can be recreated on load.
		/// </summary>
		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0005BA5A File Offset: 0x00059C5A
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0005BA62 File Offset: 0x00059C62
		[Nullable(2)]
		public ModelId ParentEventId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// Creates a combat room for the given encounter.
		/// </summary>
		/// <param name="encounter">Encounter that is taking place in this combat room.</param>
		/// <param name="runState">State of the run that this combat room is a part of.</param>
		// Token: 0x06000E77 RID: 3703 RVA: 0x0005BA6C File Offset: 0x00059C6C
		public CombatRoom(EncounterModel encounter, [Nullable(2)] IRunState runState)
		{
			encounter.AssertMutable();
			this.CombatState = new CombatState(encounter, runState, (runState != null) ? runState.Modifiers : null, (runState != null) ? runState.BadgeModels : null, (runState != null) ? runState.MultiplayerScalingModel : null);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0005BADA File Offset: 0x00059CDA
		public CombatRoom(CombatState combatState)
		{
			this.CombatState = combatState;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0005BB10 File Offset: 0x00059D10
		public new static CombatRoom FromSerializable(SerializableRoom serializableRoom, [Nullable(2)] IRunState runState)
		{
			if (serializableRoom.ExtraRewards.Count > 0 && runState == null)
			{
				throw new InvalidOperationException("Cannot load extra rewards without a run state.");
			}
			EncounterModel encounterModel = SaveUtil.EncounterOrDeprecated(serializableRoom.EncounterId).ToMutable();
			encounterModel.LoadCustomState(serializableRoom.EncounterState);
			CombatRoom combatRoom = new CombatRoom(encounterModel, runState)
			{
				GoldProportion = serializableRoom.GoldProportion,
				_isPreFinished = serializableRoom.IsPreFinished,
				ShouldResumeParentEventAfterCombat = serializableRoom.ShouldResumeParentEvent,
				ParentEventId = serializableRoom.ParentEventId
			};
			foreach (KeyValuePair<ulong, List<SerializableReward>> keyValuePair in serializableRoom.ExtraRewards)
			{
				ulong num;
				List<SerializableReward> list;
				keyValuePair.Deconstruct(out num, out list);
				ulong num2 = num;
				List<SerializableReward> list2 = list;
				Player player = runState.GetPlayer(num2);
				List<Reward> list3 = list2.Select<SerializableReward, Reward>((SerializableReward sr) => Reward.FromSerializable(sr, player)).ToList<Reward>();
				combatRoom._extraRewards.Add(player, list3);
			}
			if (serializableRoom.IsPreFinished)
			{
				combatRoom.MarkPreFinished();
			}
			return combatRoom;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0005BC34 File Offset: 0x00059E34
		public override async Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			if (isRestoringRoomStackBase)
			{
				throw new InvalidOperationException("CombatRoom does not support room stack reconstruction.");
			}
			if (this.CombatState.Players.Count == 0)
			{
				foreach (Player player in (((runState != null) ? runState.Players : null) ?? Array.Empty<Player>()))
				{
					this.CombatState.AddPlayer(player);
				}
			}
			if (this.IsPreFinished)
			{
				await this.StartPreFinishedCombat();
			}
			else
			{
				await this.StartCombat(runState);
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0005BC88 File Offset: 0x00059E88
		public override Task Exit([Nullable(2)] IRunState runState)
		{
			CombatManager.Instance.Reset(true);
			if (this.IsPreFinished)
			{
				foreach (Creature creature in this.CombatState.PlayerCreatures.ToList<Creature>())
				{
					this.CombatState.RemoveCreature(creature, true);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0005BD04 File Offset: 0x00059F04
		public override Task Resume(AbstractRoom _, [Nullable(2)] IRunState runState)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0005BD0C File Offset: 0x00059F0C
		public override SerializableRoom ToSerializable()
		{
			if (this.ParentEventId != null && !this.IsPreFinished)
			{
				throw new InvalidOperationException("Cannot serialize a CombatRoom with a ParentEventId that is not pre-finished.");
			}
			SerializableRoom serializableRoom = base.ToSerializable();
			serializableRoom.EncounterId = this.Encounter.Id;
			serializableRoom.IsPreFinished = this.IsPreFinished;
			serializableRoom.GoldProportion = this.GoldProportion;
			serializableRoom.ParentEventId = this.ParentEventId;
			serializableRoom.ShouldResumeParentEvent = this.ShouldResumeParentEventAfterCombat;
			serializableRoom.EncounterState = this.Encounter.SaveCustomState();
			foreach (KeyValuePair<Player, List<Reward>> keyValuePair in this.ExtraRewards)
			{
				Player player;
				List<Reward> list;
				keyValuePair.Deconstruct(out player, out list);
				Player player2 = player;
				List<Reward> list2 = list;
				serializableRoom.ExtraRewards[player2.NetId] = list2.Select<Reward, SerializableReward>((Reward r) => r.ToSerializable()).ToList<SerializableReward>();
			}
			return serializableRoom;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0005BE1C File Offset: 0x0005A01C
		public void MarkPreFinished()
		{
			this._isPreFinished = true;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0005BE25 File Offset: 0x0005A025
		public void AddExtraReward(Player player, Reward reward)
		{
			if (!this.ExtraRewards.ContainsKey(player))
			{
				this._extraRewards.Add(player, new List<Reward>());
			}
			this.ExtraRewards[player].Add(reward);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0005BE58 File Offset: 0x0005A058
		private async Task StartCombat([Nullable(2)] IRunState runState)
		{
			if (!this.Encounter.HaveMonstersBeenGenerated)
			{
				this.Encounter.GenerateMonstersWithSlots(this.CombatState.RunState);
			}
			if (this.ShouldCreateCombat)
			{
				await PreloadManager.LoadRoomCombatAssets(this.Encounter, runState ?? NullRunState.Instance);
			}
			foreach (ValueTuple<MonsterModel, string> valueTuple in this.Encounter.MonstersWithSlots)
			{
				MonsterModel item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				item.AssertMutable();
				if (this.ShouldCreateCombat)
				{
					Creature creature = this.CombatState.CreateCreature(item, CombatSide.Enemy, item2);
					this.CombatState.AddCreature(creature);
				}
				MapPointHistoryEntry currentMapPointHistoryEntry = this.CombatState.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds.Add(item.Id);
				}
			}
			if (this.ShouldCreateCombat)
			{
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.SetCurrentRoom(NCombatRoom.Create(this, CombatRoomMode.ActiveCombat));
				}
			}
			else
			{
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 != null)
				{
					instance2.TransitionToActiveCombat(this);
				}
			}
			CombatManager.Instance.SetUpCombat(this.CombatState);
			if (runState != null)
			{
				await Hook.AfterRoomEntered(runState, this);
			}
			CombatManager.Instance.AfterCombatRoomLoaded();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005BEA3 File Offset: 0x0005A0A3
		public void OnCombatEnded()
		{
			this.GoldProportion = this.Encounter.CalculateGoldProportion(this.CombatState);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005BEBC File Offset: 0x0005A0BC
		private async Task StartPreFinishedCombat()
		{
			this.Encounter.GenerateMonstersWithSlots(this.CombatState.RunState);
			await PreloadManager.LoadRoomCombatAssets(this.Encounter, this.CombatState.RunState);
			NCombatRoom ncombatRoom = NCombatRoom.Create(this, CombatRoomMode.FinishedCombat);
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.SetCurrentRoom(ncombatRoom);
			}
			if (ncombatRoom != null)
			{
				ncombatRoom.SetUpBackground(this.CombatState.RunState);
			}
			NMapScreen instance2 = NMapScreen.Instance;
			if (instance2 != null)
			{
				instance2.SetTravelEnabled(true);
			}
			foreach (Player player in this.CombatState.RunState.Players)
			{
				player.ResetCombatState();
			}
			RunManager.Instance.ActionExecutor.Unpause();
			if (this.Encounter.ShouldGiveRewards)
			{
				await this.OfferRoomEndRewards();
			}
			else if (ncombatRoom != null)
			{
				await ncombatRoom.Ui.ProceedWithoutRewards();
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005BF00 File Offset: 0x0005A100
		public async Task OfferRoomEndRewards()
		{
			List<RewardsSet> rewards = new List<RewardsSet>();
			foreach (Player player in this.CombatState.Players)
			{
				List<RewardsSet> list = rewards;
				RewardsSet rewardsSet = await RewardsCmd.GenerateForRoomEnd(player, this);
				list.Add(rewardsSet);
				list = null;
			}
			IEnumerator<Player> enumerator = null;
			foreach (RewardsSet reward in rewards)
			{
				await Hook.BeforeCombatRewardOffered(reward, this.CombatState.RunState, this);
				TaskHelper.RunSafely(reward.Offer());
				reward = null;
			}
			List<RewardsSet>.Enumerator enumerator2 = default(List<RewardsSet>.Enumerator);
		}

		// Token: 0x0400053B RID: 1339
		private bool _isPreFinished;

		// Token: 0x0400053D RID: 1341
		private readonly Dictionary<Player, List<Reward>> _extraRewards = new Dictionary<Player, List<Reward>>();
	}
}
