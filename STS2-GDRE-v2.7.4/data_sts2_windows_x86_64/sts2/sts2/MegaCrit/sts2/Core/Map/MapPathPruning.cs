using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// Detects and removes duplicate path segments from act maps.
	/// Both StandardActMap and SpoilsActMap use this after point type assignment
	/// to eliminate redundant paths that share the same sequence of point types.
	/// </summary>
	// Token: 0x02000BA1 RID: 2977
	[NullableContext(1)]
	[Nullable(0)]
	public static class MapPathPruning
	{
		/// <summary>
		/// Prunes duplicate segments, then repairs any point types that fell below their target
		/// count. Since repair can create new duplicates, loops until stable (up to 3 iterations).
		/// </summary>
		// Token: 0x06007C2C RID: 31788 RVA: 0x002A4E4C File Offset: 0x002A304C
		public static void PruneAndRepair([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, ActMap map, MapPointTypeCounts pointTypeCounts, Rng rng, Func<MapPointType, MapPoint, bool> isValidPointType)
		{
			for (int i = 0; i < 3; i++)
			{
				MapPathPruning.PruneDuplicateSegments(grid, startMapPoints, map.StartingMapPoint, rng);
				if (!MapPathPruning.RepairPrunedPointTypes(map, pointTypeCounts, rng, isValidPointType))
				{
					break;
				}
			}
		}

		/// <summary>
		/// After pruning removes nodes, some point types may fall below their target count.
		/// This repairs the map by replacing monsters with the missing types.
		/// </summary>
		/// <param name="map">The map to repair.</param>
		/// <param name="pointTypeCounts">Target counts for each point type.</param>
		/// <param name="rng">Random number generator used for replacement selection.</param>
		/// <param name="isValidPointType">Validates whether a type can be placed at a given point.</param>
		/// <returns>true if any repairs were made (indicating re-pruning may be needed)</returns>
		// Token: 0x06007C2D RID: 31789 RVA: 0x002A4E84 File Offset: 0x002A3084
		public static bool RepairPrunedPointTypes(ActMap map, MapPointTypeCounts pointTypeCounts, Rng rng, Func<MapPointType, MapPoint, bool> isValidPointType)
		{
			bool flag = false;
			flag |= MapPathPruning.RepairPointType(map, MapPointType.Shop, pointTypeCounts.NumOfShops, rng, isValidPointType);
			flag |= MapPathPruning.RepairPointType(map, MapPointType.Elite, pointTypeCounts.NumOfElites, rng, isValidPointType);
			flag |= MapPathPruning.RepairPointType(map, MapPointType.RestSite, pointTypeCounts.NumOfRests, rng, isValidPointType);
			return flag | MapPathPruning.RepairPointType(map, MapPointType.Unknown, pointTypeCounts.NumOfUnknowns, rng, isValidPointType);
		}

		// Token: 0x06007C2E RID: 31790 RVA: 0x002A4EDC File Offset: 0x002A30DC
		private static bool RepairPointType(ActMap map, MapPointType type, int targetCount, Rng rng, Func<MapPointType, MapPoint, bool> isValidPointType)
		{
			int num = map.GetAllMapPoints().Count<MapPoint>((MapPoint p) => p.PointType == type);
			int num2 = targetCount - num;
			if (num2 <= 0)
			{
				return false;
			}
			bool flag = false;
			List<MapPoint> list = (from p in map.GetAllMapPoints()
				where p.PointType == MapPointType.Monster && p.CanBeModified
				select p).ToList<MapPoint>();
			list.StableShuffle<MapPoint>(rng);
			foreach (MapPoint mapPoint in list)
			{
				if (num2 == 0)
				{
					break;
				}
				if (isValidPointType(type, mapPoint))
				{
					mapPoint.PointType = type;
					num2--;
					flag = true;
				}
			}
			return flag;
		}

		/// <summary>
		/// Runs the full prune loop (up to 50 iterations) to remove duplicate segments.
		/// </summary>
		// Token: 0x06007C2F RID: 31791 RVA: 0x002A4FBC File Offset: 0x002A31BC
		public static void PruneDuplicateSegments([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, MapPoint startingMapPoint, Rng rng)
		{
			int num = 0;
			List<List<MapPoint[]>> list = MapPathPruning.FindMatchingSegments(startingMapPoint);
			while (MapPathPruning.PrunePaths(grid, startMapPoints, list, rng))
			{
				num++;
				if (num > 50)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unable to prune matching segments in ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" iterations");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				list = MapPathPruning.FindMatchingSegments(startingMapPoint);
			}
		}

		// Token: 0x06007C30 RID: 31792 RVA: 0x002A5028 File Offset: 0x002A3228
		public static List<List<MapPoint[]>> FindMatchingSegments(MapPoint startingMapPoint)
		{
			List<List<MapPoint>> list = MapPathPruning.FindAllPaths(startingMapPoint);
			SortedDictionary<string, List<MapPoint[]>> sortedDictionary = new SortedDictionary<string, List<MapPoint[]>>(StringComparer.Ordinal);
			foreach (List<MapPoint> list2 in list)
			{
				MapPathPruning.AddSegmentsToDictionary(list2, sortedDictionary);
			}
			return MapPathPruning.GetDuplicateSegments(sortedDictionary);
		}

		// Token: 0x06007C31 RID: 31793 RVA: 0x002A5090 File Offset: 0x002A3290
		public unsafe static List<List<MapPoint>> FindAllPaths(MapPoint currentMapPoint)
		{
			List<List<MapPoint>> list = new List<List<MapPoint>>();
			if (currentMapPoint.PointType == MapPointType.Boss)
			{
				List<List<MapPoint>> list2 = list;
				int num = 1;
				List<MapPoint> list3 = new List<MapPoint>(num);
				CollectionsMarshal.SetCount<MapPoint>(list3, num);
				Span<MapPoint> span = CollectionsMarshal.AsSpan<MapPoint>(list3);
				int num2 = 0;
				*span[num2] = currentMapPoint;
				list2.Add(list3);
				return list;
			}
			foreach (MapPoint mapPoint in currentMapPoint.Children)
			{
				List<List<MapPoint>> list4 = MapPathPruning.FindAllPaths(mapPoint);
				foreach (List<MapPoint> list5 in list4)
				{
					int num2 = 1;
					List<MapPoint> list6 = new List<MapPoint>(num2);
					CollectionsMarshal.SetCount<MapPoint>(list6, num2);
					Span<MapPoint> span = CollectionsMarshal.AsSpan<MapPoint>(list6);
					int num = 0;
					*span[num] = currentMapPoint;
					List<MapPoint> list7 = list6;
					list7.AddRange(list5);
					list.Add(list7);
				}
			}
			return list;
		}

		// Token: 0x06007C32 RID: 31794 RVA: 0x002A5190 File Offset: 0x002A3390
		private unsafe static void AddSegmentsToDictionary(IReadOnlyList<MapPoint> path, IDictionary<string, List<MapPoint[]>> segments)
		{
			for (int i = 0; i < path.Count - 1; i++)
			{
				if (MapPathPruning.IsValidSegmentStartMapPoint(path[i]))
				{
					for (int j = 2; j < path.Count - i; j++)
					{
						MapPoint mapPoint = path[i + j];
						if (MapPathPruning.IsValidSegmentEndMapPoint(mapPoint))
						{
							MapPoint[] array = path.Skip<MapPoint>(i).Take<MapPoint>(j + 1).ToArray<MapPoint>();
							string text = MapPathPruning.GenerateSegmentKey(array);
							if (!segments.ContainsKey(text))
							{
								string text2 = text;
								int num = 1;
								List<MapPoint[]> list = new List<MapPoint[]>(num);
								CollectionsMarshal.SetCount<MapPoint[]>(list, num);
								Span<MapPoint[]> span = CollectionsMarshal.AsSpan<MapPoint[]>(list);
								int num2 = 0;
								*span[num2] = array;
								segments[text2] = list;
							}
							else if (!MapPathPruning.AnyOverlappingSegments(segments[text], array))
							{
								segments[text].Add(array);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C33 RID: 31795 RVA: 0x002A5269 File Offset: 0x002A3469
		private static bool IsValidSegmentStartMapPoint(MapPoint startMapPoint)
		{
			return startMapPoint.Children.Count > 1 || startMapPoint.coord.row == 0;
		}

		// Token: 0x06007C34 RID: 31796 RVA: 0x002A5289 File Offset: 0x002A3489
		private static bool IsValidSegmentEndMapPoint(MapPoint endMapPoint)
		{
			return endMapPoint.parents.Count >= 2;
		}

		// Token: 0x06007C35 RID: 31797 RVA: 0x002A529C File Offset: 0x002A349C
		private static string GenerateSegmentKey(IReadOnlyList<MapPoint> segment)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MapPoint mapPoint = segment[0];
			MapPoint mapPoint2 = segment[segment.Count - 1];
			if (mapPoint.coord.row == 0)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 3, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint.coord.row);
				appendInterpolatedStringHandler.AppendLiteral("-");
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint2.coord.col);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint2.coord.row);
				appendInterpolatedStringHandler.AppendLiteral("-");
				stringBuilder3.Append(ref appendInterpolatedStringHandler);
			}
			else
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 4, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint.coord.col);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint.coord.row);
				appendInterpolatedStringHandler.AppendLiteral("-");
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint2.coord.col);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<int>(mapPoint2.coord.row);
				appendInterpolatedStringHandler.AppendLiteral("-");
				stringBuilder4.Append(ref appendInterpolatedStringHandler);
			}
			stringBuilder.Append(string.Join<int>(",", segment.Select<MapPoint, int>((MapPoint point) => (int)point.PointType)));
			return stringBuilder.ToString();
		}

		// Token: 0x06007C36 RID: 31798 RVA: 0x002A5410 File Offset: 0x002A3610
		private static bool AnyOverlappingSegments(IEnumerable<MapPoint[]> existingSegments, IReadOnlyList<MapPoint> segment)
		{
			return existingSegments.Any<MapPoint[]>((MapPoint[] existingSegment) => MapPathPruning.OverlappingSegment(existingSegment, segment));
		}

		// Token: 0x06007C37 RID: 31799 RVA: 0x002A543C File Offset: 0x002A363C
		private static bool OverlappingSegment(IReadOnlyList<MapPoint> a, IReadOnlyList<MapPoint> b)
		{
			if (a.Count < 3 || b.Count < 3)
			{
				return false;
			}
			for (int i = 1; i <= a.Count - 2; i++)
			{
				if (object.Equals(a[i], b[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C38 RID: 31800 RVA: 0x002A5488 File Offset: 0x002A3688
		private static List<List<MapPoint[]>> GetDuplicateSegments(IDictionary<string, List<MapPoint[]>> segments)
		{
			return segments.Values.Where<List<MapPoint[]>>((List<MapPoint[]> segmentList) => segmentList.Count > 1).ToList<List<MapPoint[]>>();
		}

		// Token: 0x06007C39 RID: 31801 RVA: 0x002A54BC File Offset: 0x002A36BC
		private static bool PrunePaths([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, IEnumerable<List<MapPoint[]>> matchingSegments, Rng rng)
		{
			foreach (List<MapPoint[]> list in matchingSegments)
			{
				list.UnstableShuffle<MapPoint[]>(rng);
				int num = MapPathPruning.PruneAllButLast(grid, startMapPoints, list);
				if (num != 0)
				{
					return true;
				}
				if (MapPathPruning.BreakAParentChildRelationshipInAnySegment(list))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C3A RID: 31802 RVA: 0x002A5524 File Offset: 0x002A3724
		private static int PruneAllButLast([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, IReadOnlyList<MapPoint[]> matches)
		{
			int num = 0;
			foreach (MapPoint[] array in matches)
			{
				if (num == matches.Count - 1)
				{
					return num;
				}
				if (MapPathPruning.PruneSegment(grid, startMapPoints, array))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x002A5588 File Offset: 0x002A3788
		private static bool PruneSegment([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, MapPoint[] segment)
		{
			bool flag = false;
			Func<MapPoint, bool> <>9__0;
			Func<MapPoint, bool> <>9__2;
			for (int i = 0; i < segment.Length - 1; i++)
			{
				MapPoint mapPoint = segment[i];
				if (!MapPathPruning.IsInMap(grid, mapPoint))
				{
					return true;
				}
				if (mapPoint.Children.Count <= 1 && mapPoint.parents.Count <= 1)
				{
					IEnumerable<MapPoint> parents = mapPoint.parents;
					Func<MapPoint, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (MapPoint n) => n.Children.Count == 1 && !MapPathPruning.IsRemoved(grid, n));
					}
					if (!parents.Any<MapPoint>(func))
					{
						MapPoint[] array = segment.Skip<MapPoint>(i).ToArray<MapPoint>();
						if (!array.Any<MapPoint>((MapPoint n) => n.Children.Count > 1 && n.parents.Count == 1))
						{
							MapPoint[] segment2 = segment;
							if (segment2[segment2.Length - 1].parents.Count == 1)
							{
								return false;
							}
							IEnumerable<MapPoint> children = mapPoint.Children;
							Func<MapPoint, bool> func2;
							if ((func2 = <>9__2) == null)
							{
								func2 = (<>9__2 = (MapPoint c) => !segment.Contains(c));
							}
							if (!children.Where<MapPoint>(func2).Any<MapPoint>((MapPoint c) => c.parents.Count == 1))
							{
								MapPathPruning.RemovePoint(grid, startMapPoints, mapPoint);
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x002A56F8 File Offset: 0x002A38F8
		private static void RemovePoint([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, HashSet<MapPoint> startMapPoints, MapPoint mapPoint)
		{
			grid[mapPoint.coord.col, mapPoint.coord.row] = null;
			startMapPoints.Remove(mapPoint);
			foreach (MapPoint mapPoint2 in mapPoint.Children.ToList<MapPoint>())
			{
				mapPoint.RemoveChildPoint(mapPoint2);
			}
			foreach (MapPoint mapPoint3 in mapPoint.parents.ToList<MapPoint>())
			{
				mapPoint3.RemoveChildPoint(mapPoint);
			}
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x002A57BC File Offset: 0x002A39BC
		private static bool IsInMap([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, MapPoint mapPoint)
		{
			return grid[mapPoint.coord.col, mapPoint.coord.row] != null || mapPoint.PointType == MapPointType.Ancient || mapPoint.PointType == MapPointType.Boss;
		}

		// Token: 0x06007C3E RID: 31806 RVA: 0x002A57F0 File Offset: 0x002A39F0
		private static bool IsRemoved([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, MapPoint mapPoint)
		{
			return grid[mapPoint.coord.col, mapPoint.coord.row] == null;
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x002A5814 File Offset: 0x002A3A14
		private static bool BreakAParentChildRelationshipInAnySegment(List<MapPoint[]> matches)
		{
			foreach (MapPoint[] array in matches)
			{
				if (MapPathPruning.BreakAParentChildRelationshipInSegment(array))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x002A586C File Offset: 0x002A3A6C
		private static bool BreakAParentChildRelationshipInSegment(MapPoint[] segment)
		{
			bool flag = false;
			for (int i = 0; i < segment.Length - 1; i++)
			{
				MapPoint mapPoint = segment[i];
				if (mapPoint.Children.Count >= 2)
				{
					MapPoint mapPoint2 = segment[i + 1];
					if (mapPoint2.parents.Count != 1)
					{
						mapPoint.RemoveChildPoint(mapPoint2);
						flag = true;
					}
				}
			}
			return flag;
		}
	}
}
