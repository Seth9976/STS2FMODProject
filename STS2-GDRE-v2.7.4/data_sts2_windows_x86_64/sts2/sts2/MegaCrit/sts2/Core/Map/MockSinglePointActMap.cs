using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// A single-point map specifically used for tests.
	/// </summary>
	// Token: 0x02000BA9 RID: 2985
	[NullableContext(1)]
	[Nullable(0)]
	public class MockSinglePointActMap : ActMap
	{
		// Token: 0x17002275 RID: 8821
		// (get) Token: 0x06007C78 RID: 31864 RVA: 0x002A6872 File Offset: 0x002A4A72
		public override MapPoint BossMapPoint { get; } = new MapPoint(0, 0)
		{
			PointType = MapPointType.Boss
		};

		// Token: 0x17002276 RID: 8822
		// (get) Token: 0x06007C79 RID: 31865 RVA: 0x002A687A File Offset: 0x002A4A7A
		public override MapPoint StartingMapPoint { get; } = new MapPoint(0, 0)
		{
			PointType = MapPointType.Ancient
		};

		// Token: 0x17002277 RID: 8823
		// (get) Token: 0x06007C7A RID: 31866 RVA: 0x002A6882 File Offset: 0x002A4A82
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		} = new MapPoint[0, 0];

		// Token: 0x06007C7B RID: 31867 RVA: 0x002A688A File Offset: 0x002A4A8A
		public override MapPoint GetPoint(MapCoord coord)
		{
			return this._currentMapPoint;
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x002A6892 File Offset: 0x002A4A92
		public void MockCurrentMapPointType(MapPointType pointType)
		{
			this._currentMapPoint.PointType = pointType;
		}

		// Token: 0x04002976 RID: 10614
		private readonly MapPoint _currentMapPoint = new MapPoint(0, 0);
	}
}
