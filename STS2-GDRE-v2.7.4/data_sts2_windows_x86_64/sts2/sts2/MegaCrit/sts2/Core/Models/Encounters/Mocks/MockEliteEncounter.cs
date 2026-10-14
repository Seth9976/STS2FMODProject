using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CB RID: 2251
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockEliteEncounter : EncounterModel
	{
		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x06006C29 RID: 27689 RVA: 0x00281922 File Offset: 0x0027FB22
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x06006C2A RID: 27690 RVA: 0x00281925 File Offset: 0x0027FB25
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x06006C2B RID: 27691 RVA: 0x00281928 File Offset: 0x0027FB28
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BigDummy>());
			}
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x00281934 File Offset: 0x0027FB34
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null));
		}
	}
}
