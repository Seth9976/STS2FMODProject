using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000669 RID: 1641
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EchoFormPower : PowerModel
	{
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x0600580A RID: 22538 RVA: 0x0024DFF1 File Offset: 0x0024C1F1
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x0600580B RID: 22539 RVA: 0x0024DFF4 File Offset: 0x0024C1F4
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x0024DFF8 File Offset: 0x0024C1F8
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return playCount;
			}
			int num = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.Actor == base.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(base.CombatState));
			if (num >= base.Amount)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x0024E049 File Offset: 0x0024C249
		public override Task AfterModifyingCardPlayCount(CardModel card)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
