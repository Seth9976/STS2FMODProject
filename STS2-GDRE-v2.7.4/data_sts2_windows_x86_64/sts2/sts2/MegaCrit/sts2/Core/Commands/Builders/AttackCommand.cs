using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Commands.Builders
{
	/// <summary>
	/// A "builder" command that allows you to chain method calls to build up the property of an attack.
	/// This pattern is very similar to how Godot Tweens are built up.
	/// </summary>
	// Token: 0x02000D3D RID: 3389
	[NullableContext(2)]
	[Nullable(0)]
	public class AttackCommand
	{
		/// <summary>
		/// The creature performing this attack.
		/// It's safe to assume this is non-null once the attack is being executed.
		/// </summary>
		// Token: 0x1700257A RID: 9594
		// (get) Token: 0x0600888E RID: 34958 RVA: 0x002DFEAF File Offset: 0x002DE0AF
		// (set) Token: 0x0600888F RID: 34959 RVA: 0x002DFEB7 File Offset: 0x002DE0B7
		public Creature Attacker { get; private set; }

		/// <summary>
		/// The model that this attack comes from, such as an attack card.
		/// </summary>
		// Token: 0x1700257B RID: 9595
		// (get) Token: 0x06008890 RID: 34960 RVA: 0x002DFEC0 File Offset: 0x002DE0C0
		// (set) Token: 0x06008891 RID: 34961 RVA: 0x002DFEC8 File Offset: 0x002DE0C8
		public AbstractModel ModelSource { get; private set; }

		/// <summary>
		/// The card play that this attack comes from, if any.
		/// </summary>
		// Token: 0x1700257C RID: 9596
		// (get) Token: 0x06008892 RID: 34962 RVA: 0x002DFED1 File Offset: 0x002DE0D1
		// (set) Token: 0x06008893 RID: 34963 RVA: 0x002DFED9 File Offset: 0x002DE0D9
		public CardPlay CardPlay { get; private set; }

		/// <summary>
		/// The creatures being targeted by this attack.
		/// For random attacks, this contains all possible targets, and for custom attacks using
		/// <see cref="T:MegaCrit.Sts2.Core.Commands.Builders.AttackContext" />, this will be empty.
		/// After the attack has been executed, you can check <see cref="P:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand.Results" /> to see which targets were actually hit.
		/// </summary>
		// Token: 0x06008894 RID: 34964 RVA: 0x002DFEE4 File Offset: 0x002DE0E4
		[NullableContext(1)]
		private IReadOnlyList<Creature> GetPossibleTargets()
		{
			if (this.IsSingleTargeted)
			{
				return new <>z__ReadOnlySingleElementList<Creature>(this._singleTarget);
			}
			if (!this.IsMultiTargeted)
			{
				throw new InvalidOperationException("No targets set, a Targeting method must be called before Execute");
			}
			if (this._sourceType == AttackCommand.SourceType.Monster)
			{
				return this._combatState.PlayerCreatures;
			}
			if (this.Attacker == null)
			{
				throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
			}
			return this._combatState.GetOpponentsOf(this.Attacker);
		}

		// Token: 0x1700257D RID: 9597
		// (get) Token: 0x06008895 RID: 34965 RVA: 0x002DFF51 File Offset: 0x002DE151
		// (set) Token: 0x06008896 RID: 34966 RVA: 0x002DFF59 File Offset: 0x002DE159
		public CombatSide TargetSide { get; private set; }

		/// <summary>
		/// The ValueProps of the damage that this attack deals.
		/// </summary>
		// Token: 0x1700257E RID: 9598
		// (get) Token: 0x06008897 RID: 34967 RVA: 0x002DFF62 File Offset: 0x002DE162
		// (set) Token: 0x06008898 RID: 34968 RVA: 0x002DFF6A File Offset: 0x002DE16A
		public ValueProp DamageProps { get; private set; } = ValueProp.Move;

		/// <summary>
		/// Whether this attack is targeting a single creature.
		/// </summary>
		// Token: 0x1700257F RID: 9599
		// (get) Token: 0x06008899 RID: 34969 RVA: 0x002DFF73 File Offset: 0x002DE173
		public bool IsSingleTargeted
		{
			get
			{
				return this._singleTarget != null;
			}
		}

		/// <summary>
		/// Whether this attack is targeting multiple creatures.
		/// </summary>
		// Token: 0x17002580 RID: 9600
		// (get) Token: 0x0600889A RID: 34970 RVA: 0x002DFF7E File Offset: 0x002DE17E
		public bool IsMultiTargeted
		{
			get
			{
				return this._combatState != null;
			}
		}

		/// <summary>
		/// Whether this attack is randomly targeted.
		/// </summary>
		// Token: 0x17002581 RID: 9601
		// (get) Token: 0x0600889B RID: 34971 RVA: 0x002DFF89 File Offset: 0x002DE189
		// (set) Token: 0x0600889C RID: 34972 RVA: 0x002DFF91 File Offset: 0x002DE191
		public bool IsRandomlyTargeted { get; private set; }

		/// <summary>
		/// The resulting instances of damage from this attack.
		/// Each inner list is from an individual hit from this attack (so if we have a multi-attack, we have multiple lists)
		/// This will start out empty, and will be populated after <see cref="M:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand.Execute(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext)" /> finishes running.
		/// </summary>
		// Token: 0x17002582 RID: 9602
		// (get) Token: 0x0600889D RID: 34973 RVA: 0x002DFF9A File Offset: 0x002DE19A
		[Nullable(1)]
		public IEnumerable<List<DamageResult>> Results
		{
			[NullableContext(1)]
			get
			{
				return this._results;
			}
		}

		// Token: 0x17002583 RID: 9603
		// (get) Token: 0x0600889E RID: 34974 RVA: 0x002DFFA2 File Offset: 0x002DE1A2
		// (set) Token: 0x0600889F RID: 34975 RVA: 0x002DFFAA File Offset: 0x002DE1AA
		public string HitSfx { get; private set; }

		// Token: 0x17002584 RID: 9604
		// (get) Token: 0x060088A0 RID: 34976 RVA: 0x002DFFB3 File Offset: 0x002DE1B3
		// (set) Token: 0x060088A1 RID: 34977 RVA: 0x002DFFBB File Offset: 0x002DE1BB
		public string TmpHitSfx { get; private set; }

		// Token: 0x17002585 RID: 9605
		// (get) Token: 0x060088A2 RID: 34978 RVA: 0x002DFFC4 File Offset: 0x002DE1C4
		// (set) Token: 0x060088A3 RID: 34979 RVA: 0x002DFFCC File Offset: 0x002DE1CC
		public string HitVfx { get; private set; }

		/// <summary>
		/// Create a new attack command.
		/// </summary>
		/// <param name="damagePerHit">The amount of damage this attack should deal on each hit.</param>
		// Token: 0x060088A4 RID: 34980 RVA: 0x002DFFD8 File Offset: 0x002DE1D8
		public AttackCommand(decimal damagePerHit)
		{
			this._damagePerHit = damagePerHit;
			this._calculatedDamageVar = null;
		}

		/// <summary>
		/// Create a new attack command
		/// </summary>
		/// <param name="calculatedDamageVar">
		/// Dynamic var that calculates how much damage each hit does.
		/// Used for cards that do dynamic amounts of damage based on combat state, like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" />.
		/// </param>
		// Token: 0x060088A5 RID: 34981 RVA: 0x002E0060 File Offset: 0x002DE260
		[NullableContext(1)]
		public AttackCommand(CalculatedDamageVar calculatedDamageVar)
		{
			this._damagePerHit = -1m;
			this._calculatedDamageVar = calculatedDamageVar;
		}

		/// <summary>
		/// Set the attack to come from the specified card.
		/// This also automatically sets the attacker as the card's owner, and the attacker animation name/delay to the card
		/// owner's defaults.
		/// </summary>
		/// <param name="card">Card that the attack came from.</param>
		/// <param name="cardPlay"></param>
		// Token: 0x060088A6 RID: 34982 RVA: 0x002E00EC File Offset: 0x002DE2EC
		[NullableContext(1)]
		public AttackCommand FromCard(CardModel card, [Nullable(2)] CardPlay cardPlay)
		{
			if (this.Attacker != null)
			{
				throw new InvalidOperationException("Attacker has already been set.");
			}
			if (this.ModelSource != null)
			{
				throw new InvalidOperationException("ModelSource has already been set.");
			}
			if (cardPlay != null && this.CardPlay != null)
			{
				throw new InvalidOperationException("CardPlay has already been set.");
			}
			Player owner = card.Owner;
			this.Attacker = owner.Creature;
			this._attackerAnimName = "Attack";
			this._attackerAnimDelay = owner.Character.AttackAnimDelay;
			this.ModelSource = card;
			this.CardPlay = cardPlay;
			this._sourceType = AttackCommand.SourceType.Card;
			return this;
		}

		// Token: 0x060088A7 RID: 34983 RVA: 0x002E017C File Offset: 0x002DE37C
		[NullableContext(1)]
		public AttackCommand FromOsty(Creature osty, CardModel card, [Nullable(2)] CardPlay cardPlay)
		{
			if (!(osty.Monster is Osty))
			{
				throw new ArgumentException("Creature is not Osty");
			}
			this.Attacker = osty;
			this.ModelSource = card;
			this.CardPlay = cardPlay;
			this._attackerAnimName = "Attack";
			this._attackerAnimDelay = 0.3f;
			this._sourceType = AttackCommand.SourceType.Card;
			return this.WithAttackerFx(null, "event:/sfx/characters/osty/osty_attack", null);
		}

		/// <summary>
		/// Set the attack to come from the specified monster.
		/// </summary>
		/// <param name="monster">Monster that the attack came from.</param>
		// Token: 0x060088A8 RID: 34984 RVA: 0x002E01E0 File Offset: 0x002DE3E0
		[NullableContext(1)]
		public AttackCommand FromMonster(MonsterModel monster)
		{
			if (this.Attacker != null)
			{
				throw new InvalidOperationException("Attacker has already been set.");
			}
			this.Attacker = monster.Creature;
			this._attackerAnimName = "Attack";
			this._sourceType = AttackCommand.SourceType.Monster;
			return this.TargetingAllOpponents(monster.Creature.CombatState);
		}

		/// <summary>
		/// Set the attack to target the specified creature.
		/// </summary>
		/// <param name="target">Creature for the attack to target.</param>
		// Token: 0x060088A9 RID: 34985 RVA: 0x002E022F File Offset: 0x002DE42F
		[NullableContext(1)]
		public AttackCommand Targeting(Creature target)
		{
			if (this._singleTarget != null)
			{
				throw new InvalidOperationException("Targets already set.");
			}
			if (this._combatState != null)
			{
				throw new InvalidOperationException("Already set to target opponents of attacker");
			}
			this._singleTarget = target;
			this.TargetSide = target.Side;
			return this;
		}

		/// <summary>
		/// Set the attack to target the opponents of the Attacker.
		/// Differs from TargetingAll because the list of targets is refreshed between every hit
		/// meaning that new creatures added to the combat will become valid targets
		/// </summary>
		/// <param name="combatState">Combat state that we will be pulling the opponents from</param>
		// Token: 0x060088AA RID: 34986 RVA: 0x002E026C File Offset: 0x002DE46C
		[NullableContext(1)]
		public AttackCommand TargetingAllOpponents(ICombatState combatState)
		{
			if (this._singleTarget != null)
			{
				throw new InvalidOperationException("Targets already set.");
			}
			if (this._combatState != null)
			{
				throw new InvalidOperationException("Already set to target opponents of attacker");
			}
			if (this.Attacker == null)
			{
				throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
			}
			this._combatState = combatState;
			this.TargetSide = ((this.Attacker.Side == CombatSide.Enemy) ? CombatSide.Player : CombatSide.Enemy);
			return this;
		}

		/// <summary>
		/// Sets the attack to target random opponents of the attacker.
		/// A new random target will be chosen on each hit.
		/// </summary>
		/// <param name="combatState">Combat state that we will be pulling the opponents from</param>
		/// <param name="allowDuplicates">
		/// Whether the same target can be hit multiple times.
		/// In real gameplay, we pretty much always want this to be true, but we pass false sometimes for testing.
		/// </param>
		// Token: 0x060088AB RID: 34987 RVA: 0x002E02D4 File Offset: 0x002DE4D4
		[NullableContext(1)]
		public AttackCommand TargetingRandomOpponents(ICombatState combatState, bool allowDuplicates = true)
		{
			if (this._singleTarget != null)
			{
				throw new InvalidOperationException("Targets already set.");
			}
			if (this._combatState != null)
			{
				throw new InvalidOperationException("Already set to target opponents of attacker");
			}
			if (this.Attacker == null)
			{
				throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
			}
			this._combatState = combatState;
			this.IsRandomlyTargeted = true;
			this._doesRandomTargetingAllowDuplicates = allowDuplicates;
			return this;
		}

		/// <summary>
		/// Set the attack to deal unpowered damage.
		///
		/// WARNING: 99% of the time, when unpowered damage is dealt, it shouldn't be considered an
		/// attack (like via <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Burn" />). However, occasionally we want to do a real attack
		/// that skips powers (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Omnislice" />), so this is useful there.
		/// </summary>
		// Token: 0x060088AC RID: 34988 RVA: 0x002E0330 File Offset: 0x002DE530
		[NullableContext(1)]
		public AttackCommand Unpowered()
		{
			this.DamageProps |= ValueProp.Unpowered;
			return this;
		}

		/// <summary>
		/// Set the attack to play the specified animation on the attacker.
		/// </summary>
		/// <param name="animName">Name of the animation trigger.</param>
		/// <param name="delay">Amount of time to wait for the animation to complete before proceeding.</param>
		/// <param name="visualAttacker">
		/// Optional custom creature to visually display as the attacker in this animation.
		/// Good for attacks like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.ByrdSwoop" /> where we want a pet to animate for the attack, even though the
		/// actual attack is coming from the attack's owner.
		/// If this is left null, the attack's owning creature will play the animation.
		/// </param>
		// Token: 0x060088AD RID: 34989 RVA: 0x002E0341 File Offset: 0x002DE541
		[return: Nullable(1)]
		public AttackCommand WithAttackerAnim(string animName, float delay, Creature visualAttacker = null)
		{
			if (this._attackerAnimName == null)
			{
				throw new InvalidOperationException("WithAttackerAnim was called before FromCard/FromMonster/FromOsty, should be called after.");
			}
			this._attackerAnimName = animName;
			this._attackerAnimDelay = delay;
			this._visualAttacker = visualAttacker;
			return this;
		}

		// Token: 0x060088AE RID: 34990 RVA: 0x002E036C File Offset: 0x002DE56C
		[NullableContext(1)]
		public AttackCommand WithNoAttackerAnim()
		{
			this._shouldPlayAnimation = false;
			return this;
		}

		/// <summary>
		/// Logic to execute after the attacker animation plays. Good for one-offs.
		/// Note: If you find yourself calling this with the same logic in many different attacks, consider making a
		/// first-class builder method for the logic instead.
		/// </summary>
		// Token: 0x060088AF RID: 34991 RVA: 0x002E0376 File Offset: 0x002DE576
		[NullableContext(1)]
		public AttackCommand AfterAttackerAnim(Func<Task> afterAttackerAnim)
		{
			this._afterAttackerAnim = afterAttackerAnim;
			return this;
		}

		/// <summary>
		/// Set the attack to play the specified VFX/SFX on attacker.
		/// </summary>
		/// <param name="vfx">File path of the VFX.</param>
		/// <param name="sfx">File path of the SFX.</param>
		/// <param name="tmpSfx">
		/// Temporary SFX file path. If the attack uses temporary non-FMOD SFX, pass this instead of sfx using keyword args.
		/// </param>
		// Token: 0x060088B0 RID: 34992 RVA: 0x002E0380 File Offset: 0x002DE580
		[return: Nullable(1)]
		public AttackCommand WithAttackerFx(string vfx = null, string sfx = null, string tmpSfx = null)
		{
			this._attackerVfx = vfx;
			this._attackerSfx = sfx;
			this._tmpAttackerSfx = tmpSfx;
			return this;
		}

		/// <summary>
		/// Set the attack to add the specified custom VFX node to the combat VFX container when the owner attacks.
		/// </summary>
		// Token: 0x060088B1 RID: 34993 RVA: 0x002E0398 File Offset: 0x002DE598
		[NullableContext(1)]
		public AttackCommand WithAttackerFx([Nullable(new byte[] { 1, 2 })] Func<Node2D> createAttackerVfx)
		{
			this._customAttackerVfxNodes.Add(createAttackerVfx);
			return this;
		}

		/// <summary>
		/// Set a wait time before each hit and hit VFX/SFX.
		/// Uses the same wait time signature as <see cref="M:MegaCrit.Sts2.Core.Commands.Cmd.CustomScaledWait(System.Single,System.Single,System.Boolean,System.Threading.CancellationToken)" />.
		/// </summary>
		// Token: 0x060088B2 RID: 34994 RVA: 0x002E03A7 File Offset: 0x002DE5A7
		[NullableContext(1)]
		public AttackCommand WithWaitBeforeHit(float fastSeconds, float standardSeconds)
		{
			this._waitBeforeHit[0] = fastSeconds;
			this._waitBeforeHit[1] = standardSeconds;
			return this;
		}

		/// <summary>
		/// Set the attack to play the specified VFX/SFX on the target(s) when they're hit.
		/// </summary>
		/// <param name="vfx">File path of the VFX.</param>
		/// <param name="sfx">File path of the SFX.</param>
		/// <param name="tmpSfx">
		/// Temporary SFX file path. If the attack uses temporary non-FMOD SFX, pass this instead of sfx using keyword args.
		/// </param>
		// Token: 0x060088B3 RID: 34995 RVA: 0x002E03BC File Offset: 0x002DE5BC
		[return: Nullable(1)]
		public AttackCommand WithHitFx(string vfx = null, string sfx = null, string tmpSfx = null)
		{
			this.HitVfx = vfx;
			this.HitSfx = sfx;
			this.TmpHitSfx = tmpSfx;
			return this;
		}

		/// <summary>
		/// If this attack hits multiple creatures, this sets the hit VFX to spawn on each creature rather than on the side.
		/// </summary>
		// Token: 0x060088B4 RID: 34996 RVA: 0x002E03D4 File Offset: 0x002DE5D4
		[NullableContext(1)]
		public AttackCommand SpawningHitVfxOnEachCreature()
		{
			this._spawnVfxOnEachCreature = true;
			return this;
		}

		/// <summary>
		/// If there is a hit VFX set, this makes it spawn at the creature's base rather than its center.
		/// </summary>
		// Token: 0x060088B5 RID: 34997 RVA: 0x002E03DE File Offset: 0x002DE5DE
		[NullableContext(1)]
		public AttackCommand WithHitVfxSpawnedAtBase()
		{
			this._spawnVfxOnCreatureCenter = false;
			return this;
		}

		/// <summary>
		/// Set the attack to add the specified custom VFX node to the combat VFX container when the target(s) are hit.
		/// </summary>
		// Token: 0x060088B6 RID: 34998 RVA: 0x002E03E8 File Offset: 0x002DE5E8
		[NullableContext(1)]
		public AttackCommand WithHitVfxNode([Nullable(new byte[] { 1, 1, 2 })] Func<Creature, Node2D> createHitVfxNode)
		{
			this._customHitVfxNodes.Add(createHitVfxNode);
			return this;
		}

		/// <summary>
		/// Set the attack to only play animations/VFX/SFX once, even on a multi-hit.
		/// Good for situations where a single bespoke attack animation illustrates multiple hits.
		/// </summary>
		// Token: 0x060088B7 RID: 34999 RVA: 0x002E03F7 File Offset: 0x002DE5F7
		[NullableContext(1)]
		public AttackCommand OnlyPlayAnimOnce()
		{
			this._playOnEveryHit = false;
			return this;
		}

		/// <summary>
		/// Set the number of hits this attack should do.
		/// </summary>
		/// <param name="hitCount">The number of times this attack should hit.</param>
		// Token: 0x060088B8 RID: 35000 RVA: 0x002E0401 File Offset: 0x002DE601
		[NullableContext(1)]
		public AttackCommand WithHitCount(int hitCount)
		{
			this._hitCount = hitCount;
			return this;
		}

		/// <summary>
		/// Logic to execute before each instance of damage is dealt. Good for one-offs.
		/// Note: If you find yourself calling this with the same logic in many different attacks, consider making a
		/// first-class builder method for the logic instead.
		/// </summary>
		// Token: 0x060088B9 RID: 35001 RVA: 0x002E040B File Offset: 0x002DE60B
		[NullableContext(1)]
		public AttackCommand BeforeDamage(Func<Task> beforeDamage)
		{
			this._beforeDamage = beforeDamage;
			return this;
		}

		/// <summary>
		/// Creates an attack context for grouping multiple damage calls as a single attack.
		/// Use with `await using` to automatically call BeforeAttack and AfterAttack hooks.
		/// </summary>
		/// <param name="combatState">The current combat state</param>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="cardPlay">The card play that is the source of this attack</param>
		/// <returns>An AttackContext ready for use with await using</returns>
		// Token: 0x060088BA RID: 35002 RVA: 0x002E0418 File Offset: 0x002DE618
		[NullableContext(1)]
		public static async Task<AttackContext> CreateContextAsync(ICombatState combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			return await AttackContext.CreateAsync(combatState, choiceContext, cardPlay);
		}

		/// <summary>
		/// Execute this attack.
		/// If you forget to set some required values (like attacker or target), this will error.
		/// </summary>
		/// <param name="choiceContext">
		/// The context that is signalled in the event of a player choice. If null, a
		/// <see cref="T:MegaCrit.Sts2.Core.GameActions.Multiplayer.BlockingPlayerChoiceContext" /> is used. This should only be done when a monster is attacking.
		/// </param>
		// Token: 0x060088BB RID: 35003 RVA: 0x002E046C File Offset: 0x002DE66C
		[NullableContext(1)]
		public async Task<AttackCommand> Execute([Nullable(2)] PlayerChoiceContext choiceContext)
		{
			Creature attacker = this.Attacker;
			ICombatState combatState = ((attacker != null) ? attacker.CombatState : null);
			if (this.Attacker == null)
			{
				throw new InvalidOperationException("No attacker set.");
			}
			AttackCommand attackCommand;
			if (CombatManager.Instance.IsOverOrEnding && (combatState == null || combatState.IsLiveCombat()))
			{
				attackCommand = this;
			}
			else
			{
				if (combatState == null)
				{
					throw new InvalidOperationException("No combat state even though combat is not over.");
				}
				if (this.Attacker.IsDead)
				{
					attackCommand = this;
				}
				else
				{
					if (!this.IsSingleTargeted && !this.IsMultiTargeted)
					{
						throw new InvalidOperationException("No targets set.");
					}
					await Hook.BeforeAttack(combatState, this);
					decimal attackCount = Hook.ModifyAttackHitCount(combatState, this, this._hitCount);
					int i = 0;
					while (i < attackCount && !this.Attacker.IsDead)
					{
						List<Creature> validTargets = (from c in this.GetPossibleTargets()
							where c.IsAlive
							select c).ToList<Creature>();
						if (validTargets.Count == 0 && combatState.IsLiveCombat())
						{
							break;
						}
						if (this._playOnEveryHit || i == 0)
						{
							if (this._attackerVfx != null)
							{
								VfxCmd.PlayOnCreatureCenter(this.Attacker, this._attackerVfx);
							}
							foreach (Func<Node2D> func in this._customAttackerVfxNodes)
							{
								Control vfxContainer = this.Attacker.GetVfxContainer();
								if (vfxContainer != null)
								{
									vfxContainer.AddChildSafely(func());
								}
							}
							if (this._attackerSfx != null)
							{
								SfxCmd.Play(this._attackerSfx, 1f);
							}
							else if (this._tmpAttackerSfx != null)
							{
								NDebugAudioManager instance = NDebugAudioManager.Instance;
								if (instance != null)
								{
									instance.Play(this._tmpAttackerSfx, 1f, PitchVariance.None);
								}
							}
							if (this._attackerAnimName != null && this._shouldPlayAnimation)
							{
								await CreatureCmd.TriggerAnim(this._visualAttacker ?? this.Attacker, this._attackerAnimName, this._attackerAnimDelay);
							}
							if (this._afterAttackerAnim != null)
							{
								await this._afterAttackerAnim();
							}
						}
						if (validTargets.Count > 0)
						{
							if (this.HitSfx != null)
							{
								SfxCmd.Play(this.HitSfx, 1f);
							}
							else if (this.TmpHitSfx != null)
							{
								NDebugAudioManager instance2 = NDebugAudioManager.Instance;
								if (instance2 != null)
								{
									instance2.Play(this.TmpHitSfx, 1f, PitchVariance.None);
								}
							}
						}
						Creature singleTarget;
						if (this.IsRandomlyTargeted)
						{
							if (!this._doesRandomTargetingAllowDuplicates)
							{
								validTargets = validTargets.Where<Creature>((Creature c) => this._results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).All<DamageResult>((DamageResult r) => r.Receiver != c)).ToList<Creature>();
								if (validTargets.Count == 0)
								{
									throw new InvalidOperationException("No valid targets for attack with duplicates disallowed. If you're in a test, you probably need to add more enemies. If you're in real gameplay, something is wrong.");
								}
							}
							singleTarget = (this.Attacker.Player ?? this.Attacker.PetOwner).RunState.Rng.CombatTargets.NextItem<Creature>(validTargets);
						}
						else if (validTargets.Count == 1)
						{
							singleTarget = validTargets[0];
						}
						else
						{
							singleTarget = null;
						}
						if (this._waitBeforeHit.Any<float>((float w) => w > 0f))
						{
							await Cmd.CustomScaledWait(this._waitBeforeHit[0], this._waitBeforeHit[1], false, default(CancellationToken));
						}
						foreach (Func<Creature, Node2D> func2 in this._customHitVfxNodes)
						{
							if (singleTarget != null)
							{
								Control vfxContainer2 = singleTarget.GetVfxContainer();
								if (vfxContainer2 != null)
								{
									vfxContainer2.AddChildSafely(func2(singleTarget));
								}
							}
							else
							{
								foreach (Creature creature in validTargets)
								{
									Control vfxContainer3 = creature.GetVfxContainer();
									if (vfxContainer3 != null)
									{
										vfxContainer3.AddChildSafely(func2(creature));
									}
								}
							}
						}
						if (this.HitVfx != null)
						{
							if (singleTarget != null)
							{
								if (this._spawnVfxOnCreatureCenter)
								{
									VfxCmd.PlayOnCreatureCenter(singleTarget, this.HitVfx);
								}
								else
								{
									VfxCmd.PlayOnCreature(singleTarget, this.HitVfx);
								}
							}
							else if (this._spawnVfxOnEachCreature)
							{
								if (this._spawnVfxOnCreatureCenter)
								{
									VfxCmd.PlayOnCreatureCenters(validTargets, this.HitVfx);
								}
								else
								{
									VfxCmd.PlayOnCreatures(validTargets, this.HitVfx);
								}
							}
							else
							{
								VfxCmd.PlayOnSide(this.Attacker.Side.GetOppositeSide(), this.HitVfx, combatState);
							}
						}
						if (this._beforeDamage != null)
						{
							await this._beforeDamage();
						}
						decimal num = ((this._calculatedDamageVar == null) ? this._damagePerHit : this._calculatedDamageVar.Calculate(singleTarget));
						PlayerChoiceContext playerChoiceContext = choiceContext ?? new BlockingPlayerChoiceContext();
						List<Creature> list;
						if (singleTarget == null)
						{
							list = validTargets;
						}
						else
						{
							(list = new List<Creature>(1)).Add(singleTarget);
						}
						this.AddResultsInternal(await CreatureCmd.Damage(playerChoiceContext, list, num, this.DamageProps, this.Attacker, this.ModelSource as CardModel, this.CardPlay));
						validTargets = null;
						singleTarget = null;
						i++;
					}
					CombatManager.Instance.History.CreatureAttacked(combatState, this.Attacker, this._results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).ToList<DamageResult>());
					await Hook.AfterAttack(combatState, choiceContext ?? new BlockingPlayerChoiceContext(), this);
					attackCommand = this;
				}
			}
			return attackCommand;
		}

		/// <summary>
		/// NEVER CALL THIS FROM MODELS, only in system code where you know exactly what you're doing.
		/// Increment the number of hits this attack should do.
		/// </summary>
		// Token: 0x060088BC RID: 35004 RVA: 0x002E04B7 File Offset: 0x002DE6B7
		public void IncrementHitsInternal()
		{
			this._hitCount++;
		}

		/// <summary>
		/// NEVER CALL THIS FROM MODELS, only in system code where you know exactly what you're doing.
		/// Add a set of DamageResults to the list of results for this attack.
		/// </summary>
		// Token: 0x060088BD RID: 35005 RVA: 0x002E04C7 File Offset: 0x002DE6C7
		[NullableContext(1)]
		public void AddResultsInternal(IEnumerable<DamageResult> results)
		{
			this._results.Add(results.ToList<DamageResult>());
		}

		/// <summary>
		/// The amount of damage that this attack will deal on each hit.
		/// This will be ignored if <see cref="F:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand._calculatedDamageVar" /> is set.
		/// </summary>
		// Token: 0x04002F81 RID: 12161
		private readonly decimal _damagePerHit;

		/// <summary>
		/// Dynamic var that calculates how much damage each hit does.
		/// Used for cards that do dynamic amounts of damage based on combat state, like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" />.
		/// Settings this will cause <see cref="F:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand._damagePerHit" /> to be ignored.
		/// </summary>
		// Token: 0x04002F82 RID: 12162
		private readonly CalculatedDamageVar _calculatedDamageVar;

		/// <summary>
		/// The number of hits this attack should do.
		/// The actual number of hits may be modified by hooks.
		/// If you need to check the final number of hits, check <see cref="P:MegaCrit.Sts2.Core.Commands.Builders.AttackCommand.Results" /> after the attack has been executed.
		/// </summary>
		// Token: 0x04002F83 RID: 12163
		private int _hitCount = 1;

		/// <summary>
		/// The source of this attack.
		/// Without this, we have to glean it from some combination of Attacker and ModelSource, which can get tricky in
		/// cases like Osty attacks.
		/// </summary>
		// Token: 0x04002F84 RID: 12164
		private AttackCommand.SourceType _sourceType;

		// Token: 0x04002F85 RID: 12165
		private ICombatState _combatState;

		/// <summary>
		/// Set for single target attacks. Is null for AOE attacks.
		/// </summary>
		// Token: 0x04002F86 RID: 12166
		private Creature _singleTarget;

		// Token: 0x04002F8A RID: 12170
		private bool _spawnVfxOnEachCreature;

		// Token: 0x04002F8B RID: 12171
		private bool _spawnVfxOnCreatureCenter = true;

		// Token: 0x04002F8C RID: 12172
		private bool _doesRandomTargetingAllowDuplicates = true;

		// Token: 0x04002F8D RID: 12173
		private bool _shouldPlayAnimation = true;

		// Token: 0x04002F8E RID: 12174
		[Nullable(1)]
		private readonly List<List<DamageResult>> _results = new List<List<DamageResult>>();

		// Token: 0x04002F8F RID: 12175
		private string _attackerAnimName;

		// Token: 0x04002F90 RID: 12176
		private float _attackerAnimDelay;

		// Token: 0x04002F91 RID: 12177
		private Creature _visualAttacker;

		// Token: 0x04002F92 RID: 12178
		private bool _playOnEveryHit = true;

		// Token: 0x04002F93 RID: 12179
		private string _attackerVfx;

		// Token: 0x04002F94 RID: 12180
		private string _attackerSfx;

		// Token: 0x04002F95 RID: 12181
		private string _tmpAttackerSfx;

		/// <summary>
		/// fast, standard
		/// </summary>
		// Token: 0x04002F96 RID: 12182
		[Nullable(1)]
		private readonly float[] _waitBeforeHit = new float[] { -1f, -1f };

		// Token: 0x04002F9A RID: 12186
		[Nullable(new byte[] { 1, 1, 2 })]
		private readonly List<Func<Node2D>> _customAttackerVfxNodes = new List<Func<Node2D>>();

		// Token: 0x04002F9B RID: 12187
		[Nullable(new byte[] { 1, 1, 1, 2 })]
		private readonly List<Func<Creature, Node2D>> _customHitVfxNodes = new List<Func<Creature, Node2D>>();

		// Token: 0x04002F9C RID: 12188
		[Nullable(new byte[] { 2, 1 })]
		private Func<Task> _afterAttackerAnim;

		// Token: 0x04002F9D RID: 12189
		[Nullable(new byte[] { 2, 1 })]
		private Func<Task> _beforeDamage;

		// Token: 0x020024D4 RID: 9428
		[NullableContext(0)]
		private enum SourceType
		{
			// Token: 0x04009D09 RID: 40201
			None,
			// Token: 0x04009D0A RID: 40202
			Card,
			// Token: 0x04009D0B RID: 40203
			Monster
		}
	}
}
