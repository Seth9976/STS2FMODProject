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
	// Token: 0x02000873 RID: 2163
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlbugsNormal : EncounterModel
	{
		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x06006A0D RID: 27149 RVA: 0x0027EC6A File Offset: 0x0027CE6A
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Workers);
			}
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x06006A0E RID: 27150 RVA: 0x0027EC72 File Offset: 0x0027CE72
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x06006A0F RID: 27151 RVA: 0x0027EC75 File Offset: 0x0027CE75
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x06006A10 RID: 27152 RVA: 0x0027EC78 File Offset: 0x0027CE78
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				Dictionary<MonsterModel, int>.KeyCollection keys = BowlbugsNormal._workerValidCounts.Keys;
				int num = 0;
				MonsterModel[] array = new MonsterModel[1 + keys.Count];
				foreach (MonsterModel monsterModel in keys)
				{
					array[num] = monsterModel;
					num++;
				}
				array[num] = ModelDb.Monster<BowlbugRock>();
				return new <>z__ReadOnlyArray<MonsterModel>(array);
			}
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x0027ECF4 File Offset: 0x0027CEF4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected unsafe override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			List<MonsterModel> currentWorkers = new List<MonsterModel>();
			int num = 1;
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>(num);
			CollectionsMarshal.SetCount<ValueTuple<MonsterModel, string>>(list, num);
			Span<ValueTuple<MonsterModel, string>> span = CollectionsMarshal.AsSpan<ValueTuple<MonsterModel, string>>(list);
			int num2 = 0;
			*span[num2] = new ValueTuple<MonsterModel, string>(ModelDb.Monster<BowlbugRock>().ToMutable(), BowlbugsNormal._slotNames[0]);
			List<ValueTuple<MonsterModel, string>> list2 = list;
			Func<MonsterModel, bool> <>9__0;
			for (int i = 0; i < 2; i++)
			{
				IEnumerable<MonsterModel> keys = BowlbugsNormal._workerValidCounts.Keys;
				Func<MonsterModel, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (MonsterModel r) => currentWorkers.Count<MonsterModel>((MonsterModel c) => c == r) < BowlbugsNormal._workerValidCounts[r]);
				}
				List<MonsterModel> list3 = keys.Where<MonsterModel>(func).ToList<MonsterModel>();
				MonsterModel monsterModel = base.Rng.NextItem<MonsterModel>(list3);
				currentWorkers.Add(monsterModel);
				list2.Add(new ValueTuple<MonsterModel, string>(monsterModel.ToMutable(), BowlbugsNormal._slotNames[i + 1]));
			}
			return list2;
		}

		// Token: 0x04002804 RID: 10244
		private static readonly Dictionary<MonsterModel, int> _workerValidCounts = new Dictionary<MonsterModel, int>
		{
			{
				ModelDb.Monster<BowlbugEgg>(),
				1
			},
			{
				ModelDb.Monster<BowlbugSilk>(),
				1
			},
			{
				ModelDb.Monster<BowlbugNectar>(),
				1
			}
		};

		// Token: 0x04002805 RID: 10245
		private static readonly string[] _slotNames = new string[] { "first", "middle", "last" };
	}
}
