using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Map
{
	// Token: 0x02000BA7 RID: 2983
	public static class MapTravel
	{
		/// <summary>
		/// Get all the points on the map that are reachable from the specified point.
		/// </summary>
		/// <param name="runState">The state of the current run.</param>
		/// <param name="currentPoint">The current point that the players are at.</param>
		// Token: 0x06007C72 RID: 31858 RVA: 0x002A67E0 File Offset: 0x002A49E0
		[NullableContext(1)]
		public static IEnumerable<MapPoint> GetTravelablePointsFrom(IRunState runState, MapPoint currentPoint)
		{
			if (Hook.ShouldAllowFreeTravel(runState))
			{
				return runState.Map.GetPointsInRow(currentPoint.coord.row + 1);
			}
			return currentPoint.Children;
		}
	}
}
