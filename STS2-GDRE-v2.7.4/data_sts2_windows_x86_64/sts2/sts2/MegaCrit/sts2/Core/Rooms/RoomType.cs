using System;

namespace MegaCrit.Sts2.Core.Rooms
{
	/// <summary>
	/// <para>
	/// Represents a type of room that a player can be in. This is different from <see cref="T:MegaCrit.Sts2.Core.Map.MapPointType" /> in subtle but
	/// important ways.
	/// </para>
	///
	/// <para>
	/// A MapPointType represents a type of point on the map. When a player clicks on one of these points, a RoomType is
	/// "resolved". Most of the time, this resolution is straightforward:
	///
	/// <list type="bullet">
	///     <item>MapPointType.Monster always resolves to RoomType.Monster.</item>
	///     <item>MapPointType.Treasure always resolves to RoomType.Treasure.</item>
	///     <item>etc.</item>
	/// </list>
	///
	/// However, MapPointType.Unknown ("?" on the map) can resolve to RoomType.Event, RoomType.Monster, and others.
	/// Furthermore, the current RoomType can change without the MapPointType changing. For example, imagine this sequence:
	///
	/// <list type="number">
	///     <item>The player clicks on a ? map point (MapPointType.Unknown).</item>
	///     <item>The room type resolves to RoomType.Event.</item>
	///     <item>In the event, the player selects the "Start a Fight!" option.</item>
	///     <item>The room type changes to RoomType.Monster.</item>
	/// </list>
	///
	/// With this in mind, it's important to treat these two concepts as distinct.
	/// </para>
	/// </summary>
	// Token: 0x02000130 RID: 304
	public enum RoomType
	{
		// Token: 0x04000557 RID: 1367
		Unassigned,
		// Token: 0x04000558 RID: 1368
		Monster,
		// Token: 0x04000559 RID: 1369
		Elite,
		// Token: 0x0400055A RID: 1370
		Boss,
		// Token: 0x0400055B RID: 1371
		Treasure,
		// Token: 0x0400055C RID: 1372
		Shop,
		// Token: 0x0400055D RID: 1373
		Event,
		// Token: 0x0400055E RID: 1374
		RestSite,
		// Token: 0x0400055F RID: 1375
		Map
	}
}
