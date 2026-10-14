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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A2 RID: 1954
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BygoneEffigy : MonsterModel
	{
		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06005FCD RID: 24525 RVA: 0x0025BFC4 File Offset: 0x0025A1C4
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 132, 127);
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06005FCE RID: 24526 RVA: 0x0025BFD3 File Offset: 0x0025A1D3
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06005FCF RID: 24527 RVA: 0x0025BFDB File Offset: 0x0025A1DB
		private int SlashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x06005FD0 RID: 24528 RVA: 0x0025BFE8 File Offset: 0x0025A1E8
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Stone;
			}
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x0025BFEC File Offset: 0x0025A1EC
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<SlowPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0025C030 File Offset: 0x0025A230
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("SLEEP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InitialSleepMove), new AbstractIntent[]
			{
				new SleepIntent()
			});
			MoveState moveState2 = new MoveState("WAKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WakeMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState3 = new MoveState("SLEEP_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.SleepMove), new AbstractIntent[]
			{
				new SleepIntent()
			});
			MoveState moveState4 = new MoveState("SLASHES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.SlashDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState4;
			moveState3.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState4;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0025C124 File Offset: 0x0025A324
		private async Task InitialSleepMove(IReadOnlyList<Creature> targets)
		{
			LocString locString = MonsterModel.L10NMonsterLookup("BYGONE_EFFIGY.moves.SLEEP.speakLine1");
			ThinkCmd.Play(locString, base.Creature, -1.0);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0025C167 File Offset: 0x0025A367
		private Task SleepMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x0025C170 File Offset: 0x0025A370
		private async Task WakeMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.TriggerEliteSecondPhase();
				}
			}
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 10m, base.Creature, null, false);
			LocString locString = MonsterModel.L10NMonsterLookup("BYGONE_EFFIGY.moves.SLEEP.speakLine2");
			TalkCmd.Play(locString, base.Creature, VfxColor.DarkGray, VfxDuration.Long);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x0025C1B4 File Offset: 0x0025A3B4
		private async Task SlashMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature ncreature = null;
				foreach (Creature creature in targets)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null && (ncreature == null || ncreature.GlobalPosition.X > creatureNode.GlobalPosition.X))
					{
						ncreature = creatureNode;
					}
				}
				NCreature creatureNode2 = base.Creature.GetCreatureNode();
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/SpineBoneNode") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					float num = 1500f * creatureNode2.Visuals.Scale.X;
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, node2D.GlobalPosition.Y);
				}
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.RadialBlur(VfxPosition.Left);
			}
			await DamageCmd.Attack(this.SlashDamage).FromMonster(this).WithAttackerAnim("Attack", 0.1f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
			await Cmd.Wait(0.25f, false);
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x0025C200 File Offset: 0x0025A400
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			animState.AddBranch("Hit", animState4, null);
			animState2.AddBranch("Hit", animState4, null);
			animState4.AddBranch("Hit", animState4, null);
			return creatureAnimator;
		}
	}
}
