using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007BF RID: 1983
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GasBomb : MonsterModel
	{
		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06006198 RID: 24984 RVA: 0x002616EE File Offset: 0x0025F8EE
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06006199 RID: 24985 RVA: 0x002616F8 File Offset: 0x0025F8F8
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x0600619A RID: 24986 RVA: 0x00261700 File Offset: 0x0025F900
		private int ExplodeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x0600619B RID: 24987 RVA: 0x0026170C File Offset: 0x0025F90C
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x00261710 File Offset: 0x0025F910
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x0600619D RID: 24989 RVA: 0x00261753 File Offset: 0x0025F953
		// (set) Token: 0x0600619E RID: 24990 RVA: 0x0026175B File Offset: 0x0025F95B
		private bool HasExploded
		{
			get
			{
				return this._hasExploded;
			}
			set
			{
				base.AssertMutable();
				this._hasExploded = value;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x0600619F RID: 24991 RVA: 0x0026176A File Offset: 0x0025F96A
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x060061A0 RID: 24992 RVA: 0x0026176D File Offset: 0x0025F96D
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_die";
			}
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00261774 File Offset: 0x0025F974
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("EXPLODE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplodeMove), new AbstractIntent[]
			{
				new DeathBlowIntent(() => this.ExplodeDamage)
			});
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x002617C8 File Offset: 0x0025F9C8
		private async Task ExplodeMove(IReadOnlyList<Creature> targets)
		{
			this.HasExploded = true;
			await DamageCmd.Attack(this.ExplodeDamage).FromMonster(this).WithAttackerAnim("ExplodeTrigger", 0.1f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_explode", null)
				.WithHitVfxNode((Creature _) => NGaseousImpactVfx.Create(CombatSide.Player, base.CombatState, new Color("#402f45")))
				.Execute(null);
			await CreatureCmd.Kill(base.Creature, false);
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x0026180C File Offset: 0x0025FA0C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("spawn", false);
			AnimState animState3 = new AnimState("explode", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState2.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState2, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, () => !this.HasExploded);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("ExplodeTrigger", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x040025C3 RID: 9667
		private const string _explodeTrigger = "ExplodeTrigger";

		// Token: 0x040025C4 RID: 9668
		private bool _hasExploded;

		// Token: 0x040025C5 RID: 9669
		private const string _explodeSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_explode";
	}
}
