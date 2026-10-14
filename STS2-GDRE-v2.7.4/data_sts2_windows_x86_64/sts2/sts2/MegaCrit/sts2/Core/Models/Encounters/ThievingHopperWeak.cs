using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BF RID: 2239
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThievingHopperWeak : EncounterModel
	{
		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x06006BDF RID: 27615 RVA: 0x0028146D File Offset: 0x0027F66D
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x06006BE0 RID: 27616 RVA: 0x00281470 File Offset: 0x0027F670
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Thieves);
			}
		}

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x06006BE1 RID: 27617 RVA: 0x00281478 File Offset: 0x0027F678
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x06006BE2 RID: 27618 RVA: 0x0028147B File Offset: 0x0027F67B
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<ThievingHopper>());
			}
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x00281487 File Offset: 0x0027F687
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<ThievingHopper>().ToMutable(), null));
		}
	}
}
