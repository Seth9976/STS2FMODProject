using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B8 RID: 2232
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TerrorEelElite : EncounterModel
	{
		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x06006BAF RID: 27567 RVA: 0x00281192 File Offset: 0x0027F392
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x00281195 File Offset: 0x0027F395
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x0028119C File Offset: 0x0027F39C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<TerrorEel>());
			}
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x002811A8 File Offset: 0x0027F3A8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<TerrorEel>().ToMutable(), null));
		}
	}
}
