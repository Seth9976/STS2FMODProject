using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000872 RID: 2162
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BattlewornDummyEventV3Encounter : BattlewornDummyEventEncounter
	{
		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x06006A08 RID: 27144 RVA: 0x0027EC39 File Offset: 0x0027CE39
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x06006A09 RID: 27145 RVA: 0x0027EC3C File Offset: 0x0027CE3C
		public override bool ShouldGiveRewards
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x06006A0A RID: 27146 RVA: 0x0027EC3F File Offset: 0x0027CE3F
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BattleFriendV3>());
			}
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x0027EC4B File Offset: 0x0027CE4B
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BattleFriendV3>().ToMutable(), null));
		}

		// Token: 0x04002803 RID: 10243
		private const string _settingKey = "Setting";
	}
}
