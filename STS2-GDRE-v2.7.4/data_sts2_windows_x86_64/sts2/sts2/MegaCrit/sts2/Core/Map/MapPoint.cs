using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Map
{
	// Token: 0x02000BA2 RID: 2978
	[NullableContext(1)]
	[Nullable(0)]
	public class MapPoint : IComparable<MapPoint>
	{
		/// <summary>
		/// Is used to let Models that modify a map after it is generated to ensure that we don't modify this point.
		/// Is set by the ActMap if false (ie StandardActMap.AssignPointTypes).
		/// </summary>
		// Token: 0x17002269 RID: 8809
		// (get) Token: 0x06007C41 RID: 31809 RVA: 0x002A58BB File Offset: 0x002A3ABB
		// (set) Token: 0x06007C42 RID: 31810 RVA: 0x002A58C3 File Offset: 0x002A3AC3
		public bool CanBeModified { get; set; } = true;

		// Token: 0x1700226A RID: 8810
		// (get) Token: 0x06007C43 RID: 31811 RVA: 0x002A58CC File Offset: 0x002A3ACC
		// (set) Token: 0x06007C44 RID: 31812 RVA: 0x002A58D4 File Offset: 0x002A3AD4
		public MapPointType PointType { get; set; }

		// Token: 0x1700226B RID: 8811
		// (get) Token: 0x06007C45 RID: 31813 RVA: 0x002A58DD File Offset: 0x002A3ADD
		public IReadOnlyList<AbstractModel> Quests
		{
			get
			{
				return this._quests;
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06007C46 RID: 31814 RVA: 0x002A58E8 File Offset: 0x002A3AE8
		// (remove) Token: 0x06007C47 RID: 31815 RVA: 0x002A5920 File Offset: 0x002A3B20
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action NodeMarkedChanged;

		// Token: 0x06007C48 RID: 31816 RVA: 0x002A5955 File Offset: 0x002A3B55
		private bool Equals(MapPoint other)
		{
			return this.coord.Equals(other.coord);
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x002A5968 File Offset: 0x002A3B68
		[NullableContext(2)]
		public int CompareTo(MapPoint other)
		{
			if (other == null)
			{
				return 1;
			}
			return this.coord.CompareTo(other.coord);
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x002A5980 File Offset: 0x002A3B80
		public MapPoint(int col, int row)
		{
			this.coord.row = row;
			this.coord.col = col;
			this.parents = new HashSet<MapPoint>();
			this.Children = new HashSet<MapPoint>();
		}

		// Token: 0x1700226C RID: 8812
		// (get) Token: 0x06007C4B RID: 31819 RVA: 0x002A59D3 File Offset: 0x002A3BD3
		public HashSet<MapPoint> Children { get; }

		// Token: 0x06007C4C RID: 31820 RVA: 0x002A59DB File Offset: 0x002A3BDB
		public void AddChildPoint(MapPoint child)
		{
			this.Children.Add(child);
			child.parents.Add(this);
		}

		// Token: 0x06007C4D RID: 31821 RVA: 0x002A59F7 File Offset: 0x002A3BF7
		public void RemoveChildPoint(MapPoint child)
		{
			this.Children.Remove(child);
			child.parents.Remove(this);
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x002A5A14 File Offset: 0x002A3C14
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Point[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.coord.col);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.coord.row);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x002A5A7C File Offset: 0x002A3C7C
		[NullableContext(2)]
		public MapPoint LeftChild()
		{
			foreach (MapPoint mapPoint in this.Children)
			{
				if (mapPoint.IsAdjacentLeft(this))
				{
					return mapPoint;
				}
			}
			return null;
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x002A5AD8 File Offset: 0x002A3CD8
		[NullableContext(2)]
		public MapPoint RightChild()
		{
			foreach (MapPoint mapPoint in this.Children)
			{
				if (mapPoint.IsAdjacentRight(this))
				{
					return mapPoint;
				}
			}
			return null;
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x002A5B34 File Offset: 0x002A3D34
		public bool IsAdjacentLeft(MapPoint sibling)
		{
			return this.coord.col - 1 == sibling.coord.col;
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x002A5B50 File Offset: 0x002A3D50
		public bool IsAdjacentRight(MapPoint sibling)
		{
			return this.coord.col + 1 == sibling.coord.col;
		}

		// Token: 0x06007C53 RID: 31827 RVA: 0x002A5B6C File Offset: 0x002A3D6C
		public bool IsToTheLeft(MapPoint sibling)
		{
			return this.coord.col < sibling.coord.col;
		}

		// Token: 0x06007C54 RID: 31828 RVA: 0x002A5B86 File Offset: 0x002A3D86
		public bool IsToTheRight(MapPoint sibling)
		{
			return this.coord.col > sibling.coord.col;
		}

		// Token: 0x06007C55 RID: 31829 RVA: 0x002A5BA0 File Offset: 0x002A3DA0
		public bool IsInTheSameRow(MapPoint sibling)
		{
			return sibling.coord.row == this.coord.row;
		}

		// Token: 0x06007C56 RID: 31830 RVA: 0x002A5BBC File Offset: 0x002A3DBC
		[return: Nullable(2)]
		public MapPoint GetFirstCommonDescendant(MapPoint b)
		{
			if (this.Equals(b))
			{
				return b;
			}
			HashSet<MapPoint> allDescendants = this.GetAllDescendants();
			Queue<MapPoint> queue = new Queue<MapPoint>();
			queue.Enqueue(b);
			while (queue.Count > 0)
			{
				b = queue.Dequeue();
				if (allDescendants.Contains(b))
				{
					return b;
				}
				foreach (MapPoint mapPoint in b.Children)
				{
					queue.Enqueue(mapPoint);
				}
			}
			return null;
		}

		// Token: 0x06007C57 RID: 31831 RVA: 0x002A5C50 File Offset: 0x002A3E50
		private HashSet<MapPoint> GetAllDescendants()
		{
			HashSet<MapPoint> hashSet = new HashSet<MapPoint>();
			Queue<MapPoint> queue = new Queue<MapPoint>();
			queue.Enqueue(this);
			while (queue.Count > 0)
			{
				MapPoint mapPoint = queue.Dequeue();
				if (hashSet.Add(mapPoint))
				{
					foreach (MapPoint mapPoint2 in mapPoint.Children)
					{
						queue.Enqueue(mapPoint2);
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06007C58 RID: 31832 RVA: 0x002A5CD4 File Offset: 0x002A3ED4
		[return: Nullable(2)]
		public MapPoint GetCommonAncestor(MapPoint b)
		{
			if (this.coord.Equals(b.coord))
			{
				return b;
			}
			HashSet<MapPoint> allAncestors = this.GetAllAncestors();
			Queue<MapPoint> queue = new Queue<MapPoint>();
			queue.Enqueue(b);
			while (queue.Count > 0)
			{
				b = queue.Dequeue();
				if (allAncestors.Contains(b))
				{
					return b;
				}
				foreach (MapPoint mapPoint in b.parents)
				{
					queue.Enqueue(mapPoint);
				}
			}
			return null;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x002A5D70 File Offset: 0x002A3F70
		public int GetLastJunctionLength()
		{
			Queue<MapPoint> queue = new Queue<MapPoint>();
			queue.Enqueue(this);
			while (queue.Count > 0)
			{
				MapPoint mapPoint = queue.Dequeue();
				if (mapPoint.Children.Count > 1)
				{
					return this.coord.row - mapPoint.coord.row;
				}
				foreach (MapPoint mapPoint2 in mapPoint.parents)
				{
					queue.Enqueue(mapPoint2);
				}
			}
			return this.coord.row;
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x002A5E14 File Offset: 0x002A4014
		public HashSet<MapPoint> GetAllAncestors()
		{
			HashSet<MapPoint> hashSet = new HashSet<MapPoint>();
			Queue<MapPoint> queue = new Queue<MapPoint>();
			queue.Enqueue(this);
			while (queue.Count > 0)
			{
				MapPoint mapPoint = queue.Dequeue();
				if (hashSet.Add(mapPoint))
				{
					foreach (MapPoint mapPoint2 in mapPoint.parents)
					{
						queue.Enqueue(mapPoint2);
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x002A5E98 File Offset: 0x002A4098
		public IEnumerable<MapPoint> BFS_FindPath(MapPoint target)
		{
			Queue<MapPoint> queue = new Queue<MapPoint>();
			Dictionary<MapPoint, MapPoint> dictionary = new Dictionary<MapPoint, MapPoint>();
			queue.Enqueue(this);
			while (queue.Count > 0)
			{
				MapPoint mapPoint = queue.Dequeue();
				if (mapPoint.Equals(target))
				{
					return this.BuildPath(dictionary, target);
				}
				foreach (MapPoint mapPoint2 in mapPoint.Children)
				{
					if (!dictionary.ContainsKey(mapPoint2))
					{
						dictionary[mapPoint2] = mapPoint;
						queue.Enqueue(mapPoint2);
					}
				}
			}
			return new List<MapPoint>();
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x002A5F3C File Offset: 0x002A413C
		private IEnumerable<MapPoint> BuildPath(IReadOnlyDictionary<MapPoint, MapPoint> parentPoint, MapPoint target)
		{
			List<MapPoint> list = new List<MapPoint>();
			MapPoint mapPoint = target;
			while (!this.Equals(mapPoint))
			{
				list.Add(mapPoint);
				mapPoint = parentPoint[mapPoint];
			}
			list.Add(this);
			list.Reverse();
			return list;
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x002A5F7C File Offset: 0x002A417C
		[NullableContext(2)]
		public bool IsDescendantPathSame(MapPoint other)
		{
			if (other == null)
			{
				return false;
			}
			MapPoint firstCommonDescendant = this.GetFirstCommonDescendant(other);
			if (firstCommonDescendant == null)
			{
				return false;
			}
			MapPointType[] array = (from n in this.BFS_FindPath(firstCommonDescendant)
				where n.PointType > MapPointType.Unassigned
				select n.PointType).ToArray<MapPointType>();
			MapPointType[] array2 = (from n in other.BFS_FindPath(firstCommonDescendant)
				where n.PointType > MapPointType.Unassigned
				select n.PointType).ToArray<MapPointType>();
			if (array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x002A6068 File Offset: 0x002A4268
		public void AddQuest(AbstractModel model)
		{
			this._quests.Add(model);
			Action nodeMarkedChanged = this.NodeMarkedChanged;
			if (nodeMarkedChanged == null)
			{
				return;
			}
			nodeMarkedChanged();
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x002A6086 File Offset: 0x002A4286
		public void RemoveQuest(AbstractModel model)
		{
			this._quests.Remove(model);
			Action nodeMarkedChanged = this.NodeMarkedChanged;
			if (nodeMarkedChanged == null)
			{
				return;
			}
			nodeMarkedChanged();
		}

		// Token: 0x04002958 RID: 10584
		private readonly List<AbstractModel> _quests = new List<AbstractModel>();

		// Token: 0x04002959 RID: 10585
		public readonly HashSet<MapPoint> parents;

		// Token: 0x0400295A RID: 10586
		public MapCoord coord;
	}
}
