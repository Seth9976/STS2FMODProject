using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000619 RID: 1561
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WingedBoots : RelicModel
	{
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06005642 RID: 22082 RVA: 0x0024A34C File Offset: 0x0024854C
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06005643 RID: 22083 RVA: 0x0024A34F File Offset: 0x0024854F
		public override bool IsUsedUp
		{
			get
			{
				return this.TimesUsed >= 3;
			}
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06005644 RID: 22084 RVA: 0x0024A35D File Offset: 0x0024855D
		public override bool ShowCounter
		{
			get
			{
				return !this.IsUsedUp;
			}
		}

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x06005645 RID: 22085 RVA: 0x0024A368 File Offset: 0x00248568
		public override int DisplayAmount
		{
			get
			{
				return 3 - this.TimesUsed;
			}
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0024A372 File Offset: 0x00248572
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.Count == 1;
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x06005647 RID: 22087 RVA: 0x0024A382 File Offset: 0x00248582
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Rooms", 3m));
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x06005648 RID: 22088 RVA: 0x0024A399 File Offset: 0x00248599
		// (set) Token: 0x06005649 RID: 22089 RVA: 0x0024A3A1 File Offset: 0x002485A1
		[SavedProperty]
		public int TimesUsed
		{
			get
			{
				return this._timesUsed;
			}
			set
			{
				base.AssertMutable();
				this._timesUsed = value;
				base.DynamicVars["Rooms"].BaseValue = 3 - this._timesUsed;
				base.InvokeDisplayAmountChanged();
				this.CheckIfUsedUp();
			}
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x0024A3DE File Offset: 0x002485DE
		public override bool ShouldAllowFreeTravel()
		{
			return !this.IsUsedUp;
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x0024A3EC File Offset: 0x002485EC
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (this.IsUsedUp)
			{
				return Task.CompletedTask;
			}
			if (base.Owner.RunState.CurrentRoomCount > 1)
			{
				return Task.CompletedTask;
			}
			RunState runState = base.Owner.RunState as RunState;
			if (runState == null)
			{
				return Task.CompletedTask;
			}
			if (runState.VisitedMapCoords.Count <= 1)
			{
				return Task.CompletedTask;
			}
			IReadOnlyList<MapCoord> visitedMapCoords = runState.VisitedMapCoords;
			MapCoord mapCoord = visitedMapCoords[visitedMapCoords.Count - 2];
			MapPoint point = runState.Map.GetPoint(mapCoord);
			if (point == null)
			{
				return Task.CompletedTask;
			}
			MapPoint currentMapPoint = base.Owner.RunState.CurrentMapPoint;
			if (currentMapPoint == null)
			{
				return Task.CompletedTask;
			}
			if (point.Children.Contains(currentMapPoint))
			{
				return Task.CompletedTask;
			}
			int timesUsed = this.TimesUsed;
			this.TimesUsed = timesUsed + 1;
			return Task.CompletedTask;
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x0024A4BD File Offset: 0x002486BD
		private void CheckIfUsedUp()
		{
			if (this.IsUsedUp)
			{
				base.Status = RelicStatus.Disabled;
			}
		}

		// Token: 0x040024E6 RID: 9446
		private const string _roomsKey = "Rooms";

		// Token: 0x040024E7 RID: 9447
		private const int _roomCount = 3;

		// Token: 0x040024E8 RID: 9448
		private int _timesUsed;
	}
}
