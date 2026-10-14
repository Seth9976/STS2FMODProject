using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x0200080B RID: 2059
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Wriggler : MonsterModel
	{
		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x060066D0 RID: 26320 RVA: 0x00271234 File Offset: 0x0026F434
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
			}
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x060066D1 RID: 26321 RVA: 0x00271240 File Offset: 0x0026F440
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
			}
		}

		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x060066D2 RID: 26322 RVA: 0x0027124C File Offset: 0x0026F44C
		private int BiteDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x060066D3 RID: 26323 RVA: 0x00271257 File Offset: 0x0026F457
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Plant;
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x060066D4 RID: 26324 RVA: 0x0027125A File Offset: 0x0026F45A
		// (set) Token: 0x060066D5 RID: 26325 RVA: 0x00271262 File Offset: 0x0026F462
		public bool StartStunned
		{
			get
			{
				return this._startStunned;
			}
			set
			{
				base.AssertMutable();
				this._startStunned = value;
			}
		}

		// Token: 0x060066D6 RID: 26326 RVA: 0x00271274 File Offset: 0x0026F474
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NASTY_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.BiteDamage)
			});
			MoveState moveState2 = new MoveState("WRIGGLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WriggleMove), new AbstractIntent[]
			{
				new BuffIntent(),
				new StatusIntent(1)
			});
			MoveState moveState3 = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[]
			{
				new StunIntent()
			});
			ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
			conditionalBranchState.AddState(moveState, () => base.Creature.SlotName == "wriggler1");
			conditionalBranchState.AddState(moveState2, () => base.Creature.SlotName == "wriggler2");
			conditionalBranchState.AddState(moveState, () => base.Creature.SlotName == "wriggler3");
			conditionalBranchState.AddState(moveState2, () => base.Creature.SlotName == "wriggler4");
			moveState3.FollowUpState = conditionalBranchState;
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState;
			list.Add(moveState3);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(conditionalBranchState);
			MonsterState monsterState = (this.StartStunned ? moveState3 : conditionalBranchState);
			return new MonsterMoveStateMachine(list, monsterState);
		}

		// Token: 0x060066D7 RID: 26327 RVA: 0x002713AB File Offset: 0x0026F5AB
		private Task SpawnedMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x002713B4 File Offset: 0x0026F5B4
		private async Task BiteMove(IReadOnlyList<Creature> targets)
		{
			AttackCommand attackCommand = DamageCmd.Attack(this.BiteDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_bite", null, null);
			Func<Creature, Node2D> func;
			if ((func = Wriggler.<>O.<0>__Create) == null)
			{
				func = (Wriggler.<>O.<0>__Create = new Func<Creature, Node2D>(NWormyImpactVfx.Create));
			}
			await attackCommand.WithHitVfxNode(func).Execute(null);
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x002713F8 File Offset: 0x0026F5F8
		private async Task WriggleMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.AttackSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Attack", 0.15f);
			foreach (Creature creature in targets)
			{
				NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(creature);
				if (nwormyImpactVfx != null)
				{
					Control vfxContainer = creature.GetVfxContainer();
					if (vfxContainer != null)
					{
						vfxContainer.AddChildSafely(nwormyImpactVfx);
					}
				}
			}
			await CardPileCmd.AddToCombatAndPreview<Infection>(targets, PileType.Discard, 1, null, CardPilePosition.Bottom);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 2m, base.Creature, null, false);
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x00271443 File Offset: 0x0026F643
		protected override bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return moveStateId != "NASTY_BITE_MOVE" && moveStateId != "SPAWNED_MOVE";
		}

		// Token: 0x04002748 RID: 10056
		private const string _nastyBiteMoveId = "NASTY_BITE_MOVE";

		// Token: 0x04002749 RID: 10057
		private const string _spawnedMoveId = "SPAWNED_MOVE";

		// Token: 0x0400274A RID: 10058
		private const string _initMoveId = "INIT_MOVE";

		// Token: 0x0400274B RID: 10059
		private const int _wriggleStrength = 2;

		// Token: 0x0400274C RID: 10060
		private bool _startStunned;

		// Token: 0x02001EBF RID: 7871
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04007D7E RID: 32126
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <0>__Create;
		}
	}
}
