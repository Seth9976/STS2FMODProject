using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BB RID: 2235
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheInsatiableBoss : EncounterModel
	{
		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x06006BC1 RID: 27585 RVA: 0x00281254 File Offset: 0x0027F454
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act2_boss_the_insatiable";
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x06006BC2 RID: 27586 RVA: 0x0028125B File Offset: 0x0027F45B
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x0028125E File Offset: 0x0027F45E
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x06006BC4 RID: 27588 RVA: 0x00281265 File Offset: 0x0027F465
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x06006BC5 RID: 27589 RVA: 0x00281268 File Offset: 0x0027F468
		public override string AmbientSfx
		{
			get
			{
				return "event:/sfx/ambience/act2_ambience_the_insatiable";
			}
		}

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x06006BC6 RID: 27590 RVA: 0x0028126F File Offset: 0x0027F46F
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<TheInsatiable>());
			}
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x0028127B File Offset: 0x0027F47B
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<TheInsatiable>().ToMutable(), null));
		}
	}
}
