using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers
{
	/// <summary>
	/// Handler for a specific room type.
	/// </summary>
	// Token: 0x02000D73 RID: 3443
	[NullableContext(1)]
	public interface IRoomHandler : IHandler
	{
		/// <summary>The room types this handler can handle.</summary>
		// Token: 0x1700262C RID: 9772
		// (get) Token: 0x06008B0C RID: 35596
		RoomType[] HandledTypes { get; }
	}
}
