using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters.Mocks;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008C8 RID: 2248
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockArtifactEncounter : EncounterModel
	{
		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x06006C1A RID: 27674 RVA: 0x0028188F File Offset: 0x0027FA8F
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x06006C1B RID: 27675 RVA: 0x00281892 File Offset: 0x0027FA92
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x06006C1C RID: 27676 RVA: 0x00281895 File Offset: 0x0027FA95
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<MockArtifactMonster>());
			}
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x002818A1 File Offset: 0x0027FAA1
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<MockArtifactMonster>().ToMutable(), null));
		}
	}
}
