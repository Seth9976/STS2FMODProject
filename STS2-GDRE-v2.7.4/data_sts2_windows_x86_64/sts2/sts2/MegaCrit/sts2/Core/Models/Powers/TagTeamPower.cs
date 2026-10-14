using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000710 RID: 1808
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagTeamPower : PowerModel
	{
		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x002550B5 File Offset: 0x002532B5
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x002550B8 File Offset: 0x002532B8
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x002550BB File Offset: 0x002532BB
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06005BDA RID: 23514 RVA: 0x002550BE File Offset: 0x002532BE
		public override int DisplayAmount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x002550C4 File Offset: 0x002532C4
		public override int ModifyCardPlayCount(CardModel card, [Nullable(2)] Creature target, int playCount)
		{
			if (card.Type != CardType.Attack)
			{
				return playCount;
			}
			if (card.Owner.Creature == base.Applier)
			{
				return playCount;
			}
			if (card.TargetType == TargetType.AnyEnemy && target != base.Owner)
			{
				return playCount;
			}
			TargetType targetType = card.TargetType;
			if (targetType - TargetType.AnyEnemy > 1)
			{
				return playCount;
			}
			return playCount + base.Amount;
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x00255128 File Offset: 0x00253328
		public override async Task AfterModifyingCardPlayCount(CardModel card)
		{
			await PowerCmd.Remove(this);
		}
	}
}
