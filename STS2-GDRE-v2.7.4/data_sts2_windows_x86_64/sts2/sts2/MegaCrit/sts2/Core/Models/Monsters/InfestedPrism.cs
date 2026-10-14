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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C5 RID: 1989
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfestedPrism : MonsterModel
	{
		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x060061EA RID: 25066 RVA: 0x002624FB File Offset: 0x002606FB
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_die";
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x060061EB RID: 25067 RVA: 0x00262502 File Offset: 0x00260702
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack";
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x060061EC RID: 25068 RVA: 0x00262509 File Offset: 0x00260709
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 171, 161);
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x060061ED RID: 25069 RVA: 0x0026251B File Offset: 0x0026071B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x060061EE RID: 25070 RVA: 0x00262523 File Offset: 0x00260723
		private int JabDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
			}
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x060061EF RID: 25071 RVA: 0x00262530 File Offset: 0x00260730
		private int VitalSparkAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x060061F0 RID: 25072 RVA: 0x0026253B File Offset: 0x0026073B
		private int PulsateDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 8);
			}
		}

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x060061F1 RID: 25073 RVA: 0x00262547 File Offset: 0x00260747
		private int PulsateBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 20);
			}
		}

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x060061F2 RID: 25074 RVA: 0x00262553 File Offset: 0x00260753
		private int RadiateDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
			}
		}

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x060061F3 RID: 25075 RVA: 0x00262560 File Offset: 0x00260760
		private int RadiateBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
			}
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x060061F4 RID: 25076 RVA: 0x0026256D File Offset: 0x0026076D
		private int WhirlwindDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x060061F5 RID: 25077 RVA: 0x00262578 File Offset: 0x00260778
		private int WhirlwindRepeat
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x060061F6 RID: 25078 RVA: 0x0026257B File Offset: 0x0026077B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x00262580 File Offset: 0x00260780
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<VitalSparkPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.VitalSparkAmount, base.Creature, null, false);
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x002625C4 File Offset: 0x002607C4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("JAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.JabMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.JabDamage)
			});
			MoveState moveState2 = new MoveState("RADIATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RadiateMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RadiateDamage),
				new DefendIntent()
			});
			MoveState moveState3 = new MoveState("WHIRLWIND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlwindMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.WhirlwindDamage, this.WhirlwindRepeat)
			});
			MoveState moveState4 = new MoveState("PULSATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PulsateMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PulsateDamage),
				new BuffIntent(),
				new DefendIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x002626E4 File Offset: 0x002608E4
		private async Task JabMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.JabDamage).FromMonster(this).WithAttackerAnim("Attack", 0.1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x00262728 File Offset: 0x00260928
		private async Task RadiateMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.RadiateDamage).FromMonster(this).WithAttackerAnim("AttackBlock", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.RadiateBlock, ValueProp.Move, null, false);
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x0026276C File Offset: 0x0026096C
		private async Task WhirlwindMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.WhirlwindDamage).WithHitCount(this.WhirlwindRepeat).FromMonster(this)
				.WithAttackerAnim("AttackDouble", 0.2f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_spin", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x002627B0 File Offset: 0x002609B0
		private async Task PulsateMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PulsateDamage).FromMonster(this).WithAttackerAnim("AttackBlock", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.PulsateBlock, ValueProp.Move, null, false);
			await PowerCmd.Apply<VitalSparkPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.VitalSparkAmount, base.Creature, null, false);
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x002627F4 File Offset: 0x002609F4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_block", false);
			AnimState animState5 = new AnimState("attack_double", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState6.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			creatureAnimator.AddAnyState("AttackBlock", animState4, null);
			creatureAnimator.AddAnyState("AttackDouble", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x040025D2 RID: 9682
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_buff";

		// Token: 0x040025D3 RID: 9683
		private const string _attackDefendSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend";

		// Token: 0x040025D4 RID: 9684
		private const string _attackSpinSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_spin";

		// Token: 0x040025D5 RID: 9685
		private const string _attackBlockTrigger = "AttackBlock";

		// Token: 0x040025D6 RID: 9686
		private const string _attackDoubleTrigger = "AttackDouble";
	}
}
