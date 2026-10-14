using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B9 RID: 1465
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Planisphere : RelicModel
	{
		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x060053D8 RID: 21464 RVA: 0x0024587F File Offset: 0x00243A7F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x00245882 File Offset: 0x00243A82
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x0024588A File Offset: 0x00243A8A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(5m));
			}
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x0024589C File Offset: 0x00243A9C
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (!base.Owner.Creature.IsDead)
			{
				MapPoint currentMapPoint = base.Owner.RunState.CurrentMapPoint;
				if (currentMapPoint != null && currentMapPoint.PointType == MapPointType.Unknown)
				{
					if (base.Owner.RunState.CurrentRoomCount <= 1)
					{
						base.Flash();
						await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
					}
				}
			}
		}
	}
}
