using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Quality
{
	/// <summary>
	/// Represents the status of a heartbeat message. See <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker" />.
	/// </summary>
	// Token: 0x0200045A RID: 1114
	public struct HeartbeatStatus
	{
		/// <summary>
		/// The heartbeat index this is tracking.
		/// </summary>
		// Token: 0x04001B45 RID: 6981
		public int counter;

		/// <summary>
		/// The time at which this heartbeat was sent.
		/// </summary>
		// Token: 0x04001B46 RID: 6982
		public ulong sentMsec;

		/// <summary>
		/// The time at which this heartbeat was received, or null if it has not been received.
		/// </summary>
		// Token: 0x04001B47 RID: 6983
		public ulong? receivedMsec;
	}
}
