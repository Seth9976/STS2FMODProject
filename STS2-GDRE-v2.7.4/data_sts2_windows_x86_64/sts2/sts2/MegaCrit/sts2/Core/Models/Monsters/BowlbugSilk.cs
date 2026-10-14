using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A0 RID: 1952
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugSilk : MonsterModel
	{
		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06005FBA RID: 24506 RVA: 0x0025BC6C File Offset: 0x00259E6C
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 41, 40);
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06005FBB RID: 24507 RVA: 0x0025BC78 File Offset: 0x00259E78
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 43);
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06005FBC RID: 24508 RVA: 0x0025BC84 File Offset: 0x00259E84
		private int ThrashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x06005FBD RID: 24509 RVA: 0x0025BC8F File Offset: 0x00259E8F
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_die";
			}
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x0025BC96 File Offset: 0x00259E96
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			skeleton.SetSkin(skeleton.GetData().FindSkin("web"));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x0025BCB4 File Offset: 0x00259EB4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ThrashDamage, 2)
			});
			MoveState moveState2 = new MoveState("TOXIC_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpitMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0025BD3C File Offset: 0x00259F3C
		private async Task ThrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThrashDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("Attack", 0.3f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x0025BD80 File Offset: 0x00259F80
		private async Task SpitMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature ncreature = null;
				foreach (Creature creature in targets)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null && (ncreature == null || ncreature.GlobalPosition.X > creatureNode.GlobalPosition.X))
					{
						ncreature = creatureNode;
					}
				}
				NCreature creatureNode2 = base.Creature.GetCreatureNode();
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/SpineBoneNode") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					float num = 0f * creatureNode2.Visuals.Scale.X;
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, node2D.GlobalPosition.Y);
				}
			}
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_spit", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.8f);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06005FC2 RID: 24514 RVA: 0x0025BDCC File Offset: 0x00259FCC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("spit", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x04002550 RID: 9552
		private const int _thrashRepeat = 2;

		// Token: 0x04002551 RID: 9553
		private const string _spitSfx = "event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_spit";

		// Token: 0x04002552 RID: 9554
		private const string _spineSkin = "web";
	}
}
