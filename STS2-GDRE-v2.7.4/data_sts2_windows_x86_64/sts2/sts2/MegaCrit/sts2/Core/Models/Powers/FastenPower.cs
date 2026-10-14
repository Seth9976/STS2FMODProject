using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000672 RID: 1650
	[NullableContext(1)]
	[Nullable(0)]
	public class FastenPower : PowerModel
	{
		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x0600582F RID: 22575 RVA: 0x0024E2EC File Offset: 0x0024C4EC
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06005830 RID: 22576 RVA: 0x0024E2EF File Offset: 0x0024C4EF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06005831 RID: 22577 RVA: 0x0024E2F2 File Offset: 0x0024C4F2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x0024E304 File Offset: 0x0024C504
		[NullableContext(2)]
		public override decimal ModifyBlockAdditive([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != target)
			{
				return 0m;
			}
			if (!props.IsPoweredCardOrMonsterMoveBlock())
			{
				return 0m;
			}
			if (cardSource != null && !cardSource.Tags.Contains(CardTag.Defend))
			{
				return 0m;
			}
			return base.Amount;
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x0024E352 File Offset: 0x0024C552
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterModifyingBlockAmount(decimal modifiedBlock, CardModel cardSource, CardPlay cardPlay)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
