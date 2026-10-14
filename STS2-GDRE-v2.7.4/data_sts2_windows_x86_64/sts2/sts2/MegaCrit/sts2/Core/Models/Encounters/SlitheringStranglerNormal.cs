using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B1 RID: 2225
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlitheringStranglerNormal : EncounterModel
	{
		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x06006B87 RID: 27527 RVA: 0x00280D42 File Offset: 0x0027EF42
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06006B88 RID: 27528 RVA: 0x00280D45 File Offset: 0x0027EF45
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlyArray<EncounterTag>(new EncounterTag[]
				{
					EncounterTag.Jaxfruit,
					EncounterTag.Slimes
				});
			}
		}

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x00280D5C File Offset: 0x0027EF5C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				MonsterModel[] smallSlimes = SlitheringStranglerNormal._smallSlimes;
				MonsterModel[] mediumSlimes = SlitheringStranglerNormal._mediumSlimes;
				int num = 0;
				MonsterModel[] array = new MonsterModel[2 + (smallSlimes.Length + mediumSlimes.Length)];
				ReadOnlySpan<MonsterModel> readOnlySpan = new ReadOnlySpan<MonsterModel>(smallSlimes);
				readOnlySpan.CopyTo(new Span<MonsterModel>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				ReadOnlySpan<MonsterModel> readOnlySpan2 = new ReadOnlySpan<MonsterModel>(mediumSlimes);
				readOnlySpan2.CopyTo(new Span<MonsterModel>(array).Slice(num, readOnlySpan2.Length));
				num += readOnlySpan2.Length;
				array[num] = ModelDb.Monster<SnappingJaxfruit>();
				num++;
				array[num] = ModelDb.Monster<SlitheringStrangler>();
				return new <>z__ReadOnlyArray<MonsterModel>(array);
			}
		}

		// Token: 0x06006B8A RID: 27530 RVA: 0x00280E00 File Offset: 0x0027F000
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected unsafe override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			List<MonsterModel> list2;
			switch (base.Rng.NextItem<SlitheringStranglerNormal.SecondaryEnemyType>(Enum.GetValues<SlitheringStranglerNormal.SecondaryEnemyType>()))
			{
			case SlitheringStranglerNormal.SecondaryEnemyType.SnappingJaxfruit:
			{
				int num = 1;
				List<MonsterModel> list = new List<MonsterModel>(num);
				CollectionsMarshal.SetCount<MonsterModel>(list, num);
				Span<MonsterModel> span = CollectionsMarshal.AsSpan<MonsterModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Monster<SnappingJaxfruit>();
				list2 = list;
				break;
			}
			case SlitheringStranglerNormal.SecondaryEnemyType.MediumSlime:
			{
				int num2 = 1;
				List<MonsterModel> list3 = new List<MonsterModel>(num2);
				CollectionsMarshal.SetCount<MonsterModel>(list3, num2);
				Span<MonsterModel> span = CollectionsMarshal.AsSpan<MonsterModel>(list3);
				int num = 0;
				*span[num] = base.Rng.NextItem<MonsterModel>(SlitheringStranglerNormal._mediumSlimes);
				list2 = list3;
				break;
			}
			case SlitheringStranglerNormal.SecondaryEnemyType.SmallSlimes:
			{
				int num = 2;
				List<MonsterModel> list4 = new List<MonsterModel>(num);
				CollectionsMarshal.SetCount<MonsterModel>(list4, num);
				Span<MonsterModel> span = CollectionsMarshal.AsSpan<MonsterModel>(list4);
				int num2 = 0;
				*span[num2] = base.Rng.NextItem<MonsterModel>(SlitheringStranglerNormal._smallSlimes);
				num2++;
				*span[num2] = base.Rng.NextItem<MonsterModel>(SlitheringStranglerNormal._smallSlimes);
				list2 = list4;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			List<MonsterModel> list5 = list2;
			list5.Add(ModelDb.Monster<SlitheringStrangler>());
			return list5.Select<MonsterModel, ValueTuple<MonsterModel, string>>((MonsterModel m) => new ValueTuple<MonsterModel, string>(m.ToMutable(), null)).ToList<ValueTuple<MonsterModel, string>>();
		}

		// Token: 0x0400282D RID: 10285
		private static readonly MonsterModel[] _smallSlimes = new MonsterModel[]
		{
			ModelDb.Monster<LeafSlimeS>(),
			ModelDb.Monster<TwigSlimeS>()
		};

		// Token: 0x0400282E RID: 10286
		private static readonly MonsterModel[] _mediumSlimes = new MonsterModel[]
		{
			ModelDb.Monster<LeafSlimeM>(),
			ModelDb.Monster<TwigSlimeM>()
		};

		// Token: 0x02001FB8 RID: 8120
		[NullableContext(0)]
		private enum SecondaryEnemyType
		{
			// Token: 0x0400819E RID: 33182
			SnappingJaxfruit,
			// Token: 0x0400819F RID: 33183
			MediumSlime,
			// Token: 0x040081A0 RID: 33184
			SmallSlimes
		}
	}
}
