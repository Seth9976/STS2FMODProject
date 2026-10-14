using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063D RID: 1597
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BurstPower : PowerModel
	{
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x0600570B RID: 22283 RVA: 0x0024C22B File Offset: 0x0024A42B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x0600570C RID: 22284 RVA: 0x0024C22E File Offset: 0x0024A42E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x0024C231 File Offset: 0x0024A431
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return playCount;
			}
			if (card.Type != CardType.Skill)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x0024C258 File Offset: 0x0024A458
		public override async Task AfterModifyingCardPlayCount(CardModel card)
		{
			await PowerCmd.Decrement(this);
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x0024C29C File Offset: 0x0024A49C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
