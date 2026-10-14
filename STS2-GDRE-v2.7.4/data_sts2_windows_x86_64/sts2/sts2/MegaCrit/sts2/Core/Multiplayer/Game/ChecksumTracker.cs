using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Calculates checksums, sends them to peers, and validates received checksums to ensure there are no state divergences
	/// during deterministic execution (currently: during combat, after events, and after rest sites).
	/// </summary>
	// Token: 0x02000493 RID: 1171
	[NullableContext(1)]
	[Nullable(0)]
	public class ChecksumTracker : IDisposable
	{
		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06004701 RID: 18177 RVA: 0x00219437 File Offset: 0x00217637
		// (set) Token: 0x06004702 RID: 18178 RVA: 0x0021943F File Offset: 0x0021763F
		public uint NextId { get; private set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06004703 RID: 18179 RVA: 0x00219448 File Offset: 0x00217648
		// (set) Token: 0x06004704 RID: 18180 RVA: 0x00219450 File Offset: 0x00217650
		public bool IsEnabled { get; set; } = true;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06004705 RID: 18181 RVA: 0x0021945C File Offset: 0x0021765C
		// (remove) Token: 0x06004706 RID: 18182 RVA: 0x00219494 File Offset: 0x00217694
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<NetFullCombatState> StateDiverged;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06004707 RID: 18183 RVA: 0x002194CC File Offset: 0x002176CC
		// (remove) Token: 0x06004708 RID: 18184 RVA: 0x00219504 File Offset: 0x00217704
		[Nullable(new byte[] { 2, 1, 1 })]
		[field: Nullable(new byte[] { 2, 1, 1 })]
		public event Action<NetChecksumData, string, NetFullCombatState> ChecksumGenerated;

		// Token: 0x06004709 RID: 18185 RVA: 0x0021953C File Offset: 0x0021773C
		public ChecksumTracker(INetGameService netService, IRunState runState)
		{
			this._netService = netService;
			this._runState = runState;
			this._netService.RegisterMessageHandler<ChecksumDataMessage>(new MessageHandlerDelegate<ChecksumDataMessage>(this.OnReceivedChecksumDataMessage));
			this._netService.RegisterMessageHandler<StateDivergenceMessage>(new MessageHandlerDelegate<StateDivergenceMessage>(this.OnReceivedStateDivergenceMessage));
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x002195C4 File Offset: 0x002177C4
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<ChecksumDataMessage>(new MessageHandlerDelegate<ChecksumDataMessage>(this.OnReceivedChecksumDataMessage));
			this._netService.UnregisterMessageHandler<StateDivergenceMessage>(new MessageHandlerDelegate<StateDivergenceMessage>(this.OnReceivedStateDivergenceMessage));
		}

		/// <summary>
		/// Main entry point for this class. Generates a checksum of the full combat state for validation against peers'.
		/// On host, the checksum is saved for comparison against those received from clients.
		/// On clients, the checksum is saved and sent to the host for validation.
		/// This should be called the exact same amount of times for every peer, otherwise false positive mismatches will be
		/// generated.
		/// </summary>
		// Token: 0x0600470B RID: 18187 RVA: 0x002195F4 File Offset: 0x002177F4
		public NetChecksumData GenerateChecksum(string context, [Nullable(2)] GameAction action)
		{
			if (!this.IsEnabled)
			{
				return default(NetChecksumData);
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Generating checksum for context: ");
			defaultInterpolatedStringHandler.AppendFormatted(context);
			defaultInterpolatedStringHandler.AppendLiteral(" action: ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
			defaultInterpolatedStringHandler.AppendLiteral(" id: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.NextId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			NetChecksumData netChecksumData = this.ObtainAndTrackChecksum(context, action);
			if (this._netService.Type == NetGameType.Client)
			{
				this._netService.SendMessage<ChecksumDataMessage>(new ChecksumDataMessage
				{
					checksumData = netChecksumData
				});
			}
			this.CheckAgainstReplayChecksum(netChecksumData, context);
			return netChecksumData;
		}

		/// <summary>
		/// Called on the host when a client sends a message containing the checksum for their current game state.
		/// </summary>
		// Token: 0x0600470C RID: 18188 RVA: 0x002196B0 File Offset: 0x002178B0
		private void OnReceivedChecksumDataMessage(ChecksumDataMessage message, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received ChecksumDataMessage as non-host player!");
			}
			NetChecksumData remoteChecksumData = message.checksumData;
			int num = this._checksums.FindIndex((ChecksumTracker.TrackedChecksum c) => c.data.id == remoteChecksumData.id);
			if (num < 0)
			{
				this._queuedRemoteChecksums.Add(new ChecksumTracker.QueuedRemoteChecksum
				{
					data = remoteChecksumData,
					senderId = senderId
				});
				return;
			}
			ChecksumTracker.TrackedChecksum trackedChecksum = this._checksums[num];
			this.CompareChecksums(trackedChecksum, remoteChecksumData, senderId);
		}

		/// <summary>
		/// Called on client when the host sends a message indicating that the client's state has diverged from the host's.
		/// Also called on the host when the client receives the host's message, so that the host knows what state the client
		/// was in and can log both.
		/// </summary>
		// Token: 0x0600470D RID: 18189 RVA: 0x0021974C File Offset: 0x0021794C
		private void OnReceivedStateDivergenceMessage(StateDivergenceMessage message, ulong senderId)
		{
			NetChecksumData remoteChecksumData = message.senderChecksum;
			int num = this._checksums.FindIndex((ChecksumTracker.TrackedChecksum c) => c.data.id == remoteChecksumData.id);
			if (num < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(99, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received state divergence message for checksum ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(remoteChecksumData.id);
				defaultInterpolatedStringHandler.AppendLiteral(" that has fallen out of our list! (Our next ID: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this.NextId);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				ChecksumTracker.TrackedChecksum trackedChecksum = this._checksums[num];
				this.LogStateDivergence(trackedChecksum, message, senderId, num);
			}
			if (this._netService.Type == NetGameType.Host)
			{
				NetHostGameService netHostGameService = this._netService as NetHostGameService;
				if (netHostGameService != null)
				{
					netHostGameService.DisconnectClient(senderId, NetError.StateDivergence, false);
				}
				NErrorPopup nerrorPopup = NErrorPopup.Create(new NetErrorInfo(NetError.StateDivergence, true));
				if (nerrorPopup != null)
				{
					NModalContainer instance = NModalContainer.Instance;
					if (instance == null)
					{
						return;
					}
					instance.Add(nerrorPopup, true);
				}
			}
		}

		/// <summary>
		/// Call this to obtain checksum data that may be sent over the network with a message.
		/// </summary>
		// Token: 0x0600470E RID: 18190 RVA: 0x00219848 File Offset: 0x00217A48
		private NetChecksumData ObtainAndTrackChecksum(string context, [Nullable(2)] GameAction action)
		{
			NetFullCombatState netFullCombatState = NetFullCombatState.FromRun(this._runState, action);
			uint num = this.GenerateChecksum(netFullCombatState);
			NetChecksumData netChecksumData = new NetChecksumData
			{
				id = this.NextId,
				checksum = num
			};
			uint nextId = this.NextId;
			this.NextId = nextId + 1U;
			ChecksumTracker.TrackedChecksum trackedChecksum = new ChecksumTracker.TrackedChecksum
			{
				data = netChecksumData,
				context = context,
				fingerprintContext = ((action != null) ? action.GetType().Name : context),
				fullState = netFullCombatState
			};
			Action<NetChecksumData, string, NetFullCombatState> checksumGenerated = this.ChecksumGenerated;
			if (checksumGenerated != null)
			{
				checksumGenerated(netChecksumData, context, netFullCombatState);
			}
			this._checksums.Add(trackedChecksum);
			if (this._checksums.Count > 20)
			{
				this._checksums.RemoveAt(0);
			}
			for (int i = 0; i < this._queuedRemoteChecksums.Count; i++)
			{
				if (this._queuedRemoteChecksums[i].data.id == trackedChecksum.data.id)
				{
					this.CompareChecksums(trackedChecksum, this._queuedRemoteChecksums[i].data, this._queuedRemoteChecksums[i].senderId);
					this._queuedRemoteChecksums.RemoveAt(i);
					i--;
				}
			}
			return netChecksumData;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00219990 File Offset: 0x00217B90
		private void CompareChecksums(ChecksumTracker.TrackedChecksum localChecksum, NetChecksumData remoteChecksum, ulong remoteId)
		{
			if (localChecksum.data.id != remoteChecksum.id)
			{
				throw new InvalidOperationException("Trying to compare two checksums with different IDs!");
			}
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("CompareChecksums should only be called on the host!");
			}
			if (localChecksum.data.checksum != remoteChecksum.checksum)
			{
				if (!TestMode.IsOn)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(108, 5);
					defaultInterpolatedStringHandler.AppendLiteral("State divergence detected! Checksum with ID ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(localChecksum.data.id);
					defaultInterpolatedStringHandler.AppendLiteral(" for client ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(remoteId);
					defaultInterpolatedStringHandler.AppendLiteral(" doesn't match host's!\n");
					defaultInterpolatedStringHandler.AppendLiteral("Context: ");
					defaultInterpolatedStringHandler.AppendFormatted(localChecksum.context);
					defaultInterpolatedStringHandler.AppendLiteral(". Local: ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(localChecksum.data.checksum);
					defaultInterpolatedStringHandler.AppendLiteral(". Remote: ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(remoteChecksum.checksum);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				StateDivergenceMessage stateDivergenceMessage = new StateDivergenceMessage
				{
					senderChecksum = localChecksum.data,
					senderCombatState = localChecksum.fullState
				};
				this._netService.SendMessage<StateDivergenceMessage>(stateDivergenceMessage, remoteId);
			}
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00219AD8 File Offset: 0x00217CD8
		private void LogStateDivergence(ChecksumTracker.TrackedChecksum localChecksum, StateDivergenceMessage message, ulong remoteId, int checksumIndex)
		{
			if (localChecksum.data.id != message.senderChecksum.id)
			{
				throw new InvalidOperationException("Trying to compare two checksums with different IDs!");
			}
			if (!TestMode.IsOn)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(138, 9);
				defaultInterpolatedStringHandler.AppendLiteral("State divergence message received for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(remoteId);
				defaultInterpolatedStringHandler.AppendLiteral(" checksum ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(localChecksum.data.id);
				defaultInterpolatedStringHandler.AppendLiteral("! (We are ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(this._netService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._netService.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(")\n");
				defaultInterpolatedStringHandler.AppendLiteral("Context: ");
				defaultInterpolatedStringHandler.AppendFormatted(localChecksum.context);
				defaultInterpolatedStringHandler.AppendLiteral(". Local: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(localChecksum.data.checksum);
				defaultInterpolatedStringHandler.AppendLiteral(". Remote: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(message.senderChecksum.checksum);
				defaultInterpolatedStringHandler.AppendLiteral(".\n");
				defaultInterpolatedStringHandler.AppendLiteral("LOCAL STATE DUMP\n");
				defaultInterpolatedStringHandler.AppendFormatted<NetFullCombatState>(localChecksum.fullState);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendLiteral("REMOTE STATE DUMP\n");
				defaultInterpolatedStringHandler.AppendFormatted<NetFullCombatState>(message.senderCombatState);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.ReportDivergenceToSentry(localChecksum, message, remoteId, checksumIndex);
			}
			if (this._netService.Type == NetGameType.Client)
			{
				StateDivergenceMessage stateDivergenceMessage = new StateDivergenceMessage
				{
					senderChecksum = localChecksum.data,
					senderCombatState = localChecksum.fullState
				};
				this._netService.SendMessage<StateDivergenceMessage>(stateDivergenceMessage);
				Action<NetFullCombatState> stateDiverged = this.StateDiverged;
				if (stateDiverged == null)
				{
					return;
				}
				stateDiverged(message.senderCombatState);
			}
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x00219CB4 File Offset: 0x00217EB4
		private void ReportDivergenceToSentry(ChecksumTracker.TrackedChecksum localChecksum, StateDivergenceMessage message, ulong remoteId, int checksumIndex)
		{
			string role = this._netService.Type.ToString();
			string text = "Multiplayer state divergence: " + localChecksum.fingerprintContext;
			string localState = localChecksum.fullState.ToString();
			string remoteState = message.senderCombatState.ToString();
			SentryService.CaptureException(new StateDivergenceException(text), delegate(Scope scope)
			{
				EventLikeExtensions.SetFingerprint(scope, new string[] { "StateDivergence", localChecksum.fingerprintContext });
				scope.SetTag("net.role", role);
				scope.SetTag("divergence.type", localChecksum.fingerprintContext);
				scope.SetExtra("divergence.context", localChecksum.context);
				scope.SetExtra("divergence.checksum_id", localChecksum.data.id);
				scope.SetExtra("divergence.local_checksum", localChecksum.data.checksum);
				scope.SetExtra("divergence.remote_checksum", message.senderChecksum.checksum);
				scope.SetExtra("divergence.local_net_id", this._netService.NetId);
				scope.SetExtra("divergence.remote_net_id", remoteId);
				scope.SetExtra("divergence.lobby_id", this._netService.GetRawLobbyIdentifier() ?? "unknown");
				scope.AddCompressedAttachment(localState, "local_state.txt.gz");
				scope.AddCompressedAttachment(remoteState, "remote_state.txt.gz");
				if (checksumIndex > 0)
				{
					string text2 = this._checksums[checksumIndex - 1].fullState.ToString();
					scope.AddCompressedAttachment(text2, "previous_state.txt.gz");
				}
			});
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00219D63 File Offset: 0x00217F63
		public void LoadReplayChecksums(List<ReplayChecksumData> replayChecksums, uint nextId)
		{
			this.NextId = nextId;
			this._replayChecksums = replayChecksums;
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00219D74 File Offset: 0x00217F74
		private void CheckAgainstReplayChecksum(NetChecksumData localData, string context)
		{
			if (this._replayChecksums == null)
			{
				return;
			}
			int num = this._replayChecksums.FindIndex((ReplayChecksumData c) => c.checksumData.id == localData.id);
			if (num == -1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(93, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Replay state diverged! Generated checksum for ");
				defaultInterpolatedStringHandler.AppendFormatted(context);
				defaultInterpolatedStringHandler.AppendLiteral(" with id ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(localData.id);
				defaultInterpolatedStringHandler.AppendLiteral(" that doesn't exist in the replay data");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			ReplayChecksumData replayChecksumData = this._replayChecksums[num];
			uint num2 = this.GenerateChecksum(replayChecksumData.fullState);
			if (num2 != localData.checksum)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(130, 7);
				defaultInterpolatedStringHandler.AppendLiteral("Replay state divergence! Checksum ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(localData.id);
				defaultInterpolatedStringHandler.AppendLiteral("!\n");
				defaultInterpolatedStringHandler.AppendLiteral("Local context: ");
				defaultInterpolatedStringHandler.AppendFormatted(context);
				defaultInterpolatedStringHandler.AppendLiteral(". Replay context: ");
				defaultInterpolatedStringHandler.AppendFormatted(replayChecksumData.context);
				defaultInterpolatedStringHandler.AppendLiteral(". Local: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(localData.checksum);
				defaultInterpolatedStringHandler.AppendLiteral(". Replay: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(".\n");
				defaultInterpolatedStringHandler.AppendLiteral("LOCAL STATE DUMP\n");
				defaultInterpolatedStringHandler.AppendFormatted<NetFullCombatState>(this._checksums.Find((ChecksumTracker.TrackedChecksum c) => c.data.id == localData.id).fullState);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendLiteral("REPLAY STATE DUMP\n");
				defaultInterpolatedStringHandler.AppendFormatted<NetFullCombatState>(replayChecksumData.fullState);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00219F44 File Offset: 0x00218144
		public uint GenerateChecksum(NetFullCombatState state)
		{
			this._packetWriter.Reset();
			this._packetWriter.Write<NetFullCombatState>(state);
			this._packetWriter.ZeroByteRemainder();
			ReadOnlySpan<byte> readOnlySpan = this._packetWriter.Buffer.AsSpan<byte>().Slice(0, this._packetWriter.BytePosition);
			return XxHash32.HashToUInt32(readOnlySpan, 0);
		}

		// Token: 0x04001BC4 RID: 7108
		private const int _checksumsToSave = 20;

		// Token: 0x04001BC5 RID: 7109
		private readonly List<ChecksumTracker.TrackedChecksum> _checksums = new List<ChecksumTracker.TrackedChecksum>();

		// Token: 0x04001BC6 RID: 7110
		private readonly List<ChecksumTracker.QueuedRemoteChecksum> _queuedRemoteChecksums = new List<ChecksumTracker.QueuedRemoteChecksum>();

		// Token: 0x04001BC7 RID: 7111
		private readonly INetGameService _netService;

		// Token: 0x04001BC8 RID: 7112
		private readonly IRunState _runState;

		// Token: 0x04001BC9 RID: 7113
		private readonly PacketWriter _packetWriter = new PacketWriter();

		// Token: 0x04001BCA RID: 7114
		private readonly Logger _logger = new Logger("ChecksumTracker", LogType.GameSync);

		// Token: 0x04001BCB RID: 7115
		[Nullable(2)]
		private List<ReplayChecksumData> _replayChecksums;

		// Token: 0x020019AC RID: 6572
		[Nullable(0)]
		private struct TrackedChecksum
		{
			// Token: 0x04006552 RID: 25938
			public NetChecksumData data;

			// Token: 0x04006553 RID: 25939
			public string context;

			// Token: 0x04006554 RID: 25940
			public string fingerprintContext;

			// Token: 0x04006555 RID: 25941
			public NetFullCombatState fullState;
		}

		// Token: 0x020019AD RID: 6573
		[NullableContext(0)]
		private struct QueuedRemoteChecksum
		{
			// Token: 0x04006556 RID: 25942
			public NetChecksumData data;

			// Token: 0x04006557 RID: 25943
			public ulong senderId;
		}
	}
}
