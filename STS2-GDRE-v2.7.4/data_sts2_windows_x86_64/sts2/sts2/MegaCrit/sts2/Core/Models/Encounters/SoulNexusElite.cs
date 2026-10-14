using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B6 RID: 2230
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulNexusElite : EncounterModel
	{
		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x06006BA7 RID: 27559 RVA: 0x00281136 File Offset: 0x0027F336
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x06006BA8 RID: 27560 RVA: 0x00281139 File Offset: 0x0027F339
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SoulNexus>());
			}
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x00281145 File Offset: 0x0027F345
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SoulNexus>().ToMutable(), null));
		}
	}
}
