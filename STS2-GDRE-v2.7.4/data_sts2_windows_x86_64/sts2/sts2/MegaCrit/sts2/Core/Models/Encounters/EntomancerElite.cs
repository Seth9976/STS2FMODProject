using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000882 RID: 2178
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntomancerElite : EncounterModel
	{
		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x06006A63 RID: 27235 RVA: 0x0027F4FA File Offset: 0x0027D6FA
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x06006A64 RID: 27236 RVA: 0x0027F4FD File Offset: 0x0027D6FD
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Entomancer>());
			}
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x0027F509 File Offset: 0x0027D709
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Entomancer>().ToMutable(), null));
		}
	}
}
