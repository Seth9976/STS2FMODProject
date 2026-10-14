using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007DE RID: 2014
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhantasmalGardener : MonsterModel
	{
		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x0600638A RID: 25482 RVA: 0x00267387 File Offset: 0x00265587
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 26);
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x00267393 File Offset: 0x00265593
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 32, 31);
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600638C RID: 25484 RVA: 0x0026739F File Offset: 0x0026559F
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 5);
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600638D RID: 25485 RVA: 0x002673AA File Offset: 0x002655AA
		private int LashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 7);
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600638E RID: 25486 RVA: 0x002673B5 File Offset: 0x002655B5
		private int FlailDamage
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600638F RID: 25487 RVA: 0x002673B8 File Offset: 0x002655B8
		private int FlailRepeat
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06006390 RID: 25488 RVA: 0x002673C3 File Offset: 0x002655C3
		private int EnlargeStr
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06006391 RID: 25489 RVA: 0x002673CE File Offset: 0x002655CE
		private int SkittishAmount
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 7, 6);
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x06006392 RID: 25490 RVA: 0x002673D8 File Offset: 0x002655D8
		// (set) Token: 0x06006393 RID: 25491 RVA: 0x002673E0 File Offset: 0x002655E0
		public int EnlargeTriggers
		{
			get
			{
				return this._enlargeTriggers;
			}
			set
			{
				base.AssertMutable();
				this._enlargeTriggers = value;
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06006394 RID: 25492 RVA: 0x002673EF File Offset: 0x002655EF
		public override bool ShouldFadeAfterDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x06006395 RID: 25493 RVA: 0x002673F2 File Offset: 0x002655F2
		// (set) Token: 0x06006396 RID: 25494 RVA: 0x002673FA File Offset: 0x002655FA
		public float CurrentScale { get; private set; } = 1f;

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x06006397 RID: 25495 RVA: 0x00267403 File Offset: 0x00265603
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Magic;
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06006398 RID: 25496 RVA: 0x00267406 File Offset: 0x00265606
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_die";
			}
		}

		// Token: 0x06006399 RID: 25497 RVA: 0x00267410 File Offset: 0x00265610
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			string slotName = base.Creature.SlotName;
			bool flag = slotName == "first" || slotName == "third";
			if (flag)
			{
				megaSkin.AddSkin(data.FindSkin("tall"));
			}
			else
			{
				megaSkin.AddSkin(data.FindSkin("short"));
			}
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x00267490 File Offset: 0x00265690
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SkittishPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.SkittishAmount, base.Creature, null, false);
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x002674D4 File Offset: 0x002656D4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState2 = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.LashDamage)
			});
			MoveState moveState3 = new MoveState("FLAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlailMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.FlailDamage, this.FlailRepeat)
			});
			MoveState moveState4 = new MoveState("ENLARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnlargeMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			conditionalBranchState.AddState(moveState3, () => base.Creature.SlotName == "first");
			conditionalBranchState.AddState(moveState, () => base.Creature.SlotName == "second");
			conditionalBranchState.AddState(moveState2, () => base.Creature.SlotName == "third");
			conditionalBranchState.AddState(moveState4, () => base.Creature.SlotName == "fourth");
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState4);
			list.Add(moveState3);
			return new MonsterMoveStateMachine(list, conditionalBranchState);
		}

		// Token: 0x0600639C RID: 25500 RVA: 0x00267634 File Offset: 0x00265834
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.SetDefaultScaleTo(this.CurrentScale, 0.75f);
				}
			}
			await DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x00267678 File Offset: 0x00265878
		private async Task LashMove(IReadOnlyList<Creature> targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.SetDefaultScaleTo(this.CurrentScale, 0.75f);
				}
			}
			await DamageCmd.Attack(this.LashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite", null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600639E RID: 25502 RVA: 0x002676BC File Offset: 0x002658BC
		private async Task FlailMove(IReadOnlyList<Creature> targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.SetDefaultScaleTo(this.CurrentScale, 0.35f);
				}
			}
			await DamageCmd.Attack(this.FlailDamage).WithHitCount(this.FlailRepeat).OnlyPlayAnimOnce()
				.FromMonster(this)
				.WithAttackerAnim("AttackMulti", 0.3f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_lick", null)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x0600639F RID: 25503 RVA: 0x00267700 File Offset: 0x00265900
		private async Task EnlargeMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 1.5f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.EnlargeStr, base.Creature, null, false);
			this.EnlargeTriggers++;
			this.CurrentScale = 1f + 0.1f * Mathf.Log((float)this.EnlargeTriggers + 1f);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.SetDefaultScaleTo(this.CurrentScale, 0.75f);
				}
			}
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x00267744 File Offset: 0x00265944
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("buff", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_multi", false);
			AnimState animState5 = new AnimState("hurt_extended", false);
			AnimState animState6 = new AnimState("hurt", false);
			AnimState animState7 = new AnimState("die", false);
			AnimState animState8 = new AnimState("block_loop", true);
			AnimState animState9 = new AnimState("block_start", false);
			AnimState animState10 = new AnimState("block_end", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState;
			animState6.NextState = animState8;
			animState9.NextState = animState8;
			animState10.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("AttackMulti", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState7, null);
			creatureAnimator.AddAnyState("Hit", animState5, () => !base.Creature.GetPower<SkittishPower>().HasGainedBlockThisTurn);
			creatureAnimator.AddAnyState("Hit", animState6, () => base.Creature.GetPower<SkittishPower>().HasGainedBlockThisTurn);
			creatureAnimator.AddAnyState("BlockStart", animState9, null);
			creatureAnimator.AddAnyState("BlockEnd", animState10, null);
			return creatureAnimator;
		}

		// Token: 0x060063A1 RID: 25505 RVA: 0x002678AC File Offset: 0x00265AAC
		public override List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = base.GenerateBestiaryMoveList(creatureVisuals);
			list.RemoveAll((BestiaryMonsterMove m) => m.stateId == "LASH_MOVE");
			return list;
		}

		// Token: 0x04002650 RID: 9808
		private int _enlargeTriggers;

		// Token: 0x04002652 RID: 9810
		private const string _attackMultiTrigger = "AttackMulti";

		// Token: 0x04002653 RID: 9811
		public const string blockStartTrigger = "BlockStart";

		// Token: 0x04002654 RID: 9812
		public const string blockEndTrigger = "BlockEnd";

		// Token: 0x04002655 RID: 9813
		private const string _biteSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite";

		// Token: 0x04002656 RID: 9814
		private const string _lickSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_lick";

		// Token: 0x04002657 RID: 9815
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_buff";
	}
}
