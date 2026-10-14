using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// These helper methods make the map more clean looking and less cluttered together which should help with some of the
	/// complaints. Some might look like there's a lot of looping, but I profiled them and all these public static methods
	/// run together take 0 ms on my computer.
	/// </summary>
	// Token: 0x02000BA6 RID: 2982
	[NullableContext(1)]
	[Nullable(0)]
	public static class MapPostProcessing
	{
		/// <summary>
		/// Creates a copy of the given grid that is “centered” by shifting
		/// all the nodes left or right if two of the leftmost or rightmost columns are empty.
		/// The MapPoint objects are not modified; they are simply placed into different cells.
		/// </summary>
		/// <param name="grid">The original grid to be centered.</param>
		/// <returns>A grid (modified in place) with nodes moved to center the map.</returns>
		// Token: 0x06007C6B RID: 31851 RVA: 0x002A6208 File Offset: 0x002A4408
		[return: Nullable(new byte[] { 1, 2 })]
		public static MapPoint[,] CenterGrid([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid)
		{
			int length = grid.GetLength(0);
			int length2 = grid.GetLength(1);
			bool flag = MapPostProcessing.IsColumnEmpty(grid, 0) && MapPostProcessing.IsColumnEmpty(grid, 1);
			bool flag2 = MapPostProcessing.IsColumnEmpty(grid, length - 1) && MapPostProcessing.IsColumnEmpty(grid, length - 2);
			int num = 0;
			if (flag && !flag2)
			{
				num = -1;
			}
			else if (!flag && flag2)
			{
				num = 1;
			}
			if (num == 0)
			{
				return grid;
			}
			if (num > 0)
			{
				for (int i = 0; i < length2; i++)
				{
					for (int j = length - 1; j >= 0; j--)
					{
						MapPoint mapPoint = grid[j, i];
						grid[j, i] = null;
						int num2 = j + num;
						if (num2 < length)
						{
							grid[num2, i] = mapPoint;
							if (mapPoint != null)
							{
								mapPoint.coord.col = num2;
							}
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < length2; k++)
				{
					for (int l = 0; l < length; l++)
					{
						MapPoint mapPoint2 = grid[l, k];
						grid[l, k] = null;
						int num3 = l + num;
						if (num3 >= 0)
						{
							grid[num3, k] = mapPoint2;
							if (mapPoint2 != null)
							{
								mapPoint2.coord.col = num3;
							}
						}
					}
				}
			}
			return grid;
		}

		/// <summary>
		/// This method straightens out the path if there's a single line
		/// </summary>
		// Token: 0x06007C6C RID: 31852 RVA: 0x002A633C File Offset: 0x002A453C
		[return: Nullable(new byte[] { 1, 2 })]
		public static MapPoint[,] StraightenPaths([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid)
		{
			int length = grid.GetLength(0);
			int length2 = grid.GetLength(1);
			for (int i = 0; i < length2; i++)
			{
				for (int j = 0; j < length; j++)
				{
					MapPoint mapPoint = grid[j, i];
					if (mapPoint != null)
					{
						bool flag = mapPoint.parents.Count == 1 && mapPoint.Children.Count == 1;
						if (flag)
						{
							MapPoint mapPoint2 = mapPoint.parents.First<MapPoint>();
							MapPoint mapPoint3 = mapPoint.Children.First<MapPoint>();
							bool flag2 = mapPoint.coord.col < mapPoint3.coord.col && mapPoint.coord.col < mapPoint2.coord.col;
							bool flag3 = mapPoint.coord.col > mapPoint3.coord.col && mapPoint.coord.col > mapPoint2.coord.col;
							if (flag2 && j < length - 1)
							{
								int num = j + 1;
								if (grid[num, i] != null)
								{
									goto IL_015D;
								}
								mapPoint.coord.col = num;
								grid[j, i] = null;
								grid[num, i] = mapPoint;
							}
							if (flag3 && j > 0)
							{
								int num2 = j - 1;
								if (grid[num2, i] == null)
								{
									mapPoint.coord.col = num2;
									grid[j, i] = null;
									grid[num2, i] = mapPoint;
								}
							}
						}
					}
					IL_015D:;
				}
			}
			return grid;
		}

		/// <summary>
		/// Returns true if every cell in the given column is null.
		/// </summary>
		/// <param name="grid">The grid to check.</param>
		/// <param name="col">The column index.</param>
		/// <returns>True if the column is empty; otherwise, false.</returns>
		// Token: 0x06007C6D RID: 31853 RVA: 0x002A64C0 File Offset: 0x002A46C0
		private static bool IsColumnEmpty([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid, int col)
		{
			int length = grid.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				if (grid[col, i] != null)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns a set of column indices allowed for a neighbor at the specified column.
		/// Allowed positions are the same column, one column to the left, and one column to the right.
		/// </summary>
		/// <param name="column">The reference column.</param>
		/// <param name="totalColumns">Total number of columns in the grid.</param>
		/// <returns>A set of allowed column indices.</returns>
		// Token: 0x06007C6E RID: 31854 RVA: 0x002A64F0 File Offset: 0x002A46F0
		private static HashSet<int> GetNeighborAllowedPositions(int column, int totalColumns)
		{
			HashSet<int> hashSet = new HashSet<int>(3);
			for (int i = -1; i <= 1; i++)
			{
				int num = column + i;
				if (num >= 0 && num < totalColumns)
				{
					hashSet.Add(num);
				}
			}
			return hashSet;
		}

		/// <summary>
		/// Computes the set of allowed column indices for a given node based on its parents' and children’s positions.
		/// For each parent or child, the node must be in the same column or one column to the left or right.
		/// The final allowed set is the intersection of these constraints.
		/// </summary>
		/// <param name="node">The MapPoint to evaluate.</param>
		/// <param name="totalColumns">Total number of columns in the grid.</param>
		/// <returns>A set of allowed column indices.</returns>
		// Token: 0x06007C6F RID: 31855 RVA: 0x002A6528 File Offset: 0x002A4728
		private static HashSet<int> GetAllowedPositions(MapPoint node, int totalColumns)
		{
			HashSet<int> hashSet = new HashSet<int>(Enumerable.Range(0, totalColumns));
			foreach (MapPoint mapPoint in node.parents)
			{
				hashSet.IntersectWith(MapPostProcessing.GetNeighborAllowedPositions(mapPoint.coord.col, totalColumns));
			}
			foreach (MapPoint mapPoint2 in node.Children)
			{
				hashSet.IntersectWith(MapPostProcessing.GetNeighborAllowedPositions(mapPoint2.coord.col, totalColumns));
			}
			return hashSet;
		}

		/// <summary>
		/// Spreads adjacent MapPoints in the grid by moving nodes to positions that maximize the gap between neighbors,
		/// subject to connectivity constraints.
		/// </summary>
		/// <param name="grid">The grid containing MapPoints.</param>
		/// <returns>The transformed grid.</returns>
		// Token: 0x06007C70 RID: 31856 RVA: 0x002A65F0 File Offset: 0x002A47F0
		[return: Nullable(new byte[] { 1, 2 })]
		public static MapPoint[,] SpreadAdjacentMapPoints([Nullable(new byte[] { 1, 2 })] MapPoint[,] grid)
		{
			int length = grid.GetLength(0);
			int length2 = grid.GetLength(1);
			for (int i = 0; i < length2; i++)
			{
				List<MapPoint> list = new List<MapPoint>(length2);
				for (int j = 0; j < length; j++)
				{
					MapPoint mapPoint = grid[j, i];
					if (mapPoint != null)
					{
						list.Add(mapPoint);
					}
				}
				bool flag;
				do
				{
					flag = false;
					foreach (MapPoint mapPoint2 in list)
					{
						int col = mapPoint2.coord.col;
						HashSet<int> allowedPositions = MapPostProcessing.GetAllowedPositions(mapPoint2, length);
						int num = MapPostProcessing.ComputeGap(col, list, mapPoint2);
						int num2 = col;
						int num3 = num;
						foreach (int num4 in allowedPositions)
						{
							if (num4 != col && (grid[num4, i] == null || grid[num4, i] == mapPoint2))
							{
								int num5 = MapPostProcessing.ComputeGap(num4, list, mapPoint2);
								if (num5 > num3)
								{
									num2 = num4;
									num3 = num5;
								}
							}
						}
						if (num2 != col)
						{
							grid[col, i] = null;
							grid[num2, i] = mapPoint2;
							mapPoint2.coord.col = num2;
							flag = true;
						}
					}
				}
				while (flag);
			}
			return grid;
		}

		/// <summary>
		/// Computes the minimal horizontal gap from a candidate column to any other node in the same row.
		/// If there are no other nodes in the row, returns int.MaxValue.
		/// </summary>
		/// <param name="candidateCol">The candidate column for the node.</param>
		/// <param name="rowNodes">All nodes in the row.</param>
		/// <param name="currentNode">The node being moved.</param>
		/// <returns>The minimum column distance to any other node.</returns>
		// Token: 0x06007C71 RID: 31857 RVA: 0x002A6764 File Offset: 0x002A4964
		private static int ComputeGap(int candidateCol, List<MapPoint> rowNodes, MapPoint currentNode)
		{
			int num = int.MaxValue;
			foreach (MapPoint mapPoint in rowNodes)
			{
				if (mapPoint != currentNode)
				{
					num = Math.Min(num, Math.Abs(candidateCol - mapPoint.coord.col));
				}
			}
			if (num != 2147483647)
			{
				return num;
			}
			return int.MaxValue;
		}
	}
}
