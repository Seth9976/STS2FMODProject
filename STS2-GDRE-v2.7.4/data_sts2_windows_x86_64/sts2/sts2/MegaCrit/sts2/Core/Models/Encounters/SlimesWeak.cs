using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B0 RID: 2224
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlimesWeak : EncounterModel
	{
		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x00280BDC File Offset: 0x0027EDDC
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x06006B81 RID: 27521 RVA: 0x00280BDF File Offset: 0x0027EDDF
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slimes);
			}
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x06006B82 RID: 27522 RVA: 0x00280BE7 File Offset: 0x0027EDE7
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x06006B83 RID: 27523 RVA: 0x00280BEC File Offset: 0x0027EDEC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				MonsterModel[] smallSlimes = SlimesWeak._smallSlimes;
				MonsterModel[] mediumSlimes = SlimesWeak._mediumSlimes;
				int num = 0;
				MonsterModel[] array = new MonsterModel[smallSlimes.Length + mediumSlimes.Length];
				ReadOnlySpan<MonsterModel> readOnlySpan = new ReadOnlySpan<MonsterModel>(smallSlimes);
				readOnlySpan.CopyTo(new Span<MonsterModel>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				ReadOnlySpan<MonsterModel> readOnlySpan2 = new ReadOnlySpan<MonsterModel>(mediumSlimes);
				readOnlySpan2.CopyTo(new Span<MonsterModel>(array).Slice(num, readOnlySpan2.Length));
				num += readOnlySpan2.Length;
				return new <>z__ReadOnlyArray<MonsterModel>(array);
			}
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x00280C7C File Offset: 0x0027EE7C
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>();
			List<MonsterModel> list2 = SlimesWeak._smallSlimes.ToList<MonsterModel>();
			MonsterModel monsterModel = base.Rng.NextItem<MonsterModel>(list2);
			list2.Remove(monsterModel);
			MonsterModel monsterModel2 = base.Rng.NextItem<MonsterModel>(list2);
			list.Add(new ValueTuple<MonsterModel, string>(monsterModel.ToMutable(), null));
			list.Add(new ValueTuple<MonsterModel, string>(base.Rng.NextItem<MonsterModel>(SlimesWeak._mediumSlimes).ToMutable(), null));
			list.Add(new ValueTuple<MonsterModel, string>(monsterModel2.ToMutable(), null));
			return list;
		}

		// Token: 0x0400282B RID: 10283
		private static readonly MonsterModel[] _smallSlimes = new MonsterModel[]
		{
			ModelDb.Monster<LeafSlimeS>(),
			ModelDb.Monster<TwigSlimeS>()
		};

		// Token: 0x0400282C RID: 10284
		private static readonly MonsterModel[] _mediumSlimes = new MonsterModel[]
		{
			ModelDb.Monster<LeafSlimeM>(),
			ModelDb.Monster<TwigSlimeM>()
		};
	}
}
