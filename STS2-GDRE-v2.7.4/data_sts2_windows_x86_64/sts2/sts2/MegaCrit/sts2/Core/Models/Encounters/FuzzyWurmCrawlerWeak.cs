using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088B RID: 2187
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FuzzyWurmCrawlerWeak : EncounterModel
	{
		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x06006A9C RID: 27292 RVA: 0x0027FA55 File Offset: 0x0027DC55
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Crawler);
			}
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x06006A9D RID: 27293 RVA: 0x0027FA5D File Offset: 0x0027DC5D
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x06006A9E RID: 27294 RVA: 0x0027FA60 File Offset: 0x0027DC60
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x06006A9F RID: 27295 RVA: 0x0027FA63 File Offset: 0x0027DC63
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<FuzzyWurmCrawler>());
			}
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x0027FA6F File Offset: 0x0027DC6F
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<FuzzyWurmCrawler>().ToMutable(), null));
		}
	}
}
