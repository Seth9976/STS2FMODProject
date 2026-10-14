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
	// Token: 0x0200051C RID: 1308
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Chandelier : RelicModel
	{
		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x0023E2CD File Offset: 0x0023C4CD
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06004FDE RID: 20446 RVA: 0x0023E2D0 File Offset: 0x0023C4D0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(3));
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06004FDF RID: 20447 RVA: 0x0023E2DD File Offset: 0x0023C4DD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x0023E2EC File Offset: 0x0023C4EC
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 3)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x04002438 RID: 9272
		private const int _energyTurn = 3;
	}
}
