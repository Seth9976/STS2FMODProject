using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005CF RID: 1487
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RingOfTheDrake : RelicModel
	{
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x0600546A RID: 21610 RVA: 0x0024685F File Offset: 0x00244A5F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x0600546B RID: 21611 RVA: 0x00246862 File Offset: 0x00244A62
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new DynamicVar("Turns", 3m)
				});
			}
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x0024688C File Offset: 0x00244A8C
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			if (base.Owner.PlayerCombatState.TurnNumber > base.DynamicVars["Turns"].BaseValue)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.BaseValue;
		}

		// Token: 0x040024B3 RID: 9395
		private const string _turnsKey = "Turns";
	}
}
