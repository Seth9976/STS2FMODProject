using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F4 RID: 1268
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Anchor : RelicModel
	{
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x0023C0B9 File Offset: 0x0023A2B9
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06004ECB RID: 20171 RVA: 0x0023C0BC File Offset: 0x0023A2BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x0023C0D0 File Offset: 0x0023A2D0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x0023C0E4 File Offset: 0x0023A2E4
		public override async Task BeforeCombatStart()
		{
			base.Flash();
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
		}
	}
}
