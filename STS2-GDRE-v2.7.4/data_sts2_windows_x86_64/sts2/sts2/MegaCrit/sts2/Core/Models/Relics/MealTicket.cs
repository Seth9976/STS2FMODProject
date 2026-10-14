using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000585 RID: 1413
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MealTicket : RelicModel
	{
		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x00242C5E File Offset: 0x00240E5E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x00242C61 File Offset: 0x00240E61
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06005269 RID: 21097 RVA: 0x00242C69 File Offset: 0x00240E69
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(15m));
			}
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x00242C7C File Offset: 0x00240E7C
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (!base.Owner.Creature.IsDead)
			{
				if (room is MerchantRoom)
				{
					base.Flash();
					await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
				}
			}
		}
	}
}
