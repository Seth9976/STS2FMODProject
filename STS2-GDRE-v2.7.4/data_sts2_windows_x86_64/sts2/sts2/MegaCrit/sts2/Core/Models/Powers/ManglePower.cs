using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006AB RID: 1707
	[NullableContext(1)]
	[Nullable(0)]
	public class ManglePower : TemporaryStrengthPower
	{
		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06005976 RID: 22902 RVA: 0x0025091F File Offset: 0x0024EB1F
		public override AbstractModel OriginModel
		{
			get
			{
				return ModelDb.Card<Mangle>();
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06005977 RID: 22903 RVA: 0x00250926 File Offset: 0x0024EB26
		protected override bool IsPositive
		{
			get
			{
				return false;
			}
		}
	}
}
