using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A4 RID: 2212
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PunchOffEventEncounter : EncounterModel
	{
		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x06006B3D RID: 27453 RVA: 0x0028056E File Offset: 0x0027E76E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x06006B3E RID: 27454 RVA: 0x00280571 File Offset: 0x0027E771
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<PunchConstruct>());
			}
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x00280580 File Offset: 0x0027E780
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			PunchConstruct punchConstruct = (PunchConstruct)ModelDb.Monster<PunchConstruct>().ToMutable();
			punchConstruct.StartsWithFastPunch = true;
			punchConstruct.StartingHpReduction = base.Rng.NextInt(2, 10);
			PunchConstruct punchConstruct2 = (PunchConstruct)ModelDb.Monster<PunchConstruct>().ToMutable();
			punchConstruct2.StartingHpReduction = base.Rng.NextInt(2, 10);
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(punchConstruct, null),
				new ValueTuple<MonsterModel, string>(punchConstruct2, null)
			});
		}
	}
}
