using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000531 RID: 1329
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DivineRight : RelicModel
	{
		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x0600503A RID: 20538 RVA: 0x0023EC53 File Offset: 0x0023CE53
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x0600503B RID: 20539 RVA: 0x0023EC56 File Offset: 0x0023CE56
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(3));
			}
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x0023EC64 File Offset: 0x0023CE64
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is CombatRoom)
			{
				await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			}
		}
	}
}
