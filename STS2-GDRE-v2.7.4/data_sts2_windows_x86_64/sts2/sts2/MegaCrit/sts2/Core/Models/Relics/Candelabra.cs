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
	// Token: 0x02000518 RID: 1304
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Candelabra : RelicModel
	{
		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06004FC4 RID: 20420 RVA: 0x0023E01E File Offset: 0x0023C21E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x0023E021 File Offset: 0x0023C221
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x0023E02E File Offset: 0x0023C22E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x0023E03C File Offset: 0x0023C23C
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 2)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
			}
		}

		// Token: 0x04002434 RID: 9268
		private const int _energyTurn = 2;
	}
}
