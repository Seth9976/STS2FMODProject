using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200087A RID: 2170
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorpseSlugsNormal : EncounterModel
	{
		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x0027F0AD File Offset: 0x0027D2AD
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slugs);
			}
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x06006A39 RID: 27193 RVA: 0x0027F0B6 File Offset: 0x0027D2B6
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x0027F0B9 File Offset: 0x0027D2B9
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<CorpseSlug>());
			}
		}

		// Token: 0x06006A3B RID: 27195 RVA: 0x0027F0C8 File Offset: 0x0027D2C8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected unsafe override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			int num = 3;
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>(num);
			CollectionsMarshal.SetCount<ValueTuple<MonsterModel, string>>(list, num);
			Span<ValueTuple<MonsterModel, string>> span = CollectionsMarshal.AsSpan<ValueTuple<MonsterModel, string>>(list);
			int num2 = 0;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<CorpseSlug>().ToMutable(), null);
			num2++;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<CorpseSlug>().ToMutable(), null);
			num2++;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<CorpseSlug>().ToMutable(), null);
			List<ValueTuple<MonsterModel, string>> list2 = list;
			CorpseSlug.EnsureCorpseSlugsStartWithDifferentMoves(list2.Select<ValueTuple<MonsterModel, string>, MonsterModel>((ValueTuple<MonsterModel, string> kvp) => kvp.Item1), base.Rng);
			return list2;
		}
	}
}
