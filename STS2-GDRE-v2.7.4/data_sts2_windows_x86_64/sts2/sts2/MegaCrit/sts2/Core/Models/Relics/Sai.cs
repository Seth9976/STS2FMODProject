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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D7 RID: 1495
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sai : RelicModel
	{
		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x00246CC4 File Offset: 0x00244EC4
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06005495 RID: 21653 RVA: 0x00246CC7 File Offset: 0x00244EC7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x00246CDC File Offset: 0x00244EDC
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
			}
		}
	}
}
