using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007F0 RID: 2032
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulNexus : MonsterModel
	{
		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x060064C1 RID: 25793 RVA: 0x0026AEAE File Offset: 0x002690AE
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 254, 234);
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x060064C2 RID: 25794 RVA: 0x0026AEC0 File Offset: 0x002690C0
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x060064C3 RID: 25795 RVA: 0x0026AEC8 File Offset: 0x002690C8
		private int SoulBurnDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31, 29);
			}
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x060064C4 RID: 25796 RVA: 0x0026AED5 File Offset: 0x002690D5
		private int MaelstromDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x060064C5 RID: 25797 RVA: 0x0026AEE0 File Offset: 0x002690E0
		private int MaelstromRepeat
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
			}
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x060064C6 RID: 25798 RVA: 0x0026AEEB File Offset: 0x002690EB
		private int DrainLifeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 18);
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x060064C7 RID: 25799 RVA: 0x0026AEF8 File Offset: 0x002690F8
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0026AEFB File Offset: 0x002690FB
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x0026AEFE File Offset: 0x002690FE
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			spine.GetAnimationState().SetAnimation("tracks/writhe", true, 1);
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x0026AF14 File Offset: 0x00269114
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			base.Creature.Died += this.AfterDeath;
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x0026AF58 File Offset: 0x00269158
		private void AfterDeath(Creature _)
		{
			base.Creature.Died -= this.AfterDeath;
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(base.Creature);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.SpineAnimation.SetAnimation("tracks/empty", true, 1);
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x0026AFA8 File Offset: 0x002691A8
		public override void BeforeRemovedFromRoom()
		{
			if (base.CombatState.RunState.IsGameOver)
			{
				return;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(base.Creature);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.SpineAnimation.SetAnimation("tracks/empty", true, 1);
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x0026AFF8 File Offset: 0x002691F8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SOUL_BURN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulBurnMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SoulBurnDamage)
			});
			MoveState moveState2 = new MoveState("MAELSTROM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MaelstromMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.MaelstromDamage, this.MaelstromRepeat)
			});
			MoveState moveState3 = new MoveState("DRAIN_LIFE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DrainLifeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DrainLifeDamage),
				new DebuffIntent(true)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState3.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat, 1f);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat, 1f);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x0026B110 File Offset: 0x00269310
		private async Task SoulBurnMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SoulBurnDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x0026B154 File Offset: 0x00269354
		private async Task MaelstromMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.MaelstromDamage).WithHitCount(this.MaelstromRepeat).FromMonster(this)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x0026B198 File Offset: 0x00269398
		private async Task DrainLifeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DrainLifeDamage).FromMonster(this).WithAttackerAnim("Cast", 1f, null)
				.WithAttackerFx(null, this.CastSfx, null)
				.Execute(null);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x0026B1E3 File Offset: 0x002693E3
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "MAELSTROM_MOVE";
		}

		// Token: 0x040026A9 RID: 9897
		private const string _maelstromMove = "MAELSTROM_MOVE";
	}
}
