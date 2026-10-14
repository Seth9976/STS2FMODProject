using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000879 RID: 2169
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConstructMenagerieNormal : EncounterModel
	{
		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x06006A34 RID: 27188 RVA: 0x0027F027 File Offset: 0x0027D227
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x06006A35 RID: 27189 RVA: 0x0027F02A File Offset: 0x0027D22A
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<PunchConstruct>(),
					ModelDb.Monster<CubexConstruct>()
				});
			}
		}

		// Token: 0x06006A36 RID: 27190 RVA: 0x0027F048 File Offset: 0x0027D248
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<PunchConstruct>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<CubexConstruct>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<CubexConstruct>().ToMutable(), null)
			});
		}
	}
}
