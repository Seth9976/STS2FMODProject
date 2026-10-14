using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000107 RID: 263
	[NullableContext(1)]
	public interface IRunState : ICardScope, IPlayerCollection
	{
		/// <summary>
		/// The Acts in this run.
		/// Acts[0] is Act 1, Acts[1] is Act 2, etc.
		/// </summary>
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000B46 RID: 2886
		IReadOnlyList<ActModel> Acts { get; }

		/// <summary>
		/// The index of the current Act we're on.
		/// Act 1 has index 0, Act 2 has index 1, etc.
		/// </summary>
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000B47 RID: 2887
		// (set) Token: 0x06000B48 RID: 2888
		int CurrentActIndex { get; set; }

		/// <summary>
		/// The current Act we're on.
		/// </summary>
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000B49 RID: 2889
		ActModel Act { get; }

		/// <summary>
		/// The map for this run's current act.
		/// Starts out with an empty map, but should be updated during the start of the run by <see cref="T:MegaCrit.Sts2.Core.Runs.RunManager" />.
		/// </summary>
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000B4A RID: 2890
		// (set) Token: 0x06000B4B RID: 2891
		ActMap Map { get; set; }

		/// <summary>
		/// The MapCoord that we're currently at in this run.
		/// Will be null before we've entered our first map point.
		/// </summary>
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000B4C RID: 2892
		MapCoord? CurrentMapCoord { get; }

		/// <summary>
		/// The game mode that we are playing.
		/// </summary>
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000B4D RID: 2893
		GameMode GameMode { get; }

		/// <summary>
		/// The MapPoint that we're currently at in this run.
		/// Will be null before we've entered our first map point.
		/// </summary>
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000B4E RID: 2894
		[Nullable(2)]
		MapPoint CurrentMapPoint
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// The <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.RunLocation" /> that we're currently at in this run.
		/// </summary>
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000B4F RID: 2895
		RunLocation RunLocation { get; }

		/// <summary>
		/// The <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.MapLocation" /> that we're currently at in this run.
		/// </summary>
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000B50 RID: 2896
		MapLocation MapLocation { get; }

		/// <summary>
		/// The floor we are on in the current act.
		/// For example, on floor 1 in act 3, this will be 1.
		/// </summary>
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000B51 RID: 2897
		// (set) Token: 0x06000B52 RID: 2898
		int ActFloor { get; set; }

		/// <summary>
		/// The floor we are on with respect to the entire run.
		/// For example, on floor 1 in act 3, this will be around 34 (17 from act 1, 16 from act 2, 1 from act 3).
		/// </summary>
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000B53 RID: 2899
		int TotalFloor { get; }

		/// <summary>
		/// The number of rooms the player is currently in.
		///
		/// This will usually be exactly 1, but here are the reasons it may be a different number:
		/// * There may be 2+ rooms when a player makes a choice that spawns a new room without traveling to a new map
		///   point. For example, in the Dense Vegetation event, when the player chooses the Rest option, a new
		///   <see cref="T:MegaCrit.Sts2.Core.Rooms.CombatRoom" /> is pushed to this stack. Then, when that combat is over, we pop that room off the
		///   stack, allowing us to return to the event.
		/// * There may be 0 rooms during brief windows, like in the middle of traveling to a new map point.
		///
		/// Whenever we travel to a new map point, the stack of rooms is cleared and the first room for that map point is
		/// pushed onto it, so this becomes 1.
		/// </summary>
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000B54 RID: 2900
		int CurrentRoomCount { get; }

		/// <summary>
		/// The room that we're currently in in this run.
		/// Usually the same as BaseRoom because there's usually only one room in the rooms stack. However, when there
		/// are multiple rooms in the stack (like at an event where one of the options starts a combat), this will be the
		/// last room that was entered in the current map point.
		/// Usually safe to treat as non-null, but may be null during brief windows, like while we're in the middle of
		/// traveling to a new map point.
		/// </summary>
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000B55 RID: 2901
		[Nullable(2)]
		AbstractRoom CurrentRoom
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// The room at the base of the rooms stack.
		/// Usually the same as CurrentRoom because there's usually only one room in the rooms stack. However, when there
		/// are multiple rooms in the stack (like at an event where one of the options starts a combat), this will be the
		/// first room that was entered in the current map point.
		/// Usually safe to treat as non-null, but may be null during brief windows, like while we're in the middle of
		/// traveling to a new map point.
		/// </summary>
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000B56 RID: 2902
		[Nullable(2)]
		AbstractRoom BaseRoom
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Is the run in the "game over" state? i.e. are all players dead?
		/// </summary>
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000B57 RID: 2903
		bool IsGameOver { get; }

		/// <summary>
		/// The Ascension difficulty of this run.
		/// </summary>
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000B58 RID: 2904
		int AscensionLevel { get; }

		/// <summary>
		/// This run's set of run-level RNGs.
		/// </summary>
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000B59 RID: 2905
		RunRngSet Rng { get; }

		/// <summary>
		/// This run's set of run-level odds.
		/// </summary>
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000B5A RID: 2906
		RunOddsSet Odds { get; }

		/// <summary>
		/// The grab bag used in multiplayer for shared relic picking.
		/// When pulling relics for the shared screen where everyone chooses one relic of many, we want to attempt to pick
		/// relics that no one has. When a player pulls a relic from their individual grab bag, they also remove the relic
		/// from this grab bag. Refresh is allowed on this grab bag so that if we run out of relics of a certain rarity,
		/// duplicates are allowed to show up.
		/// </summary>
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000B5B RID: 2907
		RelicGrabBag SharedRelicGrabBag { get; }

		/// <summary>
		/// The unlock state for the run.
		/// In multiplayer, this encompasses the unlocks for _all_ players in the run. Only use this for things that are
		/// shared among players, like the shared treasure relics, or ancients. For checking player-specific unlocks, e.g.
		/// for player-specific rewards, use the unlock state on the individual players.
		/// In singleplayer, this is equivalent to the player's unlock state.
		/// </summary>
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000B5C RID: 2908
		UnlockState UnlockState { get; }

		/// <summary>
		/// List of custom modifiers applied to this run, for daily or custom runs.
		/// </summary>
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000B5D RID: 2909
		IReadOnlyList<ModifierModel> Modifiers { get; }

		/// <summary>
		/// Models which unlock badges for the run.
		/// </summary>
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000B5E RID: 2910
		IReadOnlyList<BadgeModel> BadgeModels { get; }

		/// <summary>
		/// The model used to scale various things (block, power application) in multiplayer.
		/// </summary>
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000B5F RID: 2911
		[Nullable(2)]
		MultiplayerScalingModel MultiplayerScalingModel
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// The map point history for the current run.
		/// MapPointHistory[0] contains all the MapPointHistoryEntries for act 1, MapPointHistory[1] for act 2, etc.
		/// </summary>
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000B60 RID: 2912
		IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory { get; }

		/// <summary>
		/// The history entry for the current map point.
		/// Can be null for a brief period at the very start of an act, before the Ancient room has been entered.
		/// </summary>
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000B61 RID: 2913
		[Nullable(2)]
		MapPointHistoryEntry CurrentMapPointHistoryEntry
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Extra fields that are used for specific pieces of content within the run.
		/// </summary>
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000B62 RID: 2914
		ExtraRunFields ExtraFields { get; }

		/// <summary>
		/// Convenience property so that we can easily check if we should filter out cards for singleplayer or multiplayer.
		/// </summary>
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0004C564 File Offset: 0x0004A764
		CardMultiplayerConstraint CardMultiplayerConstraint
		{
			get
			{
				if (this.Players.Count <= 1)
				{
					return CardMultiplayerConstraint.SingleplayerOnly;
				}
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		/// <summary>
		/// Does this state contain the specified card?
		/// </summary>
		// Token: 0x06000B64 RID: 2916
		bool ContainsCard(CardModel card);

		/// <summary>
		/// Deserialize the specified card and add it to this state.
		/// </summary>
		/// <param name="serializableCard">Serialized card to deserialize.</param>
		/// <param name="owner">The player who should own this card.</param>
		// Token: 0x06000B65 RID: 2917
		CardModel LoadCard(SerializableCard serializableCard, Player owner);

		/// <summary>
		/// Add the specified map point type and room type to the map point history.
		/// </summary>
		/// <param name="mapPointType">The type of map point that the player selected.</param>
		/// <param name="initialRoomType">
		/// The type of room that the player first entered in this map point.
		/// This is usually the _only_ room that they entered in this map point (for example, if MapPointType = Monster,
		/// RoomType will always be Monster).
		/// The most common exception is for events that contain encounters.
		/// In these cases, the player will select a MapPointType = Unknown, the initial room type will be Event, but the
		/// player will later enter another RoomType (Monster) in the same map point.
		/// This isn't tracked by the map point history though, so don't worry about it here.
		/// </param>
		/// <param name="modelId">The model ID of the encounter or event associated with the room, if any.</param>
		// Token: 0x06000B66 RID: 2918
		[NullableContext(2)]
		void AppendToMapPointHistory(MapPointType mapPointType, RoomType initialRoomType, ModelId modelId);

		/// <summary>
		/// Get the MapPointHistoryEntry for the specified location.
		/// Can be null if the player debug-travelled to a location.
		/// </summary>
		// Token: 0x06000B67 RID: 2919
		[NullableContext(2)]
		MapPointHistoryEntry GetHistoryEntryFor(MapLocation location);

		/// <summary>
		/// Get all the models that should have run hooks called on them.
		/// </summary>
		/// <param name="childCombatState">
		/// The combat state that the players are currently in.
		/// If this is null, the players are not in combat, and we should manually return all models.
		/// If this is not null, the players are in combat, and we should manually return only non-combat models (like deck
		/// cards) while delegating the rest to the combat state.
		/// </param>
		// Token: 0x06000B68 RID: 2920
		IEnumerable<AbstractModel> IterateHookListeners([Nullable(2)] ICombatState childCombatState);

		/// <summary>
		/// The next room index.
		/// This is an incrementing integer that IDs the next room that the player will enter.
		/// </summary>
		// Token: 0x06000B69 RID: 2921
		int GetAndIncrementNextRoomId();

		/// <summary>
		/// Find an IRunState from a list of creatures.
		/// Tries to return a concrete <see cref="T:MegaCrit.Sts2.Core.Runs.RunState" />, but falls back to <see cref="T:MegaCrit.Sts2.Core.Runs.NullRunState" /> if none is found.
		/// </summary>
		// Token: 0x06000B6A RID: 2922 RVA: 0x0004C578 File Offset: 0x0004A778
		public static IRunState GetFrom(IEnumerable<Creature> creatures)
		{
			Creature creature = creatures.FirstOrDefault<Creature>((Creature c) => c.IsPlayer);
			if (creature != null)
			{
				return creature.Player.RunState;
			}
			Creature creature2 = creatures.FirstOrDefault<Creature>((Creature c) => c.PetOwner != null);
			if (creature2 != null)
			{
				return creature2.PetOwner.RunState;
			}
			Creature creature3 = creatures.FirstOrDefault<Creature>((Creature c) => c.CombatState != null);
			if (creature3 != null)
			{
				return creature3.CombatState.RunState;
			}
			Log.Warn("Unable to extract RunState from creatures list! If you're in a test, this is okay, but it's probably a bug outside of tests. Falling back to null run state.", 2);
			return NullRunState.Instance;
		}
	}
}
