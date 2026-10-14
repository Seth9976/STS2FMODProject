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
	// Token: 0x020007F1 RID: 2033
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpectralKnight : MonsterModel
	{
		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x060064D4 RID: 25812 RVA: 0x0026B200 File Offset: 0x00269400
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 97, 93);
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x060064D5 RID: 25813 RVA: 0x0026B20C File Offset: 0x0026940C
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x060064D6 RID: 25814 RVA: 0x0026B214 File Offset: 0x00269414
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x060064D7 RID: 25815 RVA: 0x0026B217 File Offset: 0x00269417
		private int SoulSlashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x060064D8 RID: 25816 RVA: 0x0026B224 File Offset: 0x00269424
		private int SoulFlameDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x0026B230 File Offset: 0x00269430
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("HEX", new Func<IReadOnlyList<Creature>, Task>(this.HexMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("SOUL_SLASH", new Func<IReadOnlyList<Creature>, Task>(this.SoulSlashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SoulSlashDamage)
			});
			MoveState moveState3 = new MoveState("SOUL_FLAME", new Func<IReadOnlyList<Creature>, Task>(this.SoulFlameMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SoulFlameDamage, 3)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState2, 2);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x0026B31C File Offset: 0x0026951C
		private async Task HexMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.3f);
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_hex", 1f);
			foreach (Creature creature in targets)
			{
				await PowerCmd.Apply<HexPower>(new ThrowingPlayerChoiceContext(), creature, 2m, base.Creature, null, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x0026B368 File Offset: 0x00269568
		private async Task SoulSlashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SoulSlashDamage).FromMonster(this).WithAttackerAnim("AttackSword", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_slash", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x0026B3AC File Offset: 0x002695AC
		private async Task SoulFlameMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SoulFlameDamage).WithHitCount(3).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("AttackFlame", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_flame", null)
				.Execute(null);
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x0026B3F0 File Offset: 0x002695F0
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("attack_sword", false);
			AnimState animState4 = new AnimState("attack_flame", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("AttackSword", animState3, null);
			creatureAnimator.AddAnyState("AttackFlame", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x040026AA RID: 9898
		private const int _soulFlameRepeat = 3;

		// Token: 0x040026AB RID: 9899
		private const string _attackFlameTrigger = "AttackFlame";

		// Token: 0x040026AC RID: 9900
		private const string _attackSwordTrigger = "AttackSword";

		// Token: 0x040026AD RID: 9901
		private const string _hexSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_hex";

		// Token: 0x040026AE RID: 9902
		private const string _soulFlameSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_flame";

		// Token: 0x040026AF RID: 9903
		private const string _soulSlashSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_slash";
	}
}
