using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099A RID: 2458
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dowsing : CardModel
	{
		// Token: 0x060070D2 RID: 28882 RVA: 0x0028A11F File Offset: 0x0028831F
		public Dowsing()
			: base(-1, CardType.Quest, CardRarity.Quest, TargetType.None, true)
		{
		}

		// Token: 0x17001D9E RID: 7582
		// (get) Token: 0x060070D3 RID: 28883 RVA: 0x0028A12D File Offset: 0x0028832D
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D9F RID: 7583
		// (get) Token: 0x060070D4 RID: 28884 RVA: 0x0028A130 File Offset: 0x00288330
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Rooms", 5m));
			}
		}

		// Token: 0x17001DA0 RID: 7584
		// (get) Token: 0x060070D5 RID: 28885 RVA: 0x0028A147 File Offset: 0x00288347
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001DA1 RID: 7585
		// (get) Token: 0x060070D6 RID: 28886 RVA: 0x0028A14F File Offset: 0x0028834F
		// (set) Token: 0x060070D7 RID: 28887 RVA: 0x0028A157 File Offset: 0x00288357
		[SavedProperty]
		public int RoomsEntered
		{
			get
			{
				return this._roomsEntered;
			}
			set
			{
				base.AssertMutable();
				this._roomsEntered = value;
				base.DynamicVars["Rooms"].BaseValue = 5 - this.RoomsEntered;
			}
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x0028A188 File Offset: 0x00288388
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			CardPile pile = base.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				if (base.Owner.RunState.CurrentRoomCount <= 1)
				{
					MapPoint currentMapPoint = base.Owner.RunState.CurrentMapPoint;
					if (currentMapPoint != null && currentMapPoint.PointType == MapPointType.Unknown)
					{
						int roomsEntered = this.RoomsEntered;
						this.RoomsEntered = roomsEntered + 1;
						if (this.RoomsEntered >= 5)
						{
							PlayerCmd.CompleteQuest(this);
							await CardCmd.TransformTo<Abundance>(this, CardPreviewStyle.HorizontalLayout);
						}
					}
				}
			}
		}

		// Token: 0x0400285F RID: 10335
		public const int maxRooms = 5;

		// Token: 0x04002860 RID: 10336
		private const string _roomsKey = "Rooms";

		// Token: 0x04002861 RID: 10337
		private int _roomsEntered;
	}
}
