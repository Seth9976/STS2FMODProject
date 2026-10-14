using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// An ActMap that is reconstructed from saved data rather than generated from RNG.
	/// This ensures map topology is preserved even if map generation code changes.
	///
	/// This single class handles all map types (Standard, Spoils, GoldenPath) because
	/// the concrete type only matters during generation. After generation, all maps
	/// are simply grids of MapPoints with connections. The serialization captures
	/// the complete topology, making the original generation algorithm irrelevant
	/// for restoration.
	/// </summary>
	// Token: 0x02000BAB RID: 2987
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SavedActMap : ActMap
	{
		// Token: 0x1700227C RID: 8828
		// (get) Token: 0x06007C84 RID: 31876 RVA: 0x002A694F File Offset: 0x002A4B4F
		public override MapPoint BossMapPoint { get; }

		// Token: 0x1700227D RID: 8829
		// (get) Token: 0x06007C85 RID: 31877 RVA: 0x002A6957 File Offset: 0x002A4B57
		public override MapPoint StartingMapPoint { get; }

		// Token: 0x1700227E RID: 8830
		// (get) Token: 0x06007C86 RID: 31878 RVA: 0x002A695F File Offset: 0x002A4B5F
		[Nullable(2)]
		public override MapPoint SecondBossMapPoint
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x1700227F RID: 8831
		// (get) Token: 0x06007C87 RID: 31879 RVA: 0x002A6967 File Offset: 0x002A4B67
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x002A6970 File Offset: 0x002A4B70
		public SavedActMap(SerializableActMap saved)
		{
			this.Grid = new MapPoint[saved.GridWidth, saved.GridHeight];
			Dictionary<MapCoord, MapPoint> dictionary = new Dictionary<MapCoord, MapPoint>();
			foreach (SerializableMapPoint serializableMapPoint in saved.Points)
			{
				MapPoint mapPoint = SavedActMap.CreatePoint(serializableMapPoint);
				this.Grid[serializableMapPoint.Coord.col, serializableMapPoint.Coord.row] = mapPoint;
				dictionary[serializableMapPoint.Coord] = mapPoint;
			}
			this.BossMapPoint = SavedActMap.CreatePoint(saved.BossPoint);
			dictionary[saved.BossPoint.Coord] = this.BossMapPoint;
			this.StartingMapPoint = SavedActMap.CreatePoint(saved.StartingPoint);
			dictionary[saved.StartingPoint.Coord] = this.StartingMapPoint;
			if (saved.SecondBossPoint != null)
			{
				this.SecondBossMapPoint = SavedActMap.CreatePoint(saved.SecondBossPoint);
				dictionary[saved.SecondBossPoint.Coord] = this.SecondBossMapPoint;
			}
			SavedActMap.WireChildren(saved.Points, dictionary);
			SavedActMap.WireChildren(saved.BossPoint, dictionary);
			SavedActMap.WireChildren(saved.StartingPoint, dictionary);
			if (saved.SecondBossPoint != null)
			{
				SavedActMap.WireChildren(saved.SecondBossPoint, dictionary);
			}
			if (saved.StartMapPointCoords != null)
			{
				foreach (MapCoord mapCoord in saved.StartMapPointCoords)
				{
					MapPoint mapPoint2;
					if (dictionary.TryGetValue(mapCoord, out mapPoint2))
					{
						this.startMapPoints.Add(mapPoint2);
					}
				}
			}
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x002A6B2C File Offset: 0x002A4D2C
		private static MapPoint CreatePoint(SerializableMapPoint saved)
		{
			return new MapPoint(saved.Coord.col, saved.Coord.row)
			{
				PointType = saved.PointType,
				CanBeModified = saved.CanBeModified
			};
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x002A6B64 File Offset: 0x002A4D64
		private static void WireChildren(IEnumerable<SerializableMapPoint> points, Dictionary<MapCoord, MapPoint> lookup)
		{
			foreach (SerializableMapPoint serializableMapPoint in points)
			{
				SavedActMap.WireChildren(serializableMapPoint, lookup);
			}
		}

		// Token: 0x06007C8B RID: 31883 RVA: 0x002A6BAC File Offset: 0x002A4DAC
		private static void WireChildren(SerializableMapPoint savedPoint, Dictionary<MapCoord, MapPoint> lookup)
		{
			if (savedPoint.ChildCoords == null)
			{
				return;
			}
			MapPoint mapPoint = lookup[savedPoint.Coord];
			foreach (MapCoord mapCoord in savedPoint.ChildCoords)
			{
				MapPoint mapPoint2;
				if (lookup.TryGetValue(mapCoord, out mapPoint2))
				{
					mapPoint.AddChildPoint(mapPoint2);
				}
			}
		}
	}
}
