using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200071C RID: 1820
	public sealed class TheSealedThronePower : PowerModel
	{
		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06005C39 RID: 23609 RVA: 0x00255ECA File Offset: 0x002540CA
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x06005C3A RID: 23610 RVA: 0x00255ECD File Offset: 0x002540CD
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x00255ED0 File Offset: 0x002540D0
		[NullableContext(1)]
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				base.Flash();
				await PlayerCmd.GainStars(base.Amount, base.Owner.Player);
			}
		}
	}
}
