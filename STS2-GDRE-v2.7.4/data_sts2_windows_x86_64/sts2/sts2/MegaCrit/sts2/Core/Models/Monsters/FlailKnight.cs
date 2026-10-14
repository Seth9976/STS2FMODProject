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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B9 RID: 1977
	[NullableContext(1)]
	[Nullable(0)]
	public class FlailKnight : MonsterModel
	{
		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x06006144 RID: 24900 RVA: 0x002603FA File Offset: 0x0025E5FA
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 108, 101);
			}
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x06006145 RID: 24901 RVA: 0x00260406 File Offset: 0x0025E606
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06006146 RID: 24902 RVA: 0x0026040E File Offset: 0x0025E60E
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06006147 RID: 24903 RVA: 0x00260411 File Offset: 0x0025E611
		private int FlailDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06006148 RID: 24904 RVA: 0x0026041E File Offset: 0x0025E61E
		private int RamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x0026042C File Offset: 0x0025E62C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("WAR_CHANT", new Func<IReadOnlyList<Creature>, Task>(this.WarChantMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("FLAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlailMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.FlailDamage, 2)
			});
			MoveState moveState3 = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RamDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, 2);
			randomBranchState.AddBranch(moveState3, 2);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x00260520 File Offset: 0x0025E720
		private async Task WarChantMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_war_chant", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x00260564 File Offset: 0x0025E764
		public async Task FlailMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FlailDamage).WithHitCount(2).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("FlailAttack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_flail", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x002605A8 File Offset: 0x0025E7A8
		private async Task RamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.RamDamage).FromMonster(this).WithAttackerAnim("RamAttack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_ram", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x002605EC File Offset: 0x0025E7EC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack_flail", false);
			AnimState animState4 = new AnimState("attack_ram", false);
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
			creatureAnimator.AddAnyState("FlailAttack", animState3, null);
			creatureAnimator.AddAnyState("RamAttack", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x040025AB RID: 9643
		private const string _flailAttackTrigger = "FlailAttack";

		// Token: 0x040025AC RID: 9644
		private const string _ramAttackTrigger = "RamAttack";

		// Token: 0x040025AD RID: 9645
		private const string _flailSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_flail";

		// Token: 0x040025AE RID: 9646
		private const string _chantSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_war_chant";

		// Token: 0x040025AF RID: 9647
		private const string _ramSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_ram";

		// Token: 0x040025B0 RID: 9648
		private const int _flailRepeat = 2;
	}
}
