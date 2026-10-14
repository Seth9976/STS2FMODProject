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
	// Token: 0x02000638 RID: 1592
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlockNextTurnPower : PowerModel
	{
		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x060056ED RID: 22253 RVA: 0x0024BF63 File Offset: 0x0024A163
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x0024BF66 File Offset: 0x0024A166
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x0024BF69 File Offset: 0x0024A169
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x0024BF7C File Offset: 0x0024A17C
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				await PowerCmd.Remove(this);
			}
		}
	}
}
