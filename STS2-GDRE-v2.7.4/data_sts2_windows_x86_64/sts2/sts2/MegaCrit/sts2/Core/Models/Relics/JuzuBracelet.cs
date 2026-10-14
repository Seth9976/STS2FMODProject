using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200056D RID: 1389
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JuzuBracelet : RelicModel
	{
		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x060051BD RID: 20925 RVA: 0x002417E9 File Offset: 0x0023F9E9
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x002417EC File Offset: 0x0023F9EC
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x002417F4 File Offset: 0x0023F9F4
		public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(IReadOnlySet<RoomType> roomTypes)
		{
			HashSet<RoomType> hashSet = new HashSet<RoomType>();
			foreach (RoomType roomType in roomTypes)
			{
				hashSet.Add(roomType);
			}
			HashSet<RoomType> hashSet2 = hashSet;
			hashSet2.Remove(RoomType.Monster);
			return hashSet2;
		}
	}
}
