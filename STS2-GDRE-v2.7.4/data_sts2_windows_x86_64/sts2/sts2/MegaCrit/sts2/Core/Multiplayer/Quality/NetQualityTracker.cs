using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages;

namespace MegaCrit.Sts2.Core.Multiplayer.Quality
{
	/// <summary>
	/// Keeps track of the quality of the connection to each peer, for use in displaying "poor connection" notifications to
	/// the user.
	///
	/// To estimate connection quality, the following strategy is used:
	/// - At regular intervals, a "heartbeat" message is generated and sent to each peer. We record the time at which the
	///   message is sent.
	/// - When it is received, the peer echoes the heartbeat message back to us.
	/// - When we receive the response, we check when it was sent, and adjust our estimated round-trip time (ping) to the
	///   peer accordingly (in <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Quality.ConnectionStats" />).
	/// - If we go some time without seeing the echoed message, then we assume the packet is lost. We adjust our estimated
	///   packet loss to the peer accordingly.
	///
	/// Further work can be done here to estimate jitter, but I don't think we need it for this game.
	/// </summary>
	// Token: 0x0200045B RID: 1115
	[NullableContext(1)]
	[Nullable(0)]
	public class NetQualityTracker : IDisposable
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x00216F9D File Offset: 0x0021519D
		public bool IsGameLoading
		{
			get
			{
				return this._isLoading;
			}
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x00216FA8 File Offset: 0x002151A8
		public NetQualityTracker(INetGameService netService)
		{
			this._netService = netService;
			netService.RegisterMessageHandler<HeartbeatRequestMessage>(new MessageHandlerDelegate<HeartbeatRequestMessage>(this.HandleHeartbeatRequestMessage));
			netService.RegisterMessageHandler<HeartbeatResponseMessage>(new MessageHandlerDelegate<HeartbeatResponseMessage>(this.HandleHeartbeatResponseMessage));
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x00217002 File Offset: 0x00215202
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<HeartbeatRequestMessage>(new MessageHandlerDelegate<HeartbeatRequestMessage>(this.HandleHeartbeatRequestMessage));
			this._netService.UnregisterMessageHandler<HeartbeatResponseMessage>(new MessageHandlerDelegate<HeartbeatResponseMessage>(this.HandleHeartbeatResponseMessage));
		}

		/// <summary>
		/// Gets the current time.
		/// Pulls from godot or a mock time source.
		/// </summary>
		// Token: 0x0600457B RID: 17787 RVA: 0x00217032 File Offset: 0x00215232
		private ulong GetCurrentTime()
		{
			Func<ulong> func = this.getTimeMsec;
			if (func == null)
			{
				return Time.GetTicksMsec();
			}
			return func();
		}

		/// <summary>
		/// Should be called whenever a peer connects. Begins tracking their connection statistics.
		/// </summary>
		// Token: 0x0600457C RID: 17788 RVA: 0x00217049 File Offset: 0x00215249
		public void OnPeerConnected(ulong peerId)
		{
			this._stats.Add(new ConnectionStats(peerId));
		}

		/// <summary>
		/// Should be called whenever a peer connects. Stops tracking their connection statistics.
		/// </summary>
		// Token: 0x0600457D RID: 17789 RVA: 0x0021705C File Offset: 0x0021525C
		public void OnPeerDisconnected(ulong peerId)
		{
			this._stats.RemoveAll((ConnectionStats s) => s.PeerId == peerId);
		}

		/// <summary>
		/// Called when the game enters or exits a loading state.
		/// This is important to know because, in the middle of loading, NetService.Update can be called at irregular
		/// intervals, leading to a much larger-than-normal round-trip time even though the network connection is stable.
		/// This info is sent through the HeartbeatResponseMessage, and when it is true, the receiver should not update
		/// ping, but still update last received time.
		/// </summary>
		/// <param name="isLoading">True if the game is in a loading state, false otherwise.</param>
		// Token: 0x0600457E RID: 17790 RVA: 0x00217090 File Offset: 0x00215290
		public void SetIsLoading(bool isLoading)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Loading set to ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(isLoading);
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._isLoading = isLoading;
		}

