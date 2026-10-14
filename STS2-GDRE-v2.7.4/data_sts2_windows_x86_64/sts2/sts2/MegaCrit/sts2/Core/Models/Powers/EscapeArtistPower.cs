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

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// Just a visual timer for when <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper" /> will escape.
	/// </summary>
	// Token: 0x0200066F RID: 1647
	public sealed class EscapeArtistPower : PowerModel
	{
		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06005826 RID: 22566 RVA: 0x0024E275 File Offset: 0x0024C475
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06005827 RID: 22567 RVA: 0x0024E278 File Offset: 0x0024C478
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005828 RID: 22568 RVA: 0x0024E27C File Offset: 0x0024C47C
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Amount > 1)
				{
					await PowerCmd.Decrement(this);
				}
				if (base.Amount == 1)
				{
					base.StartPulsing();
				}
			}
		}
	}
}
