using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Hooks
{
	/// <summary>
	/// A static class containing all of the gameplay hooks.
	/// </summary>
	// Token: 0x02000BFE RID: 3070
	[NullableContext(1)]
	[Nullable(0)]
	public static class Hook
	{
		/// <summary>
		/// Iterates combat hook listeners, but yields nothing if combat is over or ending
		/// (CombatManager.IsOverOrEnding) when the dispatch begins. Most combat hooks should use this
		/// instead of <see cref="M:MegaCrit.Sts2.Core.Combat.ICombatState.IterateHookListeners" /> directly, so a hook dispatched
		/// after combat has started ending fires for no one (for example after a Strike that Hellraiser
		/// plays automatically kills the last enemy while cards are still being drawn).
		///
		/// The check is evaluated once, when enumeration begins, not per listener. A dispatch that
		/// begins while combat is live therefore runs every listener even if one of them ends combat
		/// partway through. That is intentional: combat teardown is deferred to the next safe point
		/// (CheckWinCondition), so the state stays intact for the rest of the dispatch, and rechecking
		/// per listener would drop the remaining ones in listener order (for example a Joss Paper
		/// increment that should still count when Charon's Ashes lands the killing blow on an exhaust).
		///
		/// Combat setup (CombatManager.IsStarting) is exempt: IsInProgress is still false then, so
		/// IsOverOrEnding is true, but hooks that run during setup, such as the initial deck shuffle
		/// (ModifyShuffleOrder with isInitialShuffle true), must still reach listeners.
		///
		/// A few hooks intentionally call <see cref="M:MegaCrit.Sts2.Core.Combat.ICombatState.IterateHookListeners" /> directly
		/// because they are part of the kill, death, or combat end sequence itself, where yielding
		/// nothing would break that sequence. Each such hook documents the reason in its own summary.
		/// </summary>
		// Token: 0x06007EDC RID: 32476 RVA: 0x002AF14E File Offset: 0x002AD34E
		private static IEnumerable<AbstractModel> IterateCombatHookListeners(ICombatState combatState)
		{
			Hook.<IterateCombatHookListeners>d__0 <IterateCombatHookListeners>d__ = new Hook.<IterateCombatHookListeners>d__0(-2);
			<IterateCombatHookListeners>d__.<>3__combatState = combatState;
			return <IterateCombatHookListeners>d__;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterActEntered" />.
		/// </summary>
		// Token: 0x06007EDD RID: 32477 RVA: 0x002AF160 File Offset: 0x002AD360
		public static async Task AfterActEntered(IRunState runState)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterActEntered();
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeAttack(MegaCrit.Sts2.Core.Commands.Builders.AttackCommand)" />.
		/// </summary>
		// Token: 0x06007EDE RID: 32478 RVA: 0x002AF1A4 File Offset: 0x002AD3A4
		public static async Task BeforeAttack(ICombatState combatState, AttackCommand command)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.BeforeAttack(command);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterAttack(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Commands.Builders.AttackCommand)" />.
		/// </summary>
		// Token: 0x06007EDF RID: 32479 RVA: 0x002AF1F0 File Offset: 0x002AD3F0
		public static async Task AfterAttack(ICombatState combatState, PlayerChoiceContext choiceContext, AttackCommand command)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterAttack(choiceContext, command);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterBlockBroken(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it fires from the same
		/// damage event that ends combat (the killing hit), so it must still resolve for that hit.
		/// </summary>
		// Token: 0x06007EE0 RID: 32480 RVA: 0x002AF244 File Offset: 0x002AD444
		public static async Task AfterBlockBroken(ICombatState combatState, PlayerChoiceContext choiceContext, Creature target, [Nullable(2)] Creature breaker)
		{
			foreach (AbstractModel model in combatState.IterateHookListeners())
			{
				await model.AfterBlockBroken(choiceContext, target, breaker);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterBlockCleared(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007EE1 RID: 32481 RVA: 0x002AF2A0 File Offset: 0x002AD4A0
		public static async Task AfterBlockCleared(ICombatState combatState, Creature creature)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterBlockCleared(creature);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeBlockGained(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EE2 RID: 32482 RVA: 0x002AF2EC File Offset: 0x002AD4EC
		public static async Task BeforeBlockGained(ICombatState combatState, Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.BeforeBlockGained(creature, amount, props, cardSource);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterBlockGained(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EE3 RID: 32483 RVA: 0x002AF350 File Offset: 0x002AD550
		public static async Task AfterBlockGained(ICombatState combatState, Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterBlockGained(creature, amount, props, cardSource);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeCardAutoPlayed(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Entities.Cards.AutoPlayType)" />.
		/// </summary>
		// Token: 0x06007EE4 RID: 32484 RVA: 0x002AF3B4 File Offset: 0x002AD5B4
		public static async Task BeforeCardAutoPlayed(ICombatState combatState, CardModel card, [Nullable(2)] Creature target, AutoPlayType type)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.BeforeCardAutoPlayed(card, target, type);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardChangedPiles(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.PileType,MegaCrit.Sts2.Core.Models.AbstractModel)" />.
		/// </summary>
		// Token: 0x06007EE5 RID: 32485 RVA: 0x002AF410 File Offset: 0x002AD610
		public static async Task AfterCardChangedPiles(IRunState runState, [Nullable(2)] ICombatState combatState, CardModel card, PileType oldPile, [Nullable(2)] AbstractModel clonedBy)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCardChangedPiles(card, oldPile, clonedBy);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCardChangedPilesLate(card, oldPile, clonedBy);
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardDiscarded(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007EE6 RID: 32486 RVA: 0x002AF474 File Offset: 0x002AD674
		public static async Task AfterCardDiscarded(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterCardDiscarded(choiceContext, card);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardDrawn(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.CardModel,System.Boolean)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007EE7 RID: 32487 RVA: 0x002AF4C8 File Offset: 0x002AD6C8
		public static async Task AfterCardDrawn(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterCardDrawnEarly(choiceContext, card, fromHandDraw);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterCardDrawn(choiceContext, card, fromHandDraw);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardEnteredCombat(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EE8 RID: 32488 RVA: 0x002AF524 File Offset: 0x002AD724
		public static async Task AfterCardEnteredCombat(ICombatState combatState, CardModel card)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterCardEnteredCombat(card);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardExhausted(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.CardModel,System.Boolean)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007EE9 RID: 32489 RVA: 0x002AF570 File Offset: 0x002AD770
		public static async Task AfterCardExhausted(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterCardExhausted(choiceContext, card, causedByEthereal);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardGeneratedForCombat(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007EEA RID: 32490 RVA: 0x002AF5CC File Offset: 0x002AD7CC
		public static async Task AfterCardGeneratedForCombat(ICombatState combatState, CardModel card, [Nullable(2)] Player creator)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterCardGeneratedForCombat(card, creator);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeCardPlayed(MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		// Token: 0x06007EEB RID: 32491 RVA: 0x002AF620 File Offset: 0x002AD820
		public static async Task BeforeCardPlayed(ICombatState combatState, CardPlay cardPlay)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.BeforeCardPlayed(cardPlay);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardPlayed(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it completes
		/// resolution of the card that caused the kill.
		/// </summary>
		// Token: 0x06007EEC RID: 32492 RVA: 0x002AF66C File Offset: 0x002AD86C
		public static async Task AfterCardPlayed(ICombatState combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			foreach (AbstractModel model in combatState.IterateHookListeners())
			{
				choiceContext.PushModel(model);
				await model.AfterCardPlayed(choiceContext, cardPlay);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in combatState.IterateHookListeners())
			{
				choiceContext.PushModel(model);
				await model.AfterCardPlayedLate(choiceContext, cardPlay);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeCardRemoved(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EED RID: 32493 RVA: 0x002AF6C0 File Offset: 0x002AD8C0
		public static async Task BeforeCardRemoved(IRunState runState, CardModel card)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.BeforeCardRemoved(card);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeCombatStart" />.
		/// </summary>
		// Token: 0x06007EEE RID: 32494 RVA: 0x002AF70C File Offset: 0x002AD90C
		public static async Task BeforeCombatStart(IRunState runState, [Nullable(2)] ICombatState combatState)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.BeforeCombatStart();
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.BeforeCombatStartLate();
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom)" />.
		/// </summary>
		// Token: 0x06007EEF RID: 32495 RVA: 0x002AF758 File Offset: 0x002AD958
		public static async Task AfterCombatEnd(IRunState runState, [Nullable(2)] ICombatState combatState, CombatRoom room)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCombatEnd(room);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeCombatRewardOffered(MegaCrit.Sts2.Core.Rewards.RewardsSet,MegaCrit.Sts2.Core.Rooms.CombatRoom)" />.
		/// </summary>
		// Token: 0x06007EF0 RID: 32496 RVA: 0x002AF7AC File Offset: 0x002AD9AC
		public static async Task BeforeCombatRewardOffered(RewardsSet rewards, IRunState runState, CombatRoom room)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.BeforeCombatRewardOffered(rewards, room);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCombatVictory(MegaCrit.Sts2.Core.Rooms.CombatRoom)" />.
		/// </summary>
		// Token: 0x06007EF1 RID: 32497 RVA: 0x002AF800 File Offset: 0x002ADA00
		public static async Task AfterCombatVictory(IRunState runState, [Nullable(2)] ICombatState combatState, CombatRoom room)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCombatVictoryEarly(room);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCombatVictory(room);
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCreatureAddedToCombat(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it only fires for
		/// creatures added during combat (creatures present when combat starts take a different path,
		/// before IsInProgress is set), and a creature added while combat is ending can affect whether
		/// it should actually end, so that decision is not overridden here.
		/// </summary>
		// Token: 0x06007EF2 RID: 32498 RVA: 0x002AF854 File Offset: 0x002ADA54
		public static async Task AfterCreatureAddedToCombat(ICombatState combatState, Creature creature)
		{
			foreach (AbstractModel model in combatState.IterateHookListeners())
			{
				await model.AfterCreatureAddedToCombat(creature);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCurrentHpChanged(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007EF3 RID: 32499 RVA: 0x002AF8A0 File Offset: 0x002ADAA0
		public static async Task AfterCurrentHpChanged(IRunState runState, [Nullable(2)] ICombatState combatState, Creature creature, decimal delta)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterCurrentHpChanged(creature, delta);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterDamageGiven(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Entities.Creatures.DamageResult,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it fires from the same
		/// damage event that ends combat (the killing hit), so it must still resolve for that hit.
		/// </summary>
		// Token: 0x06007EF4 RID: 32500 RVA: 0x002AF8FC File Offset: 0x002ADAFC
		public static async Task AfterDamageGiven(PlayerChoiceContext choiceContext, ICombatState combatState, [Nullable(2)] Creature dealer, DamageResult results, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in combatState.IterateHookListeners())
			{
				choiceContext.PushModel(model);
				await model.AfterDamageGiven(choiceContext, dealer, results, props, target, cardSource);
				choiceContext.PopModel(model);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeDamageReceived(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EF5 RID: 32501 RVA: 0x002AF974 File Offset: 0x002ADB74
		public static async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, IRunState runState, [Nullable(2)] ICombatState combatState, Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.BeforeDamageReceived(choiceContext, target, amount, props, dealer, cardSource);
				choiceContext.PopModel(model);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterDamageReceived(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Entities.Creatures.DamageResult,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007EF6 RID: 32502 RVA: 0x002AF9F4 File Offset: 0x002ADBF4
		public static async Task AfterDamageReceived(PlayerChoiceContext choiceContext, IRunState runState, [Nullable(2)] ICombatState combatState, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterDamageReceived(choiceContext, target, result, props, dealer, cardSource);
				choiceContext.PopModel(model);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterDamageReceivedLate(choiceContext, target, result, props, dealer, cardSource);
				choiceContext.PopModel(model);
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeDeath(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007EF7 RID: 32503 RVA: 0x002AFA74 File Offset: 0x002ADC74
		public static async Task BeforeDeath(IRunState runState, [Nullable(2)] ICombatState combatState, Creature creature)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.BeforeDeath(creature);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterDeath(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Boolean,System.Single)" />.
		/// </summary>
		// Token: 0x06007EF8 RID: 32504 RVA: 0x002AFAC8 File Offset: 0x002ADCC8
		public static async Task AfterDeath(IRunState runState, [Nullable(2)] ICombatState combatState, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				foreach (AbstractModel model in runState.IterateHookListeners(combatState))
				{
					HookPlayerChoiceContext hookPlayerChoiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, (combatState == null) ? GameActionType.NonCombat : GameActionType.Combat);
					Task task = model.AfterDeath(hookPlayerChoiceContext, creature, wasRemovalPrevented, deathAnimLength);
					await hookPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					model.InvokeExecutionFinished();
					model = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterGoldGained(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007EF9 RID: 32505 RVA: 0x002AFB2C File Offset: 0x002ADD2C
		public static async Task AfterGoldGained(IRunState runState, Player player)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterGoldGained(player);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterDiedToDoom(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it runs during death
		/// resolution, which proceeds while combat is ending.
		/// </summary>
		// Token: 0x06007EFA RID: 32506 RVA: 0x002AFB78 File Offset: 0x002ADD78
		public static async Task AfterDiedToDoom(ICombatState combatState, IReadOnlyList<Creature> creatures)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				foreach (AbstractModel model in combatState.IterateHookListeners())
				{
					HookPlayerChoiceContext hookPlayerChoiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, GameActionType.Combat);
					Task task = model.AfterDiedToDoom(hookPlayerChoiceContext, creatures);
					await hookPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					model.InvokeExecutionFinished();
					model = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterEnergyReset(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007EFB RID: 32507 RVA: 0x002AFBC4 File Offset: 0x002ADDC4
		public static async Task AfterEnergyReset(ICombatState combatState, Player player)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterEnergyReset(player);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterEnergyResetLate(player);
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterEnergySpent(MegaCrit.Sts2.Core.Models.CardModel,System.Int32)" />.
		/// </summary>
		// Token: 0x06007EFC RID: 32508 RVA: 0x002AFC10 File Offset: 0x002ADE10
		public static async Task AfterEnergySpent(ICombatState combatState, CardModel card, int amount)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterEnergySpent(card, amount);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeFlush(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007EFD RID: 32509 RVA: 0x002AFC64 File Offset: 0x002ADE64
		public static async Task BeforeFlush(ICombatState combatState, Player player)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				List<Task> tasksToAwait = new List<Task>();
				foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel, netId.Value, combatState, GameActionType.Combat);
					Task task = abstractModel.BeforeFlush(playerChoiceContext, player);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
				foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel2, netId.Value, combatState, GameActionType.Combat);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(abstractModel2.BeforeFlushLate(playerChoiceContext, player));
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				enumerator = null;
				await Task.WhenAll(tasksToAwait);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterFlush(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IReadOnlyCollection{MegaCrit.Sts2.Core.Models.CardModel},System.Collections.Generic.IReadOnlyCollection{MegaCrit.Sts2.Core.Models.CardModel})" />.
		/// </summary>
		// Token: 0x06007EFE RID: 32510 RVA: 0x002AFCB0 File Offset: 0x002ADEB0
		public static async Task AfterFlush(ICombatState combatState, Player player, PlayerChoiceContext playerChoiceContext, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				playerChoiceContext.PushModel(model);
				await model.AfterFlush(playerChoiceContext, player, flushedCards, retainedCards);
				model.InvokeExecutionFinished();
				playerChoiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterForge(System.Decimal,MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Models.AbstractModel)" />.
		/// </summary>
		// Token: 0x06007EFF RID: 32511 RVA: 0x002AFD14 File Offset: 0x002ADF14
		public static async Task AfterForge(ICombatState combatState, decimal amount, Player forger, [Nullable(2)] AbstractModel source)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterForge(amount, forger, source);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeHandDraw(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Combat.ICombatState)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007F00 RID: 32512 RVA: 0x002AFD70 File Offset: 0x002ADF70
		public static async Task BeforeHandDraw(ICombatState combatState, Player player, PlayerChoiceContext playerChoiceContext)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				playerChoiceContext.PushModel(model);
				await model.BeforeHandDraw(player, playerChoiceContext, combatState);
				model.InvokeExecutionFinished();
				playerChoiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				playerChoiceContext.PushModel(model);
				await model.BeforeHandDrawLate(player, playerChoiceContext, combatState);
				model.InvokeExecutionFinished();
				playerChoiceContext.PopModel(model);
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterHandEmptied(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007F01 RID: 32513 RVA: 0x002AFDC4 File Offset: 0x002ADFC4
		public static async Task AfterHandEmptied(ICombatState combatState, PlayerChoiceContext choiceContext, Player player)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterHandEmptied(choiceContext, player);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterItemPurchased(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Entities.Merchant.MerchantEntry,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F02 RID: 32514 RVA: 0x002AFE18 File Offset: 0x002AE018
		public static async Task AfterItemPurchased(IRunState runState, Player player, MerchantEntry itemPurchased, int goldSpent)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterItemPurchased(player, itemPurchased, goldSpent);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterMapGenerated(MegaCrit.Sts2.Core.Map.ActMap,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F03 RID: 32515 RVA: 0x002AFE74 File Offset: 0x002AE074
		public static async Task AfterMapGenerated(IRunState runState, ActMap map, int actIndex)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterMapGenerated(map, actIndex);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingBlockAmount(System.Decimal,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		// Token: 0x06007F04 RID: 32516 RVA: 0x002AFEC8 File Offset: 0x002AE0C8
		public static async Task AfterModifyingBlockAmount(ICombatState combatState, decimal modifiedBlock, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingBlockAmount(modifiedBlock, cardSource, cardPlay);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingCardPlayCount(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F05 RID: 32517 RVA: 0x002AFF2C File Offset: 0x002AE12C
		public static async Task AfterModifyingCardPlayCount(ICombatState combatState, CardModel card, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingCardPlayCount(card);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingCardRewardOptions" />.
		/// </summary>
		// Token: 0x06007F06 RID: 32518 RVA: 0x002AFF80 File Offset: 0x002AE180
		public static async Task AfterModifyingCardRewardOptions(IRunState runState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(null))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingCardRewardOptions();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingDamageAmount(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F07 RID: 32519 RVA: 0x002AFFCC File Offset: 0x002AE1CC
		public static async Task AfterModifyingDamageAmount(IRunState runState, [Nullable(2)] ICombatState combatState, [Nullable(2)] CardModel cardSource, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingDamageAmount(cardSource);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingEnergyGain" />.
		/// </summary>
		// Token: 0x06007F08 RID: 32520 RVA: 0x002B0028 File Offset: 0x002AE228
		public static async Task AfterModifyingEnergyGain(ICombatState combatState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingEnergyGain();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingGoldGained(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F09 RID: 32521 RVA: 0x002B0074 File Offset: 0x002AE274
		public static async Task AfterModifyingGoldGained(IRunState runState, [Nullable(2)] ICombatState combatState, IEnumerable<AbstractModel> modifiers, Player player, decimal amount)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingGoldGained(player, amount);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingHandDraw" />.
		/// </summary>
		// Token: 0x06007F0A RID: 32522 RVA: 0x002B00D8 File Offset: 0x002AE2D8
		public static async Task AfterModifyingHandDraw(ICombatState combatState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingHandDraw();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingHpLostBeforeOsty" />.
		/// </summary>
		// Token: 0x06007F0B RID: 32523 RVA: 0x002B0124 File Offset: 0x002AE324
		public static async Task AfterModifyingHpLostBeforeOsty(IRunState runState, [Nullable(2)] ICombatState combatState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingHpLostBeforeOsty();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingHpLostAfterOsty" />.
		/// </summary>
		// Token: 0x06007F0C RID: 32524 RVA: 0x002B0178 File Offset: 0x002AE378
		public static async Task AfterModifyingHpLostAfterOsty(IRunState runState, [Nullable(2)] ICombatState combatState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingHpLostAfterOsty();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingOrbPassiveTriggerCount(MegaCrit.Sts2.Core.Models.OrbModel)" />.
		/// </summary>
		// Token: 0x06007F0D RID: 32525 RVA: 0x002B01CC File Offset: 0x002AE3CC
		public static async Task AfterModifyingOrbPassiveTriggerCount(ICombatState combatState, OrbModel orb, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingOrbPassiveTriggerCount(orb);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingPowerAmountGiven(MegaCrit.Sts2.Core.Models.PowerModel)" />.
		/// </summary>
		// Token: 0x06007F0E RID: 32526 RVA: 0x002B0220 File Offset: 0x002AE420
		public static async Task AfterModifyingPowerAmountGiven(ICombatState combatState, IEnumerable<AbstractModel> modifiers, PowerModel modifiedPower)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingPowerAmountGiven(modifiedPower);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingPowerAmountReceived(MegaCrit.Sts2.Core.Models.PowerModel)" />.
		/// </summary>
		// Token: 0x06007F0F RID: 32527 RVA: 0x002B0274 File Offset: 0x002AE474
		public static async Task AfterModifyingPowerAmountReceived(ICombatState combatState, IEnumerable<AbstractModel> modifiers, PowerModel modifiedPower)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingPowerAmountReceived(modifiedPower);
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterModifyingRewards" />.
		/// </summary>
		// Token: 0x06007F10 RID: 32528 RVA: 0x002B02C8 File Offset: 0x002AE4C8
		public static async Task AfterModifyingRewards(IRunState runState, IEnumerable<AbstractModel> modifiers)
		{
			foreach (AbstractModel modifier in runState.IterateHookListeners(null))
			{
				if (modifiers.Contains(modifier))
				{
					await modifier.AfterModifyingRewards();
					modifier.InvokeExecutionFinished();
					modifier = null;
				}
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterOrbChanneled(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Models.OrbModel)" />.
		/// </summary>
		// Token: 0x06007F11 RID: 32529 RVA: 0x002B0314 File Offset: 0x002AE514
		public static async Task AfterOrbChanneled(ICombatState combatState, PlayerChoiceContext choiceContext, Player player, OrbModel orb)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterOrbChanneled(choiceContext, player, orb);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterOrbEvoked(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.OrbModel,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />.
		/// </summary>
		// Token: 0x06007F12 RID: 32530 RVA: 0x002B0370 File Offset: 0x002AE570
		public static async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, ICombatState combatState, OrbModel orb, IEnumerable<Creature> targets)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterOrbEvoked(choiceContext, orb, targets);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterOstyRevived(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F13 RID: 32531 RVA: 0x002B03CC File Offset: 0x002AE5CC
		public static async Task AfterOstyRevived(ICombatState combatState, Creature osty)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterOstyRevived(osty);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPlayerTurnStart(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F14 RID: 32532 RVA: 0x002B0418 File Offset: 0x002AE618
		public static async Task AfterPlayerTurnStart(ICombatState combatState, PlayerChoiceContext choiceContext, Player player)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterPlayerTurnStartEarly(choiceContext, player);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterPlayerTurnStart(choiceContext, player);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterPlayerTurnStartLate(choiceContext, player);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterAutoPostPlayPhaseEntered(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F15 RID: 32533 RVA: 0x002B046C File Offset: 0x002AE66C
		public static async Task AfterAutoPostPlayPhaseEntered(HookPlayerChoiceContext playerChoiceContext, ICombatState combatState, Player player)
		{
			if (LocalContext.NetId != null)
			{
				foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
				{
					playerChoiceContext.PushModel(model);
					await model.AfterAutoPostPlayPhaseEntered(playerChoiceContext, player);
					model.InvokeExecutionFinished();
					playerChoiceContext.PopModel(model);
					model = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterAutoPrePlayPhaseEntered(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F16 RID: 32534 RVA: 0x002B04C0 File Offset: 0x002AE6C0
		public static async Task AfterAutoPrePlayPhaseEntered(HookPlayerChoiceContext playerChoiceContext, ICombatState combatState, Player player)
		{
			if (LocalContext.NetId != null)
			{
				foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
				{
					playerChoiceContext.PushModel(model);
					await model.AfterAutoPrePlayPhaseEnteredEarly(playerChoiceContext, player);
					model.InvokeExecutionFinished();
					playerChoiceContext.PopModel(model);
					model = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
				foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
				{
					playerChoiceContext.PushModel(model);
					await model.AfterAutoPrePlayPhaseEntered(playerChoiceContext, player);
					model.InvokeExecutionFinished();
					playerChoiceContext.PopModel(model);
					model = null;
				}
				enumerator = null;
				foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
				{
					playerChoiceContext.PushModel(model);
					await model.AfterAutoPrePlayPhaseEnteredLate(playerChoiceContext, player);
					model.InvokeExecutionFinished();
					playerChoiceContext.PopModel(model);
					model = null;
				}
				enumerator = null;
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPotionDiscarded(MegaCrit.Sts2.Core.Models.PotionModel)" />.
		/// </summary>
		// Token: 0x06007F17 RID: 32535 RVA: 0x002B0514 File Offset: 0x002AE714
		public static async Task AfterPotionDiscarded(IRunState runState, [Nullable(2)] ICombatState combatState, PotionModel potion)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterPotionDiscarded(potion);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPotionProcured(MegaCrit.Sts2.Core.Models.PotionModel)" />.
		/// </summary>
		// Token: 0x06007F18 RID: 32536 RVA: 0x002B0568 File Offset: 0x002AE768
		public static async Task AfterPotionProcured(IRunState runState, [Nullable(2)] ICombatState combatState, PotionModel potion)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterPotionProcured(potion);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforePotionUsed(MegaCrit.Sts2.Core.Models.PotionModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F19 RID: 32537 RVA: 0x002B05BC File Offset: 0x002AE7BC
		public static async Task BeforePotionUsed(IRunState runState, [Nullable(2)] ICombatState combatState, PotionModel potion, [Nullable(2)] Creature target)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.BeforePotionUsed(potion, target);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPotionUsed(MegaCrit.Sts2.Core.Models.PotionModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F1A RID: 32538 RVA: 0x002B0618 File Offset: 0x002AE818
		public static async Task AfterPotionUsed(IRunState runState, [Nullable(2)] ICombatState combatState, PotionModel potion, [Nullable(2)] Creature target)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(combatState))
			{
				await model.AfterPotionUsed(potion, target);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforePowerAmountChanged(MegaCrit.Sts2.Core.Models.PowerModel,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F1B RID: 32539 RVA: 0x002B0674 File Offset: 0x002AE874
		public static async Task BeforePowerAmountChanged(ICombatState combatState, PowerModel power, decimal amount, Creature target, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
			{
				await modifier.BeforePowerAmountChanged(power, amount, target, applier, cardSource);
				modifier.InvokeExecutionFinished();
				modifier = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPowerAmountChanged(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.PowerModel,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F1C RID: 32540 RVA: 0x002B06E4 File Offset: 0x002AE8E4
		public static async Task AfterPowerAmountChanged(ICombatState combatState, PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPreventingBlockClear(MegaCrit.Sts2.Core.Models.AbstractModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F1D RID: 32541 RVA: 0x002B0754 File Offset: 0x002AE954
		public static async Task AfterPreventingBlockClear(ICombatState combatState, AbstractModel preventer, Creature creature)
		{
			if (Hook.IterateCombatHookListeners(combatState).Contains(preventer))
			{
				await preventer.AfterPreventingBlockClear(preventer, creature);
				preventer.InvokeExecutionFinished();
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPreventingDeath(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F1E RID: 32542 RVA: 0x002B07A8 File Offset: 0x002AE9A8
		public static async Task AfterPreventingDeath(IRunState runState, [Nullable(2)] ICombatState combatState, AbstractModel preventer, Creature creature)
		{
			if (runState.IterateHookListeners(combatState).Contains(preventer))
			{
				await preventer.AfterPreventingDeath(creature);
				preventer.InvokeExecutionFinished();
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPreventingDraw" />.
		/// </summary>
		// Token: 0x06007F1F RID: 32543 RVA: 0x002B0804 File Offset: 0x002AEA04
		public static async Task AfterPreventingDraw(ICombatState combatState, AbstractModel modifier)
		{
			if (Hook.IterateCombatHookListeners(combatState).Contains(modifier))
			{
				await modifier.AfterPreventingDraw();
				modifier.InvokeExecutionFinished();
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRestSiteHeal(MegaCrit.Sts2.Core.Entities.Players.Player,System.Boolean)" />.
		/// </summary>
		// Token: 0x06007F20 RID: 32544 RVA: 0x002B0850 File Offset: 0x002AEA50
		public static async Task AfterRestSiteHeal(IRunState runState, Player player, bool isMimicked)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterRestSiteHeal(player, isMimicked);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRestSiteSmith(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F21 RID: 32545 RVA: 0x002B08A4 File Offset: 0x002AEAA4
		public static async Task AfterRestSiteSmith(IRunState runState, Player player)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterRestSiteSmith(player);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRewardTaken(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Rewards.Reward)" />.
		/// </summary>
		// Token: 0x06007F22 RID: 32546 RVA: 0x002B08F0 File Offset: 0x002AEAF0
		public static async Task AfterRewardTaken(IRunState runState, Player player, Reward reward)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterRewardTaken(player, reward);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeRoomEntered(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" />.
		/// </summary>
		// Token: 0x06007F23 RID: 32547 RVA: 0x002B0944 File Offset: 0x002AEB44
		public static async Task BeforeRoomEntered(IRunState runState, AbstractRoom room)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.BeforeRoomEntered(room);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRoomEntered(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" />.
		/// </summary>
		// Token: 0x06007F24 RID: 32548 RVA: 0x002B0990 File Offset: 0x002AEB90
		public static async Task AfterRoomEntered(IRunState runState, AbstractRoom room)
		{
			foreach (AbstractModel model in runState.IterateHookListeners(null))
			{
				await model.AfterRoomEntered(room);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterShuffle(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007F25 RID: 32549 RVA: 0x002B09DC File Offset: 0x002AEBDC
		public static async Task AfterShuffle(ICombatState combatState, PlayerChoiceContext choiceContext, Player shuffler)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterShuffle(choiceContext, shuffler);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeSideTurnStart(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" />.
		/// </summary>
		// Token: 0x06007F26 RID: 32550 RVA: 0x002B0A30 File Offset: 0x002AEC30
		public static async Task BeforeSideTurnStart(ICombatState combatState, CombatSide side, IReadOnlyList<Creature> participants)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext hookPlayerChoiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, GameActionType.Combat);
					Task task = model.BeforeSideTurnStart(hookPlayerChoiceContext, side, participants, combatState);
					await hookPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					model.InvokeExecutionFinished();
					model = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" />.
		/// </summary>
		// Token: 0x06007F27 RID: 32551 RVA: 0x002B0A84 File Offset: 0x002AEC84
		public static async Task AfterSideTurnStart(ICombatState combatState, CombatSide side, IReadOnlyList<Creature> participants)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterSideTurnStart(side, participants, combatState);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterSideTurnStartLate(side, participants, combatState);
				model.InvokeExecutionFinished();
				model = null;
			}
			enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterStarsGained(System.Int32,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F28 RID: 32552 RVA: 0x002B0AD8 File Offset: 0x002AECD8
		public static async Task AfterStarsGained(ICombatState combatState, int amount, Player gainer)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterStarsGained(amount, gainer);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterStarsSpent(System.Int32,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F29 RID: 32553 RVA: 0x002B0B2C File Offset: 0x002AED2C
		public static async Task AfterStarsSpent(ICombatState combatState, int amount, Player spender)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterStarsSpent(amount, spender);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSummon(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// This takes a player choice context as an argument because it needs to block combat flow if a player choice is
		/// encountered.
		/// </summary>
		// Token: 0x06007F2A RID: 32554 RVA: 0x002B0B80 File Offset: 0x002AED80
		public static async Task AfterSummon(ICombatState combatState, PlayerChoiceContext choiceContext, Player summoner, decimal amount)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				choiceContext.PushModel(model);
				await model.AfterSummon(choiceContext, summoner, amount);
				model.InvokeExecutionFinished();
				choiceContext.PopModel(model);
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterTakingExtraTurn(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F2B RID: 32555 RVA: 0x002B0BDC File Offset: 0x002AEDDC
		public static async Task AfterTakingExtraTurn(ICombatState combatState, Player player)
		{
			foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
			{
				await model.AfterTakingExtraTurn(player);
				model.InvokeExecutionFinished();
				model = null;
			}
			IEnumerator<AbstractModel> enumerator = null;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.BeforeSideTurnEnd(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />.
		/// </summary>
		// Token: 0x06007F2C RID: 32556 RVA: 0x002B0C28 File Offset: 0x002AEE28
		public static async Task BeforeSideTurnEnd(ICombatState combatState, CombatSide side, IEnumerable<Creature> participants)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				List<Task> tasksToAwait = new List<Task>();
				foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel, netId.Value, combatState, GameActionType.Combat);
					Task task = abstractModel.BeforeSideTurnEndVeryEarly(playerChoiceContext, side, participants);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
				foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel2, netId.Value, combatState, GameActionType.Combat);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(abstractModel2.BeforeSideTurnEndEarly(playerChoiceContext, side, participants));
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				enumerator = null;
				foreach (AbstractModel abstractModel3 in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel3, netId.Value, combatState, GameActionType.Combat);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(abstractModel3.BeforeSideTurnEnd(playerChoiceContext, side, participants));
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				enumerator = null;
				await Task.WhenAll(tasksToAwait);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnEnd(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />.
		/// </summary>
		// Token: 0x06007F2D RID: 32557 RVA: 0x002B0C7C File Offset: 0x002AEE7C
		public static async Task AfterSideTurnEnd(ICombatState combatState, CombatSide side, IEnumerable<Creature> participants)
		{
			ulong? netId = LocalContext.NetId;
			if (netId != null)
			{
				List<Task> tasksToAwait = new List<Task>();
				foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel, netId.Value, combatState, GameActionType.Combat);
					Task task = abstractModel.AfterSideTurnEnd(playerChoiceContext, side, participants);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				IEnumerator<AbstractModel> enumerator = null;
				await Task.WhenAll(tasksToAwait);
				tasksToAwait.Clear();
				foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
				{
					HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(abstractModel2, netId.Value, combatState, GameActionType.Combat);
					await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(abstractModel2.AfterSideTurnEndLate(playerChoiceContext, side, participants));
					tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
					playerChoiceContext = null;
				}
				enumerator = null;
				await Task.WhenAll(tasksToAwait);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyAttackHitCount(MegaCrit.Sts2.Core.Commands.Builders.AttackCommand,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F2E RID: 32558 RVA: 0x002B0CD0 File Offset: 0x002AEED0
		public static decimal ModifyAttackHitCount(ICombatState combatState, AttackCommand attackCommand, int originalHitCount)
		{
			int num = originalHitCount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				num = abstractModel.ModifyAttackHitCount(attackCommand, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyBlockAdditive(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> and <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyBlockMultiplicative(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		// Token: 0x06007F2F RID: 32559 RVA: 0x002B0D28 File Offset: 0x002AEF28
		public static decimal ModifyBlock(ICombatState combatState, Creature target, decimal block, ValueProp props, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay, out IEnumerable<AbstractModel> modifiers)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			decimal num = block;
			if (cardSource != null && cardSource.Enchantment != null)
			{
				EnchantmentModel enchantment = cardSource.Enchantment;
				num += enchantment.EnchantBlockAdditive(num);
				num *= enchantment.EnchantBlockMultiplicative(num);
			}
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num2 = abstractModel.ModifyBlockAdditive(target, num, props, cardSource, cardPlay);
				num += num2;
				if (num2 != 0m)
				{
					list.Add(abstractModel);
				}
			}
			foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num3 = abstractModel2.ModifyBlockMultiplicative(target, num, props, cardSource, cardPlay);
				num *= num3;
				if (num3 != 1m)
				{
					list.Add(abstractModel2);
				}
			}
			modifiers = list;
			return Math.Max(0m, num);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyCardBeingAddedToDeck(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Models.CardModel@)" />.
		/// </summary>
		// Token: 0x06007F30 RID: 32560 RVA: 0x002B0E50 File Offset: 0x002AF050
		public static CardModel ModifyCardBeingAddedToDeck(IRunState runState, CardModel card, out List<AbstractModel> modifyingModels)
		{
			modifyingModels = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				CardModel cardModel;
				bool flag = abstractModel.TryModifyCardBeingAddedToDeck(card, out cardModel);
				if (flag && cardModel != null)
				{
					modifyingModels.Add(abstractModel);
					card = cardModel;
				}
				abstractModel.InvokeExecutionFinished();
			}
			foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(null))
			{
				CardModel cardModel2;
				bool flag2 = abstractModel2.TryModifyCardBeingAddedToDeckLate(card, out cardModel2);
				if (flag2 && cardModel2 != null)
				{
					modifyingModels.Add(abstractModel2);
					card = cardModel2;
				}
				abstractModel2.InvokeExecutionFinished();
			}
			return card;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardPlayCount(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F31 RID: 32561 RVA: 0x002B0F24 File Offset: 0x002AF124
		public static int ModifyCardPlayCount(ICombatState combatState, CardModel card, int playCount, [Nullable(2)] Creature target, out List<AbstractModel> modifyingModels)
		{
			modifyingModels = new List<AbstractModel>();
			int num = playCount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				int num2 = num;
				num = abstractModel.ModifyCardPlayCount(card, target, num);
				if (num != num2)
				{
					modifyingModels.Add(abstractModel);
				}
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardPlayResultLocation(MegaCrit.Sts2.Core.Models.CardModel,System.Boolean,MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo,MegaCrit.Sts2.Core.Entities.Cards.CardLocation)" />.
		/// </summary>
		// Token: 0x06007F32 RID: 32562 RVA: 0x002B0F90 File Offset: 0x002AF190
		public static CardLocation ModifyCardPlayResultLocation(ICombatState combatState, CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation cardLocation, out IEnumerable<AbstractModel> modifiers)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				CardLocation cardLocation2 = cardLocation;
				cardLocation = abstractModel.ModifyCardPlayResultLocation(card, isAutoPlay, resources, cardLocation);
				if (cardLocation2 != cardLocation)
				{
					list.Add(abstractModel);
				}
			}
			modifiers = list;
			return cardLocation;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyCardRewardAlternatives(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Rewards.CardReward,System.Collections.Generic.List{MegaCrit.Sts2.Core.Entities.CardRewardAlternatives.CardRewardAlternative})" />.
		/// </summary>
		// Token: 0x06007F33 RID: 32563 RVA: 0x002B1004 File Offset: 0x002AF204
		public static IEnumerable<AbstractModel> ModifyCardRewardAlternatives(IRunState runState, Player player, CardReward cardReward, List<CardRewardAlternative> alternatives)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.TryModifyCardRewardAlternatives(player, cardReward, alternatives))
				{
					list.Add(abstractModel);
				}
			}
			return list;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardRewardCreationOptions(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Runs.CardCreationOptions)" />.
		/// </summary>
		// Token: 0x06007F34 RID: 32564 RVA: 0x002B1064 File Offset: 0x002AF264
		public static CardCreationOptions ModifyCardRewardCreationOptions(IRunState runState, Player player, CardCreationOptions options)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				options = abstractModel.ModifyCardRewardCreationOptions(player, options);
			}
			foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(null))
			{
				options = abstractModel2.ModifyCardRewardCreationOptionsLate(player, options);
			}
			return options;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyCardRewardOptions(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.List{MegaCrit.Sts2.Core.Entities.Cards.CardCreationResult},MegaCrit.Sts2.Core.Runs.CardCreationOptions)" />.
		/// </summary>
		// Token: 0x06007F35 RID: 32565 RVA: 0x002B10F8 File Offset: 0x002AF2F8
		public static bool TryModifyCardRewardOptions(IRunState runState, Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions, out List<AbstractModel> modifiers)
		{
			bool flag = false;
			modifiers = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				bool flag2 = abstractModel.TryModifyCardRewardOptions(player, cardRewardOptions, creationOptions);
				flag = flag || flag2;
				if (flag2)
				{
					modifiers.Add(abstractModel);
				}
			}
			foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(null))
			{
				bool flag3 = abstractModel2.TryModifyCardRewardOptionsLate(player, cardRewardOptions, creationOptions);
				flag = flag || flag3;
				if (flag3)
				{
					modifiers.Add(abstractModel2);
				}
			}
			return flag;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardRewardUpgradeOdds(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Models.CardModel,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F36 RID: 32566 RVA: 0x002B11C0 File Offset: 0x002AF3C0
		public static decimal ModifyCardRewardUpgradeOdds(IRunState runState, Player player, CardModel card, decimal originalOdds)
		{
			decimal num = originalOdds;
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				num = abstractModel.ModifyCardRewardUpgradeOdds(player, card, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyDamageAdditive(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		// Token: 0x06007F37 RID: 32567 RVA: 0x002B1214 File Offset: 0x002AF414
		[NullableContext(2)]
		public static decimal ModifyDamage([Nullable(1)] IRunState runState, ICombatState combatState, Creature target, Creature dealer, decimal damage, ValueProp props, CardModel cardSource, CardPlay cardPlay, ModifyDamageHookType modifyDamageHookType, CardPreviewMode previewMode, [Nullable(1)] out IEnumerable<AbstractModel> modifiers)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			decimal num = damage;
			if (cardSource != null && cardSource.Enchantment != null)
			{
				if (modifyDamageHookType.HasFlag(ModifyDamageHookType.Additive))
				{
					num += cardSource.Enchantment.EnchantDamageAdditive(num, props);
				}
				if (modifyDamageHookType.HasFlag(ModifyDamageHookType.Multiplicative))
				{
					num *= cardSource.Enchantment.EnchantDamageMultiplicative(num, props);
				}
			}
			bool flag = target == null && previewMode == CardPreviewMode.MultiCreatureTargeting;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3;
				if (cardSource != null)
				{
					TargetType targetType = cardSource.TargetType;
					if (targetType - TargetType.AllEnemies <= 1)
					{
						CardPile pile = cardSource.Pile;
						if (pile != null)
						{
							PileType type = pile.Type;
							if (type == PileType.Hand || type == PileType.Play)
							{
								flag3 = true;
								goto IL_00BB;
							}
						}
					}
				}
				flag3 = false;
				IL_00BB:
				flag2 = flag3;
			}
			bool flag4 = flag2;
			bool flag5 = false;
			if (flag4)
			{
				bool flag6 = true;
				decimal? num2 = null;
				foreach (Creature creature in (((combatState != null) ? combatState.HittableEnemies : null) ?? Array.Empty<Creature>()))
				{
					List<AbstractModel> list2;
					decimal num3 = Hook.ModifyDamageInternal(runState, combatState, creature, dealer, num, props, cardSource, cardPlay, modifyDamageHookType, out list2);
					if (num2 == null)
					{
						num2 = new decimal?(num3);
					}
					else if ((int)num3 != (int)num2.Value)
					{
						flag6 = false;
						break;
					}
					list.AddRange(list2);
				}
				if (num2 != null && flag6)
				{
					flag5 = true;
					num = num2.Value;
					list = list.Distinct<AbstractModel>().ToList<AbstractModel>();
				}
				else
				{
					list.Clear();
				}
			}
			if (!flag4 || !flag5)
			{
				num = Hook.ModifyDamageInternal(runState, combatState, target, dealer, num, props, cardSource, cardPlay, modifyDamageHookType, out list);
			}
			modifiers = list;
			return Math.Max(0m, num);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyEnergyCostInCombat(MegaCrit.Sts2.Core.Models.CardModel,System.Decimal,System.Decimal@)" />.
		/// </summary>
		// Token: 0x06007F38 RID: 32568 RVA: 0x002B13E8 File Offset: 0x002AF5E8
		public static decimal ModifyEnergyCostInCombat(ICombatState combatState, CardModel card, decimal originalCost)
		{
			if (originalCost < 0m)
			{
				return originalCost;
			}
			decimal num = originalCost;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				abstractModel.TryModifyEnergyCostInCombat(card, num, out num);
			}
			foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
			{
				abstractModel2.TryModifyEnergyCostInCombatLate(card, num, out num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyKeywordsInCombat(MegaCrit.Sts2.Core.Models.CardModel,System.Collections.Generic.ISet{MegaCrit.Sts2.Core.Entities.Cards.CardKeyword})" />.
		/// </summary>
		// Token: 0x06007F39 RID: 32569 RVA: 0x002B1490 File Offset: 0x002AF690
		public static void ModifyKeywordsInCombat(ICombatState combatState, CardModel card, ISet<CardKeyword> keywords)
		{
			foreach (AbstractModel abstractModel in combatState.IterateHookListeners())
			{
				abstractModel.TryModifyKeywordsInCombat(card, keywords);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyEnergyGain(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F3A RID: 32570 RVA: 0x002B14E0 File Offset: 0x002AF6E0
		public static decimal ModifyEnergyGain(ICombatState combatState, Player player, decimal originalAmount, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = originalAmount;
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num2 = num;
				num = abstractModel.ModifyEnergyGain(player, num);
				if ((int)num2 != (int)num)
				{
					list.Add(abstractModel);
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyGoldGained(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F3B RID: 32571 RVA: 0x002B1554 File Offset: 0x002AF754
		public static decimal ModifyGoldGained(IRunState runState, [Nullable(2)] ICombatState combatState, decimal amount, Player player, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = amount;
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(combatState))
			{
				decimal num2 = num;
				num = abstractModel.ModifyGoldGained(player, num);
				if ((int)num2 != (int)num)
				{
					list.Add(abstractModel);
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyExtraRestSiteHealText(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Localization.LocString})" />.
		/// </summary>
		// Token: 0x06007F3C RID: 32572 RVA: 0x002B15CC File Offset: 0x002AF7CC
		public static IReadOnlyList<LocString> ModifyExtraRestSiteHealText(IRunState runState, Player player, IReadOnlyList<LocString> extraText)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				extraText = abstractModel.ModifyExtraRestSiteHealText(player, extraText);
			}
			return extraText;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyGeneratedMap(MegaCrit.Sts2.Core.Runs.IRunState,MegaCrit.Sts2.Core.Map.ActMap,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F3D RID: 32573 RVA: 0x002B1620 File Offset: 0x002AF820
		public static ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				map = abstractModel.ModifyGeneratedMap(runState, map, actIndex);
				abstractModel.InvokeExecutionFinished();
			}
			return Hook.ModifyGeneratedMapLate(runState, map, actIndex);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyGeneratedMapLate(MegaCrit.Sts2.Core.Runs.IRunState,MegaCrit.Sts2.Core.Map.ActMap,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F3E RID: 32574 RVA: 0x002B1680 File Offset: 0x002AF880
		public static ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				map = abstractModel.ModifyGeneratedMapLate(runState, map, actIndex);
				abstractModel.InvokeExecutionFinished();
			}
			return map;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHandDraw(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F3F RID: 32575 RVA: 0x002B16DC File Offset: 0x002AF8DC
		public static decimal ModifyHandDraw(ICombatState combatState, Player player, decimal originalCardCount, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = originalCardCount;
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num2 = num;
				num = abstractModel.ModifyHandDraw(player, num);
				if ((int)num2 != (int)num)
				{
					list.Add(abstractModel);
				}
			}
			foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num3 = num;
				num = abstractModel2.ModifyHandDrawLate(player, num);
				if ((int)num3 != (int)num)
				{
					list.Add(abstractModel2);
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// Run the requested HP-loss-modification hook phases on <paramref name="target" />.
		/// In CreatureCmd.Damage the two phases are invoked separately because damage redirection (Osty) sits between them
		/// and may change the target. Callers that have no redirection step (e.g. damage previews) should pass
		/// <see cref="F:MegaCrit.Sts2.Core.Hooks.HpLossHookPhase.All" />.
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostBeforeOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" /> and <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostAfterOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F40 RID: 32576 RVA: 0x002B17B0 File Offset: 0x002AF9B0
		public static decimal ModifyHpLost(IRunState runState, [Nullable(2)] ICombatState combatState, Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource, HpLossHookPhase phases, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = amount;
			List<AbstractModel> list = new List<AbstractModel>();
			if (phases.HasFlag(HpLossHookPhase.BeforeOsty))
			{
				foreach (AbstractModel abstractModel in runState.IterateHookListeners(combatState))
				{
					decimal num2 = num;
					num = abstractModel.ModifyHpLostBeforeOsty(target, num, props, dealer, cardSource);
					if (decimal.Truncate(num2) != decimal.Truncate(num))
					{
						list.Add(abstractModel);
					}
				}
				foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(combatState))
				{
					decimal num3 = num;
					num = abstractModel2.ModifyHpLostBeforeOstyLate(target, num, props, dealer, cardSource);
					if (decimal.Truncate(num3) != decimal.Truncate(num))
					{
						list.Add(abstractModel2);
					}
				}
			}
			if (phases.HasFlag(HpLossHookPhase.AfterOsty))
			{
				foreach (AbstractModel abstractModel3 in runState.IterateHookListeners(combatState))
				{
					decimal num4 = num;
					num = abstractModel3.ModifyHpLostAfterOsty(target, num, props, dealer, cardSource);
					if (decimal.Truncate(num4) != decimal.Truncate(num))
					{
						list.Add(abstractModel3);
					}
				}
				foreach (AbstractModel abstractModel4 in runState.IterateHookListeners(combatState))
				{
					decimal num5 = num;
					num = abstractModel4.ModifyHpLostAfterOstyLate(target, num, props, dealer, cardSource);
					if (decimal.Truncate(num5) != decimal.Truncate(num))
					{
						list.Add(abstractModel4);
					}
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyMaxEnergy(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F41 RID: 32577 RVA: 0x002B19A0 File Offset: 0x002AFBA0
		public static decimal ModifyMaxEnergy(ICombatState combatState, Player player, decimal amount)
		{
			decimal num = amount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				num = abstractModel.ModifyMaxEnergy(player, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyMerchantCardCreationResults(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.List{MegaCrit.Sts2.Core.Entities.Cards.CardCreationResult})" />.
		/// </summary>
		// Token: 0x06007F42 RID: 32578 RVA: 0x002B19F4 File Offset: 0x002AFBF4
		public static void ModifyMerchantCardCreationResults(IRunState runState, Player player, List<CardCreationResult> cards)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				abstractModel.ModifyMerchantCardCreationResults(player, cards);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyMerchantCardPool(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Models.CardModel})" />.
		/// </summary>
		// Token: 0x06007F43 RID: 32579 RVA: 0x002B1A44 File Offset: 0x002AFC44
		public static IEnumerable<CardModel> ModifyMerchantCardPool(IRunState runState, Player player, IEnumerable<CardModel> options)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				options = abstractModel.ModifyMerchantCardPool(player, options);
			}
			return options;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyMerchantCardPool(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Models.CardModel})" />.
		/// </summary>
		// Token: 0x06007F44 RID: 32580 RVA: 0x002B1A98 File Offset: 0x002AFC98
		public static CardRarity ModifyMerchantCardRarity(IRunState runState, Player player, CardRarity rarity)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				rarity = abstractModel.ModifyMerchantCardRarity(player, rarity);
			}
			return rarity;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyMerchantPrice(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Entities.Merchant.MerchantEntry,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F45 RID: 32581 RVA: 0x002B1AEC File Offset: 0x002AFCEC
		public static decimal ModifyMerchantPrice(IRunState runState, Player player, MerchantEntry entry, decimal result)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				result = abstractModel.ModifyMerchantPrice(player, entry, result);
			}
			return result;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyNextEvent(MegaCrit.Sts2.Core.Models.EventModel)" />.
		/// </summary>
		// Token: 0x06007F46 RID: 32582 RVA: 0x002B1B40 File Offset: 0x002AFD40
		public static EventModel ModifyNextEvent(IRunState runState, EventModel currentEvent)
		{
			EventModel eventModel = currentEvent;
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				eventModel = abstractModel.ModifyNextEvent(eventModel);
			}
			return eventModel;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyOddsIncreaseForUnrolledRoomType(MegaCrit.Sts2.Core.Rooms.RoomType,System.Single)" />.
		/// </summary>
		// Token: 0x06007F47 RID: 32583 RVA: 0x002B1B94 File Offset: 0x002AFD94
		public static float ModifyOddsIncreaseForUnrolledRoomType(IRunState runState, RoomType roomType, float oddsIncrease)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				oddsIncrease = abstractModel.ModifyOddsIncreaseForUnrolledRoomType(roomType, oddsIncrease);
			}
			return oddsIncrease;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyOrbPassiveTriggerCounts(MegaCrit.Sts2.Core.Models.OrbModel,System.Int32)" />.
		/// </summary>
		// Token: 0x06007F48 RID: 32584 RVA: 0x002B1BE8 File Offset: 0x002AFDE8
		public static int ModifyOrbPassiveTriggerCount(ICombatState combatState, OrbModel orb, int triggerCount, out List<AbstractModel> modifyingModels)
		{
			modifyingModels = new List<AbstractModel>();
			int num = triggerCount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				int num2 = num;
				num = abstractModel.ModifyOrbPassiveTriggerCounts(orb, num);
				if (num != num2)
				{
					modifyingModels.Add(abstractModel);
				}
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyOrbValue(MegaCrit.Sts2.Core.Models.OrbModel,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F49 RID: 32585 RVA: 0x002B1C50 File Offset: 0x002AFE50
		public static decimal ModifyOrbValue(ICombatState combatState, OrbModel orb, decimal amount)
		{
			decimal num = amount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				num = abstractModel.ModifyOrbValue(orb, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyPowerAmountGivenAdditive(MegaCrit.Sts2.Core.Models.PowerModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" /> and
		/// <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyPowerAmountGivenMultiplicative(MegaCrit.Sts2.Core.Models.PowerModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F4A RID: 32586 RVA: 0x002B1CA4 File Offset: 0x002AFEA4
		public static decimal ModifyPowerAmountGiven(ICombatState combatState, PowerModel power, Creature giver, decimal amount, [Nullable(2)] Creature target, [Nullable(2)] CardModel cardSource, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = amount;
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num2 = abstractModel.ModifyPowerAmountGivenAdditive(power, giver, num, target, cardSource);
				num += num2;
				if (num2 != 0m)
				{
					list.Add(abstractModel);
				}
			}
			foreach (AbstractModel abstractModel2 in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num3 = abstractModel2.ModifyPowerAmountGivenMultiplicative(power, giver, num, target, cardSource);
				num *= num3;
				if (num3 != 1m)
				{
					list.Add(abstractModel2);
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyPowerAmountReceived(MegaCrit.Sts2.Core.Models.PowerModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal@)" />.
		/// </summary>
		// Token: 0x06007F4B RID: 32587 RVA: 0x002B1D90 File Offset: 0x002AFF90
		public static decimal ModifyPowerAmountReceived(ICombatState combatState, PowerModel canonicalPower, Creature target, decimal amount, [Nullable(2)] Creature giver, out IEnumerable<AbstractModel> modifiers)
		{
			decimal num = amount;
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				decimal num2;
				if (abstractModel.TryModifyPowerAmountReceived(canonicalPower, target, num, giver, out num2))
				{
					num = num2;
					list.Add(abstractModel);
				}
			}
			modifiers = list;
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyRestSiteHealAmount(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal)" />.
		/// </summary>
		// Token: 0x06007F4C RID: 32588 RVA: 0x002B1DFC File Offset: 0x002AFFFC
		public static decimal ModifyRestSiteHealAmount(IRunState runState, Creature creature, decimal amount)
		{
			decimal num = amount;
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				num = abstractModel.ModifyRestSiteHealAmount(creature, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyRestSiteOptions(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.ICollection{MegaCrit.Sts2.Core.Entities.RestSite.RestSiteOption})" />.
		/// </summary>
		// Token: 0x06007F4D RID: 32589 RVA: 0x002B1E50 File Offset: 0x002B0050
		public static IEnumerable<AbstractModel> ModifyRestSiteOptions(IRunState runState, Player player, ICollection<RestSiteOption> options)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.TryModifyRestSiteOptions(player, options))
				{
					list.Add(abstractModel);
				}
			}
			return list;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyRestSiteHealRewards(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.List{MegaCrit.Sts2.Core.Rewards.Reward},System.Boolean)" />.
		/// </summary>
		// Token: 0x06007F4E RID: 32590 RVA: 0x002B1EB0 File Offset: 0x002B00B0
		public static IEnumerable<AbstractModel> ModifyRestSiteHealRewards(IRunState runState, Player player, List<Reward> rewards, bool isMimicked)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.TryModifyRestSiteHealRewards(player, rewards, isMimicked))
				{
					list.Add(abstractModel);
				}
			}
			return list;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyRewards(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.List{MegaCrit.Sts2.Core.Rewards.Reward},MegaCrit.Sts2.Core.Rooms.AbstractRoom)" />.
		/// </summary>
		// Token: 0x06007F4F RID: 32591 RVA: 0x002B1F10 File Offset: 0x002B0110
		public static IEnumerable<AbstractModel> ModifyRewards(IRunState runState, Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			List<AbstractModel> list = new List<AbstractModel>();
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.TryModifyRewards(player, rewards, room))
				{
					list.Add(abstractModel);
				}
			}
			foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(null))
			{
				if (abstractModel2.TryModifyRewardsLate(player, rewards, room))
				{
					list.Add(abstractModel2);
				}
			}
			return list;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyShuffleOrder(MegaCrit.Sts2.Core.Entities.Players.Player,System.Collections.Generic.List{MegaCrit.Sts2.Core.Models.CardModel},System.Boolean)" />.
		/// </summary>
		// Token: 0x06007F50 RID: 32592 RVA: 0x002B1FBC File Offset: 0x002B01BC
		public static void ModifyShuffleOrder(ICombatState combatState, Player player, List<CardModel> cards, bool isInitialShuffle)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				abstractModel.ModifyShuffleOrder(player, cards, isInitialShuffle);
			}
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyStarCost(MegaCrit.Sts2.Core.Models.CardModel,System.Decimal,System.Decimal@)" />.
		/// </summary>
		// Token: 0x06007F51 RID: 32593 RVA: 0x002B200C File Offset: 0x002B020C
		public static decimal ModifyStarCost(ICombatState combatState, CardModel card, decimal originalCost)
		{
			if (originalCost < 0m)
			{
				return originalCost;
			}
			decimal num = originalCost;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				abstractModel.TryModifyStarCost(card, num, out num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifySummonAmount(MegaCrit.Sts2.Core.Entities.Players.Player,System.Decimal,MegaCrit.Sts2.Core.Models.AbstractModel)" />.
		/// </summary>
		// Token: 0x06007F52 RID: 32594 RVA: 0x002B2070 File Offset: 0x002B0270
		public static decimal ModifySummonAmount(ICombatState combatState, Player summoner, decimal amount, [Nullable(2)] AbstractModel source)
		{
			decimal num = amount;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				num = abstractModel.ModifySummonAmount(summoner, num, source);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyUnblockedDamageTarget(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it runs during damage
		/// resolution, which proceeds while combat is ending.
		/// </summary>
		// Token: 0x06007F53 RID: 32595 RVA: 0x002B20C4 File Offset: 0x002B02C4
		public static Creature ModifyUnblockedDamageTarget(ICombatState combatState, Creature originalTarget, decimal amount, ValueProp props, [Nullable(2)] Creature dealer)
		{
			Creature creature = originalTarget;
			foreach (AbstractModel abstractModel in combatState.IterateHookListeners())
			{
				creature = abstractModel.ModifyUnblockedDamageTarget(creature, amount, props, dealer);
			}
			return creature;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyUnknownMapPointRoomTypes(System.Collections.Generic.IReadOnlySet{MegaCrit.Sts2.Core.Rooms.RoomType})" />.
		/// </summary>
		// Token: 0x06007F54 RID: 32596 RVA: 0x002B211C File Offset: 0x002B031C
		public static IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(IRunState runState, IReadOnlySet<RoomType> roomTypes)
		{
			IReadOnlySet<RoomType> readOnlySet = new HashSet<RoomType>(roomTypes);
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				readOnlySet = abstractModel.ModifyUnknownMapPointRoomTypes(readOnlySet);
			}
			return readOnlySet;
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x002B2174 File Offset: 0x002B0374
		public static int ModifyXValue(ICombatState combatState, CardModel card, int originalValue)
		{
			int num = originalValue;
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				num = abstractModel.ModifyXValue(card, num);
			}
			return num;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAddToDeck(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F56 RID: 32598 RVA: 0x002B21C8 File Offset: 0x002B03C8
		public static bool ShouldAddToDeck(IRunState runState, CardModel card, [Nullable(2)] out AbstractModel preventer)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldAddToDeck(card))
				{
					preventer = abstractModel;
					return false;
				}
			}
			preventer = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAfflict(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Models.AfflictionModel)" />.
		/// </summary>
		// Token: 0x06007F57 RID: 32599 RVA: 0x002B2228 File Offset: 0x002B0428
		public static bool ShouldAfflict(ICombatState combatState, CardModel card, AfflictionModel affliction)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldAfflict(card, affliction))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowAncient(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Models.AncientEventModel)" />.
		/// </summary>
		// Token: 0x06007F58 RID: 32600 RVA: 0x002B2280 File Offset: 0x002B0480
		public static bool ShouldAllowAncient(IRunState runState, Player player, AncientEventModel ancient)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldAllowAncient(player, ancient))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowHitting(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F59 RID: 32601 RVA: 0x002B22D8 File Offset: 0x002B04D8
		public static bool ShouldAllowHitting(ICombatState combatState, Creature creature)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldAllowHitting(creature))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowMerchantCardRemoval(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F5A RID: 32602 RVA: 0x002B2330 File Offset: 0x002B0530
		public static bool ShouldAllowMerchantCardRemoval(IRunState runState, Player player)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldAllowMerchantCardRemoval(player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowSelectingMoreCardRewards(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Rewards.CardReward)" />.
		/// </summary>
		// Token: 0x06007F5B RID: 32603 RVA: 0x002B2388 File Offset: 0x002B0588
		public static bool ShouldAllowSelectingMoreCardRewards(IRunState runState, Player player, CardReward reward)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.ShouldAllowSelectingMoreCardRewards(player, reward))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowTargeting(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F5C RID: 32604 RVA: 0x002B23E0 File Offset: 0x002B05E0
		public static bool ShouldAllowTargeting(ICombatState combatState, Creature target, [Nullable(2)] out AbstractModel preventer)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldAllowTargeting(target))
				{
					preventer = abstractModel;
					return false;
				}
			}
			preventer = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldClearBlock(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F5D RID: 32605 RVA: 0x002B243C File Offset: 0x002B063C
		public static bool ShouldClearBlock(ICombatState combatState, Creature creature, [Nullable(2)] out AbstractModel preventer)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldClearBlock(creature))
				{
					preventer = abstractModel;
					return false;
				}
			}
			preventer = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldCreatureBeRemovedFromCombatAfterDeath(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it is a predicate that
		/// drives the decision of whether a creature is removed after death, so suppressing it while
		/// combat is ending would drop the votes it collects.
		/// </summary>
		// Token: 0x06007F5E RID: 32606 RVA: 0x002B2498 File Offset: 0x002B0698
		public static bool ShouldCreatureBeRemovedFromCombatAfterDeath(ICombatState combatState, Creature creature)
		{
			foreach (AbstractModel abstractModel in combatState.IterateHookListeners())
			{
				if (!abstractModel.ShouldCreatureBeRemovedFromCombatAfterDeath(creature))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldDie(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		// Token: 0x06007F5F RID: 32607 RVA: 0x002B24F0 File Offset: 0x002B06F0
		public static bool ShouldDie(IRunState runState, [Nullable(2)] ICombatState combatState, Creature creature, [Nullable(2)] out AbstractModel preventer)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(combatState))
			{
				if (!abstractModel.ShouldDie(creature))
				{
					preventer = abstractModel;
					return false;
				}
			}
			foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(combatState))
			{
				if (!abstractModel2.ShouldDieLate(creature))
				{
					preventer = abstractModel2;
					return false;
				}
			}
			preventer = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldDisableRemainingRestSiteOptions(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F60 RID: 32608 RVA: 0x002B2598 File Offset: 0x002B0798
		public static bool ShouldDisableRemainingRestSiteOptions(IRunState runState, Player player)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldDisableRemainingRestSiteOptions(player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldDraw(MegaCrit.Sts2.Core.Entities.Players.Player,System.Boolean)" />.
		/// </summary>
		// Token: 0x06007F61 RID: 32609 RVA: 0x002B25F0 File Offset: 0x002B07F0
		public static bool ShouldDraw(ICombatState combatState, Player player, bool fromHandDraw, [Nullable(2)] out AbstractModel modifier)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldDraw(player, fromHandDraw))
				{
					modifier = abstractModel;
					return false;
				}
			}
			modifier = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldEtherealTrigger(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x06007F62 RID: 32610 RVA: 0x002B2650 File Offset: 0x002B0850
		public static bool ShouldEtherealTrigger(ICombatState combatState, CardModel card)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldEtherealTrigger(card))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldFlush(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F63 RID: 32611 RVA: 0x002B26A8 File Offset: 0x002B08A8
		public static bool ShouldFlush(ICombatState combatState, Player player)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldFlush(player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldGenerateTreasure(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F64 RID: 32612 RVA: 0x002B2700 File Offset: 0x002B0900
		public static bool ShouldGenerateTreasure(IRunState runState, Player player)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldGenerateTreasure(player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldGainStars(System.Decimal,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F65 RID: 32613 RVA: 0x002B2758 File Offset: 0x002B0958
		public static bool ShouldGainStars(ICombatState combatState, decimal amount, Player player)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldGainStars(amount, player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldPayExcessEnergyCostWithStars(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F66 RID: 32614 RVA: 0x002B27B0 File Offset: 0x002B09B0
		public static bool ShouldPayExcessEnergyCostWithStars(ICombatState combatState, Player player)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (abstractModel.ShouldPayExcessEnergyCostWithStars(player))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldPlay(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.AutoPlayType)" />.
		/// </summary>
		// Token: 0x06007F67 RID: 32615 RVA: 0x002B2808 File Offset: 0x002B0A08
		public static bool ShouldPlay(ICombatState combatState, CardModel card, [Nullable(2)] out AbstractModel preventer, AutoPlayType autoPlayType)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldPlay(card, autoPlayType))
				{
					preventer = abstractModel;
					return false;
				}
			}
			preventer = null;
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldPlayerResetEnergy(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F68 RID: 32616 RVA: 0x002B2868 File Offset: 0x002B0A68
		public static bool ShouldPlayerResetEnergy(ICombatState combatState, Player player)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (!abstractModel.ShouldPlayerResetEnergy(player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldProceedToNextMapPoint" />.
		/// </summary>
		// Token: 0x06007F69 RID: 32617 RVA: 0x002B28C0 File Offset: 0x002B0AC0
		public static bool ShouldProceedToNextMapPoint(IRunState runState)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (!abstractModel.ShouldProceedToNextMapPoint())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldProcurePotion(MegaCrit.Sts2.Core.Models.PotionModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F6A RID: 32618 RVA: 0x002B2918 File Offset: 0x002B0B18
		public static bool ShouldProcurePotion(IRunState runState, [Nullable(2)] ICombatState combatState, PotionModel potion, Player player)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(combatState))
			{
				if (!abstractModel.ShouldProcurePotion(potion, player))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldRefillMerchantEntry(MegaCrit.Sts2.Core.Entities.Merchant.MerchantEntry,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F6B RID: 32619 RVA: 0x002B2970 File Offset: 0x002B0B70
		public static bool ShouldRefillMerchantEntry(IRunState runState, MerchantEntry entry, Player player)
		{
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				if (abstractModel.ShouldRefillMerchantEntry(entry, player))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldStopCombatFromEnding" />.
		///
		/// Dispatched directly, not through the IterateCombatHookListeners guard: it is a predicate that
		/// drives the decision of whether combat ends, so suppressing it while combat is ending would
		/// drop the votes it collects.
		/// </summary>
		// Token: 0x06007F6C RID: 32620 RVA: 0x002B29C8 File Offset: 0x002B0BC8
		public static bool ShouldStopCombatFromEnding(ICombatState combatState)
		{
			foreach (AbstractModel abstractModel in combatState.IterateHookListeners())
			{
				if (abstractModel.ShouldStopCombatFromEnding())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldTakeExtraTurn(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06007F6D RID: 32621 RVA: 0x002B2A20 File Offset: 0x002B0C20
		public static bool ShouldTakeExtraTurn(ICombatState combatState, Player player)
		{
			foreach (AbstractModel abstractModel in Hook.IterateCombatHookListeners(combatState))
			{
				if (abstractModel.ShouldTakeExtraTurn(player))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldForcePotionReward(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Rooms.RoomType)" />.
		/// </summary>
		// Token: 0x06007F6E RID: 32622 RVA: 0x002B2A78 File Offset: 0x002B0C78
		public static bool ShouldForcePotionReward(IRunState runState, Player player, RoomType roomType)
		{
			bool flag = false;
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				flag = flag || abstractModel.ShouldForcePotionReward(player, roomType);
			}
			return flag;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ShouldAllowFreeTravel" />.
		/// </summary>
		// Token: 0x06007F6F RID: 32623 RVA: 0x002B2AD4 File Offset: 0x002B0CD4
		public static bool ShouldAllowFreeTravel(IRunState runState)
		{
			bool flag = false;
			foreach (AbstractModel abstractModel in runState.IterateHookListeners(null))
			{
				flag = flag || abstractModel.ShouldAllowFreeTravel();
			}
			return flag;
		}

		// Token: 0x06007F70 RID: 32624 RVA: 0x002B2B2C File Offset: 0x002B0D2C
		public static bool ShouldPowerBeRemovedOnDeath(PowerModel power)
		{
			if (power.Owner.CombatState == null)
			{
				return true;
			}
			foreach (AbstractModel abstractModel in power.CombatState.IterateHookListeners())
			{
				if (!abstractModel.ShouldPowerBeRemovedOnDeath(power))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x002B2B98 File Offset: 0x002B0D98
		[NullableContext(2)]
		private static decimal ModifyDamageInternal([Nullable(1)] IRunState runState, ICombatState combatState, Creature target, Creature dealer, decimal damage, ValueProp props, CardModel cardSource, CardPlay cardPlay, ModifyDamageHookType modifyDamageHookType, [Nullable(1)] out List<AbstractModel> modifiers)
		{
			decimal num = damage;
			List<AbstractModel> list = new List<AbstractModel>();
			if (modifyDamageHookType.HasFlag(ModifyDamageHookType.Additive))
			{
				foreach (AbstractModel abstractModel in runState.IterateHookListeners(combatState))
				{
					decimal num2 = abstractModel.ModifyDamageAdditive(target, num, props, dealer, cardSource, cardPlay);
					num += num2;
					if (num2 != 0m)
					{
						list.Add(abstractModel);
					}
				}
			}
			if (modifyDamageHookType.HasFlag(ModifyDamageHookType.Multiplicative))
			{
				foreach (AbstractModel abstractModel2 in runState.IterateHookListeners(combatState))
				{
					decimal num3 = abstractModel2.ModifyDamageMultiplicative(target, num, props, dealer, cardSource, cardPlay);
					num *= num3;
					if (num3 != 1m)
					{
						list.Add(abstractModel2);
					}
				}
			}
			if (modifyDamageHookType.HasFlag(ModifyDamageHookType.Cap))
			{
				decimal num4 = decimal.MaxValue;
				foreach (AbstractModel abstractModel3 in runState.IterateHookListeners(combatState))
				{
					decimal num5 = abstractModel3.ModifyDamageCap(target, props, dealer, cardSource, cardPlay);
					if (num5 < num4)
					{
						num4 = num5;
						if (num > num5)
						{
							num = num5;
							list.Add(abstractModel3);
						}
					}
				}
			}
			modifiers = list;
			return num;
		}
	}
}
