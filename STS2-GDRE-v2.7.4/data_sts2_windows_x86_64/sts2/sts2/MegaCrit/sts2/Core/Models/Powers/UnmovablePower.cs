using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000726 RID: 1830
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnmovablePower : PowerModel
	{
		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x06005C6E RID: 23662 RVA: 0x00256443 File Offset: 0x00254643
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x06005C6F RID: 23663 RVA: 0x00256446 File Offset: 0x00254646
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x06005C70 RID: 23664 RVA: 0x00256449 File Offset: 0x00254649
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x0025645C File Offset: 0x0025465C
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (target.IsMonster)
			{
				return 1m;
			}
			if (!props.IsCardOrMonsterMove())
			{
				return 1m;
			}
			if (cardSource != null && cardSource.Owner.Creature != base.Owner)
			{
				return 1m;
			}
			int num = CombatManager.Instance.History.Entries.OfType<BlockGainedEntry>().Count<BlockGainedEntry>((BlockGainedEntry e) => e.HappenedThisTurn(this.CombatState) && e.CardPlay != null && e.CardPlay.Player.Creature == this.Owner && e.Props.IsCardOrMonsterMove() && e.CardPlay != cardPlay);
			if (num >= base.Amount)
			{
				return 1m;
			}
			return 2m;
		}
	}
}
