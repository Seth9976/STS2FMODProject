using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
	// Token: 0x020007C3 RID: 1987
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HauntedShip : MonsterModel
	{
		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x060061D0 RID: 25040 RVA: 0x00261F73 File Offset: 0x00260173
		protected override bool HasPhobiaSpineSkin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x060061D1 RID: 25041 RVA: 0x00261F76 File Offset: 0x00260176
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 67, 63);
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x060061D2 RID: 25042 RVA: 0x00261F82 File Offset: 0x00260182
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x060061D3 RID: 25043 RVA: 0x00261F8A File Offset: 0x0026018A
		private int HauntDazed
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x060061D4 RID: 25044 RVA: 0x00261F8D File Offset: 0x0026018D
		private int SwipeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x060061D5 RID: 25045 RVA: 0x00261F9A File Offset: 0x0026019A
		private int StompDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x060061D6 RID: 25046 RVA: 0x00261FA5 File Offset: 0x002601A5
		private int StompRepeat
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x060061D7 RID: 25047 RVA: 0x00261FA8 File Offset: 0x002601A8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.ArmorBig;
			}
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x00261FAC File Offset: 0x002601AC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SwipeDamage)
			});
			MoveState moveState2 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.StompDamage, this.StompRepeat)
			});
			MoveState moveState3 = new MoveState("HAUNT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HauntMove), new AbstractIntent[]
			{
				new DebuffIntent(false),
				new StatusIntent(this.HauntDazed)
			});
			moveState3.FollowUpState = moveState;
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState3);
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x00262080 File Offset: 0x00260280
		private async Task SwipeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SwipeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x002620C4 File Offset: 0x002602C4
		private async Task StompMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.StompDamage).WithHitCount(this.StompRepeat).FromMonster(this)
				.WithAttackerAnim("AttackTriple", 0.15f, null)
				.OnlyPlayAnimOnce()
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x00262108 File Offset: 0x00260308
		private async Task HauntMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0f);
			await Cmd.Wait(0.6f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_spooky_scream");
			await Cmd.CustomScaledWait(0.2f, 0.5f, false, default(CancellationToken));
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 3m, base.Creature, null, false);
			await CardPileCmd.AddToCombatAndPreview<Dazed>(targets, PileType.Discard, this.HauntDazed, null, CardPilePosition.Bottom);
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x00262154 File Offset: 0x00260354
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("attack_triple", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("AttackTriple", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x040025CF RID: 9679
		private const string _attackTripleTrigger = "AttackTriple";
	}
}
