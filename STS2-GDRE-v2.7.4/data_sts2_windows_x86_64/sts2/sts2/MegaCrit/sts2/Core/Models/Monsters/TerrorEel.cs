using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
	// Token: 0x020007F5 RID: 2037
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TerrorEel : MonsterModel
	{
		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06006501 RID: 25857 RVA: 0x0026B9CC File Offset: 0x00269BCC
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 150, 140);
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06006502 RID: 25858 RVA: 0x0026B9DE File Offset: 0x00269BDE
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06006503 RID: 25859 RVA: 0x0026B9E6 File Offset: 0x00269BE6
		private int ShriekAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 75, 70);
			}
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x06006504 RID: 25860 RVA: 0x0026B9F2 File Offset: 0x00269BF2
		private int CrashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16);
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x06006505 RID: 25861 RVA: 0x0026B9FF File Offset: 0x00269BFF
		private int ThrashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x06006506 RID: 25862 RVA: 0x0026BA0A File Offset: 0x00269C0A
		private int ThrashRepeat
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x06006507 RID: 25863 RVA: 0x0026BA0D File Offset: 0x00269C0D
		// (set) Token: 0x06006508 RID: 25864 RVA: 0x0026BA15 File Offset: 0x00269C15
		public MoveState TerrorState
		{
			get
			{
				return this._terrorState;
			}
			private set
			{
				base.AssertMutable();
				this._terrorState = value;
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x06006509 RID: 25865 RVA: 0x0026BA24 File Offset: 0x00269C24
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x0026BA28 File Offset: 0x00269C28
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ShriekPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.ShriekAmount, base.Creature, null, false);
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x0026BA6C File Offset: 0x00269C6C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.CrashDamage)
			});
			MoveState moveState2 = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ThrashDamage, this.ThrashRepeat),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("STUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StunMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			this.TerrorState = new MoveState("TERROR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TerrorMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			moveState3.FollowUpState = this.TerrorState;
			this.TerrorState.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(this.TerrorState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x0026BB84 File Offset: 0x00269D84
		private async Task CrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.CrashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x0026BBC8 File Offset: 0x00269DC8
		private async Task ThrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThrashDamage).WithHitCount(this.ThrashRepeat).FromMonster(this)
				.WithAttackerAnim("AttackTripleTrigger", 0.25f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_attack_multi", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), base.Creature, 6m, base.Creature, null, false);
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x0026BC0B File Offset: 0x00269E0B
		private Task StunMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x0026BC14 File Offset: 0x00269E14
		private async Task TerrorMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_debuff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0f);
			await Cmd.Wait(0.7f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_scream");
			await Cmd.CustomScaledWait(0.1f, 0.3f, false, default(CancellationToken));
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 99m, base.Creature, null, false);
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x0026BC60 File Offset: 0x00269E60
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_triple", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("AttackTripleTrigger", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x0026BD27 File Offset: 0x00269F27
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "STUN_MOVE";
		}

		// Token: 0x040026B5 RID: 9909
		private const string _stunMove = "STUN_MOVE";

		// Token: 0x040026B6 RID: 9910
		private const string _debuffSfx = "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_debuff";

		// Token: 0x040026B7 RID: 9911
		private const string _attackMultiSfx = "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_attack_multi";

		// Token: 0x040026B8 RID: 9912
		private const string _thrashMoveId = "THRASH_MOVE";

		// Token: 0x040026B9 RID: 9913
		private const int _hpNormal = 140;

		// Token: 0x040026BA RID: 9914
		private const int _hpTough = 150;

		// Token: 0x040026BB RID: 9915
		private MoveState _terrorState;

		// Token: 0x040026BC RID: 9916
		private const string _attackTripleTrigger = "AttackTripleTrigger";
	}
}
