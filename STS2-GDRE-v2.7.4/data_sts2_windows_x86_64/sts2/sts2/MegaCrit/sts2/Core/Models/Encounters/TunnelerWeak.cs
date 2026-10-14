using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C2 RID: 2242
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TunnelerWeak : EncounterModel
	{
		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x06006BEF RID: 27631 RVA: 0x002815B4 File Offset: 0x0027F7B4
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Burrower);
			}
		}

		// Token: 0x17001B3A RID: 6970
		// (get) Token: 0x06006BF0 RID: 27632 RVA: 0x002815BC File Offset: 0x0027F7BC
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x06006BF1 RID: 27633 RVA: 0x002815BF File Offset: 0x0027F7BF
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x06006BF2 RID: 27634 RVA: 0x002815C2 File Offset: 0x0027F7C2
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Tunneler>());
			}
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x002815CE File Offset: 0x0027F7CE
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Tunneler>().ToMutable(), null));
		}
	}
}
