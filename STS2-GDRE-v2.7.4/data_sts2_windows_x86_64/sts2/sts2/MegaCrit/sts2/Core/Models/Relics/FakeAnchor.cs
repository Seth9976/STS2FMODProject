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
	// Token: 0x0200053E RID: 1342
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeAnchor : RelicModel
	{
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06005082 RID: 20610 RVA: 0x0023F43F File Offset: 0x0023D63F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06005083 RID: 20611 RVA: 0x0023F442 File Offset: 0x0023D642
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005084 RID: 20612 RVA: 0x0023F446 File Offset: 0x0023D646
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(4m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06005085 RID: 20613 RVA: 0x0023F459 File Offset: 0x0023D659
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0023F46C File Offset: 0x0023D66C
		public override async Task BeforeCombatStart()
		{
			base.Flash();
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
		}
	}
}
