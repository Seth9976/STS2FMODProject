using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000897 RID: 2199
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LouseProgenitorNormal : EncounterModel
	{
		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x06006AF1 RID: 27377 RVA: 0x0027FFD4 File Offset: 0x0027E1D4
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x06006AF2 RID: 27378 RVA: 0x0027FFD7 File Offset: 0x0027E1D7
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<LouseProgenitor>());
			}
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x0027FFE3 File Offset: 0x0027E1E3
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<LouseProgenitor>().ToMutable(), null));
		}
	}
}
