using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200087C RID: 2172
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CubexConstructNormal : EncounterModel
	{
		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x06006A43 RID: 27203 RVA: 0x0027F241 File Offset: 0x0027D441
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x06006A44 RID: 27204 RVA: 0x0027F244 File Offset: 0x0027D444
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<CubexConstruct>());
			}
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x0027F250 File Offset: 0x0027D450
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<CubexConstruct>().ToMutable(), null));
		}
	}
}
