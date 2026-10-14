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
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
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
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun
{
	/// <summary>
	/// Intermediate screen when loading a custom run in multiplayer. Allows players to join back into the session
	/// before it is resumed.
	/// </summary>
	// Token: 0x02000336 RID: 822
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunLoadScreen.cs")]
	public class NCustomRunLoadScreen : NSubmenu, ILoadRunLobbyListener
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002E1E RID: 11806 RVA: 0x0014BFC7 File Offset: 0x0014A1C7
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NCustomRunLoadScreen._scenePath,
					"res://scenes/screens/custom_run/modifier_tickbox.tscn"
				});
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x0014BFE4 File Offset: 0x0014A1E4
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0014BFE7 File Offset: 0x0014A1E7
		[NullableContext(2)]
		public static NCustomRunLoadScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCustomRunLoadScreen._scenePath).Instantiate<NCustomRunLoadScreen>(0L);
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x0014C008 File Offset: 0x0014A208
		public override void _Ready()
		{
			this.ConnectSignals();
			this._ascensionPanel = base.GetNode<NAscensionPanel>("%AscensionPanel");
			this._remotePlayerContainer = base.GetNode<NRemoteLoadLobbyPlayerContainer>("LeftContainer/RemotePlayerLoadContainer");
			this._readyAndWaitingContainer = base.GetNode<Control>("%ReadyAndWaitingPanel");
			this._modifiersList = base.GetNode<NCustomRunModifiersList>("%ModifiersList");
			this._seedInput = base.GetNode<LineEdit>("%SeedInput");
			this._confirmButton = base.GetNode<NConfirmButton>("ConfirmButton");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("UnreadyButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._unreadyButton.Disable();
			base.GetNode<MegaLabel>("%CustomModeTitle").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.CUSTOM_MODE_TITLE").GetFormattedText());
			base.GetNode<MegaLabel>("%ModifiersTitle").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIERS_TITLE").GetFormattedText());
			base.GetNode<MegaLabel>("%SeedLabel").SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_LABEL").GetFormattedText());
			this._seedInput.PlaceholderText = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_RANDOM_PLACEHOLDER").GetFormattedText();
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x0014C1B8 File Offset: 0x0014A3B8
		public void InitializeAsHost(INetGameService gameService, SerializableRun run)
		{
			if (gameService.Type != NetGameType.Host)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized custom run screen with NetService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when hosting!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._lobby = new LoadRunLobby(gameService, this, run);
			try
			{
				this._lobby.AddLocalHostPlayer();
				this.AfterInitialized();
			}
			catch
			{
				this.CleanUpLobby(true, NetError.InternalError);
				throw;
			}
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x0014C248 File Offset: 0x0014A448
		public void InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)
		{
			if (gameService.Type != NetGameType.Client)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with NetService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when joining!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._lobby = new LoadRunLobby(gameService, this, message);
			this.AfterInitialized();
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x0014C2B0 File Offset: 0x0014A4B0
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._confirmButton.Enable();
			this._remotePlayerContainer.Initialize(this._lobby, true);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Load);
			this._ascensionPanel.SetAscensionLevel(this._lobby.Run.Ascension);
			this._modifiersList.Initialize(MultiplayerUiMode.Load);
			NCustomRunModifiersList modifiersList = this._modifiersList;
			IEnumerable<SerializableModifier> modifiers = this._lobby.Run.Modifiers;
			Func<SerializableModifier, ModifierModel> func;
			if ((func = NCustomRunLoadScreen.<>O.<0>__FromSerializable) == null)
			{
				func = (NCustomRunLoadScreen.<>O.<0>__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable));
			}
			modifiersList.SyncModifierList(modifiers.Select<SerializableModifier, ModifierModel>(func).ToList<ModifierModel>());
			this._readyAndWaitingContainer.Visible = false;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0014C360 File Offset: 0x0014A560
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
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x0014C3BC File Offset: 0x0014A5BC
		private void OnEmbarkPressed(NButton _)
		{
			this._confirmButton.Disable();
			this._backButton.Disable();
			this._lobby.SetReady(true);
			if (!this._lobby.IsAboutToBeginGame())
			{
				this._unreadyButton.Enable();
				this._readyAndWaitingContainer.Visible = true;
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x0014C40F File Offset: 0x0014A60F
		private void OnUnreadyPressed(NButton _)
		{
			this._confirmButton.Enable();
			this._backButton.Enable();
			this._unreadyButton.Disable();
			this._lobby.SetReady(false);
			this._readyAndWaitingContainer.Visible = false;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x0014C44C File Offset: 0x0014A64C
		private void UpdateRichPresence()
		{
			if (this._lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.ConnectedPlayerIds.Count));
			}
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x0014C49F File Offset: 0x0014A69F
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

		// Token: 0x06002E2A RID: 11818 RVA: 0x0014C4CC File Offset: 0x0014A6CC
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			this._lobby.CleanUp(disconnectSession, error);
			this._lobby = null;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x0014C4E4 File Offset: 0x0014A6E4
		public async Task<bool> ShouldAllowRunToBegin()
		{
			bool flag;
			if (this._lobby.ConnectedPlayerIds.Count >= this._lobby.Run.Players.Count)
			{
				flag = true;
			}
			else
			{
				LocString locString = new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.body");
				locString.Add("MissingCount", this._lobby.Run.Players.Count - this._lobby.ConnectedPlayerIds.Count);
				NGenericPopup ngenericPopup = NGenericPopup.Create();
				NModalContainer.Instance.Add(ngenericPopup, true);
				bool flag2 = await ngenericPopup.WaitForConfirmation(locString, new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.header"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.cancel"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.confirm"));
				bool flag3 = flag2;
				flag = flag3;
			}
			return flag;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x0014C528 File Offset: 0x0014A728
		private async Task StartRun()
		{
			int num = 0;
			try
			{
				Log.Info("Loading a custom multiplayer run. Players: " + string.Join<ulong>(",", this._lobby.ConnectedPlayerIds) + ".", 2);
				SerializablePlayer serializablePlayer = this._lobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == this._lobby.NetService.NetId);
				SfxCmd.Play(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterSelectTransitionPath, null);
				RunState runState = RunState.FromSerializable(this._lobby.Run);
				await RunManager.Instance.SetUpSavedMultiplayer(runState, this._lobby);
				await NGame.Instance.LoadRun(runState, this._lobby.Run.PreFinishedRoom);
				runState = null;
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
				defaultInterpolatedStringHandler.AppendLiteral("Exception loading custom multiplayer run : ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.CleanUpLobby(true, NetError.InternalError);
				await NGame.Instance.ReturnToMainMenuWithInternalError(ex);
			}
			else
			{
				object obj = null;
				this.CleanUpLobby(false, NetError.Quit);
				await NGame.Instance.Transition.FadeIn(0.8f, "res://materials/transitions/fade_transition_mat.tres", null);
			}
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x0014C56C File Offset: 0x0014A76C
		public void PlayerConnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player connected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerConnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x0014C5B8 File Offset: 0x0014A7B8
		public void PlayerReadyChanged(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ready changed: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerChanged(playerId);
			if (playerId == this._lobby.NetService.NetId && !this._lobby.IsPlayerReady(playerId))
			{
				this._confirmButton.Enable();
			}
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x0014C628 File Offset: 0x0014A828
		public void RemotePlayerDisconnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player disconnected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerDisconnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x0014C672 File Offset: 0x0014A872
		public void BeginRun()
		{
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			this._confirmButton.Disable();
			this._unreadyButton.Disable();
			TaskHelper.RunSafely(this.StartRun());
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x0014C6A8 File Offset: 0x0014A8A8
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

		// Token: 0x06002E32 RID: 11826 RVA: 0x0014C710 File Offset: 0x0014A910
		private void AfterInitialized()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.ConnectedPlayerIds);
			NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			this.UpdateRichPresence();
			Logger.logLevelTypeMap[LogType.Network] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.Debug);
			Logger.logLevelTypeMap[LogType.Actions] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			Logger.logLevelTypeMap[LogType.GameSync] = ((this._lobby.NetService.Type == NetGameType.Singleplayer) ? LogLevel.Info : LogLevel.VeryDebug);
			NGame.Instance.DebugSeedOverride = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E33 RID: 11827 RVA: 0x0014C7D4 File Offset: 0x0014A9D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NCustomRunLoadScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.PlayerConnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.PlayerReadyChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.BeginRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunLoadScreen.MethodName.AfterInitialized, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E34 RID: 11828 RVA: 0x0014CBC4 File Offset: 0x0014ADC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunLoadScreen.MethodName.Create && args.Count == 0)
			{
				NCustomRunLoadScreen ncustomRunLoadScreen = NCustomRunLoadScreen.Create();
				ret = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref ncustomRunLoadScreen);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.PlayerConnected && args.Count == 1)
			{
				this.PlayerConnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.PlayerReadyChanged && args.Count == 1)
			{
				this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected && args.Count == 1)
			{
				this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.BeginRun && args.Count == 0)
			{
				this.BeginRun();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunLoadScreen.MethodName.AfterInitialized && args.Count == 0)
			{
				this.AfterInitialized();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0014CE58 File Offset: 0x0014B058
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunLoadScreen.MethodName.Create && args.Count == 0)
			{
				NCustomRunLoadScreen ncustomRunLoadScreen = NCustomRunLoadScreen.Create();
				ret = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref ncustomRunLoadScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002E36 RID: 11830 RVA: 0x0014CE98 File Offset: 0x0014B098
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCustomRunLoadScreen.MethodName.Create || (ref method) == NCustomRunLoadScreen.MethodName._Ready || (ref method) == NCustomRunLoadScreen.MethodName.OnSubmenuOpened || (ref method) == NCustomRunLoadScreen.MethodName.OnSubmenuClosed || (ref method) == NCustomRunLoadScreen.MethodName.OnEmbarkPressed || (ref method) == NCustomRunLoadScreen.MethodName.OnUnreadyPressed || (ref method) == NCustomRunLoadScreen.MethodName.UpdateRichPresence || (ref method) == NCustomRunLoadScreen.MethodName._Process || (ref method) == NCustomRunLoadScreen.MethodName.CleanUpLobby || (ref method) == NCustomRunLoadScreen.MethodName.PlayerConnected || (ref method) == NCustomRunLoadScreen.MethodName.PlayerReadyChanged || (ref method) == NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected || (ref method) == NCustomRunLoadScreen.MethodName.BeginRun || (ref method) == NCustomRunLoadScreen.MethodName.AfterInitialized || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002E37 RID: 11831 RVA: 0x0014CF80 File Offset: 0x0014B180
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCustomRunLoadScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._ascensionPanel)
			{
				this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer)
			{
				this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._seedInput)
			{
				this._seedInput = VariantUtils.ConvertTo<LineEdit>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._modifiersList)
			{
				this._modifiersList = VariantUtils.ConvertTo<NCustomRunModifiersList>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002E38 RID: 11832 RVA: 0x0014D070 File Offset: 0x0014B270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCustomRunLoadScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._ascensionPanel)
			{
				value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._seedInput)
			{
				value = VariantUtils.CreateFrom<LineEdit>(ref this._seedInput);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NCustomRunLoadScreen.PropertyName._modifiersList)
			{
				value = VariantUtils.CreateFrom<NCustomRunModifiersList>(ref this._modifiersList);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E39 RID: 11833 RVA: 0x0014D1A8 File Offset: 0x0014B3A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._ascensionPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._seedInput, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName._modifiersList, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunLoadScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E3A RID: 11834 RVA: 0x0014D2E8 File Offset: 0x0014B4E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCustomRunLoadScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._seedInput, Variant.From<LineEdit>(ref this._seedInput));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NCustomRunLoadScreen.PropertyName._modifiersList, Variant.From<NCustomRunModifiersList>(ref this._modifiersList));
		}

		/// <inheritdoc />
		// Token: 0x06002E3B RID: 11835 RVA: 0x0014D3AC File Offset: 0x0014B5AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NConfirmButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._backButton, ref variant2))
			{
				this._backButton = variant2.As<NBackButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._unreadyButton, ref variant3))
			{
				this._unreadyButton = variant3.As<NBackButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._ascensionPanel, ref variant4))
			{
				this._ascensionPanel = variant4.As<NAscensionPanel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, ref variant5))
			{
				this._readyAndWaitingContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._seedInput, ref variant6))
			{
				this._seedInput = variant6.As<LineEdit>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._remotePlayerContainer, ref variant7))
			{
				this._remotePlayerContainer = variant7.As<NRemoteLoadLobbyPlayerContainer>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._modifiersList, ref variant8))
			{
				this._modifiersList = variant8.As<NCustomRunModifiersList>();
			}
		}

		// Token: 0x040011A7 RID: 4519
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/custom_run/custom_run_load_screen");

		// Token: 0x040011A8 RID: 4520
		private NConfirmButton _confirmButton;

		// Token: 0x040011A9 RID: 4521
		private NBackButton _backButton;

		// Token: 0x040011AA RID: 4522
		private NBackButton _unreadyButton;

		// Token: 0x040011AB RID: 4523
		private NAscensionPanel _ascensionPanel;

		// Token: 0x040011AC RID: 4524
		private Control _readyAndWaitingContainer;

		// Token: 0x040011AD RID: 4525
		private LineEdit _seedInput;

		// Token: 0x040011AE RID: 4526
		private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x040011AF RID: 4527
		private NCustomRunModifiersList _modifiersList;

		// Token: 0x040011B0 RID: 4528
		private LoadRunLobby _lobby;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015BA RID: 5562
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400537B RID: 21371
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400537C RID: 21372
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x0400537D RID: 21373
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x0400537E RID: 21374
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x0400537F RID: 21375
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x04005380 RID: 21376
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x04005381 RID: 21377
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005382 RID: 21378
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x04005383 RID: 21379
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'PlayerConnected' method.
			/// </summary>
			// Token: 0x04005384 RID: 21380
			public static readonly StringName PlayerConnected = "PlayerConnected";

			/// <summary>
			/// Cached name for the 'PlayerReadyChanged' method.
			/// </summary>
			// Token: 0x04005385 RID: 21381
			public static readonly StringName PlayerReadyChanged = "PlayerReadyChanged";

			/// <summary>
			/// Cached name for the 'RemotePlayerDisconnected' method.
			/// </summary>
			// Token: 0x04005386 RID: 21382
			public static readonly StringName RemotePlayerDisconnected = "RemotePlayerDisconnected";

			/// <summary>
			/// Cached name for the 'BeginRun' method.
			/// </summary>
			// Token: 0x04005387 RID: 21383
			public static readonly StringName BeginRun = "BeginRun";

			/// <summary>
			/// Cached name for the 'AfterInitialized' method.
			/// </summary>
			// Token: 0x04005388 RID: 21384
			public static readonly StringName AfterInitialized = "AfterInitialized";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015BB RID: 5563
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04005389 RID: 21385
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x0400538A RID: 21386
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x0400538B RID: 21387
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x0400538C RID: 21388
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_ascensionPanel' field.
			/// </summary>
			// Token: 0x0400538D RID: 21389
			public static readonly StringName _ascensionPanel = "_ascensionPanel";

			/// <summary>
			/// Cached name for the '_readyAndWaitingContainer' field.
			/// </summary>
			// Token: 0x0400538E RID: 21390
			public static readonly StringName _readyAndWaitingContainer = "_readyAndWaitingContainer";

			/// <summary>
			/// Cached name for the '_seedInput' field.
			/// </summary>
			// Token: 0x0400538F RID: 21391
			public static readonly StringName _seedInput = "_seedInput";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x04005390 RID: 21392
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_modifiersList' field.
			/// </summary>
			// Token: 0x04005391 RID: 21393
			public static readonly StringName _modifiersList = "_modifiersList";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015BC RID: 5564
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}

		// Token: 0x020015BD RID: 5565
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04005392 RID: 21394
			[Nullable(0)]
			public static Func<SerializableModifier, ModifierModel> <0>__FromSerializable;
		}
	}
}
