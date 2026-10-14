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
	// Token: 0x020007AC RID: 1964
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DampCultist : MonsterModel
	{
		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x0600609B RID: 24731 RVA: 0x0025E43A File Offset: 0x0025C63A
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 52, 51);
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x0600609C RID: 24732 RVA: 0x0025E446 File Offset: 0x0025C646
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 54, 53);
			}
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x0600609D RID: 24733 RVA: 0x0025E452 File Offset: 0x0025C652
		private int DarkStrikeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 1);
			}
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0025E45D File Offset: 0x0025C65D
		private int IncantationAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x0600609F RID: 24735 RVA: 0x0025E468 File Offset: 0x0025C668
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x060060A0 RID: 24736 RVA: 0x0025E46B File Offset: 0x0025C66B
		public override Vector2 ExtraDeathVfxPadding
		{
			get
			{
				return new Vector2(1.5f, 1.2f);
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x060060A1 RID: 24737 RVA: 0x0025E47C File Offset: 0x0025C67C
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/cultists/cultists_die_damp";
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0025E483 File Offset: 0x0025C683
		// (set) Token: 0x060060A3 RID: 24739 RVA: 0x0025E48B File Offset: 0x0025C68B
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

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060060A4 RID: 24740 RVA: 0x0025E49A File Offset: 0x0025C69A
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/cultists/cultists_attack";
			}
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x0025E4A4 File Offset: 0x0025C6A4
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin("slug"));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x0025E4E4 File Offset: 0x0025C6E4
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

		// Token: 0x060060A7 RID: 24743 RVA: 0x0025E56C File Offset: 0x0025C76C
		private async Task IncantationMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_damp", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.45f);
			TalkCmd.Play(DampCultist._cawCawDialogue, base.Creature, VfxColor.Swamp, VfxDuration.Standard);
			await Cmd.CustomScaledWait(0.25f, 0.5f, false, default(CancellationToken));
			await PowerCmd.Apply<RitualPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.IncantationAmount, base.Creature, null, false);
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x0025E5B0 File Offset: 0x0025C7B0
		private async Task DarkStrikeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DarkStrikeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.BeforeDamage(new Func<Task>(this.PlayAttackSfx))
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060060A9 RID: 24745 RVA: 0x0025E5F4 File Offset: 0x0025C7F4
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

		// Token: 0x060060AA RID: 24746 RVA: 0x0025E697 File Offset: 0x0025C897
		private Task PlayAttackSfx()
		{
			SfxCmd.Play(this.AttackSfx, "enemy_strength", this.AttackSfxStrength, 1f);
			this.AttackSfxStrength += 0.2f;
			return Task.CompletedTask;
		}

		// Token: 0x04002588 RID: 9608
		private static readonly LocString _cawCawDialogue = new LocString("monsters", "DAMP_CULTIST.moves.INCANTATION.banter");

		// Token: 0x04002589 RID: 9609
		private float _attackSfxStrength;

		// Token: 0x0400258A RID: 9610
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_damp";
	}
}
