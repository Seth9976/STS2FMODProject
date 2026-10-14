using System;
using System.Collections.Generic;
using System.Linq;
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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C8 RID: 1992
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KinPriest : MonsterModel
	{
		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x06006228 RID: 25128 RVA: 0x00263068 File Offset: 0x00261268
		protected override string CastSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_cast";
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x06006229 RID: 25129 RVA: 0x0026306F File Offset: 0x0026126F
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_hurt";
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x0600622A RID: 25130 RVA: 0x00263076 File Offset: 0x00261276
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_die";
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600622B RID: 25131 RVA: 0x0026307D File Offset: 0x0026127D
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 199, 190);
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x0600622C RID: 25132 RVA: 0x0026308F File Offset: 0x0026128F
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x0600622D RID: 25133 RVA: 0x00263097 File Offset: 0x00261297
		private int OrbOfFrailtyDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x0600622E RID: 25134 RVA: 0x002630A3 File Offset: 0x002612A3
		private int OrbOfWeaknessDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x0600622F RID: 25135 RVA: 0x002630AF File Offset: 0x002612AF
		private int BeamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x06006230 RID: 25136 RVA: 0x002630BA File Offset: 0x002612BA
		private int RitualStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x06006231 RID: 25137 RVA: 0x002630C5 File Offset: 0x002612C5
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x06006232 RID: 25138 RVA: 0x002630C8 File Offset: 0x002612C8
		// (set) Token: 0x06006233 RID: 25139 RVA: 0x002630D0 File Offset: 0x002612D0
		private bool SpeechUsed
		{
			get
			{
				return this._speechUsed;
			}
			set
			{
				base.AssertMutable();
				this._speechUsed = value;
			}
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x002630E0 File Offset: 0x002612E0
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature.Monster is KinFollower)
			{
				if (base.Creature.IsDead)
				{
					return Task.CompletedTask;
				}
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("the_kin_progress", 1f);
				}
				IReadOnlyList<Creature> teammatesOf = base.CombatState.GetTeammatesOf(base.Creature);
				if (!teammatesOf.Any<Creature>((Creature c) => c != null && c.Monster is KinFollower && c.IsAlive))
				{
					Creature creature2 = teammatesOf.FirstOrDefault<Creature>((Creature c) => c != null && c.Monster is KinPriest && c.IsAlive);
					if (creature2 != null)
					{
						KinPriest kinPriest = creature2.Monster as KinPriest;
						if (kinPriest != null)
						{
							kinPriest.AllFollowerDeathResponse();
						}
					}
				}
			}
			else if (creature == base.Creature)
			{
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("the_kin_progress", 5f);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x002631CC File Offset: 0x002613CC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ORB_OF_FRAILTY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OrbOfFrailtyMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.OrbOfFrailtyDamage),
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("ORB_OF_WEAKNESS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OrbOfWeaknessMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.OrbOfWeaknessDamage),
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeamMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.BeamDamage, 3)
			});
			MoveState moveState4 = new MoveState("RITUAL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RitualMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x002632DC File Offset: 0x002614DC
		private async Task OrbOfFrailtyMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.OrbOfFrailtyDamage).FromMonster(this).WithAttackerAnim("AttackGrenade", 0f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade", null)
				.WithWaitBeforeHit(1f, 1f)
				.WithHitVfxNode((Creature t) => NKinPriestGrenadeVfx.Create(t))
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x00263328 File Offset: 0x00261528
		private async Task OrbOfWeaknessMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.OrbOfWeaknessDamage).FromMonster(this).WithAttackerAnim("AttackGrenade", 0f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade", null)
				.WithWaitBeforeHit(1f, 1f)
				.WithHitVfxNode((Creature t) => NKinPriestGrenadeVfx.Create(t))
				.Execute(null);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x00263374 File Offset: 0x00261574
		private async Task BeamMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BeamDamage).WithHitCount(3).FromMonster(this)
				.WithAttackerAnim("AttackLaser", 0.4f, null)
				.AfterAttackerAnim(delegate
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(base.Creature) : null);
					if (ncreature != null)
					{
						NKinPriestBeamVfx specialNode = ncreature.GetSpecialNode<NKinPriestBeamVfx>("Visuals/Beam");
						if (specialNode != null)
						{
							specialNode.Fire();
						}
					}
					SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_beam", 1f);
					return Task.CompletedTask;
				})
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.OnlyPlayAnimOnce()
				.Execute(null);
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x002633B8 File Offset: 0x002615B8
		private async Task RitualMove(IReadOnlyList<Creature> targets)
		{
			if (!this.SpeechUsed)
			{
				this.SpeechUsed = true;
				TalkCmd.Play(KinPriest._ritualApplyLine, base.Creature, VfxColor.Purple, VfxDuration.Standard);
			}
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_rally", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Rally", 1f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.RitualStrength, base.Creature, null, false);
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x002633FC File Offset: 0x002615FC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("rally", false);
			AnimState animState3 = new AnimState("attack_grenade", false);
			AnimState animState4 = new AnimState("attack_laser", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Rally", animState2, null);
			creatureAnimator.AddAnyState("AttackGrenade", animState3, null);
			creatureAnimator.AddAnyState("AttackLaser", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x002634C3 File Offset: 0x002616C3
		private void AllFollowerDeathResponse()
		{
			TalkCmd.Play(KinPriest._followersDeathLine, base.Creature, VfxColor.Purple, VfxDuration.Standard);
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x002634D8 File Offset: 0x002616D8
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.RemoveAll((BestiaryMonsterMove m) => m.stateId == "ORB_OF_WEAKNESS_MOVE");
			return list;
		}

		// Token: 0x040025E5 RID: 9701
		private const string _theKinCustomTrackName = "the_kin_progress";

		// Token: 0x040025E6 RID: 9702
		private static readonly LocString _ritualApplyLine = MonsterModel.L10NMonsterLookup("KIN_PRIEST.moves.RITUAL.speakLine1");

		// Token: 0x040025E7 RID: 9703
		private static readonly LocString _followersDeathLine = MonsterModel.L10NMonsterLookup("KIN_PRIEST.followersDeathLine");

		// Token: 0x040025E8 RID: 9704
		private const string _grenadeTrigger = "AttackGrenade";

		// Token: 0x040025E9 RID: 9705
		private const string _laserTrigger = "AttackLaser";

		// Token: 0x040025EA RID: 9706
		private const string _rallyTrigger = "Rally";

		// Token: 0x040025EB RID: 9707
		private const string _attackGrenadeAnimId = "attack_grenade";

		// Token: 0x040025EC RID: 9708
		private const string _soulBeamSfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_beam";

		// Token: 0x040025ED RID: 9709
		private const string _soulGrenadeSfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade";

		// Token: 0x040025EE RID: 9710
		private const string _rallySfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_rally";

		// Token: 0x040025EF RID: 9711
		private const int _beamRepeat = 3;

		// Token: 0x040025F0 RID: 9712
		private bool _speechUsed;
	}
}
