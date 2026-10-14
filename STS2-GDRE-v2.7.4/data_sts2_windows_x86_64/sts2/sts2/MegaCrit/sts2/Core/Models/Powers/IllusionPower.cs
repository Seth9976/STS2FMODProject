using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000698 RID: 1688
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IllusionPower : PowerModel
	{
		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x060058FE RID: 22782 RVA: 0x0024FA6E File Offset: 0x0024DC6E
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x060058FF RID: 22783 RVA: 0x0024FA71 File Offset: 0x0024DC71
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x0024FA74 File Offset: 0x0024DC74
		public override bool ShouldPlayVfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06005901 RID: 22785 RVA: 0x0024FA77 File Offset: 0x0024DC77
		// (set) Token: 0x06005902 RID: 22786 RVA: 0x0024FA7F File Offset: 0x0024DC7F
		[Nullable(2)]
		public string FollowUpStateId
		{
			[NullableContext(2)]
			get
			{
				return this._followUpStateId;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._followUpStateId = value;
			}
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x0024FA8E File Offset: 0x0024DC8E
		protected override object InitInternalData()
		{
			return new IllusionPower.Data();
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06005904 RID: 22788 RVA: 0x0024FA95 File Offset: 0x0024DC95
		public bool IsReviving
		{
			get
			{
				return base.GetInternalData<IllusionPower.Data>().isReviving;
			}
		}

		/// <summary>
		/// Illusions keep their buffs (including IllusionPower itself) after dying.
		/// We ignore Temporary powers (ie temp strength down from dark shackles or dying star) so that the
		/// temp powers they are applying also go away
		/// </summary>
		// Token: 0x06005905 RID: 22789 RVA: 0x0024FAA2 File Offset: 0x0024DCA2
		public override bool ShouldPowerBeRemovedOnDeath(PowerModel power)
		{
			return power.Type == PowerType.Debuff && !(power is ITemporaryPower);
		}

		// Token: 0x06005906 RID: 22790 RVA: 0x0024FABB File Offset: 0x0024DCBB
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			if (base.Owner.HasPower<MinionPower>())
			{
				return Task.CompletedTask;
			}
			return PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), base.Owner, 1m, null, null, false);
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x0024FAE8 File Offset: 0x0024DCE8
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					await CreatureCmd.TriggerAnim(base.Owner, "StunTrigger", 0f);
					base.GetInternalData<IllusionPower.Data>().isReviving = true;
					MoveState moveState = new MoveState("REVIVE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReviveMove), new AbstractIntent[]
					{
						new HealIntent()
					})
					{
						FollowUpStateId = (this.FollowUpStateId ?? base.Owner.Monster.MoveStateMachine.StateLog.Last<MonsterState>().Id),
						MustPerformOnceBeforeTransitioning = true
					};
					base.Owner.Monster.SetMoveImmediate(moveState, false);
				}
			}
		}

		/// <summary>
		/// This is so the owner doesn't receive powers while it is reviving.
		/// </summary>
		// Token: 0x06005908 RID: 22792 RVA: 0x0024FB3B File Offset: 0x0024DD3B
		public override bool ShouldAllowHitting(Creature creature)
		{
			return creature != base.Owner || !this.IsReviving;
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x0024FB53 File Offset: 0x0024DD53
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x0024FB64 File Offset: 0x0024DD64
		public override async Task AfterCombatEnd(CombatRoom room)
		{
			if (!base.Owner.IsAlive)
			{
				await CreatureCmd.TriggerAnim(base.Owner, "Dead", 0.1f);
			}
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x0024FBA8 File Offset: 0x0024DDA8
		private async Task ReviveMove(IReadOnlyList<Creature> targets)
		{
			await CreatureCmd.TriggerAnim(base.Owner, "WakeUpTrigger", 0f);
			base.GetInternalData<IllusionPower.Data>().isReviving = false;
			await CreatureCmd.Heal(base.Owner, base.Owner.MaxHp - base.Owner.CurrentHp, true);
			if (base.Owner.Monster is Parafright)
			{
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_heal", 1f);
			}
		}

		// Token: 0x040024FE RID: 9470
		public const string stunTrigger = "StunTrigger";

		// Token: 0x040024FF RID: 9471
		public const string wakeUpTrigger = "WakeUpTrigger";

		// Token: 0x04002500 RID: 9472
		[Nullable(2)]
		private string _followUpStateId;

		// Token: 0x02001BEE RID: 7150
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006F69 RID: 28521
			public bool isReviving;
		}
	}
}
