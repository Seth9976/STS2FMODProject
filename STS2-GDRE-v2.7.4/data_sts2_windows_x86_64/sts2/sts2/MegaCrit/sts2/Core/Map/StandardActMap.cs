using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Map
{
	// Token: 0x02000BAD RID: 2989
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StandardActMap : ActMap
	{
		// Token: 0x17002283 RID: 8835
		// (get) Token: 0x06007CA8 RID: 31912 RVA: 0x002A7AB0 File Offset: 0x002A5CB0
		public override MapPoint BossMapPoint { get; }

		// Token: 0x17002284 RID: 8836
		// (get) Token: 0x06007CA9 RID: 31913 RVA: 0x002A7AB8 File Offset: 0x002A5CB8
		public override MapPoint StartingMapPoint { get; }

		// Token: 0x17002285 RID: 8837
		// (get) Token: 0x06007CAA RID: 31914 RVA: 0x002A7AC0 File Offset: 0x002A5CC0
		[Nullable(2)]
		public override MapPoint SecondBossMapPoint
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Flags this map to generate elite nodes where the treasure nodes would be in the middle of the act map.
		/// Is set in the constructor, typically for the Warden Ascension (replace act 3 chests with elites)
		/// </summary>
		// Token: 0x17002286 RID: 8838
		// (get) Token: 0x06007CAB RID: 31915 RVA: 0x002A7AC8 File Offset: 0x002A5CC8
		public bool ShouldReplaceTreasureWithElites { get; }

		// Token: 0x17002287 RID: 8839
		// (get) Token: 0x06007CAC RID: 31916 RVA: 0x002A7AD0 File Offset: 0x002A5CD0
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		/// <summary>
		/// Creates a standard map used for most acts.
		/// </summary>
		/// <param name="mapRng">The rng for the map.</param>
		/// <param name="actModel">The act to generate the map for </param>
		/// <param name="isMultiplayer">In multiplayer, we remove one node from each map.</param>
		/// <param name="shouldReplaceTreasureWithElites">Replaces the treasure row with elites. This is for the Warden ascension.</param>
		/// <param name="hasSecondBoss">Whether this act has a second boss (Double Boss ascension mode)</param>
		/// <param name="mapPointTypeCountsOverride">Overrides the map point type counts from the map passed in</param>
		/// <param name="enablePruning">Used for tests, should always be true if used for gameplay logic</param>
		// Token: 0x06007CAD RID: 31917 RVA: 0x002A7AD8 File Offset: 0x002A5CD8
		public StandardActMap(Rng mapRng, ActModel actModel, bool isMultiplayer, bool shouldReplaceTreasureWithElites, bool hasSecondBoss = false, [Nullable(2)] MapPointTypeCounts mapPointTypeCountsOverride = null, bool enablePruning = true)
		{
			this._mapLength = actModel.GetNumberOfRooms(isMultiplayer) + 1;
			this.ShouldReplaceTreasureWithElites = shouldReplaceTreasureWithElites;
			this.Grid = new MapPoint[7, this._mapLength];
			this._rng = mapRng;
			this._pointTypeCounts = mapPointTypeCountsOverride ?? actModel.GetMapPointTypes(mapRng);
			this.BossMapPoint = new MapPoint(base.GetColumnCount() / 2, base.GetRowCount());
			this.StartingMapPoint = new MapPoint(base.GetColumnCount() / 2, 0);
			if (hasSecondBoss)
			{
				this.SecondBossMapPoint = new MapPoint(base.GetColumnCount() / 2, base.GetRowCount() + 1);
			}
			this.GenerateMap();
			this.AssignPointTypes();
			if (enablePruning)
			{
				MapPathPruning.PruneAndRepair(this.Grid, this.startMapPoints, this, this._pointTypeCounts, this._rng, new Func<MapPointType, MapPoint, bool>(this.IsValidPointType));
			}
			this.Grid = MapPostProcessing.CenterGrid(this.Grid);
			this.Grid = MapPostProcessing.SpreadAdjacentMapPoints(this.Grid);
			this.Grid = MapPostProcessing.StraightenPaths(this.Grid);
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x002A7BE8 File Offset: 0x002A5DE8
		public static StandardActMap CreateFor(RunState runState, bool replaceTreasureWithElites)
		{
			ulong seed = runState.Rng.Seed;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("act_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(runState.CurrentActIndex + 1);
			defaultInterpolatedStringHandler.AppendLiteral("_map");
			return new StandardActMap(new Rng(seed, defaultInterpolatedStringHandler.ToStringAndClear()), runState.Act, runState.Players.Count > 1, replaceTreasureWithElites, runState.Act.HasSecondBoss, null, true);
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x002A7C63 File Offset: 0x002A5E63
		private MapPoint GetOrCreateMapPoint(MapCoord coord)
		{
			return this.GetOrCreatePoint(coord.col, coord.row);
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x002A7C78 File Offset: 0x002A5E78
		private MapPoint GetOrCreatePoint(int col, int row)
		{
			MapPoint mapPoint = base.GetPoint(col, row);
			if (mapPoint != null)
			{
				return mapPoint;
			}
			mapPoint = new MapPoint(col, row);
			this.Grid[col, row] = mapPoint;
			return mapPoint;
		}

		// Token: 0x06007CB1 RID: 31921 RVA: 0x002A7CAC File Offset: 0x002A5EAC
		private void PathGenerate(MapPoint startingPoint)
		{
			MapPoint mapPoint = startingPoint;
			while (mapPoint.coord.row < this._mapLength - 1)
			{
				MapCoord mapCoord = this.GenerateNextCoord(mapPoint);
				MapPoint orCreateMapPoint = this.GetOrCreateMapPoint(mapCoord);
				mapPoint.AddChildPoint(orCreateMapPoint);
				mapPoint = orCreateMapPoint;
			}
		}

		// Token: 0x06007CB2 RID: 31922 RVA: 0x002A7CEC File Offset: 0x002A5EEC
		private unsafe MapCoord GenerateNextCoord(MapPoint current)
		{
			int col = current.coord.col;
			int num = Mathf.Max(0, col - 1);
			int num2 = Mathf.Min(col + 1, 6);
			int num3 = 3;
			List<int> list = new List<int>(num3);
			CollectionsMarshal.SetCount<int>(list, num3);
			Span<int> span = CollectionsMarshal.AsSpan<int>(list);
			int num4 = 0;
			*span[num4] = -1;
			num4++;
			*span[num4] = 0;
			num4++;
			*span[num4] = 1;
			List<int> list2 = list;
			list2.StableShuffle<int>(this._rng);
			foreach (int num5 in list2)
			{
				int num6 = current.coord.row + 1;
				switch (num5)
				{
				case -1:
					num4 = num;
					break;
				case 0:
					num4 = col;
					break;
				case 1:
					num4 = num2;
					break;
				default:
					throw new InvalidOperationException("This isn't possible");
				}
				int num7 = num4;
				if (!this.HasInvalidCrossover(current, num7))
				{
					MapCoord mapCoord = default(MapCoord);
					mapCoord.col = num7;
					mapCoord.row = num6;
					return mapCoord;
				}
			}
			throw new InvalidOperationException("Cannot find next node");
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x002A7E2C File Offset: 0x002A602C
		private bool HasInvalidCrossover(MapPoint current, int targetX)
		{
			int num = targetX - current.coord.col;
			bool flag = num == 0 || num == 7;
			if (flag)
			{
				return false;
			}
			MapPoint mapPoint = this.Grid[targetX, current.coord.row];
			if (mapPoint == null)
			{
				return false;
			}
			foreach (MapPoint mapPoint2 in mapPoint.Children)
			{
				int num2 = mapPoint2.coord.col - mapPoint.coord.col;
				if (num2 == -num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x002A7EE0 File Offset: 0x002A60E0
		private void GenerateMap()
		{
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
			StandardActMap.ForEachInRow(this.Grid, base.GetRowCount() - 1, delegate(MapPoint x)
			{
				x.AddChildPoint(this.BossMapPoint);
			}, false);
			if (this.SecondBossMapPoint != null)
			{
				this.BossMapPoint.AddChildPoint(this.SecondBossMapPoint);
			}
			StandardActMap.ForEachInRow(this.Grid, 1, delegate(MapPoint x)
			{
				this.StartingMapPoint.AddChildPoint(x);
			}, false);
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x002A7FA0 File Offset: 0x002A61A0
		private static void ForEachInRow([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, int rowIndex, Action<MapPoint> processor, bool canBeModified = false)
		{
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				MapPoint mapPoint = grid[i, rowIndex];
				if (mapPoint != null)
				{
					processor(mapPoint);
				}
			}
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x002A7FD4 File Offset: 0x002A61D4
		private void AssignPointTypes()
		{
			StandardActMap.ForEachInRow(this.Grid, base.GetRowCount() - 1, delegate(MapPoint p)
			{
				p.PointType = MapPointType.RestSite;
				p.CanBeModified = false;
			}, false);
			if (this.ShouldReplaceTreasureWithElites)
			{
				StandardActMap.ForEachInRow(this.Grid, base.GetRowCount() - 7, delegate(MapPoint p)
				{
					p.PointType = MapPointType.Elite;
					p.CanBeModified = false;
				}, false);
			}
			else
			{
				StandardActMap.ForEachInRow(this.Grid, base.GetRowCount() - 7, delegate(MapPoint p)
				{
					p.PointType = MapPointType.Treasure;
					p.CanBeModified = false;
				}, false);
			}
			StandardActMap.ForEachInRow(this.Grid, 1, delegate(MapPoint p)
			{
				p.PointType = MapPointType.Monster;
				p.CanBeModified = false;
			}, false);
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
			foreach (MapPoint mapPoint in from x in base.GetAllMapPoints()
				where x.PointType == MapPointType.Unassigned
				select x)
			{
				mapPoint.PointType = MapPointType.Monster;
			}
			this.BossMapPoint.PointType = MapPointType.Boss;
			this.StartingMapPoint.PointType = MapPointType.Ancient;
			if (this.SecondBossMapPoint != null)
			{
				this.SecondBossMapPoint.PointType = MapPointType.Boss;
			}
		}

		// Token: 0x06007CB7 RID: 31927 RVA: 0x002A81D8 File Offset: 0x002A63D8
		private void EnsureRowsContainsPointType(MapPointType pointType, List<List<MapPoint>> rows)
		{
			if (StandardActMap.RowsContainPointType(pointType, rows))
			{
				return;
			}
			Queue<MapPointType> queue = new Queue<MapPointType>();
			queue.Enqueue(pointType);
			this.AssignPointTypesToRandomRows(queue, rows);
		}

		// Token: 0x06007CB8 RID: 31928 RVA: 0x002A8204 File Offset: 0x002A6404
		private static bool RowsContainPointType(MapPointType pointType, IEnumerable<List<MapPoint>> rows)
		{
			Func<MapPoint, bool> <>9__1;
			return rows.SelectMany<List<MapPoint>, MapPoint>(delegate(List<MapPoint> row)
			{
				Func<MapPoint, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (MapPoint p) => p.PointType == pointType);
				}
				return row.Where<MapPoint>(func);
			}).Any<MapPoint>();
		}

		// Token: 0x06007CB9 RID: 31929 RVA: 0x002A8238 File Offset: 0x002A6438
		private List<List<MapPoint>> GetRows(int firstRow, int lastRow)
		{
			List<List<MapPoint>> list = new List<List<MapPoint>>();
			for (int i = firstRow; i <= lastRow; i++)
			{
				List<MapPoint> list2 = new List<MapPoint>();
				for (int j = 0; j < 7; j++)
				{
					MapPoint mapPoint = this.Grid[j, i];
					if (mapPoint != null)
					{
						list2.Add(mapPoint);
					}
				}
				list.Add(list2);
			}
			return list;
		}

		/// <summary>
		/// Assigns point types to the map by row to ensure that the point types are more evenly distributed.
		/// </summary>
		/// <param name="pointTypesToBeAssigned">Point types left to be assigned.</param>
		/// <param name="rows">Rows of map points to assign the types to.</param>
		// Token: 0x06007CBA RID: 31930 RVA: 0x002A828C File Offset: 0x002A648C
		private void AssignPointTypesToRandomRows(Queue<MapPointType> pointTypesToBeAssigned, List<List<MapPoint>> rows)
		{
			rows.UnstableShuffle<List<MapPoint>>(this._rng);
			foreach (List<MapPoint> list in rows)
			{
				list.StableShuffle<MapPoint>(this._rng);
				using (IEnumerator<MapPoint> enumerator2 = list.Where<MapPoint>((MapPoint r) => r.PointType == MapPointType.Unassigned).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						MapPoint mapPoint = enumerator2.Current;
						MapPointType nextValidPointType = this.GetNextValidPointType(pointTypesToBeAssigned, mapPoint);
						mapPoint.PointType = nextValidPointType;
					}
				}
			}
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x002A8358 File Offset: 0x002A6558
		private void AssignRemainingTypesToRandomPoints(Queue<MapPointType> pointTypesToBeAssigned)
		{
			int num = 0;
			while (num < 3 && pointTypesToBeAssigned.Count > 0)
			{
				List<MapPoint> list = (from p in base.GetAllMapPoints()
					where p.PointType == MapPointType.Unassigned
					select p).ToList<MapPoint>();
				list.StableShuffle<MapPoint>(this._rng);
				foreach (MapPoint mapPoint in list)
				{
					if (pointTypesToBeAssigned.Count == 0)
					{
						break;
					}
					mapPoint.PointType = this.GetNextValidPointType(pointTypesToBeAssigned, mapPoint);
				}
				num++;
			}
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x002A8410 File Offset: 0x002A6610
		private MapPointType GetNextValidPointType(Queue<MapPointType> pointTypesQueue, MapPoint mapPoint)
		{
			for (int i = 0; i < pointTypesQueue.Count; i++)
			{
				MapPointType mapPointType = pointTypesQueue.Dequeue();
				if (this._pointTypeCounts.ShouldIgnoreMapPointRulesForMapPointType(mapPointType))
				{
					return mapPointType;
				}
				if (this.IsValidPointType(mapPointType, mapPoint))
				{
					return mapPointType;
				}
				pointTypesQueue.Enqueue(mapPointType);
			}
			return MapPointType.Unassigned;
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x002A8459 File Offset: 0x002A6659
		public bool IsValidPointType(MapPointType pointType, MapPoint mapPoint)
		{
			return this.IsValidForUpper(pointType, mapPoint) && StandardActMap.IsValidForLower(pointType, mapPoint) && StandardActMap.IsValidWithParents(pointType, mapPoint) && StandardActMap.IsValidWithChildren(pointType, mapPoint) && StandardActMap.IsValidWithSiblings(pointType, mapPoint);
		}

		/// These point types cannot be in the lower part of the map
		// Token: 0x06007CBE RID: 31934 RVA: 0x002A8494 File Offset: 0x002A6694
		private static bool IsValidForLower(MapPointType pointType, MapPoint mapPoint)
		{
			return mapPoint.coord.row >= 6 || !StandardActMap._lowerMapPointRestrictions.Contains(pointType);
		}

		/// These point types cannot be in the upper part of the map
		// Token: 0x06007CBF RID: 31935 RVA: 0x002A84B4 File Offset: 0x002A66B4
		private bool IsValidForUpper(MapPointType pointType, MapPoint mapPoint)
		{
			return mapPoint.coord.row < this._mapLength - 3 || !StandardActMap._upperMapPointRestrictions.Contains(pointType);
		}

		/// These cannot be 2 times in a row.
		// Token: 0x06007CC0 RID: 31936 RVA: 0x002A84DC File Offset: 0x002A66DC
		private static bool IsValidWithParents(MapPointType pointType, MapPoint mapPoint)
		{
			return !StandardActMap._parentMapPointRestrictions.Contains(pointType) || !mapPoint.parents.Concat<MapPoint>(mapPoint.Children).Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		/// Checks if a point type is valid by ensuring it doesn't appear twice in a row among children
		// Token: 0x06007CC1 RID: 31937 RVA: 0x002A8530 File Offset: 0x002A6730
		private static bool IsValidWithChildren(MapPointType pointType, MapPoint mapPoint)
		{
			return !StandardActMap._childMapPointRestrictions.Contains(pointType) || !mapPoint.Children.Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		/// These point types cannot be siblings of each other.
		// Token: 0x06007CC2 RID: 31938 RVA: 0x002A8578 File Offset: 0x002A6778
		private static bool IsValidWithSiblings(MapPointType pointType, MapPoint mapPoint)
		{
			return !StandardActMap._siblingPointTypeRestrictions.Contains(pointType) || !StandardActMap.GetSiblings(mapPoint).Any<MapPoint>((MapPoint p) => pointType == p.PointType);
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x002A85C0 File Offset: 0x002A67C0
		private static IEnumerable<MapPoint> GetSiblings(MapPoint mapPoint)
		{
			return from x in mapPoint.parents.SelectMany<MapPoint, MapPoint>((MapPoint x) => x.Children)
				where !object.Equals(x, mapPoint)
				select x;
		}

		// Token: 0x04002990 RID: 10640
		public const int maxElites = 15;

		// Token: 0x04002991 RID: 10641
		private const int _iterations = 7;

		// Token: 0x04002992 RID: 10642
		private const int _mapWidth = 7;

		// Token: 0x04002993 RID: 10643
		private readonly MapPointTypeCounts _pointTypeCounts;

		// Token: 0x04002994 RID: 10644
		private readonly int _mapLength;

		// Token: 0x04002995 RID: 10645
		private readonly Rng _rng;

		// Token: 0x0400299B RID: 10651
		private static readonly HashSet<MapPointType> _lowerMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.RestSite,
			MapPointType.Elite
		};

		// Token: 0x0400299C RID: 10652
		private static readonly HashSet<MapPointType> _upperMapPointRestrictions = new HashSet<MapPointType> { MapPointType.RestSite };

		/// These cannot be 2 times in a row.
		// Token: 0x0400299D RID: 10653
		private static readonly HashSet<MapPointType> _parentMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.Elite,
			MapPointType.RestSite,
			MapPointType.Treasure,
			MapPointType.Shop
		};

		/// These cannot be 2 times in a row.
		// Token: 0x0400299E RID: 10654
		private static readonly HashSet<MapPointType> _childMapPointRestrictions = new HashSet<MapPointType>
		{
			MapPointType.Elite,
			MapPointType.RestSite,
			MapPointType.Treasure,
			MapPointType.Shop
		};

		/// Only these point types are allowed to be siblings
		// Token: 0x0400299F RID: 10655
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
