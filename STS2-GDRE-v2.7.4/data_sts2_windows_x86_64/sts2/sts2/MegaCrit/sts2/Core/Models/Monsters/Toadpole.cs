using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007FF RID: 2047
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Toadpole : MonsterModel
	{
		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x060065DF RID: 26079 RVA: 0x0026E329 File Offset: 0x0026C529
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x060065E0 RID: 26080 RVA: 0x0026E335 File Offset: 0x0026C535
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 26, 25);
			}
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x060065E1 RID: 26081 RVA: 0x0026E341 File Offset: 0x0026C541
		// (set) Token: 0x060065E2 RID: 26082 RVA: 0x0026E349 File Offset: 0x0026C549
		public bool IsFront
		{
			get
			{
				return this._isFront;
			}
			set
			{
				base.AssertMutable();
				this._isFront = value;
			}
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x060065E3 RID: 26083 RVA: 0x0026E358 File Offset: 0x0026C558
		private int SpikeSpitDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x060065E4 RID: 26084 RVA: 0x0026E363 File Offset: 0x0026C563
		private int SpikeSpitRepeat
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x060065E5 RID: 26085 RVA: 0x0026E366 File Offset: 0x0026C566
		private int WhirlDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x060065E6 RID: 26086 RVA: 0x0026E371 File Offset: 0x0026C571
		private int SpikenAmount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x060065E7 RID: 26087 RVA: 0x0026E374 File Offset: 0x0026C574
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x060065E8 RID: 26088 RVA: 0x0026E378 File Offset: 0x0026C578
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(Toadpole._eyeOptions)));
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(Toadpole._patternOptions)));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x0026E3DC File Offset: 0x0026C5DC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SPIKE_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikeSpitMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SpikeSpitDamage, this.SpikeSpitRepeat)
			});
			MoveState moveState2 = new MoveState("WHIRL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.WhirlDamage)
			});
			MoveState moveState3 = new MoveState("SPIKEN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikenMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			moveState.FollowUpState = moveState2;
			conditionalBranchState.AddState(moveState2, () => !((Toadpole)base.Creature.Monster).IsFront);
			conditionalBranchState.AddState(moveState3, () => ((Toadpole)base.Creature.Monster).IsFront);
			list.Add(conditionalBranchState);
			list.Add(moveState3);
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x0026E4E0 File Offset: 0x0026C6E0
		private async Task SpikeSpitMove(IReadOnlyList<Creature> targets)
		{
			await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), base.Creature, -this.SpikenAmount, base.Creature, null, false);
			await DamageCmd.Attack(this.SpikeSpitDamage).WithHitCount(this.SpikeSpitRepeat).FromMonster(this)
				.WithAttackerAnim("AttackTriple", 0.3f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/toadpole/toadpole_attack_spin", null)
				.WithHitFx("vfx/vfx_attack_blunt", this.AttackSfx, null)
				.Execute(null);
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x0026E524 File Offset: 0x0026C724
		private async Task WhirlMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.WhirlDamage).FromMonster(this).WithAttackerAnim("AttackSingle", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", this.AttackSfx, null)
				.Execute(null);
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x0026E568 File Offset: 0x0026C768
		private async Task SpikenMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.2f);
			await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.SpikenAmount, base.Creature, null, false);
		}

		// Token: 0x060065ED RID: 26093 RVA: 0x0026E5AC File Offset: 0x0026C7AC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack_single", false);
			AnimState animState4 = new AnimState("attack_triple", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("idle_loop_buffed", true);
			AnimState animState8 = new AnimState("attack_single_buffed", false);
			AnimState animState9 = new AnimState("attack_triple", false);
			AnimState animState10 = new AnimState("hurt_buffed", false);
			AnimState animState11 = new AnimState("die_buffed", false);
			animState2.NextState = animState7;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState8.NextState = animState7;
			animState9.NextState = animState;
			animState10.NextState = animState7;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("AttackSingle", animState3, () => !base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("AttackTriple", animState4, () => !base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("Dead", animState6, () => !base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("Hit", animState5, () => !base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("AttackSingle", animState8, () => base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("AttackTriple", animState9, () => base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("Dead", animState11, () => base.Creature.HasPower<ThornsPower>());
			creatureAnimator.AddAnyState("Hit", animState10, () => base.Creature.HasPower<ThornsPower>());
			return creatureAnimator;
		}

		// Token: 0x040026EF RID: 9967
		private static readonly string[] _eyeOptions = new string[] { "eye1", "eye2" };

		// Token: 0x040026F0 RID: 9968
		private static readonly string[] _patternOptions = new string[] { "pattern1", "pattern2" };

		// Token: 0x040026F1 RID: 9969
		private bool _isFront;

		// Token: 0x040026F2 RID: 9970
		private const string _attackSingleTrigger = "AttackSingle";

		// Token: 0x040026F3 RID: 9971
		private const string _attackTripleTrigger = "AttackTriple";

		// Token: 0x040026F4 RID: 9972
		private const string _spinAttackSfx = "event:/sfx/enemy/enemy_attacks/toadpole/toadpole_attack_spin";
	}
}
