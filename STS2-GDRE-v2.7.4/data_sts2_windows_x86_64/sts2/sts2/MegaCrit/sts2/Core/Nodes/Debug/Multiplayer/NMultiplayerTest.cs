using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer
{
	// Token: 0x020003CE RID: 974
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Debug/Multiplayer/NMultiplayerTest.cs")]
	public class NMultiplayerTest : Control, IStartRunLobbyListener
	{
		// Token: 0x06003AE6 RID: 15078 RVA: 0x001C1438 File Offset: 0x001BF638
		public override void _Ready()
		{
			this._ipField = base.GetNode<TextEdit>("IpField");
			this._idField = base.GetNode<TextEdit>("NameField");
			this._characterPaginator = base.GetNode<NMultiplayerTestCharacterPaginator>("CharacterChooser");
			Button node = base.GetNode<Button>("HostButton");
			Button node2 = base.GetNode<Button>("JoinButton");
			this._readyButton = base.GetNode<Button>("ReadyButton");
			this._readyIndicator = base.GetNode<Control>("ReadyButton/ReadyIndicator");
			Button node3 = base.GetNode<Button>("ReplayButton");
			Button node4 = base.GetNode<Button>("SaveReplayButton");
			Button node5 = base.GetNode<Button>("DeleteCloudSavesButton");
			this._loadingPanel = base.GetNode<Control>("LoadingPanel");
			Control node6 = base.GetNode<Control>("Characters");
			foreach (Node node7 in node6.GetChildren(false))
			{
				this._characterContainers.Add(new NMultiplayerTest.CharacterContainer
				{
					characterImage = node7.GetNode<TextureRect>("Image"),
					playerName = node7.GetNode<Label>("Name")
				});
			}
			node.Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.HostButtonPressed)), 0U);
			node2.Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.JoinButtonPressed)), 0U);
			this._readyButton.Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ReadyButtonPressed)), 0U);
			node3.Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ChooseReplayToLoad)), 0U);
			node4.Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ChooseReplayToSave)), 0U);
			GodotObject godotObject = node5;
			StringName buttonUp = BaseButton.SignalName.ButtonUp;
			Action action;
			if ((action = NMultiplayerTest.<>O.<0>__DeleteCloudSaves) == null)
			{
				action = (NMultiplayerTest.<>O.<0>__DeleteCloudSaves = new Action(CloudConsoleCmd.DeleteCloudSaves));
			}
			godotObject.Connect(buttonUp, Callable.From(action), 0U);
			this._characterPaginator.Visible = false;
			this._characterPaginator.CharacterChanged += this.OnCharacterChanged;
			this._readyButton.Visible = false;
			this._game = base.GetTree().Root.GetNodeOrNull<NGame>("Game");
			if (this._game == null)
			{
				this._game = SceneHelper.Instantiate<NGame>("game");
				this._game.StartOnMainMenu = false;
				Callable.From(new Action(this.AddGame)).CallDeferred(Array.Empty<Variant>());
			}
			Type type = BootstrapSettingsUtil.Get();
			if (type != null)
			{
				this._settings = (IBootstrapSettings)Activator.CreateInstance(type);
			}
			Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
			Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
			Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001C175C File Offset: 0x001BF95C
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001C1769 File Offset: 0x001BF969
		public override void _ExitTree()
		{
			this._cts.Cancel();
			this._characterPaginator.CharacterChanged -= this.OnCharacterChanged;
			if (!this._beginningRun)
			{
				StartRunLobby lobby = this._lobby;
				if (lobby == null)
				{
					return;
				}
				lobby.CleanUp(true, NetError.Quit);
			}
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001C17A8 File Offset: 0x001BF9A8
		private void AddGame()
		{
			Node root = base.GetTree().Root;
			root.AddChildSafely(this._game);
			this._game.RootSceneContainer.SetCurrentScene(this);
			TaskHelper.RunSafely(this._game.Transition.FadeIn(0.8f, "res://materials/transitions/fade_transition_mat.tres", null));
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x001C1807 File Offset: 0x001BFA07
		private void HostButtonPressed()
		{
			TaskHelper.RunSafely(this.StartHost(false));
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x001C1818 File Offset: 0x001BFA18
		private void JoinButtonPressed()
		{
			ulong num = ((this._idField.Text != string.Empty) ? ulong.Parse(this._idField.Text) : 1000UL);
			string text = ((this._ipField.Text != string.Empty) ? this._ipField.Text : "127.0.0.1");
			ENetClientConnectionInitializer enetClientConnectionInitializer = new ENetClientConnectionInitializer(num, text, 33771);
			TaskHelper.RunSafely(this.JoinToHost(enetClientConnectionInitializer));
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x001C189C File Offset: 0x001BFA9C
		private void ReadyButtonPressed()
		{
			this._localPlayerData.Deck = this._characterPaginator.Character.StartingDeck.Select<CardModel, SerializableCard>((CardModel c) => c.ToMutable().ToSerializable()).ToList<SerializableCard>();
			this._localPlayerData.Relics = this._characterPaginator.Character.StartingRelics.Select<RelicModel, SerializableRelic>((RelicModel r) => r.ToMutable().ToSerializable()).ToList<SerializableRelic>();
			this._localPlayerData.Potions = new List<SerializablePotion>();
			this._localPlayerData.Rng = new SerializablePlayerRngSet();
			this._localPlayerData.Odds = new SerializablePlayerOddsSet();
			this._localPlayerData.RelicGrabBag = new SerializableRelicGrabBag();
			this._localPlayerData.ExtraFields = new SerializableExtraPlayerFields();
			this._localPlayerData.UnlockState = new SerializableUnlockState();
			this._lobby.SetReady(true);
			this._readyIndicator.Visible = true;
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x001C19A9 File Offset: 0x001BFBA9
		public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> __)
		{
			this._loadingPanel.Visible = true;
			TaskHelper.RunSafely(this.BeginRunAsyncWrapper(seed, acts));
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x001C19C8 File Offset: 0x001BFBC8
		private async Task BeginRunAsyncWrapper(string seed, List<ActModel> acts)
		{
			try
			{
				await this.BeginRunAsync(seed, acts);
			}
			finally
			{
				if (this._loadingPanel.IsValid())
				{
					this._loadingPanel.Visible = false;
				}
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x001C1A1C File Offset: 0x001BFC1C
		private async Task BeginRunAsync(string seed, List<ActModel> acts)
		{
			this._beginningRun = true;
			IBootstrapSettings settings = this._settings;
			if (settings != null && settings.BootstrapInMultiplayer)
			{
				RunState runState;
				using (new NetLoadingHandle(this._lobby.NetService))
				{
					acts[0] = this._settings.Act;
					runState = RunState.CreateForNewRun(this._lobby.Players.Select<LobbyPlayer, Player>((LobbyPlayer p) => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id)).ToList<Player>(), acts.Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), this._settings.Modifiers, GameMode.Standard, this._lobby.Ascension, seed);
					RunManager.Instance.SetUpNewMultiplayer(runState, this._lobby, this._settings.SaveRunHistory, null);
					await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Player p) => p.Character));
					await RunManager.Instance.FinalizeStartingRelics();
					RunManager.Instance.Launch();
					this._game.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
					await RunManager.Instance.SetActInternal(0);
					await SaveManager.Instance.SaveRun(null, true);
					this._lobby.CleanUp(false, NetError.Quit);
					await this._settings.Setup(LocalContext.GetMe(runState));
					switch (this._settings.RoomType)
					{
					case RoomType.Unassigned:
						await RunManager.Instance.EnterAct(0, true);
						goto IL_05D8;
					case RoomType.Treasure:
					case RoomType.Shop:
					case RoomType.RestSite:
						await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, null, false);
						RunManager.Instance.ActionExecutor.Unpause();
						goto IL_05D8;
					case RoomType.Event:
						await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, this._settings.Event, false);
						goto IL_05D8;
					}
					await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, this._settings.RoomType.IsCombatRoom() ? this._settings.Encounter.ToMutable() : null, false);
					IL_05D8:;
				}
				NetLoadingHandle loadHandle = null;
				runState = null;
			}
			else
			{
				await this._game.StartNewMultiplayerRun(this._lobby, true, acts, Array.Empty<ModifierModel>(), seed, this._lobby.Ascension, null);
				this._lobby.CleanUp(false, NetError.Quit);
			}
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x001C1A6F File Offset: 0x001BFC6F
		private void Disconnect(NetError reason)
		{
			StartRunLobby lobby = this._lobby;
			if (lobby != null)
			{
				lobby.CleanUp(true, reason);
			}
			this._lobby = null;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001C1A8C File Offset: 0x001BFC8C
		private async Task<bool> StartHost(bool steam)
		{
			IBootstrapSettings settings = this._settings;
			if (settings != null && settings.BootstrapInMultiplayer)
			{
				PreloadManager.Enabled = this._settings.DoPreloading;
				this._game.DebugSeedOverride = this._settings.Seed;
			}
			this.Disconnect(NetError.Quit);
			NetHostGameService netService = new NetHostGameService();
			NetErrorInfo? netErrorInfo2;
			if (steam)
			{
				NetErrorInfo? netErrorInfo = await netService.StartSteamHost(4);
				netErrorInfo2 = netErrorInfo;
			}
			else
			{
				netErrorInfo2 = netService.StartENetHost(33771, 4);
			}
			if (netErrorInfo2 == null)
			{
				this._lobby = new StartRunLobby(GameMode.Standard, netService, this, 4);
				this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
				this.AfterMultiplayerStarted();
				Log.Info("Successful host", 2);
			}
			else
			{
				this._lobby = null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed host: ");
				defaultInterpolatedStringHandler.AppendFormatted<NetErrorInfo?>(netErrorInfo2);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return netErrorInfo2 == null;
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x001C1AD8 File Offset: 0x001BFCD8
		public async Task JoinToHost(IClientConnectionInitializer initializer)
		{
			IBootstrapSettings settings = this._settings;
			if (settings != null && settings.BootstrapInMultiplayer)
			{
				PreloadManager.Enabled = this._settings.DoPreloading;
				this._game.DebugSeedOverride = this._settings.Seed;
			}
			this.Disconnect(NetError.Quit);
			JoinFlow joinFlow = new JoinFlow(new NetClientGameService(), null);
			try
			{
				JoinResult joinResult = await joinFlow.Begin(initializer, base.GetTree());
				JoinResult joinResult2 = joinResult;
				if (joinResult2.sessionState.GetValueOrDefault() == RunSessionState.InLobby)
				{
					Log.Info("Successfully joined lobby", 2);
					this._lobby = new StartRunLobby(joinResult2.gameMode, joinFlow.NetService, this, -1);
					this._lobby.InitializeFromMessage(joinResult2.joinResponse.Value);
					this.AfterMultiplayerStarted();
				}
				else if (joinResult2.sessionState.GetValueOrDefault() == RunSessionState.Running)
				{
					Log.Info("Successfully joined run in-progress. Initializing run", 2);
					throw new NotImplementedException("Run re-joining has yet to be implemented!");
				}
			}
			catch (Exception ex)
			{
				joinFlow.NetService.Disconnect(NetError.RunInProgress, false);
				this._lobby = null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed join: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x001C1B24 File Offset: 0x001BFD24
		private void AfterMultiplayerStarted()
		{
			foreach (LobbyPlayer lobbyPlayer in this._lobby.Players)
			{
				this._characterContainers[lobbyPlayer.slotId].characterImage.Texture = lobbyPlayer.character.IconTexture;
				this._characterContainers[lobbyPlayer.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, lobbyPlayer.id);
			}
			this._readyButton.Visible = true;
			this._characterPaginator.Visible = true;
			this._game.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((LobbyPlayer p) => p.id));
			this._game.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			this.OnCharacterChanged(this._lobby.LocalPlayer.character);
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x001C1C64 File Offset: 0x001BFE64
		private void ChooseReplayToLoad()
		{
			this.ChooseReplay(new Action<string>(this.LoadReplay));
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x001C1C78 File Offset: 0x001BFE78
		private void ChooseReplayToSave()
		{
			this.ChooseReplay(new Action<string>(this.WriteReplayAsSave));
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x001C1C8C File Offset: 0x001BFE8C
		private void ChooseReplay(Action<string> action)
		{
			FileDialog fileDialog = new FileDialog();
			fileDialog.Filters = new string[] { "*.mcr" };
			fileDialog.UseNativeDialog = true;
			fileDialog.Title = "Choose Replay";
			fileDialog.Access = 2L;
			fileDialog.FileMode = 0L;
			fileDialog.CurrentDir = ProjectSettings.GlobalizePath("user://");
			fileDialog.Connect(FileDialog.SignalName.FileSelected, Callable.From<string>(action), 0U);
			this.AddChildSafely(fileDialog);
			fileDialog.Show();
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001C1D08 File Offset: 0x001BFF08
		private bool ValidateReplay(CombatReplay replay)
		{
			if (replay.modelIdHash != ModelIdSerializationCache.Hash)
			{
				if (!this._ignoreReplayModelIdHash)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(162, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Attempting to load replay with Model ID hash ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(replay.modelIdHash);
					defaultInterpolatedStringHandler.AppendLiteral(" that does not match ours (");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(ModelIdSerializationCache.Hash);
					defaultInterpolatedStringHandler.AppendLiteral(")! The replay will mismatch. If you want to continue anyway, try running the replay again.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this._ignoreReplayModelIdHash = true;
					return false;
				}
				Log.Warn("Ignoring model ID hash mismatch in replay.", 2);
			}
			return true;
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x001C1D98 File Offset: 0x001BFF98
		private void LoadReplay(string path)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FileAccessStream fileAccessStream = new FileAccessStream(path, 1L))
				{
					fileAccessStream.CopyTo(memoryStream);
				}
				PacketReader packetReader = new PacketReader();
				packetReader.Reset(memoryStream.ToArray());
				CombatReplay combatReplay = packetReader.Read<CombatReplay>();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Loaded replay. Game version: ");
				defaultInterpolatedStringHandler.AppendFormatted(combatReplay.version);
				defaultInterpolatedStringHandler.AppendLiteral(" Commit: ");
				defaultInterpolatedStringHandler.AppendFormatted(combatReplay.gitCommit);
				defaultInterpolatedStringHandler.AppendLiteral(" Model ID hash: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(combatReplay.modelIdHash);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (this.ValidateReplay(combatReplay))
				{
					IBootstrapSettings settings = this._settings;
					int valueOrDefault = ((settings != null) ? settings.ReplayPlayerIndex : null).GetValueOrDefault();
					TaskHelper.RunSafely(NMultiplayerTest.RunReplay(combatReplay, base.GetTree(), valueOrDefault));
				}
			}
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x001C1EB4 File Offset: 0x001C00B4
		private static async Task RunReplay(CombatReplay replay, SceneTree sceneTree, int playerIndex)
		{
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			string text = ((releaseInfo != null) ? releaseInfo.Commit : null) ?? GitHelper.ShortCommitId;
			if (replay.gitCommit != text)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Git commit in replay ");
				defaultInterpolatedStringHandler.AppendFormatted(replay.gitCommit);
				defaultInterpolatedStringHandler.AppendLiteral(" does not match ours (");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("). The replay has a chance of mismatching!");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			RunState runState = RunState.FromSerializable(replay.serializableRun);
			ulong netId = runState.Players[playerIndex].NetId;
			RunManager.Instance.SetUpReplay(runState, replay, netId);
			RunManager.Instance.CombatStateSynchronizer.IsDisabled = true;
			await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Player p) => p.Character));
			await PreloadManager.LoadActAssets(runState.Act);
			RunManager.Instance.Launch();
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			NGame.Instance.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
			await RunManager.Instance.GenerateMap();
			RunManager.Instance.ActionQueueSet.FastForwardNextActionId(replay.nextActionId);
			RunManager.Instance.ActionQueueSynchronizer.FastForwardHookId(replay.nextHookId);
			RunManager.Instance.ChecksumTracker.LoadReplayChecksums(replay.checksumData, replay.nextChecksumId);
			RunManager.Instance.PlayerChoiceSynchronizer.FastForwardChoiceIds(replay.choiceIds);
			RunManager.Instance.RewardsSetSynchronizer.FastForwardRewardIds(replay.rewardIds);
			await RunManager.Instance.LoadIntoLatestMapCoord(AbstractRoom.FromSerializable(replay.serializableRun.PreFinishedRoom, runState));
			while (RunManager.Instance.ActionExecutor.IsPaused)
			{
				await sceneTree.Root.AwaitProcessFrame(default(CancellationToken));
			}
			foreach (CombatReplayEvent replayEvent in replay.events)
			{
				switch (replayEvent.eventType)
				{
				case CombatReplayEventType.GameAction:
				{
					while (CombatManager.Instance.EndingPlayerTurnPhaseOne || CombatManager.Instance.EndingPlayerTurnPhaseTwo)
					{
						await sceneTree.Root.AwaitProcessFrame(default(CancellationToken));
					}
					Player player = runState.GetPlayer(replayEvent.playerId.Value);
					GameAction action = replayEvent.action.ToGameAction(player);
					if (action.ActionType == GameActionType.CombatPlayPhaseOnly)
					{
						while (CombatManager.Instance.DebugOnlyGetState().CurrentSide == CombatSide.Enemy)
						{
							await sceneTree.Root.AwaitProcessFrame(default(CancellationToken));
						}
					}
					RunManager.Instance.ActionQueueSet.EnqueueWithoutSynchronizing(action);
					if (action is EndPlayerTurnAction || action is ReadyToBeginEnemyTurnAction)
					{
						await RunManager.Instance.ActionExecutor.FinishedExecutingActions();
					}
					break;
				}
				case CombatReplayEventType.HookAction:
				{
					uint value = replayEvent.hookId.Value;
					GameActionType value2 = replayEvent.gameActionType.Value;
					RunManager.Instance.ActionQueueSet.EnqueueWithoutSynchronizing(RunManager.Instance.ActionQueueSynchronizer.GetHookActionForId(value, replayEvent.playerId.Value, value2));
					break;
				}
				case CombatReplayEventType.ResumeAction:
					RunManager.Instance.ActionQueueSet.ResumeActionWithoutSynchronizing(replayEvent.actionId.Value);
					break;
				case CombatReplayEventType.PlayerChoice:
				{
					Player player2 = runState.GetPlayer(replayEvent.playerId.Value);
					RunManager.Instance.PlayerChoiceSynchronizer.ReceiveReplayChoice(player2, replayEvent.choiceId.Value, replayEvent.playerChoiceResult.Value);
					break;
				}
				default:
					throw new InvalidEnumArgumentException();
				}
				replayEvent = default(CombatReplayEvent);
			}
			List<CombatReplayEvent>.Enumerator enumerator = default(List<CombatReplayEvent>.Enumerator);
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x001C1F08 File Offset: 0x001C0108
		private void WriteReplayAsSave(string path)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FileAccessStream fileAccessStream = new FileAccessStream(path, 1L))
				{
					fileAccessStream.CopyTo(memoryStream);
				}
				PacketReader packetReader = new PacketReader();
				packetReader.Reset(memoryStream.ToArray());
				CombatReplay combatReplay = packetReader.Read<CombatReplay>();
				if (this.ValidateReplay(combatReplay))
				{
					SerializableRun serializableRun = combatReplay.serializableRun;
					string text = JsonSerializer.Serialize<SerializableRun>(serializableRun, JsonSerializationUtility.GetTypeInfo<SerializableRun>());
					for (int i = 0; i < serializableRun.Players.Count; i++)
					{
						text = text.Replace(serializableRun.Players[i].NetId.ToString(), ((i == 0) ? 1 : (i * 1000)).ToString());
					}
					string text2 = ((serializableRun.Players.Count > 1) ? "current_run_mp.save" : "current_run.save");
					string text3 = Path.Combine(UserDataPathProvider.GetProfileScopedPath(SaveManager.Instance.CurrentProfileId, "saves", null, null), text2);
					Log.Info(text3, 2);
					using (FileAccess fileAccess = FileAccess.Open(text3, 2L))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						if (fileAccess == null)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Couldn't open ");
							defaultInterpolatedStringHandler.AppendFormatted(text3);
							defaultInterpolatedStringHandler.AppendLiteral(": ");
							defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						fileAccess.StoreString(text);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Wrote ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(text.Length);
						defaultInterpolatedStringHandler.AppendLiteral(" chars to ");
						defaultInterpolatedStringHandler.AppendFormatted(text3);
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x001C2134 File Offset: 0x001C0334
		private void OnCharacterChanged(CharacterModel model)
		{
			this._lobby.SetLocalCharacter(model);
			this._localPlayerData.CharacterId = model.Id;
			this._localPlayerData.CurrentHp = model.StartingHp;
			this._localPlayerData.MaxHp = model.StartingHp;
			this._localPlayerData.MaxEnergy = model.MaxEnergy;
			this._localPlayerData.MaxPotionSlotCount = 3;
			this._localPlayerData.Gold = model.StartingGold;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x001C21AE File Offset: 0x001C03AE
		public override void _Process(double delta)
		{
			StartRunLobby lobby = this._lobby;
			if (lobby == null)
			{
				return;
			}
			lobby.NetService.Update();
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x001C21C8 File Offset: 0x001C03C8
		public void PlayerConnected(LobbyPlayer player)
		{
			this._characterContainers[player.slotId].characterImage.Texture = player.character.IconTexture;
			this._characterContainers[player.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, player.id);
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001C2234 File Offset: 0x001C0434
		public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
		{
			this._characterContainers[player.slotId].characterImage.Texture = player.character.IconTexture;
			this._characterContainers[player.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, player.id);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x001C229D File Offset: 0x001C049D
		public void AscensionChanged()
		{
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x001C229F File Offset: 0x001C049F
		public void SeedChanged()
		{
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x001C22A1 File Offset: 0x001C04A1
		public void ModifiersChanged()
		{
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x001C22A3 File Offset: 0x001C04A3
		public void MaxAscensionChanged()
		{
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x001C22A5 File Offset: 0x001C04A5
		public void RemotePlayerDisconnected(LobbyPlayer player)
		{
			this._characterContainers[player.slotId].characterImage.Texture = null;
			this._characterContainers[player.slotId].playerName.Text = "?";
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x001C22E3 File Offset: 0x001C04E3
		public void LocalPlayerDisconnected(NetErrorInfo info)
		{
			this._lobby = null;
			this._characterPaginator.Visible = false;
			this._readyButton.Visible = false;
			this._characterPaginator.SetIndex(0);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B05 RID: 15109 RVA: 0x001C2310 File Offset: 0x001C0510
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NMultiplayerTest.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.AddGame, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.HostButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.JoinButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.ReadyButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.Disconnect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "reason", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerTest.MethodName.AfterMultiplayerStarted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.ChooseReplayToLoad, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.ChooseReplayToSave, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.LoadReplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerTest.MethodName.WriteReplayAsSave, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerTest.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerTest.MethodName.AscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.SeedChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.ModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTest.MethodName.MaxAscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B06 RID: 15110 RVA: 0x001C2708 File Offset: 0x001C0908
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerTest.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.AddGame && args.Count == 0)
			{
				this.AddGame();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.HostButtonPressed && args.Count == 0)
			{
				this.HostButtonPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.JoinButtonPressed && args.Count == 0)
			{
				this.JoinButtonPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.ReadyButtonPressed && args.Count == 0)
			{
				this.ReadyButtonPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.Disconnect && args.Count == 1)
			{
				this.Disconnect(VariantUtils.ConvertTo<NetError>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.AfterMultiplayerStarted && args.Count == 0)
			{
				this.AfterMultiplayerStarted();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.ChooseReplayToLoad && args.Count == 0)
			{
				this.ChooseReplayToLoad();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.ChooseReplayToSave && args.Count == 0)
			{
				this.ChooseReplayToSave();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.LoadReplay && args.Count == 1)
			{
				this.LoadReplay(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.WriteReplayAsSave && args.Count == 1)
			{
				this.WriteReplayAsSave(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.AscensionChanged && args.Count == 0)
			{
				this.AscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.SeedChanged && args.Count == 0)
			{
				this.SeedChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.ModifiersChanged && args.Count == 0)
			{
				this.ModifiersChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTest.MethodName.MaxAscensionChanged && args.Count == 0)
			{
				this.MaxAscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B07 RID: 15111 RVA: 0x001C29F0 File Offset: 0x001C0BF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerTest.MethodName._Ready || (ref method) == NMultiplayerTest.MethodName._EnterTree || (ref method) == NMultiplayerTest.MethodName._ExitTree || (ref method) == NMultiplayerTest.MethodName.AddGame || (ref method) == NMultiplayerTest.MethodName.HostButtonPressed || (ref method) == NMultiplayerTest.MethodName.JoinButtonPressed || (ref method) == NMultiplayerTest.MethodName.ReadyButtonPressed || (ref method) == NMultiplayerTest.MethodName.Disconnect || (ref method) == NMultiplayerTest.MethodName.AfterMultiplayerStarted || (ref method) == NMultiplayerTest.MethodName.ChooseReplayToLoad || (ref method) == NMultiplayerTest.MethodName.ChooseReplayToSave || (ref method) == NMultiplayerTest.MethodName.LoadReplay || (ref method) == NMultiplayerTest.MethodName.WriteReplayAsSave || (ref method) == NMultiplayerTest.MethodName._Process || (ref method) == NMultiplayerTest.MethodName.AscensionChanged || (ref method) == NMultiplayerTest.MethodName.SeedChanged || (ref method) == NMultiplayerTest.MethodName.ModifiersChanged || (ref method) == NMultiplayerTest.MethodName.MaxAscensionChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003B08 RID: 15112 RVA: 0x001C2B14 File Offset: 0x001C0D14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerTest.PropertyName._ipField)
			{
				this._ipField = VariantUtils.ConvertTo<TextEdit>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._idField)
			{
				this._idField = VariantUtils.ConvertTo<TextEdit>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._readyButton)
			{
				this._readyButton = VariantUtils.ConvertTo<Button>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._readyIndicator)
			{
				this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._loadingPanel)
			{
				this._loadingPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._characterPaginator)
			{
				this._characterPaginator = VariantUtils.ConvertTo<NMultiplayerTestCharacterPaginator>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._game)
			{
				this._game = VariantUtils.ConvertTo<NGame>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._ignoreReplayModelIdHash)
			{
				this._ignoreReplayModelIdHash = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._beginningRun)
			{
				this._beginningRun = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003B09 RID: 15113 RVA: 0x001C2C1C File Offset: 0x001C0E1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerTest.PropertyName._ipField)
			{
				value = VariantUtils.CreateFrom<TextEdit>(ref this._ipField);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._idField)
			{
				value = VariantUtils.CreateFrom<TextEdit>(ref this._idField);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._readyButton)
			{
				value = VariantUtils.CreateFrom<Button>(ref this._readyButton);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._readyIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._loadingPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._loadingPanel);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._characterPaginator)
			{
				value = VariantUtils.CreateFrom<NMultiplayerTestCharacterPaginator>(ref this._characterPaginator);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._game)
			{
				value = VariantUtils.CreateFrom<NGame>(ref this._game);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._ignoreReplayModelIdHash)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._ignoreReplayModelIdHash);
				return true;
			}
			if ((ref name) == NMultiplayerTest.PropertyName._beginningRun)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._beginningRun);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B0A RID: 15114 RVA: 0x001C2D54 File Offset: 0x001C0F54
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._ipField, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._idField, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._readyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._readyIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._loadingPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._characterPaginator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTest.PropertyName._game, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerTest.PropertyName._beginningRun, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B0B RID: 15115 RVA: 0x001C2E90 File Offset: 0x001C1090
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerTest.PropertyName._ipField, Variant.From<TextEdit>(ref this._ipField));
			info.AddProperty(NMultiplayerTest.PropertyName._idField, Variant.From<TextEdit>(ref this._idField));
			info.AddProperty(NMultiplayerTest.PropertyName._readyButton, Variant.From<Button>(ref this._readyButton));
			info.AddProperty(NMultiplayerTest.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
			info.AddProperty(NMultiplayerTest.PropertyName._loadingPanel, Variant.From<Control>(ref this._loadingPanel));
			info.AddProperty(NMultiplayerTest.PropertyName._characterPaginator, Variant.From<NMultiplayerTestCharacterPaginator>(ref this._characterPaginator));
			info.AddProperty(NMultiplayerTest.PropertyName._game, Variant.From<NGame>(ref this._game));
			info.AddProperty(NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, Variant.From<bool>(ref this._ignoreReplayModelIdHash));
			info.AddProperty(NMultiplayerTest.PropertyName._beginningRun, Variant.From<bool>(ref this._beginningRun));
		}

		/// <inheritdoc />
		// Token: 0x06003B0C RID: 15116 RVA: 0x001C2F6C File Offset: 0x001C116C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._ipField, ref variant))
			{
				this._ipField = variant.As<TextEdit>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._idField, ref variant2))
			{
				this._idField = variant2.As<TextEdit>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._readyButton, ref variant3))
			{
				this._readyButton = variant3.As<Button>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._readyIndicator, ref variant4))
			{
				this._readyIndicator = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._loadingPanel, ref variant5))
			{
				this._loadingPanel = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._characterPaginator, ref variant6))
			{
				this._characterPaginator = variant6.As<NMultiplayerTestCharacterPaginator>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._game, ref variant7))
			{
				this._game = variant7.As<NGame>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, ref variant8))
			{
				this._ignoreReplayModelIdHash = variant8.As<bool>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMultiplayerTest.PropertyName._beginningRun, ref variant9))
			{
				this._beginningRun = variant9.As<bool>();
			}
		}

		// Token: 0x040016F6 RID: 5878
		private const ushort _port = 33771;

		// Token: 0x040016F7 RID: 5879
		private TextEdit _ipField;

		// Token: 0x040016F8 RID: 5880
		private TextEdit _idField;

		// Token: 0x040016F9 RID: 5881
		private Button _readyButton;

		// Token: 0x040016FA RID: 5882
		private Control _readyIndicator;

		// Token: 0x040016FB RID: 5883
		private Control _loadingPanel;

		// Token: 0x040016FC RID: 5884
		private NMultiplayerTestCharacterPaginator _characterPaginator;

		// Token: 0x040016FD RID: 5885
		private readonly List<NMultiplayerTest.CharacterContainer> _characterContainers = new List<NMultiplayerTest.CharacterContainer>();

		// Token: 0x040016FE RID: 5886
		private NGame _game;

		// Token: 0x040016FF RID: 5887
		[Nullable(2)]
		private StartRunLobby _lobby;

		// Token: 0x04001700 RID: 5888
		private readonly SerializablePlayer _localPlayerData = new SerializablePlayer();

		// Token: 0x04001701 RID: 5889
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04001702 RID: 5890
		[Nullable(2)]
		private IBootstrapSettings _settings;

		// Token: 0x04001703 RID: 5891
		private bool _ignoreReplayModelIdHash;

		// Token: 0x04001704 RID: 5892
		private bool _beginningRun;

		// Token: 0x0200180B RID: 6155
		[Nullable(0)]
		private struct CharacterContainer
		{
			// Token: 0x04005DEA RID: 24042
			public TextureRect characterImage;

			// Token: 0x04005DEB RID: 24043
			public Label playerName;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200180C RID: 6156
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005DEC RID: 24044
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005DED RID: 24045
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005DEE RID: 24046
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'AddGame' method.
			/// </summary>
			// Token: 0x04005DEF RID: 24047
			public static readonly StringName AddGame = "AddGame";

			/// <summary>
			/// Cached name for the 'HostButtonPressed' method.
			/// </summary>
			// Token: 0x04005DF0 RID: 24048
			public static readonly StringName HostButtonPressed = "HostButtonPressed";

			/// <summary>
			/// Cached name for the 'JoinButtonPressed' method.
			/// </summary>
			// Token: 0x04005DF1 RID: 24049
			public static readonly StringName JoinButtonPressed = "JoinButtonPressed";

			/// <summary>
			/// Cached name for the 'ReadyButtonPressed' method.
			/// </summary>
			// Token: 0x04005DF2 RID: 24050
			public static readonly StringName ReadyButtonPressed = "ReadyButtonPressed";

			/// <summary>
			/// Cached name for the 'Disconnect' method.
			/// </summary>
			// Token: 0x04005DF3 RID: 24051
			public static readonly StringName Disconnect = "Disconnect";

			/// <summary>
			/// Cached name for the 'AfterMultiplayerStarted' method.
			/// </summary>
			// Token: 0x04005DF4 RID: 24052
			public static readonly StringName AfterMultiplayerStarted = "AfterMultiplayerStarted";

			/// <summary>
			/// Cached name for the 'ChooseReplayToLoad' method.
			/// </summary>
			// Token: 0x04005DF5 RID: 24053
			public static readonly StringName ChooseReplayToLoad = "ChooseReplayToLoad";

			/// <summary>
			/// Cached name for the 'ChooseReplayToSave' method.
			/// </summary>
			// Token: 0x04005DF6 RID: 24054
			public static readonly StringName ChooseReplayToSave = "ChooseReplayToSave";

			/// <summary>
			/// Cached name for the 'LoadReplay' method.
			/// </summary>
			// Token: 0x04005DF7 RID: 24055
			public static readonly StringName LoadReplay = "LoadReplay";

			/// <summary>
			/// Cached name for the 'WriteReplayAsSave' method.
			/// </summary>
			// Token: 0x04005DF8 RID: 24056
			public static readonly StringName WriteReplayAsSave = "WriteReplayAsSave";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005DF9 RID: 24057
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'AscensionChanged' method.
			/// </summary>
			// Token: 0x04005DFA RID: 24058
			public static readonly StringName AscensionChanged = "AscensionChanged";

			/// <summary>
			/// Cached name for the 'SeedChanged' method.
			/// </summary>
			// Token: 0x04005DFB RID: 24059
			public static readonly StringName SeedChanged = "SeedChanged";

			/// <summary>
			/// Cached name for the 'ModifiersChanged' method.
			/// </summary>
			// Token: 0x04005DFC RID: 24060
			public static readonly StringName ModifiersChanged = "ModifiersChanged";

			/// <summary>
			/// Cached name for the 'MaxAscensionChanged' method.
			/// </summary>
			// Token: 0x04005DFD RID: 24061
			public static readonly StringName MaxAscensionChanged = "MaxAscensionChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200180D RID: 6157
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_ipField' field.
			/// </summary>
			// Token: 0x04005DFE RID: 24062
			public static readonly StringName _ipField = "_ipField";

			/// <summary>
			/// Cached name for the '_idField' field.
			/// </summary>
			// Token: 0x04005DFF RID: 24063
			public static readonly StringName _idField = "_idField";

			/// <summary>
			/// Cached name for the '_readyButton' field.
			/// </summary>
			// Token: 0x04005E00 RID: 24064
			public static readonly StringName _readyButton = "_readyButton";

			/// <summary>
			/// Cached name for the '_readyIndicator' field.
			/// </summary>
			// Token: 0x04005E01 RID: 24065
			public static readonly StringName _readyIndicator = "_readyIndicator";

			/// <summary>
			/// Cached name for the '_loadingPanel' field.
			/// </summary>
			// Token: 0x04005E02 RID: 24066
			public static readonly StringName _loadingPanel = "_loadingPanel";

			/// <summary>
			/// Cached name for the '_characterPaginator' field.
			/// </summary>
			// Token: 0x04005E03 RID: 24067
			public static readonly StringName _characterPaginator = "_characterPaginator";

			/// <summary>
			/// Cached name for the '_game' field.
			/// </summary>
			// Token: 0x04005E04 RID: 24068
			public static readonly StringName _game = "_game";

			/// <summary>
			/// Cached name for the '_ignoreReplayModelIdHash' field.
			/// </summary>
			// Token: 0x04005E05 RID: 24069
			public static readonly StringName _ignoreReplayModelIdHash = "_ignoreReplayModelIdHash";

			/// <summary>
			/// Cached name for the '_beginningRun' field.
			/// </summary>
			// Token: 0x04005E06 RID: 24070
			public static readonly StringName _beginningRun = "_beginningRun";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200180E RID: 6158
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}

		// Token: 0x0200180F RID: 6159
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04005E07 RID: 24071
			[Nullable(0)]
			public static Action <0>__DeleteCloudSaves;
		}
	}
}
