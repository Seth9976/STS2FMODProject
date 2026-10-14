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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007AB RID: 1963
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CubexConstruct : MonsterModel
	{
		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x06006083 RID: 24707 RVA: 0x0025DEE7 File Offset: 0x0025C0E7
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 70, 65);
			}
		}

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06006084 RID: 24708 RVA: 0x0025DEF3 File Offset: 0x0025C0F3
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06006085 RID: 24709 RVA: 0x0025DEFB File Offset: 0x0025C0FB
		private int BlastDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06006086 RID: 24710 RVA: 0x0025DF06 File Offset: 0x0025C106
		private int ExpelDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06006087 RID: 24711 RVA: 0x0025DF11 File Offset: 0x0025C111
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06006088 RID: 24712 RVA: 0x0025DF14 File Offset: 0x0025C114
		// (set) Token: 0x06006089 RID: 24713 RVA: 0x0025DF1C File Offset: 0x0025C11C
		private bool IsBurrowed
		{
			get
			{
				return this._isBurrowed;
			}
			set
			{
				base.AssertMutable();
				this._isBurrowed = value;
			}
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x0600608A RID: 24714 RVA: 0x0025DF2B File Offset: 0x0025C12B
		// (set) Token: 0x0600608B RID: 24715 RVA: 0x0025DF33 File Offset: 0x0025C133
		private bool IsCharging
		{
			get
			{
				return this._isCharging;
			}
			set
			{
				base.AssertMutable();
				this._isCharging = value;
			}
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0025DF44 File Offset: 0x0025C144
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(CubexConstruct._eyeOptions)));
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(CubexConstruct._mossOptions)));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0025DFA8 File Offset: 0x0025C1A8
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await CreatureCmd.GainBlock(base.Creature, 13m, ValueProp.Move, null, false);
			await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
			base.Creature.CurrentHpChanged += this.OnHpChanged;
			this.IsBurrowed = true;
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x0025DFEB File Offset: 0x0025C1EB
		public override void BeforeRemovedFromRoom()
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack");
			base.Creature.CurrentHpChanged -= this.OnHpChanged;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0025E014 File Offset: 0x0025C214
		private void OnHpChanged(int oldHp, int newHp)
		{
			if (newHp < oldHp)
			{
				SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "enemy_hurt", 1f);
			}
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0025E030 File Offset: 0x0025C230
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CHARGE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeUpMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("REPEATER_BLAST_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RepeaterBlastMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BlastDamage),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("REPEATER_BLAST_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.RepeaterBlastMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BlastDamage),
				new BuffIntent()
			});
			MoveState moveState4 = new MoveState("EXPEL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExpelBlastMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ExpelDamage, 2)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0025E140 File Offset: 0x0025C340
		private async Task ChargeUpMove(IReadOnlyList<Creature> targets)
		{
			this.IsBurrowed = false;
			this.IsCharging = true;
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 2f);
			await CreatureCmd.TriggerAnim(base.Creature, "Charge", 0f);
			await Cmd.Wait(0.75f, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x0025E184 File Offset: 0x0025C384
		private async Task RepeaterBlastMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 1f);
			await Cmd.Wait(0.4f, false);
			await DamageCmd.Attack(this.BlastDamage).FromMonster(this).WithAttackerAnim("Attack", 0f, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(null);
			SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 0f);
			await Cmd.Wait(0.2f, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
			await CreatureCmd.TriggerAnim(base.Creature, "AttackEnd", 0f);
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0025E1C8 File Offset: 0x0025C3C8
		private async Task ExpelBlastMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 1f);
			await Cmd.Wait(0.4f, false);
			await DamageCmd.Attack(this.ExpelDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0f, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(null);
			await Cmd.Wait(0.2f, false);
			await CreatureCmd.TriggerAnim(base.Creature, "AttackEnd", 0f);
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0025E20C File Offset: 0x0025C40C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("burrowed_loop", true)
			{
				BoundsContainer = "BurrowedBounds"
			};
			AnimState animState2 = new AnimState("burrow", false);
			AnimState animState3 = new AnimState("unburrow", false);
			AnimState animState4 = new AnimState("idle_loop", true)
			{
				BoundsContainer = "IdleBounds"
			};
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("hurt", false);
			AnimState animState8 = new AnimState("charge_start", false)
			{
				BoundsContainer = "ChargingBounds"
			};
			AnimState animState9 = new AnimState("charge_loop", true);
			AnimState animState10 = new AnimState("attack_loop", true);
			AnimState animState11 = new AnimState("attack_finish", false);
			animState2.NextState = animState;
			animState3.NextState = animState8;
			animState11.NextState = animState8;
			animState5.NextState = animState4;
			animState8.NextState = animState9;
			animState7.NextState = animState9;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState2, controller);
			creatureAnimator.AddAnyState("Charge", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState10, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("AttackEnd", animState11, null);
			creatureAnimator.AddAnyState("Hit", animState5, () => !this.IsBurrowed && !this.IsCharging);
			creatureAnimator.AddAnyState("Hit", animState7, () => !this.IsBurrowed && this.IsCharging);
			return creatureAnimator;
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x0025E374 File Offset: 0x0025C574
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.RemoveAll((BestiaryMonsterMove m) => m.stateId == "REPEATER_BLAST_MOVE");
			return list;
		}

		// Token: 0x0400257E RID: 9598
		private static readonly string[] _eyeOptions = new string[] { "diamondeye", "circleeye", "squareeye" };

		// Token: 0x0400257F RID: 9599
		private static readonly string[] _mossOptions = new string[] { "moss1", "moss2", "moss3" };

		// Token: 0x04002580 RID: 9600
		private const string _chargeTrigger = "Charge";

		// Token: 0x04002581 RID: 9601
		private const string _attackEndTrigger = "AttackEnd";

		// Token: 0x04002582 RID: 9602
		private const string _chargeStartAnimId = "charge_start";

		// Token: 0x04002583 RID: 9603
		private const int _expelRepeats = 2;

		// Token: 0x04002584 RID: 9604
		private const string _burrowSfx = "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_burrow";

		// Token: 0x04002585 RID: 9605
		private const string _chargedLoopSfx = "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack";

		// Token: 0x04002586 RID: 9606
		private bool _isBurrowed;

		// Token: 0x04002587 RID: 9607
		private bool _isCharging;
	}
}
