using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088A RID: 2186
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FrogKnightNormal : EncounterModel
	{
		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x06006A98 RID: 27288 RVA: 0x0027FA27 File Offset: 0x0027DC27
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x06006A99 RID: 27289 RVA: 0x0027FA2A File Offset: 0x0027DC2A
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<FrogKnight>());
			}
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x0027FA36 File Offset: 0x0027DC36
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<FrogKnight>().ToMutable(), null));
		}
	}
}
