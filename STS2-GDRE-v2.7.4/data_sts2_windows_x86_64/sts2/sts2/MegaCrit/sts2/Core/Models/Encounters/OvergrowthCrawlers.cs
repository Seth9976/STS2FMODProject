using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089E RID: 2206
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OvergrowthCrawlers : EncounterModel
	{
		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x06006B17 RID: 27415 RVA: 0x0028022C File Offset: 0x0027E42C
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x06006B18 RID: 27416 RVA: 0x0028022F File Offset: 0x0027E42F
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterTag>(new EncounterTag[]
				{
					EncounterTag.Shrinker,
					EncounterTag.Crawler
				});
			}
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x06006B19 RID: 27417 RVA: 0x00280244 File Offset: 0x0027E444
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<ShrinkerBeetle>(),
					ModelDb.Monster<FuzzyWurmCrawler>()
				});
			}
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x00280261 File Offset: 0x0027E461
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<ShrinkerBeetle>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<FuzzyWurmCrawler>().ToMutable(), null)
			});
		}
	}
}
