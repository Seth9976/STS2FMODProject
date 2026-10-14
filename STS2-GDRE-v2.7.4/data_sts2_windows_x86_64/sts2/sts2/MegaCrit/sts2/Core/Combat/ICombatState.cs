using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Combat
{
	// Token: 0x02000D44 RID: 3396
	[NullableContext(1)]
	public interface ICombatState
	{
		/// <summary>
		/// The state of the run that this combat exists in.
		/// Will be <see cref="P:MegaCrit.Sts2.Core.Combat.ICombatState.RunState" /> in gameplay and <see cref="T:MegaCrit.Sts2.Core.Runs.NullRunState" /> in some test/debug scenarios.
		/// </summary>
		// Token: 0x170025A5 RID: 9637
		// (get) Token: 0x06008973 RID: 35187
		IRunState RunState { get; }

		/// <summary>
		/// Fired whenever the arrangement of creatures in the combat changes. Specifically, when:
		/// * A creature is added.
		/// * A creature is removed.
		/// * A creature's index changes.
		/// </summary>
		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06008974 RID: 35188
		// (remove) Token: 0x06008975 RID: 35189
		[Nullable(new byte[] { 2, 1 })]
		event Action<ICombatState> CreaturesChanged;

		/// <summary>
		/// Get all creatures on the Allies side.
		/// </summary>
		// Token: 0x170025A6 RID: 9638
		// (get) Token: 0x06008976 RID: 35190
		IReadOnlyList<Creature> Allies { get; }

		/// <summary>
		/// Get all creatures on the Enemies side.
		/// </summary>
		// Token: 0x170025A7 RID: 9639
		// (get) Token: 0x06008977 RID: 35191
		IReadOnlyList<Creature> Enemies { get; }

		/// <summary>
		/// Get all creatures in the combat on all sides.
		/// </summary>
		// Token: 0x170025A8 RID: 9640
		// (get) Token: 0x06008978 RID: 35192
		IReadOnlyList<Creature> Creatures { get; }

		/// <summary>
		/// Get all the player creatures in the combat.
		/// </summary>
		// Token: 0x170025A9 RID: 9641
		// (get) Token: 0x06008979 RID: 35193
		IReadOnlyList<Creature> PlayerCreatures { get; }

		/// <summary>
		/// Get all players in the combat.
		/// </summary>
		// Token: 0x170025AA RID: 9642
		// (get) Token: 0x0600897A RID: 35194
		IReadOnlyList<Player> Players { get; }

		/// <summary>
		/// List of custom modifiers applied to this combat, usually via daily or custom runs.
		/// </summary>
		// Token: 0x170025AB RID: 9643
		// (get) Token: 0x0600897B RID: 35195
		IReadOnlyList<ModifierModel> Modifiers { get; }

		/// <summary>
		/// The model used to scale various things (block, power application) in multiplayer.
		/// </summary>
		// Token: 0x170025AC RID: 9644
		// (get) Token: 0x0600897C RID: 35196
		[Nullable(2)]
		MultiplayerScalingModel MultiplayerScalingModel
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// BE CAREFUL! You usually want <see cref="P:MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState.TurnNumber" /> instead of this.
		/// The round of combat that we're on in this combat.
		/// A "round" encompasses both the player's turn (or turns if a player takes an extra turn) and the enemy's turn.
		/// This starts at 1, so it should never be 0.
		/// </summary>
		// Token: 0x170025AD RID: 9645
		// (get) Token: 0x0600897D RID: 35197
		// (set) Token: 0x0600897E RID: 35198
		int RoundNumber { get; set; }

		/// <summary>
		/// The side that is active in this combat.
		/// A round starts with the player side being active, then switches to the enemy side after all players have ended
		/// their turn.
		/// When the enemy turn ends, the current side changes back to the player side, and the round number is incremented.
		/// </summary>
		// Token: 0x170025AE RID: 9646
		// (get) Token: 0x0600897F RID: 35199
		// (set) Token: 0x06008980 RID: 35200
		CombatSide CurrentSide { get; set; }

		// Token: 0x170025AF RID: 9647
		// (get) Token: 0x06008981 RID: 35201
		[Nullable(2)]
		EncounterModel Encounter
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// A list of creatures that escaped (were removed without dying) during the encounter. Used when rewards are given.
		/// </summary>
		// Token: 0x170025B0 RID: 9648
		// (get) Token: 0x06008982 RID: 35202
		IReadOnlyList<Creature> EscapedCreatures { get; }

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard``1(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06008983 RID: 35203
		T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel;

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06008984 RID: 35204
		CardModel CreateCard(CardModel canonicalCard, Player owner);

		/// <summary>
		/// WARNING: If you're specifically intending to create a clone in combat for an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.DualWield" />,
		/// you should use <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.CreateClone" /> instead.
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CloneCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06008985 RID: 35205
		CardModel CloneCard(CardModel mutableCard);

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.AddCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06008986 RID: 35206
		void AddCard(CardModel card, Player owner);

		// Token: 0x06008987 RID: 35207
		void RemoveCard(CardModel card);

		/// <summary>
		/// Does this state contain the specified card?
		/// </summary>
		// Token: 0x06008988 RID: 35208
		bool ContainsCard(CardModel card);

		/// <summary>
		/// Add a player's creature on the Allies side.
		/// </summary>
		// Token: 0x06008989 RID: 35209
		void AddPlayer(Player player);

		/// <summary>
		/// Creates a new creature from a monster in this combat state.
		/// After you create the creature, you should call AddCreature to add it to combat. This can be done immediately
		/// or after some time.
		/// </summary>
		// Token: 0x0600898A RID: 35210
		Creature CreateCreature(MonsterModel monster, CombatSide side, [Nullable(2)] string slot);

		/// <summary>
		/// Call this to remove a creature that escaped rather than dying.
		/// </summary>
		// Token: 0x0600898B RID: 35211
		void CreatureEscaped(Creature creature);

		/// <summary>
		/// Removes the creature from combat.
		/// </summary>
		/// <param name="creature">The creature that will be removed.</param>
		/// <param name="unattach">If true, then the creature cannot be re-added to the combat state.</param>
		// Token: 0x0600898C RID: 35212
		void RemoveCreature(Creature creature, bool unattach = true);

		// Token: 0x0600898D RID: 35213
		bool ContainsCreature(Creature creature);

		// Token: 0x0600898E RID: 35214
		[NullableContext(0)]
		bool ContainsMonster<T>() where T : MonsterModel;

		/// <summary>
		/// Get the creature with the specified combat ID. Null if not found.
		/// </summary>
		// Token: 0x0600898F RID: 35215
		[NullableContext(2)]
		Creature GetCreature(uint? combatId);

		/// <summary>
		/// Get the creature with the specified combat ID.
		/// If the creature doesn't exist, keep checking for a while before timing out and returning null.
		/// </summary>
		/// <param name="combatId">Combat ID of the creature to get.</param>
		/// <param name="timeoutSec">How long to wait for the creature to appear.</param>
		/// <returns>Specified Creature, or null if it doesn't exist and we've waited long enough.</returns>
		// Token: 0x06008990 RID: 35216
		[return: Nullable(new byte[] { 1, 2 })]
		Task<Creature> GetCreatureAsync(uint? combatId, double timeoutSec);

		/// <summary>
		/// Get all the creatures on the specified side.
		/// </summary>
		// Token: 0x06008991 RID: 35217
		IReadOnlyList<Creature> GetCreaturesOnSide(CombatSide side);

		/// <summary>
		/// Get all the creatures on the currently-active side.
		/// </summary>
		// Token: 0x170025B1 RID: 9649
		// (get) Token: 0x06008992 RID: 35218
		IReadOnlyList<Creature> CreaturesOnCurrentSide { get; }

		/// <summary>
		/// Get all opponents of a creature.
		/// </summary>
		// Token: 0x06008993 RID: 35219
		IReadOnlyList<Creature> GetOpponentsOf(Creature creature);

		/// <summary>
		/// Get all teammates of a creature, including the creature itself.
		/// </summary>
		// Token: 0x06008994 RID: 35220
		IReadOnlyList<Creature> GetTeammatesOf(Creature creature);

		/// <summary>
		/// Get all hittable enemies.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Creatures.Creature.IsHittable" /> for a definition.
		///
		/// NOTE: We shouldn't add too many methods like this, this one is just extremely common because it's used for AOE
		/// and random attack targeting.
		/// </summary>
		// Token: 0x170025B2 RID: 9650
		// (get) Token: 0x06008995 RID: 35221
		IReadOnlyList<Creature> HittableEnemies { get; }

		// Token: 0x06008996 RID: 35222
		[NullableContext(2)]
		Player GetPlayer(ulong playerId);

		/// <summary>
		/// Get all the models that should have combat hooks called on them.
		/// </summary>
		// Token: 0x06008997 RID: 35223
		IEnumerable<AbstractModel> IterateHookListeners();

		// Token: 0x06008998 RID: 35224
		void SortEnemiesBySlotName();

		// Token: 0x06008999 RID: 35225
		void SetEnemyIndex(Creature creature, int index);

		/// <summary>
		/// Adds a creature to the combat.
		/// This should almost always be called right after CreateCreature. Until this is called, creatures cannot be targeted
		/// and their powers will not be triggered. It is separate so that creatures can be removed from combat and re-added
		/// to it later. Currently, this is only used in the Doormaker combat.
		/// </summary>
		/// <param name="creature">The creature to add to the combat.</param>
		// Token: 0x0600899A RID: 35226
		void AddCreature(Creature creature);

		/// <summary>
		/// Returns true if this represents a live combat.
		/// If this is false, then you should only trust that whatever object you retrieved this combat state from (like a
		/// Creature) exists, and not any other object, like other Creatures or the NCombatRoom.
		/// </summary>
		// Token: 0x0600899B RID: 35227
		bool IsLiveCombat();
	}
}
