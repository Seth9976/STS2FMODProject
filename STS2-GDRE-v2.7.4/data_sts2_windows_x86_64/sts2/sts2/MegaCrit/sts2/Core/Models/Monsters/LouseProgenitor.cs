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
	// Token: 0x020007CF RID: 1999
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LouseProgenitor : MonsterModel
	{
		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00264A8D File Offset: 0x00262C8D
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 138, 134);
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x00264A9F File Offset: 0x00262C9F
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 141, 136);
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x060062B4 RID: 25268 RVA: 0x00264AB1 File Offset: 0x00262CB1
		protected override string AttackSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack";
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00264AB8 File Offset: 0x00262CB8
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_die";
			}
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x060062B6 RID: 25270 RVA: 0x00264ABF File Offset: 0x00262CBF
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x060062B7 RID: 25271 RVA: 0x00264AC2 File Offset: 0x00262CC2
		// (set) Token: 0x060062B8 RID: 25272 RVA: 0x00264ACA File Offset: 0x00262CCA
		public bool Curled
		{
			get
			{
				return this._curled;
			}
			set
			{
				base.AssertMutable();
				this._curled = value;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x060062B9 RID: 25273 RVA: 0x00264AD9 File Offset: 0x00262CD9
		private int WebDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x060062BA RID: 25274 RVA: 0x00264AE6 File Offset: 0x00262CE6
		private int PounceDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x060062BB RID: 25275 RVA: 0x00264AF3 File Offset: 0x00262CF3
		private int CurlBlock
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 14);
			}
		}

		// Token: 0x060062BC RID: 25276 RVA: 0x00264B00 File Offset: 0x00262D00
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<CurlUpPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.CurlBlock, base.Creature, null, false);
		}

		// Token: 0x060062BD RID: 25277 RVA: 0x00264B44 File Offset: 0x00262D44
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("WEB_CANNON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WebMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.WebDamage),
				new DebuffIntent(false)
			});
			MoveState moveState2 = new MoveState("POUNCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PounceMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.PounceDamage)
			});
			MoveState moveState3 = new MoveState("CURL_AND_GROW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CurlAndGrowMove), new AbstractIntent[]
			{
				new DefendIntent(),
				new BuffIntent()
			});
			moveState.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState3);
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060062BE RID: 25278 RVA: 0x00264C14 File Offset: 0x00262E14
		private async Task WebMove(IReadOnlyList<Creature> targets)
		{
			if (this.Curled)
			{
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl", 1f);
				await CreatureCmd.TriggerAnim(base.Creature, "Uncurl", 0.9f);
				this.Curled = false;
			}
			await DamageCmd.Attack(this.WebDamage).FromMonster(this).WithAttackerAnim("Web", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack_web", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
			await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), targets, 2m, base.Creature, null, false);
		}

		// Token: 0x060062BF RID: 25279 RVA: 0x00264C60 File Offset: 0x00262E60
		private async Task CurlAndGrowMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Curl", 0.25f);
			await CreatureCmd.GainBlock(base.Creature, this.CurlBlock, ValueProp.Move, null, false);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
			this.Curled = true;
		}

		// Token: 0x060062C0 RID: 25280 RVA: 0x00264CA4 File Offset: 0x00262EA4
		private async Task PounceMove(IReadOnlyList<Creature> targets)
		{
			if (this.Curled)
			{
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl", 1f);
				await CreatureCmd.TriggerAnim(base.Creature, "Uncurl", 0.9f);
				this.Curled = false;
			}
			await DamageCmd.Attack(this.PounceDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.Execute(null);
		}

		// Token: 0x060062C1 RID: 25281 RVA: 0x00264CE8 File Offset: 0x00262EE8
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("curl", false);
			AnimState animState3 = new AnimState("uncurl", false);
			AnimState animState4 = new AnimState("curled_loop", true);
			AnimState animState5 = new AnimState("attack", false);
			AnimState animState6 = new AnimState("attack_web", false);
			AnimState animState7 = new AnimState("hurt", false);
			AnimState animState8 = new AnimState("die", false);
			AnimState animState9 = new AnimState("die_curled", false);
			animState.AddBranch("Curl", animState2, null);
			animState.AddBranch("Attack", animState5, null);
			animState.AddBranch("Web", animState6, null);
			animState.AddBranch("Hit", animState7, null);
			animState.AddBranch("Dead", animState8, null);
			animState2.NextState = animState4;
			animState4.AddBranch("Uncurl", animState3, null);
			animState4.AddBranch("Dead", animState9, null);
			animState5.NextState = animState;
			animState5.AddBranch("Hit", animState7, null);
			animState5.AddBranch("Dead", animState8, null);
			animState6.NextState = animState;
			animState6.AddBranch("Hit", animState7, null);
			animState6.AddBranch("Dead", animState8, null);
			animState7.NextState = animState;
			animState7.AddBranch("Hit", animState7, null);
			animState7.AddBranch("Dead", animState8, null);
			animState7.AddBranch("Curl", animState2, null);
			animState3.NextState = animState;
			return new CreatureAnimator(animState, controller);
		}

		// Token: 0x04002616 RID: 9750
		public const string curlTrigger = "Curl";

		// Token: 0x04002617 RID: 9751
		private const string _uncurlTrigger = "Uncurl";

		// Token: 0x04002618 RID: 9752
		private const string _webTrigger = "Web";

		// Token: 0x04002619 RID: 9753
		private const string _webSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack_web";

		// Token: 0x0400261A RID: 9754
		public const string curlSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl";

		// Token: 0x0400261B RID: 9755
		private const string _uncurlSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl";

		// Token: 0x0400261C RID: 9756
		private bool _curled;

		// Token: 0x0400261D RID: 9757
		private const int _webFrail = 2;

		// Token: 0x0400261E RID: 9758
		private const int _growStrength = 5;
	}
}
