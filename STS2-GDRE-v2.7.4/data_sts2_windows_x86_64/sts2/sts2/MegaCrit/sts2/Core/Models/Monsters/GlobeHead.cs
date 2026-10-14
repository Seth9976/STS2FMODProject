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
	// Token: 0x020007C0 RID: 1984
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GlobeHead : MonsterModel
	{
		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x060061A9 RID: 25001 RVA: 0x00261917 File Offset: 0x0025FB17
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 158, 148);
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x00261929 File Offset: 0x0025FB29
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x060061AB RID: 25003 RVA: 0x00261931 File Offset: 0x0025FB31
		private int ThunderStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x060061AC RID: 25004 RVA: 0x0026193C File Offset: 0x0025FB3C
		private int ShockingSlapDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
			}
		}

		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x060061AD RID: 25005 RVA: 0x00261949 File Offset: 0x0025FB49
		private int GalvanicBurstDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16);
			}
		}

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x060061AE RID: 25006 RVA: 0x00261956 File Offset: 0x0025FB56
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x0026195C File Offset: 0x0025FB5C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<GalvanicPower>(new ThrowingPlayerChoiceContext(), base.Creature, 6m, base.Creature, null, false);
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x002619A0 File Offset: 0x0025FBA0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("THUNDER_STRIKE", new Func<IReadOnlyList<Creature>, Task>(this.ThunderStrike), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ThunderStrikeDamage, 3)
			});
			MoveState moveState2 = new MoveState("SHOCKING_SLAP", new Func<IReadOnlyList<Creature>, Task>(this.ShockingSlap), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ShockingSlapDamage),
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("GALVANIC_BURST", new Func<IReadOnlyList<Creature>, Task>(this.GalvanicBurstMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.GalvanicBurstDamage),
				new BuffIntent()
			});
			moveState2.FollowUpState = moveState;
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			list.Add(moveState2);
			list.Add(moveState);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x00261A78 File Offset: 0x0025FC78
		private async Task ThunderStrike(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThunderStrikeDamage).WithHitCount(3).FromMonster(this)
				.WithAttackerAnim("Cast", 0.5f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_charge", null)
				.WithHitFx("vfx/vfx_attack_lightning", null, null)
				.Execute(null);
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x00261ABC File Offset: 0x0025FCBC
		private async Task ShockingSlap(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ShockingSlapDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_slap", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00261B08 File Offset: 0x0025FD08
		private async Task GalvanicBurstMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.GalvanicBurstDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithHitFx("vfx/vfx_attack_lightning", null, "blunt_attack.mp3")
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x00261B4B File Offset: 0x0025FD4B
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "GALVANIC_BURST";
		}

		// Token: 0x040025C6 RID: 9670
		private const string _galvanicBurstMove = "GALVANIC_BURST";

		// Token: 0x040025C7 RID: 9671
		private const string _chargeSfx = "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_charge";

		// Token: 0x040025C8 RID: 9672
		private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_slap";

		// Token: 0x040025C9 RID: 9673
		private const int _thunderStrikeRepeat = 3;

		// Token: 0x040025CA RID: 9674
		private const int _shockingSlapFrail = 2;

		// Token: 0x040025CB RID: 9675
		private const int _galvanicBurstStr = 2;
	}
}
