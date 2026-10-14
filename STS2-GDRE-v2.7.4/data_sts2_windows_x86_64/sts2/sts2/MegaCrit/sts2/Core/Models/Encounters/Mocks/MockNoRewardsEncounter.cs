using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CD RID: 2253
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockNoRewardsEncounter : EncounterModel
	{
		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x06006C33 RID: 27699 RVA: 0x00281984 File Offset: 0x0027FB84
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x06006C34 RID: 27700 RVA: 0x00281987 File Offset: 0x0027FB87
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x06006C35 RID: 27701 RVA: 0x0028198A File Offset: 0x0027FB8A
		public override bool ShouldGiveRewards
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x06006C36 RID: 27702 RVA: 0x0028198D File Offset: 0x0027FB8D
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BigDummy>());
			}
		}

		// Token: 0x06006C37 RID: 27703 RVA: 0x00281999 File Offset: 0x0027FB99
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null));
		}
	}
}
