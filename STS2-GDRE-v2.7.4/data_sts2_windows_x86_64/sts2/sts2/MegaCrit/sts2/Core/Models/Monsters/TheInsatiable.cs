using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007FB RID: 2043
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheInsatiable : MonsterModel
	{
		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x0600658B RID: 25995 RVA: 0x0026D210 File Offset: 0x0026B410
		public static string TheInsatiableTrackName
		{
			get
			{
				return "insatiable_progress";
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x0600658C RID: 25996 RVA: 0x0026D217 File Offset: 0x0026B417
		public static string EatPlayerAnim
		{
			get
			{
				return "eat_player";
			}
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x0600658D RID: 25997 RVA: 0x0026D21E File Offset: 0x0026B41E
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 341, 321);
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x0600658E RID: 25998 RVA: 0x0026D230 File Offset: 0x0026B430
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x0600658F RID: 25999 RVA: 0x0026D238 File Offset: 0x0026B438
		private int ThrashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06006590 RID: 26000 RVA: 0x0026D244 File Offset: 0x0026B444
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31, 28);
			}
		}

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06006591 RID: 26001 RVA: 0x0026D251 File Offset: 0x0026B451
		private int SalivateStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06006592 RID: 26002 RVA: 0x0026D25C File Offset: 0x0026B45C
		// (set) Token: 0x06006593 RID: 26003 RVA: 0x0026D264 File Offset: 0x0026B464
		private bool HasLiquified
		{
			get
			{
				return this._hasLiquified;
			}
			set
			{
				base.AssertMutable();
				this._hasLiquified = value;
			}
		}

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x06006594 RID: 26004 RVA: 0x0026D273 File Offset: 0x0026B473
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x0026D276 File Offset: 0x0026B476
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Creature)
			{
				return Task.CompletedTask;
			}
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter(TheInsatiable.TheInsatiableTrackName, 10f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x0026D2A8 File Offset: 0x0026B4A8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("LIQUIFY_GROUND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LiquifyMove), new AbstractIntent[]
			{
				new BuffIntent(),
				new StatusIntent(6)
			});
			MoveState moveState2 = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ThrashDamage, 2)
			});
			MoveState moveState3 = new MoveState("THRASH_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.ThrashDamage, 2)
			});
			MoveState moveState4 = new MoveState("LUNGING_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState5 = new MoveState("SALIVATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SalivateMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState4);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x0026D3E8 File Offset: 0x0026B5E8
		private async Task LiquifyMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_liquify_ground", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "LiquifySand", 0f);
			await Cmd.Wait(0.5f, false);
			VfxCmd.PlayOnCreatureCenter(base.Creature, "vfx/vfx_scream");
			await Cmd.Wait(0.75f, false);
			foreach (Creature creature in targets)
			{
				SandpitPower sandpitPower = (SandpitPower)ModelDb.Power<SandpitPower>().ToMutable(0);
				sandpitPower.Target = creature;
				await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), sandpitPower, base.Creature, 4m, base.Creature, null, false);
			}
			IEnumerator<Creature> enumerator = null;
			foreach (Creature creature2 in targets)
			{
				Player player = creature2.Player ?? creature2.PetOwner;
				List<CardPileAddResult> statusCards = new List<CardPileAddResult>();
				for (int i = 0; i < 6; i++)
				{
					CardModel cardModel = base.CombatState.CreateCard<FranticEscape>(player);
					PileType pileType = ((i < 3) ? PileType.Draw : PileType.Discard);
					List<CardPileAddResult> list = statusCards;
					list.Add(await CardPileCmd.AddGeneratedCardToCombat(cardModel, pileType, null, CardPilePosition.Random));
					list = null;
				}
				if (LocalContext.IsMe(player))
				{
					CardCmd.PreviewCardPileAdd(statusCards, 1.2f, CardPreviewStyle.HorizontalLayout);
					await Cmd.Wait(1f, false);
				}
				player = null;
				statusCards = null;
			}
			enumerator = null;
			this.HasLiquified = true;
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x0026D434 File Offset: 0x0026B634
		private async Task ThrashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ThrashDamage).WithHitCount(2).FromMonster(this)
				.WithHitFx("vfx/vfx_scratch", null, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash", null)
				.WithAttackerAnim("Thrash", 0.3f, null)
				.OnlyPlayAnimOnce()
				.Execute(null);
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x0026D478 File Offset: 0x0026B678
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Bite", 0.25f, null)
				.OnlyPlayAnimOnce()
				.WithHitFx("vfx/vfx_bite", null, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite", null)
				.Execute(null);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x0026D4BC File Offset: 0x0026B6BC
		private async Task SalivateMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Salivate", 0.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.SalivateStrength, base.Creature, null, false);
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x0026D500 File Offset: 0x0026B700
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("intro_loop", true);
			AnimState animState2 = new AnimState("liquify_sand", false);
			AnimState animState3 = new AnimState("idle_loop", true);
			AnimState animState4 = new AnimState("salivate", false);
			AnimState animState5 = new AnimState("attack_thrash", false);
			AnimState animState6 = new AnimState("attack_bite", false);
			AnimState animState7 = new AnimState(TheInsatiable.EatPlayerAnim, false);
			AnimState animState8 = new AnimState("intro_hurt", false);
			AnimState animState9 = new AnimState("hurt", false);
			AnimState animState10 = new AnimState("die", false);
			animState2.NextState = animState3;
			animState4.NextState = animState3;
			animState5.NextState = animState3;
			animState6.NextState = animState3;
			animState9.NextState = animState3;
			animState7.NextState = animState3;
			animState8.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState4, null);
			creatureAnimator.AddAnyState("Attack", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState10, null);
			creatureAnimator.AddAnyState("EatPlayerTrigger", animState7, null);
			creatureAnimator.AddAnyState("LiquifySand", animState2, null);
			creatureAnimator.AddAnyState("Salivate", animState4, null);
			creatureAnimator.AddAnyState("Thrash", animState5, null);
			creatureAnimator.AddAnyState("Bite", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState8, () => !this.HasLiquified);
			creatureAnimator.AddAnyState("Hit", animState9, () => this.HasLiquified);
			return creatureAnimator;
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x0026D678 File Offset: 0x0026B878
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.Insert(4, BestiaryMonsterMove.FromAnim(new LocString("monsters", "THE_INSATIABLE.moves.DEVOUR.title"), TheInsatiable.EatPlayerAnim, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher"));
			return list;
		}

		// Token: 0x040026D1 RID: 9937
		private const int _liquifyStatusDrawCount = 3;

		// Token: 0x040026D2 RID: 9938
		private const int _liquifyStatusDiscardCount = 3;

		// Token: 0x040026D3 RID: 9939
		private const int _thrashRepeat = 2;

		// Token: 0x040026D4 RID: 9940
		private const string _liquifySandTrigger = "LiquifySand";

		// Token: 0x040026D5 RID: 9941
		private const string _salivateTrigger = "Salivate";

		// Token: 0x040026D6 RID: 9942
		private const string _biteTrigger = "Bite";

		// Token: 0x040026D7 RID: 9943
		private const string _thrashTrigger = "Thrash";

		// Token: 0x040026D8 RID: 9944
		public const string eatPlayerTrigger = "EatPlayerTrigger";

		// Token: 0x040026D9 RID: 9945
		public const string finisherSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher";

		// Token: 0x040026DA RID: 9946
		private const string _liquifyGroundSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_liquify_ground";

		// Token: 0x040026DB RID: 9947
		private const string _lungingBiteSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite";

		// Token: 0x040026DC RID: 9948
		private const string _salivateSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate";

		// Token: 0x040026DD RID: 9949
		private const string _thrashSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash";

		// Token: 0x040026DE RID: 9950
		private bool _hasLiquified;
	}
}
