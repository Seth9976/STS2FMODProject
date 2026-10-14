using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A2 RID: 1442
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsBlood : RelicModel
	{
		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x0600531A RID: 21274 RVA: 0x0024416B File Offset: 0x0024236B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x0600531B RID: 21275 RVA: 0x0024416E File Offset: 0x0024236E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x0024417B File Offset: 0x0024237B
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.IntValue;
		}
	}
}
