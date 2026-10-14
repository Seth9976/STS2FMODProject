using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000889 RID: 2185
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FossilStalkerNormal : EncounterModel
	{
		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x06006A94 RID: 27284 RVA: 0x0027F9F9 File Offset: 0x0027DBF9
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x06006A95 RID: 27285 RVA: 0x0027F9FC File Offset: 0x0027DBFC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<FossilStalker>());
			}
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x0027FA08 File Offset: 0x0027DC08
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<FossilStalker>().ToMutable(), null));
		}
	}
}
