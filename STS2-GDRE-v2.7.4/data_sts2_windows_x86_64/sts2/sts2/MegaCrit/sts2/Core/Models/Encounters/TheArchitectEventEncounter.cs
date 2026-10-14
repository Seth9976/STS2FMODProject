using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BA RID: 2234
	[NullableContext(1)]
	[Nullable(0)]
	public class TheArchitectEventEncounter : EncounterModel
	{
		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x06006BBC RID: 27580 RVA: 0x00281223 File Offset: 0x0027F423
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x06006BBD RID: 27581 RVA: 0x00281226 File Offset: 0x0027F426
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x06006BBE RID: 27582 RVA: 0x00281229 File Offset: 0x0027F429
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Architect>());
			}
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x00281235 File Offset: 0x0027F435
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Architect>().ToMutable(), null));
		}
	}
}
