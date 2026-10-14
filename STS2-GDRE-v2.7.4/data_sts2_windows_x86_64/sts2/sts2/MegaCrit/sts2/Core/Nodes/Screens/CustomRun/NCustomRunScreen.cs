using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.UI;
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
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun
{
	/// <summary>
	/// Allows the player to start a run with any set of ascension, character, and modifiers.
	/// </summary>
	// Token: 0x02000338 RID: 824
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunScreen.cs")]
	public class NCustomRunScreen : NSubmenu, IStartRunLobbyListener, ICharacterSelectButtonDelegate
	{
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x0014DA74 File Offset: 0x0014BC74
		private string ModifiersHotkey
		{
			get
			{
				return MegaInput.topPanel;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0014DA80 File Offset: 0x0014BC80
		public StartRunLobby Lobby
		{
			get
			{
				return this._lobby;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x0014DA88 File Offset: 0x0014BC88
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NCustomRunScreen._scenePath,
					"res://scenes/screens/char_select/char_select_button.tscn",
					"res://scenes/screens/custom_run/modifier_tickbox.tscn"
				});
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x0014DAAD File Offset: 0x0014BCAD
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._charButtonContainer.GetChild<Control>(0, false);
			}
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x0014DABC File Offset: 0x0014BCBC
		[NullableContext(2)]
		public static NCustomRunScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCustomRunScreen._scenePath).Instantiate<NCustomRunScreen>(0L);
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x0014DAE0 File Offset: 0x0014BCE0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._disclaimer = base.GetNode<MegaLabel>("%Disclaimer");
			this._charButtonContainer = base.GetNode<Control>("LeftContainer/CharSelectButtons/ButtonContainer");
			this._ascensionPanel = base.GetNode<NAscensionPanel>("%AscensionPanel");
			this._remotePlayerContainer = base.GetNode<NRemoteLobbyPlayerContainer>("%RemotePlayerContainer");
			this._readyAndWaitingContainer = base.GetNode<Control>("%ReadyAndWaitingPanel");
			this._modifiersList = base.GetNode<NCustomRunModifiersList>("%ModifiersList");
			this._seedInput = base.GetNode<LineEdit>("%SeedInput");
			this._randomizeButton = base.GetNode<NCustomRunRandomizeButton>("%CustomRunRandomizeButton");
			this._confirmButton = base.GetNode<NConfirmButton>("ConfirmButton");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("UnreadyButton");
			this._modifiersHotkeyIcon = base.GetNode<TextureRect>("%ModifiersHotkeyIcon");
			this._randomizeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRandomizePressed)), 0U);
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._ascensionPanel.Connect(NAscensionPanel.SignalName.AscensionLevelChanged, Callable.From(new Action(this.OnAscensionPanelLevelChanged)), 0U);
			this._modifiersList.Connect(NCustomRunModifiersList.SignalName.ModifiersChanged, Callable.From(new Action(this.OnModifiersListChanged)), 0U);
			this._disclaimer.SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.disclaimer").GetFormattedText());
			base.GetNode<MegaLabel>("%CustomModeTitle").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.CUSTOM_MODE_TITLE").GetFormattedText());
			base.GetNode<MegaLabel>("%ModifiersTitle").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIERS_TITLE").GetFormattedText());
			base.GetNode<MegaLabel>("%SeedLabel").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_LABEL").GetFormattedText());
			this._seedInput.PlaceholderText = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_RANDOM_PLACEHOLDER").GetFormattedText();
			this._seedInput.Connect(LineEdit.SignalName.TextChanged, Callable.From<string>(new Action<string>(this.OnSeedInputSubmitted)), 0U);
			this.InitCharacterButtons();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x0014DDF4 File Offset: 0x0014BFF4
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
			this._lobby = new StartRunLobby(GameMode.Custom, gameService, this, maxPlayers);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Host);
			this._modifiersList.Initialize(MultiplayerUiMode.Host);
			this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
			this._uiMode = MultiplayerUiMode.Host;
			this._remotePlayerContainer.Visible = true;
			this.UpdateControllerButton();
			this.OnAscensionPanelLevelChanged();
			this.AfterInitialized();
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0014DEBC File Offset: 0x0014C0BC
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
			this._lobby = new StartRunLobby(GameMode.Custom, gameService, this, -1);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Client);
			this._modifiersList.Initialize(MultiplayerUiMode.Client);
			this._lobby.InitializeFromMessage(message);
			this._seedInput.Editable = false;
			this._randomizeButton.Disable();
			this._uiMode = MultiplayerUiMode.Client;
			this.UpdateControllerButton();
			this.AfterInitialized();
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0014DF6C File Offset: 0x0014C16C
		public void InitializeSingleplayer()
		{
			this._lobby = new StartRunLobby(GameMode.Custom, new NetSingleplayerGameService(), this, 1);
			this._remotePlayerContainer.Visible = false;
			this._ascensionPanel.Initialize(MultiplayerUiMode.Singleplayer);
			this._modifiersList.Initialize(MultiplayerUiMode.Singleplayer);
			this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), 0);
			this._uiMode = MultiplayerUiMode.Singleplayer;
			this.UpdateControllerButton();
			this.AfterInitialized();
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0014DFDF File Offset: 0x0014C1DF
		private void OnSeedInputSubmitted(string newText)
		{
			if (newText != string.Empty)
			{
				this.Lobby.SetSeed(newText);
				return;
			}
			this.Lobby.SetSeed(null);
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0014E008 File Offset: 0x0014C208
		private void InitCharacterButtons()
		{
			foreach (CharacterModel characterModel in ModelDb.AllCharacters)
			{
				NCharacterSelectButton ncharacterSelectButton = PreloadManager.Cache.GetScene("res://scenes/screens/char_select/char_select_button.tscn").Instantiate<NCharacterSelectButton>(0L);
				ncharacterSelectButton.Name = characterModel.Id.Entry + "_button";
				this._charButtonContainer.AddChildSafely(ncharacterSelectButton);
				ncharacterSelectButton.Init(characterModel, this);
			}
			for (int i = 0; i < this._charButtonContainer.GetChildCount(false); i++)
			{
				Control child = this._charButtonContainer.GetChild<Control>(i, false);
				child.FocusNeighborLeft = ((i > 0) ? this._charButtonContainer.GetChild<Control>(i - 1, false).GetPath() : child.GetPath());
				child.FocusNeighborRight = ((i < this._charButtonContainer.GetChildCount(false) - 1) ? this._charButtonContainer.GetChild<Control>(i + 1, false).GetPath() : child.GetPath());
				child.FocusNeighborTop = this._seedInput.GetPath();
				child.FocusNeighborBottom = child.GetPath();
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0014E144 File Offset: 0x0014C344
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.unlockCharacters, false))
			{
				this.DebugUnlockAllCharacters();
			}
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0014E15C File Offset: 0x0014C35C
		private void DebugUnlockAllCharacters()
		{
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.DebugUnlock();
			}
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0014E1B4 File Offset: 0x0014C3B4
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (!ncharacterSelectButton.IsLocked)
				{
					ncharacterSelectButton.Enable();
					ncharacterSelectButton.Reset();
				}
				else
				{
					ncharacterSelectButton.UnlockIfPossible();
				}
			}
			this._confirmButton.Enable();
			this._charButtonContainer.GetChild<NCharacterSelectButton>(0, false).Select();
			this._remotePlayerContainer.Initialize(this._lobby, true);
			if (this._lobby.NetService.Type == NetGameType.Client)
			{
				this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
				this._seedInput.Text = this._lobby.Seed ?? "";
			}
			this._readyAndWaitingContainer.Visible = false;
			foreach (LobbyPlayer lobbyPlayer in this._lobby.Players)
			{
				this.RefreshButtonSelectionForPlayer(lobbyPlayer);
			}
			NHotkeyManager.Instance.PushHotkeyPressedBinding(this.ModifiersHotkey, new Action(this.TryFocusOnModifiersList));
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x0014E310 File Offset: 0x0014C510
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			this._confirmButton.Disable();
			this._remotePlayerContainer.Cleanup();
			if (this._lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			}
			this.CleanUpLobby(true, NetError.Quit);
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(this.ModifiersHotkey, new Action(this.TryFocusOnModifiersList));
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x0014E388 File Offset: 0x0014C588
		private void OnRandomizePressed(NButton _)
		{
			if (this._lobby.NetService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("In multiplayer, only the host can randomize custom runs.");
			}
			if (this._lobby.NetService.Type == NetGameType.Singleplayer)
			{
				this.RandomizeLocalCharacter();
			}
			IReadOnlyCollection<ModifierModel> readOnlyCollection = ModifierModel.Pick2Good1Bad(Rng.Chaotic, this._lobby.Players.Select<LobbyPlayer, CharacterModel>((LobbyPlayer p) => p.character));
			this._modifiersList.SetTickedModifiers(readOnlyCollection);
		}

		/// <summary>
		/// Randomly selects one of the unlocked characters for the local player only. Selecting through the button routes
		/// to <see cref="M:MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NCustomRunScreen.SelectCharacter(MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NCharacterSelectButton,MegaCrit.Sts2.Core.Models.CharacterModel)" /> -&gt; SetLocalCharacter, so other players in a multiplayer lobby keep their own
		/// selections.
		/// </summary>
		// Token: 0x06002E5F RID: 11871 RVA: 0x0014E414 File Offset: 0x0014C614
		private void RandomizeLocalCharacter()
		{
			IEnumerable<NCharacterSelectButton> enumerable = from b in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>()
				where b != null && !b.IsLocked && !b.IsRandom
				select b;
			NCharacterSelectButton ncharacterSelectButton = Rng.Chaotic.NextItem<NCharacterSelectButton>(enumerable);
			if (ncharacterSelectButton == null)
			{
				return;
			}
			ncharacterSelectButton.Select();
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0014E46C File Offset: 0x0014C66C
		private void OnEmbarkPressed(NButton _)
		{
			this._randomizeButton.Disable();
			this._confirmButton.Disable();
			this._backButton.Disable();
			this._lobby.SetReady(true);
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.Disable();
			}
			if (this._lobby.NetService.Type.IsMultiplayer() && !this._lobby.IsAboutToBeginGame())
			{
				this._unreadyButton.Enable();
				this._readyAndWaitingContainer.Visible = true;
			}
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x0014E52C File Offset: 0x0014C72C
		private void OnUnreadyPressed(NButton _)
		{
			if (this._lobby.NetService.Type != NetGameType.Client)
			{
				this._randomizeButton.Enable();
			}
			this._confirmButton.Enable();
			this._backButton.Enable();
			this._unreadyButton.Disable();
			this._lobby.SetReady(false);
			this._readyAndWaitingContainer.Visible = false;
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				ncharacterSelectButton.Enable();
			}
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x0014E5DC File Offset: 0x0014C7DC
		private void UpdateRichPresence()
		{
			if (this._lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("CUSTOM_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
			}
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x0014E62F File Offset: 0x0014C82F
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

		// Token: 0x06002E64 RID: 11876 RVA: 0x0014E65C File Offset: 0x0014C85C
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			this._lobby.CleanUp(disconnectSession, error);
			this._lobby = null;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0014E674 File Offset: 0x0014C874
		private async Task StartNewSingleplayerRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			int num = 0;
			try
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Embarking on a CUSTOM ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lobby.LocalPlayer.character.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" run. Ascension: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lobby.Seed);
				defaultInterpolatedStringHandler.AppendLiteral(" Modifiers: ");
				defaultInterpolatedStringHandler.AppendFormatted(this.GetModifiersString());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
				await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, acts, modifiers, seed, GameMode.Custom, this._lobby.Ascension, null);
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception starting custom singleplayer run : ");
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

		// Token: 0x06002E66 RID: 11878 RVA: 0x0014E6D0 File Offset: 0x0014C8D0
		private async Task StartNewMultiplayerRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			int num = 0;
			try
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Embarking on a CUSTOM multiplayer run. Players: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<LobbyPlayer>(",", this._lobby.Players));
				defaultInterpolatedStringHandler.AppendLiteral(". Ascension: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lobby.Ascension);
				defaultInterpolatedStringHandler.AppendLiteral(" Seed: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lobby.Seed);
				defaultInterpolatedStringHandler.AppendLiteral(" Modifiers: ");
				defaultInterpolatedStringHandler.AppendFormatted(this.GetModifiersString());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, this._lobby.LocalPlayer.character.CharacterSelectTransitionPath, null);
				await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, acts, modifiers, seed, this._lobby.Ascension, null);
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
				defaultInterpolatedStringHandler.AppendLiteral("Exception starting custom multiplayer run : ");
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

		// Token: 0x06002E67 RID: 11879 RVA: 0x0014E72B File Offset: 0x0014C92B
		private string GetModifiersString()
		{
			return string.Join<ModelId>(",", this._lobby.Modifiers.Select<ModifierModel, ModelId>((ModifierModel m) => m.Id));
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0014E768 File Offset: 0x0014C968
		public void SelectCharacter(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
		{
			if (this._lobby == null)
			{
				throw new InvalidOperationException("Cannot select character while loading!");
			}
			SfxCmd.Play(characterModel.CharacterSelectSfx, 1f);
			this._selectedButton = charSelectButton;
			foreach (NCharacterSelectButton ncharacterSelectButton in this._charButtonContainer.GetChildren(false).OfType<NCharacterSelectButton>())
			{
				if (ncharacterSelectButton != this._selectedButton)
				{
					ncharacterSelectButton.Deselect();
				}
			}
			this._lobby.SetLocalCharacter(characterModel);
		}

		/// <summary>
		/// Called when the ascension is changed from the ascension panel.
		/// When the host hits the left and right arrow, we want to send the ascension change to clients.
		/// On the clients, this is called when the ascension sync message is received, but we don't want to do anything.
		/// </summary>
		// Token: 0x06002E69 RID: 11881 RVA: 0x0014E800 File Offset: 0x0014CA00
		private void OnAscensionPanelLevelChanged()
		{
			if (this._lobby.NetService.Type != NetGameType.Client && this._lobby.Ascension != this._ascensionPanel.Ascension)
			{
				this._lobby.SyncAscensionChange(this._ascensionPanel.Ascension);
			}
		}

		/// <summary>
		/// Called when the custom modifiers are changed from the modifiers list.
		/// When the host ticks a modifier on or off, we want to send that change to clients.
		/// On the clients, this is called after the modifiers list syncs with the modifier sync message, but we don't need
		/// to do anything here.
		/// </summary>
		// Token: 0x06002E6A RID: 11882 RVA: 0x0014E84E File Offset: 0x0014CA4E
		private void OnModifiersListChanged()
		{
			if (this._lobby.NetService.Type != NetGameType.Client)
			{
				this.Lobby.SetModifiers(this._modifiersList.GetModifiersTickedOn());
			}
		}

		/// <summary>
		/// Called when another player joins with a max ascension level lower than the current one.
		/// Also called when the lobby is first initialized.
		/// Also called in singleplayer if the max ascension changes due to the character changing.
		/// Called on both host and client.
		/// </summary>
		// Token: 0x06002E6B RID: 11883 RVA: 0x0014E879 File Offset: 0x0014CA79
		public void MaxAscensionChanged()
		{
			this._ascensionPanel.SetMaxAscension(this._lobby.MaxAscension);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x0014E891 File Offset: 0x0014CA91
		public void PlayerConnected(LobbyPlayer player)
		{
			this._remotePlayerContainer.OnPlayerConnected(player);
			this.RefreshButtonSelectionForPlayer(player);
			this.UpdateRichPresence();
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x0014E8AC File Offset: 0x0014CAAC
		public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
		{
			if (isRandomCharacterResolution)
			{
				throw new InvalidOperationException("Random character is not currently allowed in custom!");
			}
			this._remotePlayerContainer.OnPlayerChanged(player);
			this.RefreshButtonSelectionForPlayer(player);
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x0014E8D0 File Offset: 0x0014CAD0
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

		// Token: 0x06002E6F RID: 11887 RVA: 0x0014E984 File Offset: 0x0014CB84
		public void AscensionChanged()
		{
			if (this._lobby.NetService.Type == NetGameType.Client)
			{
				this._ascensionPanel.Visible = this._lobby.Ascension > 0;
			}
			this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x0014E9D4 File Offset: 0x0014CBD4
		public void SeedChanged()
		{
			NetGameType type = this._lobby.NetService.Type;
			bool flag = type - NetGameType.Singleplayer <= 1;
			if (flag)
			{
				return;
			}
			this._seedInput.Text = this.Lobby.Seed;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x0014EA18 File Offset: 0x0014CC18
		public void ModifiersChanged()
		{
			NetGameType type = this._lobby.NetService.Type;
			bool flag = type - NetGameType.Singleplayer <= 1;
			if (flag)
			{
				return;
			}
			this._modifiersList.SyncModifierList(this.Lobby.Modifiers);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x0014EA5C File Offset: 0x0014CC5C
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
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x0014EAEC File Offset: 0x0014CCEC
		public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
		{
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			this._confirmButton.Disable();
			this._unreadyButton.Disable();
			if (this._lobby.NetService.Type == NetGameType.Singleplayer)
			{
				TaskHelper.RunSafely(this.StartNewSingleplayerRun(seed, acts, modifiers));
				return;
			}
			TaskHelper.RunSafely(this.StartNewMultiplayerRun(seed, acts, modifiers));
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x0014EB54 File Offset: 0x0014CD54
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

		// Token: 0x06002E75 RID: 11893 RVA: 0x0014EBBC File Offset: 0x0014CDBC
		private void AfterInitialized()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((LobbyPlayer p) => p.id));
			NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			NGame.Instance.TimeoutOverlay.Initialize(this._lobby.NetService, true);
			this.UpdateRichPresence();
			if (!string.IsNullOrEmpty(this._seedInput.Text))
			{
				this._lobby.SetSeed(this._seedInput.Text);
			}
			Logger.logLevelTypeMap[LogType.Network] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.Debug);
			Logger.logLevelTypeMap[LogType.Actions] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			Logger.logLevelTypeMap[LogType.GameSync] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			NGame.Instance.DebugSeedOverride = null;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x0014ECE8 File Offset: 0x0014CEE8
		private void UpdateControllerButton()
		{
			MultiplayerUiMode uiMode = this._uiMode;
			bool flag = uiMode - MultiplayerUiMode.Singleplayer <= 1;
			if (flag)
			{
				this._modifiersHotkeyIcon.Visible = NControllerManager.Instance.IsUsingController;
				this._modifiersHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.ModifiersHotkey);
				return;
			}
			this._modifiersHotkeyIcon.Visible = false;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x0014ED48 File Offset: 0x0014CF48
		private void TryFocusOnModifiersList()
		{
			Control control = base.GetViewport().GuiGetFocusOwner();
			if (control != null && this._modifiersList.IsAncestorOf(control))
			{
				return;
			}
			MultiplayerUiMode uiMode = this._uiMode;
			bool flag = uiMode - MultiplayerUiMode.Singleplayer <= 1;
			if (flag)
			{
				Control defaultFocusedControl = this._modifiersList.DefaultFocusedControl;
				if (defaultFocusedControl == null)
				{
					return;
				}
				defaultFocusedControl.TryGrabFocus();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E78 RID: 11896 RVA: 0x0014EDA0 File Offset: 0x0014CFA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(26)
			{
				new MethodInfo(NCustomRunScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.InitializeSingleplayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnSeedInputSubmitted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "newText", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.InitCharacterButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.DebugUnlockAllCharacters, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnRandomizePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.RandomizeLocalCharacter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunScreen.MethodName.GetModifiersString, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.OnModifiersListChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.MaxAscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.AscensionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.SeedChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.ModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.AfterInitialized, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunScreen.MethodName.TryFocusOnModifiersList, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E79 RID: 11897 RVA: 0x0014F3D8 File Offset: 0x0014D5D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunScreen.MethodName.Create && args.Count == 0)
			{
				NCustomRunScreen ncustomRunScreen = NCustomRunScreen.Create();
				ret = VariantUtils.CreateFrom<NCustomRunScreen>(ref ncustomRunScreen);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.InitializeSingleplayer && args.Count == 0)
			{
				this.InitializeSingleplayer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnSeedInputSubmitted && args.Count == 1)
			{
				this.OnSeedInputSubmitted(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.InitCharacterButtons && args.Count == 0)
			{
				this.InitCharacterButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.DebugUnlockAllCharacters && args.Count == 0)
			{
				this.DebugUnlockAllCharacters();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnRandomizePressed && args.Count == 1)
			{
				this.OnRandomizePressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.RandomizeLocalCharacter && args.Count == 0)
			{
				this.RandomizeLocalCharacter();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.GetModifiersString && args.Count == 0)
			{
				string modifiersString = this.GetModifiersString();
				ret = VariantUtils.CreateFrom<string>(ref modifiersString);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged && args.Count == 0)
			{
				this.OnAscensionPanelLevelChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.OnModifiersListChanged && args.Count == 0)
			{
				this.OnModifiersListChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.MaxAscensionChanged && args.Count == 0)
			{
				this.MaxAscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.AscensionChanged && args.Count == 0)
			{
				this.AscensionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.SeedChanged && args.Count == 0)
			{
				this.SeedChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.ModifiersChanged && args.Count == 0)
			{
				this.ModifiersChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.AfterInitialized && args.Count == 0)
			{
				this.AfterInitialized();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunScreen.MethodName.TryFocusOnModifiersList && args.Count == 0)
			{
				this.TryFocusOnModifiersList();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x0014F82C File Offset: 0x0014DA2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunScreen.MethodName.Create && args.Count == 0)
			{
				NCustomRunScreen ncustomRunScreen = NCustomRunScreen.Create();
				ret = VariantUtils.CreateFrom<NCustomRunScreen>(ref ncustomRunScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002E7B RID: 11899 RVA: 0x0014F86C File Offset: 0x0014DA6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCustomRunScreen.MethodName.Create || (ref method) == NCustomRunScreen.MethodName._Ready || (ref method) == NCustomRunScreen.MethodName.InitializeSingleplayer || (ref method) == NCustomRunScreen.MethodName.OnSeedInputSubmitted || (ref method) == NCustomRunScreen.MethodName.InitCharacterButtons || (ref method) == NCustomRunScreen.MethodName._Input || (ref method) == NCustomRunScreen.MethodName.DebugUnlockAllCharacters || (ref method) == NCustomRunScreen.MethodName.OnSubmenuOpened || (ref method) == NCustomRunScreen.MethodName.OnSubmenuClosed || (ref method) == NCustomRunScreen.MethodName.OnRandomizePressed || (ref method) == NCustomRunScreen.MethodName.RandomizeLocalCharacter || (ref method) == NCustomRunScreen.MethodName.OnEmbarkPressed || (ref method) == NCustomRunScreen.MethodName.OnUnreadyPressed || (ref method) == NCustomRunScreen.MethodName.UpdateRichPresence || (ref method) == NCustomRunScreen.MethodName._Process || (ref method) == NCustomRunScreen.MethodName.CleanUpLobby || (ref method) == NCustomRunScreen.MethodName.GetModifiersString || (ref method) == NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged || (ref method) == NCustomRunScreen.MethodName.OnModifiersListChanged || (ref method) == NCustomRunScreen.MethodName.MaxAscensionChanged || (ref method) == NCustomRunScreen.MethodName.AscensionChanged || (ref method) == NCustomRunScreen.MethodName.SeedChanged || (ref method) == NCustomRunScreen.MethodName.ModifiersChanged || (ref method) == NCustomRunScreen.MethodName.AfterInitialized || (ref method) == NCustomRunScreen.MethodName.UpdateControllerButton || (ref method) == NCustomRunScreen.MethodName.TryFocusOnModifiersList || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002E7C RID: 11900 RVA: 0x0014FA08 File Offset: 0x0014DC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCustomRunScreen.PropertyName._disclaimer)
			{
				this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._selectedButton)
			{
				this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._charButtonContainer)
			{
				this._charButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._randomizeButton)
			{
				this._randomizeButton = VariantUtils.ConvertTo<NCustomRunRandomizeButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._ascensionPanel)
			{
				this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._readyAndWaitingContainer)
			{
				this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._seedInput)
			{
				this._seedInput = VariantUtils.ConvertTo<LineEdit>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._modifiersList)
			{
				this._modifiersList = VariantUtils.ConvertTo<NCustomRunModifiersList>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._modifiersHotkeyIcon)
			{
				this._modifiersHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._uiMode)
			{
				this._uiMode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002E7D RID: 11901 RVA: 0x0014FB98 File Offset: 0x0014DD98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCustomRunScreen.PropertyName.ModifiersHotkey)
			{
				string modifiersHotkey = this.ModifiersHotkey;
				value = VariantUtils.CreateFrom<string>(ref modifiersHotkey);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._disclaimer)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._selectedButton)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._charButtonContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._charButtonContainer);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._randomizeButton)
			{
				value = VariantUtils.CreateFrom<NCustomRunRandomizeButton>(ref this._randomizeButton);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._ascensionPanel)
			{
				value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._readyAndWaitingContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._seedInput)
			{
				value = VariantUtils.CreateFrom<LineEdit>(ref this._seedInput);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._modifiersList)
			{
				value = VariantUtils.CreateFrom<NCustomRunModifiersList>(ref this._modifiersList);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._modifiersHotkeyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._modifiersHotkeyIcon);
				return true;
			}
			if ((ref name) == NCustomRunScreen.PropertyName._uiMode)
			{
				value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._uiMode);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E7E RID: 11902 RVA: 0x0014FDB4 File Offset: 0x0014DFB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NCustomRunScreen.PropertyName.ModifiersHotkey, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._disclaimer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._selectedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._charButtonContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._randomizeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._ascensionPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._readyAndWaitingContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._seedInput, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._modifiersList, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName._modifiersHotkeyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCustomRunScreen.PropertyName._uiMode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E7F RID: 11903 RVA: 0x0014FFD8 File Offset: 0x0014E1D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCustomRunScreen.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
			info.AddProperty(NCustomRunScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
			info.AddProperty(NCustomRunScreen.PropertyName._charButtonContainer, Variant.From<Control>(ref this._charButtonContainer));
			info.AddProperty(NCustomRunScreen.PropertyName._randomizeButton, Variant.From<NCustomRunRandomizeButton>(ref this._randomizeButton));
			info.AddProperty(NCustomRunScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NCustomRunScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NCustomRunScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NCustomRunScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
			info.AddProperty(NCustomRunScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
			info.AddProperty(NCustomRunScreen.PropertyName._seedInput, Variant.From<LineEdit>(ref this._seedInput));
			info.AddProperty(NCustomRunScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NCustomRunScreen.PropertyName._modifiersList, Variant.From<NCustomRunModifiersList>(ref this._modifiersList));
			info.AddProperty(NCustomRunScreen.PropertyName._modifiersHotkeyIcon, Variant.From<TextureRect>(ref this._modifiersHotkeyIcon));
			info.AddProperty(NCustomRunScreen.PropertyName._uiMode, Variant.From<MultiplayerUiMode>(ref this._uiMode));
		}

		/// <inheritdoc />
		// Token: 0x06002E80 RID: 11904 RVA: 0x00150120 File Offset: 0x0014E320
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._disclaimer, ref variant))
			{
				this._disclaimer = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._selectedButton, ref variant2))
			{
				this._selectedButton = variant2.As<NCharacterSelectButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._charButtonContainer, ref variant3))
			{
				this._charButtonContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._randomizeButton, ref variant4))
			{
				this._randomizeButton = variant4.As<NCustomRunRandomizeButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._confirmButton, ref variant5))
			{
				this._confirmButton = variant5.As<NConfirmButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._backButton, ref variant6))
			{
				this._backButton = variant6.As<NBackButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._unreadyButton, ref variant7))
			{
				this._unreadyButton = variant7.As<NBackButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._ascensionPanel, ref variant8))
			{
				this._ascensionPanel = variant8.As<NAscensionPanel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._readyAndWaitingContainer, ref variant9))
			{
				this._readyAndWaitingContainer = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._seedInput, ref variant10))
			{
				this._seedInput = variant10.As<LineEdit>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._remotePlayerContainer, ref variant11))
			{
				this._remotePlayerContainer = variant11.As<NRemoteLobbyPlayerContainer>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._modifiersList, ref variant12))
			{
				this._modifiersList = variant12.As<NCustomRunModifiersList>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._modifiersHotkeyIcon, ref variant13))
			{
				this._modifiersHotkeyIcon = variant13.As<TextureRect>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCustomRunScreen.PropertyName._uiMode, ref variant14))
			{
				this._uiMode = variant14.As<MultiplayerUiMode>();
			}
		}

		// Token: 0x040011B5 RID: 4533
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/custom_run/custom_run_screen");

		// Token: 0x040011B6 RID: 4534
		private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";

		// Token: 0x040011B7 RID: 4535
		private MegaLabel _disclaimer;

		// Token: 0x040011B8 RID: 4536
		[Nullable(2)]
		private NCharacterSelectButton _selectedButton;

		// Token: 0x040011B9 RID: 4537
		private Control _charButtonContainer;

		// Token: 0x040011BA RID: 4538
		private NCustomRunRandomizeButton _randomizeButton;

		// Token: 0x040011BB RID: 4539
		private NConfirmButton _confirmButton;

		// Token: 0x040011BC RID: 4540
		private NBackButton _backButton;

		// Token: 0x040011BD RID: 4541
		private NBackButton _unreadyButton;

		// Token: 0x040011BE RID: 4542
		private NAscensionPanel _ascensionPanel;

		// Token: 0x040011BF RID: 4543
		private Control _readyAndWaitingContainer;

		// Token: 0x040011C0 RID: 4544
		private LineEdit _seedInput;

		// Token: 0x040011C1 RID: 4545
		private NRemoteLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x040011C2 RID: 4546
		private NCustomRunModifiersList _modifiersList;

		// Token: 0x040011C3 RID: 4547
		private TextureRect _modifiersHotkeyIcon;

		// Token: 0x040011C4 RID: 4548
		private StartRunLobby _lobby;

		// Token: 0x040011C5 RID: 4549
		private MultiplayerUiMode _uiMode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C3 RID: 5571
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040053A6 RID: 21414
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040053A7 RID: 21415
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'InitializeSingleplayer' method.
			/// </summary>
			// Token: 0x040053A8 RID: 21416
			public static readonly StringName InitializeSingleplayer = "InitializeSingleplayer";

			/// <summary>
			/// Cached name for the 'OnSeedInputSubmitted' method.
			/// </summary>
			// Token: 0x040053A9 RID: 21417
			public static readonly StringName OnSeedInputSubmitted = "OnSeedInputSubmitted";

			/// <summary>
			/// Cached name for the 'InitCharacterButtons' method.
			/// </summary>
			// Token: 0x040053AA RID: 21418
			public static readonly StringName InitCharacterButtons = "InitCharacterButtons";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040053AB RID: 21419
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'DebugUnlockAllCharacters' method.
			/// </summary>
			// Token: 0x040053AC RID: 21420
			public static readonly StringName DebugUnlockAllCharacters = "DebugUnlockAllCharacters";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x040053AD RID: 21421
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x040053AE RID: 21422
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnRandomizePressed' method.
			/// </summary>
			// Token: 0x040053AF RID: 21423
			public static readonly StringName OnRandomizePressed = "OnRandomizePressed";

			/// <summary>
			/// Cached name for the 'RandomizeLocalCharacter' method.
			/// </summary>
			// Token: 0x040053B0 RID: 21424
			public static readonly StringName RandomizeLocalCharacter = "RandomizeLocalCharacter";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x040053B1 RID: 21425
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x040053B2 RID: 21426
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x040053B3 RID: 21427
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040053B4 RID: 21428
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x040053B5 RID: 21429
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'GetModifiersString' method.
			/// </summary>
			// Token: 0x040053B6 RID: 21430
			public static readonly StringName GetModifiersString = "GetModifiersString";

			/// <summary>
			/// Cached name for the 'OnAscensionPanelLevelChanged' method.
			/// </summary>
			// Token: 0x040053B7 RID: 21431
			public static readonly StringName OnAscensionPanelLevelChanged = "OnAscensionPanelLevelChanged";

			/// <summary>
			/// Cached name for the 'OnModifiersListChanged' method.
			/// </summary>
			// Token: 0x040053B8 RID: 21432
			public static readonly StringName OnModifiersListChanged = "OnModifiersListChanged";

			/// <summary>
			/// Cached name for the 'MaxAscensionChanged' method.
			/// </summary>
			// Token: 0x040053B9 RID: 21433
			public static readonly StringName MaxAscensionChanged = "MaxAscensionChanged";

			/// <summary>
			/// Cached name for the 'AscensionChanged' method.
			/// </summary>
			// Token: 0x040053BA RID: 21434
			public static readonly StringName AscensionChanged = "AscensionChanged";

			/// <summary>
			/// Cached name for the 'SeedChanged' method.
			/// </summary>
			// Token: 0x040053BB RID: 21435
			public static readonly StringName SeedChanged = "SeedChanged";

			/// <summary>
			/// Cached name for the 'ModifiersChanged' method.
			/// </summary>
			// Token: 0x040053BC RID: 21436
			public static readonly StringName ModifiersChanged = "ModifiersChanged";

			/// <summary>
			/// Cached name for the 'AfterInitialized' method.
			/// </summary>
			// Token: 0x040053BD RID: 21437
			public static readonly StringName AfterInitialized = "AfterInitialized";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x040053BE RID: 21438
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";

			/// <summary>
			/// Cached name for the 'TryFocusOnModifiersList' method.
			/// </summary>
			// Token: 0x040053BF RID: 21439
			public static readonly StringName TryFocusOnModifiersList = "TryFocusOnModifiersList";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C4 RID: 5572
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ModifiersHotkey' property.
			/// </summary>
			// Token: 0x040053C0 RID: 21440
			public static readonly StringName ModifiersHotkey = "ModifiersHotkey";

			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x040053C1 RID: 21441
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_disclaimer' field.
			/// </summary>
			// Token: 0x040053C2 RID: 21442
			public static readonly StringName _disclaimer = "_disclaimer";

			/// <summary>
			/// Cached name for the '_selectedButton' field.
			/// </summary>
			// Token: 0x040053C3 RID: 21443
			public static readonly StringName _selectedButton = "_selectedButton";

			/// <summary>
			/// Cached name for the '_charButtonContainer' field.
			/// </summary>
			// Token: 0x040053C4 RID: 21444
			public static readonly StringName _charButtonContainer = "_charButtonContainer";

			/// <summary>
			/// Cached name for the '_randomizeButton' field.
			/// </summary>
			// Token: 0x040053C5 RID: 21445
			public static readonly StringName _randomizeButton = "_randomizeButton";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040053C6 RID: 21446
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x040053C7 RID: 21447
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x040053C8 RID: 21448
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_ascensionPanel' field.
			/// </summary>
			// Token: 0x040053C9 RID: 21449
			public static readonly StringName _ascensionPanel = "_ascensionPanel";

			/// <summary>
			/// Cached name for the '_readyAndWaitingContainer' field.
			/// </summary>
			// Token: 0x040053CA RID: 21450
			public static readonly StringName _readyAndWaitingContainer = "_readyAndWaitingContainer";

			/// <summary>
			/// Cached name for the '_seedInput' field.
			/// </summary>
			// Token: 0x040053CB RID: 21451
			public static readonly StringName _seedInput = "_seedInput";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x040053CC RID: 21452
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_modifiersList' field.
			/// </summary>
			// Token: 0x040053CD RID: 21453
			public static readonly StringName _modifiersList = "_modifiersList";

			/// <summary>
			/// Cached name for the '_modifiersHotkeyIcon' field.
			/// </summary>
			// Token: 0x040053CE RID: 21454
			public static readonly StringName _modifiersHotkeyIcon = "_modifiersHotkeyIcon";

			/// <summary>
			/// Cached name for the '_uiMode' field.
			/// </summary>
			// Token: 0x040053CF RID: 21455
			public static readonly StringName _uiMode = "_uiMode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C5 RID: 5573
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
