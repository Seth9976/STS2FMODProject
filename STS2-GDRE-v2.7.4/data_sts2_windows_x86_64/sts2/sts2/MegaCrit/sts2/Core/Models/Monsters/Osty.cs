using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D9 RID: 2009
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Osty : MonsterModel
	{
		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600633E RID: 25406 RVA: 0x0026647C File Offset: 0x0026467C
		public static Vector2 MinOffset
		{
			get
			{
				return new Vector2(150f, -75f);
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600633F RID: 25407 RVA: 0x0026648D File Offset: 0x0026468D
		public static Vector2 MaxOffset
		{
			get
			{
				return new Vector2(250f, -75f);
			}
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x06006340 RID: 25408 RVA: 0x0026649E File Offset: 0x0026469E
		public static Vector2 ScaleRange
		{
			get
			{
				return new Vector2(1f, 2f);
			}
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06006341 RID: 25409 RVA: 0x002664AF File Offset: 0x002646AF
		public override int MinInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06006342 RID: 25410 RVA: 0x002664B2 File Offset: 0x002646B2
		public override int MaxInitialHp
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06006343 RID: 25411 RVA: 0x002664B5 File Offset: 0x002646B5
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/characters/osty/osty_die";
			}
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x06006344 RID: 25412 RVA: 0x002664BC File Offset: 0x002646BC
		public override bool HasDeathSfx
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x002664BF File Offset: 0x002646BF
		public override bool IsHealthBarVisible
		{
			get
			{
				return base.Creature.IsAlive;
			}
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x002664CC File Offset: 0x002646CC
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x0026651C File Offset: 0x0026471C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("attack_poke", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			AnimState animState7 = new AnimState("dead_loop", true);
			AnimState animState8 = new AnimState("revive", false);
			animState.AddBranch("Hit", animState5, null);
			animState2.NextState = animState;
			animState2.AddBranch("Hit", animState5, null);
			animState3.NextState = animState;
			animState3.AddBranch("Hit", animState5, null);
			animState4.NextState = animState;
			animState4.AddBranch("Hit", animState5, null);
			animState5.NextState = animState;
			animState5.AddBranch("Hit", animState5, null);
			animState6.NextState = animState7;
			animState8.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("attack_poke", animState4, null);
			creatureAnimator.AddAnyState("Revive", animState8, null);
			return creatureAnimator;
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x00266655 File Offset: 0x00264855
		public static bool CheckMissingWithAnim(Player owner)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.ShakeOstyIfDead(owner);
			}
			return owner.IsOstyMissing;
		}

		// Token: 0x0400263C RID: 9788
		public const float attackerAnimDelay = 0.3f;

		// Token: 0x0400263D RID: 9789
		public const string pokeAnim = "attack_poke";

		// Token: 0x0400263E RID: 9790
		public const string ostyAttackSfx = "event:/sfx/characters/osty/osty_attack";
	}
}
