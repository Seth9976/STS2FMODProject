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
	// Token: 0x02000667 RID: 1639
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DuplicationPower : PowerModel
	{
		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06005801 RID: 22529 RVA: 0x0024DF27 File Offset: 0x0024C127
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06005802 RID: 22530 RVA: 0x0024DF2A File Offset: 0x0024C12A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x0024DF2D File Offset: 0x0024C12D
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x0024DF48 File Offset: 0x0024C148
		public override async Task AfterModifyingCardPlayCount(CardModel card)
		{
			await PowerCmd.Decrement(this);
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x0024DF8C File Offset: 0x0024C18C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
