using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FF RID: 1791
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpiritOfAshPower : PowerModel
	{
		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06005B71 RID: 23409 RVA: 0x002543AB File Offset: 0x002525AB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06005B72 RID: 23410 RVA: 0x002543AE File Offset: 0x002525AE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x06005B73 RID: 23411 RVA: 0x002543B1 File Offset: 0x002525B1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06005B74 RID: 23412 RVA: 0x002543D8 File Offset: 0x002525D8
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				if (cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal))
				{
					await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
