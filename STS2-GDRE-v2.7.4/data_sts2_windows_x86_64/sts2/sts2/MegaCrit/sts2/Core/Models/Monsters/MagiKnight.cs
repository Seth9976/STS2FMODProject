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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D0 RID: 2000
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MagiKnight : MonsterModel
	{
		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x060062C4 RID: 25284 RVA: 0x00264E6F File Offset: 0x0026306F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 89, 82);
			}
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x060062C5 RID: 25285 RVA: 0x00264E7B File Offset: 0x0026307B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00264E83 File Offset: 0x00263083
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x060062C7 RID: 25287 RVA: 0x00264E86 File Offset: 0x00263086
		private int PowerShieldDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x060062C8 RID: 25288 RVA: 0x00264E91 File Offset: 0x00263091
		private int PowerShieldBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 5);
			}
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x060062C9 RID: 25289 RVA: 0x00264E9C File Offset: 0x0026309C
		private int SpearDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
			}
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x060062CA RID: 25290 RVA: 0x00264EA9 File Offset: 0x002630A9
		private int BombDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x060062CB RID: 25291 RVA: 0x00264EB6 File Offset: 0x002630B6
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_hurt";
			}
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x00264EC0 File Offset: 0x002630C0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("POWER_SHIELD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PowerShieldMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PowerShieldDamage),
				new DefendIntent()
			});
			MoveState moveState2 = new MoveState("DAMPEN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DampenMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState3 = new MoveState("PREP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrepMove), new AbstractIntent[]
			{
				new DefendIntent()
			});
			MoveState moveState4 = new MoveState("MAGIC_BOMB", new Func<IReadOnlyList<Creature>, Task>(this.MagicBombMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BombDamage)
			});
			MoveState moveState5 = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpearMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SpearDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState5);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x00265000 File Offset: 0x00263200
		private async Task DampenMove(IReadOnlyList<Creature> targets)
		{
			foreach (Creature creature in targets)
			{
				DampenPower dampenPower = creature.GetPower<DampenPower>();
				bool flag = dampenPower == null;
				if (flag)
				{
					dampenPower = (DampenPower)ModelDb.Power<DampenPower>().ToMutable(0);
				}
				dampenPower.AddCaster(base.Creature);
				if (flag)
				{
					await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), dampenPower, creature, 1m, base.Creature, null, false);
				}
			}
			IEnumerator<Creature> enumerator = null;
			TalkCmd.Play(MagiKnight._dampenDialogue, base.Creature, VfxColor.Orange, VfxDuration.Long);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x0026504C File Offset: 0x0026324C
		private async Task PowerShieldMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PowerShieldDamage).FromMonster(this).WithAttackerAnim("ShieldAttack", 0.6f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await CreatureCmd.GainBlock(base.Creature, this.PowerShieldBlock, ValueProp.Move, null, false);
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x00265090 File Offset: 0x00263290
		private async Task PrepMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "ShieldAttack", 0.6f);
			await CreatureCmd.GainBlock(base.Creature, this.PowerShieldBlock, ValueProp.Move, null, false);
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x002650D4 File Offset: 0x002632D4
		private async Task MagicBombMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff && base.CombatState.IsLiveCombat())
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
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/AttackDistanceControl") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					float num = 600f * creatureNode2.Visuals.Scale.X;
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, node2D.GlobalPosition.Y);
				}
			}
			await DamageCmd.Attack(this.BombDamage).FromMonster(this).WithAttackerAnim("BombCast", 1.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_bomb", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060062D1 RID: 25297 RVA: 0x00265120 File Offset: 0x00263320
		private async Task SpearMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SpearDamage).FromMonster(this).WithAttackerAnim("RamAttack", 1.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_ram", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060062D2 RID: 25298 RVA: 0x00265164 File Offset: 0x00263364
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_bomb", false);
			AnimState animState3 = new AnimState("attack_ram", false);
			AnimState animState4 = new AnimState("cast_shield", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("BombCast", animState2, null);
			creatureAnimator.AddAnyState("RamAttack", animState3, null);
			creatureAnimator.AddAnyState("ShieldAttack", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x060062D3 RID: 25299 RVA: 0x00265239 File Offset: 0x00263439
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "PREP_MOVE";
		}

		// Token: 0x0400261F RID: 9759
		private static readonly LocString _dampenDialogue = new LocString("powers", "DAMPEN_POWER.banter");

		// Token: 0x04002620 RID: 9760
		private const string _prepMove = "PREP_MOVE";

		// Token: 0x04002621 RID: 9761
		private const string _bombTrigger = "BombCast";

		// Token: 0x04002622 RID: 9762
		private const string _ramAttackTrigger = "RamAttack";

		// Token: 0x04002623 RID: 9763
		private const string _shieldTrigger = "ShieldAttack";

		// Token: 0x04002624 RID: 9764
		private const string _ramSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_ram";

		// Token: 0x04002625 RID: 9765
		private const string _bombSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_bomb";

		// Token: 0x04002626 RID: 9766
		private const string _castShieldSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield";
	}
}
