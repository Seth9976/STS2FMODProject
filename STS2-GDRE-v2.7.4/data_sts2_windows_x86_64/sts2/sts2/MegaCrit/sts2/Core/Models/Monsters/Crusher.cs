using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007AA RID: 1962
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Crusher : MonsterModel
	{
		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600606A RID: 24682 RVA: 0x0025DA00 File Offset: 0x0025BC00
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_die";
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600606B RID: 24683 RVA: 0x0025DA08 File Offset: 0x0025BC08
		public unsafe override IEnumerable<string> AssetPaths
		{
			get
			{
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = SceneHelper.GetScenePath("vfx/monsters/kaiser_crab_boss_explosion");
				List<string> list2 = list;
				return list2.Concat<string>(base.AssetPaths);
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x0600606C RID: 24684 RVA: 0x0025DA4D File Offset: 0x0025BC4D
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x0025DA50 File Offset: 0x0025BC50
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x0025DA53 File Offset: 0x0025BC53
		public override float DeathAnimLengthOverride
		{
			get
			{
				return 2.5f;
			}
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x0600606F RID: 24687 RVA: 0x0025DA5C File Offset: 0x0025BC5C
		[Nullable(2)]
		private NKaiserCrabBossBackground Background
		{
			[NullableContext(2)]
			get
			{
				base.AssertMutable();
				if (this._background == null)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NCombatBackground ncombatBackground;
					if ((ncombatBackground = ((instance != null) ? instance.Background : null)) == null)
					{
						NBestiary instance2 = NBestiary.Instance;
						ncombatBackground = ((instance2 != null) ? instance2.Layout : null);
					}
					Control control = ncombatBackground;
					this._background = ((control != null) ? control.GetNode<NKaiserCrabBossBackground>("%KaiserCrab") : null);
				}
				return this._background;
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x0025DAC0 File Offset: 0x0025BCC0
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 219, 209);
			}
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06006071 RID: 24689 RVA: 0x0025DAD2 File Offset: 0x0025BCD2
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06006072 RID: 24690 RVA: 0x0025DADA File Offset: 0x0025BCDA
		private int ThrashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
			}
		}

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06006073 RID: 24691 RVA: 0x0025DAE7 File Offset: 0x0025BCE7
		private int EnlargingStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x06006074 RID: 24692 RVA: 0x0025DAF2 File Offset: 0x0025BCF2
		private int BugStingDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x06006075 RID: 24693 RVA: 0x0025DAFD File Offset: 0x0025BCFD
		private int BugStingTimes
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0025DB00 File Offset: 0x0025BD00
		private int AdaptStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x06006077 RID: 24695 RVA: 0x0025DB0B File Offset: 0x0025BD0B
		private int GuardedStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
			}
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x0025DB18 File Offset: 0x0025BD18
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ThrashDamage)
			});
			MoveState moveState2 = new MoveState("ENLARGING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnlargingStrikeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.EnlargingStrikeDamage)
			});
			MoveState moveState3 = new MoveState("BUG_STING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BugStingMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.BugStingDamage, this.BugStingTimes),
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("ADAPT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AdaptMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState5 = new MoveState("GUARDED_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GuardedStrikeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.GuardedStrikeDamage),
				new DefendIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006079 RID: 24697 RVA: 0x0025DC68 File Offset: 0x0025BE68
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<BackAttackLeftPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			await PowerCmd.Apply<CrabRagePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.SetVisible(true);
			}
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x0025DCAB File Offset: 0x0025BEAB
		public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
		{
			if (creature != base.Creature || delta >= 0m)
			{
				return Task.CompletedTask;
			}
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.PlayHurtAnim(NKaiserCrabBossBackground.ArmSide.Left);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x0025DCE0 File Offset: 0x0025BEE0
		public override Task BeforeDeath(Creature creature)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NAudioManager.Instance.PlayOneShot(this.DeathSfx, 1f);
			NKaiserCrabBossBackground background = this.Background;
			if (background != null)
			{
				background.PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide.Left);
			}
			if (CombatManager.Instance.IsOverOrEnding)
			{
				NKaiserCrabBossBackground background2 = this.Background;
				if (background2 != null)
				{
					background2.PlayBodyDeathAnim();
				}
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("kaiser_crab_progress", 5f);
				}
			}
			else
			{
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("kaiser_crab_progress", 2f);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0025DD7C File Offset: 0x0025BF7C
		private async Task ThrashMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scoop", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_heavy", 1f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.ThrashDamage).FromMonster(this).WithHitFx("vfx/vfx_attack_blunt", "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_attack_slam", null)
				.Execute(null);
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0025DDC0 File Offset: 0x0025BFC0
		private async Task BugStingMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scissor", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_double", 0.5f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.BugStingDamage).WithHitCount(this.BugStingTimes).FromMonster(this)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x0025DE0C File Offset: 0x0025C00C
		private async Task AdaptMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_buff", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "buff", 0.8f) : null) ?? Task.CompletedTask);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.AdaptStrengthGain, base.Creature, null, false);
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x0025DE50 File Offset: 0x0025C050
		private async Task EnlargingStrikeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_med", 0.65f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.EnlargingStrikeDamage).FromMonster(this).WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(null);
		}

		// Token: 0x06006080 RID: 24704 RVA: 0x0025DE94 File Offset: 0x0025C094
		private async Task GuardedStrikeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam", 1f);
			NKaiserCrabBossBackground background = this.Background;
			await (((background != null) ? background.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_med", 0.65f) : null) ?? Task.CompletedTask);
			await DamageCmd.Attack(this.GuardedStrikeDamage).FromMonster(this).WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, 18m, ValueProp.Move, null, false);
		}

		// Token: 0x04002577 RID: 9591
		public const string deathVfxPath = "vfx/monsters/kaiser_crab_boss_explosion";

		// Token: 0x04002578 RID: 9592
		private const string _attackSlamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam";

		// Token: 0x04002579 RID: 9593
		private const string _attackScoopSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scoop";

		// Token: 0x0400257A RID: 9594
		private const string _attackScissorSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scissor";

		// Token: 0x0400257B RID: 9595
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_buff";

		// Token: 0x0400257C RID: 9596
		private const string _slamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_attack_slam";

		// Token: 0x0400257D RID: 9597
		[Nullable(2)]
		private NKaiserCrabBossBackground _background;
	}
}
