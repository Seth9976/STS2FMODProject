using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C9 RID: 1993
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnowledgeDemon : MonsterModel
	{
		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x06006240 RID: 25152 RVA: 0x0026358E File Offset: 0x0026178E
		// (set) Token: 0x06006241 RID: 25153 RVA: 0x00263596 File Offset: 0x00261796
		private int CurseOfKnowledgeCounter
		{
			get
			{
				return this._curseOfKnowledgeCounter;
			}
			set
			{
				base.AssertMutable();
				this._curseOfKnowledgeCounter = value;
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x06006242 RID: 25154 RVA: 0x002635A5 File Offset: 0x002617A5
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 399, 379);
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x06006243 RID: 25155 RVA: 0x002635B7 File Offset: 0x002617B7
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06006244 RID: 25156 RVA: 0x002635BF File Offset: 0x002617BF
		private int SlapDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 17);
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x002635CC File Offset: 0x002617CC
		private int PonderDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06006246 RID: 25158 RVA: 0x002635D9 File Offset: 0x002617D9
		private int KnowledgeOverwhelmingDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06006247 RID: 25159 RVA: 0x002635E5 File Offset: 0x002617E5
		private int PonderStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06006248 RID: 25160 RVA: 0x002635F0 File Offset: 0x002617F0
		// (set) Token: 0x06006249 RID: 25161 RVA: 0x002635F8 File Offset: 0x002617F8
		public bool IsBurnt
		{
			get
			{
				return this._isBurnt;
			}
			set
			{
				base.AssertMutable();
				this._isBurnt = value;
			}
		}

		// Token: 0x0600624A RID: 25162 RVA: 0x00263607 File Offset: 0x00261807
		public override void BeforeRemovedFromRoom()
		{
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.UpdateMusicParameter("knowledge_demon_progress", 5f);
		}

		// Token: 0x0600624B RID: 25163 RVA: 0x00263624 File Offset: 0x00261824
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CURSE_OF_KNOWLEDGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CurseOfKnowledgeMove), new AbstractIntent[]
			{
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("SLAP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlapMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SlapDamage)
			});
			MoveState moveState3 = new MoveState("KNOWLEDGE_OVERWHELMING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.KnowledgeOverwhelmingMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.KnowledgeOverwhelmingDamage, 3)
			});
			MoveState moveState4 = new MoveState("PONDER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PonderMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PonderDamage),
				new HealIntent(),
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("CurseOfKnowledgeBranch");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = conditionalBranchState;
			conditionalBranchState.AddState(moveState, () => this._curseOfKnowledgeCounter < 3);
			conditionalBranchState.AddState(moveState2, () => this._curseOfKnowledgeCounter >= 3);
			list.Add(conditionalBranchState);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState4);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600624C RID: 25164 RVA: 0x00263770 File Offset: 0x00261970
		private async Task CurseOfKnowledgeMove(IReadOnlyList<Creature> targets)
		{
			if (this.CurseOfKnowledgeCounter >= KnowledgeDemon._curseOfKnowledgeSets.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("There are no valid sets at this index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurseOfKnowledgeCounter);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			TalkCmd.Play(KnowledgeDemon._curseOfKnowledgeStartLine, base.Creature, VfxColor.Gold, VfxDuration.Standard);
			await CreatureCmd.TriggerAnim(base.Creature, "MindRotTrigger", 1f);
			List<Task> list = new List<Task>();
			foreach (Creature creature in targets)
			{
				list.Add(this.ChooseCurse(creature));
			}
			await Task.WhenAll(list);
			TalkCmd.Play(KnowledgeDemon._curseOfKnowledgeDoneLine, base.Creature, VfxColor.Gold, VfxDuration.Standard);
			if (base.CombatState.IsLiveCombat())
			{
				this.CurseOfKnowledgeCounter++;
			}
		}

		// Token: 0x0600624D RID: 25165 RVA: 0x002637BC File Offset: 0x002619BC
		private async Task ChooseCurse(Creature target)
		{
			if (!target.IsDead)
			{
				int disintegrationDamage = KnowledgeDemon._disintegrationDamageValues[this.CurseOfKnowledgeCounter];
				List<CardModel> list = KnowledgeDemon._curseOfKnowledgeSets[this.CurseOfKnowledgeCounter].Select<KnowledgeDemon.IChoosable, CardModel>(delegate(KnowledgeDemon.IChoosable c)
				{
					CardModel cardModel3 = this.CombatState.CreateCard((CardModel)c, target.Player);
					if (cardModel3 is Disintegration)
					{
						cardModel3.DynamicVars["DisintegrationPower"].BaseValue = disintegrationDamage;
					}
					return cardModel3;
				}).ToList<CardModel>();
				CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), list, target.Player, false);
				CardModel cardModel2 = cardModel;
				if (cardModel2 != null)
				{
					await ((KnowledgeDemon.IChoosable)cardModel2).OnChosen();
				}
			}
		}

		// Token: 0x0600624E RID: 25166 RVA: 0x00263808 File Offset: 0x00261A08
		private async Task SlapMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SlapDamage).FromMonster(this).WithAttackerAnim("MediumAttackTrigger", 0.5f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_slap", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(null);
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x0026384C File Offset: 0x00261A4C
		private async Task KnowledgeOverwhelmingMove(IReadOnlyList<Creature> targets)
		{
			this.IsBurnt = true;
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("knowledge_demon_progress", 1f);
			}
			await DamageCmd.Attack(this.KnowledgeOverwhelmingDamage).WithHitCount(3).FromMonster(this)
				.WithAttackerAnim("HeavyAttackTrigger", 0.85f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_clap", null)
				.OnlyPlayAnimOnce()
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(null);
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x00263890 File Offset: 0x00261A90
		private async Task PonderMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "HealTrigger", 1.8f);
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("knowledge_demon_progress", 2f);
			}
			this.IsBurnt = false;
			await DamageCmd.Attack(this.PonderDamage).FromMonster(this).WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(null);
			await CreatureCmd.Heal(base.Creature, 30 * base.CombatState.Players.Count, true);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.PonderStrength, base.Creature, null, false);
		}

		// Token: 0x06006251 RID: 25169 RVA: 0x002638D4 File Offset: 0x00261AD4
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_light", false);
			AnimState animState3 = new AnimState("attack_medium", false);
			AnimState animState4 = new AnimState("attack_heavy", false);
			AnimState animState5 = new AnimState("brain_rot", false);
			AnimState animState6 = new AnimState("heal", false);
			AnimState animState7 = new AnimState("burnt_loop", true);
			AnimState animState8 = new AnimState("hurt", false);
			AnimState animState9 = new AnimState("die", false);
			AnimState animState10 = new AnimState("hurt_burnt", false);
			AnimState animState11 = new AnimState("die_burnt", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState7;
			animState5.NextState = animState;
			animState6.NextState = animState;
			animState8.NextState = animState;
			animState10.NextState = animState7;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("LightAttackTrigger", animState2, null);
			creatureAnimator.AddAnyState("MediumAttackTrigger", animState3, null);
			creatureAnimator.AddAnyState("HeavyAttackTrigger", animState4, null);
			creatureAnimator.AddAnyState("MindRotTrigger", animState5, null);
			creatureAnimator.AddAnyState("HealTrigger", animState6, null);
			creatureAnimator.AddAnyState("Dead", animState9, () => !this._isBurnt);
			creatureAnimator.AddAnyState("Hit", animState8, () => !this._isBurnt);
			creatureAnimator.AddAnyState("Dead", animState11, () => this._isBurnt);
			creatureAnimator.AddAnyState("Hit", animState10, () => this._isBurnt);
			return creatureAnimator;
		}

		// Token: 0x040025F1 RID: 9713
		private const string _knowledgeDemonCustomTrackName = "knowledge_demon_progress";

		// Token: 0x040025F2 RID: 9714
		private const string _clapSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_clap";

		// Token: 0x040025F3 RID: 9715
		private const string _flameSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_flame";

		// Token: 0x040025F4 RID: 9716
		private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_slap";

		// Token: 0x040025F5 RID: 9717
		private static readonly LocString _curseOfKnowledgeStartLine = MonsterModel.L10NMonsterLookup("KNOWLEDGE_DEMON.moves.CURSE_OF_KNOWLEDGE.startLine");

		// Token: 0x040025F6 RID: 9718
		private static readonly LocString _curseOfKnowledgeDoneLine = MonsterModel.L10NMonsterLookup("KNOWLEDGE_DEMON.moves.CURSE_OF_KNOWLEDGE.doneLine");

		// Token: 0x040025F7 RID: 9719
		private static readonly int[] _disintegrationDamageValues = new int[] { 6, 7, 8 };

		// Token: 0x040025F8 RID: 9720
		private static readonly IReadOnlyList<IReadOnlyList<KnowledgeDemon.IChoosable>> _curseOfKnowledgeSets = new <>z__ReadOnlyArray<IReadOnlyList<KnowledgeDemon.IChoosable>>(new IReadOnlyList<KnowledgeDemon.IChoosable>[]
		{
			new <>z__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[]
			{
				ModelDb.Card<Disintegration>(),
				ModelDb.Card<MindRot>()
			}),
			new <>z__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[]
			{
				ModelDb.Card<Disintegration>(),
				ModelDb.Card<Sloth>()
			}),
			new <>z__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[]
			{
				ModelDb.Card<Disintegration>(),
				ModelDb.Card<WasteAway>()
			})
		});

		// Token: 0x040025F9 RID: 9721
		private int _curseOfKnowledgeCounter;

		// Token: 0x040025FA RID: 9722
		private const int _knowledgeOverwhelmingRepeat = 3;

		// Token: 0x040025FB RID: 9723
		private const int _ponderHeal = 30;

		// Token: 0x040025FC RID: 9724
		private bool _isBurnt;

		// Token: 0x040025FD RID: 9725
		private const string _mindRotTrigger = "MindRotTrigger";

		// Token: 0x040025FE RID: 9726
		private const string _lightAttackTrigger = "LightAttackTrigger";

		// Token: 0x040025FF RID: 9727
		private const string _mediumAttackTrigger = "MediumAttackTrigger";

		// Token: 0x04002600 RID: 9728
		private const string _heavyAttackTrigger = "HeavyAttackTrigger";

		// Token: 0x04002601 RID: 9729
		private const string _healTrigger = "HealTrigger";

		/// <summary>
		/// For cards that specifically come from Knowledge Demon's Curse of Knowledge move.
		/// Unlike normal cards, these have an immediate effect when chosen by the player.
		/// </summary>
		// Token: 0x02001DC6 RID: 7622
		public interface IChoosable
		{
			// Token: 0x0600B434 RID: 46132
			Task OnChosen();
		}
	}
}
