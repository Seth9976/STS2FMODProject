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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200080A RID: 2058
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WaterfallGiant : MonsterModel
	{
		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x060066A1 RID: 26273 RVA: 0x0027093F File Offset: 0x0026EB3F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 250, 240);
			}
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x060066A2 RID: 26274 RVA: 0x00270951 File Offset: 0x0026EB51
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x060066A3 RID: 26275 RVA: 0x00270959 File Offset: 0x0026EB59
		public int SiphonHeal
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 10);
			}
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x060066A4 RID: 26276 RVA: 0x00270965 File Offset: 0x0026EB65
		private int PressurizeAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 15);
			}
		}

		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x060066A5 RID: 26277 RVA: 0x00270972 File Offset: 0x0026EB72
		private int StompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 15);
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x060066A6 RID: 26278 RVA: 0x0027097F File Offset: 0x0026EB7F
		private int RamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x060066A7 RID: 26279 RVA: 0x0027098C File Offset: 0x0026EB8C
		private int PressureUpDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
			}
		}

		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x060066A8 RID: 26280 RVA: 0x00270999 File Offset: 0x0026EB99
		private int BasePressureGunDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 20);
			}
		}

		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x060066A9 RID: 26281 RVA: 0x002709A6 File Offset: 0x0026EBA6
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return !base.Creature.HasPower<SteamEruptionPower>();
			}
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x060066AA RID: 26282 RVA: 0x002709B6 File Offset: 0x0026EBB6
		private int PressureGunIncrease
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x060066AB RID: 26283 RVA: 0x002709B9 File Offset: 0x0026EBB9
		// (set) Token: 0x060066AC RID: 26284 RVA: 0x002709C1 File Offset: 0x0026EBC1
		private int CurrentPressureGunDamage
		{
			get
			{
				return this._currentPressureGunDamage;
			}
			set
			{
				base.AssertMutable();
				this._currentPressureGunDamage = value;
			}
		}

		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x060066AD RID: 26285 RVA: 0x002709D0 File Offset: 0x0026EBD0
		// (set) Token: 0x060066AE RID: 26286 RVA: 0x002709D8 File Offset: 0x0026EBD8
		private int SteamEruptionDamage
		{
			get
			{
				return this._steamEruptionDamage;
			}
			set
			{
				base.AssertMutable();
				this._steamEruptionDamage = value;
			}
		}

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x060066AF RID: 26287 RVA: 0x002709E7 File Offset: 0x0026EBE7
		// (set) Token: 0x060066B0 RID: 26288 RVA: 0x002709EF File Offset: 0x0026EBEF
		private MoveState AboutToBlowState
		{
			get
			{
				return this._aboutToBlowState;
			}
			set
			{
				base.AssertMutable();
				this._aboutToBlowState = value;
			}
		}

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x060066B1 RID: 26289 RVA: 0x002709FE File Offset: 0x0026EBFE
		// (set) Token: 0x060066B2 RID: 26290 RVA: 0x00270A06 File Offset: 0x0026EC06
		private bool IsAboutToBlow
		{
			get
			{
				return this._isAboutToBlow;
			}
			set
			{
				base.AssertMutable();
				this._isAboutToBlow = value;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x060066B3 RID: 26291 RVA: 0x00270A15 File Offset: 0x0026EC15
		// (set) Token: 0x060066B4 RID: 26292 RVA: 0x00270A1D File Offset: 0x0026EC1D
		private int PressureBuildupIdx
		{
			get
			{
				return this._pressureBuildupIdx;
			}
			set
			{
				base.AssertMutable();
				this._pressureBuildupIdx = value;
			}
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x060066B5 RID: 26293 RVA: 0x00270A2C File Offset: 0x0026EC2C
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x060066B6 RID: 26294 RVA: 0x00270A2F File Offset: 0x0026EC2F
		public override bool HasDeathSfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x060066B7 RID: 26295 RVA: 0x00270A32 File Offset: 0x0026EC32
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return this.PressureBuildupIdx == 0;
			}
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x00270A40 File Offset: 0x0026EC40
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			this.CurrentPressureGunDamage = this.BasePressureGunDamage;
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 2f);
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x00270A84 File Offset: 0x0026EC84
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			if (base.Creature.HasPower<SteamEruptionPower>())
			{
				return Task.CompletedTask;
			}
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("waterfall_giant_progress", 5f);
			}
			this.StopAmbientSfx();
			return Task.CompletedTask;
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x00270AD8 File Offset: 0x0026ECD8
		public override void BeforeRemovedFromRoom()
		{
			this.StopAmbientSfx();
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x00270AE0 File Offset: 0x0026ECE0
		private void StopAmbientSfx()
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient");
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x00270AF4 File Offset: 0x0026ECF4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("PRESSURIZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressurizeMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StompDamage),
				new DebuffIntent(false),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.RamDamage),
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("SIPHON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SiphonMove), new AbstractIntent[]
			{
				new HealIntent(),
				new BuffIntent()
			});
			MoveState moveState5 = new MoveState("PRESSURE_GUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressureGunMove), new AbstractIntent[]
			{
				new SingleAttackIntent(() => this.CurrentPressureGunDamage),
				new BuffIntent()
			});
			MoveState moveState6 = new MoveState("PRESSURE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressureUpMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PressureUpDamage),
				new BuffIntent()
			});
			this.AboutToBlowState = new MoveState("ABOUT_TO_BLOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AboutToBlowMove), new AbstractIntent[]
			{
				new StunIntent()
			})
			{
				MustPerformOnceBeforeTransitioning = true
			};
			MoveState moveState7 = new MoveState("EXPLODE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplodeMove), new AbstractIntent[]
			{
				new DeathBlowIntent(() => this.SteamEruptionDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState6;
			moveState6.FollowUpState = moveState2;
			this.AboutToBlowState.FollowUpState = moveState7;
			moveState7.FollowUpState = moveState7;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			list.Add(moveState6);
			list.Add(moveState7);
			list.Add(this.AboutToBlowState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x00270D28 File Offset: 0x0026EF28
		private async Task PressurizeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Heal", 0.8f);
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PressurizeAmount, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x00270D6C File Offset: 0x0026EF6C
		private async Task PressureUpMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PressureUpDamage).FromMonster(this).WithAttackerAnim("AttackBuff", 0.15f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x00270DB0 File Offset: 0x0026EFB0
		private async Task StompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StompDamage).FromMonster(this).WithAttackerAnim("AttackDebuff", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x00270DFC File Offset: 0x0026EFFC
		private async Task RamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.RamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x00270E40 File Offset: 0x0026F040
		private async Task SiphonMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Heal", 0.8f);
			await CreatureCmd.Heal(base.Creature, this.SiphonHeal * base.CombatState.Players.Count, true);
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x00270E84 File Offset: 0x0026F084
		private async Task PressureGunMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.CurrentPressureGunDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			this.CurrentPressureGunDamage += this.PressureGunIncrease;
			await PowerCmd.Apply<SteamEruptionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x00270EC8 File Offset: 0x0026F0C8
		private async Task AboutToBlowMove(IReadOnlyList<Creature> targets)
		{
			this.SteamEruptionDamage = base.Creature.GetPowerAmount<SteamEruptionPower>();
			await PowerCmd.Remove<SteamEruptionPower>(base.Creature);
			this.PressureBuildupIdx = 6;
			this.IncrementBuildUpAnimationTrack();
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x00270F0C File Offset: 0x0026F10C
		private async Task ExplodeMove(IReadOnlyList<Creature> targets)
		{
			this.StopAmbientSfx();
			await DamageCmd.Attack(this.SteamEruptionDamage).FromMonster(this).WithAttackerAnim("Erupt", 0.1f, null)
				.WithAttackerFx(null, this.DeathSfx, null)
				.Execute(null);
			await CreatureCmd.Kill(base.Creature, false);
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("waterfall_giant_progress", 5f);
			}
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x00270F50 File Offset: 0x0026F150
		public async Task TriggerAboutToBlowState()
		{
			this.IsAboutToBlow = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_knockout", 1f);
			await CreatureCmd.SetMaxAndCurrentHp(base.Creature, 999999999m);
			base.Creature.HpDisplay = HpDisplay.InfiniteWithoutNumbers;
			base.SetMoveImmediate(this.AboutToBlowState, true);
			if (base.CombatState.IsLiveCombat())
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("waterfall_giant_progress", 2f);
				}
				SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 3f);
			}
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x00270F94 File Offset: 0x0026F194
		private void IncrementBuildUpAnimationTrack()
		{
			if (!TestMode.IsOff)
			{
				return;
			}
			if (base.CombatState.IsLiveCombat())
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("waterfall_giant_progress", 1f);
				}
				SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 1f);
			}
			int pressureBuildupIdx = this.PressureBuildupIdx;
			this.PressureBuildupIdx = pressureBuildupIdx + 1;
			int num = Mathf.FloorToInt((float)this.PressureBuildupIdx * 0.5f);
			num = Mathf.Clamp(num, 1, 3);
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode != null)
			{
				SpineAnimationAccess spineAnimation = creatureNode.SpineAnimation;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("_tracks/buildup");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				spineAnimation.SetAnimation(defaultInterpolatedStringHandler.ToStringAndClear(), true, 1);
			}
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x00271054 File Offset: 0x0026F254
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_buff", false);
			AnimState animState5 = new AnimState("attack_debuff", false);
			AnimState animState6 = new AnimState("heal", false);
			AnimState animState7 = new AnimState("hurt", false);
			AnimState animState8 = new AnimState("die", false);
			AnimState animState9 = new AnimState("die_loop", true);
			AnimState animState10 = new AnimState("erupt", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			animState7.NextState = animState;
			animState8.NextState = animState9;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState8, () => !this.IsAboutToBlow);
			creatureAnimator.AddAnyState("Hit", animState7, () => !this.IsAboutToBlow);
			creatureAnimator.AddAnyState("AttackBuff", animState4, null);
			creatureAnimator.AddAnyState("AttackDebuff", animState5, null);
			creatureAnimator.AddAnyState("Heal", animState6, null);
			creatureAnimator.AddAnyState("Erupt", animState10, null);
			return creatureAnimator;
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x002711AB File Offset: 0x0026F3AB
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SIPHON_MOVE";
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x002711B8 File Offset: 0x0026F3B8
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.RemoveAll(delegate(BestiaryMonsterMove m)
			{
				string stateId = m.stateId;
				return stateId == "PRESSURE_UP_MOVE" || stateId == "PRESSURE_GUN_MOVE";
			});
			return list;
		}

		// Token: 0x04002731 RID: 10033
		private const string _siphonMove = "SIPHON_MOVE";

		// Token: 0x04002732 RID: 10034
		private const string _waterfallGiantTrackName = "waterfall_giant_progress";

		// Token: 0x04002733 RID: 10035
		private const int _endCombatBgmFlag = 5;

		// Token: 0x04002734 RID: 10036
		private const int _maxIntensityBgmFlag = 2;

		// Token: 0x04002735 RID: 10037
		private const int _increaseIntensityBgmFlag = 1;

		// Token: 0x04002736 RID: 10038
		private const int _increaseIntensityAmbienceFlag = 1;

		// Token: 0x04002737 RID: 10039
		private const int _maxIntensityAmbienceFlag = 3;

		// Token: 0x04002738 RID: 10040
		private const int _endAmbienceFlag = 2;

		// Token: 0x04002739 RID: 10041
		private int _currentPressureGunDamage;

		// Token: 0x0400273A RID: 10042
		private int _steamEruptionDamage;

		// Token: 0x0400273B RID: 10043
		private MoveState _aboutToBlowState;

		// Token: 0x0400273C RID: 10044
		private bool _isAboutToBlow;

		// Token: 0x0400273D RID: 10045
		private int _pressureBuildupIdx;

		// Token: 0x0400273E RID: 10046
		private const int _maxPressureBuildup = 6;

		// Token: 0x0400273F RID: 10047
		private const string _attackBuffTrigger = "AttackBuff";

		// Token: 0x04002740 RID: 10048
		private const string _attackDebuffTrigger = "AttackDebuff";

		// Token: 0x04002741 RID: 10049
		private const string _healTrigger = "Heal";

		// Token: 0x04002742 RID: 10050
		private const string _eruptTrigger = "Erupt";

		// Token: 0x04002743 RID: 10051
		private const string _attackKickSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick";

		// Token: 0x04002744 RID: 10052
		private const string _attackStompSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp";

		// Token: 0x04002745 RID: 10053
		private const string _eruptionSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption";

		// Token: 0x04002746 RID: 10054
		private const string _knockoutSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_knockout";

		// Token: 0x04002747 RID: 10055
		private const string _ambientSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient";
	}
}
