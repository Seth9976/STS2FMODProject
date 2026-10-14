using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages;

namespace MegaCrit.Sts2.Core.Multiplayer.Quality
{
	/// <summary>
	/// Holds statistics about a particular connection to a network peer.
	/// See <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker" /> for how this class is generally used.
	/// </summary>
	// Token: 0x02000459 RID: 1113
	[NullableContext(1)]
	[Nullable(0)]
	public class ConnectionStats
	{
		/// <summary>
		/// The peer for which this object gathers statistics.
		/// </summary>
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x0600456A RID: 17770 RVA: 0x00216B1B File Offset: 0x00214D1B
		// (set) Token: 0x0600456B RID: 17771 RVA: 0x00216B23 File Offset: 0x00214D23
		public ulong PeerId { get; private set; }

		/// <summary>
		/// The estimated average round-trip time to the peer.
		/// </summary>
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x00216B2C File Offset: 0x00214D2C
		// (set) Token: 0x0600456D RID: 17773 RVA: 0x00216B34 File Offset: 0x00214D34
		public float PingMsec { get; private set; }

		/// <summary>
		/// The estimated average packet loss to the peer.
		/// Note that this is the sum of both outbound and inbound packet loss.
		/// </summary>
		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x00216B3D File Offset: 0x00214D3D
		// (set) Token: 0x0600456F RID: 17775 RVA: 0x00216B45 File Offset: 0x00214D45
		public float PacketLoss { get; private set; }

		/// <summary>
		/// The last time (msec) we received a packet from the peer.
		/// If the difference between the current time and this is a large value, then we might be about to time out.
		/// </summary>
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x00216B4E File Offset: 0x00214D4E
		// (set) Token: 0x06004571 RID: 17777 RVA: 0x00216B56 File Offset: 0x00214D56
		public ulong? LastReceivedTime { get; private set; }

		/// <summary>
		/// True if the remote end is currently loading, and we should not expect consistent packets.
		/// Used in conjunction with LastReceivedTime to determine whether we should show a "connection interrupted" overlay.
		/// </summary>
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06004572 RID: 17778 RVA: 0x00216B5F File Offset: 0x00214D5F
		// (set) Token: 0x06004573 RID: 17779 RVA: 0x00216B67 File Offset: 0x00214D67
		public bool RemoteIsLoading { get; private set; }

		// Token: 0x06004574 RID: 17780 RVA: 0x00216B70 File Offset: 0x00214D70
		public ConnectionStats(ulong peerId)
		{
			this.PeerId = peerId;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted("ConnectionStats");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(peerId);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			this._logger = new Logger(defaultInterpolatedStringHandler.ToStringAndClear(), LogType.Network);
		}

		/// <summary>
		/// Generates a heartbeat request to be sent to the peer.
		/// </summary>
		/// <param name="timeMsec">The current time in milliseconds.</param>
		// Token: 0x06004575 RID: 17781 RVA: 0x00216BE0 File Offset: 0x00214DE0
		public HeartbeatRequestMessage GenerateHeartbeat(ulong timeMsec)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Generating heartbeat ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._nextIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" for time ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(timeMsec);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int num = this._nextIndex % 20;
			HeartbeatStatus heartbeatStatus = new HeartbeatStatus
			{
				counter = this._nextIndex,
				sentMsec = timeMsec
			};
			HeartbeatStatus heartbeatStatus2 = heartbeatStatus;
			HeartbeatStatus? heartbeatStatus3 = this._statuses[num];
			if (heartbeatStatus3 != null)
			{
				heartbeatStatus = heartbeatStatus3.Value;
				if (heartbeatStatus.receivedMsec == null)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Heartbeat ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(heartbeatStatus3.Value.counter);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(heartbeatStatus3.Value.sentMsec);
					defaultInterpolatedStringHandler.AppendLiteral(") was never received, marking as lost");
					logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.OnPacketLost();
				}
			}
			this._statuses[num] = new HeartbeatStatus?(heartbeatStatus2);
			HeartbeatRequestMessage heartbeatRequestMessage = new HeartbeatRequestMessage
			{
				counter = this._nextIndex
			};
			this._nextIndex++;
			return heartbeatRequestMessage;
		}

		/// <summary>
		/// Call me when we receive a heartbeat message for the peer.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		/// <param name="timeMsec">The current time in milliseconds.</param>
		// Token: 0x06004576 RID: 17782 RVA: 0x00216D38 File Offset: 0x00214F38
		public void OnHeartbeatReceived(HeartbeatResponseMessage message, ulong timeMsec)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received heartbeat for ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.counter);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			int num = this._nextIndex - 20;
			if (message.counter < num || message.counter >= this._nextIndex)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Counter ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.counter);
				defaultInterpolatedStringHandler.AppendLiteral(" is less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" and greater than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._nextIndex);
				logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			int num2 = message.counter % 20;
			if (num2 >= this._statuses.Length)
			{
				return;
			}
			HeartbeatStatus valueOrDefault = this._statuses[num2].GetValueOrDefault();
			if (valueOrDefault.counter != message.counter)
			{
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Counter in message ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.counter);
				defaultInterpolatedStringHandler.AppendLiteral(" does not match counter at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(", which is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault.counter);
				logger3.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			if (valueOrDefault.receivedMsec != null)
			{
				Logger logger4 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Already received message for index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(message.counter);
				logger4.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			valueOrDefault.receivedMsec = new ulong?(timeMsec);
			this._statuses[num2] = new HeartbeatStatus?(valueOrDefault);
			if (!message.isLoading)
			{
				int num3 = (int)(valueOrDefault.receivedMsec.Value - valueOrDefault.sentMsec);
				this.PingMsec = Mathf.Lerp(this.PingMsec, (float)num3, 0.2f);
			}
			else
			{
				Log.Debug("Not updating ping because sender is loading", 2);
			}
			this.PacketLoss = Mathf.Lerp(this.PacketLoss, 0f, 0.2f);
			this.LastReceivedTime = new ulong?(valueOrDefault.receivedMsec.Value);
			this.RemoteIsLoading = message.isLoading;
		}

		/// <summary>
		/// Called when we're about to overwrite a heartbeat status in the ring buffer, and a response to it was never
		/// received.
		/// </summary>
		// Token: 0x06004577 RID: 17783 RVA: 0x00216F80 File Offset: 0x00215180
		private void OnPacketLost()
		{
			this.PacketLoss = Mathf.Lerp(this.PacketLoss, 1f, 0.2f);
		}

		// Token: 0x04001B3B RID: 6971
		public const int ringBufferSize = 20;

		// Token: 0x04001B3C RID: 6972
		private const float _weightedAverageFactor = 0.2f;

		/// <summary>
		/// A ring buffer of heartbeat statuses.
		/// Heartbeats use a monotonically increasing counter for identification. We use that index to overwrite old
		/// heartbeat statuses in this buffer for tracking.
		/// </summary>
		// Token: 0x04001B3D RID: 6973
		private HeartbeatStatus?[] _statuses = new HeartbeatStatus?[20];

		// Token: 0x04001B3E RID: 6974
		private int _nextIndex;

		/// <summary>
		/// Logger object for this connection
		/// </summary>
		// Token: 0x04001B3F RID: 6975
		private readonly Logger _logger;
	}
}
