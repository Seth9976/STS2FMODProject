using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters.Mocks;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008C9 RID: 2249
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockAttackAndSummonEncounter : EncounterModel
	{
		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x06006C1F RID: 27679 RVA: 0x002818C0 File Offset: 0x0027FAC0
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x06006C20 RID: 27680 RVA: 0x002818C3 File Offset: 0x0027FAC3
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x06006C21 RID: 27681 RVA: 0x002818C6 File Offset: 0x0027FAC6
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<MockAttackAndSummonMinionMonster>());
			}
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x002818D2 File Offset: 0x0027FAD2
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<MockAttackAndSummonMinionMonster>().ToMutable(), null));
		}
	}
}
