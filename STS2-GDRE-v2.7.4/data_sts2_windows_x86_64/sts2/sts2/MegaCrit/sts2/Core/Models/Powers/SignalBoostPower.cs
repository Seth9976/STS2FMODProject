using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006EF RID: 1775
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SignalBoostPower : PowerModel
	{
		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06005B12 RID: 23314 RVA: 0x002539C7 File Offset: 0x00251BC7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x06005B13 RID: 23315 RVA: 0x002539CA File Offset: 0x00251BCA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x002539CD File Offset: 0x00251BCD
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return playCount;
			}
			if (card.Type != CardType.Power)
			{
				return playCount;
			}
			return playCount + 1;
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x002539F4 File Offset: 0x00251BF4
		public override async Task AfterModifyingCardPlayCount(CardModel card)
		{
			await PowerCmd.Decrement(this);
		}
	}
}
