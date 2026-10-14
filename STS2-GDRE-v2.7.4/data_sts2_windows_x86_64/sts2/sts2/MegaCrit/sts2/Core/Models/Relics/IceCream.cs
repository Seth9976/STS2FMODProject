using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000565 RID: 1381
	public sealed class IceCream : RelicModel
	{
		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x00241143 File Offset: 0x0023F343
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x00241146 File Offset: 0x0023F346
		[NullableContext(1)]
		public override bool ShouldPlayerResetEnergy(Player player)
		{
			return player != base.Owner || base.Owner.PlayerCombatState.TurnNumber == 1;
		}
	}
}
