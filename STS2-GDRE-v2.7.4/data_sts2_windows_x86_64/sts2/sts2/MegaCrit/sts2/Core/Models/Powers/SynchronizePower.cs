using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070F RID: 1807
	[NullableContext(1)]
	[Nullable(0)]
	public class SynchronizePower : TemporaryFocusPower
	{
		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x002550A6 File Offset: 0x002532A6
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Synchronize>();
			}
		}
	}
}
