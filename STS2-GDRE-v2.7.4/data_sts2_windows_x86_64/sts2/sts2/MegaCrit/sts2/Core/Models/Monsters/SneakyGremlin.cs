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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007EE RID: 2030
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SneakyGremlin : MonsterModel
	{
		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x06006498 RID: 25752 RVA: 0x0026A63C File Offset: 0x0026883C
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 11, 10);
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06006499 RID: 25753 RVA: 0x0026A648 File Offset: 0x00268848
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 14);
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x0600649A RID: 25754 RVA: 0x0026A654 File Offset: 0x00268854
		private int TackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x0600649B RID: 25755 RVA: 0x0026A661 File Offset: 0x00268861
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x0600649C RID: 25756 RVA: 0x0026A664 File Offset: 0x00268864
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/gremlin_merc/sneaky_gremlin_attack";
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600649D RID: 25757 RVA: 0x0026A66B File Offset: 0x0026886B
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/gremlin_merc/sneaky_gremlin_die";
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x0600649E RID: 25758 RVA: 0x0026A672 File Offset: 0x00268872
		// (set) Token: 0x0600649F RID: 25759 RVA: 0x0026A67A File Offset: 0x0026887A
		private bool IsAwake
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

		// Token: 0x060064A0 RID: 25760 RVA: 0x0026A68C File Offset: 0x0026888C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			MoveState moveState2 = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TackleDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x0026A714 File Offset: 0x00268914
		private async Task SpawnedMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "WakeUpTrigger", 0.8f);
			this.IsAwake = true;
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x0026A758 File Offset: 0x00268958
		private async Task TackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x0026A79C File Offset: 0x0026899C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("awake_loop", true);
			AnimState animState2 = new AnimState("spawn", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("stunned_loop", true);
			AnimState animState5 = new AnimState("wake_up", false);
			AnimState animState6 = new AnimState("hurt_stunned", false);
			AnimState animState7 = new AnimState("hurt_awake", false);
			AnimState animState8 = new AnimState("die", false);
			animState2.NextState = animState4;
			animState6.NextState = animState4;
			animState7.NextState = animState;
			animState5.NextState = animState;
			animState3.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState2, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("WakeUpTrigger", animState5, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState8, null);
			creatureAnimator.AddAnyState("Hit", animState7, () => this.IsAwake);
			creatureAnimator.AddAnyState("Hit", animState6, () => !this.IsAwake);
			return creatureAnimator;
		}

		// Token: 0x0400269C RID: 9884
		private const string _wakeUpTrigger = "WakeUpTrigger";

		// Token: 0x0400269D RID: 9885
		private bool _isAwake;
	}
}
