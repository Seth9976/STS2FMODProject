using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E7 RID: 1511
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SneckoSkull : RelicModel
	{
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06005504 RID: 21764 RVA: 0x00247D13 File Offset: 0x00245F13
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06005505 RID: 21765 RVA: 0x00247D18 File Offset: 0x00245F18
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06005506 RID: 21766 RVA: 0x00247D38 File Offset: 0x00245F38
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(1m));
			}
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x00247D49 File Offset: 0x00245F49
		public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, [Nullable(2)] Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (!(power is PoisonPower))
			{
				return 0m;
			}
			if (giver != base.Owner.Creature)
			{
				return 0m;
			}
			return base.DynamicVars.Poison.BaseValue;
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x00247D7D File Offset: 0x00245F7D
		public override Task AfterModifyingPowerAmountGiven(PowerModel power)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
