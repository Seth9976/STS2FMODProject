using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Map
{
	// Token: 0x02000BA5 RID: 2981
	[NullableContext(1)]
	[Nullable(0)]
	public class MapPointTypeCounts
	{
		// Token: 0x1700226D RID: 8813
		// (get) Token: 0x06007C60 RID: 31840 RVA: 0x002A60A5 File Offset: 0x002A42A5
		// (set) Token: 0x06007C61 RID: 31841 RVA: 0x002A60AD File Offset: 0x002A42AD
		public HashSet<MapPointType> PointTypesThatIgnoreRules { get; set; } = new HashSet<MapPointType>();

		// Token: 0x1700226E RID: 8814
		// (get) Token: 0x06007C62 RID: 31842 RVA: 0x002A60B6 File Offset: 0x002A42B6
		// (set) Token: 0x06007C63 RID: 31843 RVA: 0x002A60BE File Offset: 0x002A42BE
		public int NumOfElites { get; set; } = (int)Math.Round((double)(5f * (AscensionHelper.HasAscension(AscensionLevel.SwarmingElites) ? 1.6f : 1f)));

		// Token: 0x1700226F RID: 8815
		// (get) Token: 0x06007C64 RID: 31844 RVA: 0x002A60C7 File Offset: 0x002A42C7
		public int NumOfShops { get; } = 3;

		// Token: 0x17002270 RID: 8816
		// (get) Token: 0x06007C65 RID: 31845 RVA: 0x002A60CF File Offset: 0x002A42CF
		public int NumOfUnknowns { get; }

		// Token: 0x17002271 RID: 8817
		// (get) Token: 0x06007C66 RID: 31846 RVA: 0x002A60D7 File Offset: 0x002A42D7
		public int NumOfRests { get; }

		/// <summary>
		/// Whether the map generation MapPointType assignment ignores the rules or follows the rules.
		/// </summary>
		// Token: 0x06007C67 RID: 31847 RVA: 0x002A60DF File Offset: 0x002A42DF
		public bool ShouldIgnoreMapPointRulesForMapPointType(MapPointType pointType)
		{
			return this.PointTypesThatIgnoreRules.Contains(pointType);
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x002A60ED File Offset: 0x002A42ED
		public static int StandardRandomUnknownCount(Rng rng)
		{
			return rng.NextGaussianInt(12, 1, 10, 14);
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x002A60FC File Offset: 0x002A42FC
		public MapPointTypeCounts(int unknownCount, int restCount)
		{
			this.NumOfUnknowns = unknownCount;
			this.NumOfRests = restCount;
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x002A6158 File Offset: 0x002A4358
		public MapPointTypeCounts(ActMap existingMap)
		{
			MapPoint[] array = existingMap.GetAllMapPoints().ToArray<MapPoint>();
			this.NumOfUnknowns = array.Count<MapPoint>((MapPoint p) => p.PointType == MapPointType.Unknown);
			this.NumOfRests = array.Count<MapPoint>((MapPoint p) => p.PointType == MapPointType.RestSite);
		}
	}
}
