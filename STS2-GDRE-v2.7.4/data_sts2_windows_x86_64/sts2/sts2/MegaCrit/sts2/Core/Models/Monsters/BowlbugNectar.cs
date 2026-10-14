using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200079E RID: 1950
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugNectar : MonsterModel
	{
		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06005F98 RID: 24472 RVA: 0x0025B62F File Offset: 0x0025982F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 35);
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06005F99 RID: 24473 RVA: 0x0025B63B File Offset: 0x0025983B
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 39, 38);
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x06005F9A RID: 24474 RVA: 0x0025B647 File Offset: 0x00259847
		private int ThrashDamage
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06005F9B RID: 24475 RVA: 0x0025B64A File Offset: 0x0025984A
		private int BuffStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 15);
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06005F9C RID: 24476 RVA: 0x0025B657 File Offset: 0x00259857
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_goop/workbug_goop_die";
			}
		}

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x0025B65E File Offset: 0x0025985E
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x0025B661 File Offset: 0x00259861
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			skeleton.SetSkin(skeleton.GetData().FindSkin("goop"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x0025B680 File Offset: 0x00259880
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ThrashDamage)
			});
			MoveState moveState2 = new MoveState("BUFF_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BuffMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("THRASH2_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ThrashDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState3;
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x0025B740 File Offset: 0x00259940
		private async Task ThrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThrashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/workbug_goop/workbug_goop_spit", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x0025B784 File Offset: 0x00259984
		private async Task BuffMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Buff", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.BuffStrengthGain, base.Creature, null, false);
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0025B7C8 File Offset: 0x002599C8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("spit", false);
			AnimState animState3 = new AnimState("buff", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Buff", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x04002547 RID: 9543
		private const string _buffTrigger = "Buff";

		// Token: 0x04002548 RID: 9544
		private const string _spitSfx = "event:/sfx/enemy/enemy_attacks/workbug_goop/workbug_goop_spit";

		// Token: 0x04002549 RID: 9545
		private const string _spineSkin = "goop";
	}
}
