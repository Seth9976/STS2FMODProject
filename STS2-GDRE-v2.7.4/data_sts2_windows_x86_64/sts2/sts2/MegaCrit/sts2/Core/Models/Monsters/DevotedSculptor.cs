using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B2 RID: 1970
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DevotedSculptor : MonsterModel
	{
		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x060060DC RID: 24796 RVA: 0x0025EDE8 File Offset: 0x0025CFE8
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 172, 162);
			}
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x060060DD RID: 24797 RVA: 0x0025EDFA File Offset: 0x0025CFFA
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x060060DE RID: 24798 RVA: 0x0025EE02 File Offset: 0x0025D002
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x060060DF RID: 24799 RVA: 0x0025EE05 File Offset: 0x0025D005
		private int SavageDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 12);
			}
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x0025EE14 File Offset: 0x0025D014
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("FORBIDDEN_INCANTATION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ForbiddenIncantationMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("SAVAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SavageMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SavageDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x0025EE9C File Offset: 0x0025D09C
		private async Task ForbiddenIncantationMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0f);
			await Cmd.Wait(0.3f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_scream");
			TalkCmd.Play(DevotedSculptor._forbiddenIncantationDialogue, base.Creature, VfxColor.Blue, VfxDuration.Custom);
			await Cmd.CustomScaledWait(0.75f, 1f, false, default(CancellationToken));
			await PowerCmd.Apply<RitualPower>(new ThrowingPlayerChoiceContext(), base.Creature, this._ritualGain, null, null, false);
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x0025EEE0 File Offset: 0x0025D0E0
		private async Task SavageMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SavageDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x04002593 RID: 9619
		private static readonly LocString _forbiddenIncantationDialogue = new LocString("monsters", "DEVOTED_SCULPTOR.moves.FORBIDDEN_INCANTATION.banter");

		// Token: 0x04002594 RID: 9620
		private readonly int _ritualGain = 9;
	}
}
