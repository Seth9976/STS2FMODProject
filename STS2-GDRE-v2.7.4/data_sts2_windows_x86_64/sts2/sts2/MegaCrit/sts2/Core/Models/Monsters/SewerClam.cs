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
	// Token: 0x020007E5 RID: 2021
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SewerClam : MonsterModel
	{
		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x06006423 RID: 25635 RVA: 0x00268FF7 File Offset: 0x002671F7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 58, 56);
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x06006424 RID: 25636 RVA: 0x00269003 File Offset: 0x00267203
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x06006425 RID: 25637 RVA: 0x0026900B File Offset: 0x0026720B
		private int JetDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06006426 RID: 25638 RVA: 0x00269018 File Offset: 0x00267218
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x0026901C File Offset: 0x0026721C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			int valueIfAscension = AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 8);
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, valueIfAscension, base.Creature, null, false);
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x00269060 File Offset: 0x00267260
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PRESSURIZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressurizeMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("JET_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.JetMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.JetDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x002690E8 File Offset: 0x002672E8
		private async Task PressurizeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/sewer_clam/sewer_clam_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 1f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 4m, base.Creature, null, false);
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x0026912C File Offset: 0x0026732C
		private async Task JetMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.JetDamage).FromMonster(this).WithAttackerAnim("Attack", 0.45f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x00269170 File Offset: 0x00267370
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x0400267D RID: 9853
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/sewer_clam/sewer_clam_buff";
	}
}
