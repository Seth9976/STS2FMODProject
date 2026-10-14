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
	// Token: 0x020007BC RID: 1980
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FossilStalker : MonsterModel
	{
		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06006164 RID: 24932 RVA: 0x00260C2F File Offset: 0x0025EE2F
		protected override bool HasPhobiaSpineSkin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06006165 RID: 24933 RVA: 0x00260C32 File Offset: 0x0025EE32
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 54, 51);
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x00260C3E File Offset: 0x0025EE3E
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 56, 53);
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06006167 RID: 24935 RVA: 0x00260C4A File Offset: 0x0025EE4A
		private int TackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06006168 RID: 24936 RVA: 0x00260C57 File Offset: 0x0025EE57
		private int LatchDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x06006169 RID: 24937 RVA: 0x00260C64 File Offset: 0x0025EE64
		private int LashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x0600616A RID: 24938 RVA: 0x00260C6F File Offset: 0x0025EE6F
		private int LashRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x0600616B RID: 24939 RVA: 0x00260C72 File Offset: 0x0025EE72
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x0600616C RID: 24940 RVA: 0x00260C75 File Offset: 0x0025EE75
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_hurt";
			}
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00260C7C File Offset: 0x0025EE7C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SuckPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x00260CC0 File Offset: 0x0025EEC0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TackleDamage),
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("LATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LatchMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.LatchDamage)
			});
			MoveState moveState3 = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashAttack), new AbstractIntent[]
			{
				new MultiAttackIntent(this.LashDamage, this.LashRepeat)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState2.FollowUpState = randomBranchState;
			moveState.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState2, 2);
			randomBranchState.AddBranch(moveState, 2);
			randomBranchState.AddBranch(moveState3, 2);
			list.Add(randomBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x00260DC8 File Offset: 0x0025EFC8
		private async Task TackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TackleDamage).FromMonster(this).WithAttackerAnim("Cast", 0.35f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_buff", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x00260E14 File Offset: 0x0025F014
		private async Task LatchMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.LatchDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_single", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00260E58 File Offset: 0x0025F058
		private async Task LashAttack(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.LashDamage).WithHitCount(this.LashRepeat).FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.2f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_double", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x00260E9C File Offset: 0x0025F09C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("attack_double", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("AttackDouble", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x040025B5 RID: 9653
		private const string _attackDoubleTrigger = "AttackDouble";

		// Token: 0x040025B6 RID: 9654
		private const string _attackBuff = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_buff";

		// Token: 0x040025B7 RID: 9655
		private const string _attackDouble = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_double";

		// Token: 0x040025B8 RID: 9656
		private const string _attackSingle = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_single";
	}
}
