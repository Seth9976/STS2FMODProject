using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000887 RID: 2183
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlyconidNormal : EncounterModel
	{
		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x06006A88 RID: 27272 RVA: 0x0027F8A0 File Offset: 0x0027DAA0
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x06006A89 RID: 27273 RVA: 0x0027F8A3 File Offset: 0x0027DAA3
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterTag>(new EncounterTag[]
				{
					EncounterTag.Mushroom,
					EncounterTag.Slimes
				});
			}
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x06006A8A RID: 27274 RVA: 0x0027F8BC File Offset: 0x0027DABC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				MonsterModel monsterModel = ModelDb.Monster<Flyconid>();
				MonsterModel[] mediumSlimes = FlyconidNormal._mediumSlimes;
				int num = 0;
				MonsterModel[] array = new MonsterModel[1 + mediumSlimes.Length];
				array[num] = monsterModel;
				num++;
				ReadOnlySpan<MonsterModel> readOnlySpan = new ReadOnlySpan<MonsterModel>(mediumSlimes);
				readOnlySpan.CopyTo(new Span<MonsterModel>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				return new <>z__ReadOnlyArray<MonsterModel>(array);
			}
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x0027F920 File Offset: 0x0027DB20
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(base.Rng.NextItem<MonsterModel>(FlyconidNormal._mediumSlimes).ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Flyconid>().ToMutable(), null)
			});
		}

		// Token: 0x04002810 RID: 10256
		private static readonly MonsterModel[] _mediumSlimes = new MonsterModel[]
		{
			ModelDb.Monster<LeafSlimeM>(),
			ModelDb.Monster<TwigSlimeM>()
		};
	}
}
