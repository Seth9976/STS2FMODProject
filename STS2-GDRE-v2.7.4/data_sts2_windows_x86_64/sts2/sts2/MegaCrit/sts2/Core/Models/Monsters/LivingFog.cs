using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007CD RID: 1997
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LivingFog : MonsterModel
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600628E RID: 25230 RVA: 0x0026452F File Offset: 0x0026272F
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 82, 80);
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x0600628F RID: 25231 RVA: 0x0026453B File Offset: 0x0026273B
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06006290 RID: 25232 RVA: 0x00264543 File Offset: 0x00262743
		private int AdvancedGasDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06006291 RID: 25233 RVA: 0x0026454F File Offset: 0x0026274F
		private int BloatDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06006292 RID: 25234 RVA: 0x0026455A File Offset: 0x0026275A
		private int SuperGasBlastDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06006293 RID: 25235 RVA: 0x00264566 File Offset: 0x00262766
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06006294 RID: 25236 RVA: 0x00264569 File Offset: 0x00262769
		// (set) Token: 0x06006295 RID: 25237 RVA: 0x00264571 File Offset: 0x00262771
		private int BloatAmount
		{
			get
			{
				return this._bloatAmount;
			}
			set
			{
				base.AssertMutable();
				this._bloatAmount = value;
			}
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06006296 RID: 25238 RVA: 0x00264580 File Offset: 0x00262780
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06006297 RID: 25239 RVA: 0x00264583 File Offset: 0x00262783
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_die";
			}
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x0026458C File Offset: 0x0026278C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ADVANCED_GAS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AdvancedGasMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.AdvancedGasDamage),
				new CardDebuffIntent()
			});
			MoveState moveState2 = new MoveState("BLOAT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BloatMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BloatDamage),
				new SummonIntent()
			});
			MoveState moveState3 = new MoveState("SUPER_GAS_BLAST_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SuperGasBlastMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SuperGasBlastDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState3);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006299 RID: 25241 RVA: 0x00264664 File Offset: 0x00262864
		private async Task AdvancedGasMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.AdvancedGasDamage).FromMonster(this).WithAttackerAnim("Cast", 1.25f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow", null)
				.WithHitVfxNode((Creature _) => NGaseousImpactVfx.Create(CombatSide.Player, base.CombatState, new Color("#402f45")))
				.Execute(null);
			await PowerCmd.Apply<SmoggyPower>(new ThrowingPlayerChoiceContext(), targets, 1m, base.Creature, null, false);
		}

		// Token: 0x0600629A RID: 25242 RVA: 0x002646B0 File Offset: 0x002628B0
		private async Task BloatMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/living_fog/living_fog_summon", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "SpawnBomb", 0.35f);
			for (int i = 0; i < this.BloatAmount; i++)
			{
				EncounterModel encounter = base.CombatState.Encounter;
				string text = ((encounter != null) ? encounter.GetNextSlot(base.CombatState) : null);
				if (!string.IsNullOrEmpty(text))
				{
					SfxCmd.Play("event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_appear", 1f);
					await CreatureCmd.Add<GasBomb>(base.CombatState, text);
				}
			}
			await DamageCmd.Attack(this.BloatDamage).FromMonster(this).WithAttackerAnim("Attack", 0.1f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow", null)
				.WithHitVfxNode((Creature _) => NGaseousImpactVfx.Create(CombatSide.Player, base.CombatState, new Color("#402f45")))
				.Execute(null);
		}

		// Token: 0x0600629B RID: 25243 RVA: 0x002646F4 File Offset: 0x002628F4
		private async Task SuperGasBlastMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.SuperGasBlastDamage).FromMonster(this).WithAttackerAnim("Attack", 0.1f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow", null)
				.WithHitVfxNode((Creature _) => NGaseousImpactVfx.Create(CombatSide.Player, base.CombatState, new Color("#402f45")))
				.Execute(null);
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x00264738 File Offset: 0x00262938
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("debuff", false);
			AnimState animState3 = new AnimState("spawn_bomb", false);
			AnimState animState4 = new AnimState("attack", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState3.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			creatureAnimator.AddAnyState("SpawnBomb", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x04002611 RID: 9745
		private int _bloatAmount = 1;

		// Token: 0x04002612 RID: 9746
		private const string _spawnBombTrigger = "SpawnBomb";

		// Token: 0x04002613 RID: 9747
		private const string _attackBlowSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow";

		// Token: 0x04002614 RID: 9748
		private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_summon";

		// Token: 0x04002615 RID: 9749
		private const string _appearsSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_appear";
	}
}
