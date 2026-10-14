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
using MegaCrit.Sts2.Core.Models.Encounters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000634 RID: 1588
	public sealed class BattlewornDummyTimeLimitPower : PowerModel
	{
		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x060056D7 RID: 22231 RVA: 0x0024BCF5 File Offset: 0x00249EF5
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x060056D8 RID: 22232 RVA: 0x0024BCF8 File Offset: 0x00249EF8
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x0024BCFC File Offset: 0x00249EFC
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Amount > 1)
				{
					await PowerCmd.Decrement(this);
				}
				else
				{
					BattlewornDummyEventEncounter battlewornDummyEventEncounter = base.Owner.CombatState.Encounter as BattlewornDummyEventEncounter;
					if (battlewornDummyEventEncounter != null)
					{
						battlewornDummyEventEncounter.RanOutOfTime = true;
					}
					await CreatureCmd.Escape(base.Owner, true);
				}
			}
		}
	}
}
