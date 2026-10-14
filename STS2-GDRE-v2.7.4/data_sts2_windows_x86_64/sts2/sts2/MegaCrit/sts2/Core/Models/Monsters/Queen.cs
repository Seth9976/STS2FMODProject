using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E1 RID: 2017
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Queen : MonsterModel
	{
		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x060063C6 RID: 25542 RVA: 0x00267E67 File Offset: 0x00266067
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/queen/queen_arms_attack";
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x060063C7 RID: 25543 RVA: 0x00267E6E File Offset: 0x0026606E
		public override IEnumerable<string> AssetPaths
		{
			get
			{
				return base.AssetPaths.Concat<string>(ModelDb.Monster<TorchHeadAmalgam>().AssetPaths);
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x060063C8 RID: 25544 RVA: 0x00267E85 File Offset: 0x00266085
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 419, 400);
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x060063C9 RID: 25545 RVA: 0x00267E97 File Offset: 0x00266097
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x060063CA RID: 25546 RVA: 0x00267E9F File Offset: 0x0026609F
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x060063CB RID: 25547 RVA: 0x00267EA2 File Offset: 0x002660A2
		private int OffWithYourHeadDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x060063CC RID: 25548 RVA: 0x00267EAD File Offset: 0x002660AD
		private int ExecutionDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 15);
			}
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x060063CD RID: 25549 RVA: 0x00267EBA File Offset: 0x002660BA
		// (set) Token: 0x060063CE RID: 25550 RVA: 0x00267EC2 File Offset: 0x002660C2
		private bool HasAmalgamDied
		{
			get
			{
				return this._hasAmalgamDied;
			}
			set
			{
				base.AssertMutable();
				this._hasAmalgamDied = value;
			}
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x060063CF RID: 25551 RVA: 0x00267ED1 File Offset: 0x002660D1
		// (set) Token: 0x060063D0 RID: 25552 RVA: 0x00267ED9 File Offset: 0x002660D9
		[Nullable(2)]
		private Creature Amalgam
		{
			[NullableContext(2)]
			get
			{
				return this._amalgam;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._amalgam = value;
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x060063D1 RID: 25553 RVA: 0x00267EE8 File Offset: 0x002660E8
		// (set) Token: 0x060063D2 RID: 25554 RVA: 0x00267EF0 File Offset: 0x002660F0
		private MoveState BurnBrightForMeState
		{
			get
			{
				return this._burnBrightForMeState;
			}
			set
			{
				base.AssertMutable();
				this._burnBrightForMeState = value;
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x060063D3 RID: 25555 RVA: 0x00267EFF File Offset: 0x002660FF
		// (set) Token: 0x060063D4 RID: 25556 RVA: 0x00267F07 File Offset: 0x00266107
		private MoveState EnragedState
		{
			get
			{
				return this._enragedState;
			}
			set
			{
				base.AssertMutable();
				this._enragedState = value;
			}
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x00267F16 File Offset: 0x00266116
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			spine.GetAnimationState().SetAnimation("tracks/writhe", true, 1);
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x00267F2C File Offset: 0x0026612C
		public override void BeforeRemovedFromRoom()
		{
			if (base.CombatState.RunState.IsGameOver)
			{
				return;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(base.Creature);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.SpineAnimation.SetAnimation("tracks/empty", true, 1);
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x00267F7C File Offset: 0x0026617C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			this.Amalgam = base.CombatState.Enemies.First<Creature>((Creature c) => c.Monster is TorchHeadAmalgam);
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("queen_progress", 1f);
			}
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x00267FC0 File Offset: 0x002661C0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PUPPET_STRINGS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PuppetStringsMove), new AbstractIntent[]
			{
				new CardDebuffIntent()
			});
			MoveState moveState2 = new MoveState("YOU_ARE_MINE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.YoureMineMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("YOURE_MINE_NOW_BRANCH");
			this.BurnBrightForMeState = new MoveState("BURN_BRIGHT_FOR_ME_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurnBrightForMeMove), new AbstractIntent[]
			{
				new BuffIntent(),
				new DefendIntent()
			});
			ConditionalBranchState conditionalBranchState2 = new ConditionalBranchState("BURN_BRIGHT_FOR_ME_BRANCH");
			MoveState moveState3 = new MoveState("OFF_WITH_YOUR_HEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OffWithYourHeadMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.OffWithYourHeadDamage, 5)
			});
			MoveState moveState4 = new MoveState("EXECUTION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExecutionMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ExecutionDamage)
			});
			this.EnragedState = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(this.BurnBrightForMeState, () => !this.HasAmalgamDied);
			conditionalBranchState.AddState(moveState3, () => this.HasAmalgamDied);
			this.BurnBrightForMeState.FollowUpState = conditionalBranchState2;
			conditionalBranchState2.AddState(this.BurnBrightForMeState, () => !this.HasAmalgamDied);
			conditionalBranchState2.AddState(moveState3, () => this.HasAmalgamDied);
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = this.EnragedState;
			this.EnragedState.FollowUpState = moveState3;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(this.BurnBrightForMeState);
			list.Add(conditionalBranchState2);
			list.Add(conditionalBranchState);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(this.EnragedState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x002681D0 File Offset: 0x002663D0
		private async Task PuppetStringsMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<ChainsOfBindingPower>(new ThrowingPlayerChoiceContext(), targets, 3m, base.Creature, null, false);
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0026821C File Offset: 0x0026641C
		private async Task YoureMineMove(IReadOnlyList<Creature> targets)
		{
			LocString locString = MonsterModel.L10NMonsterLookup("QUEEN.banter");
			TalkCmd.Play(locString, base.Creature, VfxColor.Purple, VfxDuration.Custom);
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 99m, base.Creature, null, false);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 99m, base.Creature, null, false);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 99m, base.Creature, null, false);
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x00268268 File Offset: 0x00266468
		private async Task BurnBrightForMeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.8f);
			int strengthAmount = AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
			List<Creature> list = base.CombatState.GetTeammatesOf(base.Creature).ToList<Creature>();
			foreach (Creature creature in list.Where<Creature>((Creature teammate) => teammate != base.Creature))
			{
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, strengthAmount, base.Creature, null, false);
			}
			IEnumerator<Creature> enumerator = null;
			await CreatureCmd.GainBlock(base.Creature, 20m, ValueProp.Move, null, false);
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x002682AC File Offset: 0x002664AC
		private async Task OffWithYourHeadMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.OffWithYourHeadDamage).WithHitCount(5).FromMonster(this)
				.WithAttackerAnim("Attack", 0.6f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x002682F0 File Offset: 0x002664F0
		private async Task ExecutionMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ExecutionDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x00268334 File Offset: 0x00266534
		private async Task EnrageMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x00268378 File Offset: 0x00266578
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature.Monster is TorchHeadAmalgam && base.Creature.IsAlive)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("queen_progress", 2f);
				}
				this.HasAmalgamDied = true;
				this.Amalgam = null;
				LocString locString = MonsterModel.L10NMonsterLookup("QUEEN.amalgamDeathSpeakLine");
				TalkCmd.Play(locString, base.Creature, VfxColor.Purple, VfxDuration.Custom);
				if (base.NextMove == this.BurnBrightForMeState)
				{
					base.SetMoveImmediate(this.EnragedState, false);
				}
			}
			if (creature == base.Creature)
			{
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("queen_progress", 5f);
				}
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					creatureNode.SpineAnimation.SetAnimation("tracks/empty", true, 1);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x00268449 File Offset: 0x00266649
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "PUPPET_STRINGS_MOVE" && moveStateId != "BURN_BRIGHT_FOR_ME_MOVE" && moveStateId != "ENRAGE_MOVE" && moveStateId != "EXECUTION_MOVE";
		}

		// Token: 0x04002660 RID: 9824
		private const string _puppetStringsMove = "PUPPET_STRINGS_MOVE";

		// Token: 0x04002661 RID: 9825
		private const string _burnBrightForMeMove = "BURN_BRIGHT_FOR_ME_MOVE";

		// Token: 0x04002662 RID: 9826
		private const string _executionMove = "EXECUTION_MOVE";

		// Token: 0x04002663 RID: 9827
		private const string _enrageMove = "ENRAGE_MOVE";

		// Token: 0x04002664 RID: 9828
		private const string _queenTrackName = "queen_progress";

		// Token: 0x04002665 RID: 9829
		private const string _castSfx = "event:/sfx/enemy/enemy_attacks/queen/queen_cast";

		// Token: 0x04002666 RID: 9830
		private const int _offWithYourHeadRepeat = 5;

		// Token: 0x04002667 RID: 9831
		private bool _hasAmalgamDied;

		// Token: 0x04002668 RID: 9832
		[Nullable(2)]
		private Creature _amalgam;

		// Token: 0x04002669 RID: 9833
		private MoveState _burnBrightForMeState;

		// Token: 0x0400266A RID: 9834
		private MoveState _enragedState;
	}
}
