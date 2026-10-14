using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D2 RID: 1746
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RadiancePower : PowerModel
	{
		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06005A5D RID: 23133 RVA: 0x002522A5 File Offset: 0x002504A5
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x002522A8 File Offset: 0x002504A8
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06005A5F RID: 23135 RVA: 0x002522AB File Offset: 0x002504AB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06005A60 RID: 23136 RVA: 0x002522B8 File Offset: 0x002504B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x002522C8 File Offset: 0x002504C8
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner.Player)
			{
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, player);
				await PowerCmd.Decrement(this);
			}
		}
	}
}
