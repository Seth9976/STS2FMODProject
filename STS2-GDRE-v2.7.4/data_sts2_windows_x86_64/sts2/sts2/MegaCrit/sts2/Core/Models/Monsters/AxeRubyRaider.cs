using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000798 RID: 1944
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AxeRubyRaider : MonsterModel
	{
		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x0025AD7B File Offset: 0x00258F7B
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 21, 20);
			}
		}

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x06005F64 RID: 24420 RVA: 0x0025AD87 File Offset: 0x00258F87
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 23, 22);
			}
		}

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x06005F65 RID: 24421 RVA: 0x0025AD93 File Offset: 0x00258F93
		private int SwingDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x06005F66 RID: 24422 RVA: 0x0025AD9E File Offset: 0x00258F9E
		private int SwingBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x06005F67 RID: 24423 RVA: 0x0025ADA9 File Offset: 0x00258FA9
		private int BigSwingDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
			}
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06005F68 RID: 24424 RVA: 0x0025ADB6 File Offset: 0x00258FB6
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x0025ADBC File Offset: 0x00258FBC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SWING_1", new Func<IReadOnlyList<Creature>, Task>(this.SwingMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwingDamage),
				new DefendIntent()
			});
			MoveState moveState2 = new MoveState("SWING_2", new Func<IReadOnlyList<Creature>, Task>(this.SwingMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwingDamage),
				new DefendIntent()
			});
			MoveState moveState3 = new MoveState("BIG_SWING", new Func<IReadOnlyList<Creature>, Task>(this.BigSwingMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BigSwingDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x0025AE94 File Offset: 0x00259094
		private async Task SwingMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SwingDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.SwingBlock, ValueProp.Move, null, false);
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x0025AED8 File Offset: 0x002590D8
		private async Task BigSwingMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BigSwingDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x0025AF1B File Offset: 0x0025911B
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SWING_1" && moveStateId != "SWING_2";
		}

		// Token: 0x04002545 RID: 9541
		private const string _swing1Move = "SWING_1";

		// Token: 0x04002546 RID: 9542
		private const string _swing2Move = "SWING_2";
	}
}
