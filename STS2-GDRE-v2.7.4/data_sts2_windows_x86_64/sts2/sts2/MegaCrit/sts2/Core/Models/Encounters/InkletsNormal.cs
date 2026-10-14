using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000891 RID: 2193
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InkletsNormal : EncounterModel
	{
		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x06006ABD RID: 27325 RVA: 0x0027FC29 File Offset: 0x0027DE29
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x06006ABE RID: 27326 RVA: 0x0027FC2C File Offset: 0x0027DE2C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Inklet>());
			}
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x0027FC38 File Offset: 0x0027DE38
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Inklet inklet = (Inklet)ModelDb.Monster<Inklet>().ToMutable();
			Inklet inklet2 = (Inklet)ModelDb.Monster<Inklet>().ToMutable();
			Inklet inklet3 = (Inklet)ModelDb.Monster<Inklet>().ToMutable();
			inklet2.MiddleInklet = true;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(inklet, null),
				new ValueTuple<MonsterModel, string>(inklet2, null),
				new ValueTuple<MonsterModel, string>(inklet3, null)
			});
		}
	}
}
