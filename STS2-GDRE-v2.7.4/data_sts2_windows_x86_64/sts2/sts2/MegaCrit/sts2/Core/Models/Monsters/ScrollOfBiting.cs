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
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E3 RID: 2019
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScrollOfBiting : MonsterModel
	{
		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x060063FF RID: 25599 RVA: 0x00268947 File Offset: 0x00266B47
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 33, 30);
			}
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x06006400 RID: 25600 RVA: 0x00268953 File Offset: 0x00266B53
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 39, 37);
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x06006401 RID: 25601 RVA: 0x0026895F File Offset: 0x00266B5F
		private int ChompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06006402 RID: 25602 RVA: 0x0026896C File Offset: 0x00266B6C
		private int ChewDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06006403 RID: 25603 RVA: 0x00268977 File Offset: 0x00266B77
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06006404 RID: 25604 RVA: 0x0026897A File Offset: 0x00266B7A
		public override bool HasDeathSfx
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x06006405 RID: 25605 RVA: 0x0026897D File Offset: 0x00266B7D
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_die";
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x06006406 RID: 25606 RVA: 0x00268984 File Offset: 0x00266B84
		// (set) Token: 0x06006407 RID: 25607 RVA: 0x0026898C File Offset: 0x00266B8C
		public int StarterMoveIdx
		{
			get
			{
				return this._starterMoveIdx;
			}
			set
			{
				base.AssertMutable();
				this._starterMoveIdx = value;
			}
		}

		// Token: 0x06006408 RID: 25608 RVA: 0x0026899C File Offset: 0x00266B9C
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(ScrollOfBiting._skinOptions)));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06006409 RID: 25609 RVA: 0x002689E4 File Offset: 0x00266BE4
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<PaperCutsPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x0600640A RID: 25610 RVA: 0x00268A28 File Offset: 0x00266C28
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CHOMP", new Func<IReadOnlyList<Creature>, Task>(this.ChompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ChompDamage)
			});
			MoveState moveState2 = new MoveState("CHEW", new Func<IReadOnlyList<Creature>, Task>(this.ChewState), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ChewDamage, 2)
			});
			MoveState moveState3 = new MoveState("MORE_TEETH", new Func<IReadOnlyList<Creature>, Task>(this.MoreTeethMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("rand");
			moveState.FollowUpState = moveState3;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = moveState2;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, 2);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			int num = this.StarterMoveIdx % 3;
			if (num == 0)
			{
				return new MonsterMoveStateMachine(list, moveState);
			}
			if (num != 1)
			{
				return new MonsterMoveStateMachine(list, moveState3);
			}
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x00268B34 File Offset: 0x00266D34
		private async Task ChompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ChompDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x00268B78 File Offset: 0x00266D78
		private async Task ChewState(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ChewDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("ATTACK_DOUBLE", 0.2f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite_double", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x00268BBC File Offset: 0x00266DBC
		private async Task MoreTeethMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.8f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x00268C00 File Offset: 0x00266E00
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_double", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("ATTACK_DOUBLE", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002671 RID: 9841
		private static readonly string[] _skinOptions = new string[] { "skin1", "skin2" };

		// Token: 0x04002672 RID: 9842
		private const int _chewRepeat = 2;

		// Token: 0x04002673 RID: 9843
		private const int _buffAmt = 2;

		// Token: 0x04002674 RID: 9844
		private int _starterMoveIdx;

		// Token: 0x04002675 RID: 9845
		private const string _attackDoubleTrigger = "ATTACK_DOUBLE";

		// Token: 0x04002676 RID: 9846
		public const string biteSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite";

		// Token: 0x04002677 RID: 9847
		public const string biteDoubleSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite_double";

		// Token: 0x04002678 RID: 9848
		public const string buffSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_buff";
	}
}
