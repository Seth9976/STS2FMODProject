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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000800 RID: 2048
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TorchHeadAmalgam : MonsterModel
	{
		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x060065FA RID: 26106 RVA: 0x0026E848 File Offset: 0x0026CA48
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 211, 199);
			}
		}

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x060065FB RID: 26107 RVA: 0x0026E85A File Offset: 0x0026CA5A
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x060065FC RID: 26108 RVA: 0x0026E862 File Offset: 0x0026CA62
		private int StrongTackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 32, 26);
			}
		}

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x060065FD RID: 26109 RVA: 0x0026E86F File Offset: 0x0026CA6F
		private int TackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 22, 18);
			}
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x060065FE RID: 26110 RVA: 0x0026E87C File Offset: 0x0026CA7C
		private int WeakTackleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x060065FF RID: 26111 RVA: 0x0026E889 File Offset: 0x0026CA89
		private int SoulBeamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 8);
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06006600 RID: 26112 RVA: 0x0026E894 File Offset: 0x0026CA94
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06006601 RID: 26113 RVA: 0x0026E898 File Offset: 0x0026CA98
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x0026E8DC File Offset: 0x0026CADC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("STRONG_TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StrongTackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.StrongTackleDamage)
			});
			MoveState moveState2 = new MoveState("TACKLE_2_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TackleDamage)
			});
			MoveState moveState3 = new MoveState("BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulBeamMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SoulBeamDamage, 3)
			});
			MoveState moveState4 = new MoveState("TACKLE_3_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WeakTackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.WeakTackleDamage)
			});
			MoveState moveState5 = new MoveState("TACKLE_4_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WeakTackleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.WeakTackleDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState3;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006603 RID: 26115 RVA: 0x0026EA1C File Offset: 0x0026CC1C
		public override void OnDieToDoom()
		{
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/torch1Slot/fire1_small_green/light_small");
					if (specialNode != null)
					{
						specialNode.SetVisible(false);
					}
					Node2D specialNode2 = creatureNode.GetSpecialNode<Node2D>("Visuals/torch2Slot/fire2_small_green/light_small");
					if (specialNode2 != null)
					{
						specialNode2.SetVisible(false);
					}
					Node2D specialNode3 = creatureNode.GetSpecialNode<Node2D>("Visuals/torch3Slot/fire3_small_green/light_small");
					if (specialNode3 == null)
					{
						return;
					}
					specialNode3.SetVisible(false);
				}
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x0026EA84 File Offset: 0x0026CC84
		private async Task StrongTackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StrongTackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x0026EAC8 File Offset: 0x0026CCC8
		private async Task TackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.TackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x0026EB0C File Offset: 0x0026CD0C
		private async Task WeakTackleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.WeakTackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.6f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x0026EB50 File Offset: 0x0026CD50
		private async Task SoulBeamMove(IReadOnlyList<Creature> targets)
		{
			NCreature creatureNode = base.Creature.GetCreatureNode();
			Node2D node2D = ((creatureNode != null) ? creatureNode.GetSpecialNode<Node2D>("Visuals/LaserControlBone") : null);
			if (node2D != null && creatureNode != null)
			{
				if (targets.Count > 0)
				{
					NCreature creatureNode2 = targets[0].GetCreatureNode();
					if (creatureNode2 != null)
					{
						float num = 400f * creatureNode.Visuals.Scale.X;
						node2D.GlobalPosition = new Vector2(creatureNode2.GlobalPosition.X + num, node2D.GlobalPosition.Y);
					}
				}
				else
				{
					node2D.Position = Vector2.Left * 3000f;
				}
			}
			await DamageCmd.Attack(this.SoulBeamDamage).WithHitCount(3).FromMonster(this)
				.WithAttackerAnim("DebuffTrigger", 0.8f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/torch_head_amalgam/torch_head_amalgam_beam", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x0026EB9C File Offset: 0x0026CD9C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("DebuffTrigger", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x040026F5 RID: 9973
		private const int _soulBeamRepeat = 3;

		// Token: 0x040026F6 RID: 9974
		private const string _debuffTrigger = "DebuffTrigger";

		// Token: 0x040026F7 RID: 9975
		private const string _beamSfx = "event:/sfx/enemy/enemy_attacks/torch_head_amalgam/torch_head_amalgam_beam";
	}
}
