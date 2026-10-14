using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089A RID: 2202
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MysteriousKnightEventEncounter : EncounterModel
	{
		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x06006AFF RID: 27391 RVA: 0x00280076 File Offset: 0x0027E276
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x06006B00 RID: 27392 RVA: 0x00280079 File Offset: 0x0027E279
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<MysteriousKnight>());
			}
		}

		// Token: 0x06006B01 RID: 27393 RVA: 0x00280085 File Offset: 0x0027E285
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<MysteriousKnight>().ToMutable(), null));
		}
	}
}
