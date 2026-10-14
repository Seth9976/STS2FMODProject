using System;
using System.Collections.Generic;
using System.Linq;
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
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A8 RID: 1960
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorpseSlug : MonsterModel
	{
		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x0600603F RID: 24639 RVA: 0x0025D293 File Offset: 0x0025B493
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 25);
			}
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06006040 RID: 24640 RVA: 0x0025D29F File Offset: 0x0025B49F
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 27);
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06006041 RID: 24641 RVA: 0x0025D2AB File Offset: 0x0025B4AB
		private int WhipSlapDamage
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06006042 RID: 24642 RVA: 0x0025D2AE File Offset: 0x0025B4AE
		private int WhipSlapRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06006043 RID: 24643 RVA: 0x0025D2B1 File Offset: 0x0025B4B1
		private int GlompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06006044 RID: 24644 RVA: 0x0025D2BD File Offset: 0x0025B4BD
		private int GoopFrailAmt
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06006045 RID: 24645 RVA: 0x0025D2C0 File Offset: 0x0025B4C0
		private int RavenousStr
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06006046 RID: 24646 RVA: 0x0025D2CB File Offset: 0x0025B4CB
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack";
			}
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06006047 RID: 24647 RVA: 0x0025D2D2 File Offset: 0x0025B4D2
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_die";
			}
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06006048 RID: 24648 RVA: 0x0025D2D9 File Offset: 0x0025B4D9
		// (set) Token: 0x06006049 RID: 24649 RVA: 0x0025D2E1 File Offset: 0x0025B4E1
		public bool IsRavenous
		{
			get
			{
				return this._isRavenous;
			}
			set
			{
				base.AssertMutable();
				this._isRavenous = value;
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600604A RID: 24650 RVA: 0x0025D2F0 File Offset: 0x0025B4F0
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600604B RID: 24651 RVA: 0x0025D2F3 File Offset: 0x0025B4F3
		// (set) Token: 0x0600604C RID: 24652 RVA: 0x0025D2FB File Offset: 0x0025B4FB
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

		// Token: 0x0600604D RID: 24653 RVA: 0x0025D30C File Offset: 0x0025B50C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<RavenousPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.RavenousStr, base.Creature, null, false);
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x0025D350 File Offset: 0x0025B550
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("WHIP_SLAP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhipSlapMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.WhipSlapDamage, this.WhipSlapRepeat)
			});
			MoveState moveState2 = new MoveState("GLOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GlompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.GlompDamage)
			});
			MoveState moveState3 = new MoveState("GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			int num = this.StarterMoveIdx % 3;
			MoveState moveState4;
			if (num != 0)
			{
				if (num != 1)
				{
					moveState4 = moveState3;
				}
				else
				{
					moveState4 = moveState2;
				}
			}
			else
			{
				moveState4 = moveState;
			}
			MoveState moveState5 = moveState4;
			return new MonsterMoveStateMachine(list, moveState5);
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x0025D440 File Offset: 0x0025B640
		private async Task WhipSlapMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.WhipSlapDamage).WithHitCount(this.WhipSlapRepeat).FromMonster(this)
				.WithAttackerAnim("DoubleAttackTrigger", 0.3f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack_light", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0025D484 File Offset: 0x0025B684
		private async Task GlompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.GlompDamage).FromMonster(this).WithAttackerAnim("HeavyAttackTrigger", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0025D4C8 File Offset: 0x0025B6C8
		private async Task GoopMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Attack", 0.2f);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, this.GoopFrailAmt, base.Creature, null, false);
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0025D514 File Offset: 0x0025B714
		public static void EnsureCorpseSlugsStartWithDifferentMoves(IEnumerable<MonsterModel> monsters, Rng rng)
		{
			IEnumerable<CorpseSlug> enumerable = monsters.OfType<CorpseSlug>();
			int num = rng.NextInt(3);
			foreach (CorpseSlug corpseSlug in enumerable)
			{
				corpseSlug.StarterMoveIdx = num % 3;
				num++;
			}
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0025D574 File Offset: 0x0025B774
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("attack_heavy", false);
			AnimState animState4 = new AnimState("attack_double", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("devour_loop", true);
			AnimState animState8 = new AnimState("devour_start", false);
			AnimState animState9 = new AnimState("devour_end", false);
			AnimState animState10 = new AnimState("hurt_devouring", false);
			AnimState animState11 = new AnimState("die_devouring", false);
			animState3.NextState = animState;
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState8.NextState = animState7;
			animState9.NextState = animState;
			animState10.NextState = animState7;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("HeavyAttackTrigger", animState3, null);
			creatureAnimator.AddAnyState("DoubleAttackTrigger", animState4, null);
			creatureAnimator.AddAnyState("DevourStartTrigger", animState8, () => !this._isRavenous);
			creatureAnimator.AddAnyState("DevourEndkTrigger", animState9, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Dead", animState6, () => !this._isRavenous);
			creatureAnimator.AddAnyState("Hit", animState5, () => !this._isRavenous);
			creatureAnimator.AddAnyState("Dead", animState11, () => this._isRavenous);
			creatureAnimator.AddAnyState("Hit", animState10, () => this._isRavenous);
			return creatureAnimator;
		}

		// Token: 0x0400256B RID: 9579
		private const string _heavyAttackTrigger = "HeavyAttackTrigger";

		// Token: 0x0400256C RID: 9580
		private const string _doubleAttackTrigger = "DoubleAttackTrigger";

		// Token: 0x0400256D RID: 9581
		public const string devourStartTrigger = "DevourStartTrigger";

		// Token: 0x0400256E RID: 9582
		public const string devourEndTrigger = "DevourEndkTrigger";

		// Token: 0x0400256F RID: 9583
		private const string _attackLightSfx = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack_light";

		// Token: 0x04002570 RID: 9584
		public const string ravenousSfx = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous";

		// Token: 0x04002571 RID: 9585
		public const string ravenousUpSfxDouble = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous_up_double";

		// Token: 0x04002572 RID: 9586
		private bool _isRavenous;

		// Token: 0x04002573 RID: 9587
		private int _starterMoveIdx;
	}
}
