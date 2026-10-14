using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007BE RID: 1982
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FuzzyWurmCrawler : MonsterModel
	{
		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06006189 RID: 24969 RVA: 0x0026140A File Offset: 0x0025F60A
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 58, 55);
			}
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x0600618A RID: 24970 RVA: 0x00261416 File Offset: 0x0025F616
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 59, 57);
			}
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x0600618B RID: 24971 RVA: 0x00261422 File Offset: 0x0025F622
		private int AcidGoopDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 4);
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x0600618C RID: 24972 RVA: 0x0026142D File Offset: 0x0025F62D
		// (set) Token: 0x0600618D RID: 24973 RVA: 0x00261435 File Offset: 0x0025F635
		private bool IsPuffed
		{
			get
			{
				return this._isPuffed;
			}
			set
			{
				base.AssertMutable();
				this._isPuffed = value;
			}
		}

		// Token: 0x0600618E RID: 24974 RVA: 0x00261444 File Offset: 0x0025F644
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("FIRST_ACID_GOOP", new Func<IReadOnlyList<Creature>, Task>(this.AcidGoop), new AbstractIntent[]
			{
				new SingleAttackIntent(this.AcidGoopDamage)
			});
			MoveState moveState2 = new MoveState("ACID_GOOP", new Func<IReadOnlyList<Creature>, Task>(this.AcidGoop), new AbstractIntent[]
			{
				new SingleAttackIntent(this.AcidGoopDamage)
			});
			MoveState moveState3 = new MoveState("INHALE", new Func<IReadOnlyList<Creature>, Task>(this.Inhale), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x00261504 File Offset: 0x0025F704
		private async Task AcidGoop(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff && targets.Any<Creature>())
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
					if (specialNode != null)
					{
						specialNode.Position = Vector2.Left * (creatureNode.GlobalPosition.X - NCombatRoom.Instance.GetCreatureNode(targets.First<Creature>()).GlobalPosition.X);
					}
				}
			}
			this.IsPuffed = false;
			await DamageCmd.Attack(this.AcidGoopDamage).FromMonster(this).WithAttackerAnim("Attack", 1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x00261550 File Offset: 0x0025F750
		private async Task Inhale(IReadOnlyList<Creature> targets)
		{
			this.IsPuffed = true;
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Inhale", 0.6f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 7m, base.Creature, null, false);
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x00261594 File Offset: 0x0025F794
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("hurt", false);
			AnimState animState4 = new AnimState("die", false);
			AnimState animState5 = new AnimState("inhale", false);
			AnimState animState6 = new AnimState("idle_loop_puffed", true);
			AnimState animState7 = new AnimState("hurt_puffed", false);
			AnimState animState8 = new AnimState("die_puffed", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState6;
			animState7.NextState = animState6;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Inhale", animState5, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("Dead", animState4, () => !this.IsPuffed);
			creatureAnimator.AddAnyState("Dead", animState8, () => this.IsPuffed);
			creatureAnimator.AddAnyState("Hit", animState3, () => !this.IsPuffed);
			creatureAnimator.AddAnyState("Hit", animState7, () => this.IsPuffed);
			return creatureAnimator;
		}

		// Token: 0x06006192 RID: 24978 RVA: 0x002616B3 File Offset: 0x0025F8B3
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "FIRST_ACID_GOOP";
		}

		// Token: 0x040025C0 RID: 9664
		private const string _firstAcidGoopMove = "FIRST_ACID_GOOP";

		// Token: 0x040025C1 RID: 9665
		private const string _inhaleTrigger = "Inhale";

		// Token: 0x040025C2 RID: 9666
		private bool _isPuffed;
	}
}
