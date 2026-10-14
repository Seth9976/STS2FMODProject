using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Object which manages the join flow for the local player, who joins as a client.
	/// </summary>
	// Token: 0x0200049B RID: 1179
	[NullableContext(1)]
	[Nullable(0)]
	public class JoinFlow
	{
		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004761 RID: 18273 RVA: 0x0021B9A1 File Offset: 0x00219BA1
		public INetClientGameService NetService { get; }

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06004762 RID: 18274 RVA: 0x0021B9A9 File Offset: 0x00219BA9
		public CancellationTokenSource CancelToken { get; } = new CancellationTokenSource();

		// Token: 0x06004763 RID: 18275 RVA: 0x0021B9B1 File Offset: 0x00219BB1
		public JoinFlow(INetClientGameService netService, JoinFlow.MockInfo? mockInfo = null)
		{
			this._mockInfo = mockInfo;
			this.NetService = netService;
		}

		/// <summary>
		/// Begins the join flow.
		/// This task returns either a JoinResult with the final message in the join flow, for use initializing the run or
		/// the Lobby in the case of a rejoin or a join, respectively.
		/// </summary>
		/// <param name="initializer">The object to use when initializing the connection.</param>
		/// <param name="sceneTree">Scene tree to use for hooking into update.
		/// If null, NetService.Update is NOT called and the caller needs to do it manually.</param>
		/// <throws>ClientConnectionFailedException if the join fails. In this case, you should not use NetService. An error
		/// should be shown to the user and they may try to join again.</throws>
		// Token: 0x06004764 RID: 18276 RVA: 0x0021B9E4 File Offset: 0x00219BE4
		public async Task<JoinResult> Begin(IClientConnectionInitializer initializer, [Nullable(2)] SceneTree sceneTree)
		{
			Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
			Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
			Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
			if (this._connectCompletion != null)
			{
				throw new InvalidOperationException("JoinFlow object can only be used once!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Beginning join with initializer ");
			defaultInterpolatedStringHandler.AppendFormatted<IClientConnectionInitializer>(initializer);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.CancelToken.Token.Register(new Action(this.Cancel));
			CancellationTokenSource updateLoopCancelSource = new CancellationTokenSource();
			if (sceneTree != null)
			{
				TaskHelper.RunSafely(this.NetServiceUpdateLoop(updateLoopCancelSource, sceneTree));
			}
			object obj = null;
			int num = 0;
			JoinResult joinResult;
			try
			{
				try
				{
					this.NetService.RegisterMessageHandler<InitialGameInfoMessage>(new MessageHandlerDelegate<InitialGameInfoMessage>(this.HandleInitialGameInfoMessage));
					this.NetService.RegisterMessageHandler<ClientLobbyJoinResponseMessage>(new MessageHandlerDelegate<ClientLobbyJoinResponseMessage>(this.HandleJoinResponseMessage));
					this.NetService.RegisterMessageHandler<ClientLoadJoinResponseMessage>(new MessageHandlerDelegate<ClientLoadJoinResponseMessage>(this.HandleLoadJoinResponseMessage));
					this.NetService.RegisterMessageHandler<ClientRejoinResponseMessage>(new MessageHandlerDelegate<ClientRejoinResponseMessage>(this.HandleRejoinResponseMessage));
					this.NetService.Disconnected += this.OnDisconnected;
					this._connectCompletion = new TaskCompletionSource<InitialGameInfoMessage>();
					NetErrorInfo? netErrorInfo = await initializer.Connect(this.NetService, this.CancelToken.Token);
					NetErrorInfo? netErrorInfo2 = netErrorInfo;
					if (netErrorInfo2 != null)
					{
						Logger logger2 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Connection failed: ");
						defaultInterpolatedStringHandler.AppendFormatted<NetErrorInfo?>(netErrorInfo2);
						logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						throw new ClientConnectionFailedException("Could not connect", netErrorInfo2.Value);
					}
					this._logger.Info("Initializer connection completed, awaiting initial game info message", 1);
					InitialGameInfoMessage initialMessage = await this._connectCompletion.Task;
					if (initialMessage.connectionFailureReason != null)
					{
						Logger logger3 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Received initial join message with failure: ");
						defaultInterpolatedStringHandler.AppendFormatted<ConnectionFailureReason?>(initialMessage.connectionFailureReason);
						logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						throw new ClientConnectionFailedException("Got connection failure from host", new NetErrorInfo(initialMessage.connectionFailureReason.Value, null));
					}
					RunSessionState state = initialMessage.sessionState;
					Logger logger4 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Got initial game info message. Version: ");
					defaultInterpolatedStringHandler.AppendFormatted(initialMessage.version);
					defaultInterpolatedStringHandler.AppendLiteral(" Hash: ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(initialMessage.idDatabaseHash);
					defaultInterpolatedStringHandler.AppendLiteral(" Type: ");
					defaultInterpolatedStringHandler.AppendFormatted<GameMode>(initialMessage.gameMode);
					defaultInterpolatedStringHandler.AppendLiteral(" State: ");
					defaultInterpolatedStringHandler.AppendFormatted<RunSessionState>(state);
					logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					ConnectionFailureExtraInfo connectionFailureExtraInfo = new ConnectionFailureExtraInfo
					{
						hostBranch = new PlatformBranch?(initialMessage.branch),
						hostVersion = initialMessage.version,
						hostHash = new ulong?((ulong)initialMessage.idDatabaseHash),
						localVersion = (((this._mockInfo != null) ? this._mockInfo.GetValueOrDefault().version : null) ?? NGame.GetGameVersion()),
						localBranch = new PlatformBranch?((this._mockInfo != null) ? this._mockInfo.GetValueOrDefault().branch : PlatformUtil.GetPlatformBranch()),
						localHash = new ulong?((ulong)((this._mockInfo != null) ? this._mockInfo.GetValueOrDefault().hash : ModelIdSerializationCache.Hash))
					};
					if (initialMessage.version != connectionFailureExtraInfo.localVersion)
					{
						NetErrorInfo netErrorInfo3 = new NetErrorInfo(ConnectionFailureReason.VersionMismatch, connectionFailureExtraInfo);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Version mismatch. Host: ");
						defaultInterpolatedStringHandler.AppendFormatted(initialMessage.version);
						defaultInterpolatedStringHandler.AppendLiteral(" Ours: ");
						defaultInterpolatedStringHandler.AppendFormatted(connectionFailureExtraInfo.localVersion);
						defaultInterpolatedStringHandler.AppendLiteral(" Host branch: ");
						defaultInterpolatedStringHandler.AppendFormatted<PlatformBranch>(initialMessage.branch);
						throw new ClientConnectionFailedException(defaultInterpolatedStringHandler.ToStringAndClear(), netErrorInfo3);
					}
					List<string> list = ((this._mockInfo == null) ? ModManager.GetGameplayRelevantModNameList() : this._mockInfo.Value.gameplayAffectingMods) ?? new List<string>();
					List<string> list2 = initialMessage.gameplayAffectingMods ?? new List<string>();
					List<string> list3 = list2.Except<string>(list).ToList<string>();
					List<string> list4 = list.Except<string>(list2).ToList<string>();
					connectionFailureExtraInfo.missingModsOnHost = list4;
					connectionFailureExtraInfo.missingModsOnLocal = list3;
					if (list3.Count > 0 || list4.Count > 0)
					{
						Logger logger5 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(124, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Mismatch in gameplay-relevant mods with the host!\n");
						defaultInterpolatedStringHandler.AppendLiteral("Mods that host has that we don't: ");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", list3));
						defaultInterpolatedStringHandler.AppendLiteral(".\n");
						defaultInterpolatedStringHandler.AppendLiteral("Mods that we have that host doesn't: ");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", list4));
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger5.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						throw new ClientConnectionFailedException("Mod mismatch. Host mods: " + string.Join(",", list2) + " Local mods: " + string.Join(",", list), new NetErrorInfo(ConnectionFailureReason.ModMismatch, connectionFailureExtraInfo));
					}
					ulong num2 = (ulong)initialMessage.idDatabaseHash;
					ulong? localHash = connectionFailureExtraInfo.localHash;
					if (!((num2 == localHash.GetValueOrDefault()) & (localHash != null)))
					{
						this._logger.Warn("Our version " + connectionFailureExtraInfo.localVersion + " matches the host's, but our Model ID hash does not! Disconnecting", 1);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
						defaultInterpolatedStringHandler.AppendLiteral("ModelDb hash mismatch. Host: ");
						defaultInterpolatedStringHandler.AppendFormatted<uint>(initialMessage.idDatabaseHash);
						defaultInterpolatedStringHandler.AppendLiteral(" Ours: ");
						defaultInterpolatedStringHandler.AppendFormatted<uint>(ModelIdSerializationCache.Hash);
						throw new ClientConnectionFailedException(defaultInterpolatedStringHandler.ToStringAndClear(), new NetErrorInfo(ConnectionFailureReason.VersionMismatch, connectionFailureExtraInfo));
					}
					List<string> list5 = ((this._mockInfo == null) ? ModManager.GetNonGameplayRelevantModNameList() : this._mockInfo.Value.nonGameplayAffectingMods) ?? new List<string>();
					List<string> list6 = initialMessage.otherMods ?? new List<string>();
					List<string> list7 = list6.Except<string>(list5).ToList<string>();
					List<string> list8 = list5.Except<string>(list6).ToList<string>();
					if (list8.Count > 0 || list7.Count > 0)
					{
						Logger logger6 = this._logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(251, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Mismatch in non-gameplay relevant mods. This is allowed, but it's up to the mod authors to guarantee that it doesn't break anything.\n");
						defaultInterpolatedStringHandler.AppendLiteral("Non-gameplay relevant mods that host has that we don't: ");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", list7));
						defaultInterpolatedStringHandler.AppendLiteral(".\n");
						defaultInterpolatedStringHandler.AppendLiteral("Non-gameplay relevant mods that we have that host doesn't: ");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", list8));
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger6.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					}
					switch (state)
					{
					case RunSessionState.InLobby:
					{
						ClientLobbyJoinResponseMessage clientLobbyJoinResponseMessage = await this.AttemptJoin();
						joinResult = new JoinResult
						{
							gameMode = initialMessage.gameMode,
							sessionState = new RunSessionState?(state),
							joinResponse = new ClientLobbyJoinResponseMessage?(clientLobbyJoinResponseMessage)
						};
						break;
					}
					case RunSessionState.InLoadedLobby:
					{
						ClientLoadJoinResponseMessage clientLoadJoinResponseMessage = await this.AttemptLoadJoin();
						joinResult = new JoinResult
						{
							gameMode = initialMessage.gameMode,
							sessionState = new RunSessionState?(state),
							loadJoinResponse = new ClientLoadJoinResponseMessage?(clientLoadJoinResponseMessage)
						};
						break;
					}
					case RunSessionState.Running:
					{
						ClientRejoinResponseMessage clientRejoinResponseMessage = await this.AttemptRejoin();
						joinResult = new JoinResult
						{
							gameMode = initialMessage.gameMode,
							sessionState = new RunSessionState?(state),
							rejoinResponse = new ClientRejoinResponseMessage?(clientRejoinResponseMessage)
						};
						break;
					}
					default:
						this.NetService.Disconnect(NetError.InternalError, true);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Received invalid state ");
						defaultInterpolatedStringHandler.AppendFormatted<RunSessionState>(state);
						defaultInterpolatedStringHandler.AppendLiteral(" from connection!");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				catch (Exception ex)
				{
					if (this.NetService.IsConnected)
					{
						NetError netError = ((ex is OperationCanceledException) ? NetError.CancelledJoin : NetError.InternalError);
						this.NetService.Disconnect(netError, false);
					}
					Logger.logLevelTypeMap[LogType.Network] = LogLevel.Info;
					Logger.logLevelTypeMap[LogType.Actions] = LogLevel.Info;
					Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.Info;
					throw;
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			await updateLoopCancelSource.CancelAsync();
			this.NetService.UnregisterMessageHandler<InitialGameInfoMessage>(new MessageHandlerDelegate<InitialGameInfoMessage>(this.HandleInitialGameInfoMessage));
			this.NetService.UnregisterMessageHandler<ClientLobbyJoinResponseMessage>(new MessageHandlerDelegate<ClientLobbyJoinResponseMessage>(this.HandleJoinResponseMessage));
			this.NetService.UnregisterMessageHandler<ClientLoadJoinResponseMessage>(new MessageHandlerDelegate<ClientLoadJoinResponseMessage>(this.HandleLoadJoinResponseMessage));
			this.NetService.UnregisterMessageHandler<ClientRejoinResponseMessage>(new MessageHandlerDelegate<ClientRejoinResponseMessage>(this.HandleRejoinResponseMessage));
			this.NetService.Disconnected -= this.OnDisconnected;
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex2 = obj2 as Exception;
				if (ex2 == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex2).Throw();
			}
			JoinResult joinResult2;
			if (num == 1)
			{
				joinResult2 = joinResult;
			}
			else
			{
				obj = null;
				joinResult = default(JoinResult);
				updateLoopCancelSource = null;
			}
			return joinResult2;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0021BA38 File Offset: 0x00219C38
		private async Task NetServiceUpdateLoop(CancellationTokenSource token, SceneTree sceneTree)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					this.NetService.Update();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), 2);
				}
				await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
			}
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0021BA8C File Offset: 0x00219C8C
		private async Task<ClientLobbyJoinResponseMessage> AttemptJoin()
		{
			this._joinCompletion = new TaskCompletionSource<ClientLobbyJoinResponseMessage>();
			this._logger.Info("Sending ClientLobbyJoinRequestMessage and waiting for response message", 1);
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			ClientLobbyJoinRequestMessage clientLobbyJoinRequestMessage = new ClientLobbyJoinRequestMessage
			{
				maxAscensionUnlocked = SaveManager.Instance.Progress.MaxMultiplayerAscension,
				unlockState = unlockState.ToSerializable()
			};
			this.NetService.SendMessage<ClientLobbyJoinRequestMessage>(clientLobbyJoinRequestMessage);
			ClientLobbyJoinResponseMessage clientLobbyJoinResponseMessage = await this._joinCompletion.Task;
			ClientLobbyJoinResponseMessage clientLobbyJoinResponseMessage2 = clientLobbyJoinResponseMessage;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("ClientLobbyJoinResponseMessage");
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<ClientLobbyJoinResponseMessage>(clientLobbyJoinResponseMessage2);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			return clientLobbyJoinResponseMessage2;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0021BAD0 File Offset: 0x00219CD0
		private async Task<ClientLoadJoinResponseMessage> AttemptLoadJoin()
		{
			this._loadJoinCompletion = new TaskCompletionSource<ClientLoadJoinResponseMessage>();
			this._logger.Info("Sending ClientLoadJoinRequestMessage and waiting for rejoin response message", 1);
			this.NetService.SendMessage<ClientLoadJoinRequestMessage>(default(ClientLoadJoinRequestMessage));
			ClientLoadJoinResponseMessage clientLoadJoinResponseMessage = await this._loadJoinCompletion.Task;
			ClientLoadJoinResponseMessage clientLoadJoinResponseMessage2 = clientLoadJoinResponseMessage;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received ClientLoadJoinResponseMessage: ");
			defaultInterpolatedStringHandler.AppendFormatted<ClientLoadJoinResponseMessage>(clientLoadJoinResponseMessage2);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			return clientLoadJoinResponseMessage2;
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0021BB14 File Offset: 0x00219D14
		private async Task<ClientRejoinResponseMessage> AttemptRejoin()
		{
			this._rejoinCompletion = new TaskCompletionSource<ClientRejoinResponseMessage>();
			this._logger.Info("Sending ClientRequestRejoinMessage and waiting for rejoin response message", 1);
			this.NetService.SendMessage<ClientRejoinRequestMessage>(default(ClientRejoinRequestMessage));
			ClientRejoinResponseMessage clientRejoinResponseMessage = await this._rejoinCompletion.Task;
			ClientRejoinResponseMessage clientRejoinResponseMessage2 = clientRejoinResponseMessage;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Received ClientRejoinResponseMessage: ");
			defaultInterpolatedStringHandler.AppendFormatted<ClientRejoinResponseMessage>(clientRejoinResponseMessage2);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			return clientRejoinResponseMessage2;
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0021BB58 File Offset: 0x00219D58
		private void HandleInitialGameInfoMessage(InitialGameInfoMessage message, ulong _)
		{
			if (this._connectCompletion == null || this._connectCompletion.Task.IsCompleted)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("InitialGameInfoMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" when we weren't expecting it! Completion status: ");
				defaultInterpolatedStringHandler.AppendFormatted<TaskCompletionSource<InitialGameInfoMessage>>(this._connectCompletion);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._connectCompletion.SetResult(message);
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0021BBDC File Offset: 0x00219DDC
		private void HandleRejoinResponseMessage(ClientRejoinResponseMessage message, ulong senderId)
		{
			if (this._rejoinCompletion == null || this._rejoinCompletion.Task.IsCompleted)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("ClientRejoinResponseMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" when we weren't expecting it! Completion status: ");
				defaultInterpolatedStringHandler.AppendFormatted<TaskCompletionSource<InitialGameInfoMessage>>(this._connectCompletion);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._rejoinCompletion.SetResult(message);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0021BC60 File Offset: 0x00219E60
		private void HandleLoadJoinResponseMessage(ClientLoadJoinResponseMessage message, ulong senderId)
		{
			if (this._loadJoinCompletion == null || this._loadJoinCompletion.Task.IsCompleted)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("ClientLoadJoinResponseMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" when we weren't expecting it! Completion status: ");
				defaultInterpolatedStringHandler.AppendFormatted<TaskCompletionSource<InitialGameInfoMessage>>(this._connectCompletion);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._loadJoinCompletion.SetResult(message);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0021BCE4 File Offset: 0x00219EE4
		private void HandleJoinResponseMessage(ClientLobbyJoinResponseMessage message, ulong senderId)
		{
			if (this._joinCompletion == null || this._joinCompletion.Task.IsCompleted)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("ClientLobbyJoinResponseMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" when we weren't expecting it! Completion status: ");
				defaultInterpolatedStringHandler.AppendFormatted<TaskCompletionSource<InitialGameInfoMessage>>(this._connectCompletion);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			this._joinCompletion.SetResult(message);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x0021BD68 File Offset: 0x00219F68
		private void OnDisconnected(NetErrorInfo info)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnected during join flow, reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			defaultInterpolatedStringHandler.AppendLiteral(". Failing with an exception");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unexpectedly disconnected from host while joining. Reason: ");
			defaultInterpolatedStringHandler.AppendFormatted<NetError>(info.GetReason());
			ClientConnectionFailedException ex = new ClientConnectionFailedException(defaultInterpolatedStringHandler.ToStringAndClear(), info);
			TaskCompletionSource<InitialGameInfoMessage> connectCompletion = this._connectCompletion;
			if (connectCompletion != null)
			{
				Task<InitialGameInfoMessage> task = connectCompletion.Task;
				if (task != null && !task.IsCompleted)
				{
					this._connectCompletion.SetException(ex);
				}
			}
			TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion = this._joinCompletion;
			if (joinCompletion != null)
			{
				Task<ClientLobbyJoinResponseMessage> task2 = joinCompletion.Task;
				if (task2 != null && !task2.IsCompleted)
				{
					TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion2 = this._joinCompletion;
					if (joinCompletion2 != null)
					{
						joinCompletion2.SetException(ex);
					}
				}
			}
			TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion = this._loadJoinCompletion;
			if (loadJoinCompletion != null)
			{
				Task<ClientLoadJoinResponseMessage> task3 = loadJoinCompletion.Task;
				if (task3 != null && !task3.IsCompleted)
				{
					TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion2 = this._loadJoinCompletion;
					if (loadJoinCompletion2 != null)
					{
						loadJoinCompletion2.SetException(ex);
					}
				}
			}
			TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion = this._rejoinCompletion;
			if (rejoinCompletion != null)
			{
				Task<ClientRejoinResponseMessage> task4 = rejoinCompletion.Task;
				if (task4 != null && !task4.IsCompleted)
				{
					TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion2 = this._rejoinCompletion;
					if (rejoinCompletion2 == null)
					{
						return;
					}
					rejoinCompletion2.SetException(ex);
				}
			}
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0021BEB0 File Offset: 0x0021A0B0
		private void Cancel()
		{
			TaskCompletionSource<InitialGameInfoMessage> connectCompletion = this._connectCompletion;
			if (connectCompletion != null)
			{
				Task<InitialGameInfoMessage> task = connectCompletion.Task;
				if (task != null && !task.IsCompleted)
				{
					this._connectCompletion.SetCanceled();
				}
			}
			TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion = this._joinCompletion;
			if (joinCompletion != null)
			{
				Task<ClientLobbyJoinResponseMessage> task2 = joinCompletion.Task;
				if (task2 != null && !task2.IsCompleted)
				{
					TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion2 = this._joinCompletion;
					if (joinCompletion2 != null)
					{
						joinCompletion2.SetCanceled();
					}
				}
			}
			TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion = this._loadJoinCompletion;
			if (loadJoinCompletion != null)
			{
				Task<ClientLoadJoinResponseMessage> task3 = loadJoinCompletion.Task;
				if (task3 != null && !task3.IsCompleted)
				{
					TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion2 = this._loadJoinCompletion;
					if (loadJoinCompletion2 != null)
					{
						loadJoinCompletion2.SetCanceled();
					}
				}
			}
			TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion = this._rejoinCompletion;
			if (rejoinCompletion != null)
			{
				Task<ClientRejoinResponseMessage> task4 = rejoinCompletion.Task;
				if (task4 != null && !task4.IsCompleted)
				{
					TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion2 = this._rejoinCompletion;
					if (rejoinCompletion2 == null)
					{
						return;
					}
					rejoinCompletion2.SetCanceled();
				}
			}
		}

		// Token: 0x04001BEB RID: 7147
		[Nullable(2)]
		private TaskCompletionSource<InitialGameInfoMessage> _connectCompletion;

		// Token: 0x04001BEC RID: 7148
		[Nullable(2)]
		private TaskCompletionSource<ClientRejoinResponseMessage> _rejoinCompletion;

		// Token: 0x04001BED RID: 7149
		[Nullable(2)]
		private TaskCompletionSource<ClientLoadJoinResponseMessage> _loadJoinCompletion;

		// Token: 0x04001BEE RID: 7150
		[Nullable(2)]
		private TaskCompletionSource<ClientLobbyJoinResponseMessage> _joinCompletion;

		// Token: 0x04001BEF RID: 7151
		private readonly Logger _logger = new Logger("JoinFlow", LogType.Network);

		// Token: 0x04001BF0 RID: 7152
		private readonly JoinFlow.MockInfo? _mockInfo;

		// Token: 0x020019B6 RID: 6582
		[NullableContext(0)]
		public struct MockInfo
		{
			// Token: 0x0400656E RID: 25966
			[Nullable(1)]
			public string version;

			// Token: 0x0400656F RID: 25967
			public uint hash;

			// Token: 0x04006570 RID: 25968
			public PlatformBranch branch;

			// Token: 0x04006571 RID: 25969
			[Nullable(new byte[] { 2, 1 })]
			public List<string> gameplayAffectingMods;

			// Token: 0x04006572 RID: 25970
			[Nullable(new byte[] { 2, 1 })]
			public List<string> nonGameplayAffectingMods;
		}
	}
}
