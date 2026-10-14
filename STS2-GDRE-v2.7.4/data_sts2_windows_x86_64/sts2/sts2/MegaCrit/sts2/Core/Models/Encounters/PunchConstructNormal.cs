using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A3 RID: 2211
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PunchConstructNormal : EncounterModel
	{
		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x06006B39 RID: 27449 RVA: 0x00280540 File Offset: 0x0027E740
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x06006B3A RID: 27450 RVA: 0x00280543 File Offset: 0x0027E743
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<PunchConstruct>());
			}
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x0028054F File Offset: 0x0027E74F
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<PunchConstruct>().ToMutable(), null));
		}
	}
}
