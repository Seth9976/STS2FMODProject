using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B6 RID: 1974
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fabricator : MonsterModel
	{
		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06006112 RID: 24850 RVA: 0x0025F7F0 File Offset: 0x0025D9F0
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/fabricator/fabricator_hurt";
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06006113 RID: 24851 RVA: 0x0025F7F7 File Offset: 0x0025D9F7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 155, 150);
			}
		}

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06006114 RID: 24852 RVA: 0x0025F809 File Offset: 0x0025DA09
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06006115 RID: 24853 RVA: 0x0025F811 File Offset: 0x0025DA11
		private int FabricatingStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 18);
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x06006116 RID: 24854 RVA: 0x0025F81E File Offset: 0x0025DA1E
		private int DisintegrateDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06006117 RID: 24855 RVA: 0x0025F82B File Offset: 0x0025DA2B
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x0025F830 File Offset: 0x0025DA30
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("FABRICATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FabricateMove), new AbstractIntent[]
			{
				new SummonIntent()
			});
			MoveState moveState2 = new MoveState("FABRICATING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FabricatingStrikeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.FabricatingStrikeDamage),
				new SummonIntent()
			});
			MoveState moveState3 = new MoveState("DISINTEGRATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DisintegrateMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DisintegrateDamage)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			randomBranchState.AddBranch(moveState, MoveRepeatType.CanRepeatForever, () => 1f);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CanRepeatForever, () => 1f);
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("fabricateBranch");
			conditionalBranchState.AddState(randomBranchState, () => this.CanFabricate);
			conditionalBranchState.AddState(moveState3, () => !this.CanFabricate);
			moveState.FollowUpState = conditionalBranchState;
			moveState3.FollowUpState = conditionalBranchState;
			moveState2.FollowUpState = conditionalBranchState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(conditionalBranchState);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06006119 RID: 24857 RVA: 0x0025F99C File Offset: 0x0025DB9C
		private bool CanFabricate
		{
			get
			{
				return base.CombatState.GetTeammatesOf(base.Creature).Count<Creature>((Creature c) => c.IsAlive) < 4;
			}
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x0025F9D8 File Offset: 0x0025DBD8
		private async Task FabricateMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "fabricate", 0f);
			await this.SpawnDefensiveBot();
			await this.SpawnAggroBot();
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x0025FA1C File Offset: 0x0025DC1C
		private async Task FabricatingStrikeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.FabricatingStrikeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await this.SpawnAggroBot();
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x0025FA60 File Offset: 0x0025DC60
		private async Task DisintegrateMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DisintegrateDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x0025FAA4 File Offset: 0x0025DCA4
		private async Task SpawnDefensiveBot()
		{
			await this.SpawnBot(Fabricator.defenseSpawns);
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x0025FAE8 File Offset: 0x0025DCE8
		private async Task SpawnAggroBot()
		{
			await this.SpawnBot(Fabricator.aggroSpawns);
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x0025FB2C File Offset: 0x0025DD2C
		private async Task SpawnBot(IEnumerable<MonsterModel> options)
		{
			if (base.CombatState.IsLiveCombat())
			{
				List<MonsterModel> list = options.Where<MonsterModel>((MonsterModel m) => m != this._lastSpawned).ToList<MonsterModel>();
				MonsterModel monsterModel = base.RunRng.MonsterAi.NextItem<MonsterModel>(list);
				this._lastSpawned = monsterModel;
				Creature creature = await CreatureCmd.Add(monsterModel.ToMutable(), base.CombatState, CombatSide.Enemy, base.CombatState.Encounter.GetNextSlot(base.CombatState));
				Creature creature2 = creature;
				await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), creature2, 1m, base.Creature, null, false);
			}
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x0025FB78 File Offset: 0x0025DD78
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("fabricate", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			animState.AddBranch("Hit", animState4, null);
			animState2.AddBranch("Hit", animState4, null);
			animState4.AddBranch("Hit", animState4, null);
			creatureAnimator.AddAnyState("fabricate", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x0025FC58 File Offset: 0x0025DE58
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "FABRICATING_STRIKE_MOVE";
		}

		// Token: 0x0400259D RID: 9629
		private const string _fabricateTrigger = "fabricate";

		// Token: 0x0400259E RID: 9630
		private const string _fabricatingStrikeMove = "FABRICATING_STRIKE_MOVE";

		// Token: 0x0400259F RID: 9631
		public static readonly HashSet<MonsterModel> aggroSpawns = new HashSet<MonsterModel>
		{
			ModelDb.Monster<Zapbot>(),
			ModelDb.Monster<Stabbot>()
		};

		// Token: 0x040025A0 RID: 9632
		public static readonly HashSet<MonsterModel> defenseSpawns = new HashSet<MonsterModel>
		{
			ModelDb.Monster<Guardbot>(),
			ModelDb.Monster<Noisebot>()
		};

		// Token: 0x040025A1 RID: 9633
		[Nullable(2)]
		private MonsterModel _lastSpawned;
	}
}
