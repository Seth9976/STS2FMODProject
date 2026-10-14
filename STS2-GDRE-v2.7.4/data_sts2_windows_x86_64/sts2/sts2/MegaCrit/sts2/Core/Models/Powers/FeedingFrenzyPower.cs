using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000673 RID: 1651
	[NullableContext(1)]
	[Nullable(0)]
	public class FeedingFrenzyPower : TemporaryStrengthPower
	{
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06005835 RID: 22581 RVA: 0x0024E367 File Offset: 0x0024C567
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<FeedingFrenzy>();
			}
		}
	}
}
