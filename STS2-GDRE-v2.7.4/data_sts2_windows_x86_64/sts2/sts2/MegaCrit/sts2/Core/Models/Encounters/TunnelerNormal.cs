using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C1 RID: 2241
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TunnelerNormal : EncounterModel
	{
		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x06006BEA RID: 27626 RVA: 0x00281522 File Offset: 0x0027F722
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterTag>(new EncounterTag[]
				{
					EncounterTag.Burrower,
					EncounterTag.Chomper
				});
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x06006BEB RID: 27627 RVA: 0x00281537 File Offset: 0x0027F737
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x06006BEC RID: 27628 RVA: 0x0028153A File Offset: 0x0027F73A
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Tunneler>(),
					ModelDb.Monster<Chomper>()
				});
			}
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x00281558 File Offset: 0x0027F758
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Chomper chomper = (Chomper)ModelDb.Monster<Chomper>().ToMutable();
			chomper.ScreamFirst = true;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(chomper, null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Tunneler>().ToMutable(), null)
			});
		}
	}
}
