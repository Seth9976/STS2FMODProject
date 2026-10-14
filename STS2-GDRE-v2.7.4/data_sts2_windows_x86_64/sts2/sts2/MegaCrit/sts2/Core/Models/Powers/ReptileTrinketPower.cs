using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006DB RID: 1755
	[NullableContext(1)]
	[Nullable(0)]
	public class ReptileTrinketPower : TemporaryStrengthPower
	{
		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06005A99 RID: 23193 RVA: 0x00252A9F File Offset: 0x00250C9F
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Relic<ReptileTrinket>();
			}
		}
	}
}