		/// <summary>
		/// Should be called on a regular basis to send heartbeat messages.
		/// </summary>
		// Token: 0x0600457F RID: 17791 RVA: 0x002170D0 File Offset: 0x002152D0
		public void Update()
		{
			ulong currentTime = this.GetCurrentTime();
			if (this._lastUpdateMsec == null)
			{
				this._lastUpdateMsec = new ulong?(currentTime);
			}
			else if (currentTime - this._lastUpdateMsec.Value >= 200UL)
			{
				foreach (ConnectionStats connectionStats in this._stats)
				{
					HeartbeatRequestMessage heartbeatRequestMessage = connectionStats.GenerateHeartbeat(currentTime);
					this._netService.SendMessage<HeartbeatRequestMessage>(heartbeatRequestMessage, connectionStats.PeerId);
				}
				this._lastUpdateMsec = new ulong?(currentTime);
			}
			if (this._logger.WillLog(LogLevel.Debug))
			{
				if (this._lastLogMsec == null)
				{
					this._lastLogMsec = new ulong?(currentTime);
					return;
				}
				ulong? num = currentTime - this._lastLogMsec;
				ulong num2 = 20000UL;
				if (((num.GetValueOrDefault() >= num2) & (num != null)) && this._stats.Count > 0)
				{
					this._lastLogMsec = new ulong?(currentTime);
					this._logger.Debug("Connection statistics at " + Log.Timestamp + ":", 1);
					foreach (ConnectionStats connectionStats2 in this._stats)
					{
						Logger logger = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
						defaultInterpolatedStringHandler.AppendLiteral("\t");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(connectionStats2.PeerId);
						defaultInterpolatedStringHandler.AppendLiteral(" - Ping: ");
						defaultInterpolatedStringHandler.AppendFormatted<float>(connectionStats2.PingMsec);
						defaultInterpolatedStringHandler.AppendLiteral(". Packet Loss: ");
						defaultInterpolatedStringHandler.AppendFormatted<float>(connectionStats2.PacketLoss);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					}
				}
			}
		}

		/// <summary>
		/// Called whenever a heartbeat request comes in.
		/// This just echoes the heartbeat response back to the sender.
		/// </summary>
		// Token: 0x06004580 RID: 17792 RVA: 0x002172F0 File Offset: 0x002154F0
		private void HandleHeartbeatRequestMessage(HeartbeatRequestMessage message, ulong senderId)
		{
			this._netService.SendMessage<HeartbeatResponseMessage>(new HeartbeatResponseMessage
			{
				counter = message.counter,
				isLoading = this._isLoading
			}, senderId);
		}

		/// <summary>
		/// Called whenever a response to our heartbeat request comes back.
		/// Processes the message and updates our statistics.
		/// </summary>
		// Token: 0x06004581 RID: 17793 RVA: 0x0021732C File Offset: 0x0021552C
		private void HandleHeartbeatResponseMessage(HeartbeatResponseMessage message, ulong senderId)
		{
			ConnectionStats statsForPeer = this.GetStatsForPeer(senderId);
			if (statsForPeer != null)
			{
				statsForPeer.OnHeartbeatReceived(message, this.GetCurrentTime());
			}
		}

		/// <summary>
		/// Returns the statistics for a given peer.
		/// </summary>
		// Token: 0x06004582 RID: 17794 RVA: 0x00217354 File Offset: 0x00215554
		[NullableContext(2)]
		public ConnectionStats GetStatsForPeer(ulong peerId)
		{
			foreach (ConnectionStats connectionStats in this._stats)
			{
				if (connectionStats.PeerId == peerId)
				{
					return connectionStats;
				}
			}
			return null;
		}

		/// <summary>
		/// The time that must elapse before sending another heartbeat packet.
		/// </summary>
		// Token: 0x04001B48 RID: 6984
		public const int sendRateMsec = 200;

		/// <summary>
		/// The time that must elapse before we log the connection statistics to the log.
		/// </summary>
		// Token: 0x04001B49 RID: 6985
		public const int logRateMsec = 20000;

		// Token: 0x04001B4A RID: 6986
		private readonly INetGameService _netService;

		// Token: 0x04001B4B RID: 6987
		private readonly Logger _logger = new Logger("NetQualityTracker", LogType.Network);

		/// <summary>
		/// Connection statistics objects, one per peer.
		/// </summary>
		// Token: 0x04001B4C RID: 6988
		private readonly List<ConnectionStats> _stats = new List<ConnectionStats>();

		/// <summary>
		/// The last time we sent a heartbeat packet.
		/// </summary>
		// Token: 0x04001B4D RID: 6989
		private ulong? _lastUpdateMsec;

		/// <summary>
		/// The last time we logged the connection statistics.
		/// </summary>
		// Token: 0x04001B4E RID: 6990
		private ulong? _lastLogMsec;

		/// <summary>
		/// True if the game is in a loading state. See <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker.SetIsLoading(System.Boolean)" />.
		/// </summary>
		// Token: 0x04001B4F RID: 6991
		private bool _isLoading;

		/// <summary>
		/// This is called when to get the current time. Used to mock time in tests. If null, Time.GetTicksMsec is used.
		/// </summary>
		// Token: 0x04001B50 RID: 6992
		[Nullable(2)]
		public Func<ulong> getTimeMsec;
	}
}
