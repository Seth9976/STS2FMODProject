using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000803 RID: 2051
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tunneler : MonsterModel
	{
		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x06006631 RID: 26161 RVA: 0x0026F1EC File Offset: 0x0026D3EC
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 92, 87);
			}
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x06006632 RID: 26162 RVA: 0x0026F1F8 File Offset: 0x0026D3F8
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x06006633 RID: 26163 RVA: 0x0026F200 File Offset: 0x0026D400
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_attack";
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x06006634 RID: 26164 RVA: 0x0026F207 File Offset: 0x0026D407
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hurt";
			}
		}

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x06006635 RID: 26165 RVA: 0x0026F20E File Offset: 0x0026D40E
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_die";
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x06006636 RID: 26166 RVA: 0x0026F215 File Offset: 0x0026D415
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
			}
		}

		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x06006637 RID: 26167 RVA: 0x0026F222 File Offset: 0x0026D422
		private int BlockGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 37, 32);
			}
		}

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x06006638 RID: 26168 RVA: 0x0026F22E File Offset: 0x0026D42E
		private int BelowDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 26, 23);
			}
		}

		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x06006639 RID: 26169 RVA: 0x0026F23B File Offset: 0x0026D43B
		// (set) Token: 0x0600663A RID: 26170 RVA: 0x0026F243 File Offset: 0x0026D443
		public bool IsStunned
		{
			get
			{
				return this._isStunned;
			}
			set
			{
				base.AssertMutable();
				this._isStunned = value;
			}
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x0026F254 File Offset: 0x0026D454
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState2 = new MoveState("BURROW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurrowMove), new AbstractIntent[]
			{
				new BuffIntent(),
				new DefendIntent()
			});
			MoveState moveState3 = new MoveState("BELOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BelowMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BelowDamage)
			});
			MoveState moveState4 = new MoveState("DIZZY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StillDizzyMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState3;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x0026F354 File Offset: 0x0026D554
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			this.IsStunned = false;
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x0026F398 File Offset: 0x0026D598
		private async Task BurrowMove(IReadOnlyList<Creature> targets)
		{
			this.IsStunned = false;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_burrow", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Burrow", 0.25f);
			await PowerCmd.Apply<BurrowedPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			await CreatureCmd.GainBlock(base.Creature, this.BlockGain, ValueProp.Move, null, false);
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x0026F3DC File Offset: 0x0026D5DC
		private async Task BelowMove(IReadOnlyList<Creature> targets)
		{
			this.IsStunned = false;
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				Node2D node2D = ((creatureNode != null) ? creatureNode.GetSpecialNode<Node2D>("Visuals/SpineBoneNode") : null);
				if (node2D != null && creatureNode != null)
				{
					if (targets.Count > 0)
					{
						NCreature creatureNode2 = targets[0].GetCreatureNode();
						if (creatureNode2 != null)
						{
							float num = 400f * creatureNode.Visuals.Scale.X;
							node2D.GlobalPosition = new Vector2(creatureNode2.GlobalPosition.X + num, node2D.GlobalPosition.Y);
						}
					}
					else
					{
						node2D.Position = Vector2.Left * 400f;
					}
				}
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hidden_attack", 1f);
				await CreatureCmd.TriggerAnim(base.Creature, "BurrowAttack", 0.25f);
				await Cmd.Wait(1f, false);
			}
			await DamageCmd.Attack(this.BelowDamage).FromMonster(this).WithNoAttackerAnim()
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x0026F428 File Offset: 0x0026D628
		public async Task GetStunned()
		{
			this.IsStunned = true;
			await CreatureCmd.TriggerAnim(base.Creature, "Stun", 0.25f);
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x0026F46C File Offset: 0x0026D66C
		public async Task StillDizzyMove(IReadOnlyList<Creature> targets)
		{
			this.IsStunned = false;
			await CreatureCmd.TriggerAnim(base.Creature, "WakeUp", 0.25f);
		}

		// Token: 0x06006641 RID: 26177 RVA: 0x0026F4B0 File Offset: 0x0026D6B0
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("die", false);
			AnimState animState3 = new AnimState("hurt", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("stun", false);
			AnimState animState6 = new AnimState("stunned_loop", true);
			AnimState animState7 = new AnimState("stunned_hurt", false);
			AnimState animState8 = new AnimState("wake_up", false);
			AnimState animState9 = new AnimState("burrow", false);
			AnimState animState10 = new AnimState("hidden_loop", true);
			AnimState animState11 = new AnimState("hidden_attack", false);
			AnimState animState12 = new AnimState("hidden_die", false);
			animState9.NextState = animState10;
			animState11.NextState = animState10;
			animState4.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState6;
			animState7.NextState = animState6;
			animState8.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Hit", animState3, () => !base.Creature.HasPower<BurrowedPower>() && !this.IsStunned);
			creatureAnimator.AddAnyState("Hit", animState7, () => !base.Creature.HasPower<BurrowedPower>() && this.IsStunned);
			creatureAnimator.AddAnyState("Dead", animState2, () => !base.Creature.HasPower<BurrowedPower>());
			creatureAnimator.AddAnyState("Dead", animState12, () => base.Creature.HasPower<BurrowedPower>());
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("BurrowAttack", animState11, null);
			creatureAnimator.AddAnyState("Burrow", animState9, null);
			creatureAnimator.AddAnyState("Stun", animState5, null);
			creatureAnimator.AddAnyState("WakeUp", animState8, null);
			return creatureAnimator;
		}

		// Token: 0x06006642 RID: 26178 RVA: 0x0026F64C File Offset: 0x0026D84C
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.Insert(3, BestiaryMonsterMove.FromStun(new Func<Task>(this.GetStunned)));
			return list;
		}

		// Token: 0x04002700 RID: 9984
		private const string _dizzyMoveId = "DIZZY_MOVE";

		// Token: 0x04002701 RID: 9985
		public const string biteMoveId = "BITE_MOVE";

		// Token: 0x04002702 RID: 9986
		private const string _burrowedAttackTrigger = "BurrowAttack";

		// Token: 0x04002703 RID: 9987
		private const string _burrowTrigger = "Burrow";

		// Token: 0x04002704 RID: 9988
		private const string _stunTrigger = "Stun";

		// Token: 0x04002705 RID: 9989
		private const string _wakeUpTrigger = "WakeUp";

		// Token: 0x04002706 RID: 9990
		private const string _burrowSfx = "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_burrow";

		// Token: 0x04002707 RID: 9991
		private const string _hiddenBurrowAttackSfx = "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hidden_attack";

		// Token: 0x04002708 RID: 9992
		private bool _isStunned;
	}
}
