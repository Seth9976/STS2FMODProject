using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// A map specifically for tests that allows you to specify the size and contents of the map.
	/// </summary>
	// Token: 0x02000BA8 RID: 2984
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockCraftedActMap : ActMap
	{
		// Token: 0x17002272 RID: 8818
		// (get) Token: 0x06007C73 RID: 31859 RVA: 0x002A6809 File Offset: 0x002A4A09
		public override MapPoint BossMapPoint { get; }

		// Token: 0x17002273 RID: 8819
		// (get) Token: 0x06007C74 RID: 31860 RVA: 0x002A6811 File Offset: 0x002A4A11
		public override MapPoint StartingMapPoint { get; }

		// Token: 0x17002274 RID: 8820
		// (get) Token: 0x06007C75 RID: 31861 RVA: 0x002A6819 File Offset: 0x002A4A19
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x002A6821 File Offset: 0x002A4A21
		public MockCraftedActMap(int width, int height, MapPoint startingPoint, MapPoint bossPoint)
		{
			this.Grid = new MapPoint[width, height];
			this.StartingMapPoint = startingPoint;
			this.BossMapPoint = bossPoint;
		}

		/// <summary>
		/// Creates a new point with the specified type at the specified coordinates.
		/// </summary>
		// Token: 0x06007C77 RID: 31863 RVA: 0x002A6848 File Offset: 0x002A4A48
		public void Put(int col, int row, MapPointType type = MapPointType.Monster)
		{
			MapPoint mapPoint = new MapPoint(col, row)
			{
				PointType = type
			};
			this.Grid[col, row] = mapPoint;
		}
	}
}
