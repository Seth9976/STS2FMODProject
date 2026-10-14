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
	// Token: 0x020007A3 RID: 1955
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Byrdonis : MonsterModel
	{
		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x06005FDA RID: 24538 RVA: 0x0025C2CC File Offset: 0x0025A4CC
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 90, 81);
			}
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06005FDB RID: 24539 RVA: 0x0025C2D8 File Offset: 0x0025A4D8
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 90, 84);
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06005FDC RID: 24540 RVA: 0x0025C2E4 File Offset: 0x0025A4E4
		private static int PeckDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x06005FDD RID: 24541 RVA: 0x0025C2EF File Offset: 0x0025A4EF
		private static int PeckRepeat
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06005FDE RID: 24542 RVA: 0x0025C2FA File Offset: 0x0025A4FA
		private static int SwoopDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
			}
		}

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x06005FDF RID: 24543 RVA: 0x0025C307 File Offset: 0x0025A507
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/byrdonis/byrdonis_die";
			}
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x06005FE0 RID: 24544 RVA: 0x0025C30E File Offset: 0x0025A50E
		public override string TakeDamageSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/byrdonis/byrdonis_hurt";
			}
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0025C318 File Offset: 0x0025A518
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<TerritorialPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0025C35C File Offset: 0x0025A55C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PECK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PeckMove), new AbstractIntent[]
			{
				new MultiAttackIntent(Byrdonis.PeckDamage, Byrdonis.PeckRepeat)
			});
			MoveState moveState2 = new MoveState("SWOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwoopMove), new AbstractIntent[]
			{
				new SingleAttackIntent(Byrdonis.SwoopDamage)
			});
			moveState2.FollowUpState = moveState;
			moveState.FollowUpState = moveState2;
			list.Add(moveState2);
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0025C3EC File Offset: 0x0025A5EC
		private async Task PeckMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(Byrdonis.PeckDamage).WithHitCount(Byrdonis.PeckRepeat).FromMonster(this)
				.WithAttackerAnim("Attack", 0.4f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0025C430 File Offset: 0x0025A630
		private async Task SwoopMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(Byrdonis.SwoopDamage).FromMonster(this).WithAttackerAnim("Attack", 0.4f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0025C474 File Offset: 0x0025A674
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("die", false);
			AnimState animState5 = new AnimState("get_angry", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Angry", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState4, null);
			creatureAnimator.AddAnyState("Hit", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x04002555 RID: 9557
		private const string _angryTrigger = "Angry";
	}
}
