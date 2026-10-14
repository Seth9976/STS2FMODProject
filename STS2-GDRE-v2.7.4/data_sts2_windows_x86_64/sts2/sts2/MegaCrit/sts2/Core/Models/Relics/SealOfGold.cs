using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DC RID: 1500
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SealOfGold : RelicModel
	{
		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060054B2 RID: 21682 RVA: 0x00247313 File Offset: 0x00245513
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x00247316 File Offset: 0x00245516
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new GoldVar(5)
				});
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060054B4 RID: 21684 RVA: 0x00247335 File Offset: 0x00245535
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x00247344 File Offset: 0x00245544
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.Gold >= base.DynamicVars.Gold.IntValue)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					await PlayerCmd.LoseGold(base.DynamicVars.Gold.IntValue, base.Owner, GoldLossType.Lost);
				}
			}
		}
	}
}
