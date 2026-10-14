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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000807 RID: 2055
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TwoTailedRat : MonsterModel
	{
		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x06006662 RID: 26210 RVA: 0x0026FB27 File Offset: 0x0026DD27
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_die";
			}
		}

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x06006663 RID: 26211 RVA: 0x0026FB2E File Offset: 0x0026DD2E
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_hurt";
			}
		}

		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x06006664 RID: 26212 RVA: 0x0026FB35 File Offset: 0x0026DD35
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
			}
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x06006665 RID: 26213 RVA: 0x0026FB41 File Offset: 0x0026DD41
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x06006666 RID: 26214 RVA: 0x0026FB4D File Offset: 0x0026DD4D
		private int ScratchDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x06006667 RID: 26215 RVA: 0x0026FB59 File Offset: 0x0026DD59
		private int DiseaseBiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x06006668 RID: 26216 RVA: 0x0026FB64 File Offset: 0x0026DD64
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x06006669 RID: 26217 RVA: 0x0026FB67 File Offset: 0x0026DD67
		// (set) Token: 0x0600666A RID: 26218 RVA: 0x0026FB6F File Offset: 0x0026DD6F
		public int StarterMoveIndex
		{
			get
			{
				return this._starterMoveIndex;
			}
			set
			{
				base.AssertMutable();
				this._starterMoveIndex = value;
			}
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600666B RID: 26219 RVA: 0x0026FB7E File Offset: 0x0026DD7E
		// (set) Token: 0x0600666C RID: 26220 RVA: 0x0026FB86 File Offset: 0x0026DD86
		private int TurnsUntilSummonable
		{
			get
			{
				return this._turnsUntilSummonable;
			}
			set
			{
				base.AssertMutable();
				this._turnsUntilSummonable = value;
			}
		}

		/// <summary>
		/// Track how many times call for backup has happened this combat. This is so we can limit it to 3.
		/// </summary>
		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x0600666D RID: 26221 RVA: 0x0026FB95 File Offset: 0x0026DD95
		// (set) Token: 0x0600666E RID: 26222 RVA: 0x0026FB9D File Offset: 0x0026DD9D
		public int CallForBackupCount
		{
			get
			{
				return this._callForBackupCount;
			}
			set
			{
				base.AssertMutable();
				this._callForBackupCount = value;
			}
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x0026FBAC File Offset: 0x0026DDAC
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(TwoTailedRat._barnacleOptions)));
			megaSkin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>(TwoTailedRat._headOptions)));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x0026FC10 File Offset: 0x0026DE10
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x0026FC54 File Offset: 0x0026DE54
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SCRATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScratchMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ScratchDamage)
			});
			MoveState moveState2 = new MoveState("DISEASE_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DiseaseBiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DiseaseBiteDamage)
			});
			MoveState moveState3 = new MoveState("SCREECH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreechMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("CALL_FOR_BACKUP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CallForBackup), new AbstractIntent[]
			{
				new SummonIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			moveState4.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat, delegate
			{
				if (!this.CanSummon())
				{
					return 1f;
				}
				return 0.083333336f;
			});
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat, delegate
			{
				if (!this.CanSummon())
				{
					return 1f;
				}
				return 0.083333336f;
			});
			randomBranchState.AddBranch(moveState3, 3, MoveRepeatType.CannotRepeat, delegate
			{
				if (!this.CanSummon())
				{
					return 1f;
				}
				return 0.083333336f;
			});
			randomBranchState.AddBranch(moveState4, MoveRepeatType.UseOnlyOnce, delegate
			{
				if (!this.CanSummon())
				{
					return 0f;
				}
				return 0.75f;
			});
			list.Add(randomBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			if (this.StarterMoveIndex == -1)
			{
				return new MonsterMoveStateMachine(list, randomBranchState);
			}
			int num = this.StarterMoveIndex % 3;
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

		// Token: 0x06006672 RID: 26226 RVA: 0x0026FDF4 File Offset: 0x0026DFF4
		private async Task ScratchMove(IReadOnlyList<Creature> targets)
		{
			int turnsUntilSummonable = this.TurnsUntilSummonable;
			this.TurnsUntilSummonable = turnsUntilSummonable - 1;
			await DamageCmd.Attack(this.ScratchDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x0026FE38 File Offset: 0x0026E038
		private async Task DiseaseBiteMove(IReadOnlyList<Creature> targets)
		{
			int turnsUntilSummonable = this.TurnsUntilSummonable;
			this.TurnsUntilSummonable = turnsUntilSummonable - 1;
			await DamageCmd.Attack(this.DiseaseBiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x0026FE7C File Offset: 0x0026E07C
		private async Task ScreechMove(IReadOnlyList<Creature> targets)
		{
			int turnsUntilSummonable = this.TurnsUntilSummonable;
			this.TurnsUntilSummonable = turnsUntilSummonable - 1;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_bite", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.3f);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x0026FEC8 File Offset: 0x0026E0C8
		private async Task CallForBackup(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_summon", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Summon", 0.3f);
			if (base.CombatState.IsLiveCombat())
			{
				string nextSlot = base.CombatState.Encounter.Slots.LastOrDefault((string s) => this.CombatState.Enemies.All<Creature>((Creature c) => c.SlotName != s), string.Empty);
				if (!string.IsNullOrEmpty(nextSlot))
				{
					await Cmd.Wait(0.5f, false);
					await CreatureCmd.Add<TwoTailedRat>(base.CombatState, nextSlot);
				}
				List<TwoTailedRat> list = base.CombatState.Enemies.Select<Creature, MonsterModel>((Creature c) => c.Monster).OfType<TwoTailedRat>().ToList<TwoTailedRat>();
				int maxCallForBackupCount = list.Max<TwoTailedRat>((TwoTailedRat c) => c.CallForBackupCount + 1);
				list.ForEach(delegate(TwoTailedRat r)
				{
					r.CallForBackupCount = maxCallForBackupCount;
				});
			}
		}

		// Token: 0x06006676 RID: 26230 RVA: 0x0026FF0C File Offset: 0x0026E10C
		private bool CanSummon()
		{
			if (this.TurnsUntilSummonable > 0)
			{
				return false;
			}
			if (this.CallForBackupCount >= 3)
			{
				return false;
			}
			EncounterModel encounter = base.CombatState.Encounter;
			if (string.IsNullOrEmpty((encounter != null) ? encounter.GetNextSlot(base.CombatState) : null))
			{
				return false;
			}
			List<Creature> list = (from c in base.CombatState.GetTeammatesOf(base.Creature)
				where c != base.Creature
				select c).ToList<Creature>();
			foreach (Creature creature in list)
			{
				if (creature.Monster.NextMove.Id.Equals("CALL_FOR_BACKUP_MOVE"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x0026FFDC File Offset: 0x0026E1DC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("summon", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState3.NextState = animState;
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Summon", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x0400270D RID: 9997
		private const string _attackHandsSfx = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands";

		// Token: 0x0400270E RID: 9998
		private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_summon";

		// Token: 0x0400270F RID: 9999
		private const string _attackBite = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_bite";

		// Token: 0x04002710 RID: 10000
		private static readonly string[] _barnacleOptions = new string[] { "barnacle1", "barnacle1", "barnacle3" };

		// Token: 0x04002711 RID: 10001
		private static readonly string[] _headOptions = new string[] { "head1", "head2", "head3" };

		// Token: 0x04002712 RID: 10002
		private const string _callForBackupMoveId = "CALL_FOR_BACKUP_MOVE";

		// Token: 0x04002713 RID: 10003
		private const float _callForBackupChance = 0.75f;

		// Token: 0x04002714 RID: 10004
		private const int _callForBackupLimit = 3;

		// Token: 0x04002715 RID: 10005
		private int _starterMoveIndex = -1;

		// Token: 0x04002716 RID: 10006
		private int _turnsUntilSummonable = 2;

		// Token: 0x04002717 RID: 10007
		private int _callForBackupCount;

		// Token: 0x04002718 RID: 10008
		private const string _summonTrigger = "Summon";
	}
}
