using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
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
	// Token: 0x020007A1 RID: 1953
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BruteRubyRaider : MonsterModel
	{
		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06005FC4 RID: 24516 RVA: 0x0025BE77 File Offset: 0x0025A077
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 31, 30);
			}
		}

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06005FC5 RID: 24517 RVA: 0x0025BE83 File Offset: 0x0025A083
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 34, 33);
			}
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06005FC6 RID: 24518 RVA: 0x0025BE8F File Offset: 0x0025A08F
		private int BeatDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x06005FC7 RID: 24519 RVA: 0x0025BE9A File Offset: 0x0025A09A
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x0025BEA0 File Offset: 0x0025A0A0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BEAT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeatMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BeatDamage)
			});
			MoveState moveState2 = new MoveState("ROAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RoarMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x0025BF28 File Offset: 0x0025A128
		private async Task BeatMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BeatDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x0025BF6C File Offset: 0x0025A16C
		private async Task RoarMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x0025BFAF File Offset: 0x0025A1AF
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "ROAR_MOVE";
		}

		// Token: 0x04002553 RID: 9555
		private const string _roarMove = "ROAR_MOVE";

		// Token: 0x04002554 RID: 9556
		private const int _roarStrength = 3;
	}
}
