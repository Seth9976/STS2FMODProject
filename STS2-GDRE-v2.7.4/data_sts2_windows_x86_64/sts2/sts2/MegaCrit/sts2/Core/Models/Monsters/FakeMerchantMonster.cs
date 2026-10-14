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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B7 RID: 1975
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeMerchantMonster : MonsterModel
	{
		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06006127 RID: 24871 RVA: 0x0025FCE2 File Offset: 0x0025DEE2
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 175, 165);
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06006128 RID: 24872 RVA: 0x0025FCF4 File Offset: 0x0025DEF4
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 175, 165);
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06006129 RID: 24873 RVA: 0x0025FD06 File Offset: 0x0025DF06
		private int SwipeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
			}
		}

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x0600612A RID: 24874 RVA: 0x0025FD13 File Offset: 0x0025DF13
		private int ThrowRelicDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x0600612B RID: 24875 RVA: 0x0025FD20 File Offset: 0x0025DF20
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x0600612C RID: 24876 RVA: 0x0025FD23 File Offset: 0x0025DF23
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/npcs/reverse_merchant/reverse_merchant_die";
			}
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x0600612D RID: 24877 RVA: 0x0025FD2A File Offset: 0x0025DF2A
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/npcs/reverse_merchant/reverse_merchant_hurt";
			}
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x0025FD34 File Offset: 0x0025DF34
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwipeDamage)
			});
			MoveState moveState2 = new MoveState("SPEW_COINS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpewCoinsMove), new AbstractIntent[]
			{
				new MultiAttackIntent(2, 8)
			});
			MoveState moveState3 = new MoveState("THROW_RELIC_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrowRelicMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ThrowRelicDamage),
				new DebuffIntent(false)
			});
			MoveState moveState4 = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND_MOVE");
			randomBranchState.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			randomBranchState.AddBranch(moveState4, 3, MoveRepeatType.CannotRepeat);
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			moveState4.FollowUpState = randomBranchState;
			RandomBranchState randomBranchState2 = new RandomBranchState("RAND_ATTACK_MOVE");
			randomBranchState2.AddBranch(moveState, MoveRepeatType.CannotRepeat);
			randomBranchState2.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			randomBranchState2.AddBranch(moveState3, MoveRepeatType.CannotRepeat);
			moveState3.FollowUpState = randomBranchState2;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(randomBranchState);
			list.Add(randomBranchState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x0025FEA4 File Offset: 0x0025E0A4
		private async Task SwipeMove(IReadOnlyList<Creature> targets)
		{
			await this.ShowDialogueForMove("SWIPE");
			await DamageCmd.Attack(this.SwipeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x0025FEE8 File Offset: 0x0025E0E8
		private async Task SpewCoinsMove(IReadOnlyList<Creature> targets)
		{
			await this.ShowDialogueForMove("SPEW_COINS");
			await DamageCmd.Attack(2m).FromMonster(this).WithHitCount(8)
				.OnlyPlayAnimOnce()
				.WithAttackerAnim("spew", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x0025FF2C File Offset: 0x0025E12C
		private async Task ThrowRelicMove(IReadOnlyList<Creature> targets)
		{
			await this.ShowDialogueForMove("THROW_RELIC");
			await DamageCmd.Attack(this.ThrowRelicDamage).FromMonster(this).WithAttackerAnim("throw", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x0025FF78 File Offset: 0x0025E178
		private async Task EnrageMove(IReadOnlyList<Creature> targets)
		{
			await this.ShowDialogueForMove("ENRAGE");
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006133 RID: 24883 RVA: 0x0025FFBC File Offset: 0x0025E1BC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("combat_idle_loop", true);
			AnimState animState2 = new AnimState("attack", false);
			AnimState animState3 = new AnimState("attack_multi", false);
			AnimState animState4 = new AnimState("attack_throw", false);
			AnimState animState5 = new AnimState("buff", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState6, null);
			creatureAnimator.AddAnyState("Attack", animState2, null);
			creatureAnimator.AddAnyState("spew", animState3, null);
			creatureAnimator.AddAnyState("throw", animState4, null);
			creatureAnimator.AddAnyState("Cast", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x002600A8 File Offset: 0x0025E2A8
		private async Task ShowDialogueForMove(string moveId)
		{
			LocString locString = Rng.Chaotic.NextItem<LocString>(this.GetLinesForMove(moveId));
			if (locString != null)
			{
				TalkCmd.Play(locString, base.Creature, VfxColor.Blue, VfxDuration.Custom);
				await Cmd.Wait(0.5f, false);
			}
		}

		// Token: 0x06006135 RID: 24885 RVA: 0x002600F4 File Offset: 0x0025E2F4
		private IEnumerable<LocString> GetLinesForMove(string moveId)
		{
			LocTable table = LocManager.Instance.GetTable("monsters");
			return table.GetLocStringsWithPrefix(base.Id.Entry + ".moves." + moveId + ".speakLine");
		}

		// Token: 0x040025A2 RID: 9634
		private const string _spewCoinsTrigger = "spew";

		// Token: 0x040025A3 RID: 9635
		private const string _throwRelicTrigger = "throw";

		// Token: 0x040025A4 RID: 9636
		private const int _spewCoinsDamage = 2;

		// Token: 0x040025A5 RID: 9637
		private const int _spewCoinsRepeat = 8;

		// Token: 0x040025A6 RID: 9638
		private const string _attackMultiTrigger = "attack_multi";
	}
}
