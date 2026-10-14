using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000890 RID: 2192
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfestedPrismsElite : EncounterModel
	{
		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x0027FBEA File Offset: 0x0027DDEA
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x0027FBED File Offset: 0x0027DDED
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Tainted>().OverlayPath);
			}
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x0027FBFE File Offset: 0x0027DDFE
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<InfestedPrism>().ToMutable(), null));
		}

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x06006ABB RID: 27323 RVA: 0x0027FC15 File Offset: 0x0027DE15
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<InfestedPrism>());
			}
		}
	}
}
