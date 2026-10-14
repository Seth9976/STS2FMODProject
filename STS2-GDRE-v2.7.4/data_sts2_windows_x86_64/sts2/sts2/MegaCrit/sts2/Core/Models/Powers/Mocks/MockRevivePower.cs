using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x0200073E RID: 1854
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockRevivePower : PowerModel
	{
		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x06005D0F RID: 23823 RVA: 0x002574FC File Offset: 0x002556FC
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x06005D10 RID: 23824 RVA: 0x002574FF File Offset: 0x002556FF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x06005D11 RID: 23825 RVA: 0x00257502 File Offset: 0x00255702
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x00257505 File Offset: 0x00255705
		protected override object InitInternalData()
		{
			return new MockRevivePower.Data();
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x06005D13 RID: 23827 RVA: 0x0025750C File Offset: 0x0025570C
		private bool IsReviving
		{
			get
			{
				return base.GetInternalData<MockRevivePower.Data>().isReviving;
			}
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x00257519 File Offset: 0x00255719
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!this.IsReviving)
			{
				return 1m;
			}
			return 0m;
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x0025753D File Offset: 0x0025573D
		public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, [Nullable(2)] Creature applier, out decimal modifiedAmount)
		{
			modifiedAmount = amount;
			return target == base.Owner && this.IsReviving;
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x0025755D File Offset: 0x0025575D
		public override bool ShouldAllowHitting(Creature creature)
		{
			return !this.IsReviving;
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x00257568 File Offset: 0x00255768
		public override bool ShouldDie(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x00257576 File Offset: 0x00255776
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x0025757C File Offset: 0x0025577C
		public override async Task AfterPreventingDeath(Creature creature)
		{
			if (creature == base.Owner)
			{
				base.GetInternalData<MockRevivePower.Data>().isReviving = true;
				await CreatureCmd.Heal(base.Owner, 1m, false);
			}
		}

		// Token: 0x02001CC3 RID: 7363
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x040073E1 RID: 29665
			public bool isReviving;
		}
	}
}
