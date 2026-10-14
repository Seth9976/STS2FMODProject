using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200087D RID: 2173
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CultistsNormal : EncounterModel
	{
		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x06006A47 RID: 27207 RVA: 0x0027F26F File Offset: 0x0027D46F
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06006A48 RID: 27208 RVA: 0x0027F272 File Offset: 0x0027D472
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return CultistsNormal._cultists;
			}
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x0027F279 File Offset: 0x0027D479
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<CalcifiedCultist>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<DampCultist>().ToMutable(), null)
			});
		}

		// Token: 0x04002806 RID: 10246
		private static readonly MonsterModel[] _cultists = new MonsterModel[]
		{
			ModelDb.Monster<CalcifiedCultist>(),
			ModelDb.Monster<DampCultist>()
		};
	}
}
