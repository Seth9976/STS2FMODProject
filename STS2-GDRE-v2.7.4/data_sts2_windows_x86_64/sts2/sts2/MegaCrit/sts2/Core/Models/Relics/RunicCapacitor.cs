using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D5 RID: 1493
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RunicCapacitor : RelicModel
	{
		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x0600548D RID: 21645 RVA: 0x00246C48 File Offset: 0x00244E48
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x0600548E RID: 21646 RVA: 0x00246C4B File Offset: 0x00244E4B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(3));
			}
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00246C58 File Offset: 0x00244E58
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await OrbCmd.AddSlots(base.Owner, base.DynamicVars.Repeat.IntValue);
				}
			}
		}
	}
}
