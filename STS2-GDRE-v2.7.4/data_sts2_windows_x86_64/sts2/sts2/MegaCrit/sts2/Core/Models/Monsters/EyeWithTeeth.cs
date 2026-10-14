using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B5 RID: 1973
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EyeWithTeeth : MonsterModel
	{
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06006107 RID: 24839 RVA: 0x0025F64C File Offset: 0x0025D84C
		public override int MinInitialHp
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06006108 RID: 24840 RVA: 0x0025F64F File Offset: 0x0025D84F
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06006109 RID: 24841 RVA: 0x0025F657 File Offset: 0x0025D857
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600610A RID: 24842 RVA: 0x0025F65A File Offset: 0x0025D85A
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x0025F660 File Offset: 0x0025D860
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<IllusionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x0025F6A4 File Offset: 0x0025D8A4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("DISTRACT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DistractMove), new AbstractIntent[]
			{
				new StatusIntent(3)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x0025F6F4 File Offset: 0x0025D8F4
		private async Task DistractMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.AttackSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Attack", 0.7f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_attack_slash");
			await CardPileCmd.AddToCombatAndPreview<Dazed>(targets, PileType.Discard, 3, null, CardPilePosition.Bottom);
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x0025F740 File Offset: 0x0025D940
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("die", false);
			animState2.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Dead", animState3, () => !base.CombatState.GetTeammatesOf(base.Creature).Any<Creature>((Creature t) => t != null && t.IsPrimaryEnemy && t.IsAlive));
			return creatureAnimator;
		}

		// Token: 0x0400259C RID: 9628
		private const int _distractAmount = 3;
	}
}
