using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000691 RID: 1681
	[NullableContext(1)]
	[Nullable(0)]
	public class HelicalDartPower : TemporaryDexterityPower
	{
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x060058DA RID: 22746 RVA: 0x0024F5EC File Offset: 0x0024D7EC
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Relic<HelicalDart>();
			}
		}
	}
}
