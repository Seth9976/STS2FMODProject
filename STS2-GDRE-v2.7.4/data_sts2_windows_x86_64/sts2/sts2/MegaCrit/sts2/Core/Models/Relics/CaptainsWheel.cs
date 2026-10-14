using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000519 RID: 1305
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CaptainsWheel : RelicModel
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x0023E08F File Offset: 0x0023C28F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06004FCA RID: 20426 RVA: 0x0023E092 File Offset: 0x0023C292
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(18m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x0023E0A6 File Offset: 0x0023C2A6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x0023E0B8 File Offset: 0x0023C2B8
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner.Creature)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 3)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
				}
			}
		}
	}
}
