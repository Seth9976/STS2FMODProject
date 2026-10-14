using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007BA RID: 1978
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Flyconid : MonsterModel
	{
		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x0600614F RID: 24911 RVA: 0x002606BB File Offset: 0x0025E8BB
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 47);
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06006150 RID: 24912 RVA: 0x002606C7 File Offset: 0x0025E8C7
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 53, 49);
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06006151 RID: 24913 RVA: 0x002606D3 File Offset: 0x0025E8D3
		private int SmashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06006152 RID: 24914 RVA: 0x002606E0 File Offset: 0x0025E8E0
		private int SporeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x002606EC File Offset: 0x0025E8EC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("VULNERABLE_SPORES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.VulnerableSporesMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("FRAIL_SPORES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FrailSporesMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SporeDamage),
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SmashDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			RandomBranchState randomBranchState2 = new RandomBranchState("INITIAL");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, 3, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, 2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			randomBranchState2.AddBranch(moveState2, 2, MoveRepeatType.CannotRepeat);
			randomBranchState2.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			list.Add(randomBranchState2);
			return new MonsterMoveStateMachine(list, randomBranchState2);
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x00260814 File Offset: 0x0025EA14
		private async Task VulnerableSporesMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					NFlyconidSporesVfx node = creatureNode.Visuals.GetCurrentBody().GetNode<NFlyconidSporesVfx>("%VfxController");
					if (node != null)
					{
						node.SetSporeTypeIsVulnerable(true);
					}
				}
			}
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x06006155 RID: 24917 RVA: 0x00260860 File Offset: 0x0025EA60
		private async Task FrailSporesMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					NFlyconidSporesVfx node = creatureNode.Visuals.GetCurrentBody().GetNode<NFlyconidSporesVfx>("%VfxController");
					if (node != null)
					{
						node.SetSporeTypeIsVulnerable(false);
					}
				}
			}
			await DamageCmd.Attack(this.SporeDamage).FromMonster(this).WithAttackerAnim("Cast", 0.5f, null)
				.WithAttackerFx(null, this.CastSfx, null)
				.WithWaitBeforeHit(0f, 0.6f)
				.WithHitVfxNode((Creature t) => NSporeImpactVfx.Create(t, new Color("8aad7d")))
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x002608AC File Offset: 0x0025EAAC
		private async Task SmashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SmashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}
	}
}
