using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064E RID: 1614
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorruptionPower : PowerModel
	{
		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x0600576A RID: 22378 RVA: 0x0024CD57 File Offset: 0x0024AF57
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x0600576B RID: 22379 RVA: 0x0024CD5A File Offset: 0x0024AF5A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600576C RID: 22380 RVA: 0x0024CD5D File Offset: 0x0024AF5D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x0024CD6A File Offset: 0x0024AF6A
		public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			if (card.Owner.Creature != base.Owner || card.Type != CardType.Skill)
			{
				modifiedCost = originalCost;
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x0024CD99 File Offset: 0x0024AF99
		public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation location)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return location;
			}
			if (card.Type != CardType.Skill)
			{
				return location;
			}
			location.pileType = PileType.Exhaust;
			return location;
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x0024CDC7 File Offset: 0x0024AFC7
		public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
