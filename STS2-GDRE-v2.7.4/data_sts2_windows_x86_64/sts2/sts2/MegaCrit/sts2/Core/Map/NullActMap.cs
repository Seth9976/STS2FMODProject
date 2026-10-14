using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// An empty map used as a placeholder before a real map is generated.
	/// </summary>
	// Token: 0x02000BAA RID: 2986
	[NullableContext(1)]
	[Nullable(0)]
	public class NullActMap : ActMap
	{
		// Token: 0x17002278 RID: 8824
		// (get) Token: 0x06007C7E RID: 31870 RVA: 0x002A68F5 File Offset: 0x002A4AF5
		public static NullActMap Instance { get; } = new NullActMap();

		// Token: 0x17002279 RID: 8825
		// (get) Token: 0x06007C7F RID: 31871 RVA: 0x002A68FC File Offset: 0x002A4AFC
		public override MapPoint BossMapPoint { get; } = new MapPoint(0, 0);

		// Token: 0x1700227A RID: 8826
		// (get) Token: 0x06007C80 RID: 31872 RVA: 0x002A6904 File Offset: 0x002A4B04
		public override MapPoint StartingMapPoint { get; } = new MapPoint(0, 0);

		// Token: 0x1700227B RID: 8827
		// (get) Token: 0x06007C81 RID: 31873 RVA: 0x002A690C File Offset: 0x002A4B0C
		[Nullable(new byte[] { 1, 2 })]
		protected override MapPoint[,] Grid
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get;
		} = new MapPoint[0, 0];
	}
}
