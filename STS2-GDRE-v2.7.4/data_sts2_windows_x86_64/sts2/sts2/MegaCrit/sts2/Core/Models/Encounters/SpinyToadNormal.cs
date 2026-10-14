using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B7 RID: 2231
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpinyToadNormal : EncounterModel
	{
		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x06006BAB RID: 27563 RVA: 0x00281164 File Offset: 0x0027F364
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x06006BAC RID: 27564 RVA: 0x00281167 File Offset: 0x0027F367
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SpinyToad>());
			}
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x00281173 File Offset: 0x0027F373
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SpinyToad>().ToMutable(), null));
		}
	}
}
