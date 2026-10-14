using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A6 RID: 1958
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CeremonialBeast : MonsterModel
	{
		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x06006000 RID: 24576 RVA: 0x0025C889 File Offset: 0x0025AA89
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 262, 252);
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x06006001 RID: 24577 RVA: 0x0025C89B File Offset: 0x0025AA9B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06006002 RID: 24578 RVA: 0x0025C8A3 File Offset: 0x0025AAA3
		private int PlowAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 160, 150);
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x06006003 RID: 24579 RVA: 0x0025C8B6 File Offset: 0x0025AAB6
		private int PlowDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x06006004 RID: 24580 RVA: 0x0025C8C3 File Offset: 0x0025AAC3
		private int PlowStrength
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06006005 RID: 24581 RVA: 0x0025C8C6 File Offset: 0x0025AAC6
		private int StompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06006006 RID: 24582 RVA: 0x0025C8D3 File Offset: 0x0025AAD3
		private int CrushDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06006007 RID: 24583 RVA: 0x0025C8E0 File Offset: 0x0025AAE0
		private int CrushStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06006008 RID: 24584 RVA: 0x0025C8EB File Offset: 0x0025AAEB
		public override bool ShouldDisappearFromDoom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06006009 RID: 24585 RVA: 0x0025C8EE File Offset: 0x0025AAEE
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x0600600A RID: 24586 RVA: 0x0025C8F1 File Offset: 0x0025AAF1
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_die";
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x0600600B RID: 24587 RVA: 0x0025C8F8 File Offset: 0x0025AAF8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600600C RID: 24588 RVA: 0x0025C8FB File Offset: 0x0025AAFB
		// (set) Token: 0x0600600D RID: 24589 RVA: 0x0025C903 File Offset: 0x0025AB03
		private bool IsStunnedByPlowRemoval
		{
			get
			{
				return this._isStunnedByPlowRemoval;
			}
			set
			{
				base.AssertMutable();
				this._isStunnedByPlowRemoval = value;
			}
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600600E RID: 24590 RVA: 0x0025C912 File Offset: 0x0025AB12
		// (set) Token: 0x0600600F RID: 24591 RVA: 0x0025C91A File Offset: 0x0025AB1A
		public bool IsInSecondPhase
		{
			get
			{
				return this._isInSecondPhase;
			}
			private set
			{
				base.AssertMutable();
				this._isInSecondPhase = value;
			}
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06006010 RID: 24592 RVA: 0x0025C929 File Offset: 0x0025AB29
		private bool ShouldPlayRegularHurtAnim
		{
			get
			{
				return !this.IsStunnedByPlowRemoval && !this.InMidCharge;
			}
		}

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06006011 RID: 24593 RVA: 0x0025C93E File Offset: 0x0025AB3E
		// (set) Token: 0x06006012 RID: 24594 RVA: 0x0025C946 File Offset: 0x0025AB46
		private bool InMidCharge
		{
			get
			{
				return this._inMidCharge;
			}
			set
			{
				base.AssertMutable();
				this._inMidCharge = value;
			}
		}

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06006013 RID: 24595 RVA: 0x0025C955 File Offset: 0x0025AB55
		// (set) Token: 0x06006014 RID: 24596 RVA: 0x0025C95D File Offset: 0x0025AB5D
		public MoveState BeastCryState
		{
			get
			{
				return this._beastCryState;
			}
			set
			{
				base.AssertMutable();
				this._beastCryState = value;
			}
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0025C96C File Offset: 0x0025AB6C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("STAMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StampMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("PLOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PlowMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PlowDamage),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("STUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove), new AbstractIntent[]
			{
				new StunIntent()
			})
			{
				MustPerformOnceBeforeTransitioning = true
			};
			this.BeastCryState = new MoveState("BEAST_CRY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeastCryMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StompDamage)
			});
			MoveState moveState5 = new MoveState("CRUSH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrushMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.CrushDamage),
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			moveState3.FollowUpState = this.BeastCryState;
			this.BeastCryState.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = this.BeastCryState;
			list.Add(moveState2);
			list.Add(moveState);
			list.Add(moveState3);
			list.Add(this.BeastCryState);
			list.Add(moveState4);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0025CB04 File Offset: 0x0025AD04
		private async Task StampMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.AttackSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Attack", 0.6f);
			await Cmd.CustomScaledWait(0f, 0.4f, false, default(CancellationToken));
			await PowerCmd.Apply<PlowPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PlowAmount, base.Creature, null, false);
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0025CB48 File Offset: 0x0025AD48
		private async Task PlowMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow", 1f);
			this.InMidCharge = true;
			await CreatureCmd.TriggerAnim(base.Creature, "Plow", 0f);
			await Cmd.Wait(0.5f, false);
			Control vfxContainer = base.Creature.GetVfxContainer();
			if (vfxContainer != null)
			{
				vfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(new Color("BFFFC880"), 1.2, false));
			}
			await Cmd.Wait(0.5f, false);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.RadialBlur(VfxPosition.Left);
			}
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_attack_blunt");
			using (IEnumerator<Creature> enumerator = targets.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Creature creature = enumerator.Current;
					Control vfxContainer2 = creature.GetVfxContainer();
					if (vfxContainer2 != null)
					{
						vfxContainer2.AddChildSafely(NLineBurstVfx.Create(creature));
					}
				}
			}
			NGame instance2 = NGame.Instance;
			if (instance2 != null)
			{
				instance2.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
			}
			await DamageCmd.Attack(this.PlowDamage).FromMonster(this).WithNoAttackerAnim()
				.Execute(null);
			NGame instance3 = NGame.Instance;
			if (instance3 != null)
			{
				instance3.DoHitStop(ShakeStrength.Strong, ShakeDuration.Normal);
			}
			this.InMidCharge = false;
			await Cmd.Wait(0.2f, false);
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow_end", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "EndPlow", 0f);
			await Cmd.Wait(0.5f, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PlowStrength, base.Creature, null, false);
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0025CB94 File Offset: 0x0025AD94
		public async Task SetStunned()
		{
			this.IsStunnedByPlowRemoval = true;
			this.IsInSecondPhase = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_stun", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Stun", 0.6f);
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x0025CBD8 File Offset: 0x0025ADD8
		public async Task StunnedMove(IReadOnlyList<Creature> targets)
		{
			this.IsStunnedByPlowRemoval = false;
			await CreatureCmd.TriggerAnim(base.Creature, "Unstun", 0.6f);
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0025CC1C File Offset: 0x0025AE1C
		private async Task BeastCryMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_shrill", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0f);
			await Cmd.Wait(0.3f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_scream");
			await Cmd.Wait(0.75f, false);
			await PowerCmd.Apply<RingingPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x0025CC68 File Offset: 0x0025AE68
		private async Task StompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StompDamage).FromMonster(this).WithAttackerAnim("Attack", 1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.AfterAttackerAnim(delegate
				{
					NGame instance = NGame.Instance;
					if (instance != null)
					{
						instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
					}
					return Task.CompletedTask;
				})
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.WithHitVfxNode((Creature _) => NSpikeSplashVfx.Create(base.Creature, VfxColor.Cyan))
				.Execute(null);
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x0025CCAC File Offset: 0x0025AEAC
		private async Task CrushMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.AttackSfx, 1f);
			await DamageCmd.Attack(this.CrushDamage).FromMonster(this).WithAttackerAnim("Attack", 1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.AfterAttackerAnim(delegate
				{
					NGame instance = NGame.Instance;
					if (instance != null)
					{
						instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
					}
					return Task.CompletedTask;
				})
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.WithHitVfxNode((Creature _) => NSpikeSplashVfx.Create(base.Creature, VfxColor.Cyan))
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.CrushStrength, base.Creature, null, false);
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0025CCF0 File Offset: 0x0025AEF0
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("shrill", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("plow", false);
			AnimState animState5 = new AnimState("plow_end", false);
			AnimState animState6 = new AnimState("plow_end_die", false);
			AnimState animState7 = new AnimState("stun", false);
			AnimState animState8 = new AnimState("stun_loop", true);
			AnimState animState9 = new AnimState("wake_up", false);
			AnimState animState10 = new AnimState("hurt", false);
			AnimState animState11 = new AnimState("die", false);
			animState.AddBranch("Plow", animState4, null);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState10.NextState = animState;
			animState4.AddBranch("EndPlow", animState5, null);
			animState5.NextState = animState;
			animState7.NextState = animState8;
			animState9.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Unstun", animState9, null);
			creatureAnimator.AddAnyState("Dead", animState11, () => !this.InMidCharge);
			creatureAnimator.AddAnyState("Dead", animState6, () => this.InMidCharge);
			animState.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState2.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState10.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState3.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState7.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState8.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState9.AddBranch("Hit", animState10, () => this.ShouldPlayRegularHurtAnim);
			animState.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState2.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState10.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState3.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState7.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState8.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			animState9.AddBranch("Hit", animState7, () => this.IsStunnedByPlowRemoval);
			creatureAnimator.AddAnyState("Stun", animState7, null);
			creatureAnimator.AddAnyState("Plow", animState4, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("PlowHit", animState10, null);
			return creatureAnimator;
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0025CFCE File Offset: 0x0025B1CE
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "STUN_MOVE";
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x0025CFDC File Offset: 0x0025B1DC
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.RemoveAll(delegate(BestiaryMonsterMove m)
			{
				string stateId = m.stateId;
				return stateId == "STAMP_MOVE" || stateId == "CRUSH_MOVE";
			});
			list.Insert(2, BestiaryMonsterMove.FromStun(new Func<Task>(this.SetStunned)));
			return list;
		}

		// Token: 0x04002559 RID: 9561
		private const string _stunMove = "STUN_MOVE";

		// Token: 0x0400255A RID: 9562
		private const string _plowTrigger = "Plow";

		// Token: 0x0400255B RID: 9563
		private const string _plowEndTrigger = "EndPlow";

		// Token: 0x0400255C RID: 9564
		private const string _stunTrigger = "Stun";

		// Token: 0x0400255D RID: 9565
		private const string _unStunTrigger = "Unstun";

		// Token: 0x0400255E RID: 9566
		private const string _plowHitTrigger = "PlowHit";

		// Token: 0x0400255F RID: 9567
		private const string _plowSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow";

		// Token: 0x04002560 RID: 9568
		private const string _plowEndSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow_end";

		// Token: 0x04002561 RID: 9569
		private const string _shrillSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_shrill";

		// Token: 0x04002562 RID: 9570
		private const string _stunSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_stun";

		// Token: 0x04002563 RID: 9571
		private bool _isStunnedByPlowRemoval;

		// Token: 0x04002564 RID: 9572
		private bool _isInSecondPhase;

		// Token: 0x04002565 RID: 9573
		private bool _inMidCharge;

		// Token: 0x04002566 RID: 9574
		private MoveState _beastCryState;
	}
}
