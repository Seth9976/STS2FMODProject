using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007DF RID: 2015
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhrogParasite : MonsterModel
	{
		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x060063AA RID: 25514 RVA: 0x00267986 File Offset: 0x00265B86
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 66, 61);
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x060063AB RID: 25515 RVA: 0x00267992 File Offset: 0x00265B92
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 68, 64);
			}
		}

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x060063AC RID: 25516 RVA: 0x0026799E File Offset: 0x00265B9E
		private int LashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x060063AD RID: 25517 RVA: 0x002679A9 File Offset: 0x00265BA9
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Plant;
			}
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x002679AC File Offset: 0x00265BAC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<InfestedPower>(new ThrowingPlayerChoiceContext(), base.Creature, 4m, base.Creature, null, false);
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x002679F0 File Offset: 0x00265BF0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("INFECT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InfectMove), new AbstractIntent[]
			{
				new StatusIntent(3)
			});
			MoveState moveState2 = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.LashDamage, 4)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060063B0 RID: 25520 RVA: 0x00267A9C File Offset: 0x00265C9C
		private async Task LashMove(IReadOnlyList<Creature> targets)
		{
			AttackCommand attackCommand = DamageCmd.Attack(this.LashDamage).WithHitCount(4).FromMonster(this)
				.WithAttackerAnim("Attack", 0.55f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null);
			Func<Creature, Node2D> func;
			if ((func = PhrogParasite.<>O.<0>__Create) == null)
			{
				func = (PhrogParasite.<>O.<0>__Create = new Func<Creature, Node2D>(NWormyImpactVfx.Create));
			}
			await attackCommand.WithHitVfxNode(func).Execute(null);
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x00267AE0 File Offset: 0x00265CE0
		private async Task InfectMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.75f);
			foreach (Creature creature in targets)
			{
				NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(creature);
				if (nwormyImpactVfx != null)
				{
					Control vfxContainer = creature.GetVfxContainer();
					if (vfxContainer != null)
					{
						vfxContainer.AddChildSafely(nwormyImpactVfx);
					}
				}
			}
			await CardPileCmd.AddToCombatAndPreview<Infection>(targets, PileType.Discard, 3, null, CardPilePosition.Bottom);
		}

		// Token: 0x04002658 RID: 9816
		private const int _lashRepeat = 4;

		// Token: 0x04002659 RID: 9817
		private const int _infestAmt = 3;

		// Token: 0x02001E10 RID: 7696
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04007A34 RID: 31284
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <0>__Create;
		}
	}
}
