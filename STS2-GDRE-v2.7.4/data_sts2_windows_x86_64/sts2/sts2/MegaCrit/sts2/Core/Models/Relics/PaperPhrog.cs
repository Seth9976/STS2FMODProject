using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005AF RID: 1455
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaperPhrog : RelicModel
	{
		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x00245021 File Offset: 0x00243221
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x00245024 File Offset: 0x00243224
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x00245044 File Offset: 0x00243244
		[NullableContext(2)]
		public decimal ModifyVulnerableMultiplier([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target == base.Owner.Creature)
			{
				return amount;
			}
			if (!props.IsPoweredAttack())
			{
				return amount;
			}
			return amount + 0.25m;
		}
	}
}
