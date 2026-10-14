using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Map
{
	// Token: 0x02000B9F RID: 2975
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoldenPathActMap : ActMap
	{
		// Token: 0x17002266 RID: 8806
		// (get) Token: 0x06007C1E RID: 31774 RVA: 0x002A4B8A File Offset: 0x002A2D8A
		public override MapPoint BossMapPoint { get; }

		// Token: 0x17002267 RID: 8807
		// (get) Token: 0x06007C1F RID: 31775 RVA: 0x002A4B92 File Offset: 0x002A2D92
		public override MapPoint StartingMapPoint { get; }

		// Token: 0x17002268 RID: 8808
		// (get) Token: 0x06007C20 RID: 31776 RVA: 0x002A4B9A File Offset: 0x002A2D9A
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x002A4BA4 File Offset: 0x002A2DA4
		public GoldenPathActMap(IRunState runState)
		{
			List<MapPointType> list = this._defaultPointTypes.ToList<MapPointType>();
			bool flag = runState.Players.Count > 1;
			if (flag)
			{
				list.RemoveAt(2);
			}
			this.Grid = new MapPoint[7, list.Count + 1];
			this.BossMapPoint = new MapPoint(base.GetColumnCount() / 2, base.GetRowCount())
			{
				PointType = MapPointType.Boss
			};
			this.StartingMapPoint = new MapPoint(base.GetColumnCount() / 2, 0)
			{
				PointType = MapPointType.Ancient
			};
			for (int i = 0; i < list.Count; i++)
			{
				MapPoint mapPoint = new MapPoint(3, i + 1);
				this.Grid[3, i + 1] = mapPoint;
				mapPoint.PointType = list[i];
				if (i > 0)
				{
					this.Grid[3, i].AddChildPoint(mapPoint);
				}
			}
			this.startMapPoints.Add(this.Grid[3, 1]);
			this.Grid[3, base.GetRowCount() - 1].AddChildPoint(this.BossMapPoint);
			this.StartingMapPoint.AddChildPoint(this.Grid[3, 1]);
		}

		// Token: 0x04002950 RID: 10576
		private readonly MapPointType[] _defaultPointTypes = new MapPointType[]
		{
			MapPointType.Monster,
			MapPointType.Unknown,
			MapPointType.Monster,
			MapPointType.RestSite,
			MapPointType.Monster,
			MapPointType.RestSite,
			MapPointType.Unknown,
			MapPointType.Treasure,
			MapPointType.Unknown,
			MapPointType.Treasure,
			MapPointType.Unknown,
			MapPointType.Shop,
			MapPointType.Elite,
			MapPointType.RestSite,
			MapPointType.Elite,
			MapPointType.RestSite
		};

		// Token: 0x04002951 RID: 10577
		private const int _width = 7;

		// Token: 0x04002952 RID: 10578
		private const int _middle = 3;
	}
}
