using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004D5 RID: 1237
	[NullableContext(1)]
	[Nullable(0)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public abstract class AbstractModel : IComparable<AbstractModel>
	{
		/// <summary>
		/// Fires when a hook on this model has finished executing.
		/// This event is a little unreliable, so you should only use it for UI things that can't be done any other way.
		/// </summary>
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06004971 RID: 18801 RVA: 0x0022617C File Offset: 0x0022437C
		// (remove) Token: 0x06004972 RID: 18802 RVA: 0x002261B4 File Offset: 0x002243B4
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<AbstractModel> ExecutionFinished;

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x002261E9 File Offset: 0x002243E9
		public ModelId Id { get; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004974 RID: 18804 RVA: 0x002261F1 File Offset: 0x002243F1
		// (set) Token: 0x06004975 RID: 18805 RVA: 0x002261F9 File Offset: 0x002243F9
		public bool IsMutable { get; private set; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x00226202 File Offset: 0x00224402
		public bool IsCanonical
		{
			get
			{
				return !this.IsMutable;
			}
		}

		/// <summary>
		/// The category ID of the model, as reported by ModelIdSerializationCache.
		/// Used in the deterministic sort in DeterministicModelComparer for speed purposes; it's much faster to compare
		/// integers than strings.
		/// </summary>
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x0022620D File Offset: 0x0022440D
		// (set) Token: 0x06004978 RID: 18808 RVA: 0x00226215 File Offset: 0x00224415
		public int CategorySortingId { get; private set; }

		/// <summary>
		/// The entry ID of the model, as reported by ModelIdSerializationCache.
		/// Used in the deterministic sort in DeterministicModelComparer for speed purposes; it's much faster to compare
		/// integers than strings.
		/// </summary>
		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x0022621E File Offset: 0x0022441E
		// (set) Token: 0x0600497A RID: 18810 RVA: 0x00226226 File Offset: 0x00224426
		public int EntrySortingId { get; private set; }

		// Token: 0x0600497B RID: 18811 RVA: 0x00226230 File Offset: 0x00224430
		protected AbstractModel()
		{
			Type type = base.GetType();
			ModelId id = ModelDb.GetId(type);
			AbstractModel byIdOrNull = ModelDb.GetByIdOrNull<AbstractModel>(id);
			if (byIdOrNull != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(221, 3);
				defaultInterpolatedStringHandler.AppendLiteral("ModelDb already contains ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
				defaultInterpolatedStringHandler.AppendLiteral(" mapped to type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(byIdOrNull.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(", but you are trying to map it to type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(". Possible causes:\n");
				defaultInterpolatedStringHandler.AppendLiteral(" - You have called a constructor on an AbstractModel. Use ModelDb instead.\n");
				defaultInterpolatedStringHandler.AppendLiteral(" - There is a conflict in mod content names.");
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				throw new DuplicateModelException(text);
			}
			this.Id = id;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x002262E6 File Offset: 0x002244E6
		public void InitId(ModelId id)
		{
			this.AssertCanonical();
			this.CategorySortingId = ModelIdSerializationCache.GetNetIdForCategory(this.Id.Category);
			this.EntrySortingId = ModelIdSerializationCache.GetNetIdForEntry(this.Id.Entry);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x0022631A File Offset: 0x0022451A
		[NullableContext(2)]
		public virtual int CompareTo(AbstractModel other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			return this.Id.CompareTo(other.Id);
		}

		/// <summary>
		/// Is this a mock model used only for testing?
		/// Overridden to return true in mock model subclasses.
		/// </summary>
		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600497E RID: 18814 RVA: 0x00226338 File Offset: 0x00224538
		public virtual bool IsMock
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Ensures that this model instance is mutable. Throws an exception if it's canonical.
		/// Use this in places where you want to ensure that you have a "real" model that you can use in combat and modify.
		///
		/// WARNING: If you're getting an exception from here, don't just convert the model to mutable at the top level
		/// of the stack trace. Instead, find the root of where the model is coming from and convert it there, so the
		/// correct instance is used in the correct places.
		///
		/// For example, canonical CardModels throw an exception if you try to add them to a pile, but you shouldn't just
		/// convert the CardModel to mutable in CardPile.AddCard(). Instead, find where that card was created and
		/// convert it there.
		/// </summary>
		/// <exception cref="T:MegaCrit.Sts2.Core.Models.Exceptions.CanonicalModelException"></exception>
		// Token: 0x0600497F RID: 18815 RVA: 0x0022633B File Offset: 0x0022453B
		public void AssertMutable()
		{
			if (!this.IsMutable)
			{
				throw new CanonicalModelException(base.GetType());
			}
		}

		/// <summary>
		/// Ensures that this model instance is canonical. Throws an exception if it's mutable.
		/// Use this in places where you want a reference to the "concept" of a model. For example, CardPools only hold
		/// canonical CardModels.
		/// <exception cref="T:MegaCrit.Sts2.Core.Models.Exceptions.MutableModelException"></exception>
		/// </summary>
		// Token: 0x06004980 RID: 18816 RVA: 0x00226351 File Offset: 0x00224551
		public void AssertCanonical()
		{
			if (this.IsMutable)
			{
				throw new MutableModelException(base.GetType());
			}
		}

		/// <summary>
		/// Get a "clone" of this model, preserving its mutability status.
		/// This is useful in more backend-ish areas of the codebase, where a model can either be mutable or canonical,
		/// and we don't want to create a new mutable clone if we currently have a reference to the canonical instance.
		/// </summary>
		/// <returns>
		/// If this instance of the model is canonical, this method just returns itself.
		/// If this instance of the model is mutable, it returns another mutable clone.
		/// </returns>
		// Token: 0x06004981 RID: 18817 RVA: 0x00226367 File Offset: 0x00224567
		public AbstractModel ClonePreservingMutability()
		{
			if (!this.IsMutable)
			{
				return this;
			}
			return this.MutableClone();
		}

		/// <summary>
		/// WARNING: You almost always want to use `ToMutable()` or `ClonePreservingMutability()` instead, since we usually
		/// don't want to make mutable clones of already-mutable models.
		///
		/// Get a mutable "clone" of this model.
		/// This is useful in very generalized spots where we could either have a mutable or canonical model, and we want a
		/// mutable clone regardless.
		/// </summary>
		/// <returns>A mutable clone of this model.</returns>
		// Token: 0x06004982 RID: 18818 RVA: 0x0022637C File Offset: 0x0022457C
		public AbstractModel MutableClone()
		{
			AbstractModel abstractModel = (AbstractModel)base.MemberwiseClone();
			abstractModel.IsMutable = true;
			abstractModel.DeepCloneFields();
			abstractModel.AfterCloned();
			return abstractModel;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x002263A9 File Offset: 0x002245A9
		protected virtual void DeepCloneFields()
		{
			this.AssertMutable();
		}

		/// <summary>
		/// Called after cloning to clean up shallow-copied references.
		/// IMPORTANT: MemberwiseClone() creates shallow copies of all fields, including event delegates.
		/// Subclasses that declare events MUST override this method and set all events to null,
		/// otherwise clones will fire events to the original's subscribers (correctness bug) and may
		/// prevent garbage collection through retained delegate references (memory leak).
		/// </summary>
		// Token: 0x06004984 RID: 18820 RVA: 0x002263B1 File Offset: 0x002245B1
		protected virtual void AfterCloned()
		{
			this.ExecutionFinished = null;
		}

		/// <summary>
		/// See <see cref="E:MegaCrit.Sts2.Core.Models.AbstractModel.ExecutionFinished" />.
		/// </summary>
		// Token: 0x06004985 RID: 18821 RVA: 0x002263BA File Offset: 0x002245BA
		public void InvokeExecutionFinished()
		{
			Action<AbstractModel> executionFinished = this.ExecutionFinished;
			if (executionFinished == null)
			{
				return;
			}
			executionFinished(this);
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004986 RID: 18822 RVA: 0x002263CD File Offset: 0x002245CD
		public virtual bool PreviewOutsideOfCombat
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Whether or not this model should have combat hooks called on it.
		/// For example, AfterCardPlayed is only relevant in combat, and should be called on models that want to "listen" to
		/// the combat (powers, relics, cards in a combat pile, enchantments on those cards, etc.), but not on models that
		/// are disconnected from combat (cards in your deck, enchantments in your deck, etc.).
		/// Conversely, AfterRoomEntered is relevant outside of combat, and should be called on all models.
		/// Similarly, AfterDamageReceived is relevant outside of combat (since damage can be received in events and other
		/// non-combat rooms), so it should be called on all models.
		/// </summary>
		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004987 RID: 18823
		public abstract bool ShouldReceiveCombatHooks { get; }

		/// <summary>
		/// Runs after a player enters a new act.
		/// </summary>
		// Token: 0x06004988 RID: 18824 RVA: 0x002263D0 File Offset: 0x002245D0
		public virtual Task AfterActEntered()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a model prevents a card from being added to the player's deck.
		/// </summary>
		/// <param name="card">Card that would've been added.</param>
		// Token: 0x06004989 RID: 18825 RVA: 0x002263D7 File Offset: 0x002245D7
		public virtual Task AfterAddToDeckPrevented(CardModel card)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a creature attacks.
		/// For a multi-attack, this will run once before any hits are done (as opposed to BeforeDamageGiven, which will run
		/// before each hit).
		/// </summary>
		/// <param name="command">The attack command that will be executed.</param>
		// Token: 0x0600498A RID: 18826 RVA: 0x002263DE File Offset: 0x002245DE
		public virtual Task BeforeAttack(AttackCommand command)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature attacks.
		/// For a multi-attack, this will run once after all hits are done (as opposed to AfterDamageGiven, which will run
		/// after each hit).
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="command">The attack command that was executed.</param>
		// Token: 0x0600498B RID: 18827 RVA: 0x002263E5 File Offset: 0x002245E5
		public virtual Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPostPlay" /> phase of a player's turn has been entered.
		/// Effects that auto-play cards at the end of a player's turn should use this hook.
		/// See <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPostPlay" /> for examples.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPostPlay" /> phase was entered.</param>
		// Token: 0x0600498C RID: 18828 RVA: 0x002263EC File Offset: 0x002245EC
		public virtual Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterAutoPrePlayPhaseEntered instead of this.
		/// Runs after the <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase of a player's turn has been entered.
		/// Effects that auto-play cards at the start of a player's turn should use this hook.
		/// See <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> for examples.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase was entered.</param>
		// Token: 0x0600498D RID: 18829 RVA: 0x002263F3 File Offset: 0x002245F3
		public virtual Task AfterAutoPrePlayPhaseEnteredEarly(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase of a player's turn has been entered.
		/// Effects that auto-play cards at the start of a player's turn should use this hook.
		/// See <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> for examples.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase was entered.</param>
		// Token: 0x0600498E RID: 18830 RVA: 0x002263FA File Offset: 0x002245FA
		public virtual Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterAutoPrePlayPhaseEntered instead of this.
		/// Runs after the <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase of a player's turn has been entered.
		/// Effects that auto-play cards at the start of a player's turn should use this hook.
		/// See <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> for examples.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> phase was entered.</param>
		// Token: 0x0600498F RID: 18831 RVA: 0x00226401 File Offset: 0x00224601
		public virtual Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature's block has been cleared at the beginning of their round.
		/// Combat-only hook.
		/// </summary>
		/// <param name="creature">Creature whose block was reset.</param>
		// Token: 0x06004990 RID: 18832 RVA: 0x00226408 File Offset: 0x00224608
		public virtual Task AfterBlockCleared(Creature creature)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a creature gains block.
		/// Combat-only hook.
		/// </summary>
		/// <param name="creature">Creature that will gain block.</param>
		/// <param name="amount">Amount of block they will gain.</param>
		/// <param name="props">ValueProps for the block they will gain.</param>
		/// <param name="cardSource">Optional card that will add the block.</param>
		// Token: 0x06004991 RID: 18833 RVA: 0x0022640F File Offset: 0x0022460F
		public virtual Task BeforeBlockGained(Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature gains block.
		/// Combat-only hook.
		/// </summary>
		/// <param name="creature">Creature that gained block.</param>
		/// <param name="amount">Amount of block they gained.</param>
		/// <param name="props">ValueProps for the block they gained.</param>
		/// <param name="cardSource">Optional card that added the block.</param>
		// Token: 0x06004992 RID: 18834 RVA: 0x00226416 File Offset: 0x00224616
		public virtual Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature's block is completely removed.
		/// Prefer this over checking result.WasBlockBroken in AfterDamageReceived, as block can be broken by non-damage,
		/// like Expose.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature whose block was broken.</param>
		/// <param name="breaker">
		/// Creature who broke target's block.
		/// Null if no creature was involved in the breaking, like if it was removed by a power.
		/// </param>
		// Token: 0x06004993 RID: 18835 RVA: 0x0022641D File Offset: 0x0022461D
		public virtual Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, [Nullable(2)] Creature breaker)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card moves from one pile to another.
		/// NOTE: The new pile can be determined by just checking card.Pile.
		/// </summary>
		/// <param name="card">Card that changed piles.</param>
		/// <param name="oldPileType">Type of the card's previous pile</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		// Token: 0x06004994 RID: 18836 RVA: 0x00226424 File Offset: 0x00224624
		public virtual Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterCardChangedPiles instead of this.
		/// Runs after a card moves from one pile to another.
		/// Works just like AfterCardChangedPiles, but runs after it.
		/// NOTE: The new pile can be determined by just checking card.Pile.
		/// </summary>
		/// <param name="card">Card that changed piles.</param>
		/// <param name="oldPileType">Type of the card's previous pile</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		// Token: 0x06004995 RID: 18837 RVA: 0x0022642B File Offset: 0x0022462B
		public virtual Task AfterCardChangedPilesLate(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card is discarded.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card that was discarded.</param>
		// Token: 0x06004996 RID: 18838 RVA: 0x00226432 File Offset: 0x00224632
		public virtual Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterCardDrawn instead of this.
		/// Runs after a card is drawn.
		/// Combat-only hook.
		/// Works just like AfterCardDrawn, but runs before it.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card that was drawn.</param>
		/// <param name="fromHandDraw">If this draw happened as part of the initial card draws at the start of your turn.</param>
		// Token: 0x06004997 RID: 18839 RVA: 0x00226439 File Offset: 0x00224639
		public virtual Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card is drawn.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card that was drawn.</param>
		/// <param name="fromHandDraw">If this draw happened as part of the initial card draws at the start of your turn.</param>
		// Token: 0x06004998 RID: 18840 RVA: 0x00226440 File Offset: 0x00224640
		public virtual Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card is put into a combat pile (see <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.PileTypeExtensions.IsCombatPile(MegaCrit.Sts2.Core.Entities.Cards.PileType)" /> details on combat piles).
		/// Combat-only hook.
		/// </summary>
		/// <param name="card">Card that was put into a combat pile.</param>
		// Token: 0x06004999 RID: 18841 RVA: 0x00226447 File Offset: 0x00224647
		public virtual Task AfterCardEnteredCombat(CardModel card)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player generated a card for combat pile. This is different from AfterCardEnteredCombat
		/// because the player explicitly generated it themselves, excluding status cards that are applied by enemies.
		/// Combat-only hook.
		/// </summary>
		/// <param name="card">Card that was put into a combat pile.</param>
		/// <param name="creator">The player that created the card
		/// True if this card is being added by an effect from the player (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.InfiniteBlades" />).
		/// False if an enemy is adding it (like a monster giving you a curse).
		/// </param>
		// Token: 0x0600499A RID: 18842 RVA: 0x0022644E File Offset: 0x0022464E
		public virtual Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card is exhausted.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card that was exhausted.</param>
		/// <param name="causedByEthereal">Was this Exhaust caused by Ethereal?</param>
		// Token: 0x0600499B RID: 18843 RVA: 0x00226455 File Offset: 0x00224655
		public virtual Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a card is automatically played by another card.
		/// Combat-only hook.
		/// </summary>
		/// <param name="card">Card that will be played.</param>
		/// <param name="target">Creature that will be targeted.</param>
		/// <param name="type">The method of autoplay.</param>
		// Token: 0x0600499C RID: 18844 RVA: 0x0022645C File Offset: 0x0022465C
		public virtual Task BeforeCardAutoPlayed(CardModel card, [Nullable(2)] Creature target, AutoPlayType type)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a card is played.
		/// Combat-only hook.
		/// </summary>
		// Token: 0x0600499D RID: 18845 RVA: 0x00226463 File Offset: 0x00224663
		public virtual Task BeforeCardPlayed(CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a card is played.
		/// Combat-only hook.
		/// </summary>
		// Token: 0x0600499E RID: 18846 RVA: 0x0022646A File Offset: 0x0022466A
		public virtual Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterCardPlayed instead of this.
		/// Runs after a card is played.
		/// Works just like AfterCardPlayed, but runs after it.
		/// Combat-only hook.
		/// </summary>
		// Token: 0x0600499F RID: 18847 RVA: 0x00226471 File Offset: 0x00224671
		public virtual Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before combat starts.
		/// Note: This may sound like a combat-only hook, but combat start is relevant to non-combat models (like a deck
		/// card that transforms when you start your third combat).
		/// </summary>
		// Token: 0x060049A0 RID: 18848 RVA: 0x00226478 File Offset: 0x00224678
		public virtual Task BeforeCombatStart()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use BeforeCombatStart instead of this.
		/// Runs before combat starts.
		/// Note: This may sound like a combat-only hook, but combat start is relevant to non-combat models (like a deck
		/// card that transforms when you start your third combat).
		/// Works just like BeforeCombatStart, but runs after it.
		/// </summary>
		// Token: 0x060049A1 RID: 18849 RVA: 0x0022647F File Offset: 0x0022467F
		public virtual Task BeforeCombatStartLate()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after combat ends.
		/// Note: This may sound like a combat-only hook, but combat end is relevant to non-combat models (like Guilty).
		/// </summary>
		/// <param name="room">The room where the combat ended.</param>
		// Token: 0x060049A2 RID: 18850 RVA: 0x00226486 File Offset: 0x00224686
		public virtual Task AfterCombatEnd(CombatRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before the combat rewards are offered to the player.
		/// Note: This may sound like a combat-only hook, but the rewards are relevant to non-combat models (like a relic
		/// that adds a bonus reward every few combats).
		/// </summary>
		/// <param name="rewards">The set of rewards about to be offered.</param>
		/// <param name="room">The room where the combat ended.</param>
		// Token: 0x060049A3 RID: 18851 RVA: 0x0022648D File Offset: 0x0022468D
		public virtual Task BeforeCombatRewardOffered(RewardsSet rewards, CombatRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterCombatVictory instead of this.
		/// Runs after combat ends in a player victory.
		/// Note: This may sound like a combat-only hook, but combat victory is relevant to non-combat models (like a deck
		/// card that transforms after 3 combat victories).
		/// Works just like AfterCombatVictory, but runs before it.
		/// </summary>
		/// <param name="room">The room where the player won the combat.</param>
		// Token: 0x060049A4 RID: 18852 RVA: 0x00226494 File Offset: 0x00224694
		public virtual Task AfterCombatVictoryEarly(CombatRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after combat ends in a player victory.
		/// Note: This may sound like a combat-only hook, but combat victory is relevant to non-combat models (like a deck
		/// card that transforms after 3 combat victories).
		/// </summary>
		/// <param name="room">The room where the player won the combat.</param>
		// Token: 0x060049A5 RID: 18853 RVA: 0x0022649B File Offset: 0x0022469B
		public virtual Task AfterCombatVictory(CombatRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a new creature is added to combat.
		/// </summary>
		/// <param name="creature">Creature that was added to combat.</param>
		// Token: 0x060049A6 RID: 18854 RVA: 0x002264A2 File Offset: 0x002246A2
		public virtual Task AfterCreatureAddedToCombat(Creature creature)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature's HP changes for any reason (damage, heal, HP loss, etc).
		/// </summary>
		/// <param name="creature">Creature whose HP changed.</param>
		/// <param name="delta">
		///     Amount that the creature's HP changed by.
		///     A negative amount represents damage, while a positive amount represents healing.
		/// </param>
		// Token: 0x060049A7 RID: 18855 RVA: 0x002264A9 File Offset: 0x002246A9
		public virtual Task AfterCurrentHpChanged(Creature creature, decimal delta)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature might dealt damage.
		/// Note: Even if the actual damage amount is 0 (due to block or something else), this will still be called.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="dealer">Creature who dealt the damage.</param>
		/// <param name="result">Results from the damage they dealt.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="target">Creature who received the damage.</param>
		/// <param name="cardSource">Optional card that dealt the damage.</param>
		// Token: 0x060049A8 RID: 18856 RVA: 0x002264B0 File Offset: 0x002246B0
		public virtual Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a creature might receive damage.
		/// Note: Even if the actual damage amount will be 0 (due to Weak or something else), this will still be called.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature who will receive the damage.</param>
		/// <param name="amount">Amount of damage they will receive.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who will be dealt the damage.</param>
		/// <param name="cardSource">Optional card that will deal the damage.</param>
		// Token: 0x060049A9 RID: 18857 RVA: 0x002264B7 File Offset: 0x002246B7
		public virtual Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature might receive damage.
		/// Note: Even if the actual damage amount is 0 (due to block or something else), this will still be called.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature who received the damage.</param>
		/// <param name="result">Results from the damage they received.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who dealt the damage.</param>
		/// <param name="cardSource">Optional card that dealt the damage.</param>
		// Token: 0x060049AA RID: 18858 RVA: 0x002264BE File Offset: 0x002246BE
		public virtual Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterDamageReceived instead of this.
		/// Runs after a creature might receive damage.
		/// Works just like AfterDamageReceived, but runs after it.
		/// Note: Even if the actual damage amount is 0 (due to block or something else), this will still be called.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature who received the damage.</param>
		/// <param name="result">Results from the damage they received.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who dealt the damage.</param>
		/// <param name="cardSource">Optional card that dealt the damage.</param>
		// Token: 0x060049AB RID: 18859 RVA: 0x002264C5 File Offset: 0x002246C5
		public virtual Task AfterDamageReceivedLate(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a creature dies.
		/// Note: This will run even if some effect (like Fairy in a Bottle) would prevent the creature's death.
		/// </summary>
		/// <param name="creature">Creature who is about to die.</param>
		// Token: 0x060049AC RID: 18860 RVA: 0x002264CC File Offset: 0x002246CC
		public virtual Task BeforeDeath(Creature creature)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a creature dies.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="creature">Creature who died.</param>
		/// <param name="wasRemovalPrevented">
		/// Whether the creature's removal from combat was prevented. Usually false, but true for things like reviving
		/// powers.
		/// </param>
		/// <param name="deathAnimLength">
		/// Number of seconds the creature's death animation will last. Good for delaying visuals until the animation
		/// is over.
		/// </param>
		// Token: 0x060049AD RID: 18861 RVA: 0x002264D3 File Offset: 0x002246D3
		public virtual Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after creatures died to Doom.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="creatures">The creatures that died to Doom.</param>
		// Token: 0x060049AE RID: 18862 RVA: 0x002264DA File Offset: 0x002246DA
		public virtual Task AfterDiedToDoom(PlayerChoiceContext choiceContext, IReadOnlyList<Creature> creatures)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the player's energy is reset at the beginning of their turn.
		/// Combat-only hook.
		/// </summary>
		/// <param name="player">Player whose energy is reset.</param>
		// Token: 0x060049AF RID: 18863 RVA: 0x002264E1 File Offset: 0x002246E1
		public virtual Task AfterEnergyReset(Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterEnergyReset instead of this.
		/// Runs after the player's energy is reset at the beginning of their turn.
		/// Combat-only hook.
		/// Works just like AfterEnergyReset, but runs after it.
		/// </summary>
		/// <param name="player">Player whose energy is reset.</param>
		// Token: 0x060049B0 RID: 18864 RVA: 0x002264E8 File Offset: 0x002246E8
		public virtual Task AfterEnergyResetLate(Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the player's energy is spent.
		/// Combat-only hook.
		/// </summary>
		/// <param name="card">Card that spent the energy was spent on.</param>
		/// <param name="amount">Amount of energy that was spent.</param>
		// Token: 0x060049B1 RID: 18865 RVA: 0x002264EF File Offset: 0x002246EF
		public virtual Task AfterEnergySpent(CardModel card, int amount)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a card is removed from your deck.
		/// </summary>
		/// <param name="card">Card that is being removed.</param>
		// Token: 0x060049B2 RID: 18866 RVA: 0x002264F6 File Offset: 0x002246F6
		public virtual Task BeforeCardRemoved(CardModel card)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a player's hand is flushed at the end of their turn.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose hand will be flushed.</param>
		// Token: 0x060049B3 RID: 18867 RVA: 0x002264FD File Offset: 0x002246FD
		public virtual Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use BeforeFlush instead of this.
		/// Runs before a player's hand is flushed at the end of their turn.
		/// Combat-only hook.
		/// Works just like BeforeFlush, but runs after it.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose hand will be flushed.</param>
		// Token: 0x060049B4 RID: 18868 RVA: 0x00226504 File Offset: 0x00224704
		public virtual Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player's hand is flushed at the end of their turn.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose hand was flushed.</param>
		/// <param name="flushedCards">Cards that were discarded during the flush.</param>
		/// <param name="retainedCards">Cards that were retained during the flush.</param>
		/// <returns></returns>
		// Token: 0x060049B5 RID: 18869 RVA: 0x0022650B File Offset: 0x0022470B
		public virtual Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the player gains gold.
		/// </summary>
		/// <param name="player">Player who gained the gold.</param>
		// Token: 0x060049B6 RID: 18870 RVA: 0x00226512 File Offset: 0x00224712
		public virtual Task AfterGoldGained(Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before the player draws their hand at the beginning of their turn.
		/// Combat-only hook.
		/// </summary>
		/// <param name="player">The player who is about to draw cards.</param>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="combatState">The CombatState that the hand draw will occur in.</param>
		// Token: 0x060049B7 RID: 18871 RVA: 0x00226519 File Offset: 0x00224719
		public virtual Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use BeforeHandDraw instead of this.
		/// Runs before the player draws their hand at the beginning of their turn.
		/// Combat-only hook.
		/// Works just like BeforeHandDraw, but runs after it.
		/// </summary>
		/// <param name="player">The player who is about to draw cards.</param>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="combatState">The CombatState that the hand draw will occur in.</param>
		// Token: 0x060049B8 RID: 18872 RVA: 0x00226520 File Offset: 0x00224720
		public virtual Task BeforeHandDrawLate(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the player's hand becomes empty.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose hand was emptied.</param>
		// Token: 0x060049B9 RID: 18873 RVA: 0x00226527 File Offset: 0x00224727
		public virtual Task AfterHandEmptied(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player purchase an item from the shop.
		/// </summary>
		/// <param name="player">Player who purchased the item.</param>
		/// <param name="itemPurchased">Item that was purchased.</param>
		/// <param name="goldSpent">Amount of gold that was spent to purchase the item.</param>
		// Token: 0x060049BA RID: 18874 RVA: 0x0022652E File Offset: 0x0022472E
		public virtual Task AfterItemPurchased(Player player, MerchantEntry itemPurchased, int goldSpent)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a map is generated for an act.
		/// </summary>
		/// <param name="map">The generated map.</param>
		/// <param name="actIndex">The act index for which the map was generated.</param>
		// Token: 0x060049BB RID: 18875 RVA: 0x00226535 File Offset: 0x00224735
		public virtual Task AfterMapGenerated(ActMap map, int actIndex)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of block was modified.
		/// Combat-only hook.
		/// </summary>
		/// <param name="modifiedAmount">The amount of block after all modifications have been applied.</param>
		/// <param name="cardSource">Card that added the block.</param>
		/// <param name="cardPlay">
		/// CardPlay that added block.
		/// Null if the block is not associated with a CardPlay.
		/// This can be null even if <paramref name="cardSource" /> is not null, because this is also called to preview block
		/// values on cards that are in your hand and haven't been played yet.
		/// </param>
		// Token: 0x060049BC RID: 18876 RVA: 0x0022653C File Offset: 0x0022473C
		[NullableContext(2)]
		[return: Nullable(1)]
		public virtual Task AfterModifyingBlockAmount(decimal modifiedAmount, CardModel cardSource, CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the play count of a card was modified.
		/// </summary>
		/// <param name="card">Card whose play count was modified.</param>
		// Token: 0x060049BD RID: 18877 RVA: 0x00226543 File Offset: 0x00224743
		public virtual Task AfterModifyingCardPlayCount(CardModel card)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the result pile of a played card was modified.
		/// </summary>
		/// <param name="card">Card whose result pile was modified.</param>
		/// <param name="cardLocation">Final location that the card will be put in.</param>
		// Token: 0x060049BE RID: 18878 RVA: 0x0022654A File Offset: 0x0022474A
		public virtual Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the trigger count of an orb was modified.
		/// </summary>
		/// <param name="orb">Orb whose trigger count was modified.</param>
		// Token: 0x060049BF RID: 18879 RVA: 0x00226551 File Offset: 0x00224751
		public virtual Task AfterModifyingOrbPassiveTriggerCount(OrbModel orb)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the play count of a card was modified.
		/// </summary>
		// Token: 0x060049C0 RID: 18880 RVA: 0x00226558 File Offset: 0x00224758
		public virtual Task AfterModifyingCardRewardOptions()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of damage was modified.
		/// </summary>
		// Token: 0x060049C1 RID: 18881 RVA: 0x0022655F File Offset: 0x0022475F
		public virtual Task AfterModifyingDamageAmount([Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of energy to gain was modified.
		/// </summary>
		// Token: 0x060049C2 RID: 18882 RVA: 0x00226566 File Offset: 0x00224766
		public virtual Task AfterModifyingEnergyGain()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of gold to gain was modified.
		/// </summary>
		/// <param name="player">Player who is about to gain gold.</param>
		/// <param name="amount">The modified amount of gold they will gain.</param>
		// Token: 0x060049C3 RID: 18883 RVA: 0x0022656D File Offset: 0x0022476D
		public virtual Task AfterModifyingGoldGained(Player player, decimal amount)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the number of cards drawn at the beginning of the player's turn was modified.
		/// Combat-only hook.
		/// </summary>
		// Token: 0x060049C4 RID: 18884 RVA: 0x00226574 File Offset: 0x00224774
		public virtual Task AfterModifyingHandDraw()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after card draw was prevented.
		/// Combat-only hook.
		/// </summary>
		// Token: 0x060049C5 RID: 18885 RVA: 0x0022657B File Offset: 0x0022477B
		public virtual Task AfterPreventingDraw()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of HP a creature should lose (before Osty redirection) was modified.
		/// </summary>
		// Token: 0x060049C6 RID: 18886 RVA: 0x00226582 File Offset: 0x00224782
		public virtual Task AfterModifyingHpLostBeforeOsty()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the amount of HP a creature should lose (after Osty redirection) was modified.
		/// </summary>
		// Token: 0x060049C7 RID: 18887 RVA: 0x00226589 File Offset: 0x00224789
		public virtual Task AfterModifyingHpLostAfterOsty()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a power has its amount modified during application.
		/// Combat-only hook.
		/// </summary>
		/// <param name="power">Power whose amount was modified.</param>
		// Token: 0x060049C8 RID: 18888 RVA: 0x00226590 File Offset: 0x00224790
		public virtual Task AfterModifyingPowerAmountReceived(PowerModel power)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a power has its amount modified during application.
		/// Combat-only hook.
		/// </summary>
		/// <param name="power">Power whose amount was modified.</param>
		// Token: 0x060049C9 RID: 18889 RVA: 0x00226597 File Offset: 0x00224797
		public virtual Task AfterModifyingPowerAmountGiven(PowerModel power)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a list of rewards has its contents modified.
		/// </summary>
		// Token: 0x060049CA RID: 18890 RVA: 0x0022659E File Offset: 0x0022479E
		public virtual Task AfterModifyingRewards()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after player channeled an orb
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player who channeled the orb.</param>
		/// <param name="orb">Orb that was channeled.</param>
		// Token: 0x060049CB RID: 18891 RVA: 0x002265A5 File Offset: 0x002247A5
		public virtual Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after player evokes a orb
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="orb">Orb that was evoked.</param>
		/// <param name="targets">who the orb affected</param>
		// Token: 0x060049CC RID: 18892 RVA: 0x002265AC File Offset: 0x002247AC
		public virtual Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after Osty is revived.
		/// </summary>
		/// <param name="osty">The osty that was revived</param>
		// Token: 0x060049CD RID: 18893 RVA: 0x002265B3 File Offset: 0x002247B3
		public virtual Task AfterOstyRevived(Creature osty)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a potion is used.
		/// Note: This only invokes for relics when out of combat.
		/// </summary>
		/// <param name="potion">Potion that will be used.</param>
		/// <param name="target">Creature that will be targeted.</param>
		// Token: 0x060049CE RID: 18894 RVA: 0x002265BA File Offset: 0x002247BA
		public virtual Task BeforePotionUsed(PotionModel potion, [Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a potion is used.
		/// Note: This only invokes for relics when out of combat.
		/// </summary>
		/// <param name="potion">Potion that was used.</param>
		/// <param name="target">Creature that was targeted.</param>
		// Token: 0x060049CF RID: 18895 RVA: 0x002265C1 File Offset: 0x002247C1
		public virtual Task AfterPotionUsed(PotionModel potion, [Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a potion is discarded.
		/// Note: This only invokes for relics when out of combat.
		/// </summary>
		/// <param name="potion">Potion that was used.</param>
		// Token: 0x060049D0 RID: 18896 RVA: 0x002265C8 File Offset: 0x002247C8
		public virtual Task AfterPotionDiscarded(PotionModel potion)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a potion is procured.
		/// Note: This only invokes for relics when out of combat.
		/// </summary>
		/// <param name="potion">Potion that was used.</param>
		// Token: 0x060049D1 RID: 18897 RVA: 0x002265CF File Offset: 0x002247CF
		public virtual Task AfterPotionProcured(PotionModel potion)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a power's amount is changed, whether it's a new power being added or an existing power's amount being
		/// changed.
		/// Combat-only hook.
		/// </summary>
		/// <param name="power">Power that will be applied.</param>
		/// <param name="amount">Amount of the power that will be added.</param>
		/// <param name="target">The creature that the power will be added to</param>
		/// <param name="applier">
		/// (Optional) The creature that will change the power amount. Null if the change is not caused by a creature (usually
		/// in tests).
		/// </param>
		/// <param name="cardSource">Optional card that is changing the power amount.</param>
		// Token: 0x060049D2 RID: 18898 RVA: 0x002265D6 File Offset: 0x002247D6
		public virtual Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a power's amount is changed, whether it's a new power being added or an existing power's amount being
		/// changed.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="power">Power that was applied.</param>
		/// <param name="amount">Amount of the power that was added</param>
		/// <param name="applier">
		/// (Optional) The creature that changed the power amount. Null if the change was not caused by a creature (usually
		/// in tests).
		/// </param>
		/// <param name="cardSource">
		/// (Optional) The card that changed the power amount. Null if the change was not caused by a card.
		/// </param>
		// Token: 0x060049D3 RID: 18899 RVA: 0x002265DD File Offset: 0x002247DD
		public virtual Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a power prevents the creature from clearing block.
		/// Combat-only hook.
		/// </summary>
		/// <param name="preventer">Model that prevented the block clear.</param>
		/// <param name="creature">Creature whose block clear was prevented.</param>
		// Token: 0x060049D4 RID: 18900 RVA: 0x002265E4 File Offset: 0x002247E4
		public virtual Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after this model prevents a creature's death.
		/// </summary>
		/// <param name="creature">Creature whose death was prevented.</param>
		// Token: 0x060049D5 RID: 18901 RVA: 0x002265EB File Offset: 0x002247EB
		public virtual Task AfterPreventingDeath(Creature creature)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// After player rests at a rest site.
		/// </summary>
		/// <param name="player">Player that rested.</param>
		/// <param name="isMimicked">
		/// Is a mimicked rest site heal?
		/// Used in spots that mimicking a rest site heal, but are not actual rest site heals.
		/// For example: <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" /> mimicks a rest site heal when you select certain options.
		/// This should count as a rest site heal for <see cref="T:MegaCrit.Sts2.Core.Models.Relics.RegalPillow" />, but not for <see cref="T:MegaCrit.Sts2.Core.Models.Modifiers.NightTerrors" />.
		/// </param>
		// Token: 0x060049D6 RID: 18902 RVA: 0x002265F2 File Offset: 0x002247F2
		public virtual Task AfterRestSiteHeal(Player player, bool isMimicked)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// After player smiths at a rest site.
		/// </summary>
		/// <param name="player">Player that smithed.</param>
		// Token: 0x060049D7 RID: 18903 RVA: 0x002265F9 File Offset: 0x002247F9
		public virtual Task AfterRestSiteSmith(Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player successfully takes a reward from the reward screen.
		/// </summary>
		/// <param name="player">Player who took the reward.</param>
		/// <param name="reward">The reward taken.</param>
		// Token: 0x060049D8 RID: 18904 RVA: 0x00226600 File Offset: 0x00224800
		public virtual Task AfterRewardTaken(Player player, Reward reward)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before a player enters a room.
		/// </summary>
		/// <param name="room">Room that will be entered.</param>
		// Token: 0x060049D9 RID: 18905 RVA: 0x00226607 File Offset: 0x00224807
		public virtual Task BeforeRoomEntered(AbstractRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player enters a room.
		/// For "start of combat" effects that should execute before the start of the first turn (like applying powers), you
		/// should use this hook with a "room is CombatRoom" check.
		/// For "start of combat" effects that should execute after the start of the first turn (like dealing damage or
		/// gaining block), you should use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" /> with a RoundNumber check instead.
		/// </summary>
		/// <param name="room">Room that was entered.</param>
		// Token: 0x060049DA RID: 18906 RVA: 0x0022660E File Offset: 0x0022480E
		public virtual Task AfterRoomEntered(AbstractRoom room)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player shuffles their draw pile.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="shuffler">Player who shuffled.</param>
		// Token: 0x060049DB RID: 18907 RVA: 0x00226615 File Offset: 0x00224815
		public virtual Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player spends stars on something.
		/// Combat-only hook.
		/// </summary>
		/// <param name="amount">Amount of stars that were spent.</param>
		/// <param name="spender">Player that spent the stars.</param>
		// Token: 0x060049DC RID: 18908 RVA: 0x0022661C File Offset: 0x0022481C
		public virtual Task AfterStarsSpent(int amount, Player spender)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player gains stars.
		/// Combat-only hook.
		/// </summary>
		/// <param name="amount">Amount of stars that were gained.</param>
		/// <param name="gainer">Player that gained the stars.</param>
		// Token: 0x060049DD RID: 18909 RVA: 0x00226623 File Offset: 0x00224823
		public virtual Task AfterStarsGained(int amount, Player gainer)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player triggers a Forge effect, increasing the damage of Sovereign Blade.
		/// Combat-only hook.
		/// </summary>
		/// <param name="amount">Amount of forge that was triggered.</param>
		/// <param name="forger">Player that triggered the forge.</param>
		/// <param name="source">The abstract model that triggered the forging</param>
		// Token: 0x060049DE RID: 18910 RVA: 0x0022662A File Offset: 0x0022482A
		public virtual Task AfterForge(decimal amount, Player forger, [Nullable(2)] AbstractModel source)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after a player summons.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="summoner">Player that summoned.</param>
		/// <param name="amount">Amount that was summoned.</param>
		// Token: 0x060049DF RID: 18911 RVA: 0x00226631 File Offset: 0x00224831
		public virtual Task AfterSummon(PlayerChoiceContext choiceContext, Player summoner, decimal amount)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the player takes an extra turn.
		/// Combat-only hook.
		/// <param name="player">Player that took the extra turn.</param>
		/// </summary>
		// Token: 0x060049E0 RID: 18912 RVA: 0x00226638 File Offset: 0x00224838
		public virtual Task AfterTakingExtraTurn(Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// WARNING: ONLY DO VFX/SFX THINGS IN THIS HOOK. It is not meant for things that affect game state.
		/// Runs after attempting to target this creature with a card is blocked.
		/// </summary>
		/// <param name="blocker">Creature who blocked the targeting.</param>
		// Token: 0x060049E1 RID: 18913 RVA: 0x0022663F File Offset: 0x0022483F
		public virtual Task AfterTargetingBlockedVfx(Creature blocker)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before the start of a side's turn, before any effects like energy reset or card draw.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext"></param>
		/// <param name="side">Side whose turn is about to start.</param>
		/// <param name="participants">
		/// Creatures participating in this turn.
		/// If a player is taking an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		/// <param name="combatState">State of the combat that the turn is about to start in in.</param>
		// Token: 0x060049E2 RID: 18914 RVA: 0x00226646 File Offset: 0x00224846
		public virtual Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the start of a side's turn.
		/// For "start of combat" effects that should execute after the start of the first turn (like dealing damage or
		/// gaining block), you should use this hook with a RoundNumber check.
		/// For "start of combat" effects that should execute before the start of the first turn (like applying powers), you
		/// should use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRoomEntered(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" /> with a "room is CombatRoom" check instead.
		/// <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPlayerTurnStart(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" /> should be used for any hooks that trigger a player choice.
		/// Combat-only hook.
		/// </summary>
		/// <param name="side">Side whose turn started.</param>
		/// <param name="participants">
		/// Creatures participating in this turn.
		/// If a player is taking an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		/// <param name="combatState">State of the combat that the turn is about to start in in.</param>
		// Token: 0x060049E3 RID: 18915 RVA: 0x0022664D File Offset: 0x0022484D
		public virtual Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterSideTurnStart instead of this.
		/// Runs after the start of a side's turn.
		/// For "start of combat" effects that should execute after the start of the first turn (like dealing damage or
		/// gaining block), you should use this hook with a RoundNumber check.
		/// For "start of combat" effects that should execute before the start of the first turn (like applying powers), you
		/// should use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterRoomEntered(MegaCrit.Sts2.Core.Rooms.AbstractRoom)" /> with a "room is CombatRoom" check instead.
		/// <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterPlayerTurnStart(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" /> should be used for any hooks that trigger a player choice.
		/// Combat-only hook.
		/// </summary>
		/// <param name="side">Side whose turn started.</param>
		/// <param name="participants">
		/// Creatures participating in this turn.
		/// If a player is taking an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		/// <param name="combatState">State of the combat that the turn is about to start in in.</param>
		// Token: 0x060049E4 RID: 18916 RVA: 0x00226654 File Offset: 0x00224854
		public virtual Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterPlayerTurnStart instead of this.
		/// Runs after the start of a player's turn.
		/// Most of the time, this and <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" /> are interchangeable. However, if you need to make a
		/// player choice call, do it here. Otherwise, problems will arise when draw effects that trigger player choice
		/// (e.g. Mayhem, Stratagem) interact with the hook.
		/// Works just like AfterPlayerTurnStart, but runs before it.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose turn started.</param>
		// Token: 0x060049E5 RID: 18917 RVA: 0x0022665B File Offset: 0x0022485B
		public virtual Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the start of a player's turn.
		/// Most of the time, this and <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" /> are interchangeable. However, if you need to make a
		/// player choice call, do it here. Otherwise, problems will arise when draw effects that trigger player choice
		/// (e.g. Mayhem, Stratagem) interact with the hook.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose turn started.</param>
		// Token: 0x060049E6 RID: 18918 RVA: 0x00226662 File Offset: 0x00224862
		public virtual Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterPlayerTurnStart instead of this.
		/// Runs after the start of a player's turn.
		/// Most of the time, this and <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},MegaCrit.Sts2.Core.Combat.ICombatState)" /> are interchangeable. However, if you need to make a
		/// player choice call, do it here. Otherwise, problems will arise when draw effects that trigger player choice
		/// (e.g. Mayhem, Stratagem) interact with the hook.
		/// Works just like AfterPlayerTurnStart, but runs after it.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose turn started.</param>
		// Token: 0x060049E7 RID: 18919 RVA: 0x00226669 File Offset: 0x00224869
		public virtual Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use BeforeSideTurnEnd or BeforeSideTurnEndEarly instead of this.
		/// Runs before the end of a side's turn.
		/// Works just like BeforeSideTurnEnd and BeforeSideTurnEndEarly, but runs before either of them.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="side">Side whose turn ended.</param>
		/// <param name="participants">
		/// Creatures who participated in this turn.
		/// If a player took an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		// Token: 0x060049E8 RID: 18920 RVA: 0x00226670 File Offset: 0x00224870
		public virtual Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use BeforeSideTurnEnd instead of this.
		/// Runs before the end of a side's turn.
		/// Works just like BeforeSideTurnEnd, but runs before it.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="side">Side whose turn ended.</param>
		/// <param name="participants">
		/// Creatures who participated in this turn.
		/// If a player took an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		// Token: 0x060049E9 RID: 18921 RVA: 0x00226677 File Offset: 0x00224877
		public virtual Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs before the end of a side's turn.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="side">Side whose turn ended.</param>
		/// <param name="participants">
		/// Creatures who participated in this turn.
		/// If a player took an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		// Token: 0x060049EA RID: 18922 RVA: 0x0022667E File Offset: 0x0022487E
		public virtual Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs after the end of a side's turn.
		/// Note: Enemy-damaging effects (Bedlam Beacon, The Bomb, etc) should NOT go in here. Put them in BeforeSideTurnEnd
		/// instead. Self-damaging effects (Acid Dust, Magic Bomb, etc) can go in here.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="side">Side whose turn ended.</param>
		/// <param name="participants">
		/// Creatures who participated in this turn.
		/// If a player took an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		// Token: 0x060049EB RID: 18923 RVA: 0x00226685 File Offset: 0x00224885
		public virtual Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// CAREFUL! You should usually use AfterSideTurnEnd instead of this.
		/// Runs after the end of a side's turn.
		/// Note: Enemy-damaging effects (Bedlam Beacon, The Bomb, etc) should NOT go in here. Put them in BeforeSideTurnEnd
		/// instead. Self-damaging effects (Acid Dust, Magic Bomb, etc) can go in here.
		/// Combat-only hook.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="side">Side whose turn ended.</param>
		/// <param name="participants">
		/// Creatures who participated in this turn.
		/// If a player took an extra turn in multiplayer, other players may be missing from this list, and the models
		/// they own should usually exit early in this case.
		/// </param>
		// Token: 0x060049EC RID: 18924 RVA: 0x0022668C File Offset: 0x0022488C
		public virtual Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Modify the number of times the specified attack will hit.
		/// </summary>
		/// <param name="attack">Attack whose hit count we're modifying.</param>
		/// <param name="hitCount">The current number of times this attack should hit.</param>
		/// <returns>The new number of times this attack should hit.</returns>
		// Token: 0x060049ED RID: 18925 RVA: 0x00226693 File Offset: 0x00224893
		public virtual int ModifyAttackHitCount(AttackCommand attack, int hitCount)
		{
			return hitCount;
		}

		/// <summary>
		/// Add to the amount of block that will be gained.
		/// Use this for effects that add amounts to existing block, like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DexterityPower" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Fasten" />.
		/// </summary>
		/// <param name="target">Creature who will gain the block.</param>
		/// <param name="block">Initial amount of block.</param>
		/// <param name="props">ValueProp for the block.</param>
		/// <param name="cardSource">
		/// Card that will be adding the block.
		/// Null if the block is coming from something other than a card (like a Relic).
		/// </param>
		/// <param name="cardPlay">
		/// CardPlay that will be adding the block.
		/// Null if the block is not associated with a CardPlay.
		/// This can be null even if <paramref name="cardSource" /> is not null, because this is also called to preview block
		/// values on cards that are in your hand and haven't been played yet.
		/// </param>
		/// <returns>Amount of block to be added.</returns>
		// Token: 0x060049EE RID: 18926 RVA: 0x00226696 File Offset: 0x00224896
		[NullableContext(2)]
		public virtual decimal ModifyBlockAdditive([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			return 0m;
		}

		/// <summary>
		/// Multiply the amount of damage that will be gained.
		/// Use this for effects that multiply existing block by some amount, like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Unmovable" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Powers.FrailPower" />.
		/// </summary>
		/// <param name="target">Creature who will receive the block.</param>
		/// <param name="block">Initial amount of block.</param>
		/// <param name="props">ValueProp for the block.</param>
		/// <param name="cardSource">
		/// Card that will be adding the block.
		/// Null if the block is coming from something other than a card (like a Relic).
		/// </param>
		/// <param name="cardPlay">
		/// CardPlay that will be adding the block.
		/// Null if the block is not associated with a CardPlay.
		/// This can be null even if <paramref name="cardSource" /> is not null, because this is also called to preview block
		/// values on cards that are in your hand and haven't been played yet.
		/// </param>
		/// <returns>Amount that the block should be multiplied by.</returns>
		// Token: 0x060049EF RID: 18927 RVA: 0x0022669D File Offset: 0x0022489D
		[NullableContext(2)]
		public virtual decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			return 1m;
		}

		/// <summary>
		/// Modify the number of times the currently-being-played card will be played.
		/// Good for "Your next card is played twice" effects.
		/// </summary>
		/// <param name="card">The card that is being played.</param>
		/// <param name="playCount">The number of times it should be played.</param>
		/// <param name="target">Creature this card is targeting</param>
		/// <returns>The new number of times this card should be played.</returns>
		// Token: 0x060049F0 RID: 18928 RVA: 0x002266A4 File Offset: 0x002248A4
		public virtual int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			return playCount;
		}

		/// <summary>
		/// Modify the pile that the card will be put in after it is played, and the position it will have in that pile.
		/// </summary>
		/// <param name="card">The card that is being played.</param>
		/// <param name="isAutoPlay">
		/// Whether this card is being auto-played.
		/// False when the player plays the card manually from their hand.
		/// True when played automatically by an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MayhemPower" />.
		/// </param>
		/// <param name="resources">Info about the resources used when playing this card.</param>
		/// <param name="cardLocation">The result location for the card.</param>
		/// <returns>The new location where the card will be placed after play.</returns>
		// Token: 0x060049F1 RID: 18929 RVA: 0x002266A7 File Offset: 0x002248A7
		public virtual CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation cardLocation)
		{
			return cardLocation;
		}

		/// <summary>
		/// Modify the number of times an orb's passive will fire.
		/// Doesn't trigger if the passive is fired manually (ie loop).
		/// </summary>
		/// <param name="orb">Orb whose passive is being triggered.</param>
		/// <param name="triggerCount">The number of times the orb's passive should be triggered.</param>
		/// <returns></returns>
		// Token: 0x060049F2 RID: 18930 RVA: 0x002266AB File Offset: 0x002248AB
		public virtual int ModifyOrbPassiveTriggerCounts(OrbModel orb, int triggerCount)
		{
			return triggerCount;
		}

		/// <summary>
		/// Modify the cards reward options before a card reward is generated from them.
		/// You may remove cards, add cards, or add duplicate cards (to increase the chances of that card showing up) to
		/// the card pool.
		/// </summary>
		/// <param name="player">The player who is being offered rewards.</param>
		/// <param name="options">What the card rewards are being generated for.</param>
		// Token: 0x060049F3 RID: 18931 RVA: 0x002266AE File Offset: 0x002248AE
		public virtual CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
		{
			return options;
		}

		/// <summary>
		/// CAREFUL! You should usually use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardRewardCreationOptions(MegaCrit.Sts2.Core.Entities.Players.Player,MegaCrit.Sts2.Core.Runs.CardCreationOptions)" /> instead of this.
		/// Modify the cards reward options before a card reward is generated from them.
		/// You may remove cards, add cards, or add duplicate cards (to increase the chances of that card showing up) to
		/// the card pool.
		/// </summary>
		/// <param name="player">The player who is being offered rewards.</param>
		/// <param name="options">What the card rewards are being generated for.</param>
		// Token: 0x060049F4 RID: 18932 RVA: 0x002266B1 File Offset: 0x002248B1
		public virtual CardCreationOptions ModifyCardRewardCreationOptionsLate(Player player, CardCreationOptions options)
		{
			return options;
		}

		/// <summary>
		/// Modify the odds of a card reward being offered upgraded.
		/// </summary>
		/// <param name="player">Player who is being offered rewards.</param>
		/// <param name="card">Card reward.</param>
		/// <param name="odds">Initial odds of it being upgraded.</param>
		/// <returns>New odds of it being upgraded.</returns>
		// Token: 0x060049F5 RID: 18933 RVA: 0x002266B4 File Offset: 0x002248B4
		public virtual decimal ModifyCardRewardUpgradeOdds(Player player, CardModel card, decimal odds)
		{
			return odds;
		}

		/// <summary>
		/// Add to the amount of damage that will be dealt.
		/// Use this for effects that add amounts to existing damage, like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.StrengthPower" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Powers.VigorPower" />.
		/// </summary>
		/// <param name="target">Creature who will receive the damage.</param>
		/// <param name="amount">Current amount of damage that will be dealt.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who will deal the damage.</param>
		/// <param name="cardSource">Card that will be dealing the damage.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>Amount of damage to be added.</returns>
		// Token: 0x060049F6 RID: 18934 RVA: 0x002266B7 File Offset: 0x002248B7
		[NullableContext(2)]
		public virtual decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			return 0m;
		}

		/// <summary>
		/// Set the maximum amount of damage that will be dealt in a single hit.
		/// </summary>
		/// <param name="target">Creature who will receive the damage.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who will deal the damage.</param>
		/// <param name="cardSource">Card that will be dealing the damage.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns></returns>
		// Token: 0x060049F7 RID: 18935 RVA: 0x002266BE File Offset: 0x002248BE
		[NullableContext(2)]
		public virtual decimal ModifyDamageCap(Creature target, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			return decimal.MaxValue;
		}

		/// <summary>
		/// Multiply the amount of damage that will be dealt.
		/// Use this for effects that multiply existing damage by some amount, like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.VulnerablePower" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Powers.WeakPower" />.
		/// </summary>
		/// <param name="target">Creature who will receive the damage.</param>
		/// <param name="amount">Current amount of damage that will be dealt.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <param name="dealer">Creature who will deal the damage.</param>
		/// <param name="cardSource">Card that will be dealing the damage.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>Amount that the damage should be multiplied by.</returns>
		// Token: 0x060049F8 RID: 18936 RVA: 0x002266CA File Offset: 0x002248CA
		[NullableContext(2)]
		public virtual decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			return 1m;
		}

		/// <summary>
		/// When the player is about to gain energy, modify the amount that they should gain.
		/// </summary>
		/// <param name="player">Player who will gain energy.</param>
		/// <param name="amount">Original amount of energy they would gain.</param>
		/// <returns>New amount of energy to gain.</returns>
		// Token: 0x060049F9 RID: 18937 RVA: 0x002266D1 File Offset: 0x002248D1
		public virtual decimal ModifyEnergyGain(Player player, decimal amount)
		{
			return amount;
		}

		/// <summary>
		/// When the player is about to gain gold, modify the amount that they should gain.
		/// </summary>
		/// <param name="player">Player who will gain gold.</param>
		/// <param name="amount">Original amount of gold they would gain.</param>
		/// <returns>New amount of gold to gain.</returns>
		// Token: 0x060049FA RID: 18938 RVA: 0x002266D4 File Offset: 0x002248D4
		public virtual decimal ModifyGoldGained(Player player, decimal amount)
		{
			return amount;
		}

		/// <summary>
		/// Modify the dungeon map before it is set up.
		/// </summary>
		/// <param name="runState">Run state.</param>
		/// <param name="map">The generated map.</param>
		/// <param name="actIndex">The act index for which the map was generated.</param>
		// Token: 0x060049FB RID: 18939 RVA: 0x002266D7 File Offset: 0x002248D7
		public virtual ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
		{
			return map;
		}

		/// <summary>
		/// Annotate the dungeon map after all topology-replacing hooks have run.
		/// Must not replace the map (i.e., return a different ActMap instance) because this hook also
		/// runs on saved maps during multiplayer reconnect, where the topology must be preserved.
		/// Use this for quest markers, metadata, etc. To replace the map, use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyGeneratedMap(MegaCrit.Sts2.Core.Runs.IRunState,MegaCrit.Sts2.Core.Map.ActMap,System.Int32)" />.
		/// </summary>
		/// <param name="runState">Run state.</param>
		/// <param name="map">The generated map.</param>
		/// <param name="actIndex">The act index for which the map was generated.</param>
		// Token: 0x060049FC RID: 18940 RVA: 0x002266DA File Offset: 0x002248DA
		public virtual ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
		{
			return map;
		}

		/// <summary>
		/// Modify the amount of cards the player should draw at the start of their turn.
		/// </summary>
		/// <param name="player">Player who will draw the cards.</param>
		/// <param name="count">Original number of cards that would be drawn.</param>
		/// <returns>New amount of cards to be drawn.</returns>
		// Token: 0x060049FD RID: 18941 RVA: 0x002266DD File Offset: 0x002248DD
		public virtual decimal ModifyHandDraw(Player player, decimal count)
		{
			return count;
		}

		/// <summary>
		/// CAREFUL! You should usually use ModifyHandDraw instead of this.
		/// Modify the amount of cards the player should draw at the start of their turn.
		/// </summary>
		/// <param name="player">Player who will draw the cards.</param>
		/// <param name="count">Original number of cards that would be drawn.</param>
		/// <returns>New amount of cards to be drawn.</returns>
		// Token: 0x060049FE RID: 18942 RVA: 0x002266E0 File Offset: 0x002248E0
		public virtual decimal ModifyHandDrawLate(Player player, decimal count)
		{
			return count;
		}

		/// <summary>
		/// Modify the amount of HP that will be lost by the target.
		/// This runs when damage is being dealt, after block is applied.
		/// This runs BEFORE damage is redirected from Necrobinder to Osty. If you want to modify damage AFTER this
		/// redirection, use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostAfterOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// Note: Only <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DieForYouPower" /> causes damage redirection. If we ever get a second source of damage
		/// redirection, we should rename this method to be more general.
		/// </summary>
		/// <param name="target">Creature who will lose HP.</param>
		/// <param name="amount">Original amount of HP to be lost.</param>
		/// <param name="props">ValueProp for amount.</param>
		/// <param name="dealer">Creature who will be causing the HP loss.</param>
		/// <param name="cardSource">Card that will be causing the HP loss.</param>
		/// <returns>New amount of HP to be lost.</returns>
		// Token: 0x060049FF RID: 18943 RVA: 0x002266E3 File Offset: 0x002248E3
		[NullableContext(2)]
		public virtual decimal ModifyHpLostBeforeOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			return amount;
		}

		/// <summary>
		/// CAREFUL! You should usually use ModifyHpLostBeforeOsty instead of this.
		/// Modify the amount of HP that will be lost by the target.
		/// This runs when damage is being dealt, after block is applied.
		/// This runs BEFORE damage is redirected from Necrobinder to Osty. If you want to modify damage AFTER this
		/// redirection, use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostAfterOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// Note: Only <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DieForYouPower" /> causes damage redirection. If we ever get a second source of damage
		/// redirection, we should rename this method to be more general.
		/// </summary>
		/// <param name="target">Creature who will lose HP.</param>
		/// <param name="amount">Original amount of HP to be lost.</param>
		/// <param name="props">ValueProp for amount.</param>
		/// <param name="dealer">Creature who will be causing the HP loss.</param>
		/// <param name="cardSource">Card that will be causing the HP loss.</param>
		/// <returns>New amount of HP to be lost.</returns>
		// Token: 0x06004A00 RID: 18944 RVA: 0x002266E6 File Offset: 0x002248E6
		[NullableContext(2)]
		public virtual decimal ModifyHpLostBeforeOstyLate([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			return amount;
		}

		/// <summary>
		/// Modify the amount of HP that will be lost by the target.
		/// This runs when damage is being dealt, after block is applied.
		/// This runs AFTER damage is redirected from Necrobinder to Osty. If you want to modify damage BEFORE this
		/// redirection, use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostBeforeOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// Note: Only <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DieForYouPower" /> causes damage redirection. If we ever get a second source of damage
		/// redirection, we should rename this method to be more general.
		/// </summary>
		/// <param name="target">Creature who will lose HP.</param>
		/// <param name="amount">Original amount of HP to be lost.</param>
		/// <param name="props">ValueProp for amount.</param>
		/// <param name="dealer">Creature who will be causing the HP loss.</param>
		/// <param name="cardSource">Card that will be causing the HP loss.</param>
		/// <returns>New amount of HP to be lost.</returns>
		// Token: 0x06004A01 RID: 18945 RVA: 0x002266E9 File Offset: 0x002248E9
		[NullableContext(2)]
		public virtual decimal ModifyHpLostAfterOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			return amount;
		}

		/// <summary>
		/// CAREFUL! You should usually use ModifyHpLostAfterOsty instead of this.
		/// Modify the amount of HP that will be lost by the target.
		/// This runs when damage is being dealt, after block is applied.
		/// This runs AFTER damage is redirected from Necrobinder to Osty. If you want to modify damage BEFORE this
		/// redirection, use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyHpLostBeforeOsty(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// Note: Only <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DieForYouPower" /> causes damage redirection. If we ever get a second source of damage
		/// redirection, we should rename this method to be more general.
		/// </summary>
		/// <param name="target">Creature who will lose HP.</param>
		/// <param name="amount">Original amount of HP to be lost.</param>
		/// <param name="props">ValueProp for amount.</param>
		/// <param name="dealer">Creature who will be causing the HP loss.</param>
		/// <param name="cardSource">Card that will be causing the HP loss.</param>
		/// <returns>New amount of HP to be lost.</returns>
		// Token: 0x06004A02 RID: 18946 RVA: 0x002266EC File Offset: 0x002248EC
		[NullableContext(2)]
		public virtual decimal ModifyHpLostAfterOstyLate([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			return amount;
		}

		/// <summary>
		/// Modify the max energy a player has.
		/// </summary>
		/// <param name="player">Player who we are modifying.</param>
		/// <param name="amount">Current amount of max energy they have.</param>
		// Token: 0x06004A03 RID: 18947 RVA: 0x002266EF File Offset: 0x002248EF
		public virtual decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			return amount;
		}

		/// <summary>
		/// Modify the cards available in the merchant's card pool.
		/// You may remove cards, add cards, or add duplicate cards (to increase the chances of that card showing up).
		/// </summary>
		/// <param name="player">The player who is at the merchant.</param>
		/// <param name="options">The original options in the card pool.</param>
		/// <returns>The modified card pool.</returns>
		// Token: 0x06004A04 RID: 18948 RVA: 0x002266F2 File Offset: 0x002248F2
		public virtual IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
		{
			return options;
		}

		/// <summary>
		/// Modify the rarity of a merchant card entry
		/// </summary>
		/// <param name="player">The player who is at the merchant.</param>
		/// <param name="rarity">Original rarity of the merchant card entry.</param>
		/// <returns>New rarity.</returns>
		// Token: 0x06004A05 RID: 18949 RVA: 0x002266F5 File Offset: 0x002248F5
		public virtual CardRarity ModifyMerchantCardRarity(Player player, CardRarity rarity)
		{
			return rarity;
		}

		/// <summary>
		/// Modify the cards created to sell at the merchant.
		/// </summary>
		/// <param name="player">Player who is at the merchant</param>
		/// <param name="cards">
		///     CardCreationResults created to sell at the merchant.
		///     Note that they could already have been modified by another source.
		/// </param>
		// Token: 0x06004A06 RID: 18950 RVA: 0x002266F8 File Offset: 0x002248F8
		public virtual void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
		{
		}

		/// <summary>
		/// Modify the cost of items at the merchant.
		/// </summary>
		/// <param name="player">The player who is at the merchant.</param>
		/// <param name="entry">the merchant entry we are modifying the price for</param>
		/// <param name="cost">Original cost of the merchant item.</param>
		/// <returns>New cost of the merchant item.</returns>
		// Token: 0x06004A07 RID: 18951 RVA: 0x002266FA File Offset: 0x002248FA
		public virtual decimal ModifyMerchantPrice(Player player, MerchantEntry entry, decimal cost)
		{
			return cost;
		}

		/// <summary>
		/// Modify orb value for this creature.
		/// </summary>
		/// <param name="orb">Orb we are modifying.</param>
		/// <param name="value">Original value that the orb would use for its action.</param>
		/// <returns>New value that the orb should use for its action.</returns>
		// Token: 0x06004A08 RID: 18952 RVA: 0x002266FD File Offset: 0x002248FD
		public virtual decimal ModifyOrbValue(OrbModel orb, decimal value)
		{
			return value;
		}

		/// <summary>
		/// Add to the amount of a power that will be applied by a creature.
		/// Use this for effects that add a flat amount to the power, like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.SneckoSkull" />.
		/// This runs before <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyPowerAmountGivenMultiplicative(MegaCrit.Sts2.Core.Models.PowerModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		/// <param name="power">The power that will be applied.</param>
		/// <param name="giver">Creature who will be applying the power.</param>
		/// <param name="amount">Current amount of the power to be applied.</param>
		/// <param name="target">Optional creature who the power will be applied to.</param>
		/// <param name="cardSource">Optional card that applied the power.</param>
		/// <returns>Amount to add to the power.</returns>
		// Token: 0x06004A09 RID: 18953 RVA: 0x00226700 File Offset: 0x00224900
		public virtual decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, [Nullable(2)] Creature target, [Nullable(2)] CardModel cardSource)
		{
			return 0m;
		}

		/// <summary>
		/// Multiply the amount of a power that will be applied by a creature.
		/// Use this for effects that scale the power, like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.UnsettlingLamp" />.
		/// This runs after <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyPowerAmountGivenAdditive(MegaCrit.Sts2.Core.Models.PowerModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		/// <param name="power">The power that will be applied.</param>
		/// <param name="giver">Creature who will be applying the power.</param>
		/// <param name="amount">Current amount of the power to be applied (after additive modifiers).</param>
		/// <param name="target">Optional creature who the power will be applied to.</param>
		/// <param name="cardSource">Optional card that applied the power.</param>
		/// <returns>Amount that the power should be multiplied by.</returns>
		// Token: 0x06004A0A RID: 18954 RVA: 0x00226707 File Offset: 0x00224907
		public virtual decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, [Nullable(2)] Creature target, [Nullable(2)] CardModel cardSource)
		{
			return 1m;
		}

		/// <summary>
		/// Modify the amount to be healed at the rest site.
		/// </summary>
		/// <param name="creature">Creature who will be healing (usually the player, but Osty when playing Necrobinder).</param>
		/// <param name="amount">The original amount that would be healed.</param>
		/// <returns>The new amount to be healed.</returns>
		// Token: 0x06004A0B RID: 18955 RVA: 0x0022670E File Offset: 0x0022490E
		public virtual decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
		{
			return amount;
		}

		/// <summary>
		/// Modify the order in which cards are shuffled into the player's draw pile.
		/// </summary>
		/// <param name="player">Player whose draw pile is being shuffled.</param>
		/// <param name="cards">
		/// Cards that are being shuffled. Modifications made to this list will be reflected in the draw pile.
		/// </param>
		/// <param name="isInitialShuffle">
		/// True if this is the initial shuffle at the start of combat, false if this is a reshuffle during combat.
		/// </param>
		// Token: 0x06004A0C RID: 18956 RVA: 0x00226711 File Offset: 0x00224911
		public virtual void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
		{
		}

		/// <summary>
		/// Modify the amount of a Summon.
		/// </summary>
		/// <param name="summoner">Player who is doing the summoning.</param>
		/// <param name="amount">Amount that is being summoned.</param>
		/// <param name="source">
		/// The model that this Summon came from. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Bodyguard" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Relics.BoundPhylactery" />
		/// pass themselves here.
		/// Null if the Summon did not come from any model (generally only relevant in tests).
		/// </param>
		/// <returns>The new amount to be summoned.</returns>
		// Token: 0x06004A0D RID: 18957 RVA: 0x00226713 File Offset: 0x00224913
		public virtual decimal ModifySummonAmount(Player summoner, decimal amount, [Nullable(2)] AbstractModel source)
		{
			return amount;
		}

		/// <summary>
		/// Modify the creature that will receive unblocked damage.
		/// </summary>
		/// <param name="target">Creature who will receive the damage.</param>
		/// <param name="amount">Amount of damage to be dealt.</param>
		/// <param name="props">ValueProp for amount.</param>
		/// <param name="dealer">Creature who will be dealing the damage.</param>
		/// <returns>New creature to receive the unblocked damage.</returns>
		// Token: 0x06004A0E RID: 18958 RVA: 0x00226716 File Offset: 0x00224916
		public virtual Creature ModifyUnblockedDamageTarget(Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer)
		{
			return target;
		}

		/// <summary>
		/// Modify the current event for an event room
		/// </summary>
		/// <param name="currentEvent">The currently rolled event.</param>
		/// <returns>New event</returns>
		// Token: 0x06004A0F RID: 18959 RVA: 0x00226719 File Offset: 0x00224919
		public virtual EventModel ModifyNextEvent(EventModel currentEvent)
		{
			return currentEvent;
		}

		/// <summary>
		/// Modify the room types that can be rolled when the player enters an unknown location.
		/// </summary>
		/// <param name="roomTypes">Room types that can be rolled.</param>
		/// <returns>New set of room types that can be rolled.</returns>
		// Token: 0x06004A10 RID: 18960 RVA: 0x0022671C File Offset: 0x0022491C
		public virtual IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(IReadOnlySet<RoomType> roomTypes)
		{
			return roomTypes;
		}

		/// <summary>
		/// When a room type is not chosen as the resolved room for an unknown map point, this modifies the increased odds
		/// that it will show up the next time.
		/// </summary>
		/// <param name="roomType">The room type whose odds are being increased.</param>
		/// <param name="oddsIncrease">The amount the room would increase odds without modification.</param>
		/// <returns>The modified odds increase.</returns>
		// Token: 0x06004A11 RID: 18961 RVA: 0x0022671F File Offset: 0x0022491F
		public virtual float ModifyOddsIncreaseForUnrolledRoomType(RoomType roomType, float oddsIncrease)
		{
			return oddsIncrease;
		}

		/// <summary>
		/// Modify the X-value used for an X-cost card's effect.
		/// </summary>
		/// <param name="card">Card whose X-value we're modifying</param>
		/// <param name="originalValue">Original X-value that we're modifying.</param>
		/// <returns>The new X-value to use.</returns>
		// Token: 0x06004A12 RID: 18962 RVA: 0x00226722 File Offset: 0x00224922
		public virtual int ModifyXValue(CardModel card, int originalValue)
		{
			return originalValue;
		}

		/// <summary>
		/// Runs before a card is added to deck to give relics an opportunity to modify the card.
		/// Note that this can be run as part of transformation as well.
		/// The new owner can be determined by checking card.Owner.
		/// </summary>
		/// <param name="card">Card that was put into a deck.</param>
		/// <param name="newCard">New card that should be put into the deck instead.</param>
		// Token: 0x06004A13 RID: 18963 RVA: 0x00226725 File Offset: 0x00224925
		public virtual bool TryModifyCardBeingAddedToDeck(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			return false;
		}

		/// <summary>
		/// NOTE: You probably want to use TryModifyCardBeingAddedToDeck instead!
		/// Runs before a card is added to deck to give relics an opportunity to modify the card.
		/// Note that this can be run as part of transformation as well.
		/// The new owner can be determined by checking card.Owner.
		/// </summary>
		/// <param name="card">Card that was put into a deck.</param>
		/// <param name="newCard">New card that should be put into the deck instead.</param>
		// Token: 0x06004A14 RID: 18964 RVA: 0x0022672B File Offset: 0x0022492B
		public virtual bool TryModifyCardBeingAddedToDeckLate(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			return false;
		}

		/// <summary>
		/// Modify the alternative options (options other than picking a card offered in a card reward).
		/// Ex: Skip, Heal +2 (Dream Catcher), Sacrifice (Pael Wing).
		/// </summary>
		/// <param name="player">Player to whom we are presenting the options.</param>
		/// <param name="cardReward">The reward that is being displayed.</param>
		/// <param name="alternatives">Card reward alternatives.</param>
		/// <returns>Whether the alternatives were modified.</returns>
		// Token: 0x06004A15 RID: 18965 RVA: 0x00226731 File Offset: 0x00224931
		public virtual bool TryModifyCardRewardAlternatives(Player player, CardReward cardReward, List<CardRewardAlternative> alternatives)
		{
			return false;
		}

		/// <summary>
		/// Modifies the options for a card reward.
		/// </summary>
		/// <param name="player">The player receiving the rewards.</param>
		/// <param name="cardRewardOptions">Options in the card reward.</param>
		/// <param name="creationOptions">How the original cards were created.</param>
		// Token: 0x06004A16 RID: 18966 RVA: 0x00226734 File Offset: 0x00224934
		public virtual bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
		{
			return false;
		}

		/// <summary>
		/// CAREFUL! You should usually use TryModifyCardRewardOptions instead of this.
		/// Modifies the options for a card reward.
		/// </summary>
		/// <param name="player">The player receiving the rewards.</param>
		/// <param name="cardRewardOptions">Options in the card reward.</param>
		/// <param name="creationOptions">How the original cards were created.</param>
		// Token: 0x06004A17 RID: 18967 RVA: 0x00226737 File Offset: 0x00224937
		public virtual bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
		{
			return false;
		}

		/// <summary>
		/// Modify the amount of energy that a card will cost during combat.
		/// This will never be called while outside of combat.
		/// </summary>
		/// <param name="card">Card whose energy cost we're modifying.</param>
		/// <param name="originalCost">Original energy cost.</param>
		/// <param name="modifiedCost">Modified energy cost.</param>
		/// <returns>Whether the energy cost was modified</returns>
		// Token: 0x06004A18 RID: 18968 RVA: 0x0022673A File Offset: 0x0022493A
		public virtual bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			return false;
		}

		/// <summary>
		/// CAREFUL! You should usually use TryModifyEnergyCostInCombat instead of this.
		/// Modify the amount of energy that a card will cost during combat.
		/// This will never be called while outside of combat.
		/// </summary>
		/// <param name="card">Card whose energy cost we're modifying.</param>
		/// <param name="originalCost">Original energy cost.</param>
		/// <param name="modifiedCost">Modified energy cost.</param>
		/// <returns>Whether the energy cost was modified</returns>
		// Token: 0x06004A19 RID: 18969 RVA: 0x00226744 File Offset: 0x00224944
		public virtual bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			return false;
		}

		/// <summary>
		/// Add or remove keywords on a card during combat. This is the global counterpart to the local keywords stored on
		/// the card itself (see <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.KeywordSources" />), and works just like <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.TryModifyEnergyCostInCombat(MegaCrit.Sts2.Core.Models.CardModel,System.Decimal,System.Decimal@)" />
		/// does for energy cost: a model contributes keywords for as long as it exists, with no explicit cleanup needed when
		/// it's removed.
		/// This will never be called while outside of combat.
		/// Mutate <paramref name="keywords" /> in place (add or remove).
		/// </summary>
		/// <param name="card">Card whose keywords we're modifying.</param>
		/// <param name="keywords">The card's current keyword set, to be mutated in place.</param>
		/// <returns>Whether the keyword set was modified.</returns>
		// Token: 0x06004A1A RID: 18970 RVA: 0x0022674E File Offset: 0x0022494E
		public virtual bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
		{
			return false;
		}

		/// <summary>
		/// Modify the amount of star that a card will cost.
		/// </summary>
		/// <param name="card">Card whose star cost we're modifying.</param>
		/// <param name="originalCost">Original star cost.</param>
		/// <param name="modifiedCost">Modified star cost.</param>
		/// <returns>Whether the energy cost was modified</returns>
		// Token: 0x06004A1B RID: 18971 RVA: 0x00226751 File Offset: 0x00224951
		public virtual bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			return false;
		}

		/// <summary>
		/// Modify the amount of a power that will be applied to the target.
		/// </summary>
		/// <param name="canonicalPower">Canonical power that will be applied.</param>
		/// <param name="target">Creature who the power will be applied to.</param>
		/// <param name="amount">Original amount of the power to be applied.</param>
		/// <param name="applier">Optional creature who will be applying the power.</param>
		/// <param name="modifiedAmount">New amount of the power to be applied.</param>
		/// <returns>Whether the amount was modified.</returns>
		// Token: 0x06004A1C RID: 18972 RVA: 0x0022675B File Offset: 0x0022495B
		public virtual bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, [Nullable(2)] Creature applier, out decimal modifiedAmount)
		{
			modifiedAmount = amount;
			return false;
		}

		/// <summary>
		/// Modify the options offered to the player when they enter a rest site.
		/// </summary>
		/// <param name="player">Player who owns the rest site options</param>
		/// <param name="options">Rest site options</param>
		/// <returns>Whether the options were modified.</returns>
		// Token: 0x06004A1D RID: 18973 RVA: 0x00226766 File Offset: 0x00224966
		public virtual bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			return false;
		}

		/// <summary>
		/// Modify the rewards received when resting at the rest site. Normally this is empty.
		/// </summary>
		/// <param name="player">The player being offered rewards.</param>
		/// <param name="rewards">The rewards offered to the player.</param>
		/// <param name="isMimicked">
		/// Is a mimicked rest site heal?
		/// Used in spots that mimicking a rest site heal, but are not actual rest site heals.
		/// For example: <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" /> mimicks a rest site heal when you select certain options.
		/// This should count as a rest site heal for <see cref="T:MegaCrit.Sts2.Core.Models.Relics.RegalPillow" />, but not for <see cref="T:MegaCrit.Sts2.Core.Models.Modifiers.NightTerrors" />.
		/// </param>
		/// <returns>Whether the rewards were modified.</returns>
		// Token: 0x06004A1E RID: 18974 RVA: 0x00226769 File Offset: 0x00224969
		public virtual bool TryModifyRestSiteHealRewards(Player player, List<Reward> rewards, bool isMimicked)
		{
			return false;
		}

		/// <summary>
		/// Modify the rewards offered to the player.
		/// Remember that rewards can be generated in several ways:
		/// - As part of the end of an encounter
		/// - By relics like Orrery
		/// - At a rest site by Tiny Mailbox or Dreamcatcher
		/// - By an event like The Future of Potions
		/// </summary>
		/// <param name="player">The player being offered rewards.</param>
		/// <param name="rewards">The rewards offered to the player.</param>
		/// <param name="room">The room which the rewards are generated for. If this is null, then the rewards were generated
		/// as part of something that wasn't room completion (e.g. an event choice or a relic pickup).</param>
		/// <returns>Whether the rewards were modified.</returns>
		// Token: 0x06004A1F RID: 18975 RVA: 0x0022676C File Offset: 0x0022496C
		public virtual bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			return false;
		}

		/// <summary>
		/// CAREFUL! You should usually use TryModifyRewards instead of this.
		/// Modify the rewards offered to the player after completing a particular room/encounter.
		/// Works just like TryModifyRewards, but runs after it.
		/// </summary>
		/// <param name="player">The player being offered rewards.</param>
		/// <param name="rewards">Rewards</param>
		/// <param name="room">The room which the rewards are generated for. If this is null, then the rewards were generated
		/// as part of something that wasn't room completion (e.g. an event choice or a relic pickup).</param>
		/// <returns>Whether the rewards were modified.</returns>
		// Token: 0x06004A20 RID: 18976 RVA: 0x0022676F File Offset: 0x0022496F
		public virtual bool TryModifyRewardsLate(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			return false;
		}

		/// <summary>
		/// Modify the extra text that will be displayed when the player heals at the rest site.
		/// Most implementors use this to add extra lines of text.
		/// </summary>
		/// <param name="player">The player who is at the rest site.</param>
		/// <param name="currentExtraText">The current extra text that will be displayed.</param>
		/// <returns>The new extra text that will be displayed.</returns>
		// Token: 0x06004A21 RID: 18977 RVA: 0x00226772 File Offset: 0x00224972
		public virtual IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
		{
			return currentExtraText;
		}

		/// <summary>
		/// If a card would be added to the player's deck, should it still be?
		/// </summary>
		/// <param name="card">Card to be added.</param>
		/// <returns>Whether to allow it.</returns>
		// Token: 0x06004A22 RID: 18978 RVA: 0x00226775 File Offset: 0x00224975
		public virtual bool ShouldAddToDeck(CardModel card)
		{
			return true;
		}

		/// <summary>
		/// Should we allow the specified affliction to be added to this card?
		/// </summary>
		/// <param name="card">Card to be afflicted.</param>
		/// <param name="affliction">Affliction that would be added to the card.</param>
		/// <returns>Whether to allow the affliction.</returns>
		// Token: 0x06004A23 RID: 18979 RVA: 0x00226778 File Offset: 0x00224978
		public virtual bool ShouldAfflict(CardModel card, AfflictionModel affliction)
		{
			return true;
		}

		/// <summary>
		/// Should we allow the player to speak to this Ancient?
		/// Used by things like the Wax Choker relic.
		/// </summary>
		/// <param name="player">Player speaking to the Ancient.</param>
		/// <param name="ancient">Ancient the player is trying to speak to.</param>
		/// <returns>Whether to allow it.</returns>
		// Token: 0x06004A24 RID: 18980 RVA: 0x0022677B File Offset: 0x0022497B
		public virtual bool ShouldAllowAncient(Player player, AncientEventModel ancient)
		{
			return true;
		}

		/// <summary>
		/// If an effect would normally hit a creature, should it still?
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Creatures.Creature.IsHittable" /> for how hitting differs from targeting.
		/// </summary>
		/// <param name="creature">Creature who we want to hit.</param>
		/// <returns></returns>
		// Token: 0x06004A25 RID: 18981 RVA: 0x0022677E File Offset: 0x0022497E
		public virtual bool ShouldAllowHitting(Creature creature)
		{
			return true;
		}

		/// <summary>
		/// If a player tries to target a creature, should they be allowed to?
		/// </summary>
		/// <param name="target">Creature who we want to target.</param>
		/// <returns>Whether to allow player to target creature.</returns>
		// Token: 0x06004A26 RID: 18982 RVA: 0x00226781 File Offset: 0x00224981
		public virtual bool ShouldAllowTargeting(Creature target)
		{
			return true;
		}

		/// <summary>
		/// Called after a reward is selected at the rewards screen to determine whether or not we should close the rewards screen.
		/// </summary>
		/// <param name="player">Player to whom we are presenting the options.</param>
		/// <param name="cardReward">The reward that is being displayed.</param>
		/// <returns>True to allow the player to continue to select rewards, false otherwise.</returns>
		// Token: 0x06004A27 RID: 18983 RVA: 0x00226784 File Offset: 0x00224984
		public virtual bool ShouldAllowSelectingMoreCardRewards(Player player, CardReward cardReward)
		{
			return false;
		}

		/// <summary>
		/// If a creature's block is about to be cleared, should it still be cleared?
		/// </summary>
		/// <param name="creature">Creature whose block is about to be cleared.</param>
		/// <returns>Whether the creature's block should still be cleared.</returns>
		// Token: 0x06004A28 RID: 18984 RVA: 0x00226787 File Offset: 0x00224987
		public virtual bool ShouldClearBlock(Creature creature)
		{
			return true;
		}

		/// <summary>
		/// If a creature is about to die, should they still?
		/// </summary>
		/// <param name="creature">Creature who is about to die.</param>
		/// <returns>Whether the creature should still die.</returns>
		// Token: 0x06004A29 RID: 18985 RVA: 0x0022678A File Offset: 0x0022498A
		public virtual bool ShouldDie(Creature creature)
		{
			return true;
		}

		/// <summary>
		/// CAREFUL! You should usually use ShouldDie instead of this.
		/// If a creature is about to die, should they still?
		/// Works just like ShouldDie, but runs after it.
		/// </summary>
		/// <param name="creature">Creature who is about to die.</param>
		/// <returns>Whether the creature should still die.</returns>
		// Token: 0x06004A2A RID: 18986 RVA: 0x0022678D File Offset: 0x0022498D
		public virtual bool ShouldDieLate(Creature creature)
		{
			return true;
		}

		/// <summary>
		/// After the player selects a Rest Site option, should we still disable the rest of them?
		/// </summary>
		/// <returns>Whether to disable remaining Rest Site options.</returns>
		// Token: 0x06004A2B RID: 18987 RVA: 0x00226790 File Offset: 0x00224990
		public virtual bool ShouldDisableRemainingRestSiteOptions(Player player)
		{
			return true;
		}

		/// <summary>
		/// If a player is about to draw cards, should they still?
		/// </summary>
		/// <param name="player">Player is about to draw cards.</param>
		/// <param name="fromHandDraw">draw is part of the initial hand draw at the start of your turn.</param>
		/// <returns>Whether the player should still draw cards.</returns>
		// Token: 0x06004A2C RID: 18988 RVA: 0x00226793 File Offset: 0x00224993
		public virtual bool ShouldDraw(Player player, bool fromHandDraw)
		{
			return true;
		}

		/// <summary>
		/// If Ethereal is about to trigger for a card, should it still?
		/// </summary>
		/// <param name="card">The card that is about to be Exhausted due to Ethereal.</param>
		/// <returns>Whether the card should still be Exhausted.</returns>
		// Token: 0x06004A2D RID: 18989 RVA: 0x00226796 File Offset: 0x00224996
		public virtual bool ShouldEtherealTrigger(CardModel card)
		{
			return true;
		}

		/// <summary>
		/// If a player is about to flush their hand, should they still?
		/// </summary>
		/// <param name="player">Player who is about to flush their hand.</param>
		/// <returns>Whether the player should still flush their hand.</returns>
		// Token: 0x06004A2E RID: 18990 RVA: 0x00226799 File Offset: 0x00224999
		public virtual bool ShouldFlush(Player player)
		{
			return true;
		}

		/// <summary>
		/// If a player is about to gain stars, should they still?
		/// </summary>
		/// <param name="amount">Amount of stars they would gain.</param>
		/// <param name="player">Player who is about to gain stars.</param>
		/// <returns>Whether the player should still gain stars.</returns>
		// Token: 0x06004A2F RID: 18991 RVA: 0x0022679C File Offset: 0x0022499C
		public virtual bool ShouldGainStars(decimal amount, Player player)
		{
			return true;
		}

		/// <summary>
		/// If a player is about to receive treasure from a treasure room, should they?
		/// </summary>
		/// <param name="player">Player who is about to receive the treasure.</param>
		/// <returns>Whether the player should receive the treasure.</returns>
		// Token: 0x06004A30 RID: 18992 RVA: 0x0022679F File Offset: 0x0022499F
		public virtual bool ShouldGenerateTreasure(Player player)
		{
			return true;
		}

		/// <summary>
		/// If a player is about to pay an energy cost that they don't have enough energy for, should they be allowed to
		/// spend stars to pay for the excess energy cost?
		/// </summary>
		/// <param name="player">Player who is about to spend energy.</param>
		/// <returns>Whether the player can spend stars to pay for the excess energy cost.</returns>
		// Token: 0x06004A31 RID: 18993 RVA: 0x002267A2 File Offset: 0x002249A2
		public virtual bool ShouldPayExcessEnergyCostWithStars(Player player)
		{
			return false;
		}

		/// <summary>
		/// If a card is about to be played, should it still?
		/// </summary>
		/// <param name="card">Card that is about to be played</param>
		/// <param name="autoPlayType">The type of autoplay this card play is (ie played by echo form). None if its not an auto play.</param>
		/// <returns>Whether the card should still be played.</returns>
		// Token: 0x06004A32 RID: 18994 RVA: 0x002267A5 File Offset: 0x002249A5
		public virtual bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
		{
			return true;
		}

		/// <summary>
		/// If the player's energy is about to be reset, should it be reset?
		/// If not, the player's max energy is still added on to the current amount.
		/// </summary>
		/// <returns>Whether the player's energy should be reset.</returns>
		// Token: 0x06004A33 RID: 18995 RVA: 0x002267A8 File Offset: 0x002249A8
		public virtual bool ShouldPlayerResetEnergy(Player player)
		{
			return true;
		}

		/// <summary>
		/// If the player is attempting to proceed to the next point on the map, should they be allowed to?
		/// </summary>
		/// <returns>Whether to allow it.</returns>
		// Token: 0x06004A34 RID: 18996 RVA: 0x002267AB File Offset: 0x002249AB
		public virtual bool ShouldProceedToNextMapPoint()
		{
			return true;
		}

		/// <summary>
		/// If the player would procure the specified potion, should they still?
		/// </summary>
		/// <param name="potion">Potion we are procuring.</param>
		/// <param name="player">Player trying to procure the potion.</param>
		/// <returns>Whether to allow it.</returns>
		// Token: 0x06004A35 RID: 18997 RVA: 0x002267AE File Offset: 0x002249AE
		public virtual bool ShouldProcurePotion(PotionModel potion, Player player)
		{
			return true;
		}

		/// <summary>
		/// If the power should be removed after the creature has died. For things like Calcify
		/// which can prevent this.
		/// </summary>
		/// <returns>Whether the power should be removed on death</returns>
		// Token: 0x06004A36 RID: 18998 RVA: 0x002267B1 File Offset: 0x002249B1
		public virtual bool ShouldPowerBeRemovedOnDeath(PowerModel power)
		{
			return true;
		}

		/// <summary>
		/// If the player has just purchased something from the merchant, should the now-empty entry be refilled?
		/// </summary>
		/// <param name="entry">Entry that might be refilled.</param>
		/// <param name="player">Player who is at the merchant.</param>
		/// <returns>Whether the merchant entry should be refilled.</returns>
		// Token: 0x06004A37 RID: 18999 RVA: 0x002267B4 File Offset: 0x002249B4
		public virtual bool ShouldRefillMerchantEntry(MerchantEntry entry, Player player)
		{
			return false;
		}

		/// <summary>
		/// At the merchant, should the player be allowed to remove cards?
		/// </summary>
		/// <param name="player">Player who is at the merchant.</param>
		/// <returns>True if the player should be allowed to remove cards, false otherwise.</returns>
		// Token: 0x06004A38 RID: 19000 RVA: 0x002267B7 File Offset: 0x002249B7
		public virtual bool ShouldAllowMerchantCardRemoval(Player player)
		{
			return true;
		}

		/// <summary>
		/// If this creature just died, should it be removed from combat?
		/// Usually true, but things like reviving powers should return false so they can revive the creature later.
		/// </summary>
		/// <param name="creature">Creature that died.</param>
		/// <returns>Whether the creature should be removed from combat.</returns>
		// Token: 0x06004A39 RID: 19001 RVA: 0x002267BA File Offset: 0x002249BA
		public virtual bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return true;
		}

		/// <summary>
		/// If all enemies are dead, should we stop combat from ending?
		/// This should be set to true when all enemies are dead but new enemies are about to spawn.
		/// </summary>
		/// <returns>Whether combat should be marked as about to end</returns>
		// Token: 0x06004A3A RID: 19002 RVA: 0x002267BD File Offset: 0x002249BD
		public virtual bool ShouldStopCombatFromEnding()
		{
			return false;
		}

		/// <summary>
		/// If the player should be allowed to take an extra turn.
		/// </summary>
		/// <param name="player">Player ending turn.</param>
		/// <returns>Whether to allow it.</returns>
		// Token: 0x06004A3B RID: 19003 RVA: 0x002267C0 File Offset: 0x002249C0
		public virtual bool ShouldTakeExtraTurn(Player player)
		{
			return false;
		}

		/// <summary>
		/// If true is returned from this hook, a potion will always be generated in the rewards for the room, no matter what.
		/// </summary>
		/// <param name="player">Player who is receiving the reward.</param>
		/// <param name="roomType">Type of room that the reward is being offered in.</param>
		// Token: 0x06004A3C RID: 19004 RVA: 0x002267C3 File Offset: 0x002249C3
		public virtual bool ShouldForcePotionReward(Player player, RoomType roomType)
		{
			return false;
		}

		/// <summary>
		/// When the player is able to choose a map point, should they be allowed to choose any map point in the next row?
		/// </summary>
		// Token: 0x06004A3D RID: 19005 RVA: 0x002267C6 File Offset: 0x002249C6
		public virtual bool ShouldAllowFreeTravel()
		{
			return false;
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x002267CC File Offset: 0x002249CC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(RuntimeHelpers.GetHashCode(this));
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0022681B File Offset: 0x00224A1B
		protected void NeverEverCallThisOutsideOfTests_SetIsMutable(bool isMutable)
		{
			if (TestMode.IsOff)
			{
				throw new InvalidOperationException("You monster!");
			}
			this.IsMutable = isMutable;
		}
	}
}
