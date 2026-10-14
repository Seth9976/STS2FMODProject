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
	// Token: 0x020005AD RID: 1453
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pantograph : RelicModel
	{
		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x00244EFB File Offset: 0x002430FB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x00244EFE File Offset: 0x002430FE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(25m));
			}
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x00244F14 File Offset: 0x00243114
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (base.Owner.Creature.IsDead)
			{
				return Task.CompletedTask;
			}
			base.Status = (base.Owner.RunState.Map.BossMapPoint.parents.Contains(base.Owner.RunState.CurrentMapPoint) ? RelicStatus.Active : RelicStatus.Normal);
			return Task.CompletedTask;
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x00244F7C File Offset: 0x0024317C
		public override async Task BeforeCombatStart()
		{
			if (!base.Owner.Creature.IsDead)
			{
				if (base.Owner.RunState.CurrentRoom.RoomType == RoomType.Boss)
				{
					base.Flash();
					await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
				}
			}
		}
	}
}
