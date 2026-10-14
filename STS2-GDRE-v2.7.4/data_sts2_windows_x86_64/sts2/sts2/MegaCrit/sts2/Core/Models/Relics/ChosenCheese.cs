using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000520 RID: 1312
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChosenCheese : RelicModel
	{
		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x0023E4CF File Offset: 0x0023C6CF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x0023E4D2 File Offset: 0x0023C6D2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(1m));
			}
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x0023E4E4 File Offset: 0x0023C6E4
		public override async Task AfterCombatEnd(CombatRoom _)
		{
			base.Flash();
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
