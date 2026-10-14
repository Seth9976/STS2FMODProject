using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000884 RID: 2180
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ExoskeletonsWeak : EncounterModel
	{
		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x06006A6E RID: 27246 RVA: 0x0027F5FC File Offset: 0x0027D7FC
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x06006A6F RID: 27247 RVA: 0x0027F5FF File Offset: 0x0027D7FF
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Exoskeletons);
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x06006A70 RID: 27248 RVA: 0x0027F608 File Offset: 0x0027D808
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "first", "second", "third" });
			}
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x06006A71 RID: 27249 RVA: 0x0027F62D File Offset: 0x0027D82D
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x06006A72 RID: 27250 RVA: 0x0027F630 File Offset: 0x0027D830
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x06006A73 RID: 27251 RVA: 0x0027F633 File Offset: 0x0027D833
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Exoskeleton>());
			}
		}

		// Token: 0x06006A74 RID: 27252 RVA: 0x0027F640 File Offset: 0x0027D840
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "second"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "third")
			});
		}
	}
}
