using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088E RID: 2190
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HauntedShipNormal : EncounterModel
	{
		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x0027FB8E File Offset: 0x0027DD8E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x0027FB91 File Offset: 0x0027DD91
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<HauntedShip>());
			}
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x0027FB9D File Offset: 0x0027DD9D
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<HauntedShip>().ToMutable(), null));
		}
	}
}
