using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B4 RID: 2228
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SnappingJaxfruitNormal : EncounterModel
	{
		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x00281060 File Offset: 0x0027F260
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x06006B9B RID: 27547 RVA: 0x00281063 File Offset: 0x0027F263
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterTag>(new EncounterTag[]
				{
					EncounterTag.Mushroom,
					EncounterTag.Jaxfruit
				});
			}
		}

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x06006B9C RID: 27548 RVA: 0x0028107A File Offset: 0x0027F27A
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<SnappingJaxfruit>(),
					ModelDb.Monster<Flyconid>()
				});
			}
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x00281097 File Offset: 0x0027F297
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<SnappingJaxfruit>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Flyconid>().ToMutable(), null)
			});
		}
	}
}
