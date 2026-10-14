using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A23 RID: 2595
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LanternKey : CardModel
	{
		// Token: 0x060073B7 RID: 29623 RVA: 0x0028FF80 File Offset: 0x0028E180
		public LanternKey()
			: base(-1, CardType.Quest, CardRarity.Quest, TargetType.Self, true)
		{
		}

		// Token: 0x17001ED9 RID: 7897
		// (get) Token: 0x060073B8 RID: 29624 RVA: 0x0028FF8E File Offset: 0x0028E18E
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001EDA RID: 7898
		// (get) Token: 0x060073B9 RID: 29625 RVA: 0x0028FF91 File Offset: 0x0028E191
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x0028FF9C File Offset: 0x0028E19C
		public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(IReadOnlySet<RoomType> roomTypes)
		{
			if (2 != base.Owner.RunState.CurrentActIndex)
			{
				return roomTypes;
			}
			return new HashSet<RoomType> { RoomType.Event };
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x0028FFCD File Offset: 0x0028E1CD
		public override EventModel ModifyNextEvent(EventModel currentEvent)
		{
			if (2 != base.Owner.RunState.CurrentActIndex)
			{
				return currentEvent;
			}
			return ModelDb.Event<WarHistorianRepy>();
		}

		// Token: 0x04002885 RID: 10373
		private const int _gloryActIndex = 2;
	}
}
