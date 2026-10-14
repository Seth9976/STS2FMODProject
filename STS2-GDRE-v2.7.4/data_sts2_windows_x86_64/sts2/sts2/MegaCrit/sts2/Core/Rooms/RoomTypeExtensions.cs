using System;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x02000131 RID: 305
	public static class RoomTypeExtensions
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x0005CB00 File Offset: 0x0005AD00
		public static bool IsCombatRoom(this RoomType room)
		{
			return room - RoomType.Monster <= 2;
		}
	}
}
