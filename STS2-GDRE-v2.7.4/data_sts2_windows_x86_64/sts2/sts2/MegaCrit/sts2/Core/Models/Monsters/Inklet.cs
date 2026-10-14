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
	// Token: 0x020007C6 RID: 1990
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Inklet : MonsterModel
	{
		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06006200 RID: 25088 RVA: 0x002628EF File Offset: 0x00260AEF
		public override float HurtAnimationTrackOffsetForDoom
		{
			get
			{
				return 0.04f;
			}
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06006201 RID: 25089 RVA: 0x002628F6 File Offset: 0x00260AF6
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 11);
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06006202 RID: 25090 RVA: 0x00262902 File Offset: 0x00260B02
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x06006203 RID: 25091 RVA: 0x0026290E File Offset: 0x00260B0E
		private int JabDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x06006204 RID: 25092 RVA: 0x00262919 File Offset: 0x00260B19
		private int WhirlwindDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x06006205 RID: 25093 RVA: 0x00262924 File Offset: 0x00260B24
		private int PiercingGazeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x06006206 RID: 25094 RVA: 0x00262931 File Offset: 0x00260B31
		// (set) Token: 0x06006207 RID: 25095 RVA: 0x00262939 File Offset: 0x00260B39
		public bool MiddleInklet
		{
			get
			{
				return this._middleInklet;
			}
			set
			{
				base.AssertMutable();
				this._middleInklet = value;
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x06006208 RID: 25096 RVA: 0x00262948 File Offset: 0x00260B48
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x06006209 RID: 25097 RVA: 0x0026294B File Offset: 0x00260B4B
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/inklet/inklet_hurt";
			}
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x00262954 File Offset: 0x00260B54
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SlipperyPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x00262998 File Offset: 0x00260B98
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("JAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.JabMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.JabDamage)
			});
			MoveState moveState2 = new MoveState("WHIRLWIND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlwindMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.WhirlwindDamage, 3)
			});
			MoveState moveState3 = new MoveState("PIERCING_GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingGazeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PiercingGazeDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("INIT_RAND");
			RandomBranchState randomBranchState2 = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState2;
			moveState3.FollowUpState = randomBranchState2;
			moveState2.FollowUpState = randomBranchState2;
			randomBranchState.AddBranch(moveState, 2, 1f);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState2.AddBranch(moveState3, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState2.AddBranch(moveState2, MoveRepeatType.CannotRepeat, 1f);
			moveState.FollowUpState = randomBranchState2;
			moveState2.FollowUpState = moveState;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState3);
			list.Add(moveState2);
			list.Add(randomBranchState2);
			MoveState moveState4 = (this._middleInklet ? moveState2 : moveState);
			return new MonsterMoveStateMachine(list, moveState4);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x00262AE0 File Offset: 0x00260CE0
		private async Task JabMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.JabDamage).FromMonster(this).WithAttackerAnim("Attack", 0.75f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x00262B24 File Offset: 0x00260D24
		private async Task WhirlwindMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.WhirlwindDamage).WithHitCount(3).FromMonster(this)
				.WithAttackerAnim("TRIPLE_ATTACK", 0.3f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/inklet/inklet_attack_triple", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x00262B68 File Offset: 0x00260D68
		private async Task PiercingGazeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PiercingGazeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.75f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x00262BAC File Offset: 0x00260DAC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_fast", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("TRIPLE_ATTACK", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x00262C73 File Offset: 0x00260E73
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "PIERCING_GAZE_MOVE";
		}

		// Token: 0x040025D7 RID: 9687
		private const string _piercingGazeMove = "PIERCING_GAZE_MOVE";

		// Token: 0x040025D8 RID: 9688
		private const string _attackTripleTrigger = "TRIPLE_ATTACK";

		// Token: 0x040025D9 RID: 9689
		private const int _whirlwindRepeat = 3;

		// Token: 0x040025DA RID: 9690
		private bool _middleInklet;

		// Token: 0x040025DB RID: 9691
		private const string _attackTripleSfx = "event:/sfx/enemy/enemy_attacks/inklet/inklet_attack_triple";
	}
}
