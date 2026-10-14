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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007B8 RID: 1976
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FatGremlin : MonsterModel
	{
		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x0026013A File Offset: 0x0025E33A
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 14, 13);
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06006138 RID: 24888 RVA: 0x00260146 File Offset: 0x0025E346
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06006139 RID: 24889 RVA: 0x00260152 File Offset: 0x0025E352
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x0600613A RID: 24890 RVA: 0x00260155 File Offset: 0x0025E355
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_die";
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x0600613B RID: 24891 RVA: 0x0026015C File Offset: 0x0025E35C
		// (set) Token: 0x0600613C RID: 24892 RVA: 0x00260164 File Offset: 0x0025E364
		private bool IsAwake
		{
			get
			{
				return this._isAwake;
			}
			set
			{
				base.AssertMutable();
				this._isAwake = value;
			}
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x00260174 File Offset: 0x0025E374
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			MoveState moveState2 = new MoveState("FLEE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FleeMove), new AbstractIntent[]
			{
				new EscapeIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x002601F4 File Offset: 0x0025E3F4
		private async Task SpawnedMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Creature, "WakeUpTrigger", 0.8f);
			this.IsAwake = true;
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00260238 File Offset: 0x0025E438
		private async Task FleeMove(IReadOnlyList<Creature> targets)
		{
			LocString locString = MonsterModel.L10NMonsterLookup("FAT_GREMLIN.moves.FLEE.banter");
			TalkCmd.Play(locString, base.Creature, VfxColor.Swamp, VfxDuration.Standard);
			await Cmd.CustomScaledWait(0.75f, 1.25f, false, default(CancellationToken));
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.ToggleIsInteractable(false);
				}
			}
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_escape", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "FleeTrigger", 0f);
			await Cmd.Wait(1.25f, false);
			await CreatureCmd.Escape(base.Creature, true);
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x0026027C File Offset: 0x0025E47C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			string text = ((base.Creature.HasPower<HeistPower>() || !base.CombatState.IsLiveCombat()) ? string.Empty : "_no_bag/");
			AnimState animState = new AnimState(text + "awake_loop", true);
			AnimState animState2 = new AnimState(text + "spawn", false);
			AnimState animState3 = new AnimState(text + "flee", false);
			AnimState animState4 = new AnimState(text + "stunned_loop", true);
			AnimState animState5 = new AnimState(text + "wake_up", false);
			AnimState animState6 = new AnimState(text + "hurt_stunned", false);
			AnimState animState7 = new AnimState(text + "hurt_awake", false);
			AnimState animState8 = new AnimState(text + "die", false);
			animState2.NextState = animState4;
			animState6.NextState = animState4;
			animState7.NextState = animState;
			animState5.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState2, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("FleeTrigger", animState3, null);
			creatureAnimator.AddAnyState("WakeUpTrigger", animState5, null);
			creatureAnimator.AddAnyState("Dead", animState8, null);
			creatureAnimator.AddAnyState("Hit", animState7, () => this.IsAwake);
			creatureAnimator.AddAnyState("Hit", animState6, () => !this.IsAwake);
			return creatureAnimator;
		}

		// Token: 0x040025A7 RID: 9639
		private const string _fleeTrigger = "FleeTrigger";

		// Token: 0x040025A8 RID: 9640
		private const string _wakeUpTrigger = "WakeUpTrigger";

		// Token: 0x040025A9 RID: 9641
		private const string _escapeSfx = "event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_escape";

		// Token: 0x040025AA RID: 9642
		private bool _isAwake;
	}
}
