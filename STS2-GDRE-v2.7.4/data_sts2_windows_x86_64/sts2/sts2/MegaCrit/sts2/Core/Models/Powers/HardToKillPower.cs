using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200068D RID: 1677
	public sealed class HardToKillPower : PowerModel
	{
		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x060058C8 RID: 22728 RVA: 0x0024F44A File Offset: 0x0024D64A
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x060058C9 RID: 22729 RVA: 0x0024F44D File Offset: 0x0024D64D
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x0024F450 File Offset: 0x0024D650
		[NullableContext(2)]
		public override decimal ModifyDamageCap(Creature target, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return decimal.MaxValue;
			}
			return base.Amount;
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x0024F471 File Offset: 0x0024D671
		[NullableContext(1)]
		public override Task AfterModifyingDamageAmount([Nullable(2)] CardModel cardSource)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
