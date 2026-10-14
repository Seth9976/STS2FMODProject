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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007EC RID: 2028
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlumberingBeetle : MonsterModel
	{
		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06006479 RID: 25721 RVA: 0x0026A124 File Offset: 0x00268324
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 89, 86);
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x0600647A RID: 25722 RVA: 0x0026A130 File Offset: 0x00268330
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x0600647B RID: 25723 RVA: 0x0026A138 File Offset: 0x00268338
		private int RolloutDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16);
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x0600647C RID: 25724 RVA: 0x0026A145 File Offset: 0x00268345
		private int PlatingAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 15);
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600647D RID: 25725 RVA: 0x0026A151 File Offset: 0x00268351
		// (set) Token: 0x0600647E RID: 25726 RVA: 0x0026A159 File Offset: 0x00268359
		public bool IsAwake
		{
			get
			{
				return this._isAwake;
			}
			set
			{
				base.AssertMutable();
				this._isAwake = value;
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600647F RID: 25727 RVA: 0x0026A168 File Offset: 0x00268368
		// (set) Token: 0x06006480 RID: 25728 RVA: 0x0026A170 File Offset: 0x00268370
		[Nullable(2)]
		private NSleepingVfx SleepingVfx
		{
			[NullableContext(2)]
			get
			{
				return this._sleepingVfx;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._sleepingVfx = value;
			}
		}

		// Token: 0x06006481 RID: 25729 RVA: 0x0026A180 File Offset: 0x00268380
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PlatingAmount, base.Creature, null, false);
			await PowerCmd.Apply<SlumberPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
			NCreature creatureNode = base.Creature.GetCreatureNode();
			Marker2D marker2D = ((creatureNode != null) ? creatureNode.GetSpecialNode<Marker2D>("%SleepVfxPos") : null);
			if (marker2D != null)
			{
				this.SleepingVfx = NSleepingVfx.Create(marker2D.GlobalPosition, true);
				marker2D.AddChildSafely(this.SleepingVfx);
				this.SleepingVfx.Position = Vector2.Zero;
			}
			base.Creature.Died += this.AfterDeath;
		}

		// Token: 0x06006482 RID: 25730 RVA: 0x0026A1C3 File Offset: 0x002683C3
		private void AfterDeath(Creature _)
		{
			base.Creature.Died -= this.AfterDeath;
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
		}

		// Token: 0x06006483 RID: 25731 RVA: 0x0026A1F4 File Offset: 0x002683F4
		public async Task WakeUpMove(IReadOnlyList<Creature> _)
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
			NSleepingVfx sleepingVfx = this.SleepingVfx;
			if (sleepingVfx != null)
			{
				sleepingVfx.Stop();
			}
			this.SleepingVfx = null;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_wake_up", 1f);
			this.IsAwake = true;
			await CreatureCmd.TriggerAnim(base.Creature, "WakeUp", 0.6f);
			if (base.Creature.HasPower<PlatingPower>())
			{
				await PowerCmd.Remove(base.Creature.GetPower<PlatingPower>());
			}
		}

		// Token: 0x06006484 RID: 25732 RVA: 0x0026A238 File Offset: 0x00268438
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SNORE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SnoreMove), new AbstractIntent[]
			{
				new SleepIntent()
			});
			MoveState moveState2 = new MoveState("ROLL_OUT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RolloutMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RolloutDamage),
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SNORE_NEXT");
			moveState.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(moveState, () => base.Creature.HasPower<SlumberPower>());
			conditionalBranchState.AddState(moveState2, () => !base.Creature.HasPower<SlumberPower>());
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(conditionalBranchState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x0026A2FE File Offset: 0x002684FE
		private Task SnoreMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x0026A308 File Offset: 0x00268508
		private async Task RolloutMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature ncreature = null;
				foreach (Creature creature in targets)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null && (ncreature == null || ncreature.GlobalPosition.X > creatureNode.GlobalPosition.X))
					{
						ncreature = creatureNode;
					}
				}
				NCreature creatureNode2 = base.Creature.GetCreatureNode();
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/SpineBoneNode") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					float num = 750f * creatureNode2.Visuals.Scale.X;
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, node2D.GlobalPosition.Y);
				}
			}
			await DamageCmd.Attack(this.RolloutDamage).FromMonster(this).WithAttackerAnim("Rollout", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_roll", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006487 RID: 25735 RVA: 0x0026A354 File Offset: 0x00268554
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("sleep_loop", true);
			AnimState animState2 = new AnimState("wake_up", false);
			AnimState animState3 = new AnimState("idle_loop", true);
			AnimState animState4 = new AnimState("cast", false);
			AnimState animState5 = new AnimState("attack", false);
			AnimState animState6 = new AnimState("roll", false);
			AnimState animState7 = new AnimState("hurt", false);
			AnimState animState8 = new AnimState("die", false);
			animState2.NextState = animState3;
			animState5.NextState = animState3;
			animState4.NextState = animState3;
			animState6.NextState = animState3;
			animState7.NextState = animState3;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("WakeUp", animState2, null);
			creatureAnimator.AddAnyState("Dead", animState8, null);
			creatureAnimator.AddAnyState("Attack", animState5, null);
			creatureAnimator.AddAnyState("Rollout", animState6, null);
			creatureAnimator.AddAnyState("Cast", animState4, null);
			creatureAnimator.AddAnyState("Hit", animState7, () => this.IsAwake);
			return creatureAnimator;
		}

		// Token: 0x04002693 RID: 9875
		public const string wakeUpTrigger = "WakeUp";

		// Token: 0x04002694 RID: 9876
		private const string _rolloutTrigger = "Rollout";

		// Token: 0x04002695 RID: 9877
		public const string rolloutMoveId = "ROLL_OUT_MOVE";

		// Token: 0x04002696 RID: 9878
		private const string _rollSfx = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_roll";

		// Token: 0x04002697 RID: 9879
		public const string wakeUp = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_wake_up";

		// Token: 0x04002698 RID: 9880
		public const string sleepLoop = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop";

		// Token: 0x04002699 RID: 9881
		private bool _isAwake;

		// Token: 0x0400269A RID: 9882
		[Nullable(2)]
		private NSleepingVfx _sleepingVfx;
	}
}
