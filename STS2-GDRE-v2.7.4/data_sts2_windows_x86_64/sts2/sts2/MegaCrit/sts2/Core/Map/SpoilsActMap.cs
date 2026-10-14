using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// An hourglass-shaped map where all paths converge through a single centered treasure node.
	/// Used when SpoilsMap card is active in Act 2.
	///
	/// This map is generated from scratch rather than post-processing StandardActMap.
	/// The hourglass structure requires fundamentally different path topology. All paths must converge
	/// through a single centered treasure node, with rows narrowing towards the treasure then widening after.
	/// Post-processing would require deleting most nodes and rewiring all edges, which is roughly as complex
	/// as generating from scratch.
	///
	/// Keeping these implementations separate makes both easier to understand. StandardActMap stays clean,
	/// and the hourglass-specific constraints live here explicitly instead of being tangled up in post-processing logic.
	/// Each can evolve independently.
	///
	/// The tradeoff is this duplicates some map generation logic (~400 lines). If StandardActMap's core rules change
	/// (adjacency constraints, parent/child limits, point type placement), we'll need to manually sync those changes here.
	/// Tests in SpoilsActMapTest verify the core map invariants hold.
	/// </summary>
	// Token: 0x02000BAC RID: 2988
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpoilsActMap : ActMap
	{
		// Token: 0x17002280 RID: 8832
		// (get) Token: 0x06007C8C RID: 31884 RVA: 0x002A6C20 File Offset: 0x002A4E20
		public override MapPoint BossMapPoint { get; }

		// Token: 0x17002281 RID: 8833
		// (get) Token: 0x06007C8D RID: 31885 RVA: 0x002A6C28 File Offset: 0x002A4E28
		public override MapPoint StartingMapPoint { get; }

		// Token: 0x17002282 RID: 8834
		// (get) Token: 0x06007C8E RID: 31886 RVA: 0x002A6C30 File Offset: 0x002A4E30
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		/// <summary>
		/// Creates a map used only when the player has the <see cref="T:MegaCrit.Sts2.Core.Models.Cards.SpoilsMap" />.
		/// </summary>
		/// <param name="runState">The run state for this run.</param>
		/// <param name="mapPointTypeCountsOverride">If passed, replaces the default point counts used to generate the map.</param>
		// Token: 0x06007C8F RID: 31887 RVA: 0x002A6C38 File Offset: 0x002A4E38
		public SpoilsActMap(IRunState runState, [Nullable(2)] MapPointTypeCounts mapPointTypeCountsOverride = null)
		{
			ActModel act = runState.Act;
			bool flag = runState.Players.Count > 1;
			this._mapLength = act.GetNumberOfRooms(flag) + 1;
			this._rng = new Rng(runState.Rng.Seed, "spoils_map");
			this._pointTypeCounts = mapPointTypeCountsOverride ?? act.GetMapPointTypes(this._rng);
			this.Grid = new MapPoint[7, this._mapLength];
			this._treasureRow = base.GetRowCount() - 7;
			this.BossMapPoint = new MapPoint(base.GetColumnCount() / 2, base.GetRowCount());
			this.StartingMapPoint = new MapPoint(base.GetColumnCount() / 2, 0);
			this.GenerateHourglassMap();
			this.AssignPointTypes();
			MapPathPruning.PruneAndRepair(this.Grid, this.startMapPoints, this, this._pointTypeCounts, this._rng, new Func<MapPointType, MapPoint, bool>(this.IsValidPointType));
		}

		// Token: 0x06007C90 RID: 31888 RVA: 0x002A6D28 File Offset: 0x002A4F28
		private MapPoint GetOrCreatePoint(int col, int row)
		{
			if (col >= 0 && col < base.GetColumnCount() && row >= 0 && row < base.GetRowCount())
			{
				MapPoint mapPoint = this.Grid[col, row];
				if (mapPoint != null)
				{
					return mapPoint;
				}
			}
			MapPoint mapPoint2 = new MapPoint(col, row);
			this.Grid[col, row] = mapPoint2;
			return mapPoint2;
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x002A6D7C File Offset: 0x002A4F7C
		private void GenerateHourglassMap()
		{
			int num = base.GetColumnCount() / 2;
			if (this._treasureRow <= 0 || this._treasureRow >= base.GetRowCount())
			{
				throw new InvalidOperationException("Treasure row is out of bounds for SpoilsActMap");
			}
			for (int i = 0; i < 7; i++)
			{
				MapPoint mapPoint = this.GetOrCreatePoint(this._rng.NextInt(0, 7), 1);
				if (i == 1)
				{
					while (this.startMapPoints.Contains(mapPoint))
					{
						mapPoint = this.GetOrCreatePoint(this._rng.NextInt(0, 7), 1);
					}
				}
				this.startMapPoints.Add(mapPoint);
				this.PathGenerate(mapPoint);
			}
			MapPoint orCreatePoint = this.GetOrCreatePoint(num, this._treasureRow);
			orCreatePoint.PointType = MapPointType.Treasure;
			orCreatePoint.CanBeModified = false;
			foreach (MapPoint mapPoint2 in base.GetPointsInRow(this._treasureRow).ToList<MapPoint>())
			{
				if (mapPoint2 != orCreatePoint)
				{
					this.RedirectToTreasure(mapPoint2, orCreatePoint);
				}
			}
			this.ConnectRowToBoss();
			this.ConnectRowToStart();
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x002A6E94 File Offset: 0x002A5094
		private void PathGenerate(MapPoint startingPoint)
		{
			MapPoint mapPoint = startingPoint;
			while (mapPoint.coord.row < this._mapLength - 1)
			{
				MapCoord mapCoord = this.GenerateNextCoord(mapPoint);
				MapPoint orCreatePoint = this.GetOrCreatePoint(mapCoord.col, mapCoord.row);
				mapPoint.AddChildPoint(orCreatePoint);
				mapPoint = orCreatePoint;
			}
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x002A6EE0 File Offset: 0x002A50E0
		private MapCoord GenerateNextCoord(MapPoint current)
		{
			int num = current.coord.row + 1;
			ValueTuple<int, int> allowedColumnsForRow = this.GetAllowedColumnsForRow(num);
			int item = allowedColumnsForRow.Item1;
			int item2 = allowedColumnsForRow.Item2;
			int num2 = base.GetColumnCount() / 2;
			List<int> list = new List<int> { -1, 0, 1 };
			int num3 = this._treasureRow - current.coord.row;
			if (num3 > 3)
			{
				list.StableShuffle<int>(this._rng);
			}
			else if (num3 > 0)
			{
				list = this.BuildCenteredPriorityList(current.coord.col, num2);
			}
			else
			{
				list.StableShuffle<int>(this._rng);
			}
			foreach (int num4 in list)
			{
				int nextColumn = this.GetNextColumn(current.coord.col, num4);
				if (nextColumn >= item && nextColumn <= item2 && !this.HasInvalidCrossover(current, nextColumn))
				{
					MapPoint point = base.GetPoint(nextColumn, num);
					if ((point == null || point.parents.Contains(current) || point.parents.Count < 3) && (current == this.StartingMapPoint || current.Children.Count < 3 || (point != null && current.Children.Contains(point))))
					{
						if (Math.Abs(nextColumn - current.coord.col) > 1)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Invalid step from (");
							defaultInterpolatedStringHandler.AppendFormatted<int>(current.coord.col);
							defaultInterpolatedStringHandler.AppendLiteral(", ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(current.coord.row);
							defaultInterpolatedStringHandler.AppendLiteral(") to column ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(nextColumn);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						MapCoord mapCoord = default(MapCoord);
						mapCoord.col = nextColumn;
						mapCoord.row = num;
						return mapCoord;
					}
				}
			}
			int num5 = Math.Clamp(num2, item, item2);
			if (Math.Abs(num5 - current.coord.col) > 1)
			{
				num5 = Math.Clamp(current.coord.col + Math.Sign(num5 - current.coord.col), item, item2);
			}
			if (this.HasInvalidCrossover(current, num5))
			{
				int num6 = Math.Clamp(current.coord.col, item, item2);
				num5 = num6;
			}
			if (Math.Abs(num5 - current.coord.col) > 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Fallback step from (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(current.coord.col);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(current.coord.row);
				defaultInterpolatedStringHandler.AppendLiteral(") to column ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num5);
				defaultInterpolatedStringHandler.AppendLiteral(" exceeds adjacency");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new MapCoord
			{
				col = num5,
				row = num
			};
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x002A721C File Offset: 0x002A541C
		private List<int> BuildCenteredPriorityList(int currentCol, int centerCol)
		{
			List<int> list = new List<int>();
			int num = Math.Sign(centerCol - currentCol);
			if (num != 0)
			{
				list.Add(num);
			}
			list.Add(0);
			int num2 = -num;
			if (num != 0)
			{
				list.Add(num2);
			}
			if (!list.Contains(-1))
			{
				list.Add(-1);
			}
			if (!list.Contains(1))
			{
				list.Add(1);
			}
			return list;
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x002A7278 File Offset: 0x002A5478
		private int GetNextColumn(int currentCol, int direction)
		{
			int num;
			switch (direction)
			{
			case -1:
				num = Math.Max(0, currentCol - 1);
				break;
			case 0:
				num = currentCol;
				break;
			case 1:
				num = Math.Min(6, currentCol + 1);
				break;
			default:
				num = currentCol;
				break;
			}
			return num;
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x002A72BC File Offset: 0x002A54BC
		[NullableContext(0)]
		[return: TupleElementNames(new string[] { "minCol", "maxCol" })]
		private ValueTuple<int, int> GetAllowedColumnsForRow(int row)
		{
			int num = base.GetColumnCount() / 2;
			int num2 = Math.Abs(row - this._treasureRow);
			int num3 = this._mapLength - 1 - row;
			int num4 = Math.Min(num, Math.Max(0, num3) + 1);
			int num5 = Math.Min(num, Math.Min(num2, num4));
			int num6 = Math.Max(0, num - num5);
			int num7 = Math.Min(6, num + num5);
			return new ValueTuple<int, int>(num6, num7);
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x002A732C File Offset: 0x002A552C
		private bool HasInvalidCrossover(MapPoint current, int targetCol)
		{
			int num = targetCol - current.coord.col;
			if (num == 0)
			{
				return false;
			}
			MapPoint point = base.GetPoint(targetCol, current.coord.row);
			if (point == null)
			{
				return false;
			}
			foreach (MapPoint mapPoint in point.Children)
			{
				int num2 = mapPoint.coord.col - point.coord.col;
				if (num2 == -num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x002A73CC File Offset: 0x002A55CC
		private void RedirectToTreasure(MapPoint strayNode, MapPoint treasure)
		{
			foreach (MapPoint mapPoint in strayNode.parents.ToList<MapPoint>())
			{
				mapPoint.RemoveChildPoint(strayNode);
				mapPoint.AddChildPoint(treasure);
			}
			foreach (MapPoint mapPoint2 in strayNode.Children.ToList<MapPoint>())
			{
				strayNode.RemoveChildPoint(mapPoint2);
				treasure.AddChildPoint(mapPoint2);
			}
			this.Grid[strayNode.coord.col, strayNode.coord.row] = null;
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x002A749C File Offset: 0x002A569C
		private void ConnectRowToBoss()
		{
			int num = base.GetRowCount() - 1;
			for (int i = 0; i < base.GetColumnCount(); i++)
			{
				MapPoint mapPoint = this.Grid[i, num];
				if (mapPoint != null && !mapPoint.Children.Contains(this.BossMapPoint))
				{
					mapPoint.AddChildPoint(this.BossMapPoint);
				}
			}
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x002A74F4 File Offset: 0x002A56F4
		private void ConnectRowToStart()
		{
			for (int i = 0; i < base.GetColumnCount(); i++)
			{
				MapPoint mapPoint = this.Grid[i, 1];
				if (mapPoint != null && !this.StartingMapPoint.Children.Contains(mapPoint))
				{
					this.StartingMapPoint.AddChildPoint(mapPoint);
				}
			}
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x002A7544 File Offset: 0x002A5744
		private void AssignPointTypes()
		{
			this.ForEachInRow(base.GetRowCount() - 1, delegate(MapPoint p)
			{
				p.PointType = MapPointType.RestSite;
				p.CanBeModified = false;
			});
			this.ForEachInRow(1, delegate(MapPoint p)
			{
				p.PointType = MapPointType.Monster;
				p.CanBeModified = false;
			});
			List<MapPointType> list = new List<MapPointType>();
			for (int i = 0; i < this._pointTypeCounts.NumOfRests; i++)
			{
				list.Add(MapPointType.RestSite);
			}
			for (int j = 0; j < this._pointTypeCounts.NumOfShops; j++)
			{
				list.Add(MapPointType.Shop);
			}
			for (int k = 0; k < this._pointTypeCounts.NumOfElites; k++)
			{
				list.Add(MapPointType.Elite);
			}
			for (int l = 0; l < this._pointTypeCounts.NumOfUnknowns; l++)
			{
				list.Add(MapPointType.Unknown);
			}
			Queue<MapPointType> queue = new Queue<MapPointType>(list);
			this.AssignRemainingTypesToRandomPoints(queue);
			foreach (MapPoint mapPoint in from p in base.GetAllMapPoints()
				where p.PointType == MapPointType.Unassigned
				select p)
			{
				mapPoint.PointType = MapPointType.Monster;
			}
			this.BossMapPoint.PointType = MapPointType.Boss;
			this.StartingMapPoint.PointType = MapPointType.Ancient;
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x002A76B8 File Offset: 0x002A58B8
		private void ForEachInRow(int rowIndex, Action<MapPoint> processor)
		{
			for (int i = 0; i < base.GetColumnCount(); i++)
			{
				MapPoint mapPoint = this.Grid[i, rowIndex];
				if (mapPoint != null)
				{
					processor(mapPoint);
				}
			}
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x002A76F0 File Offset: 0x002A58F0
		private void AssignRemainingTypesToRandomPoints(Queue<MapPointType> pointTypesToBeAssigned)
		{
			int num = 0;
			while (num < 3 && pointTypesToBeAssigned.Count > 0)
			{
				List<MapPoint> list = (from p in base.GetAllMapPoints()
					where p != this.BossMapPoint && p != this.StartingMapPoint
					where p.PointType == MapPointType.Unassigned
					select p).ToList<MapPoint>();
				list.StableShuffle<MapPoint>(this._rng);
				foreach (MapPoint mapPoint in list)
				{
					if (pointTypesToBeAssigned.Count == 0)
					{
						break;
					}
					MapPointType nextValidPointType = this.GetNextValidPointType(pointTypesToBeAssigned, mapPoint);
					if (nextValidPointType != MapPointType.Unassigned)
					{
						mapPoint.PointType = nextValidPointType;
					}
				}
				num++;
			}
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x002A77C0 File Offset: 0x002A59C0
		private MapPointType GetNextValidPointType(Queue<MapPointType> pointTypesQueue, MapPoint mapPoint)
		{
			if (pointTypesQueue.Count == 0)
			{
				return MapPointType.Unassigned;
			}
			int count = pointTypesQueue.Count;
			for (int i = 0; i < count; i++)
			{
				MapPointType mapPointType = pointTypesQueue.Dequeue();
				if (this._pointTypeCounts.ShouldIgnoreMapPointRulesForMapPointType(mapPointType) || this.IsValidPointType(mapPointType, mapPoint))
				{
					return mapPointType;
				}
				pointTypesQueue.Enqueue(mapPointType);
			}
			return MapPointType.Unassigned;
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x002A7813 File Offset: 0x002A5A13
		private bool IsValidPointType(MapPointType pointType, MapPoint mapPoint)
		{
			return this.IsValidForUpper(pointType, mapPoint) && SpoilsActMap.IsValidForLower(pointType, mapPoint) && SpoilsActMap.IsValidWithParents(pointType, mapPoint) && SpoilsActMap.IsValidWithChildren(pointType, mapPoint) && SpoilsActMap.IsValidWithSiblings(pointType, mapPoint);
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x002A784E File Offset: 0x002A5A4E
		private static bool IsValidForLower(MapPointType pointType, MapPoint mapPoint)
		{
			return mapPoint.coord.row >= 6 || !SpoilsActMap._lowerMapPointRestrictions.Contains(pointType);
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x002A786E File Offset: 0x002A5A6E
		private bool IsValidForUpper(MapPointType pointType, MapPoint mapPoint)
		{
			return mapPoint.coord.row < this._mapLength - 3 || !SpoilsActMap._upperMapPointRestrictions.Contains(pointType);
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x002A7898 File Offset: 0x002A5A98
		private static bool IsValidWithParents(MapPointType pointType, MapPoint mapPoint)
		{
			return !SpoilsActMap._parentMapPointRestrictions.Contains(pointType) || !mapPoint.parents.Concat<MapPoint>(mapPoint.Children).Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x002A78EC File Offset: 0x002A5AEC
		private static bool IsValidWithChildren(MapPointType pointType, MapPoint mapPoint)
		{
			return !SpoilsActMap._childMapPointRestrictions.Contains(pointType) || !mapPoint.Children.Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x002A7934 File Offset: 0x002A5B34
		private static bool IsValidWithSiblings(MapPointType pointType, MapPoint mapPoint)
		{
			return !SpoilsActMap._siblingPointTypeRestrictions.Contains(pointType) || !SpoilsActMap.GetSiblings(mapPoint).Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x002A797C File Offset: 0x002A5B7C
		private static IEnumerable<MapPoint> GetSiblings(MapPoint mapPoint)
		{
			return from x in mapPoint.parents.SelectMany<MapPoint, MapPoint>((MapPoint x) => x.Children)
				where !object.Equals(x, mapPoint)
				select x;
		}

		// Token: 0x04002982 RID: 10626
		private const int _mapWidth = 7;

		// Token: 0x04002983 RID: 10627
		private const int _pathCount = 7;

		// Token: 0x04002987 RID: 10631
		private readonly int _mapLength;

		// Token: 0x04002988 RID: 10632
		private readonly Rng _rng;

		// Token: 0x04002989 RID: 10633
		private readonly int _treasureRow;

		// Token: 0x0400298A RID: 10634
		private readonly MapPointTypeCounts _pointTypeCounts;

		// Token: 0x0400298B RID: 10635
		private static readonly HashSet<MapPointType> _lowerMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.RestSite,
			MapPointType.Elite
		};

		// Token: 0x0400298C RID: 10636
		private static readonly HashSet<MapPointType> _upperMapPointRestrictions = new HashSet<MapPointType> { MapPointType.RestSite };

		// Token: 0x0400298D RID: 10637
		private static readonly HashSet<MapPointType> _parentMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.Elite,
			MapPointType.RestSite,
			MapPointType.Treasure,
			MapPointType.Shop
		};

		// Token: 0x0400298E RID: 10638
		private static readonly HashSet<MapPointType> _childMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.Elite,
			MapPointType.RestSite,
			MapPointType.Treasure,
			MapPointType.Shop
		};

		// Token: 0x0400298F RID: 10639
		private static readonly HashSet<MapPointType> _siblingPointTypeRestrictions = new HashSet<MapPointType>
		{
			MapPointType.RestSite,
			MapPointType.Monster,
			MapPointType.Unknown,
			MapPointType.Elite,
			MapPointType.Shop
		};
	}
}
