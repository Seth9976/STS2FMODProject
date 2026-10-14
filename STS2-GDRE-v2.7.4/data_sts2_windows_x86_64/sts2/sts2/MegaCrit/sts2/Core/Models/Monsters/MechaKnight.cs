using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D2 RID: 2002
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MechaKnight : MonsterModel
	{
		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x060062E0 RID: 25312 RVA: 0x00265521 File Offset: 0x00263721
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 320, 300);
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x060062E1 RID: 25313 RVA: 0x00265533 File Offset: 0x00263733
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x060062E2 RID: 25314 RVA: 0x0026553B File Offset: 0x0026373B
		private static int ChargeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 30, 25);
			}
		}

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x060062E3 RID: 25315 RVA: 0x00265548 File Offset: 0x00263748
		private static int HeavyCleaveDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
			}
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x060062E4 RID: 25316 RVA: 0x00265555 File Offset: 0x00263755
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_die";
			}
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x060062E5 RID: 25317 RVA: 0x0026555C File Offset: 0x0026375C
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_hurt";
			}
		}

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x060062E6 RID: 25318 RVA: 0x00265563 File Offset: 0x00263763
		// (set) Token: 0x060062E7 RID: 25319 RVA: 0x0026556B File Offset: 0x0026376B
		private bool IsWoundUp
		{
			get
			{
				return this._isWoundUp;
			}
			set
			{
				base.AssertMutable();
				this._isWoundUp = value;
			}
		}

		// Token: 0x060062E8 RID: 25320 RVA: 0x0026557C File Offset: 0x0026377C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x002655C0 File Offset: 0x002637C0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CHARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(MechaKnight.ChargeDamage)
			});
			MoveState moveState2 = new MoveState("FLAMETHROWER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlamethrowerMove), new AbstractIntent[]
			{
				new StatusIntent(4)
			});
			MoveState moveState3 = new MoveState("WINDUP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WindupMove), new AbstractIntent[]
			{
				new DefendIntent(),
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("HEAVY_CLEAVE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeavyCleaveMove), new AbstractIntent[]
			{
				new SingleAttackIntent(MechaKnight.HeavyCleaveDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState4);
			list.Add(moveState3);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x002656C0 File Offset: 0x002638C0
		private async Task ChargeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(MechaKnight.ChargeDamage).FromMonster(this).WithAttackerAnim("charge", 0.25f, null)
				.WithWaitBeforeHit(0.5f, 1f)
				.WithHitVfxNode((Creature _) => NSpikeSplashVfx.Create(base.Creature, VfxColor.Gold))
				.WithHitFx(null, "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_dash", null)
				.WithHitVfxNode((Creature _) => NBigSlashImpactVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, base.CombatState).Value, 180f, new Color("#80dbff")))
				.Execute(null);
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x00265704 File Offset: 0x00263904
		private async Task HeavyCleaveMove(IReadOnlyList<Creature> targets)
		{
			this.IsWoundUp = false;
			await DamageCmd.Attack(MechaKnight.HeavyCleaveDamage).FromMonster(this).WithAttackerAnim("Attack", 0.4f, null)
				.WithHitFx(null, "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_heavy_attack", null)
				.AfterAttackerAnim(delegate
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.RadialBlur(VfxPosition.Left);
					}
					NGame instance2 = NGame.Instance;
					if (instance2 != null)
					{
						instance2.DoHitStop(ShakeStrength.Strong, ShakeDuration.Normal);
					}
					return Task.CompletedTask;
				})
				.WithHitVfxNode((Creature _) => NBigSlashVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, base.CombatState).Value, false))
				.WithHitVfxNode((Creature _) => NBigSlashImpactVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, base.CombatState).Value, 180f, new Color("#80dbff")))
				.Execute(null);
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x00265748 File Offset: 0x00263948
		private async Task WindupMove(IReadOnlyList<Creature> targets)
		{
			this.IsWoundUp = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "windUp", 0.5f);
			await CreatureCmd.GainBlock(base.Creature, 15m, ValueProp.Move, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x0026578C File Offset: 0x0026398C
		private async Task FlamethrowerMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_flamethrower", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "flamethrower", 1.5f);
			await CardPileCmd.AddToCombatAndPreview<Burn>(targets, PileType.Hand, 4, null, CardPilePosition.Bottom);
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x002657D8 File Offset: 0x002639D8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("attack_flame", false);
			AnimState animState5 = new AnimState("attack_cleave", false);
			AnimState animState6 = new AnimState("charge", false);
			AnimState animState7 = new AnimState("wind_up", false);
			AnimState animState8 = new AnimState("idle_loop_wound", true);
			AnimState animState9 = new AnimState("hurt_wound", false);
			animState5.NextState = animState;
			animState6.NextState = animState;
			animState4.NextState = animState;
			animState2.NextState = animState;
			animState7.NextState = animState8;
			animState9.NextState = animState8;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState5, null);
			creatureAnimator.AddAnyState("flamethrower", animState4, null);
			creatureAnimator.AddAnyState("charge", animState6, null);
			creatureAnimator.AddAnyState("windUp", animState7, null);
			animState.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState4.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState2.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState5.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState7.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState8.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState9.AddBranch("Hit", animState2, () => !this.IsWoundUp);
			animState.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState4.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState2.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState5.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState7.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState8.AddBranch("Hit", animState9, () => this.IsWoundUp);
			animState9.AddBranch("Hit", animState9, () => this.IsWoundUp);
			return creatureAnimator;
		}

		// Token: 0x04002628 RID: 9768
		private const int _flamethrowerCardCount = 4;

		// Token: 0x04002629 RID: 9769
		private const int _windupBlock = 15;

		// Token: 0x0400262A RID: 9770
		private const string _windUpTrigger = "windUp";

		// Token: 0x0400262B RID: 9771
		private const string _flameAttackTrigger = "flamethrower";

		// Token: 0x0400262C RID: 9772
		private const string _chargeTrigger = "charge";

		// Token: 0x0400262D RID: 9773
		private bool _isWoundUp;

		// Token: 0x0400262E RID: 9774
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_buff";

		// Token: 0x0400262F RID: 9775
		private const string _dashSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_dash";

		// Token: 0x04002630 RID: 9776
		private const string _flamethrowerSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_flamethrower";

		// Token: 0x04002631 RID: 9777
		private const string _heavyAttackSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_heavy_attack";
	}
}
