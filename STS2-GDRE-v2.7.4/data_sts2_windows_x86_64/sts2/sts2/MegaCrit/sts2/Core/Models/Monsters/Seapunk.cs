using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E4 RID: 2020
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Seapunk : MonsterModel
	{
		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x06006412 RID: 25618 RVA: 0x00268CF4 File Offset: 0x00266EF4
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 47, 44);
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x06006413 RID: 25619 RVA: 0x00268D00 File Offset: 0x00266F00
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 49, 46);
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06006414 RID: 25620 RVA: 0x00268D0C File Offset: 0x00266F0C
		private int SeaKickDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x06006415 RID: 25621 RVA: 0x00268D19 File Offset: 0x00266F19
		private int SpinningKickDamage
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06006416 RID: 25622 RVA: 0x00268D1C File Offset: 0x00266F1C
		private int SpinningKickRepeat
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x06006417 RID: 25623 RVA: 0x00268D1F File Offset: 0x00266F1F
		private int BubbleBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x06006418 RID: 25624 RVA: 0x00268D29 File Offset: 0x00266F29
		private int BubbleStr
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06006419 RID: 25625 RVA: 0x00268D34 File Offset: 0x00266F34
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x0600641A RID: 25626 RVA: 0x00268D37 File Offset: 0x00266F37
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_hurt";
			}
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x00268D40 File Offset: 0x00266F40
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x00268D84 File Offset: 0x00266F84
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SEA_KICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SeaKickMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SeaKickDamage)
			});
			MoveState moveState2 = new MoveState("SPINNING_KICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpinningKickMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.SpinningKickDamage, this.SpinningKickRepeat)
			});
			MoveState moveState3 = new MoveState("BUBBLE_BURP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BubbleBurpMove), new AbstractIntent[]
			{
				new BuffIntent(),
				new DefendIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x00268E54 File Offset: 0x00267054
		private async Task SeaKickMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SeaKickDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x00268E98 File Offset: 0x00267098
		private async Task SpinningKickMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SpinningKickDamage).WithHitCount(this.SpinningKickRepeat).FromMonster(this)
				.WithAttackerAnim("MultiAttack", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick_multi", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x00268EDC File Offset: 0x002670DC
		private async Task BubbleBurpMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/seapunk/seapunk_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.75f);
			await CreatureCmd.GainBlock(base.Creature, this.BubbleBlock, ValueProp.Move, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.BubbleStr, base.Creature, null, false);
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x00268F20 File Offset: 0x00267120
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_multi", false);
			AnimState animState3 = new AnimState("cast", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState2.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("MultiAttack", animState2, null);
			return creatureAnimator;
		}

		// Token: 0x04002679 RID: 9849
		private const string _multiAttackTrigger = "MultiAttack";

		// Token: 0x0400267A RID: 9850
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_buff";

		// Token: 0x0400267B RID: 9851
		private const string _kickSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick";

		// Token: 0x0400267C RID: 9852
		private const string _kickMultiSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick_multi";
	}
}
