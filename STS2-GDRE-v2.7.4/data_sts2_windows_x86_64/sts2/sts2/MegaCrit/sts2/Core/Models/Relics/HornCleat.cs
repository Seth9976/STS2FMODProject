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
	// Token: 0x02000564 RID: 1380
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HornCleat : RelicModel
	{
		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x0600517E RID: 20862 RVA: 0x002410C6 File Offset: 0x0023F2C6
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x002410C9 File Offset: 0x0023F2C9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(14m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06005180 RID: 20864 RVA: 0x002410DD File Offset: 0x0023F2DD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x002410F0 File Offset: 0x0023F2F0
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner.Creature)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 2)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
				}
			}
		}
	}
}
