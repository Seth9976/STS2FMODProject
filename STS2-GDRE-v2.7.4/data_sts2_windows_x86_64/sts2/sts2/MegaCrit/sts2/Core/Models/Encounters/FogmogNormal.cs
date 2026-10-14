using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000888 RID: 2184
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FogmogNormal : EncounterModel
	{
		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x06006A8E RID: 27278 RVA: 0x0027F996 File Offset: 0x0027DB96
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x06006A8F RID: 27279 RVA: 0x0027F999 File Offset: 0x0027DB99
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "illusion", "fogmog" });
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x06006A90 RID: 27280 RVA: 0x0027F9B6 File Offset: 0x0027DBB6
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x06006A91 RID: 27281 RVA: 0x0027F9B9 File Offset: 0x0027DBB9
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Fogmog>(),
					ModelDb.Monster<EyeWithTeeth>()
				});
			}
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x0027F9D6 File Offset: 0x0027DBD6
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Fogmog>().ToMutable(), "fogmog"));
		}

		// Token: 0x04002811 RID: 10257
		public const string illusionSlot = "illusion";

		// Token: 0x04002812 RID: 10258
		public const string fogmogSlot = "fogmog";
	}
}
