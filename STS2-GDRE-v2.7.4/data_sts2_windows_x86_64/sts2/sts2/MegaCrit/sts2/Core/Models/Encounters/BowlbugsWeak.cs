using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000874 RID: 2164
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugsWeak : EncounterModel
	{
		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x06006A14 RID: 27156 RVA: 0x0027EE36 File Offset: 0x0027D036
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x06006A15 RID: 27157 RVA: 0x0027EE39 File Offset: 0x0027D039
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Workers);
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x06006A16 RID: 27158 RVA: 0x0027EE41 File Offset: 0x0027D041
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x06006A17 RID: 27159 RVA: 0x0027EE44 File Offset: 0x0027D044
		private static MonsterModel[] Bugs
		{
			get
			{
				return new MonsterModel[]
				{
					ModelDb.Monster<BowlbugEgg>(),
					ModelDb.Monster<BowlbugNectar>()
				};
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x06006A18 RID: 27160 RVA: 0x0027EE5C File Offset: 0x0027D05C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return BowlbugsWeak.Bugs.Concat<MonsterModel>(new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BowlbugRock>()));
			}
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x0027EE74 File Offset: 0x0027D074
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<BowlbugRock>().ToMutable(), "odd"),
				new ValueTuple<MonsterModel, string>(base.Rng.NextItem<MonsterModel>(BowlbugsWeak.Bugs).ToMutable(), "even")
			});
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x06006A1A RID: 27162 RVA: 0x0027EECD File Offset: 0x0027D0CD
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}
	}
}
