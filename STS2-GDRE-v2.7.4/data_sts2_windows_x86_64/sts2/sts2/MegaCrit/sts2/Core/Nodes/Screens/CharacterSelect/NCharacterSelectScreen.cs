using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x0200033F RID: 831
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectScreen.cs")]
	public class NCharacterSelectScreen : NSubmenu, IStartRunLobbyListener, ICharacterSelectButtonDelegate
	{
		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x00156588 File Offset: 0x00154788
		public StartRunLobby Lobby
		{
			get
			{
				return this._lobby;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002F21 RID: 12065 RVA: 0x00156590 File Offset: 0x00154790
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(NCharacterSelectScreen._scenePath);
				list.Add("res://scenes/screens/char_select/char_select_button.tscn");
				list.AddRange(NCharacterSelectButton.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x001565BD File Offset: 0x001547BD
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._charButtonContainer.GetChild<Control>(0, false);
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x001565CC File Offset: 0x001547CC
		[NullableContext(2)]
		public static NCharacterSelectScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return ResourceLoader.Load<PackedScene>(NCharacterSelectScreen._scenePath, null, 1L).Instantiate<NCharacterSelectScreen>(0L);
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x001565EC File Offset: 0x001547EC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._infoPanel = base.GetNode<Control>("InfoPanel");
			this._name = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/Name");
			this._description = base.GetNode<MegaRichTextLabel>("InfoPanel/VBoxContainer/DescriptionLabel");
			this._hp = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Hp/Label");
			this._gold = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Gold/Label");
			this._relicTitle = base.GetNode<MegaRichTextLabel>("InfoPanel/VBoxContainer/Relic/Name/RichTextLabel");
			this._relicDescription = base.GetNode<MegaRichTextLabel>("InfoPanel/VBoxContainer/Relic/Description");
			this._relicIcon = base.GetNode<TextureRect>("InfoPanel/VBoxContainer/Relic/Icon");
			this._relicIconOutline = base.GetNode<TextureRect>("InfoPanel/VBoxContainer/Relic/Icon/Outline");
			this._bgContainer = base.GetNode<Control>("AnimatedBg");
			this._charButtonContainer = base.GetNode<Control>("CharSelectButtons/ButtonContainer");
			this._ascensionPanel = base.GetNode<NAscensionPanel>("%AscensionPanel");
			this._actDropdown = base.GetNode<NActDropdown>("%ActDropdown");
			this._actDropdownLabel = base.GetNode<MegaRichTextLabel>("ActLabel");
			this._remotePlayerContainer = base.GetNode<NRemoteLobbyPlayerContainer>("RemotePlayerContainer");
			this._readyAndWaitingContainer = base.GetNode<Control>("ReadyAndWaitingPanel");
			base.GetNode<MegaRichTextLabel>("%WaitingForPlayers").Text = new LocString("main_menu_ui", "CHARACTER_SELECT.waitingForPlayers").GetFormattedText();
			this._characterUnlockAnimationBackstop = base.GetNode<Control>("%CharacterUnlockAnimationBackstop");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("UnreadyButton");
			this._embarkButton = base.GetNode<NConfirmButton>("ConfirmButton");
			this._embarkButton.OverrideHotkeys(new string[] { MegaInput.select });
			this._embarkButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._ascensionPanel.Connect(NAscensionPanel.SignalName.AscensionLevelChanged, Callable.From(new Action(this.OnAscensionPanelLevelChanged)), 0U);
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._unreadyButton.Disable();
			this.InitCharacterButtons();
			Type type = BootstrapSettingsUtil.Get();
			if (type != null)
			{
				this._settings = (IBootstrapSettings)Activator.CreateInstance(type);
				PreloadManager.Enabled = this._settings.DoPreloading;
			}
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x001568A8 File Offset: 0x00154AA8
		public void InitializeMultiplayerAsHost(INetGameService gameService, int maxPlayers)
		{
			if (gameService.Type != NetGameType.Host)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with GameService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when hosting!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._lobby = new StartRunLobby(GameMode.Standard, gameService, this, maxPlayers);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Host);
			this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
			this.OnAscensionPanelLevelChanged();
			this.CheckForMultiplayerAscensionPopup();
			this.AfterInitialized();
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x00156954 File Offset: 0x00154B54
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
			this._lobby = new StartRunLobby(GameMode.Standard, gameService, this, -1);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Client);
			this._lobby.InitializeFromMessage(message);
			this.CheckForMultiplayerAscensionPopup();
			this.AfterInitialized();
		}

		/// <summary>
		/// Checks if we need to show the Ascension Multiplayer FTUE-like popup and displays one if so.
		/// </summary>
		// Token: 0x06002F27 RID: 12071 RVA: 0x001569DC File Offset: 0x00154BDC
		private void CheckForMultiplayerAscensionPopup()
		{
			if (SaveManager.Instance.Progress.MaxMultiplayerAscension > 0 && !SaveManager.Instance.SeenPopup("ascension_multiplayer_ftue"))
			{
				NAscensionMultiplayerFtue nascensionMultiplayerFtue = NAscensionMultiplayerFtue.Create();
				if (nascensionMultiplayerFtue != null)
				{
					NModalContainer.Instance.Add(nascensionMultiplayerFtue, true);
				}
			}
		}

		/// <summary>
		/// Opens this screen
		/// </summary>
		// Token: 0x06002F28 RID: 12072 RVA: 0x00156A24 File Offset: 0x00154C24
		public void InitializeSingleplayer()
		{
			this._lobby = new StartRunLobby(GameMode.Standard, new NetSingleplayerGameService(), this, 1);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Singleplayer);
			this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), 0);
			this.AfterInitialized();
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x00156A74 File Offset: 0x00154C74
		private void InitCharacterButtons()
		{
			foreach (CharacterModel characterModel in ModelDb.AllCharacters)
			{
				NCharacterSelectButton ncharacterSelectButton = this._charSelectButtonScene.Instantiate<NCharacterSelectButton>(0L);
				ncharacterSelectButton.Name = characterModel.Id.Entry + "_button";
				this._charButtonContainer.AddChildSafely(ncharacterSelectButton);
				ncharacterSelectButton.Init(characterModel, this);
			}
			this._randomCharacterButton = this._charSelectButtonScene.Instantiate<NCharacterSelectButton>(0L);
			this._charButtonContainer.AddChildSafely(this._randomCharacterButton);
			this._randomCharacterButton.Init(ModelDb.Character<RandomCharacter>(), this);
			this.UpdateRandomCharacterVisibility();
			List<NCharacterSelectButton> list = (from c in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>()
				where c.Visible
				select c).ToList<NCharacterSelectButton>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].FocusNeighborTop = list[i].GetPath();
				list[i].FocusNeighborBottom = list[i].GetPath();
				Control control = list[i];
				NodePath nodePath;
				if (i <= 0)
				{
					List<NCharacterSelectButton> list2 = list;
					nodePath = list2[list2.Count - 1].GetPath();
				}
				else
				{
					nodePath = list[i - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				list[i].FocusNeighborRight = ((i < list.Count - 1) ? list[i + 1].GetPath() : list[0].GetPath());
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x00156C2C File Offset: 0x00154E2C
		private void UpdateRandomCharacterVisibility()
		{
			if (this._lobby == null)
			{
				return;
			}
			bool flag = false;
			foreach (LobbyPlayer lobbyPlayer in this._lobby.Players)
			{
				UnlockState unlockState = UnlockState.FromSerializable(lobbyPlayer.unlockState);
				bool flag2 = true;
				foreach (CharacterModel characterModel in ModelDb.AllCharacters)
				{
					if (!unlockState.Characters.Contains(characterModel))
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					flag = true;
					break;
				}
			}
			this._randomCharacterButton.Visible = flag;
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x00156CFC File Offset: 0x00154EFC
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.unlockCharacters, false))
			{
				this.DebugUnlockAllCharacters();
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x00156D14 File Offset: 0x00154F14
		private void DebugUnlockAllCharacters()
		{
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.DebugUnlock();
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x00156D6C File Offset: 0x00154F6C
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (!ncharacterSelectButton.IsLocked)
				{
					ncharacterSelectButton.Enable();
				}
				else
				{
					ncharacterSelectButton.UnlockIfPossible();
				}
				ncharacterSelectButton.Reset();
			}
			this._embarkButton.Enable();
			if (SaveManager.Instance.Progress.PendingCharacterUnlock == ModelId.none)
			{
				this._charButtonContainer.GetChild<NCharacterSelectButton>(0, false).Select();
			}
			else
			{
				TaskHelper.RunSafely(this.PlayUnlockCharacterAnimation(SaveManager.Instance.Progress.PendingCharacterUnlock));
			}
			this._remotePlayerContainer.Visible = this._lobby.NetService.Type != NetGameType.Singleplayer;
			this._remotePlayerContainer.Initialize(this._lobby, true);
			if (this._lobby.NetService.Type == NetGameType.Client)
			{
				this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
			}
			this._actDropdown.Visible = this.ShouldShowActDropdown;
			this._actDropdownLabel.Visible = this._actDropdown.Visible;
			this._readyAndWaitingContainer.Visible = false;
			foreach (LobbyPlayer lobbyPlayer in this._lobby.Players)
			{
				this.RefreshButtonSelectionForPlayer(lobbyPlayer);
			}
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x00156F0C File Offset: 0x0015510C
		private async Task PlayUnlockCharacterAnimation(ModelId character)
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			this._backButton.Disable();
			this._embarkButton.Disable();
			this._infoPanel.Visible = false;
			this._characterUnlockAnimationBackstop.Visible = true;
			foreach (NCharacterSelectButton button in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (button.Character.Id == character)
				{
					button.LockForAnimation();
					await Cmd.Wait(0.3f, false);
					await button.AnimateUnlock();
					button.Select();
				}
				button = null;
			}
			IEnumerator<NCharacterSelectButton> enumerator = null;
			this._infoPanel.Visible = true;
			this._characterUnlockAnimationBackstop.Visible = false;
			this._backButton.Enable();
			this._embarkButton.Enable();
			SaveManager.Instance.Progress.PendingCharacterUnlock = ModelId.none;
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x00156F57 File Offset: 0x00155157
		private bool ShouldShowActDropdown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00156F5C File Offset: 0x0015515C
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			this._embarkButton.Disable();
			this._remotePlayerContainer.Cleanup();
			this._ascensionPanel.Cleanup();
			if (this._lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			}
			this.CleanUpLobby(true, NetError.Quit);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x00156FC4 File Offset: 0x001551C4
		private void OnEmbarkPressed(NButton _)
		{
			this._embarkButton.Disable();
			if (!SaveManager.Instance.SeenFtue("accept_tutorials_ftue"))
			{
				NModalContainer.Instance.Add(NAcceptTutorialsFtue.Create(this, delegate
				{
					this.OnEmbarkPressed(null);
				}), true);
				return;
			}
			NetGameType type = this._lobby.NetService.Type;
			bool flag = type - NetGameType.Singleplayer <= 1;
			if (flag)
			{
				this._lobby.Act1 = this._actDropdown.CurrentOption;
			}
			this._lobby.SetReady(true);
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.Disable();
			}
			this._backButton.Disable();
			if (this._lobby.NetService.Type.IsMultiplayer() && !this._lobby.IsAboutToBeginGame())
			{
				this._readyAndWaitingContainer.Visible = true;
				this._unreadyButton.Enable();
			}
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x001570E0 File Offset: 0x001552E0
		public override void _Process(double delta)
		{
			if (this._lobby == null)
			{
				return;
			}
			if (this._lobby.NetService.IsConnected)
			{
				this._lobby.NetService.Update();
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x0015710D File Offset: 0x0015530D
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			this._lobby.CleanUp(disconnectSession, error);
			this._lobby = null;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x00157124 File Offset: 0x00155324
		private void OnLocalCharacterChangedForRandom(CharacterModel characterModel)
		{
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 90f);
			}
			SfxCmd.Play(characterModel.CharacterSelectSfx, 1f);
			Control control = PreloadManager.Cache.GetScene(characterModel.CharacterSelectBg).Instantiate<Control>(0L);
			control.Name = characterModel.Id.Entry + "_bg";
			this._bgContainer.AddChildSafely(control);
			this._delayEmbarkForCharacterSelect = true;
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x001571A4 File Offset: 0x001553A4
		private async Task StartNewSingleplayerRun(string seed, List<ActModel> acts)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Embarking on a singleplayer ");
			defaultInterpolatedStringHandler.AppendFormatted(this._lobby.LocalPlayer.character.Id.Entry);
			defaultInterpolatedStringHandler.AppendLiteral(" run. Ascension: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
			defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
			defaultInterpolatedStringHandler.AppendFormatted(seed);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			int ascensionToEmbark = this._lobby.Ascension;
			if (this._delayEmbarkForCharacterSelect)
			{
				await Cmd.Wait(1f, false);
				this._delayEmbarkForCharacterSelect = false;
			}
			int num = 0;
			try
			{
				SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
				await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, acts, Array.Empty<ModifierModel>(), seed, GameMode.Standard, ascensionToEmbark, null);
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception starting singleplayer run : ");
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

		// Token: 0x06002F36 RID: 12086 RVA: 0x001571F8 File Offset: 0x001553F8
		private async Task StartNewMultiplayerRun(string seed, List<ActModel> acts)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Embarking on a multiplayer run. Players: ");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join<LobbyPlayer>(",", this._lobby.Players));
			defaultInterpolatedStringHandler.AppendLiteral(". Ascension: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
			defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
			defaultInterpolatedStringHandler.AppendFormatted(seed);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (this._delayEmbarkForCharacterSelect)
			{
				await Cmd.Wait(1f, false);
				this._delayEmbarkForCharacterSelect = false;
			}
			SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
			await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
			IBootstrapSettings settings = this._settings;
			if (settings != null && settings.BootstrapInMultiplayer)
			{
				RunState runState;
				using (new NetLoadingHandle(this._lobby.NetService))
				{
					acts[0] = this._settings.Act;
					int num = 0;
					try
					{
						runState = RunState.CreateForNewRun(this._lobby.Players.Select<LobbyPlayer, Player>((LobbyPlayer p) => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id)).ToList<Player>(), acts.Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), this._settings.Modifiers, GameMode.Standard, this._lobby.Ascension, seed);
						RunManager.Instance.SetUpNewMultiplayer(runState, this._lobby, this._settings.SaveRunHistory, null);
						await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Player p) => p.Character));
						await RunManager.Instance.FinalizeStartingRelics();
						RunManager.Instance.Launch();
						NGame.Instance.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
						await RunManager.Instance.SetActInternal(0);
						await SaveManager.Instance.SaveRun(null, true);
					}
					catch (Exception obj)
					{
						num = 1;
					}
					object obj;
					if (num == 1)
					{
						Exception ex = (Exception)obj;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Exception starting bootstrap multiplayer run : ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						this.CleanUpLobby(true, NetError.InternalError);
						await NGame.Instance.ReturnToMainMenuWithInternalError(ex);
						return;
					}
					obj = null;
					this.CleanUpLobby(false, NetError.Quit);
					await this._settings.Setup(LocalContext.GetMe(runState));
					switch (this._settings.RoomType)
					{
					case RoomType.Unassigned:
						await RunManager.Instance.EnterAct(0, true);
						goto IL_088E;
					case RoomType.Treasure:
					case RoomType.Shop:
					case RoomType.RestSite:
						await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, null, false);
						RunManager.Instance.ActionExecutor.Unpause();
						goto IL_088E;
					case RoomType.Event:
						await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, this._settings.Event, false);
						goto IL_088E;
					}
					await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, MapPointType.Unassigned, this._settings.RoomType.IsCombatRoom() ? this._settings.Encounter.ToMutable() : null, false);
					IL_088E:;
				}
				NetLoadingHandle loadHandle = null;
				runState = null;
			}
			else
			{
				int num = 0;
				try
				{
					await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, acts, Array.Empty<ModifierModel>(), seed, this._lobby.Ascension, null);
				}
				catch (Exception obj)
				{
					num = 1;
				}
				if (num == 1)
				{
					object obj;
					Exception ex2 = (Exception)obj;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Exception starting multiplayer run : ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex2);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this.CleanUpLobby(true, NetError.InternalError);
					await NGame.Instance.ReturnToMainMenuWithInternalError(ex2);
				}
				else
				{
					object obj = null;
					this.CleanUpLobby(false, NetError.Quit);
				}
			}
		}

		/// <summary>
		/// Called when the player selects a Character! Also called when you open the character select screen.
		/// </summary>
		// Token: 0x06002F37 RID: 12087 RVA: 0x0015724C File Offset: 0x0015544C
		public void SelectCharacter(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
		{
			if (!charSelectButton.IsRandom)
			{
				SfxCmd.Play(characterModel.CharacterSelectSfx, 1f);
			}
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 90f);
			}
			if (this._infoPanelTween != null)
			{
				this._infoPanel.Position = this._infoPanelPosFinalVal;
			}
			this._infoPanelPosFinalVal = this._infoPanel.Position;
			Tween infoPanelTween = this._infoPanelTween;
			if (infoPanelTween != null)
			{
				infoPanelTween.Kill();
			}
			this._infoPanelTween = base.CreateTween().SetParallel(true);
			this._infoPanelTween.TweenProperty(this._infoPanel, "position", this._infoPanel.Position, 0.5).SetEase(1L).SetTrans(5L)
				.From(this._infoPanel.Position - new Vector2(300f, 0f));
			foreach (Node node in this._bgContainer.GetChildren(false))
			{
				this._bgContainer.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
			this._selectedButton = charSelectButton;
			if (!charSelectButton.IsLocked)
			{
				this._embarkButton.Enable();
				Control control = PreloadManager.Cache.GetScene(characterModel.CharacterSelectBg).Instantiate<Control>(0L);
				control.Name = characterModel.Id.Entry + "_bg";
				this._bgContainer.AddChildSafely(control);
				string formattedText = new LocString("characters", characterModel.CharacterSelectTitle).GetFormattedText();
				this._name.SetTextAutoSize(formattedText);
				this._description.Text = new LocString("characters", characterModel.CharacterSelectDesc).GetFormattedText();
				if (!this._selectedButton.IsRandom)
				{
					MegaLabel hp = this._hp;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingHp);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingHp);
					hp.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
					MegaLabel gold = this._gold;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingGold);
					gold.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
					RelicModel relicModel = characterModel.StartingRelics[0];
					this._relicTitle.Text = relicModel.Title.GetFormattedText();
					this._relicDescription.Text = relicModel.DynamicDescription.GetFormattedText();
					this._relicIcon.Texture = relicModel.Icon;
					this._relicIconOutline.Texture = relicModel.IconOutline;
					this._relicIcon.SelfModulate = Colors.White;
					this._relicIconOutline.SelfModulate = StsColors.halfTransparentBlack;
				}
				else
				{
					this._hp.SetTextAutoSize("??/??");
					this._gold.SetTextAutoSize("???");
					this._relicIcon.SelfModulate = StsColors.transparentBlack;
					this._relicIconOutline.SelfModulate = StsColors.transparentBlack;
					this._relicTitle.Text = string.Empty;
					this._relicDescription.Text = string.Empty;
				}
				this._lobby.SetLocalCharacter(characterModel);
				if (!this._lobby.NetService.Type.IsMultiplayer())
				{
					this._ascensionPanel.AnimIn();
				}
			}
			else
			{
				this._embarkButton.Disable();
				string formattedText2 = new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title").GetFormattedText();
				this._name.SetTextAutoSize(formattedText2);
				this._description.Text = characterModel.GetUnlockText().GetFormattedText();
				this._hp.SetTextAutoSize("??/??");
				this._gold.SetTextAutoSize("???");
				if (!this._selectedButton.IsRandom)
				{
					RelicModel relicModel2 = characterModel.StartingRelics[0];
					this._relicTitle.Text = new LocString("main_menu_ui", "CHARACTER_SELECT.lockedRelic.title").GetFormattedText();
					this._relicDescription.Text = new LocString("main_menu_ui", "CHARACTER_SELECT.lockedRelic.description").GetFormattedText();
					this._relicIcon.Texture = relicModel2.Icon;
					this._relicIconOutline.Texture = relicModel2.IconOutline;
					this._relicIcon.SelfModulate = StsColors.ninetyPercentBlack;
					this._relicIconOutline.SelfModulate = StsColors.halfTransparentWhite;
				}
				else
				{
					this._relicIcon.SelfModulate = StsColors.transparentBlack;
					this._relicIconOutline.SelfModulate = StsColors.transparentBlack;
					this._relicTitle.Text = string.Empty;
					this._relicDescription.Text = string.Empty;
				}
				this._ascensionPanel.Visible = false;
			}
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (ncharacterSelectButton != this._selectedButton)
				{
					ncharacterSelectButton.Deselect();
				}
			}
		}

		/// <summary>
		/// Called when the ascension is changed from the ascension panel.
		/// When the host hits the left and right arrow, we want to send the ascension change to clients.
		/// On the clients, this is called when the ascension sync message is received, but we don't want to do anything.
		/// </summary>
		// Token: 0x06002F38 RID: 12088 RVA: 0x00157770 File Offset: 0x00155970
		private void OnAscensionPanelLevelChanged()
		{
			if (this._lobby.NetService.Type != NetGameType.Client && this._lobby.Ascension != this._ascensionPanel.Ascension)
			{
				this._lobby.SyncAscensionChange(this._ascensionPanel.Ascension);
			}
		}

		/// <summary>
		/// Called when the player presses the unready button in multiplayer. You can't unready in singleplayer.
		/// </summary>
		// Token: 0x06002F39 RID: 12089 RVA: 0x001577C0 File Offset: 0x001559C0
		private void OnUnreadyPressed(NButton _)
		{
			this._lobby.SetReady(false);
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.Enable();
			}
			NCharacterSelectButton selectedButton = this._selectedButton;
			if (selectedButton != null)
			{
				selectedButton.TryGrabFocus();
			}
			this._readyAndWaitingContainer.Visible = false;
			this._embarkButton.Enable();
			this._backButton.Enable();
			this._unreadyButton.Disable();
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x00157864 File Offset: 0x00155A64
		private void UpdateRichPresence()
		{
			if (this._lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("STANDARD_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
			}
		}

		/// <summary>
		/// Called when another player joins with a max ascension level lower than the current one.
		/// Also called when the lobby is first initialized.
		/// Also called in singleplayer if choosing a character changes the max ascension.
		/// Called on both host and client.
		/// </summary>
		// Token: 0x06002F3B RID: 12091 RVA: 0x001578B7 File Offset: 0x00155AB7
		public void MaxAscensionChanged()
		{
			this._ascensionPanel.SetMaxAscension(this._lobby.MaxAscension);
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x001578CF File Offset: 0x00155ACF
		public void PlayerConnected(LobbyPlayer player)
		{
			this._remotePlayerContainer.OnPlayerConnected(player);
			this.RefreshButtonSelectionForPlayer(player);
			this.UpdateRichPresence();
			this.UpdateRandomCharacterVisibility();
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x001578F0 File Offset: 0x00155AF0
		public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
		{
			if (player.id == this._lobby.LocalPlayer.id && isRandomCharacterResolution)
			{
				this.OnLocalCharacterChangedForRandom(player.character);
			}
			this._remotePlayerContainer.OnPlayerChanged(player);
			this.RefreshButtonSelectionForPlayer(player);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x0015792C File Offset: 0x00155B2C
		private void RefreshButtonSelectionForPlayer(LobbyPlayer player)
		{
			if (player.id == this._lobby.LocalPlayer.id)
			{
				return;
			}
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (ncharacterSelectButton.RemoteSelectedPlayers.Contains(player.id) && player.character != ncharacterSelectButton.Character)
				{
					ncharacterSelectButton.OnRemotePlayerDeselected(player.id);
				}
				else if (player.character == ncharacterSelectButton.Character)
				{
					ncharacterSelectButton.OnRemotePlayerSelected(player.id);
				}
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x001579E0 File Offset: 0x00155BE0
		public void AscensionChanged()
		{
			if (this._lobby.NetService.Type == NetGameType.Client)
			{
				this._ascensionPanel.Visible = this._lobby.Ascension > 0;
			}
			this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x00157A2F File Offset: 0x00155C2F
		public void SeedChanged()
		{
			throw new NotImplementedException("Seed should not be changed in standard mode!");
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x00157A3B File Offset: 0x00155C3B
		public void ModifiersChanged()
		{
			throw new NotImplementedException("Modifiers should not be changed in standard mode!");
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x00157A48 File Offset: 0x00155C48
		public void RemotePlayerDisconnected(LobbyPlayer player)
		{
			this._remotePlayerContainer.OnPlayerDisconnected(player);
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (ncharacterSelectButton.RemoteSelectedPlayers.Contains(player.id) && player.character == ncharacterSelectButton.Character)
				{
					ncharacterSelectButton.OnRemotePlayerDeselected(player.id);
				}
			}
			this.UpdateRichPresence();
			this.UpdateRandomCharacterVisibility();
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x00157AE0 File Offset: 0x00155CE0
		public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			if (modifiers.Count > 0)
			{
				Log.Error("Modifiers list is not empty while starting a standard run, ignoring!", 2);
			}
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			this._ascensionPanel.Cleanup();
			this._ascensionPanel.Disable();
			this._embarkButton.Disable();
			this._unreadyButton.Disable();
			if (this._lobby.NetService.Type == NetGameType.Singleplayer)
			{
				TaskHelper.RunSafely(this.StartNewSingleplayerRun(seed, acts));
				return;
			}
			TaskHelper.RunSafely(this.StartNewMultiplayerRun(seed, acts));
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x00157B70 File Offset: 0x00155D70
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

		// Token: 0x06002F45 RID: 12101 RVA: 0x00157BD8 File Offset: 0x00155DD8
		private void AfterInitialized()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((LobbyPlayer p) => p.id));
			NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			NGame.Instance.TimeoutOverlay.Initialize(this._lobby.NetService, true);
			this.UpdateRichPresence();
			this.UpdateRandomCharacterVisibility();
			Logger.logLevelTypeMap[LogType.Network] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.Debug);
			Logger.logLevelTypeMap[LogType.Actions] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			Logger.logLevelTypeMap[LogType.GameSync] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			if (this._lobby.NetService.Type != NetGameType.Singleplayer)
			{
				IBootstrapSettings settings = this._settings;
				if (settings != null && settings.BootstrapInMultiplayer)
				{
					NGame.Instance.DebugSeedOverride = this._settings.Seed;
					return;
				}
			}
			NGame.Instance.DebugSeedOverride = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F46 RID: 12102 RVA: 0x00157D20 File Offset: 0x00155F20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(21)
			{
				new MethodInfo(NCharacterSelectScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.InitializeSingleplayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.InitCharacterButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCharacterSelectScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.MaxAscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.AscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.SeedChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.ModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreen.MethodName.AfterInitialized, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F47 RID: 12103 RVA: 0x00158218 File Offset: 0x00156418
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCharacterSelectScreen.MethodName.Create && args.Count == 0)
			{
				NCharacterSelectScreen ncharacterSelectScreen = NCharacterSelectScreen.Create();
				ret = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref ncharacterSelectScreen);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup && args.Count == 0)
			{
				this.CheckForMultiplayerAscensionPopup();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.InitializeSingleplayer && args.Count == 0)
			{
				this.InitializeSingleplayer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.InitCharacterButtons && args.Count == 0)
			{
				this.InitCharacterButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility && args.Count == 0)
			{
				this.UpdateRandomCharacterVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters && args.Count == 0)
			{
				this.DebugUnlockAllCharacters();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged && args.Count == 0)
			{
				this.OnAscensionPanelLevelChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.MaxAscensionChanged && args.Count == 0)
			{
				this.MaxAscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.AscensionChanged && args.Count == 0)
			{
				this.AscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.SeedChanged && args.Count == 0)
			{
				this.SeedChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.ModifiersChanged && args.Count == 0)
			{
				this.ModifiersChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreen.MethodName.AfterInitialized && args.Count == 0)
			{
				this.AfterInitialized();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x00158590 File Offset: 0x00156790
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCharacterSelectScreen.MethodName.Create && args.Count == 0)
			{
				NCharacterSelectScreen ncharacterSelectScreen = NCharacterSelectScreen.Create();
				ret = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref ncharacterSelectScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002F49 RID: 12105 RVA: 0x001585D0 File Offset: 0x001567D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCharacterSelectScreen.MethodName.Create || (ref method) == NCharacterSelectScreen.MethodName._Ready || (ref method) == NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup || (ref method) == NCharacterSelectScreen.MethodName.InitializeSingleplayer || (ref method) == NCharacterSelectScreen.MethodName.InitCharacterButtons || (ref method) == NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility || (ref method) == NCharacterSelectScreen.MethodName._Input || (ref method) == NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters || (ref method) == NCharacterSelectScreen.MethodName.OnSubmenuOpened || (ref method) == NCharacterSelectScreen.MethodName.OnSubmenuClosed || (ref method) == NCharacterSelectScreen.MethodName.OnEmbarkPressed || (ref method) == NCharacterSelectScreen.MethodName._Process || (ref method) == NCharacterSelectScreen.MethodName.CleanUpLobby || (ref method) == NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged || (ref method) == NCharacterSelectScreen.MethodName.OnUnreadyPressed || (ref method) == NCharacterSelectScreen.MethodName.UpdateRichPresence || (ref method) == NCharacterSelectScreen.MethodName.MaxAscensionChanged || (ref method) == NCharacterSelectScreen.MethodName.AscensionChanged || (ref method) == NCharacterSelectScreen.MethodName.SeedChanged || (ref method) == NCharacterSelectScreen.MethodName.ModifiersChanged || (ref method) == NCharacterSelectScreen.MethodName.AfterInitialized || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002F4A RID: 12106 RVA: 0x00158720 File Offset: 0x00156920
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCharacterSelectScreen.PropertyName._name)
			{
				this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanel)
			{
				this._infoPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._hp)
			{
				this._hp = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._gold)
			{
				this._gold = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicTitle)
			{
				this._relicTitle = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicDescription)
			{
				this._relicDescription = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicIcon)
			{
				this._relicIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicIconOutline)
			{
				this._relicIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._selectedButton)
			{
				this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._charButtonContainer)
			{
				this._charButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._bgContainer)
			{
				this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._readyAndWaitingContainer)
			{
				this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._embarkButton)
			{
				this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._ascensionPanel)
			{
				this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._actDropdown)
			{
				this._actDropdown = VariantUtils.ConvertTo<NActDropdown>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._actDropdownLabel)
			{
				this._actDropdownLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop)
			{
				this._characterUnlockAnimationBackstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._randomCharacterButton)
			{
				this._randomCharacterButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanelTween)
			{
				this._infoPanelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal)
			{
				this._infoPanelPosFinalVal = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect)
			{
				this._delayEmbarkForCharacterSelect = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._charSelectButtonScene)
			{
				this._charSelectButtonScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002F4B RID: 12107 RVA: 0x001589F4 File Offset: 0x00156BF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCharacterSelectScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName.ShouldShowActDropdown)
			{
				bool shouldShowActDropdown = this.ShouldShowActDropdown;
				value = VariantUtils.CreateFrom<bool>(ref shouldShowActDropdown);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._name)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._infoPanel);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._hp)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._hp);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._gold)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._gold);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicTitle)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicTitle);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicDescription)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicDescription);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._relicIcon);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._relicIconOutline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._relicIconOutline);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._selectedButton)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._charButtonContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._charButtonContainer);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._bgContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._readyAndWaitingContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._embarkButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._ascensionPanel)
			{
				value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._actDropdown)
			{
				value = VariantUtils.CreateFrom<NActDropdown>(ref this._actDropdown);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._actDropdownLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._actDropdownLabel);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterUnlockAnimationBackstop);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._randomCharacterButton)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._randomCharacterButton);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._infoPanelTween);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._infoPanelPosFinalVal);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._delayEmbarkForCharacterSelect);
				return true;
			}
			if ((ref name) == NCharacterSelectScreen.PropertyName._charSelectButtonScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._charSelectButtonScene);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F4C RID: 12108 RVA: 0x00158D90 File Offset: 0x00156F90
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._name, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._infoPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._hp, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._gold, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._relicTitle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._relicDescription, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._relicIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._relicIconOutline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._selectedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._charButtonContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._bgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._embarkButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._ascensionPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._actDropdown, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._actDropdownLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._randomCharacterButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._infoPanelTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName._charSelectButtonScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NCharacterSelectScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectScreen.PropertyName.ShouldShowActDropdown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F4D RID: 12109 RVA: 0x00159140 File Offset: 0x00157340
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCharacterSelectScreen.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
			info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanel, Variant.From<Control>(ref this._infoPanel));
			info.AddProperty(NCharacterSelectScreen.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NCharacterSelectScreen.PropertyName._hp, Variant.From<MegaLabel>(ref this._hp));
			info.AddProperty(NCharacterSelectScreen.PropertyName._gold, Variant.From<MegaLabel>(ref this._gold));
			info.AddProperty(NCharacterSelectScreen.PropertyName._relicTitle, Variant.From<MegaRichTextLabel>(ref this._relicTitle));
			info.AddProperty(NCharacterSelectScreen.PropertyName._relicDescription, Variant.From<MegaRichTextLabel>(ref this._relicDescription));
			info.AddProperty(NCharacterSelectScreen.PropertyName._relicIcon, Variant.From<TextureRect>(ref this._relicIcon));
			info.AddProperty(NCharacterSelectScreen.PropertyName._relicIconOutline, Variant.From<TextureRect>(ref this._relicIconOutline));
			info.AddProperty(NCharacterSelectScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
			info.AddProperty(NCharacterSelectScreen.PropertyName._charButtonContainer, Variant.From<Control>(ref this._charButtonContainer));
			info.AddProperty(NCharacterSelectScreen.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
			info.AddProperty(NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
			info.AddProperty(NCharacterSelectScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NCharacterSelectScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NCharacterSelectScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
			info.AddProperty(NCharacterSelectScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
			info.AddProperty(NCharacterSelectScreen.PropertyName._actDropdown, Variant.From<NActDropdown>(ref this._actDropdown));
			info.AddProperty(NCharacterSelectScreen.PropertyName._actDropdownLabel, Variant.From<MegaRichTextLabel>(ref this._actDropdownLabel));
			info.AddProperty(NCharacterSelectScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, Variant.From<Control>(ref this._characterUnlockAnimationBackstop));
			info.AddProperty(NCharacterSelectScreen.PropertyName._randomCharacterButton, Variant.From<NCharacterSelectButton>(ref this._randomCharacterButton));
			info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanelTween, Variant.From<Tween>(ref this._infoPanelTween));
			info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, Variant.From<Vector2>(ref this._infoPanelPosFinalVal));
			info.AddProperty(NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, Variant.From<bool>(ref this._delayEmbarkForCharacterSelect));
			info.AddProperty(NCharacterSelectScreen.PropertyName._charSelectButtonScene, Variant.From<PackedScene>(ref this._charSelectButtonScene));
		}

		/// <inheritdoc />
		// Token: 0x06002F4E RID: 12110 RVA: 0x00159390 File Offset: 0x00157590
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._name, ref variant))
			{
				this._name = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanel, ref variant2))
			{
				this._infoPanel = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._hp, ref variant4))
			{
				this._hp = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._gold, ref variant5))
			{
				this._gold = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicTitle, ref variant6))
			{
				this._relicTitle = variant6.As<MegaRichTextLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicDescription, ref variant7))
			{
				this._relicDescription = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicIcon, ref variant8))
			{
				this._relicIcon = variant8.As<TextureRect>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicIconOutline, ref variant9))
			{
				this._relicIconOutline = variant9.As<TextureRect>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._selectedButton, ref variant10))
			{
				this._selectedButton = variant10.As<NCharacterSelectButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._charButtonContainer, ref variant11))
			{
				this._charButtonContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._bgContainer, ref variant12))
			{
				this._bgContainer = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, ref variant13))
			{
				this._readyAndWaitingContainer = variant13.As<Control>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._backButton, ref variant14))
			{
				this._backButton = variant14.As<NBackButton>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._unreadyButton, ref variant15))
			{
				this._unreadyButton = variant15.As<NBackButton>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._embarkButton, ref variant16))
			{
				this._embarkButton = variant16.As<NConfirmButton>();
			}
			Variant variant17;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._ascensionPanel, ref variant17))
			{
				this._ascensionPanel = variant17.As<NAscensionPanel>();
			}
			Variant variant18;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._actDropdown, ref variant18))
			{
				this._actDropdown = variant18.As<NActDropdown>();
			}
			Variant variant19;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._actDropdownLabel, ref variant19))
			{
				this._actDropdownLabel = variant19.As<MegaRichTextLabel>();
			}
			Variant variant20;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._remotePlayerContainer, ref variant20))
			{
				this._remotePlayerContainer = variant20.As<NRemoteLobbyPlayerContainer>();
			}
			Variant variant21;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, ref variant21))
			{
				this._characterUnlockAnimationBackstop = variant21.As<Control>();
			}
			Variant variant22;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._randomCharacterButton, ref variant22))
			{
				this._randomCharacterButton = variant22.As<NCharacterSelectButton>();
			}
			Variant variant23;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanelTween, ref variant23))
			{
				this._infoPanelTween = variant23.As<Tween>();
			}
			Variant variant24;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, ref variant24))
			{
				this._infoPanelPosFinalVal = variant24.As<Vector2>();
			}
			Variant variant25;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, ref variant25))
			{
				this._delayEmbarkForCharacterSelect = variant25.As<bool>();
			}
			Variant variant26;
			if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._charSelectButtonScene, ref variant26))
			{
				this._charSelectButtonScene = variant26.As<PackedScene>();
			}
		}

		// Token: 0x0400120F RID: 4623
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/character_select_screen");

		// Token: 0x04001210 RID: 4624
		private MegaLabel _name;

		// Token: 0x04001211 RID: 4625
		private Control _infoPanel;

		// Token: 0x04001212 RID: 4626
		private MegaRichTextLabel _description;

		// Token: 0x04001213 RID: 4627
		private MegaLabel _hp;

		// Token: 0x04001214 RID: 4628
		private MegaLabel _gold;

		// Token: 0x04001215 RID: 4629
		private MegaRichTextLabel _relicTitle;

		// Token: 0x04001216 RID: 4630
		private MegaRichTextLabel _relicDescription;

		// Token: 0x04001217 RID: 4631
		private TextureRect _relicIcon;

		// Token: 0x04001218 RID: 4632
		private TextureRect _relicIconOutline;

		// Token: 0x04001219 RID: 4633
		[Nullable(2)]
		private NCharacterSelectButton _selectedButton;

		// Token: 0x0400121A RID: 4634
		private Control _charButtonContainer;

		// Token: 0x0400121B RID: 4635
		private Control _bgContainer;

		// Token: 0x0400121C RID: 4636
		private Control _readyAndWaitingContainer;

		// Token: 0x0400121D RID: 4637
		private NBackButton _backButton;

		// Token: 0x0400121E RID: 4638
		private NBackButton _unreadyButton;

		// Token: 0x0400121F RID: 4639
		private NConfirmButton _embarkButton;

		// Token: 0x04001220 RID: 4640
		private NAscensionPanel _ascensionPanel;

		// Token: 0x04001221 RID: 4641
		private NActDropdown _actDropdown;

		// Token: 0x04001222 RID: 4642
		private MegaRichTextLabel _actDropdownLabel;

		// Token: 0x04001223 RID: 4643
		private NRemoteLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x04001224 RID: 4644
		private Control _characterUnlockAnimationBackstop;

		// Token: 0x04001225 RID: 4645
		private NCharacterSelectButton _randomCharacterButton;

		// Token: 0x04001226 RID: 4646
		[Nullable(2)]
		private Tween _infoPanelTween;

		// Token: 0x04001227 RID: 4647
		private Vector2 _infoPanelPosFinalVal;

		// Token: 0x04001228 RID: 4648
		private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";

		// Token: 0x04001229 RID: 4649
		private bool _delayEmbarkForCharacterSelect;

		// Token: 0x0400122A RID: 4650
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _charSelectButtonScene;

		// Token: 0x0400122B RID: 4651
		[Nullable(2)]
		private IBootstrapSettings _settings;

		// Token: 0x0400122C RID: 4652
		private StartRunLobby _lobby;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015DF RID: 5599
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005475 RID: 21621
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005476 RID: 21622
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CheckForMultiplayerAscensionPopup' method.
			/// </summary>
			// Token: 0x04005477 RID: 21623
			public static readonly StringName CheckForMultiplayerAscensionPopup = "CheckForMultiplayerAscensionPopup";

			/// <summary>
			/// Cached name for the 'InitializeSingleplayer' method.
			/// </summary>
			// Token: 0x04005478 RID: 21624
			public static readonly StringName InitializeSingleplayer = "InitializeSingleplayer";

			/// <summary>
			/// Cached name for the 'InitCharacterButtons' method.
			/// </summary>
			// Token: 0x04005479 RID: 21625
			public static readonly StringName InitCharacterButtons = "InitCharacterButtons";

			/// <summary>
			/// Cached name for the 'UpdateRandomCharacterVisibility' method.
			/// </summary>
			// Token: 0x0400547A RID: 21626
			public static readonly StringName UpdateRandomCharacterVisibility = "UpdateRandomCharacterVisibility";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x0400547B RID: 21627
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'DebugUnlockAllCharacters' method.
			/// </summary>
			// Token: 0x0400547C RID: 21628
			public static readonly StringName DebugUnlockAllCharacters = "DebugUnlockAllCharacters";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x0400547D RID: 21629
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x0400547E RID: 21630
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x0400547F RID: 21631
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005480 RID: 21632
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x04005481 RID: 21633
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'OnAscensionPanelLevelChanged' method.
			/// </summary>
			// Token: 0x04005482 RID: 21634
			public static readonly StringName OnAscensionPanelLevelChanged = "OnAscensionPanelLevelChanged";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x04005483 RID: 21635
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x04005484 RID: 21636
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the 'MaxAscensionChanged' method.
			/// </summary>
			// Token: 0x04005485 RID: 21637
			public static readonly StringName MaxAscensionChanged = "MaxAscensionChanged";

			/// <summary>
			/// Cached name for the 'AscensionChanged' method.
			/// </summary>
			// Token: 0x04005486 RID: 21638
			public static readonly StringName AscensionChanged = "AscensionChanged";

			/// <summary>
			/// Cached name for the 'SeedChanged' method.
			/// </summary>
			// Token: 0x04005487 RID: 21639
			public static readonly StringName SeedChanged = "SeedChanged";

			/// <summary>
			/// Cached name for the 'ModifiersChanged' method.
			/// </summary>
			// Token: 0x04005488 RID: 21640
			public static readonly StringName ModifiersChanged = "ModifiersChanged";

			/// <summary>
			/// Cached name for the 'AfterInitialized' method.
			/// </summary>
			// Token: 0x04005489 RID: 21641
			public static readonly StringName AfterInitialized = "AfterInitialized";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E0 RID: 5600
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x0400548A RID: 21642
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the 'ShouldShowActDropdown' property.
			/// </summary>
			// Token: 0x0400548B RID: 21643
			public static readonly StringName ShouldShowActDropdown = "ShouldShowActDropdown";

			/// <summary>
			/// Cached name for the '_name' field.
			/// </summary>
			// Token: 0x0400548C RID: 21644
			public static readonly StringName _name = "_name";

			/// <summary>
			/// Cached name for the '_infoPanel' field.
			/// </summary>
			// Token: 0x0400548D RID: 21645
			public static readonly StringName _infoPanel = "_infoPanel";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x0400548E RID: 21646
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_hp' field.
			/// </summary>
			// Token: 0x0400548F RID: 21647
			public static readonly StringName _hp = "_hp";

			/// <summary>
			/// Cached name for the '_gold' field.
			/// </summary>
			// Token: 0x04005490 RID: 21648
			public static readonly StringName _gold = "_gold";

			/// <summary>
			/// Cached name for the '_relicTitle' field.
			/// </summary>
			// Token: 0x04005491 RID: 21649
			public static readonly StringName _relicTitle = "_relicTitle";

			/// <summary>
			/// Cached name for the '_relicDescription' field.
			/// </summary>
			// Token: 0x04005492 RID: 21650
			public static readonly StringName _relicDescription = "_relicDescription";

			/// <summary>
			/// Cached name for the '_relicIcon' field.
			/// </summary>
			// Token: 0x04005493 RID: 21651
			public static readonly StringName _relicIcon = "_relicIcon";

			/// <summary>
			/// Cached name for the '_relicIconOutline' field.
			/// </summary>
			// Token: 0x04005494 RID: 21652
			public static readonly StringName _relicIconOutline = "_relicIconOutline";

			/// <summary>
			/// Cached name for the '_selectedButton' field.
			/// </summary>
			// Token: 0x04005495 RID: 21653
			public static readonly StringName _selectedButton = "_selectedButton";

			/// <summary>
			/// Cached name for the '_charButtonContainer' field.
			/// </summary>
			// Token: 0x04005496 RID: 21654
			public static readonly StringName _charButtonContainer = "_charButtonContainer";

			/// <summary>
			/// Cached name for the '_bgContainer' field.
			/// </summary>
			// Token: 0x04005497 RID: 21655
			public static readonly StringName _bgContainer = "_bgContainer";

			/// <summary>
			/// Cached name for the '_readyAndWaitingContainer' field.
			/// </summary>
			// Token: 0x04005498 RID: 21656
			public static readonly StringName _readyAndWaitingContainer = "_readyAndWaitingContainer";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04005499 RID: 21657
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x0400549A RID: 21658
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_embarkButton' field.
			/// </summary>
			// Token: 0x0400549B RID: 21659
			public static readonly StringName _embarkButton = "_embarkButton";

			/// <summary>
			/// Cached name for the '_ascensionPanel' field.
			/// </summary>
			// Token: 0x0400549C RID: 21660
			public static readonly StringName _ascensionPanel = "_ascensionPanel";

			/// <summary>
			/// Cached name for the '_actDropdown' field.
			/// </summary>
			// Token: 0x0400549D RID: 21661
			public static readonly StringName _actDropdown = "_actDropdown";

			/// <summary>
			/// Cached name for the '_actDropdownLabel' field.
			/// </summary>
			// Token: 0x0400549E RID: 21662
			public static readonly StringName _actDropdownLabel = "_actDropdownLabel";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x0400549F RID: 21663
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_characterUnlockAnimationBackstop' field.
			/// </summary>
			// Token: 0x040054A0 RID: 21664
			public static readonly StringName _characterUnlockAnimationBackstop = "_characterUnlockAnimationBackstop";

			/// <summary>
			/// Cached name for the '_randomCharacterButton' field.
			/// </summary>
			// Token: 0x040054A1 RID: 21665
			public static readonly StringName _randomCharacterButton = "_randomCharacterButton";

			/// <summary>
			/// Cached name for the '_infoPanelTween' field.
			/// </summary>
			// Token: 0x040054A2 RID: 21666
			public static readonly StringName _infoPanelTween = "_infoPanelTween";

			/// <summary>
			/// Cached name for the '_infoPanelPosFinalVal' field.
			/// </summary>
			// Token: 0x040054A3 RID: 21667
			public static readonly StringName _infoPanelPosFinalVal = "_infoPanelPosFinalVal";

			/// <summary>
			/// Cached name for the '_delayEmbarkForCharacterSelect' field.
			/// </summary>
			// Token: 0x040054A4 RID: 21668
			public static readonly StringName _delayEmbarkForCharacterSelect = "_delayEmbarkForCharacterSelect";

			/// <summary>
			/// Cached name for the '_charSelectButtonScene' field.
			/// </summary>
			// Token: 0x040054A5 RID: 21669
			public static readonly StringName _charSelectButtonScene = "_charSelectButtonScene";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E1 RID: 5601
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
