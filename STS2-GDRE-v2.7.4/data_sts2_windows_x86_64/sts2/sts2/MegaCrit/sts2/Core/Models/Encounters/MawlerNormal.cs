using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000898 RID: 2200
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MawlerNormal : EncounterModel
	{
		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x00280002 File Offset: 0x0027E202
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x00280005 File Offset: 0x0027E205
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Mawler>());
			}
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x00280011 File Offset: 0x0027E211
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Mawler>().ToMutable(), null));
		}
	}
}
