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
	// Token: 0x020006BE RID: 1726
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OneTwoPunchPower : PowerModel
	{
		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x060059DC RID: 23004 RVA: 0x00251395 File Offset: 0x0024F595
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x060059DD RID: 23005 RVA: 0x00251398 File Offset: 0x0024F598
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x0025139B File Offset: 0x0024F59B
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return playCount;
			}
			if (card.Type != CardType.Attack)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x002513C0 File Offset: 0x0024F5C0
		public override async Task AfterModifyingCardPlayCount(CardModel card)
		{
			await PowerCmd.Decrement(this);
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x00251404 File Offset: 0x0024F604
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
