using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A0 RID: 2208
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OwlMagistrateNormal : EncounterModel
	{
		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x06006B24 RID: 27428 RVA: 0x00280366 File Offset: 0x0027E566
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x06006B25 RID: 27429 RVA: 0x00280369 File Offset: 0x0027E569
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<OwlMagistrate>());
			}
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x00280375 File Offset: 0x0027E575
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<OwlMagistrate>().ToMutable(), null));
		}
	}
}
