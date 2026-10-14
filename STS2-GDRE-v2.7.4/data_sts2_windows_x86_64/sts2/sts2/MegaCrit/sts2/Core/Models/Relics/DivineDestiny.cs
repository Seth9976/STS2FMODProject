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
	// Token: 0x02000530 RID: 1328
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DivineDestiny : RelicModel
	{
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005036 RID: 20534 RVA: 0x0023EBEF File Offset: 0x0023CDEF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x0023EBF2 File Offset: 0x0023CDF2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(7));
			}
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0023EC00 File Offset: 0x0023CE00
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
				}
			}
		}
	}
}
