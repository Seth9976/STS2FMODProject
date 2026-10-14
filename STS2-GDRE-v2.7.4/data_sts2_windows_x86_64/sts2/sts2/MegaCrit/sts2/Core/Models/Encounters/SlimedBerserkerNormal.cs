using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AE RID: 2222
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlimedBerserkerNormal : EncounterModel
	{
		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x06006B75 RID: 27509 RVA: 0x00280ABB File Offset: 0x0027ECBB
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x06006B76 RID: 27510 RVA: 0x00280ABE File Offset: 0x0027ECBE
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SlimedBerserker>());
			}
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x00280ACA File Offset: 0x0027ECCA
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SlimedBerserker>().ToMutable(), null));
		}
	}
}
