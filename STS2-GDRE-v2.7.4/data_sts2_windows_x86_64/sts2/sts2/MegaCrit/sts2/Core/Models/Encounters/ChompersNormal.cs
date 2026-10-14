using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000878 RID: 2168
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChompersNormal : EncounterModel
	{
		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x06006A2F RID: 27183 RVA: 0x0027EFAD File Offset: 0x0027D1AD
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Chomper);
			}
		}

		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x0027EFB5 File Offset: 0x0027D1B5
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x06006A31 RID: 27185 RVA: 0x0027EFB8 File Offset: 0x0027D1B8
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Chomper>());
			}
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x0027EFC4 File Offset: 0x0027D1C4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Chomper chomper = (Chomper)ModelDb.Monster<Chomper>().ToMutable();
			Chomper chomper2 = (Chomper)ModelDb.Monster<Chomper>().ToMutable();
			chomper2.ScreamFirst = true;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(chomper, null),
				new ValueTuple<MonsterModel, string>(chomper2, null)
			});
		}
	}
}
