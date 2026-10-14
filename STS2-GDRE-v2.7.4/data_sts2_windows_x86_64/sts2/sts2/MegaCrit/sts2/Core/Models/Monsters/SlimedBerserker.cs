using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E9 RID: 2025
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlimedBerserker : MonsterModel
	{
		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06006451 RID: 25681 RVA: 0x00269841 File Offset: 0x00267A41
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 281, 261);
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06006452 RID: 25682 RVA: 0x00269853 File Offset: 0x00267A53
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06006453 RID: 25683 RVA: 0x0026985B File Offset: 0x00267A5B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06006454 RID: 25684 RVA: 0x0026985E File Offset: 0x00267A5E
		public override bool HasDeathSfx
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06006455 RID: 25685 RVA: 0x00269861 File Offset: 0x00267A61
		protected override string CastSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/slimed_berserker/slimed_berserker_buff";
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06006456 RID: 25686 RVA: 0x00269868 File Offset: 0x00267A68
		private string SlimeSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/slimed_berserker/slimed_berserker_slime";
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06006457 RID: 25687 RVA: 0x0026986F File Offset: 0x00267A6F
		private int PummelingDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06006458 RID: 25688 RVA: 0x0026987A File Offset: 0x00267A7A
		private int SmotherDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 33, 30);
			}
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x00269888 File Offset: 0x00267A88
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("VOMIT_ICHOR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.VomitIchorMove), new AbstractIntent[]
			{
				new StatusIntent(10)
			});
			MoveState moveState2 = new MoveState("LEECHING_HUG_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LeechingHugMove), new AbstractIntent[]
			{
				new DebuffIntent(false),
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("SMOTHER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmotherMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SmotherDamage)
			});
			MoveState moveState4 = new MoveState("FURIOUS_PUMMELING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FuriousPummelingMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.PummelingDamage, 4)
			});
			moveState.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState3);
			list.Add(moveState2);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600645A RID: 25690 RVA: 0x0026998C File Offset: 0x00267B8C
		private async Task VomitIchorMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.SlimeSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Vomit", 0.7f);
			await CardPileCmd.AddToCombatAndPreview<Slimed>(targets, PileType.Discard, 10, null, CardPilePosition.Bottom);
		}

		// Token: 0x0600645B RID: 25691 RVA: 0x002699D8 File Offset: 0x00267BD8
		private async Task LeechingHugMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Hug", 0.65f);
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 3m, null, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null, false);
		}

		// Token: 0x0600645C RID: 25692 RVA: 0x00269A24 File Offset: 0x00267C24
		private async Task FuriousPummelingMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.PummelingDamage).WithHitCount(4).OnlyPlayAnimOnce()
				.FromMonster(this)
				.WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.Execute(null);
		}

		// Token: 0x0600645D RID: 25693 RVA: 0x00269A68 File Offset: 0x00267C68
		private async Task SmotherMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SmotherDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.Execute(null);
		}

		// Token: 0x0600645E RID: 25694 RVA: 0x00269AAC File Offset: 0x00267CAC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hug", false);
			AnimState animState3 = new AnimState("vomit", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Hug", animState2, null);
			creatureAnimator.AddAnyState("Vomit", animState3, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x00269B73 File Offset: 0x00267D73
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "SMOTHER_MOVE";
		}

		// Token: 0x04002686 RID: 9862
		private const string _smotherMove = "SMOTHER_MOVE";

		// Token: 0x04002687 RID: 9863
		private const int _pummelingRepeat = 4;

		// Token: 0x04002688 RID: 9864
		private const int _leechingDrain = 3;

		// Token: 0x04002689 RID: 9865
		private const int _vomitSlimeInDiscard = 10;

		// Token: 0x0400268A RID: 9866
		private const string _hugTrigger = "Hug";

		// Token: 0x0400268B RID: 9867
		private const string _vomitTrigger = "Vomit";
	}
}
