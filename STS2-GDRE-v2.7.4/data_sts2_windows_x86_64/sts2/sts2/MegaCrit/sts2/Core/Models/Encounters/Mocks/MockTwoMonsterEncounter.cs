using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CF RID: 2255
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockTwoMonsterEncounter : EncounterModel
	{
		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x06006C40 RID: 27712 RVA: 0x00281A28 File Offset: 0x0027FC28
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x06006C41 RID: 27713 RVA: 0x00281A2B File Offset: 0x0027FC2B
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x06006C42 RID: 27714 RVA: 0x00281A2E File Offset: 0x0027FC2E
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BigDummy>());
			}
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x00281A3A File Offset: 0x0027FC3A
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null)
			});
		}
	}
}
