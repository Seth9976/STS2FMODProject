using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CC6 RID: 3270
	[RequiredMember]
	public struct ResourceInfo
	{
		/// <summary>
		/// The amount of energy that was spent when playing this card.
		/// </summary>
		// Token: 0x17002450 RID: 9296
		// (get) Token: 0x060084E9 RID: 34025 RVA: 0x002CB389 File Offset: 0x002C9589
		// (set) Token: 0x060084EA RID: 34026 RVA: 0x002CB391 File Offset: 0x002C9591
		[RequiredMember]
		public int EnergySpent { readonly get; set; }

		/// <summary>
		/// The amount of energy that this card cost when it was played.
		/// Note that this is not necessarily the same as <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo.EnergySpent" />.
		/// For example, if you auto-play a 3-energy-cost card, this will be 3, while <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo.EnergySpent" /> will be 0.
		/// </summary>
		// Token: 0x17002451 RID: 9297
		// (get) Token: 0x060084EB RID: 34027 RVA: 0x002CB39A File Offset: 0x002C959A
		// (set) Token: 0x060084EC RID: 34028 RVA: 0x002CB3A2 File Offset: 0x002C95A2
		[RequiredMember]
		public int EnergyValue { readonly get; set; }

		/// <summary>
		/// The number of stars that were spent when playing this card.
		/// </summary>
		// Token: 0x17002452 RID: 9298
		// (get) Token: 0x060084ED RID: 34029 RVA: 0x002CB3AB File Offset: 0x002C95AB
		// (set) Token: 0x060084EE RID: 34030 RVA: 0x002CB3B3 File Offset: 0x002C95B3
		[RequiredMember]
		public int StarsSpent { readonly get; set; }

		/// <summary>
		/// The number of stars that this card cost when it was played.
		/// Note that this is not necessarily the same as <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo.StarsSpent" />.
		/// For example, if you auto-play a 3-star-cost card, this will be 3, while <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo.StarsSpent" /> will be 0.
		/// </summary>
		// Token: 0x17002453 RID: 9299
		// (get) Token: 0x060084EF RID: 34031 RVA: 0x002CB3BC File Offset: 0x002C95BC
		// (set) Token: 0x060084F0 RID: 34032 RVA: 0x002CB3C4 File Offset: 0x002C95C4
		[RequiredMember]
		public int StarValue { readonly get; set; }
	}
}
