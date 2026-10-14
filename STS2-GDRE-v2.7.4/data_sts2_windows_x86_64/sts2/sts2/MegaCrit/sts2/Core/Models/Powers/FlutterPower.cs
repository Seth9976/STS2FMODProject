using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000679 RID: 1657
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlutterPower : PowerModel
	{
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06005856 RID: 22614 RVA: 0x0024E746 File Offset: 0x0024C946
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06005857 RID: 22615 RVA: 0x0024E749 File Offset: 0x0024C949
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06005858 RID: 22616 RVA: 0x0024E74C File Offset: 0x0024C94C
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06005859 RID: 22617 RVA: 0x0024E74F File Offset: 0x0024C94F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 50m));
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x0024E767 File Offset: 0x0024C967
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			return base.DynamicVars["DamageDecrease"].BaseValue / 100m;
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x0024E7A8 File Offset: 0x0024C9A8
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage != 0)
				{
					if (props.IsPoweredAttack())
					{
						await PowerCmd.Decrement(this);
						if (base.Amount <= 0)
						{
							await CreatureCmd.TriggerAnim(base.Owner, "StunTrigger", 0.6f);
							string nextState = base.Owner.Monster.MoveStateMachine.StateLog.Last<MonsterState>().GetNextState(base.Owner, base.Owner.Monster.RunRng.MonsterAi);
							await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove), nextState);
							((ThievingHopper)base.Owner.Monster).IsHovering = false;
							SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
							base.Flash();
							await Cmd.Wait(0.25f, false);
						}
					}
				}
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x0024E804 File Offset: 0x0024CA04
		private Task StunnedMove(IReadOnlyList<Creature> targets)
		{
			return Task.CompletedTask;
		}

		// Token: 0x040024F8 RID: 9464
		private const string _damageDecreaseKey = "DamageDecrease";
	}
}
