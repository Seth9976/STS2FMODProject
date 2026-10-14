using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x02000331 RID: 817
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunScreen.cs")]
	public class NDailyRunScreen : NSubmenu, IStartRunLobbyListener
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x00147CF1 File Offset: 0x00145EF1
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NDailyRunScreen._scenePath };
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002DAE RID: 11694 RVA: 0x00147D01 File Offset: 0x00145F01
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00147D04 File Offset: 0x00145F04
		[NullableContext(2)]
		public static NDailyRunScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDailyRunScreen._scenePath).Instantiate<NDailyRunScreen>(0L);
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00147D28 File Offset: 0x00145F28
		public override void _Ready()
		{
			this.ConnectSignals();
			this._titleLabel = base.GetNode<MegaLabel>("%Title");
			this._disclaimer = base.GetNode<MegaLabel>("%Disclaimer");
			this._dateLabel = base.GetNode<MegaRichTextLabel>("%Date");
			this._embarkButton = base.GetNode<NConfirmButton>("%ConfirmButton");
			this._backButton = base.GetNode<NBackButton>("%BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("%UnreadyButton");
			this._timeLeftLabel = base.GetNode<MegaRichTextLabel>("%TimeLeft");
			this._leaderboard = base.GetNode<NDailyRunLeaderboard>("%Leaderboards");
			this._modifiersTitleLabel = base.GetNode<MegaLabel>("%ModifiersLabel");
			this._modifiersContainer = base.GetNode<Control>("%ModifiersContainer");
			this._characterContainer = base.GetNode<NDailyRunCharacterContainer>("ChallengeContainer/CenterContainer/HBoxContainer/CharacterContainer");
			this._remotePlayerContainer = base.GetNode<NRemoteLobbyPlayerContainer>("%RemotePlayerContainer");
			this._readyAndWaitingContainer = base.GetNode<Control>("%ReadyAndWaitingPanel");
			this._titleLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.DAILY_TITLE").GetFormattedText());
			this._disclaimer.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.disclaimer").GetFormattedText());
			this._modifiersTitleLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.MODIFIERS").GetFormattedText());
			this._dateLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.FETCHING_TIME").GetFormattedText());
			foreach (NDailyRunScreenModifier ndailyRunScreenModifier in this._modifiersContainer.GetChildren(false).OfType<NDailyRunScreenModifier>())
			{
				this._modifierContainers.Add(ndailyRunScreenModifier);
			}
			this._embarkButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._embarkButton.Disable();
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._unreadyButton.Disable();
			this._remotePlayerContainer.Visible = false;
			this._readyAndWaitingContainer.Visible = false;
			this._leaderboard.Cleanup();
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x00147FA4 File Offset: 0x001461A4
		public void InitializeMultiplayerAsHost(INetGameService gameService)
		{
			if (gameService.Type != NetGameType.Host)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with GameService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when hosting!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._netService = gameService;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00148000 File Offset: 0x00146200
		public void InitializeMultiplayerAsClient(INetGameService gameService, ClientLobbyJoinResponseMessage message)
		{
			if (gameService.Type != NetGameType.Client)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with GameService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when joining!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._netService = gameService;
			this._lobby = new StartRunLobby(GameMode.Daily, gameService, this, message.dailyTime.Value, -1);
			this._lobby.InitializeFromMessage(message);
			this.SetupLobbyParams(this._lobby);
			this.AfterLobbyInitialized();
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00148092 File Offset: 0x00146292
		public void InitializeSingleplayer()
		{
			this._netService = new NetSingleplayerGameService();
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x001480A0 File Offset: 0x001462A0
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			NetGameType type = this._netService.Type;
			bool flag = type - NetGameType.Singleplayer <= 1;
			if (flag)
			{
				TaskHelper.RunSafely(this.SetupLobbyForHostOrSingleplayer());
				return;
			}
			this.SetIsLoading(false);
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x001480E4 File Offset: 0x001462E4
		public override void OnSubmenuClosed()
		{
			this._embarkButton.Disable();
			this._remotePlayerContainer.Cleanup();
			this._leaderboard.Cleanup();
			StartRunLobby lobby = this._lobby;
			if (lobby != null && lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			}
			this.CleanUpLobby(true, NetError.Quit);
		}

		/// <summary>
		/// This is called:
		/// - Once when the screen is shown
		/// - Anytime when the player count changes
		/// </summary>
		// Token: 0x06002DB6 RID: 11702 RVA: 0x0014814C File Offset: 0x0014634C
		private void InitializeLeaderboard()
		{
			this._leaderboard.Initialize(this._lobby.DailyTime.Value.serverTime, this._lobby.Players.Select<LobbyPlayer, ulong>((LobbyPlayer p) => p.id), false);
		}

		/// <summary>
		/// Fetches the time from the time server and initializes the lobby for multiplayer or singleplayer.
		/// After this completes, _lobby will be initialized. Its time will be set with the time fetched from the time server,
		/// or with the local time, if we couldn't fetch the time from the server.
		/// </summary>
		// Token: 0x06002DB7 RID: 11703 RVA: 0x001481AC File Offset: 0x001463AC
		private async Task SetupLobbyForHostOrSingleplayer()
		{
			if (this._netService.Type != NetGameType.Host && this._netService.Type != NetGameType.Singleplayer)
			{
				throw new InvalidOperationException("Should only be called as host or singleplayer!");
			}
			this.SetIsLoading(true);
			TimeServerResult timeServerResult = await this.GetTimeServerTime();
			TimeServerResult timeServerResult2 = timeServerResult;
			if (GodotObject.IsInstanceValid(this))
			{
				this._lobby = new StartRunLobby(GameMode.Daily, this._netService, this, timeServerResult2, 4);
				this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
				this.SetupLobbyParams(this._lobby);
				this.AfterLobbyInitialized();
				this.SetIsLoading(false);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Daily initialized with seed: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lobby.Seed);
				defaultInterpolatedStringHandler.AppendLiteral(" time: ");
				defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset>(this.GetServerRelativeTime());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Attempt to get the time from the time server.
		/// If any time server request succeeded in the past, this uses the cached time server time. Otherwise, it requests
		/// a new time. Falls back to local time if the server is unreachable.
		/// </summary>
		// Token: 0x06002DB8 RID: 11704 RVA: 0x001481F0 File Offset: 0x001463F0
		private async Task<TimeServerResult> GetTimeServerTime()
		{
			TimeServerResult? result = null;
			Task<TimeServerResult?> requestTimeTask = TimeServer.RequestTimeTask;
			if (requestTimeTask != null && requestTimeTask.IsCompleted)
			{
				if (!TimeServer.RequestTimeTask.IsFaulted)
				{
					TimeServerResult? timeServerResult = await TimeServer.RequestTimeTask;
					result = timeServerResult;
				}
				if (result != null)
				{
					goto IL_0219;
				}
				try
				{
					result = await TimeServer.FetchDailyTime();
					goto IL_0219;
				}
				catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
				{
					Log.Error(ex.ToString(), 2);
					goto IL_0219;
				}
			}
			try
			{
				result = await TimeServer.FetchDailyTime();
			}
			catch (Exception ex2) when (ex2 is HttpRequestException || ex2 is TaskCanceledException)
			{
				Log.Error(ex2.ToString(), 2);
			}
			IL_0219:
			if (result == null)
			{
				Log.Info("Couldn't retrieve time from time server, using local time", 2);
				result = new TimeServerResult?(new TimeServerResult
				{
					serverTime = DateTimeOffset.UtcNow,
					localReceivedTime = DateTimeOffset.UtcNow
				});
			}
			return result.Value;
		}

		/// <summary>
		/// Returns the time on the Mega Crit time server.
		/// </summary>
		// Token: 0x06002DB9 RID: 11705 RVA: 0x0014822C File Offset: 0x0014642C
		private DateTimeOffset GetServerRelativeTime()
		{
			return this._lobby.DailyTime.Value.serverTime + (DateTimeOffset.UtcNow - this._lobby.DailyTime.Value.localReceivedTime);
		}

		/// <summary>
		/// This is called on both host and client after the lobby is setup to sync the state of the lobby.
		/// It is also called when any player leaves or rejoins the game to reroll modifiers so that CharacterCards can
		/// properly avoid hitting any characters that have been rolled.
		/// </summary>
		// Token: 0x06002DBA RID: 11706 RVA: 0x00148278 File Offset: 0x00146478
		private void SetupLobbyParams(StartRunLobby lobby)
		{
			DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
			string text = SeedHelper.CanonicalizeSeed(serverRelativeTime.ToString("dd_MM_yyyy"));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("dd_MM_yyyy_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(lobby.Players.Count);
			defaultInterpolatedStringHandler.AppendLiteral("p");
			string text2 = SeedHelper.CanonicalizeSeed(serverRelativeTime.ToString(defaultInterpolatedStringHandler.ToStringAndClear()));
			Rng rng = new Rng(StringHelper.GetDeterministicHashCode(text));
			Rng rng2 = new Rng(rng.NextUnsignedLong());
			Rng rng3 = new Rng(rng.NextUnsignedLong());
			Rng rng4 = new Rng(rng.NextUnsignedLong());
			CharacterModel characterModel = null;
			foreach (LobbyPlayer lobbyPlayer in lobby.Players)
			{
				CharacterModel characterModel2 = rng2.NextItem<CharacterModel>(ModelDb.AllCharacters);
				if (lobbyPlayer.id == lobby.LocalPlayer.id)
				{
					characterModel = characterModel2;
				}
			}
			int num = rng3.NextInt(0, 11);
			IReadOnlyCollection<ModifierModel> readOnlyCollection = ModifierModel.Pick2Good1Bad(rng4, this._lobby.Players.Select<LobbyPlayer, CharacterModel>((LobbyPlayer p) => p.character));
			NetGameType type = lobby.NetService.Type;
			bool flag = type - NetGameType.Singleplayer <= 1;
			if (flag)
			{
				if (lobby.Seed != text2)
				{
					lobby.SetSeed(text2);
				}
				if (lobby.Ascension != num)
				{
					lobby.SyncAscensionChange(num);
				}
				if (readOnlyCollection.Any<ModifierModel>((ModifierModel m) => lobby.Modifiers.FirstOrDefault<ModifierModel>(new Func<ModifierModel, bool>(m.IsEquivalent)) == null))
				{
					lobby.SetModifiers(readOnlyCollection);
				}
			}
			if (lobby.LocalPlayer.character != characterModel)
			{
				lobby.SetLocalCharacter(characterModel);
			}
			this.InitializeDisplay();
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x00148490 File Offset: 0x00146690
		private void InitializeDisplay()
		{
			if (this._lobby == null)
			{
				throw new InvalidOperationException("Tried to initialize daily run display before lobby was initialized!");
			}
			DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
			this._endOfDay = new DateTimeOffset(serverRelativeTime.Year, serverRelativeTime.Month, serverRelativeTime.Day, 0, 0, 0, TimeSpan.Zero) + TimeSpan.FromDays(1);
			this._remotePlayerContainer.Visible = this._lobby.NetService.Type.IsMultiplayer();
			CharacterModel character = this._lobby.LocalPlayer.character;
			this._characterContainer.Fill(character, this._lobby.LocalPlayer.id, this._lobby.Ascension, this._lobby.NetService);
			this._dateLabel.Modulate = StsColors.blue;
			this._dateLabel.Text = serverRelativeTime.ToString(NDailyRunScreen.dateFormat);
			for (int i = 0; i < this._lobby.Modifiers.Count; i++)
			{
				this._modifierContainers[i].Fill(this._lobby.Modifiers[i]);
			}
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x001485B4 File Offset: 0x001467B4
		private void SetIsLoading(bool isLoading)
		{
			if (isLoading)
			{
				this._remotePlayerContainer.Visible = false;
				this._readyAndWaitingContainer.Visible = false;
			}
			this._timeLeftLabel.Visible = !isLoading;
			this._characterContainer.Visible = !isLoading;
			this._modifiersTitleLabel.Visible = !isLoading;
			this._modifiersContainer.Visible = !isLoading;
			if (isLoading)
			{
				this._embarkButton.Disable();
				return;
			}
			this._embarkButton.Enable();
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x00148634 File Offset: 0x00146834
		public override void _Process(double delta)
		{
			if (this._lobby == null)
			{
				return;
			}
			DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
			if (serverRelativeTime > this._endOfDay)
			{
				this.SetupLobbyParams(this._lobby);
			}
			TimeSpan timeSpan = this._endOfDay - serverRelativeTime;
			int? lastSetTimeLeftSecond = this._lastSetTimeLeftSecond;
			int seconds = timeSpan.Seconds;
			if (!((lastSetTimeLeftSecond.GetValueOrDefault() == seconds) & (lastSetTimeLeftSecond != null)))
			{
				string text = timeSpan.ToString(NDailyRunScreen._timeLeftFormat);
				NDailyRunScreen._timeLeftLoc.Add("time", text);
				this._timeLeftLabel.Text = NDailyRunScreen._timeLeftLoc.GetFormattedText();
				this._lastSetTimeLeftSecond = new int?(timeSpan.Seconds);
			}
			if (this._lobby.NetService.IsConnected)
			{
				this._lobby.NetService.Update();
			}
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x00148703 File Offset: 0x00146903
		public void PlayerConnected(LobbyPlayer player)
		{
			this._remotePlayerContainer.OnPlayerConnected(player);
			this.SetupLobbyParams(this._lobby);
			this.InitializeLeaderboard();
			this.UpdateRichPresence();
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0014872C File Offset: 0x0014692C
		public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
		{
			if (isRandomCharacterResolution)
			{
				throw new InvalidOperationException("Random character is not currently allowed in daily!");
			}
			this._remotePlayerContainer.OnPlayerChanged(player);
			if (player.id == this._netService.NetId && this._netService.Type.IsMultiplayer())
			{
				this._characterContainer.SetIsReady(player.isReady);
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x00148789 File Offset: 0x00146989
		public void MaxAscensionChanged()
		{
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0014878B File Offset: 0x0014698B
		public void AscensionChanged()
		{
			this.InitializeDisplay();
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x00148793 File Offset: 0x00146993
		public void SeedChanged()
		{
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x00148795 File Offset: 0x00146995
		public void ModifiersChanged()
		{
			this.InitializeDisplay();
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x0014879D File Offset: 0x0014699D
		public void RemotePlayerDisconnected(LobbyPlayer player)
		{
			this._remotePlayerContainer.OnPlayerDisconnected(player);
			this.SetupLobbyParams(this._lobby);
			this.InitializeLeaderboard();
			this.UpdateRichPresence();
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x001487C4 File Offset: 0x001469C4
		public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			this._embarkButton.Disable();
			this._unreadyButton.Disable();
			if (this._lobby.NetService.Type == NetGameType.Singleplayer)
			{
				TaskHelper.RunSafely(this.StartNewSingleplayerRun(seed, acts, modifiers));
				return;
			}
			TaskHelper.RunSafely(this.StartNewMultiplayerRun(seed, acts, modifiers));
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x0014882C File Offset: 0x00146A2C
		public void LocalPlayerDisconnected(NetErrorInfo info)
		{
			if (info.SelfInitiated && info.GetReason() == NetError.Quit)
			{
				return;
			}
			if (!this.IsValid() || this._stack == null)
			{
				return;
			}
			if (this._stack.Peek() == this)
			{
				this._stack.Pop();
			}
			if (TestMode.IsOff)
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(info);
				if (nerrorPopup != null)
				{
					NModalContainer.Instance.Add(nerrorPopup, true);
				}
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x00148894 File Offset: 0x00146A94
		private void OnEmbarkPressed(NButton _)
		{
			this._embarkButton.Disable();
			this._backButton.Disable();
			this._lobby.SetReady(true);
			if (this._lobby.NetService.Type != NetGameType.Singleplayer && !this._lobby.IsAboutToBeginGame())
			{
				this._readyAndWaitingContainer.Visible = true;
				this._unreadyButton.Enable();
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x001488FA File Offset: 0x00146AFA
		private void OnUnreadyPressed(NButton _)
		{
			this._lobby.SetReady(false);
			this._readyAndWaitingContainer.Visible = false;
			this._embarkButton.Enable();
			this._backButton.Enable();
			this._unreadyButton.Disable();
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x00148938 File Offset: 0x00146B38
		private void UpdateRichPresence()
		{
			StartRunLobby lobby = this._lobby;
			if (lobby != null && lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("DAILY_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
			}
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x00148994 File Offset: 0x00146B94
		public async Task StartNewSingleplayerRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			int num = 0;
			try
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Embarking on a DAILY ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lobby.LocalPlayer.character.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" run with ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Players.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" players. Ascension: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
				defaultInterpolatedStringHandler.AppendFormatted(seed);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
				await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, acts, modifiers, seed, GameMode.Daily, this._lobby.Ascension, new DateTimeOffset?(this._lobby.DailyTime.Value.serverTime));
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception starting daily singleplayer run : ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.CleanUpLobby(true, NetError.InternalError);
				await NGame.Instance.ReturnToMainMenuWithInternalError(ex);
			}
			else
			{
				object obj = null;
				this.CleanUpLobby(false, NetError.Quit);
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x001489F0 File Offset: 0x00146BF0
		public async Task StartNewMultiplayerRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			int num = 0;
			try
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Embarking on a DAILY multiplayer run. Players: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<LobbyPlayer>(",", this._lobby.Players));
				defaultInterpolatedStringHandler.AppendLiteral(". Ascension: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
				defaultInterpolatedStringHandler.AppendFormatted(seed);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
				await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, acts, modifiers, seed, this._lobby.Ascension, new DateTimeOffset?(this._lobby.DailyTime.Value.serverTime));
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception starting daily multiplayer run : ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.CleanUpLobby(true, NetError.InternalError);
				await NGame.Instance.ReturnToMainMenuWithInternalError(ex);
			}
			else
			{
				object obj = null;
				this.CleanUpLobby(false, NetError.Quit);
			}
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x00148A4B File Offset: 0x00146C4B
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			StartRunLobby lobby = this._lobby;
			if (lobby != null)
			{
				lobby.CleanUp(disconnectSession, error);
			}
			this._lobby = null;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x00148A68 File Offset: 0x00146C68
		private void AfterLobbyInitialized()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((LobbyPlayer p) => p.id));
			NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			NGame.Instance.TimeoutOverlay.Initialize(this._lobby.NetService, true);
			this._remotePlayerContainer.Initialize(this._lobby, false);
			this.UpdateRichPresence();
			Logger.logLevelTypeMap[LogType.Network] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.Debug);
			Logger.logLevelTypeMap[LogType.Actions] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			Logger.logLevelTypeMap[LogType.GameSync] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			NGame.Instance.DebugSeedOverride = null;
			this._embarkButton.Enable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DCE RID: 11726 RVA: 0x00148B88 File Offset: 0x00146D88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NDailyRunScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.InitializeSingleplayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.InitializeLeaderboard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.InitializeDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.SetIsLoading, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isLoading", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunScreen.MethodName.MaxAscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.AscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.SeedChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.ModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDailyRunScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDailyRunScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunScreen.MethodName.AfterLobbyInitialized, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DCF RID: 11727 RVA: 0x00148FE8 File Offset: 0x001471E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunScreen.MethodName.Create && args.Count == 0)
			{
				NDailyRunScreen ndailyRunScreen = NDailyRunScreen.Create();
				ret = VariantUtils.CreateFrom<NDailyRunScreen>(ref ndailyRunScreen);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.InitializeSingleplayer && args.Count == 0)
			{
				this.InitializeSingleplayer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.InitializeLeaderboard && args.Count == 0)
			{
				this.InitializeLeaderboard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.InitializeDisplay && args.Count == 0)
			{
				this.InitializeDisplay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.SetIsLoading && args.Count == 1)
			{
				this.SetIsLoading(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.MaxAscensionChanged && args.Count == 0)
			{
				this.MaxAscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.AscensionChanged && args.Count == 0)
			{
				this.AscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.SeedChanged && args.Count == 0)
			{
				this.SeedChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.ModifiersChanged && args.Count == 0)
			{
				this.ModifiersChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScreen.MethodName.AfterLobbyInitialized && args.Count == 0)
			{
				this.AfterLobbyInitialized();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x001492F4 File Offset: 0x001474F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunScreen.MethodName.Create && args.Count == 0)
			{
				NDailyRunScreen ndailyRunScreen = NDailyRunScreen.Create();
				ret = VariantUtils.CreateFrom<NDailyRunScreen>(ref ndailyRunScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002DD1 RID: 11729 RVA: 0x00149334 File Offset: 0x00147534
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunScreen.MethodName.Create || (ref method) == NDailyRunScreen.MethodName._Ready || (ref method) == NDailyRunScreen.MethodName.InitializeSingleplayer || (ref method) == NDailyRunScreen.MethodName.OnSubmenuOpened || (ref method) == NDailyRunScreen.MethodName.OnSubmenuClosed || (ref method) == NDailyRunScreen.MethodName.InitializeLeaderboard || (ref method) == NDailyRunScreen.MethodName.InitializeDisplay || (ref method) == NDailyRunScreen.MethodName.SetIsLoading || (ref method) == NDailyRunScreen.MethodName._Process || (ref method) == NDailyRunScreen.MethodName.MaxAscensionChanged || (ref method) == NDailyRunScreen.MethodName.AscensionChanged || (ref method) == NDailyRunScreen.MethodName.SeedChanged || (ref method) == NDailyRunScreen.MethodName.ModifiersChanged || (ref method) == NDailyRunScreen.MethodName.OnEmbarkPressed || (ref method) == NDailyRunScreen.MethodName.OnUnreadyPressed || (ref method) == NDailyRunScreen.MethodName.UpdateRichPresence || (ref method) == NDailyRunScreen.MethodName.CleanUpLobby || (ref method) == NDailyRunScreen.MethodName.AfterLobbyInitialized || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002DD2 RID: 11730 RVA: 0x00149458 File Offset: 0x00147658
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunScreen.PropertyName._titleLabel)
			{
				this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._disclaimer)
			{
				this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._dateLabel)
			{
				this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._timeLeftLabel)
			{
				this._timeLeftLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._characterContainer)
			{
				this._characterContainer = VariantUtils.ConvertTo<NDailyRunCharacterContainer>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._embarkButton)
			{
				this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._leaderboard)
			{
				this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._modifiersTitleLabel)
			{
				this._modifiersTitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._modifiersContainer)
			{
				this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._readyAndWaitingContainer)
			{
				this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002DD3 RID: 11731 RVA: 0x001495CC File Offset: 0x001477CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._titleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._disclaimer)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._dateLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._timeLeftLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._timeLeftLabel);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._characterContainer)
			{
				value = VariantUtils.CreateFrom<NDailyRunCharacterContainer>(ref this._characterContainer);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._embarkButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._leaderboard)
			{
				value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._modifiersTitleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._modifiersTitleLabel);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._modifiersContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NDailyRunScreen.PropertyName._readyAndWaitingContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DD4 RID: 11732 RVA: 0x001497A4 File Offset: 0x001479A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._titleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._disclaimer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._dateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._timeLeftLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._characterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._embarkButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._leaderboard, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._modifiersTitleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._modifiersContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreen.PropertyName._readyAndWaitingContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DD5 RID: 11733 RVA: 0x00149988 File Offset: 0x00147B88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunScreen.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
			info.AddProperty(NDailyRunScreen.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
			info.AddProperty(NDailyRunScreen.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
			info.AddProperty(NDailyRunScreen.PropertyName._timeLeftLabel, Variant.From<MegaRichTextLabel>(ref this._timeLeftLabel));
			info.AddProperty(NDailyRunScreen.PropertyName._characterContainer, Variant.From<NDailyRunCharacterContainer>(ref this._characterContainer));
			info.AddProperty(NDailyRunScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
			info.AddProperty(NDailyRunScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NDailyRunScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NDailyRunScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
			info.AddProperty(NDailyRunScreen.PropertyName._modifiersTitleLabel, Variant.From<MegaLabel>(ref this._modifiersTitleLabel));
			info.AddProperty(NDailyRunScreen.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
			info.AddProperty(NDailyRunScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NDailyRunScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
		}

		/// <inheritdoc />
		// Token: 0x06002DD6 RID: 11734 RVA: 0x00149ABC File Offset: 0x00147CBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._titleLabel, ref variant))
			{
				this._titleLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._disclaimer, ref variant2))
			{
				this._disclaimer = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._dateLabel, ref variant3))
			{
				this._dateLabel = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._timeLeftLabel, ref variant4))
			{
				this._timeLeftLabel = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._characterContainer, ref variant5))
			{
				this._characterContainer = variant5.As<NDailyRunCharacterContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._embarkButton, ref variant6))
			{
				this._embarkButton = variant6.As<NConfirmButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._backButton, ref variant7))
			{
				this._backButton = variant7.As<NBackButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._unreadyButton, ref variant8))
			{
				this._unreadyButton = variant8.As<NBackButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._leaderboard, ref variant9))
			{
				this._leaderboard = variant9.As<NDailyRunLeaderboard>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._modifiersTitleLabel, ref variant10))
			{
				this._modifiersTitleLabel = variant10.As<MegaLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._modifiersContainer, ref variant11))
			{
				this._modifiersContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._remotePlayerContainer, ref variant12))
			{
				this._remotePlayerContainer = variant12.As<NRemoteLobbyPlayerContainer>();
			}
			Variant variant13;
			if (info.TryGetProperty(NDailyRunScreen.PropertyName._readyAndWaitingContainer, ref variant13))
			{
				this._readyAndWaitingContainer = variant13.As<Control>();
			}
		}

		// Token: 0x0400117A RID: 4474
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_screen");

		// Token: 0x0400117B RID: 4475
		private static readonly LocString _timeLeftLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.TIME_LEFT");

		// Token: 0x0400117C RID: 4476
		public static readonly string dateFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.DATE_FORMAT");

		// Token: 0x0400117D RID: 4477
		private static readonly string _timeLeftFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.TIME_FORMAT");

		// Token: 0x0400117E RID: 4478
		private MegaLabel _titleLabel;

		// Token: 0x0400117F RID: 4479
		private MegaLabel _disclaimer;

		// Token: 0x04001180 RID: 4480
		private MegaRichTextLabel _dateLabel;

		// Token: 0x04001181 RID: 4481
		private MegaRichTextLabel _timeLeftLabel;

		// Token: 0x04001182 RID: 4482
		private NDailyRunCharacterContainer _characterContainer;

		// Token: 0x04001183 RID: 4483
		private NConfirmButton _embarkButton;

		// Token: 0x04001184 RID: 4484
		private NBackButton _backButton;

		// Token: 0x04001185 RID: 4485
		private NBackButton _unreadyButton;

		// Token: 0x04001186 RID: 4486
		private NDailyRunLeaderboard _leaderboard;

		// Token: 0x04001187 RID: 4487
		private MegaLabel _modifiersTitleLabel;

		// Token: 0x04001188 RID: 4488
		private Control _modifiersContainer;

		// Token: 0x04001189 RID: 4489
		private readonly List<NDailyRunScreenModifier> _modifierContainers = new List<NDailyRunScreenModifier>();

		// Token: 0x0400118A RID: 4490
		private NRemoteLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x0400118B RID: 4491
		private Control _readyAndWaitingContainer;

		// Token: 0x0400118C RID: 4492
		private DateTimeOffset _endOfDay;

		// Token: 0x0400118D RID: 4493
		private INetGameService _netService;

		// Token: 0x0400118E RID: 4494
		[Nullable(2)]
		private StartRunLobby _lobby;

		// Token: 0x0400118F RID: 4495
		private int? _lastSetTimeLeftSecond;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A5 RID: 5541
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400530D RID: 21261
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400530E RID: 21262
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'InitializeSingleplayer' method.
			/// </summary>
			// Token: 0x0400530F RID: 21263
			public static readonly StringName InitializeSingleplayer = "InitializeSingleplayer";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04005310 RID: 21264
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04005311 RID: 21265
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'InitializeLeaderboard' method.
			/// </summary>
			// Token: 0x04005312 RID: 21266
			public static readonly StringName InitializeLeaderboard = "InitializeLeaderboard";

			/// <summary>
			/// Cached name for the 'InitializeDisplay' method.
			/// </summary>
			// Token: 0x04005313 RID: 21267
			public static readonly StringName InitializeDisplay = "InitializeDisplay";

			/// <summary>
			/// Cached name for the 'SetIsLoading' method.
			/// </summary>
			// Token: 0x04005314 RID: 21268
			public static readonly StringName SetIsLoading = "SetIsLoading";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005315 RID: 21269
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'MaxAscensionChanged' method.
			/// </summary>
			// Token: 0x04005316 RID: 21270
			public static readonly StringName MaxAscensionChanged = "MaxAscensionChanged";

			/// <summary>
			/// Cached name for the 'AscensionChanged' method.
			/// </summary>
			// Token: 0x04005317 RID: 21271
			public static readonly StringName AscensionChanged = "AscensionChanged";

			/// <summary>
			/// Cached name for the 'SeedChanged' method.
			/// </summary>
			// Token: 0x04005318 RID: 21272
			public static readonly StringName SeedChanged = "SeedChanged";

			/// <summary>
			/// Cached name for the 'ModifiersChanged' method.
			/// </summary>
			// Token: 0x04005319 RID: 21273
			public static readonly StringName ModifiersChanged = "ModifiersChanged";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x0400531A RID: 21274
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x0400531B RID: 21275
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x0400531C RID: 21276
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x0400531D RID: 21277
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'AfterLobbyInitialized' method.
			/// </summary>
			// Token: 0x0400531E RID: 21278
			public static readonly StringName AfterLobbyInitialized = "AfterLobbyInitialized";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A6 RID: 5542
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x0400531F RID: 21279
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_titleLabel' field.
			/// </summary>
			// Token: 0x04005320 RID: 21280
			public static readonly StringName _titleLabel = "_titleLabel";

			/// <summary>
			/// Cached name for the '_disclaimer' field.
			/// </summary>
			// Token: 0x04005321 RID: 21281
			public static readonly StringName _disclaimer = "_disclaimer";

			/// <summary>
			/// Cached name for the '_dateLabel' field.
			/// </summary>
			// Token: 0x04005322 RID: 21282
			public static readonly StringName _dateLabel = "_dateLabel";

			/// <summary>
			/// Cached name for the '_timeLeftLabel' field.
			/// </summary>
			// Token: 0x04005323 RID: 21283
			public static readonly StringName _timeLeftLabel = "_timeLeftLabel";

			/// <summary>
			/// Cached name for the '_characterContainer' field.
			/// </summary>
			// Token: 0x04005324 RID: 21284
			public static readonly StringName _characterContainer = "_characterContainer";

			/// <summary>
			/// Cached name for the '_embarkButton' field.
			/// </summary>
			// Token: 0x04005325 RID: 21285
			public static readonly StringName _embarkButton = "_embarkButton";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04005326 RID: 21286
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x04005327 RID: 21287
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_leaderboard' field.
			/// </summary>
			// Token: 0x04005328 RID: 21288
			public static readonly StringName _leaderboard = "_leaderboard";

			/// <summary>
			/// Cached name for the '_modifiersTitleLabel' field.
			/// </summary>
			// Token: 0x04005329 RID: 21289
			public static readonly StringName _modifiersTitleLabel = "_modifiersTitleLabel";

			/// <summary>
			/// Cached name for the '_modifiersContainer' field.
			/// </summary>
			// Token: 0x0400532A RID: 21290
			public static readonly StringName _modifiersContainer = "_modifiersContainer";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x0400532B RID: 21291
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_readyAndWaitingContainer' field.
			/// </summary>
			// Token: 0x0400532C RID: 21292
			public static readonly StringName _readyAndWaitingContainer = "_readyAndWaitingContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A7 RID: 5543
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
