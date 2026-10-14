using System;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000521 RID: 1313
	public sealed class Circlet : RelicModel
	{
		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06004FF4 RID: 20468 RVA: 0x0023E52F File Offset: 0x0023C72F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.None;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06004FF5 RID: 20469 RVA: 0x0023E532 File Offset: 0x0023C732
		public override bool IsStackable
		{
			get
			{
				return true;
			}
		}
	}
}
