using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AA RID: 2218
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SeapunkWeak : EncounterModel
	{
		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x06006B61 RID: 27489 RVA: 0x002809EC File Offset: 0x0027EBEC
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x06006B62 RID: 27490 RVA: 0x002809EF File Offset: 0x0027EBEF
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Seapunk);
			}
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x06006B63 RID: 27491 RVA: 0x002809F8 File Offset: 0x0027EBF8
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x06006B64 RID: 27492 RVA: 0x002809FB File Offset: 0x0027EBFB
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Seapunk>());
			}
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x00280A07 File Offset: 0x0027EC07
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Seapunk>().ToMutable(), null));
		}
	}
}
