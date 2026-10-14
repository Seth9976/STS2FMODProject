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
	// Token: 0x02000797 RID: 1943
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Axebot : MonsterModel
	{
		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x06005F51 RID: 24401 RVA: 0x0025A9F3 File Offset: 0x00258BF3
		private int BootUpBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 10);
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x06005F52 RID: 24402 RVA: 0x0025AA00 File Offset: 0x00258C00
		private int OneTwoDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06005F53 RID: 24403 RVA: 0x0025AA0D File Offset: 0x00258C0D
		private int BootUpStrGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06005F54 RID: 24404 RVA: 0x0025AA18 File Offset: 0x00258C18
		private int HammerUppercutDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
			}
		}

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x06005F55 RID: 24405 RVA: 0x0025AA25 File Offset: 0x00258C25
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 76, 70);
			}
		}

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x06005F56 RID: 24406 RVA: 0x0025AA31 File Offset: 0x00258C31
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 86, 78);
			}
		}

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x06005F57 RID: 24407 RVA: 0x0025AA3D File Offset: 0x00258C3D
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x06005F58 RID: 24408 RVA: 0x0025AA40 File Offset: 0x00258C40
		// (set) Token: 0x06005F59 RID: 24409 RVA: 0x0025AA4E File Offset: 0x00258C4E
		public int StockAmount
		{
			get
			{
				return this._stockOverrideAmount.GetValueOrDefault(2);
			}
			set
			{
				base.AssertMutable();
				this._stockOverrideAmount = new int?(value);
			}
		}

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x06005F5A RID: 24410 RVA: 0x0025AA62 File Offset: 0x00258C62
		// (set) Token: 0x06005F5B RID: 24411 RVA: 0x0025AA6A File Offset: 0x00258C6A
		public bool ShouldPlaySpawnAnimation
		{
			get
			{
				return this._shouldPlaySpawnAnimation;
			}
			set
			{
				base.AssertMutable();
				this._shouldPlaySpawnAnimation = value;
			}
		}

		// Token: 0x06005F5C RID: 24412 RVA: 0x0025AA7C File Offset: 0x00258C7C
		public override async Task AfterAddedToRoom()
		{
			if (this.StockAmount > 0)
			{
				await PowerCmd.Apply<StockPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.StockAmount, null, null, false);
			}
		}

		// Token: 0x06005F5D RID: 24413 RVA: 0x0025AAC0 File Offset: 0x00258CC0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BOOT_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BootUpMove), new AbstractIntent[]
			{
				new DefendIntent(),
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("ONE_TWO_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OneTwoMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.OneTwoDamage, 2)
			});
			MoveState moveState3 = new MoveState("HAMMER_UPPERCUT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HammerUppercutMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.HammerUppercutDamage),
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			if (this._stockOverrideAmount != null)
			{
				return new MonsterMoveStateMachine(list, moveState);
			}
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x0025ABA8 File Offset: 0x00258DA8
		private async Task BootUpMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/axebot/axebot_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "sharpen", 0.3f);
			await Cmd.Wait(0.25f, false);
			await CreatureCmd.GainBlock(base.Creature, this.BootUpBlock, ValueProp.Move, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.BootUpStrGain * (2 - this.StockAmount), base.Creature, null, false);
		}

		// Token: 0x06005F5F RID: 24415 RVA: 0x0025ABEC File Offset: 0x00258DEC
		private async Task OneTwoMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.OneTwoDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0.35f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.OnlyPlayAnimOnce()
				.Execute(null);
		}

		// Token: 0x06005F60 RID: 24416 RVA: 0x0025AC30 File Offset: 0x00258E30
		private async Task HammerUppercutMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HammerUppercutDamage).FromMonster(this).WithAttackerAnim("uppercut", 0.8f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/axebot/axebot_attack_spin", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x06005F61 RID: 24417 RVA: 0x0025AC7C File Offset: 0x00258E7C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("special", false);
			AnimState animState4 = new AnimState("sharpen", false);
			AnimState animState5 = new AnimState("respawn", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(this._shouldPlaySpawnAnimation ? animState5 : animState, controller);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("uppercut", animState3, null);
			creatureAnimator.AddAnyState("sharpen", animState4, null);
			creatureAnimator.AddAnyState("respawn", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x0400253D RID: 9533
		private const int _oneTwoRepeat = 2;

		// Token: 0x0400253E RID: 9534
		private const string _hammerUppercutTrigger = "uppercut";

		// Token: 0x0400253F RID: 9535
		private const string _sharpenTrigger = "sharpen";

		// Token: 0x04002540 RID: 9536
		public const string respawnTrigger = "respawn";

		// Token: 0x04002541 RID: 9537
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/axebot/axebot_buff";

		// Token: 0x04002542 RID: 9538
		private const string _spinSfx = "event:/sfx/enemy/enemy_attacks/axebot/axebot_attack_spin";

		// Token: 0x04002543 RID: 9539
		private int? _stockOverrideAmount;

		// Token: 0x04002544 RID: 9540
		private bool _shouldPlaySpawnAnimation;
	}
}
