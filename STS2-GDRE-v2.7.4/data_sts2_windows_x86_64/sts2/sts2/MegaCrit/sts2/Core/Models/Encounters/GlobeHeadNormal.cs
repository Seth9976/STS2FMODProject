using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088C RID: 2188
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GlobeHeadNormal : EncounterModel
	{
		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0027FA8E File Offset: 0x0027DC8E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0027FA91 File Offset: 0x0027DC91
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<GlobeHead>());
			}
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x0027FA9D File Offset: 0x0027DC9D
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Galvanized>().OverlayPath);
			}
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x0027FAAE File Offset: 0x0027DCAE
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<GlobeHead>().ToMutable(), null));
		}
	}
}
