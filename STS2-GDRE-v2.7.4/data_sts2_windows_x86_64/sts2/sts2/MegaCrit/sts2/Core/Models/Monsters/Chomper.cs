using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A7 RID: 1959
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Chomper : MonsterModel
	{
		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06006033 RID: 24627 RVA: 0x0025D0D7 File Offset: 0x0025B2D7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 63, 60);
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x0025D0E3 File Offset: 0x0025B2E3
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 67, 64);
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x06006035 RID: 24629 RVA: 0x0025D0EF File Offset: 0x0025B2EF
		private static int ClampDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x06006036 RID: 24630 RVA: 0x0025D0FB File Offset: 0x0025B2FB
		// (set) Token: 0x06006037 RID: 24631 RVA: 0x0025D103 File Offset: 0x0025B303
		public bool ScreamFirst
		{
			get
			{
				return this._screamFirst;
			}
			set
			{
				base.AssertMutable();
				this._screamFirst = value;
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x06006038 RID: 24632 RVA: 0x0025D112 File Offset: 0x0025B312
		public override string TakeDamageSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/chomper/chomper_hurt";
			}
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0025D11C File Offset: 0x0025B31C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0025D160 File Offset: 0x0025B360
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CLAMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClampMove), new AbstractIntent[]
			{
				new MultiAttackIntent(Chomper.ClampDamage, 2)
			});
			MoveState moveState2 = new MoveState("SCREECH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreechMove), new AbstractIntent[]
			{
				new StatusIntent(3)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			MoveState moveState3 = (this._screamFirst ? moveState2 : moveState);
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0025D1F4 File Offset: 0x0025B3F4
		private async Task ClampMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(Chomper.ClampDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x0025D238 File Offset: 0x0025B438
		private async Task ScreechMove(IReadOnlyList<Creature> targets)
		{
			LocString locString = MonsterModel.L10NMonsterLookup("CHOMPER.moves.SCREECH.title");
			TalkCmd.Play(locString, base.Creature, VfxColor.Cyan, VfxDuration.Custom);
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 1f);
			await CardPileCmd.AddToCombatAndPreview<Dazed>(targets, PileType.Discard, 3, null, CardPilePosition.Bottom);
		}

		// Token: 0x04002567 RID: 9575
		public const string screechMoveId = "SCREECH_MOVE";

		// Token: 0x04002568 RID: 9576
		private const int _screechStatusCount = 3;

		// Token: 0x04002569 RID: 9577
		private const int _clampRepeat = 2;

		// Token: 0x0400256A RID: 9578
		private bool _screamFirst;
	}
}
