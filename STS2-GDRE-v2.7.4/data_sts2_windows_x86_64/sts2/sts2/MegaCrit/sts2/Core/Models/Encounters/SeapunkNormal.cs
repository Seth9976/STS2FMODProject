using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A9 RID: 2217
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SeapunkNormal : EncounterModel
	{
		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x06006B5C RID: 27484 RVA: 0x00280980 File Offset: 0x0027EB80
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x06006B5D RID: 27485 RVA: 0x00280983 File Offset: 0x0027EB83
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Seapunk);
			}
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x06006B5E RID: 27486 RVA: 0x0028098C File Offset: 0x0027EB8C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Seapunk>(),
					ModelDb.Monster<CalcifiedCultist>()
				});
			}
		}

		// Token: 0x06006B5F RID: 27487 RVA: 0x002809A9 File Offset: 0x0027EBA9
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<CalcifiedCultist>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Seapunk>().ToMutable(), null)
			});
		}
	}
}
