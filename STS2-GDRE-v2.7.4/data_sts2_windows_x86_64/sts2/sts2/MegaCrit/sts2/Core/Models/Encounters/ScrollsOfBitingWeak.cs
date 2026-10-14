using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A8 RID: 2216
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScrollsOfBitingWeak : EncounterModel
	{
		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x06006B56 RID: 27478 RVA: 0x002808C1 File Offset: 0x0027EAC1
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x06006B57 RID: 27479 RVA: 0x002808C4 File Offset: 0x0027EAC4
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x06006B58 RID: 27480 RVA: 0x002808C7 File Offset: 0x0027EAC7
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Scrolls);
			}
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x06006B59 RID: 27481 RVA: 0x002808D0 File Offset: 0x0027EAD0
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<ScrollOfBiting>());
			}
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x002808DC File Offset: 0x0027EADC
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			ScrollOfBiting scrollOfBiting = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			ScrollOfBiting scrollOfBiting2 = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			ScrollOfBiting scrollOfBiting3 = (ScrollOfBiting)ModelDb.Monster<ScrollOfBiting>().ToMutable();
			int num = base.Rng.NextInt(3);
			scrollOfBiting.StarterMoveIdx = num;
			scrollOfBiting2.StarterMoveIdx = (num + 1) % 3;
			scrollOfBiting3.StarterMoveIdx = (num + 2) % 3;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(scrollOfBiting, null),
				new ValueTuple<MonsterModel, string>(scrollOfBiting2, null),
				new ValueTuple<MonsterModel, string>(scrollOfBiting3, null)
			});
		}
	}
}
