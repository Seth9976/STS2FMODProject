using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000876 RID: 2166
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ByrdonisElite : EncounterModel
	{
		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x06006A22 RID: 27170 RVA: 0x0027EF1E File Offset: 0x0027D11E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x06006A23 RID: 27171 RVA: 0x0027EF21 File Offset: 0x0027D121
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Byrdonis>());
			}
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x0027EF2D File Offset: 0x0027D12D
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Byrdonis>().ToMutable(), null));
		}
	}
}
