using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D5 RID: 2005
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Myte : MonsterModel
	{
		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x0600630C RID: 25356 RVA: 0x00265C7B File Offset: 0x00263E7B
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64, 61);
			}
		}

		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x0600630D RID: 25357 RVA: 0x00265C87 File Offset: 0x00263E87
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 69, 67);
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x0600630E RID: 25358 RVA: 0x00265C93 File Offset: 0x00263E93
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x0600630F RID: 25359 RVA: 0x00265CA0 File Offset: 0x00263EA0
		private int SuckDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 4);
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06006310 RID: 25360 RVA: 0x00265CAB File Offset: 0x00263EAB
		private int SuckStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06006311 RID: 25361 RVA: 0x00265CB6 File Offset: 0x00263EB6
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/mite/mite_die";
			}
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x00265CC0 File Offset: 0x00263EC0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("TOXIC_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ToxicMove), new AbstractIntent[]
			{
				new StatusIntent(2)
			});
			MoveState moveState2 = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState3 = new MoveState("SUCK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SuckMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SuckDamage),
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			conditionalBranchState.AddState(moveState, () => base.Creature.SlotName == "first");
			conditionalBranchState.AddState(moveState3, () => base.Creature.SlotName == "second");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x00265DC0 File Offset: 0x00263FC0
		private async Task ToxicMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					Player me = LocalContext.GetMe(base.CombatState);
					Creature creature = ((me != null) ? me.Creature : null);
					NMyteVfx specialNode = creatureNode.GetSpecialNode<NMyteVfx>("%NMyteVfx");
					if (specialNode != null)
					{
						specialNode.SetTarget(creature);
					}
				}
			}
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mite/mite_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			await CardPileCmd.AddToCombatAndPreview<Toxic>(targets, PileType.Hand, 2, null, CardPilePosition.Bottom);
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x00265E0C File Offset: 0x0026400C
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/mite/mite_attack", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x06006315 RID: 25365 RVA: 0x00265E50 File Offset: 0x00264050
		private async Task SuckMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SuckDamage).FromMonster(this).WithAttackerAnim("Suck", 0.4f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/mite/mite_suck", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.SuckStrength, base.Creature, null, false);
		}

		// Token: 0x06006316 RID: 25366 RVA: 0x00265E94 File Offset: 0x00264094
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("suck", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Suck", animState6, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x04002633 RID: 9779
		private const int _toxicCount = 2;

		// Token: 0x04002634 RID: 9780
		private const string _suckTrigger = "Suck";

		// Token: 0x04002635 RID: 9781
		private const string _attackSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_attack";

		// Token: 0x04002636 RID: 9782
		private const string _castSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_cast";

		// Token: 0x04002637 RID: 9783
		private const string _suckSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_suck";
	}
}
