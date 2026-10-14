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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007ED RID: 2029
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SnappingJaxfruit : MonsterModel
	{
		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600648D RID: 25741 RVA: 0x0026A48E File Offset: 0x0026868E
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 34, 31);
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600648E RID: 25742 RVA: 0x0026A49A File Offset: 0x0026869A
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 33);
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x0600648F RID: 25743 RVA: 0x0026A4A6 File Offset: 0x002686A6
		private int EnergyDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06006490 RID: 25744 RVA: 0x0026A4B1 File Offset: 0x002686B1
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Plant;
			}
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x0026A4B4 File Offset: 0x002686B4
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop");
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x0026A4F7 File Offset: 0x002686F7
		public override void BeforeRemovedFromRoom()
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop");
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x0026A50C File Offset: 0x0026870C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("ENERGY_ORB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnergyOrb), new AbstractIntent[]
			{
				new SingleAttackIntent(this.EnergyDamage),
				new BuffIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x0026A568 File Offset: 0x00268768
		public async Task EnergyOrb(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature creatureNode = base.Creature.GetCreatureNode();
				if (creatureNode != null)
				{
					Player me = LocalContext.GetMe(base.CombatState);
					Creature creature = ((me != null) ? me.Creature : null);
					NSnappingJaxfruitVfx specialNode = creatureNode.GetSpecialNode<NSnappingJaxfruitVfx>("Visuals/NSnappingJaxfruitVfx");
					if (specialNode != null)
					{
						specialNode.SetTarget(creature);
					}
				}
			}
			await DamageCmd.Attack(this.EnergyDamage).FromMonster(this).WithAttackerAnim("Cast", 0.25f, null)
				.Execute(null);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x0026A5AC File Offset: 0x002687AC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("die", false);
			AnimState animState4 = new AnimState("cast", false);
			animState2.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Dead", animState3, null);
			creatureAnimator.AddAnyState("Cast", animState4, null);
			creatureAnimator.AddAnyState("Hit", animState2, null);
			return creatureAnimator;
		}

		// Token: 0x0400269B RID: 9883
		private const string _idleLoopSfx = "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop";
	}
}
