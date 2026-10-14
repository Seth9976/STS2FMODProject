using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AD RID: 2221
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SkulkingColonyElite : EncounterModel
	{
		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x06006B71 RID: 27505 RVA: 0x00280A8D File Offset: 0x0027EC8D
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x06006B72 RID: 27506 RVA: 0x00280A90 File Offset: 0x0027EC90
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SkulkingColony>());
			}
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x00280A9C File Offset: 0x0027EC9C
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SkulkingColony>().ToMutable(), null));
		}
	}
}
