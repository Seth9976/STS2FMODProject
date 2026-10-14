using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AB RID: 2219
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SewerClamNormal : EncounterModel
	{
		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x06006B67 RID: 27495 RVA: 0x00280A26 File Offset: 0x0027EC26
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x00280A29 File Offset: 0x0027EC29
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SewerClam>());
			}
		}

		// Token: 0x06006B69 RID: 27497 RVA: 0x00280A35 File Offset: 0x0027EC35
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SewerClam>().ToMutable(), null));
		}
	}
}
