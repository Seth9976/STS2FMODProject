using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E4 RID: 1764
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SelfFormingClayPower : PowerModel
	{
		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x002532F1 File Offset: 0x002514F1
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x002532F4 File Offset: 0x002514F4
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x002532F7 File Offset: 0x002514F7
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x0025330C File Offset: 0x0025150C
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner)
			{
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				await PowerCmd.Remove(this);
			}
		}
	}
}
