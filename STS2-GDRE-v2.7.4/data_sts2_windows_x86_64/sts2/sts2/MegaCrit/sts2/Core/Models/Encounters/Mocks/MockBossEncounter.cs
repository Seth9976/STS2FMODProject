using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CA RID: 2250
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockBossEncounter : EncounterModel
	{
		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x06006C24 RID: 27684 RVA: 0x002818F1 File Offset: 0x0027FAF1
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x06006C25 RID: 27685 RVA: 0x002818F4 File Offset: 0x0027FAF4
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x06006C26 RID: 27686 RVA: 0x002818F7 File Offset: 0x0027FAF7
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BigDummy>());
			}
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x00281903 File Offset: 0x0027FB03
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BigDummy>().ToMutable(), null));
		}
	}
}
