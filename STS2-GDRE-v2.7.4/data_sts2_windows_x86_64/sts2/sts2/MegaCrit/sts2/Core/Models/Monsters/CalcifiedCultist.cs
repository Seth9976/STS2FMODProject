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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A5 RID: 1957
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CalcifiedCultist : MonsterModel
	{
		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x06005FEE RID: 24558 RVA: 0x0025C5E3 File Offset: 0x0025A7E3
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 39, 38);
			}
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x06005FEF RID: 24559 RVA: 0x0025C5EF File Offset: 0x0025A7EF
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 42, 41);
			}
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x06005FF0 RID: 24560 RVA: 0x0025C5FB File Offset: 0x0025A7FB
		private int DarkStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
			}
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x0025C608 File Offset: 0x0025A808
		private int IncantationAmount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06005FF2 RID: 24562 RVA: 0x0025C60B File Offset: 0x0025A80B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x0025C60E File Offset: 0x0025A80E
		public override Vector2 ExtraDeathVfxPadding
		{
			get
			{
				return new Vector2(1.5f, 1.2f);
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x06005FF4 RID: 24564 RVA: 0x0025C61F File Offset: 0x0025A81F
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/cultists/cultists_die_calcified";
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x06005FF5 RID: 24565 RVA: 0x0025C626 File Offset: 0x0025A826
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/cultists/cultists_attack";
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06005FF6 RID: 24566 RVA: 0x0025C62D File Offset: 0x0025A82D
		// (set) Token: 0x06005FF7 RID: 24567 RVA: 0x0025C635 File Offset: 0x0025A835
		private float AttackSfxStrength
		{
			get
			{
				return this._attackSfxStrength;
			}
			set
			{
				base.AssertMutable();
				this._attackSfxStrength = value;
			}
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x0025C644 File Offset: 0x0025A844
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin("coral"));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x0025C684 File Offset: 0x0025A884
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("INCANTATION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IncantationMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState2 = new MoveState("DARK_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DarkStrikeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DarkStrikeDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x0025C70C File Offset: 0x0025A90C
		private async Task IncantationMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_calcified", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			TalkCmd.Play(CalcifiedCultist._cawCawDialogue, base.Creature, VfxColor.Purple, VfxDuration.Standard);
			await Cmd.CustomScaledWait(0.25f, 0.5f, false, default(CancellationToken));
			await PowerCmd.Apply<RitualPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.IncantationAmount, base.Creature, null, false);
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x0025C750 File Offset: 0x0025A950
		private async Task DarkStrikeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DarkStrikeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.BeforeDamage(new Func<Task>(this.PlayAttackSfx))
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x0025C794 File Offset: 0x0025A994
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x0025C837 File Offset: 0x0025AA37
		private Task PlayAttackSfx()
		{
			SfxCmd.Play(this.AttackSfx, "enemy_strength", this.AttackSfxStrength, 1f);
			this.AttackSfxStrength += 0.2f;
			return Task.CompletedTask;
		}

		// Token: 0x04002556 RID: 9558
		private static readonly LocString _cawCawDialogue = new LocString("monsters", "CALCIFIED_CULTIST.moves.INCANTATION.banter");

		// Token: 0x04002557 RID: 9559
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_calcified";

		// Token: 0x04002558 RID: 9560
		private float _attackSfxStrength;
	}
}
