using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// Handles the creation of map points for a map.
	/// </summary>
	// Token: 0x02000B9E RID: 2974
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ActMap
	{
		// Token: 0x17002262 RID: 8802
		// (get) Token: 0x06007C11 RID: 31761
		public abstract MapPoint BossMapPoint { get; }

		// Token: 0x17002263 RID: 8803
		// (get) Token: 0x06007C12 RID: 31762
		public abstract MapPoint StartingMapPoint { get; }

		/// <summary>
		/// The second boss map point for Double Boss mode (Ascension 10+).
		/// Null if not in double boss mode.
		/// </summary>
		// Token: 0x17002264 RID: 8804
		// (get) Token: 0x06007C13 RID: 31763 RVA: 0x002A48CF File Offset: 0x002A2ACF
		[Nullable(2)]
		public virtual MapPoint SecondBossMapPoint
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17002265 RID: 8805
		// (get) Token: 0x06007C14 RID: 31764
		[Nullable(new byte[] { 1, 2 })]
		protected abstract MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x002A48D2 File Offset: 0x002A2AD2
		public int GetColumnCount()
		{
			return this.Grid.GetLength(0);
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x002A48E0 File Offset: 0x002A2AE0
		public int GetRowCount()
		{
			return this.Grid.GetLength(1);
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x002A48EE File Offset: 0x002A2AEE
		public IEnumerable<MapPoint> GetAllMapPoints()
		{
			ActMap.<GetAllMapPoints>d__11 <GetAllMapPoints>d__ = new ActMap.<GetAllMapPoints>d__11(-2);
			<GetAllMapPoints>d__.<>4__this = this;
			return <GetAllMapPoints>d__;
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x002A48FE File Offset: 0x002A2AFE
		public IEnumerable<MapPoint> GetPointsInRow(int row)
		{
			ActMap.<GetPointsInRow>d__12 <GetPointsInRow>d__ = new ActMap.<GetPointsInRow>d__12(-2);
			<GetPointsInRow>d__.<>4__this = this;
			<GetPointsInRow>d__.<>3__row = row;
			return <GetPointsInRow>d__;
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x002A4915 File Offset: 0x002A2B15
		[NullableContext(2)]
		public virtual MapPoint GetPoint(MapCoord coord)
		{
			return this.GetPoint(coord.col, coord.row);
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x002A492C File Offset: 0x002A2B2C
		[NullableContext(2)]
		public MapPoint GetPoint(int col, int row)
		{
			if (col == this.BossMapPoint.coord.col && row == this.BossMapPoint.coord.row)
			{
				return this.BossMapPoint;
			}
			if (this.SecondBossMapPoint != null && col == this.SecondBossMapPoint.coord.col && row == this.SecondBossMapPoint.coord.row)
			{
				return this.SecondBossMapPoint;
			}
			if (col == this.StartingMapPoint.coord.col && row == this.StartingMapPoint.coord.row)
			{
				return this.StartingMapPoint;
			}
			if (col >= 0 && col < this.Grid.GetLength(0) && row >= 0 && row < this.Grid.GetLength(1))
			{
				return this.Grid[col, row];
			}
			return null;
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x002A4A00 File Offset: 0x002A2C00
		public bool IsInMap(MapPoint mapPoint)
		{
			if (mapPoint.PointType == MapPointType.Ancient || mapPoint.PointType == MapPointType.Boss)
			{
				return true;
			}
			int col = mapPoint.coord.col;
			int row = mapPoint.coord.row;
			return col >= 0 && col < this.Grid.GetLength(0) && row >= 0 && row < this.Grid.GetLength(1) && this.Grid[col, row] != null;
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x002A4A74 File Offset: 0x002A2C74
		public bool HasPoint(MapCoord coord)
		{
			return (coord.col == this.BossMapPoint.coord.col && coord.row == this.BossMapPoint.coord.row) || (this.SecondBossMapPoint != null && coord.col == this.SecondBossMapPoint.coord.col && coord.row == this.SecondBossMapPoint.coord.row) || (coord.col == this.StartingMapPoint.coord.col && coord.row == this.StartingMapPoint.coord.row) || (coord.col >= 0 && coord.col < this.Grid.GetLength(0) && coord.row >= 0 && coord.row < this.Grid.GetLength(1) && this.Grid[coord.col, coord.row] != null);
		}

		// Token: 0x0400294F RID: 10575
		public readonly HashSet<MapPoint> startMapPoints = new HashSet<MapPoint>();
	}
}
