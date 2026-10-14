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
	// Token: 0x02000802 RID: 2050
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TrackerRubyRaider : MonsterModel
	{
		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x06006627 RID: 26151 RVA: 0x0026F084 File Offset: 0x0026D284
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
			}
		}

		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x0026F090 File Offset: 0x0026D290
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 26, 25);
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x0026F09C File Offset: 0x0026D29C
		private int HoundsDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
			}
		}

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x0600662A RID: 26154 RVA: 0x0026F0A7 File Offset: 0x0026D2A7
		private int HoundsRepeat
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x0600662B RID: 26155 RVA: 0x0026F0B3 File Offset: 0x0026D2B3
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x0026F0B8 File Offset: 0x0026D2B8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TRACK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TrackMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("HOUNDS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HoundsMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.HoundsDamage, this.HoundsRepeat)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x0026F148 File Offset: 0x0026D348
		private async Task TrackMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Attack", 0.8f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_attack_slash");
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x0026F194 File Offset: 0x0026D394
		private async Task HoundsMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HoundsDamage).WithHitCount(this.HoundsRepeat).FromMonster(this)
				.WithAttackerAnim("Attack", 0.5f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x0026F1D7 File Offset: 0x0026D3D7
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "TRACK_MOVE";
		}

		// Token: 0x040026FF RID: 9983
		private const string _trackMove = "TRACK_MOVE";
	}
}
