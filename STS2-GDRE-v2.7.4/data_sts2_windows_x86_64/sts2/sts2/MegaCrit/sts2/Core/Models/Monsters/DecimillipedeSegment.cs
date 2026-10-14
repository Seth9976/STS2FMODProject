using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007AD RID: 1965
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class DecimillipedeSegment : MonsterModel
	{
		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x060060AD RID: 24749 RVA: 0x0025E6E9 File Offset: 0x0025C8E9
		private string PhobiaModeAliveTexture
		{
			get
			{
				return ImageHelper.GetImagePath("monsters/phobia_mode/" + base.Id.Entry.ToLowerInvariant() + "_phobia.png");
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060060AE RID: 24750 RVA: 0x0025E70F File Offset: 0x0025C90F
		private string PhobiaModeDeadTexture
		{
			get
			{
				return ImageHelper.GetImagePath("monsters/phobia_mode/" + base.Id.Entry.ToLowerInvariant() + "_shriveled_phobia.png");
			}
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x0025E735 File Offset: 0x0025C935
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("DECIMILLIPEDE_SEGMENT.name");
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x060060B0 RID: 24752 RVA: 0x0025E741 File Offset: 0x0025C941
		// (set) Token: 0x060060B1 RID: 24753 RVA: 0x0025E749 File Offset: 0x0025C949
		public int StarterMoveIdx
		{
			get
			{
				return this._starterMoveIdx;
			}
			set
			{
				base.AssertMutable();
				this._starterMoveIdx = value;
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x060060B2 RID: 24754 RVA: 0x0025E758 File Offset: 0x0025C958
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 40);
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x060060B3 RID: 24755 RVA: 0x0025E764 File Offset: 0x0025C964
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 52, 46);
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x060060B4 RID: 24756 RVA: 0x0025E770 File Offset: 0x0025C970
		private int WritheDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x060060B5 RID: 24757 RVA: 0x0025E77B File Offset: 0x0025C97B
		private int ConstrictDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x060060B6 RID: 24758 RVA: 0x0025E787 File Offset: 0x0025C987
		private int BulkDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x060060B7 RID: 24759 RVA: 0x0025E792 File Offset: 0x0025C992
		private int BulkStrength
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x060060B8 RID: 24760 RVA: 0x0025E795 File Offset: 0x0025C995
		public override float HpBarSizeReduction
		{
			get
			{
				return 35f;
			}
		}

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x060060B9 RID: 24761 RVA: 0x0025E79C File Offset: 0x0025C99C
		// (set) Token: 0x060060BA RID: 24762 RVA: 0x0025E7A4 File Offset: 0x0025C9A4
		public MoveState DeadState
		{
			get
			{
				return this._deadState;
			}
			private set
			{
				base.AssertMutable();
				this._deadState = value;
			}
		}

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x060060BB RID: 24763 RVA: 0x0025E7B3 File Offset: 0x0025C9B3
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x060060BC RID: 24764 RVA: 0x0025E7B6 File Offset: 0x0025C9B6
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x0025E7B9 File Offset: 0x0025C9B9
		public override bool ShouldShowInCompendium
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x060060BE RID: 24766 RVA: 0x0025E7BC File Offset: 0x0025C9BC
		public override bool CanChangeScale
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x060060BF RID: 24767 RVA: 0x0025E7BF File Offset: 0x0025C9BF
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_die";
			}
		}

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x060060C0 RID: 24768 RVA: 0x0025E7C8 File Offset: 0x0025C9C8
		public unsafe override IEnumerable<string> AssetPaths
		{
			get
			{
				int num = 3;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = DecimillipedeSegment.rocksVfxPath;
				num2++;
				*span[num2] = this.PhobiaModeAliveTexture;
				num2++;
				*span[num2] = this.PhobiaModeDeadTexture;
				List<string> list2 = list;
				return list2.Concat<string>(base.AssetPaths);
			}
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x0025E830 File Offset: 0x0025CA30
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			decimal maxHp = base.Creature.MaxHp;
			if (maxHp % 2m == 1m)
			{
				maxHp += 1m;
			}
			IReadOnlyList<Player> players = base.CombatState.Players;
			int count = players.Count;
			int currentActIndex = base.CombatState.RunState.CurrentActIndex;
			List<Creature> list = (from c in base.CombatState.GetTeammatesOf(base.Creature)
				where c != this.Creature
				select c).ToList<Creature>();
			while (list.Any<Creature>((Creature c) => c.MaxHp == maxHp))
			{
				maxHp += 2m;
				if (maxHp > Creature.ScaleHpForMultiplayer(this.MaxInitialHp, base.CombatState.Encounter, count, currentActIndex))
				{
					maxHp = Creature.ScaleHpForMultiplayer(this.MinInitialHp, base.CombatState.Encounter, count, currentActIndex);
				}
			}
			await CreatureCmd.SetMaxAndCurrentHp(base.Creature, maxHp);
			await PowerCmd.Apply<ReattachPower>(new ThrowingPlayerChoiceContext(), base.Creature, 25m, base.Creature, null, false);
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x0025E874 File Offset: 0x0025CA74
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("WRITHE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WritheMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.WritheDamage, 2)
			});
			MoveState moveState2 = new MoveState("BULK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BulkMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BulkDamage),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("CONSTRICT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ConstrictMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ConstrictDamage),
				new DebuffIntent(false)
			});
			this.DeadState = new MoveState("DEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DeadMove), Array.Empty<AbstractIntent>());
			MoveState moveState4 = new MoveState("REATTACH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReattachMove), new AbstractIntent[]
			{
				new HealIntent()
			})
			{
				MustPerformOnceBeforeTransitioning = true
			};
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			moveState.FollowUpState = moveState3;
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			this.DeadState.FollowUpState = moveState4;
			moveState4.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(this.DeadState);
			list.Add(moveState4);
			list.Add(randomBranchState);
			int num = this.StarterMoveIdx % 3;
			MoveState moveState5;
			if (num != 0)
			{
				if (num != 1)
				{
					moveState5 = moveState3;
				}
				else
				{
					moveState5 = moveState2;
				}
			}
			else
			{
				moveState5 = moveState;
			}
			MoveState moveState6 = moveState5;
			return new MonsterMoveStateMachine(list, moveState6);
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x0025EA1C File Offset: 0x0025CC1C
		private async Task WritheMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_triple", 1f);
			await this.AnimSegmentsAttack();
			await DamageCmd.Attack(this.WritheDamage).WithHitCount(2).FromMonster(this)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0025EA60 File Offset: 0x0025CC60
		private async Task BulkMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_buff", 1f);
			await this.AnimSegmentsAttack();
			await DamageCmd.Attack(this.BulkDamage).FromMonster(this).WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.BulkStrength, base.Creature, null, false);
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x0025EAA4 File Offset: 0x0025CCA4
		private async Task ConstrictMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_weaken", 1f);
			await this.AnimSegmentsAttack();
			await DamageCmd.Attack(this.ConstrictDamage).FromMonster(this).WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x0025EAEF File Offset: 0x0025CCEF
		private Task DeadMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x0025EAF8 File Offset: 0x0025CCF8
		private async Task ReattachMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_heal", 1f);
			if (base.CombatState.IsLiveCombat())
			{
				await base.Creature.GetPower<ReattachPower>().DoReattach();
			}
			this.ChangePhobiaModeTexture(ResourceLoader.Load<Texture2D>(this.PhobiaModeAliveTexture, null, 1L));
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x0025EB3C File Offset: 0x0025CD3C
		private async Task AnimSegmentsAttack()
		{
			if (!TestMode.IsOn)
			{
				IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Creature)
					where c.Monster is DecimillipedeSegment
					select c;
				foreach (Creature creature in enumerable)
				{
					((DecimillipedeSegment)creature.Monster).SegmentAttack();
				}
				Node2D node2D = PreloadManager.Cache.GetScene(DecimillipedeSegment.rocksVfxPath).Instantiate<Node2D>(0L);
				Control vfxContainer = base.Creature.GetVfxContainer();
				if (vfxContainer != null)
				{
					vfxContainer.AddChildSafely(node2D);
				}
				node2D.GlobalPosition = NGame.Instance.GetViewportRect().Size * 0.5f;
				await Cmd.Wait(0.5f, false);
			}
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x0025EB7F File Offset: 0x0025CD7F
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			if (TestMode.IsOn)
			{
				return Task.CompletedTask;
			}
			this.ChangePhobiaModeTexture(ResourceLoader.Load<Texture2D>(this.PhobiaModeDeadTexture, null, 1L));
			return Task.CompletedTask;
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x0025EBB8 File Offset: 0x0025CDB8
		private void ChangePhobiaModeTexture(Texture2D tex)
		{
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode == null)
			{
				return;
			}
			if (!creatureNode.Visuals.IsUsingPhobiaModeBody)
			{
				return;
			}
			((Sprite2D)creatureNode.Body).Texture = tex;
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0025EBF4 File Offset: 0x0025CDF4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("dead_loop", true);
			AnimState animState4 = new AnimState("wither", false);
			AnimState animState5 = new AnimState("regenerate", false);
			animState2.NextState = animState;
			animState4.NextState = animState3;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Revive", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState, null);
			creatureAnimator.AddAnyState("Dead", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x060060CC RID: 24780
		public abstract void SegmentAttack();

		// Token: 0x0400258B RID: 9611
		private int _starterMoveIdx;

		// Token: 0x0400258C RID: 9612
		private const int _writheRepeat = 2;

		// Token: 0x0400258D RID: 9613
		private MoveState _deadState;

		// Token: 0x0400258E RID: 9614
		public static readonly string rocksVfxPath = SceneHelper.GetScenePath("vfx/vfx_decimillipede_rocks");

		// Token: 0x0400258F RID: 9615
		private const string _healSfx = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_heal";

		// Token: 0x04002590 RID: 9616
		private const string _attackTriple = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_triple";

		// Token: 0x04002591 RID: 9617
		private const string _attackBuff = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_buff";

		// Token: 0x04002592 RID: 9618
		private const string _attackWeaken = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_weaken";
	}
}
