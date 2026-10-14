using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F5 RID: 1781
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlumberPower : PowerModel
	{
		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x00253E38 File Offset: 0x00252038
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06005B40 RID: 23360 RVA: 0x00253E3B File Offset: 0x0025203B
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x00253E40 File Offset: 0x00252040
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage != 0)
				{
					await PowerCmd.Decrement(this);
					if (base.Amount <= 0)
					{
						SlumberingBeetle slumberingBeetle = (SlumberingBeetle)base.Owner.Monster;
						await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(slumberingBeetle.WakeUpMove), "ROLL_OUT_MOVE");
					}
				}
			}
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x00253E93 File Offset: 0x00252093
		public override Task AfterRemoved(Creature oldOwner)
		{
			SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
			return Task.CompletedTask;
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x00253EA4 File Offset: 0x002520A4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
				if (base.Amount <= 0)
				{
					await ((SlumberingBeetle)base.Owner.Monster).WakeUpMove(Array.Empty<Creature>());
				}
			}
		}
	}
}
