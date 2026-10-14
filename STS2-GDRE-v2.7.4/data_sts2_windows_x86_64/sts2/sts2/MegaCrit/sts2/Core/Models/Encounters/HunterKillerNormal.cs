using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088F RID: 2191
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HunterKillerNormal : EncounterModel
	{
		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x06006AB4 RID: 27316 RVA: 0x0027FBBC File Offset: 0x0027DDBC
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x0027FBBF File Offset: 0x0027DDBF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<HunterKiller>());
			}
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0027FBCB File Offset: 0x0027DDCB
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<HunterKiller>().ToMutable(), null));
		}
	}
}
