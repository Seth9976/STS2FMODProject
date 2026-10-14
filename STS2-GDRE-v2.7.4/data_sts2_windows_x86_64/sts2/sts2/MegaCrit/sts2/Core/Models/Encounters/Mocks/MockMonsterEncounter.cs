using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CC RID: 2252
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockMonsterEncounter : EncounterModel
	{
		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x06006C2E RID: 27694 RVA: 0x00281953 File Offset: 0x0027FB53
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x06006C2F RID: 27695 RVA: 0x00281956 File Offset: 0x0027FB56
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x06006C30 RID: 27696 RVA: 0x00281959 File Offset: 0x0027FB59
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BigDummy>());
			}
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x00281965 File Offset: 0x0027FB65
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null));
		}
	}
}
