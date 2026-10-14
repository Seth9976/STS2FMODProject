using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A6 RID: 2214
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RubyRaidersNormal : EncounterModel
	{
		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x06006B4C RID: 27468 RVA: 0x002806D6 File Offset: 0x0027E8D6
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x06006B4D RID: 27469 RVA: 0x002806D9 File Offset: 0x0027E8D9
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return RubyRaidersNormal._raiderValidCounts.Keys;
			}
		}

		// Token: 0x06006B4E RID: 27470 RVA: 0x002806E8 File Offset: 0x0027E8E8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			List<MonsterModel> currentRaiders = new List<MonsterModel>();
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>();
			Func<MonsterModel, bool> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IEnumerable<MonsterModel> keys = RubyRaidersNormal._raiderValidCounts.Keys;
				Func<MonsterModel, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (MonsterModel r) => currentRaiders.Count<MonsterModel>((MonsterModel c) => c == r) < RubyRaidersNormal._raiderValidCounts[r]);
				}
				List<MonsterModel> list2 = keys.Where<MonsterModel>(func).ToList<MonsterModel>();
				MonsterModel monsterModel = base.Rng.NextItem<MonsterModel>(list2);
				currentRaiders.Add(monsterModel);
				list.Add(new ValueTuple<MonsterModel, string>(monsterModel.ToMutable(), null));
			}
			return list;
		}

		// Token: 0x0400282A RID: 10282
		private static readonly Dictionary<MonsterModel, int> _raiderValidCounts = new Dictionary<MonsterModel, int>
		{
			{
				ModelDb.Monster<AxeRubyRaider>(),
				1
			},
			{
				ModelDb.Monster<AssassinRubyRaider>(),
				1
			},
			{
				ModelDb.Monster<BruteRubyRaider>(),
				1
			},
			{
				ModelDb.Monster<CrossbowRubyRaider>(),
				1
			},
			{
				ModelDb.Monster<TrackerRubyRaider>(),
				1
			}
		};
	}
}
