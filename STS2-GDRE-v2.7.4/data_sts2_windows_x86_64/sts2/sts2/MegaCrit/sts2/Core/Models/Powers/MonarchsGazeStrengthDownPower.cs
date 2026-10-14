using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B1 RID: 1713
	[NullableContext(1)]
	[Nullable(0)]
	public class MonarchsGazeStrengthDownPower : TemporaryStrengthPower
	{
		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06005991 RID: 22929 RVA: 0x00250AD9 File Offset: 0x0024ECD9
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<MonarchsGaze>();
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06005992 RID: 22930 RVA: 0x00250AE0 File Offset: 0x0024ECE0
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
