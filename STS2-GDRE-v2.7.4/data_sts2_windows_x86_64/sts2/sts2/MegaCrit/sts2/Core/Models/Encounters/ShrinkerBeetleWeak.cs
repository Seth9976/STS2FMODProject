using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AC RID: 2220
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShrinkerBeetleWeak : EncounterModel
	{
		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x06006B6B RID: 27499 RVA: 0x00280A54 File Offset: 0x0027EC54
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x06006B6C RID: 27500 RVA: 0x00280A57 File Offset: 0x0027EC57
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Shrinker);
			}
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x06006B6D RID: 27501 RVA: 0x00280A5F File Offset: 0x0027EC5F
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x06006B6E RID: 27502 RVA: 0x00280A62 File Offset: 0x0027EC62
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<ShrinkerBeetle>());
			}
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x00280A6E File Offset: 0x0027EC6E
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<ShrinkerBeetle>().ToMutable(), null));
		}
	}
}
