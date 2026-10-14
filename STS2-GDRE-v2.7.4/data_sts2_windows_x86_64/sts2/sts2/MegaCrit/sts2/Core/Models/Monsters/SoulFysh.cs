using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007EF RID: 2031
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulFysh : MonsterModel
	{
		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x060064A7 RID: 25767 RVA: 0x0026A8C7 File Offset: 0x00268AC7
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_hurt";
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x060064A8 RID: 25768 RVA: 0x0026A8CE File Offset: 0x00268ACE
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 221, 211);
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x060064A9 RID: 25769 RVA: 0x0026A8E0 File Offset: 0x00268AE0
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x060064AA RID: 25770 RVA: 0x0026A8E8 File Offset: 0x00268AE8
		private int DeGasDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16);
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x060064AB RID: 25771 RVA: 0x0026A8F5 File Offset: 0x00268AF5
		private int ScreamDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x060064AC RID: 25772 RVA: 0x0026A902 File Offset: 0x00268B02
		private int GazeDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x060064AD RID: 25773 RVA: 0x0026A90D File Offset: 0x00268B0D
		private int BeckonMoveAmount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x060064AE RID: 25774 RVA: 0x0026A910 File Offset: 0x00268B10
		private int GazeMoveAmount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x060064AF RID: 25775 RVA: 0x0026A913 File Offset: 0x00268B13
		private int ScreamMoveAmount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x060064B0 RID: 25776 RVA: 0x0026A916 File Offset: 0x00268B16
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x060064B1 RID: 25777 RVA: 0x0026A919 File Offset: 0x00268B19
		// (set) Token: 0x060064B2 RID: 25778 RVA: 0x0026A921 File Offset: 0x00268B21
		public bool IsInvisible
		{
			get
			{
				return this._isInvisible;
			}
			set
			{
				base.AssertMutable();
				this._isInvisible = value;
			}
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x0026A930 File Offset: 0x00268B30
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("soulfysh_progress", 5f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060064B4 RID: 25780 RVA: 0x0026A960 File Offset: 0x00268B60
		public override Task AfterCardChangedPilesLate(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			if (LocalContext.GetMe(base.CombatState) != card.Owner)
			{
				return Task.CompletedTask;
			}
			if (!(card is Beckon))
			{
				return Task.CompletedTask;
			}
			if (CombatManager.Instance.IsOverOrEnding)
			{
				return Task.CompletedTask;
			}
			bool flag = card.Owner.PlayerCombatState.Hand.Cards.Any<CardModel>((CardModel c) => c is Beckon);
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("beckon", flag > false);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060064B5 RID: 25781 RVA: 0x0026AA00 File Offset: 0x00268C00
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BECKON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeckonMove), new AbstractIntent[]
			{
				new StatusIntent(this.BeckonMoveAmount)
			});
			MoveState moveState2 = new MoveState("DE_GAS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DeGasMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.DeGasDamage)
			});
			MoveState moveState3 = new MoveState("GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GazeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.GazeDamage),
				new StatusIntent(this.GazeMoveAmount)
			});
			MoveState moveState4 = new MoveState("FADE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FadeMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState5 = new MoveState("SCREAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreamMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ScreamDamage),
				new DebuffIntent(false)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState5);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x0026AB50 File Offset: 0x00268D50
		private async Task BeckonMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Beckon", 0f);
			await Cmd.Wait(0.3f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_spooky_scream");
			await Cmd.CustomScaledWait(0f, 0.3f, false, default(CancellationToken));
			foreach (Creature creature in targets)
			{
				Player player = creature.Player ?? creature.PetOwner;
				CardPileAddResult[] statusCards = new CardPileAddResult[this.BeckonMoveAmount];
				CardModel cardModel = base.CombatState.CreateCard<Beckon>(player);
				CardPileAddResult[] array = statusCards;
				array[0] = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, null, CardPilePosition.Random);
				array = null;
				CardModel cardModel2 = base.CombatState.CreateCard<Beckon>(player);
				array = statusCards;
				array[1] = await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Discard, null, CardPilePosition.Bottom);
				array = null;
				if (LocalContext.IsMe(player))
				{
					CardCmd.PreviewCardPileAdd(statusCards, 1.2f, CardPreviewStyle.HorizontalLayout);
					await Cmd.Wait(1f, false);
				}
				player = null;
				statusCards = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x0026AB9C File Offset: 0x00268D9C
		private async Task GazeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.GazeDamage).FromMonster(this).WithAttackerAnim("AttackBeckon", 0.6f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			foreach (Creature creature in targets)
			{
				Player player = creature.Player ?? creature.PetOwner;
				CardPileAddResult[] statusCards = new CardPileAddResult[1];
				CardModel cardModel = base.CombatState.CreateCard<Beckon>(player);
				CardPileAddResult[] array = statusCards;
				array[0] = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, null, CardPilePosition.Bottom);
				array = null;
				if (LocalContext.IsMe(player))
				{
					CardCmd.PreviewCardPileAdd(statusCards, 1.2f, CardPreviewStyle.HorizontalLayout);
					await Cmd.Wait(1f, false);
				}
				player = null;
				statusCards = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060064B8 RID: 25784 RVA: 0x0026ABE8 File Offset: 0x00268DE8
		private async Task DeGasMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.DeGasDamage).FromMonster(this).WithAttackerAnim("Attack", 0.45f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060064B9 RID: 25785 RVA: 0x0026AC2C File Offset: 0x00268E2C
		private async Task ScreamMove(IReadOnlyList<Creature> targets)
		{
			this.IsInvisible = false;
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("soulfysh_progress", 2f);
			}
			await DamageCmd.Attack(this.ScreamDamage).FromMonster(this).WithAttackerAnim("AttackDebuffTrigger", 0.65f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_wave", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, this.ScreamMoveAmount, base.Creature, null, false);
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_reappear", 1f);
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x0026AC78 File Offset: 0x00268E78
		private async Task FadeMove(IReadOnlyList<Creature> targets)
		{
			this.IsInvisible = true;
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("soulfysh_progress", 1f);
			}
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_intangible", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "IntangibleStart", 0.8f);
			await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060064BB RID: 25787 RVA: 0x0026ACBC File Offset: 0x00268EBC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack_heavy", false);
			AnimState animState4 = new AnimState("attack_beckon", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("beckon", false);
			AnimState animState8 = new AnimState("intangible_loop", true);
			AnimState animState9 = new AnimState("intangible_start", false);
			AnimState animState10 = new AnimState("intangible_end", false);
			AnimState animState11 = new AnimState("hurt_intangible", false);
			AnimState animState12 = new AnimState("die_intangible", false);
			AnimState animState13 = new AnimState("attack_debuff", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState7.NextState = animState;
			animState9.NextState = animState8;
			animState11.NextState = animState8;
			animState13.NextState = animState10;
			animState10.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("AttackBeckon", animState4, null);
			creatureAnimator.AddAnyState("Beckon", animState7, null);
			creatureAnimator.AddAnyState("IntangibleStart", animState9, null);
			creatureAnimator.AddAnyState("AttackDebuffTrigger", animState13, null);
			creatureAnimator.AddAnyState("Dead", animState6, () => !this.IsInvisible);
			creatureAnimator.AddAnyState("Hit", animState5, () => !this.IsInvisible);
			creatureAnimator.AddAnyState("Dead", animState12, () => this.IsInvisible);
			creatureAnimator.AddAnyState("Hit", animState11, () => this.IsInvisible);
			return creatureAnimator;
		}

		// Token: 0x0400269E RID: 9886
		private const string _soulFyshCustomTrackName = "soulfysh_progress";

		// Token: 0x0400269F RID: 9887
		private const string _beckonCustomTrackName = "beckon";

		// Token: 0x040026A0 RID: 9888
		private const string _intangibleSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_intangible";

		// Token: 0x040026A1 RID: 9889
		private const string _beckonSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon";

		// Token: 0x040026A2 RID: 9890
		private const string _waveSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_wave";

		// Token: 0x040026A3 RID: 9891
		private const string _reappearSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_reappear";

		// Token: 0x040026A4 RID: 9892
		private const string _attackBeckonTrigger = "AttackBeckon";

		// Token: 0x040026A5 RID: 9893
		private const string _intangibleStartTrigger = "IntangibleStart";

		// Token: 0x040026A6 RID: 9894
		private const string _attackDebuffTrigger = "AttackDebuffTrigger";

		// Token: 0x040026A7 RID: 9895
		private const string _beckonTrigger = "Beckon";

		// Token: 0x040026A8 RID: 9896
		private bool _isInvisible;
	}
}
