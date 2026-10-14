using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007F6 RID: 2038
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TestSubject : MonsterModel
	{
		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x06006514 RID: 25876 RVA: 0x0026BD44 File Offset: 0x00269F44
		public override LocString Title
		{
			get
			{
				LocString title = base.Title;
				title.Add("Count", SaveManager.Instance.Progress.TestSubjectKills + 8);
				return title;
			}
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06006515 RID: 25877 RVA: 0x0026BD7A File Offset: 0x00269F7A
		public override int MinInitialHp
		{
			get
			{
				return this.FirstFormHp;
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06006516 RID: 25878 RVA: 0x0026BD82 File Offset: 0x00269F82
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06006517 RID: 25879 RVA: 0x0026BD8A File Offset: 0x00269F8A
		public int FirstFormHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 111, 100);
			}
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06006518 RID: 25880 RVA: 0x0026BD96 File Offset: 0x00269F96
		public int SecondFormHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 212, 200);
			}
		}

		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x06006519 RID: 25881 RVA: 0x0026BDA8 File Offset: 0x00269FA8
		public int ThirdFormHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 313, 300);
			}
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x0600651A RID: 25882 RVA: 0x0026BDBA File Offset: 0x00269FBA
		private int EnrageAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x0600651B RID: 25883 RVA: 0x0026BDC5 File Offset: 0x00269FC5
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 22, 20);
			}
		}

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x0600651C RID: 25884 RVA: 0x0026BDD2 File Offset: 0x00269FD2
		private int SkullBashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x0600651D RID: 25885 RVA: 0x0026BDDF File Offset: 0x00269FDF
		private int MultiClawDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x0600651E RID: 25886 RVA: 0x0026BDEC File Offset: 0x00269FEC
		private int BaseMultiClawCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x0600651F RID: 25887 RVA: 0x0026BDEF File Offset: 0x00269FEF
		private int Phase3LacerateDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x06006520 RID: 25888 RVA: 0x0026BDFC File Offset: 0x00269FFC
		private int BigPounceDamage
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x06006521 RID: 25889 RVA: 0x0026BE00 File Offset: 0x0026A000
		private int BurningGrowlBurnCount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 3);
			}
		}

		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06006522 RID: 25890 RVA: 0x0026BE0B File Offset: 0x0026A00B
		private int BurningGrowlStrengthGain
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06006523 RID: 25891 RVA: 0x0026BE16 File Offset: 0x0026A016
		public override string DeathSfx
		{
			get
			{
				if (!base.IsMutable)
				{
					return base.DeathSfx;
				}
				if (!base.Creature.HasPower<AdaptablePower>())
				{
					return base.DeathSfx;
				}
				return "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_knock_out";
			}
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06006524 RID: 25892 RVA: 0x0026BE40 File Offset: 0x0026A040
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06006525 RID: 25893 RVA: 0x0026BE43 File Offset: 0x0026A043
		// (set) Token: 0x06006526 RID: 25894 RVA: 0x0026BE4B File Offset: 0x0026A04B
		private MoveState DeadState
		{
			get
			{
				return this._deadState;
			}
			set
			{
				base.AssertMutable();
				this._deadState = value;
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06006527 RID: 25895 RVA: 0x0026BE5A File Offset: 0x0026A05A
		// (set) Token: 0x06006528 RID: 25896 RVA: 0x0026BE62 File Offset: 0x0026A062
		private int Respawns
		{
			get
			{
				return this._respawns;
			}
			set
			{
				base.AssertMutable();
				this._respawns = value;
			}
		}

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06006529 RID: 25897 RVA: 0x0026BE71 File Offset: 0x0026A071
		// (set) Token: 0x0600652A RID: 25898 RVA: 0x0026BE79 File Offset: 0x0026A079
		private int ExtraMultiClawCount
		{
			get
			{
				return this._extraMultiClawCount;
			}
			set
			{
				base.AssertMutable();
				this._extraMultiClawCount = value;
			}
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x0600652B RID: 25899 RVA: 0x0026BE88 File Offset: 0x0026A088
		private int MultiClawTotalCount
		{
			get
			{
				return this.BaseMultiClawCount + this.ExtraMultiClawCount;
			}
		}

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x0600652C RID: 25900 RVA: 0x0026BE97 File Offset: 0x0026A097
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return this.Respawns >= 2;
			}
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x0026BEA8 File Offset: 0x0026A0A8
		public async Task TriggerDeadState()
		{
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("test_subject_progress", 1f);
			}
			ExtraRunFields extraFields = base.CombatState.RunState.ExtraFields;
			int testSubjectKills = extraFields.TestSubjectKills;
			extraFields.TestSubjectKills = testSubjectKills + 1;
			await CreatureCmd.TriggerAnim(base.Creature, "DeadTrigger", 0f);
			base.SetMoveImmediate(this.DeadState, true);
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x0026BEEC File Offset: 0x0026A0EC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<AdaptablePower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			await PowerCmd.Apply<EnragePower>(new ThrowingPlayerChoiceContext(), base.Creature, this.EnrageAmount, base.Creature, null, false);
			base.Creature.PowerApplied += this.AfterPowerApplied;
			base.Creature.PowerRemoved += this.AfterPowerRemoved;
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x0026BF2F File Offset: 0x0026A12F
		public override void BeforeRemovedFromRoom()
		{
			base.Creature.PowerApplied -= this.AfterPowerApplied;
			base.Creature.PowerRemoved -= this.AfterPowerRemoved;
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x0026BF60 File Offset: 0x0026A160
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			this.DeadState = new MoveState("RESPAWN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RespawnMove), new AbstractIntent[]
			{
				new HealIntent(),
				new BuffIntent()
			})
			{
				MustPerformOnceBeforeTransitioning = true
			};
			MoveState moveState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState2 = new MoveState("SKULL_BASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SkullBashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SkullBashDamage),
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("MULTI_CLAW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MultiClawMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.MultiClawDamage, () => this.MultiClawTotalCount)
			});
			MoveState moveState4 = new MoveState("PHASE3_LACERATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.Phase3LacerateMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.Phase3LacerateDamage, 3)
			});
			MoveState moveState5 = new MoveState("BIG_POUNCE", new Func<IReadOnlyList<Creature>, Task>(this.BigPounceMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BigPounceDamage)
			});
			MoveState moveState6 = new MoveState("BURNING_GROWL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurningGrowlMove), new AbstractIntent[]
			{
				new StatusIntent(this.BurningGrowlBurnCount),
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("REVIVE_BRANCH");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			moveState3.FollowUpState = moveState3;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState6;
			moveState6.FollowUpState = moveState4;
			this.DeadState.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(moveState3, () => this.Respawns < 2);
			conditionalBranchState.AddState(moveState4, () => this.Respawns >= 2);
			list.Add(this.DeadState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			list.Add(moveState6);
			list.Add(conditionalBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x0026C18C File Offset: 0x0026A38C
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			if (base.Creature.HasPower<AdaptablePower>())
			{
				return Task.CompletedTask;
			}
			this.SetColor(Colors.White);
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("test_subject_progress", 5f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x0026C1E5 File Offset: 0x0026A3E5
		private void AfterPowerApplied(PowerModel power)
		{
			if (power is IntangiblePower)
			{
				this.SetColor(StsColors.halfTransparentWhite);
			}
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x0026C1FA File Offset: 0x0026A3FA
		private void AfterPowerRemoved(PowerModel power)
		{
			if (power is IntangiblePower)
			{
				this.SetColor(Colors.White);
			}
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x0026C210 File Offset: 0x0026A410
		private async Task RespawnMove(IReadOnlyList<Creature> targets)
		{
			int respawns = this.Respawns;
			this.Respawns = respawns + 1;
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("test_subject_progress", 2f);
			}
			SfxCmd.Play((this.Respawns == 1) ? "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_two_heads" : "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_three_heads", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "RespawnTrigger", 0f);
			await Cmd.Wait(0.8f, false);
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode != null)
			{
				creatureNode.SetDefaultScaleTo(1f + (float)this.Respawns * 0.1f, 0.1f);
			}
			await Cmd.Wait(1.15f, false);
			AdaptablePower power = base.Creature.GetPower<AdaptablePower>();
			if (power != null)
			{
				power.DoRevive();
			}
			int respawns2 = this.Respawns;
			if (respawns2 != 1)
			{
				if (respawns2 == 2)
				{
					await this.Revive(this.ThirdFormHp);
					await PowerCmd.Apply<NemesisPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
					await PowerCmd.Remove<AdaptablePower>(base.Creature);
					await PowerCmd.Remove<PainfulStabsPower>(base.Creature);
				}
			}
			else
			{
				await this.Revive(this.SecondFormHp);
				await PowerCmd.Apply<PainfulStabsPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			}
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x0026C254 File Offset: 0x0026A454
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("BiteTrigger", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x0026C298 File Offset: 0x0026A498
		private async Task SkullBashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SkullBashDamage).FromMonster(this).WithAttackerAnim("BiteTrigger", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x0026C2E4 File Offset: 0x0026A4E4
		private async Task MultiClawMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.MultiClawDamage).WithHitCount(this.MultiClawTotalCount).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("MultiAttackTrigger", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash", null)
				.Execute(null);
			this.ExtraMultiClawCount++;
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x0026C328 File Offset: 0x0026A528
		private async Task Phase3LacerateMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.Phase3LacerateDamage).WithHitCount(3).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("MultiAttackTrigger", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash", null)
				.Execute(null);
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x0026C36C File Offset: 0x0026A56C
		private async Task BigPounceMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BigPounceDamage).FromMonster(this).WithAttackerAnim("BiteTrigger", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x0026C3B0 File Offset: 0x0026A5B0
		private async Task BurningGrowlMove(IReadOnlyList<Creature> targets)
		{
			this.SetColor(Colors.White);
			Control backVfxContainer = base.Creature.GetBackVfxContainer();
			if (backVfxContainer != null)
			{
				backVfxContainer.AddChildSafely(NTestSubjectBurnVfx.Create());
			}
			await CreatureCmd.TriggerAnim(base.Creature, "BurnTrigger", 1.25f);
			await CardPileCmd.AddToCombatAndPreview<Burn>(targets, PileType.Discard, this.BurningGrowlBurnCount, null, CardPilePosition.Bottom);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.BurningGrowlStrengthGain, base.Creature, null, false);
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x0026C3FC File Offset: 0x0026A5FC
		private async Task Revive(int baseRespawnHp)
		{
			base.AssertMutable();
			decimal scaledHp = Creature.ScaleHpForMultiplayer(baseRespawnHp, base.CombatState.Encounter, base.CombatState.Players.Count, base.CombatState.RunState.CurrentActIndex);
			await CreatureCmd.SetMaxHp(base.Creature, scaledHp);
			await CreatureCmd.Heal(base.Creature, scaledHp, true);
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x0026C448 File Offset: 0x0026A648
		private void SetColor(Color color)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(base.Creature) : null);
			if (ncreature != null)
			{
				CanvasGroup specialNode = ncreature.GetSpecialNode<CanvasGroup>("%CanvasGroup");
				if (specialNode == null)
				{
					return;
				}
				specialNode.SetSelfModulate(color);
			}
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x0026C488 File Offset: 0x0026A688
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop1", true);
			AnimState animState2 = new AnimState("hurt1", false);
			AnimState animState3 = new AnimState("attack_double1", false);
			AnimState animState4 = new AnimState("attack_big1", false);
			AnimState animState5 = new AnimState("heal1", false);
			AnimState animState6 = new AnimState("knockout1", false);
			AnimState animState7 = new AnimState("knocked_out_loop1", true);
			AnimState animState8 = new AnimState("regenerate1", false)
			{
				BoundsContainer = "RespawnBounds1"
			};
			AnimState animState9 = new AnimState("idle_loop2", true);
			AnimState animState10 = new AnimState("hurt2", false);
			AnimState animState11 = new AnimState("attack_double2", false);
			AnimState animState12 = new AnimState("attack_big2", false);
			AnimState animState13 = new AnimState("heal2", false);
			AnimState animState14 = new AnimState("knockout2", false);
			AnimState animState15 = new AnimState("knocked_out_loop2", true);
			AnimState animState16 = new AnimState("regenerate2", false)
			{
				BoundsContainer = "RespawnBounds2"
			};
			AnimState animState17 = new AnimState("idle_loop3", true);
			AnimState animState18 = new AnimState("hurt3", false);
			AnimState animState19 = new AnimState("attack_double3", false);
			AnimState animState20 = new AnimState("attack_big3", false);
			AnimState animState21 = new AnimState("burn", false);
			AnimState animState22 = new AnimState("heal3", false);
			AnimState animState23 = new AnimState("die", false);
			animState5.NextState = animState;
			animState4.NextState = animState;
			animState3.NextState = animState;
			animState2.NextState = animState;
			animState6.NextState = animState7;
			animState8.NextState = animState9;
			animState13.NextState = animState9;
			animState12.NextState = animState9;
			animState11.NextState = animState9;
			animState10.NextState = animState9;
			animState14.NextState = animState15;
			animState16.NextState = animState17;
			animState22.NextState = animState17;
			animState20.NextState = animState17;
			animState19.NextState = animState17;
			animState21.NextState = animState17;
			animState18.NextState = animState17;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Hit", animState2, () => this.Respawns == 0);
			creatureAnimator.AddAnyState("BiteTrigger", animState4, () => this.Respawns == 0);
			creatureAnimator.AddAnyState("MultiAttackTrigger", animState3, () => this.Respawns == 0);
			creatureAnimator.AddAnyState("GrowthSpurtTrigger", animState5, () => this.Respawns == 0);
			creatureAnimator.AddAnyState("DeadTrigger", animState6, () => this.Respawns == 0);
			creatureAnimator.AddAnyState("RespawnTrigger", animState8, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("Hit", animState10, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("BiteTrigger", animState12, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("MultiAttackTrigger", animState11, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("GrowthSpurtTrigger", animState13, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("DeadTrigger", animState14, () => this.Respawns == 1);
			creatureAnimator.AddAnyState("RespawnTrigger", animState16, () => this.Respawns >= 2);
			creatureAnimator.AddAnyState("Hit", animState18, () => this.Respawns >= 2);
			creatureAnimator.AddAnyState("BiteTrigger", animState20, () => this.Respawns >= 2);
			creatureAnimator.AddAnyState("MultiAttackTrigger", animState19, () => this.Respawns >= 2);
			creatureAnimator.AddAnyState("GrowthSpurtTrigger", animState22, () => this.Respawns >= 2);
			creatureAnimator.AddAnyState("Dead", animState23, null);
			creatureAnimator.AddAnyState("BurnTrigger", animState21, null);
			return creatureAnimator;
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x0026C82A File Offset: 0x0026AA2A
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "BITE_MOVE" && moveStateId != "SKULL_BASH_MOVE" && moveStateId != "PHASE3_LACERATE_MOVE";
		}

		// Token: 0x040026BD RID: 9917
		private const string _biteMove = "BITE_MOVE";

		// Token: 0x040026BE RID: 9918
		private const string _skullBashMove = "SKULL_BASH_MOVE";

		// Token: 0x040026BF RID: 9919
		private const string _lacerateMove = "PHASE3_LACERATE_MOVE";

		// Token: 0x040026C0 RID: 9920
		private const string _testSubjectCustomTrackName = "test_subject_progress";

		// Token: 0x040026C1 RID: 9921
		private const int _baseTestSubjectNum = 8;

		// Token: 0x040026C2 RID: 9922
		private const int _phase3LacerateRepeat = 3;

		// Token: 0x040026C3 RID: 9923
		private const string _growthSpurtTrigger = "GrowthSpurtTrigger";

		// Token: 0x040026C4 RID: 9924
		private const string _bigAttackTrigger = "BiteTrigger";

		// Token: 0x040026C5 RID: 9925
		private const string _multiAttackTrigger = "MultiAttackTrigger";

		// Token: 0x040026C6 RID: 9926
		private const string _deadTrigger = "DeadTrigger";

		// Token: 0x040026C7 RID: 9927
		private const string _respawnTrigger = "RespawnTrigger";

		// Token: 0x040026C8 RID: 9928
		private const string _burnTrigger = "BurnTrigger";

		// Token: 0x040026C9 RID: 9929
		private const string _biteSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite";

		// Token: 0x040026CA RID: 9930
		private const string _slashSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash";

		// Token: 0x040026CB RID: 9931
		private const string _knockOutSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_knock_out";

		// Token: 0x040026CC RID: 9932
		private const string _reviveTwoHeadsSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_two_heads";

		// Token: 0x040026CD RID: 9933
		private const string _reviveThreeHeadsSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_three_heads";

		// Token: 0x040026CE RID: 9934
		private MoveState _deadState;

		// Token: 0x040026CF RID: 9935
		private int _respawns;

		// Token: 0x040026D0 RID: 9936
		private int _extraMultiClawCount;
	}
}
