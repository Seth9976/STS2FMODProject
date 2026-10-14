using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000896 RID: 2198
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LivingFogNormal : EncounterModel
	{
		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x06006AE9 RID: 27369 RVA: 0x0027FF39 File Offset: 0x0027E139
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x06006AEA RID: 27370 RVA: 0x0027FF3C File Offset: 0x0027E13C
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x06006AEB RID: 27371 RVA: 0x0027FF3F File Offset: 0x0027E13F
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "bomb1", "bomb2", "bomb3", "bomb4", "bomb5", "livingFog" });
			}
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x0027FF7C File Offset: 0x0027E17C
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x06006AED RID: 27373 RVA: 0x0027FF83 File Offset: 0x0027E183
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<LivingFog>(),
					ModelDb.Monster<GasBomb>()
				});
			}
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x06006AEE RID: 27374 RVA: 0x0027FFA0 File Offset: 0x0027E1A0
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Smog>().OverlayPath);
			}
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x0027FFB1 File Offset: 0x0027E1B1
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<LivingFog>().ToMutable(), "livingFog"));
		}

		// Token: 0x0400281A RID: 10266
		private const string _livingFogSlot = "livingFog";

		// Token: 0x0400281B RID: 10267
		private const string _bombSlotPrefix = "bomb";
	}
}
