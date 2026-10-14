using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006BF RID: 1727
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OrbitPower : PowerModel
	{
		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x060059E2 RID: 23010 RVA: 0x00251457 File Offset: 0x0024F657
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x0025145A File Offset: 0x0024F65A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x0025145D File Offset: 0x0024F65D
		public override int DisplayAmount
		{
			get
			{
				return 4 - base.GetInternalData<OrbitPower.Data>().energySpent % 4;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x0025146E File Offset: 0x0024F66E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x0025147B File Offset: 0x0024F67B
		protected override object InitInternalData()
		{
			return new OrbitPower.Data();
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x060059E7 RID: 23015 RVA: 0x00251482 File Offset: 0x0024F682
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x060059E8 RID: 23016 RVA: 0x00251485 File Offset: 0x0024F685
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(4));
			}
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x00251494 File Offset: 0x0024F694
		public override async Task AfterEnergySpent(CardModel card, int amount)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (amount > 0)
				{
					OrbitPower.Data data = base.GetInternalData<OrbitPower.Data>();
					data.energySpent += amount;
					int triggers = data.energySpent / 4 - data.triggerCount;
					if (triggers > 0)
					{
						base.Flash();
						await PlayerCmd.GainEnergy(base.Amount * triggers, base.Owner.Player);
						data.triggerCount += triggers;
					}
					base.InvokeDisplayAmountChanged();
				}
			}
		}

		// Token: 0x04002509 RID: 9481
		private const int _energyIncrement = 4;

		// Token: 0x02001C1A RID: 7194
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04007047 RID: 28743
			public int energySpent;

			// Token: 0x04007048 RID: 28744
			public int triggerCount;
		}
	}
}
