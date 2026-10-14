using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200071D RID: 1821
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThieveryPower : PowerModel
	{
		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06005C3D RID: 23613 RVA: 0x00255F23 File Offset: 0x00254123
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x00255F26 File Offset: 0x00254126
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x00255F29 File Offset: 0x00254129
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x00255F2C File Offset: 0x0025412C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(0));
			}
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x00255F3C File Offset: 0x0025413C
		public async Task Steal()
		{
			if (base.Target != null)
			{
				if (!base.Target.IsDead)
				{
					if (base.Target.Player.Gold > 0)
					{
						int amount = Math.Min(base.Amount, base.Target.Player.Gold);
						await PlayerCmd.LoseGold(amount, base.Target.Player, GoldLossType.Stolen);
						base.DynamicVars.Gold.BaseValue += amount;
					}
				}
			}
		}
	}
}
