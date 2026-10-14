using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200060F RID: 1551
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VeryHotCocoa : RelicModel
	{
		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06005612 RID: 22034 RVA: 0x00249CB7 File Offset: 0x00247EB7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x06005613 RID: 22035 RVA: 0x00249CBA File Offset: 0x00247EBA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(4));
			}
		}

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06005614 RID: 22036 RVA: 0x00249CC7 File Offset: 0x00247EC7
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x00249CD4 File Offset: 0x00247ED4
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
			}
		}
	}
}
