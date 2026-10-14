using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000674 RID: 1652
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FeelNoPainPower : PowerModel
	{
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06005837 RID: 22583 RVA: 0x0024E376 File Offset: 0x0024C576
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06005838 RID: 22584 RVA: 0x0024E379 File Offset: 0x0024C579
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06005839 RID: 22585 RVA: 0x0024E37C File Offset: 0x0024C57C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x0024E390 File Offset: 0x0024C590
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
		{
			if (card.Owner.Creature == base.Owner)
			{
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
			}
		}
	}
}
