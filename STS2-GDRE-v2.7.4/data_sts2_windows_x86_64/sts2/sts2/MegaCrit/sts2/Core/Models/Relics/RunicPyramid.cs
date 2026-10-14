using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D6 RID: 1494
	public sealed class RunicPyramid : RelicModel
	{
		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06005491 RID: 21649 RVA: 0x00246CAB File Offset: 0x00244EAB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x00246CAE File Offset: 0x00244EAE
		[NullableContext(1)]
		public override bool ShouldFlush(Player player)
		{
			return player != base.Owner;
		}
	}
}
