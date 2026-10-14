using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000871 RID: 2161
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattlewornDummyEventV2Encounter : BattlewornDummyEventEncounter
	{
		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x0027EC08 File Offset: 0x0027CE08
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x0027EC0B File Offset: 0x0027CE0B
		public override bool ShouldGiveRewards
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x0027EC0E File Offset: 0x0027CE0E
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BattleFriendV2>());
			}
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x0027EC1A File Offset: 0x0027CE1A
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BattleFriendV2>().ToMutable(), null));
		}
	}
}
