using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000809 RID: 2057
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VineShambler : MonsterModel
	{
		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x06006696 RID: 26262 RVA: 0x0027066F File Offset: 0x0026E86F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64, 61);
			}
		}

		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x06006697 RID: 26263 RVA: 0x0027067B File Offset: 0x0026E87B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x06006698 RID: 26264 RVA: 0x00270683 File Offset: 0x0026E883
		private int GraspingVinesDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x06006699 RID: 26265 RVA: 0x0027068F File Offset: 0x0026E88F
		private int SwipeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600669A RID: 26266 RVA: 0x0027069A File Offset: 0x0026E89A
		private int ChompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16);
			}
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x002706A8 File Offset: 0x0026E8A8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("GRASPING_VINES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GraspingVinesMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.GraspingVinesDamage),
				new CardDebuffIntent()
			});
			MoveState moveState2 = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SwipeDamage, 2)
			});
			MoveState moveState3 = new MoveState("CHOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ChompDamage)
			});
			moveState2.FollowUpState = moveState;
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x00270778 File Offset: 0x0026E978
		private async Task GraspingVinesMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.GraspingVinesDamage).FromMonster(this).WithAttackerAnim("Vines", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_cast", null)
				.WithHitFx("vfx/monsters/vine_shambler_vines/vine_shambler_vines_vfx", null, null)
				.SpawningHitVfxOnEachCreature()
				.WithHitVfxSpawnedAtBase()
				.Execute(null);
			await PowerCmd.Apply<TangledPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x0600669D RID: 26269 RVA: 0x002707C4 File Offset: 0x0026E9C4
		private async Task SwipeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SwipeDamage).WithHitCount(2).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("SwipePower", 0.4f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_defensive_swipe", null)
				.WithHitFx("vfx/vfx_scratch", null, null)
				.Execute(null);
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x00270808 File Offset: 0x0026EA08
		private async Task ChompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ChompDamage).FromMonster(this).WithAttackerAnim("Chomp", 0.4f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_chomp", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x0027084C File Offset: 0x0026EA4C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack_chomp", false);
			AnimState animState4 = new AnimState("attack_swipe", false);
			AnimState animState5 = new AnimState("attack_vines", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Chomp", animState3, null);
			creatureAnimator.AddAnyState("SwipePower", animState4, null);
			creatureAnimator.AddAnyState("Vines", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x04002729 RID: 10025
		private const string _vineShamblerVfxPath = "vfx/monsters/vine_shambler_vines/vine_shambler_vines_vfx";

		// Token: 0x0400272A RID: 10026
		private const int _swipeRepeat = 2;

		// Token: 0x0400272B RID: 10027
		private const string _swipeTrigger = "SwipePower";

		// Token: 0x0400272C RID: 10028
		private const string _vinesTrigger = "Vines";

		// Token: 0x0400272D RID: 10029
		private const string _chompTrigger = "Chomp";

		// Token: 0x0400272E RID: 10030
		private const string _chomp = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_chomp";

		// Token: 0x0400272F RID: 10031
		private const string _defensiveSwipe = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_defensive_swipe";

		// Token: 0x04002730 RID: 10032
		private const string _graspingVines = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_cast";
	}
}
