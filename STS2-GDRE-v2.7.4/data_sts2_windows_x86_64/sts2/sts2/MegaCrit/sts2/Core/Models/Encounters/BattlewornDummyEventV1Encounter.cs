using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000870 RID: 2160
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattlewornDummyEventV1Encounter : BattlewornDummyEventEncounter
	{
		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x060069FE RID: 27134 RVA: 0x0027EBD7 File Offset: 0x0027CDD7
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x060069FF RID: 27135 RVA: 0x0027EBDA File Offset: 0x0027CDDA
		public override bool ShouldGiveRewards
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x0027EBDD File Offset: 0x0027CDDD
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BattleFriendV1>());
			}
		}

		// Token: 0x06006A01 RID: 27137 RVA: 0x0027EBE9 File Offset: 0x0027CDE9
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BattleFriendV1>().ToMutable(), null));
		}
	}
}
