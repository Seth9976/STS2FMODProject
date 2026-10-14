using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B2 RID: 2226
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SludgeSpinnerWeak : EncounterModel
	{
		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x06006B8D RID: 27533 RVA: 0x00280F70 File Offset: 0x0027F170
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x06006B8E RID: 27534 RVA: 0x00280F73 File Offset: 0x0027F173
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x06006B8F RID: 27535 RVA: 0x00280F76 File Offset: 0x0027F176
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SludgeSpinner>());
			}
		}

		// Token: 0x06006B90 RID: 27536 RVA: 0x00280F82 File Offset: 0x0027F182
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SludgeSpinner>().ToMutable(), null));
		}
	}
}
