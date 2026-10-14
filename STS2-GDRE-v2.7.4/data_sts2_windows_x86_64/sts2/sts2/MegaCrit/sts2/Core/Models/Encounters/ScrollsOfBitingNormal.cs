using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A7 RID: 2215
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScrollsOfBitingNormal : EncounterModel
	{
		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x06006B51 RID: 27473 RVA: 0x002807DB File Offset: 0x0027E9DB
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x06006B52 RID: 27474 RVA: 0x002807DE File Offset: 0x0027E9DE
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Scrolls);
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x06006B53 RID: 27475 RVA: 0x002807E7 File Offset: 0x0027E9E7
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<ScrollOfBiting>());
			}
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x002807F4 File Offset: 0x0027E9F4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			ScrollOfBiting scrollOfBiting = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			ScrollOfBiting scrollOfBiting2 = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			ScrollOfBiting scrollOfBiting3 = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			ScrollOfBiting scrollOfBiting4 = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			int num = base.Rng.NextInt(3);
			scrollOfBiting.StarterMoveIdx = num;
			scrollOfBiting2.StarterMoveIdx = (num + 1) % 3;
			scrollOfBiting3.StarterMoveIdx = (num + 2) % 3;
			scrollOfBiting4.StarterMoveIdx = 2;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(scrollOfBiting, null),
				new ValueTuple<MonsterModel, string>(scrollOfBiting2, null),
				new ValueTuple<MonsterModel, string>(scrollOfBiting3, null),
				new ValueTuple<MonsterModel, string>(scrollOfBiting4, null)
			});
		}
	}
}
