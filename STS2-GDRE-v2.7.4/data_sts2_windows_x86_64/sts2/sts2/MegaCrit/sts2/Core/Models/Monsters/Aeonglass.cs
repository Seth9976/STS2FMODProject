using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
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
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000794 RID: 1940
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Aeonglass : MonsterModel
	{
		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x06005F2C RID: 24364 RVA: 0x0025A459 File Offset: 0x00258659
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 535, 512);
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x06005F2D RID: 24365 RVA: 0x0025A46B File Offset: 0x0025866B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x06005F2E RID: 24366 RVA: 0x0025A473 File Offset: 0x00258673
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x06005F2F RID: 24367 RVA: 0x0025A476 File Offset: 0x00258676
		private int EbbDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 26, 22);
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06005F30 RID: 24368 RVA: 0x0025A483 File Offset: 0x00258683
		private int EbbBlock
		{
			get
			{
				return 33;
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06005F31 RID: 24369 RVA: 0x0025A487 File Offset: 0x00258687
		private int EyeLasersDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x06005F32 RID: 24370 RVA: 0x0025A494 File Offset: 0x00258694
		private int EyeLasersRepeat
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06005F33 RID: 24371 RVA: 0x0025A497 File Offset: 0x00258697
		private int IncreasingIntensityBaseStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06005F34 RID: 24372 RVA: 0x0025A4A2 File Offset: 0x002586A2
		private int WitherAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06005F35 RID: 24373 RVA: 0x0025A4AD File Offset: 0x002586AD
		// (set) Token: 0x06005F36 RID: 24374 RVA: 0x0025A4B5 File Offset: 0x002586B5
		private int AdditionalStrength
		{
			get
			{
				return this._additionalStrength;
			}
			set
			{
				base.AssertMutable();
				this._additionalStrength = value;
			}
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x06005F37 RID: 24375 RVA: 0x0025A4C4 File Offset: 0x002586C4
		// (set) Token: 0x06005F38 RID: 24376 RVA: 0x0025A4CC File Offset: 0x002586CC
		public int WitherUpgradeCount
		{
			get
			{
				return this._witherUpgradeCount;
			}
			set
			{
				base.AssertMutable();
				this._witherUpgradeCount = value;
			}
		}

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x06005F39 RID: 24377 RVA: 0x0025A4DB File Offset: 0x002586DB
		private int IncreasingIntensityTotalStrength
		{
			get
			{
				return this.IncreasingIntensityBaseStrength + this.AdditionalStrength;
			}
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x0025A4EC File Offset: 0x002586EC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("queen_progress", 1f);
			}
			foreach (Creature creature in base.CombatState.GetOpponentsOf(base.Creature))
			{
				WitheringPresencePower witheringPresencePower = (WitheringPresencePower)ModelDb.Power<WitheringPresencePower>().ToMutable(0);
				witheringPresencePower.Target = creature;
				await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), witheringPresencePower, base.Creature, 6m, base.Creature, null, false);
			}
			IEnumerator<Creature> enumerator = null;
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x0025A52F File Offset: 0x0025872F
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("queen_progress", 5f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x0025A560 File Offset: 0x00258760
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("EBB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EbbMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.EbbDamage),
				new DefendIntent()
			});
			MoveState moveState2 = new MoveState("EYE_LASERS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EyeLasersMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.EyeLasersDamage, this.EyeLasersRepeat)
			});
			MoveState moveState3 = new MoveState("INCREASING_INTENSITY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IncreasingIntensityMove), new AbstractIntent[]
			{
				new StatusIntent(this.WitherAmount),
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x0025A63C File Offset: 0x0025883C
		private async Task EbbMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.EbbDamage).FromMonster(this).WithAttackerAnim("AttackHeavy", 0.3f, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.EbbBlock, ValueProp.Move, null, false);
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x0025A680 File Offset: 0x00258880
		private async Task EyeLasersMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.EyeLasersDamage).FromMonster(this).WithHitCount(this.EyeLasersRepeat)
				.WithAttackerAnim("AttackDouble", 0.4f, null)
				.OnlyPlayAnimOnce()
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x0025A6C4 File Offset: 0x002588C4
		private async Task IncreasingIntensityMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.4f);
			foreach (Creature creature in targets)
			{
				Player player = creature.Player;
				if (((player != null) ? player.PlayerCombatState : null) != null)
				{
					foreach (CardModel cardModel in creature.Player.PlayerCombatState.AllCards)
					{
						Wither wither = cardModel as Wither;
						if (wither != null)
						{
							wither.FakeUpgrade();
						}
					}
				}
			}
			this.WitherUpgradeCount++;
			await CardPileCmd.AddToCombatAndPreview<Wither>(targets, PileType.Discard, this.WitherAmount, null, CardPilePosition.Bottom);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.IncreasingIntensityTotalStrength, base.Creature, null, false);
			this.AdditionalStrength++;
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x0025A710 File Offset: 0x00258910
		public override Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			Wither wither = card as Wither;
			if (wither == null)
			{
				return Task.CompletedTask;
			}
			this.MatchWitherToUpgradeCount(wither);
			return Task.CompletedTask;
		}

		// Token: 0x06005F41 RID: 24385 RVA: 0x0025A73C File Offset: 0x0025893C
		public void MatchWitherToUpgradeCount(Wither wither)
		{
			for (int i = 0; i < this.WitherUpgradeCount; i++)
			{
				wither.FakeUpgrade();
			}
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x0025A760 File Offset: 0x00258960
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("wither", false);
			AnimState animState3 = new AnimState("attack_heavy", false);
			AnimState animState4 = new AnimState("attack_double", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("AttackHeavy", animState3, null);
			creatureAnimator.AddAnyState("AttackDouble", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x04002538 RID: 9528
		private const string _attackHeavyTrigger = "AttackHeavy";

		// Token: 0x04002539 RID: 9529
		private const string _attackDoubleTrigger = "AttackDouble";

		// Token: 0x0400253A RID: 9530
		private const string _aeonglassTrackName = "queen_progress";

		// Token: 0x0400253B RID: 9531
		private int _additionalStrength;

		// Token: 0x0400253C RID: 9532
		private int _witherUpgradeCount;
	}
}
