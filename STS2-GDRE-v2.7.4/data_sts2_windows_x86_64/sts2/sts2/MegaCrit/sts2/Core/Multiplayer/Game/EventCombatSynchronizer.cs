using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Handles entering combat from events.
	/// This isn't a synchronizer per se, as it doesn't pass any messages.
	/// </summary>
	// Token: 0x02000494 RID: 1172
	[NullableContext(2)]
	[Nullable(0)]
	public class EventCombatSynchronizer
	{
		/// <summary>
		/// The mutable encounter that will be used when we transition to combat.
		/// This is only set if the event used to initialize the object has LayoutType == EventLayoutType.Combat.
		/// </summary>
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06004715 RID: 18197 RVA: 0x00219FA4 File Offset: 0x002181A4
		// (set) Token: 0x06004716 RID: 18198 RVA: 0x00219FAC File Offset: 0x002181AC
		public EncounterModel MutableEncounterForLayout { get; private set; }

		/// <summary>
		/// The combat state that will be used when we transition to combat.
		/// This is only set if the event used to initialize the object has LayoutType == EventLayoutType.Combat.
		/// </summary>
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004717 RID: 18199 RVA: 0x00219FB5 File Offset: 0x002181B5
		// (set) Token: 0x06004718 RID: 18200 RVA: 0x00219FBD File Offset: 0x002181BD
		public CombatState CombatStateForLayout { get; private set; }

		// Token: 0x06004719 RID: 18201 RVA: 0x00219FC8 File Offset: 0x002181C8
		[NullableContext(1)]
		public EventCombatSynchronizer(IPlayerCollection playerCollection, IRunState runState)
		{
			this._runState = runState;
			foreach (Player player in playerCollection.Players)
			{
				this._states.Add(null);
			}
		}

		/// <summary>
		/// Should be called whenever an event is entered that can transition to combat.
		/// </summary>
		// Token: 0x0600471A RID: 18202 RVA: 0x0021A044 File Offset: 0x00218244
		[NullableContext(1)]
		public void InitializeForEvent(EventModel localEvent)
		{
			this._canonicalEvent = localEvent.CanonicalInstance;
			if (localEvent.LayoutType == EventLayoutType.Combat)
			{
				if (localEvent.CanonicalEncounter == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Canonical encounter is not set for event ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(localEvent.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" with combat layout!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.MutableEncounterForLayout = localEvent.CanonicalEncounter.ToMutable();
				this.MutableEncounterForLayout.GenerateMonstersWithSlots(this._runState);
				this.CombatStateForLayout = this.CreateCombatState(this.MutableEncounterForLayout);
				foreach (Player player in this._runState.Players)
				{
					this.CombatStateForLayout.AddPlayer(player);
				}
				foreach (ValueTuple<MonsterModel, string> valueTuple in this.CombatStateForLayout.Encounter.MonstersWithSlots)
				{
					MonsterModel item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					Creature creature = this.CombatStateForLayout.CreateCreature(item, CombatSide.Enemy, item2);
					this.CombatStateForLayout.AddCreature(creature);
				}
			}
		}

		/// <summary>
		/// Called when an individual event instance is prepared to enter combat.
		/// When a shared event is in-progress, there is one EventModel instance per player. Each one calls this method
		/// individually to match the semantics of how events normally work. Once all event instances have called in, this
		/// object automatically does the transition to combat.
		/// </summary>
		/// <param name="canonicalEncounter">The canonical encounter to transition to. This must match among all calls to
		/// this method in a single shared event.</param>
		/// <param name="player">The player to which the event belongs.</param>
		/// <param name="extraRewards">Any extra rewards that should be granted to the player.</param>
		/// <param name="shouldResumeAfterCombat">Whether to resume the event post-combat or not. Must be the same among all
		/// calls to this method in a single shared event.</param>
		// Token: 0x0600471B RID: 18203 RVA: 0x0021A198 File Offset: 0x00218398
		[NullableContext(1)]
		public void ReadyToEnterCombat(EncounterModel canonicalEncounter, Player player, IReadOnlyList<Reward> extraRewards, bool shouldResumeAfterCombat)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" is ready to enter combat ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(canonicalEncounter.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" (extra rewards: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(extraRewards.Count);
			defaultInterpolatedStringHandler.AppendLiteral(", should resume after: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(shouldResumeAfterCombat);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int playerSlotIndex = this._runState.GetPlayerSlotIndex(player);
			if (this._states[playerSlotIndex] != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" became ready to enter combat ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(canonicalEncounter.Id);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendLiteral("but they are already set to ready for ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this._states[playerSlotIndex].canonicalEncounter.Id);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._states[playerSlotIndex] = new EventCombatSynchronizer.EventCombatState
			{
				canonicalEncounter = canonicalEncounter,
				extraRewards = extraRewards,
				shouldResumeAfterCombat = shouldResumeAfterCombat
			};
			if (this._states.All<EventCombatSynchronizer.EventCombatState>((EventCombatSynchronizer.EventCombatState s) => s != null))
			{
				this.EnterCombat();
			}
		}

		/// <summary>
		/// Called after all event instances call <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Game.EventCombatSynchronizer.ReadyToEnterCombat(MegaCrit.Sts2.Core.Models.EncounterModel,MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Rewards.Reward},System.Boolean)" />.
		/// Does the actual transition into combat.
		/// </summary>
		// Token: 0x0600471C RID: 18204 RVA: 0x0021A330 File Offset: 0x00218530
		private void EnterCombat()
		{
			if (this._canonicalEvent == null)
			{
				throw new InvalidOperationException("GenerateInternalCombatState must be called before EnterCombat!");
			}
			EncounterModel canonicalEncounter = this._states[0].canonicalEncounter;
			bool shouldResumeAfterCombat = this._states[0].shouldResumeAfterCombat;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			for (int i = 0; i < this._states.Count; i++)
			{
				if (this._states[i].shouldResumeAfterCombat != shouldResumeAfterCombat)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(115, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Event for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._runState.Players[i].NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" tried to start event combat");
					defaultInterpolatedStringHandler.AppendLiteral("with shouldResumeAfterCombat set to ");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(this._states[i].shouldResumeAfterCombat);
					defaultInterpolatedStringHandler.AppendLiteral(", but the host says it should be ");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(shouldResumeAfterCombat);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (this._states[i].canonicalEncounter != canonicalEncounter)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(94, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Event for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._runState.Players[i].NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" tried to start event combat");
					defaultInterpolatedStringHandler.AppendLiteral("with encounter ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this._states[i].canonicalEncounter.Id);
					defaultInterpolatedStringHandler.AppendLiteral(", but the host says it should be ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(canonicalEncounter.Id);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Entering combat ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(canonicalEncounter.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" from event");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			CombatState combatState = this.CombatStateForLayout ?? this.CreateCombatState(canonicalEncounter.ToMutable());
			CombatRoom combatRoom = new CombatRoom(combatState)
			{
				ShouldCreateCombat = (this._canonicalEvent.LayoutType != EventLayoutType.Combat),
				ShouldResumeParentEventAfterCombat = shouldResumeAfterCombat,
				ParentEventId = this._canonicalEvent.Id
			};
			foreach (EventCombatSynchronizer.EventCombatState eventCombatState in this._states)
			{
				foreach (Reward reward in eventCombatState.extraRewards)
				{
					combatRoom.AddExtraReward(reward.Player, reward);
				}
			}
			TaskHelper.RunSafely(RunManager.Instance.EnterRoomWithoutExitingCurrentRoom(combatRoom, this._canonicalEvent.LayoutType != EventLayoutType.Combat));
		}

		/// <summary>
		/// Resets the combat synchronizer state for the next combat.
		/// This must be called before another event combat can begin.
		/// </summary>
		// Token: 0x0600471D RID: 18205 RVA: 0x0021A630 File Offset: 0x00218830
		public void ResetState()
		{
			CombatState combatStateForLayout = this.CombatStateForLayout;
			foreach (Creature creature in (((combatStateForLayout != null) ? combatStateForLayout.Creatures : null) ?? Array.Empty<Creature>()))
			{
				CombatState combatStateForLayout2 = this.CombatStateForLayout;
				if (combatStateForLayout2 != null)
				{
					combatStateForLayout2.RemoveCreature(creature, true);
				}
			}
			this._canonicalEvent = null;
			this.MutableEncounterForLayout = null;
			this.CombatStateForLayout = null;
			for (int i = 0; i < this._states.Count; i++)
			{
				this._states[i] = null;
			}
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0021A6D8 File Offset: 0x002188D8
		[NullableContext(1)]
		private CombatState CreateCombatState(EncounterModel mutableEncounter)
		{
			return new CombatState(mutableEncounter, this._runState, this._runState.Modifiers, this._runState.BadgeModels, this._runState.MultiplayerScalingModel);
		}

		// Token: 0x04001BD0 RID: 7120
		[Nullable(1)]
		private readonly IRunState _runState;

		// Token: 0x04001BD1 RID: 7121
		[Nullable(new byte[] { 1, 2 })]
		private readonly List<EventCombatSynchronizer.EventCombatState> _states = new List<EventCombatSynchronizer.EventCombatState>();

		// Token: 0x04001BD2 RID: 7122
		[Nullable(1)]
		private readonly Logger _logger = new Logger("EventCombatSynchronizer", LogType.GameSync);

		// Token: 0x04001BD3 RID: 7123
		private EventModel _canonicalEvent;

		// Token: 0x020019B2 RID: 6578
		[NullableContext(1)]
		[Nullable(0)]
		[RequiredMember]
		private class EventCombatState
		{
			// Token: 0x0600AB58 RID: 43864 RVA: 0x00360734 File Offset: 0x0035E934
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public EventCombatState()
			{
			}

			// Token: 0x04006563 RID: 25955
			[RequiredMember]
			public EncounterModel canonicalEncounter;

			// Token: 0x04006564 RID: 25956
			[RequiredMember]
			public IReadOnlyList<Reward> extraRewards;

			// Token: 0x04006565 RID: 25957
			public bool shouldResumeAfterCombat;
		}
	}
}
