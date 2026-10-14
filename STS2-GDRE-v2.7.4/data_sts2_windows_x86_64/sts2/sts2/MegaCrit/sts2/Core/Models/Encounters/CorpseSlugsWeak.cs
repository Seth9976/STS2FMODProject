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
	// Token: 0x0200087B RID: 2171
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorpseSlugsWeak : EncounterModel
	{
		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06006A3D RID: 27197 RVA: 0x0027F186 File Offset: 0x0027D386
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slugs);
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x06006A3E RID: 27198 RVA: 0x0027F18F File Offset: 0x0027D38F
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x06006A3F RID: 27199 RVA: 0x0027F192 File Offset: 0x0027D392
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x06006A40 RID: 27200 RVA: 0x0027F195 File Offset: 0x0027D395
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<CorpseSlug>());
			}
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x0027F1A4 File Offset: 0x0027D3A4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected unsafe override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			int num = 2;
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>(num);
			CollectionsMarshal.SetCount<ValueTuple<MonsterModel, string>>(list, num);
			Span<ValueTuple<MonsterModel, string>> span = CollectionsMarshal.AsSpan<ValueTuple<MonsterModel, string>>(list);
			int num2 = 0;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<CorpseSlug>().ToMutable(), null);
			num2++;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<CorpseSlug>().ToMutable(), null);
			List<ValueTuple<MonsterModel, string>> list2 = list;
			CorpseSlug.EnsureCorpseSlugsStartWithDifferentMoves(list2.Select<ValueTuple<MonsterModel, string>, MonsterModel>((ValueTuple<MonsterModel, string> kvp) => kvp.Item1), base.Rng);
			return list2;
		}
	}
}
