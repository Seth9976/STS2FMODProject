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
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	/// <summary>
	/// Screen that displays when loading a multiplayer daily run, allowing other players to join the game before it starts.
	/// </summary>
	// Token: 0x0200032F RID: 815
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLoadScreen.cs")]
	public class NDailyRunLoadScreen : NSubmenu, ILoadRunLobbyListener
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x00146317 File Offset: 0x00144517
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NDailyRunLoadScreen._scenePath };
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x00146327 File Offset: 0x00144527
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x0014632A File Offset: 0x0014452A
		[NullableContext(2)]
		public static NDailyRunLoadScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDailyRunLoadScreen._scenePath).Instantiate<NDailyRunLoadScreen>(0L);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x0014634C File Offset: 0x0014454C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._embarkButton = base.GetNode<NConfirmButton>("%ConfirmButton");
			this._backButton = base.GetNode<NBackButton>("%BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("%UnreadyButton");
			this._dateLabel = base.GetNode<MegaRichTextLabel>("%Date");
			this._leaderboard = base.GetNode<NDailyRunLeaderboard>("%Leaderboards");
			this._modifiersTitleLabel = base.GetNode<MegaLabel>("%ModifiersLabel");
			this._modifiersContainer = base.GetNode<Control>("%ModifiersContainer");
			this._characterContainer = base.GetNode<NDailyRunCharacterContainer>("ChallengeContainer/CenterContainer/HBoxContainer/CharacterContainer");
			this._remotePlayerContainer = base.GetNode<NRemoteLoadLobbyPlayerContainer>("%RemotePlayerLoadContainer");
			this._readyAndWaitingContainer = base.GetNode<Control>("%ReadyAndWaitingPanel");
			foreach (NDailyRunScreenModifier ndailyRunScreenModifier in this._modifiersContainer.GetChildren(false).OfType<NDailyRunScreenModifier>())
			{
				this._modifierContainers.Add(ndailyRunScreenModifier);
			}
			this._readyAndWaitingContainer.Visible = false;
			this._embarkButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._unreadyButton.Disable();
			this._leaderboard.Cleanup();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x001464F4 File Offset: 0x001446F4
		public void InitializeAsHost(INetGameService gameService, SerializableRun run)
		{
			if (gameService.Type != NetGameType.Host)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized daily run load screen with net service of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when hosting!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._lobby = new LoadRunLobby(gameService, this, run);
			try
			{
				this._lobby.AddLocalHostPlayer();
				this.AfterMultiplayerStarted();
			}
			catch
			{
				this.CleanUpLobby(true, NetError.InternalError);
				throw;
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x00146584 File Offset: 0x00144784
		public void InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)
		{
			if (gameService.Type != NetGameType.Client)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized daily run load screen with net service of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when joining!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._lobby = new LoadRunLobby(gameService, this, message);
			this.AfterMultiplayerStarted();
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x001465EC File Offset: 0x001447EC
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._leaderboard.Initialize(this._lobby.Run.DailyTime.Value, this._lobby.Run.Players.Select<SerializablePlayer, ulong>((SerializablePlayer p) => p.NetId), true);
			this._embarkButton.Enable();
			this._remotePlayerContainer.Initialize(this._lobby, false);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x00146674 File Offset: 0x00144874
		public override void OnSubmenuClosed()
		{
			this._embarkButton.Disable();
			this._remotePlayerContainer.Cleanup();
			this._leaderboard.Cleanup();
			LoadRunLobby lobby = this._lobby;
			if (lobby != null && lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			}
			this.CleanUpLobby(true, NetError.Quit);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x001466DC File Offset: 0x001448DC
		private void InitializeDisplay()
		{
			if (this._lobby == null)
			{
				throw new InvalidOperationException("Tried to initialize daily run display before lobby was initialized!");
			}
			NDailyRunLoadScreen._ascensionLoc.Add("ascension", this._lobby.Run.Ascension);
			DateTimeOffset value = this._lobby.Run.DailyTime.Value;
			SerializablePlayer serializablePlayer = this._lobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == this._lobby.NetService.NetId);
			CharacterModel byId = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
			this._characterContainer.Fill(byId, serializablePlayer.NetId, this._lobby.Run.Ascension, this._lobby.NetService);
			this._dateLabel.SetTextAutoSize(value.ToString(NDailyRunLoadScreen.dateFormat));
			this._embarkButton.Enable();
			for (int i = 0; i < this._lobby.Run.Modifiers.Count; i++)
			{
				ModifierModel modifierModel = ModifierModel.FromSerializable(this._lobby.Run.Modifiers[i]);
				this._modifierContainers[i].Fill(modifierModel);
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0014680C File Offset: 0x00144A0C
		private void OnEmbarkPressed(NButton _)
		{
			this._embarkButton.Disable();
			this._backButton.Disable();
			this._lobby.SetReady(true);
			if (!this._lobby.IsAboutToBeginGame())
			{
				this._readyAndWaitingContainer.Visible = true;
				this._unreadyButton.Enable();
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x0014685F File Offset: 0x00144A5F
		private void OnUnreadyPressed(NButton _)
		{
			this._embarkButton.Enable();
			this._unreadyButton.Disable();
			this._backButton.Enable();
			this._lobby.SetReady(true);
			this._readyAndWaitingContainer.Visible = false;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x0014689C File Offset: 0x00144A9C
		private void UpdateRichPresence()
		{
			LoadRunLobby lobby = this._lobby;
			if (lobby != null && lobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.ConnectedPlayerIds.Count));
			}
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001468F6 File Offset: 0x00144AF6
		public override void _Process(double delta)
		{
			LoadRunLobby lobby = this._lobby;
			if (lobby != null && lobby.NetService.IsConnected)
			{
				this._lobby.NetService.Update();
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x00146921 File Offset: 0x00144B21
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			this._lobby.CleanUp(disconnectSession, error);
			this._lobby = null;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x00146938 File Offset: 0x00144B38
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

		// Token: 0x06002D90 RID: 11664 RVA: 0x0014697C File Offset: 0x00144B7C
		private async Task StartRun()
		{
			int num = 0;
			try
			{
				Log.Info("Loading a multiplayer run. Players: " + string.Join<ulong>(",", this._lobby.ConnectedPlayerIds) + ".", 2);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception loading daily multiplayer run : ");
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

		// Token: 0x06002D91 RID: 11665 RVA: 0x001469C0 File Offset: 0x00144BC0
		public void PlayerConnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player connected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerConnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x00146A0C File Offset: 0x00144C0C
		public void PlayerReadyChanged(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ready changed: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerChanged(playerId);
			if (playerId == this._lobby.NetService.NetId && !this._lobby.IsPlayerReady(playerId))
			{
				this._embarkButton.Enable();
			}
			if (playerId == this._lobby.NetService.NetId && this._lobby.NetService.Type.IsMultiplayer())
			{
				this._characterContainer.SetIsReady(this._lobby.IsPlayerReady(playerId));
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x00146AC0 File Offset: 0x00144CC0
		public void RemotePlayerDisconnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player disconnected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerDisconnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x00146B0A File Offset: 0x00144D0A
		public void BeginRun()
		{
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.StopMusic();
			}
			this._embarkButton.Disable();
			this._unreadyButton.Disable();
			TaskHelper.RunSafely(this.StartRun());
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x00146B40 File Offset: 0x00144D40
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

		// Token: 0x06002D96 RID: 11670 RVA: 0x00146BA8 File Offset: 0x00144DA8
		private void AfterMultiplayerStarted()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.ConnectedPlayerIds);
			NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
			this.InitializeDisplay();
			this.UpdateRichPresence();
			Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
			Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
			Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D97 RID: 11671 RVA: 0x00146C24 File Offset: 0x00144E24
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NDailyRunLoadScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.InitializeDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.PlayerConnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.PlayerReadyChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.BeginRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D98 RID: 11672 RVA: 0x00147044 File Offset: 0x00145244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLoadScreen.MethodName.Create && args.Count == 0)
			{
				NDailyRunLoadScreen ndailyRunLoadScreen = NDailyRunLoadScreen.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref ndailyRunLoadScreen);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.InitializeDisplay && args.Count == 0)
			{
				this.InitializeDisplay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.PlayerConnected && args.Count == 1)
			{
				this.PlayerConnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.PlayerReadyChanged && args.Count == 1)
			{
				this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected && args.Count == 1)
			{
				this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.BeginRun && args.Count == 0)
			{
				this.BeginRun();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted && args.Count == 0)
			{
				this.AfterMultiplayerStarted();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x001472FC File Offset: 0x001454FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLoadScreen.MethodName.Create && args.Count == 0)
			{
				NDailyRunLoadScreen ndailyRunLoadScreen = NDailyRunLoadScreen.Create();
				ret = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref ndailyRunLoadScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002D9A RID: 11674 RVA: 0x0014733C File Offset: 0x0014553C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunLoadScreen.MethodName.Create || (ref method) == NDailyRunLoadScreen.MethodName._Ready || (ref method) == NDailyRunLoadScreen.MethodName.OnSubmenuOpened || (ref method) == NDailyRunLoadScreen.MethodName.OnSubmenuClosed || (ref method) == NDailyRunLoadScreen.MethodName.InitializeDisplay || (ref method) == NDailyRunLoadScreen.MethodName.OnEmbarkPressed || (ref method) == NDailyRunLoadScreen.MethodName.OnUnreadyPressed || (ref method) == NDailyRunLoadScreen.MethodName.UpdateRichPresence || (ref method) == NDailyRunLoadScreen.MethodName._Process || (ref method) == NDailyRunLoadScreen.MethodName.CleanUpLobby || (ref method) == NDailyRunLoadScreen.MethodName.PlayerConnected || (ref method) == NDailyRunLoadScreen.MethodName.PlayerReadyChanged || (ref method) == NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected || (ref method) == NDailyRunLoadScreen.MethodName.BeginRun || (ref method) == NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002D9B RID: 11675 RVA: 0x00147434 File Offset: 0x00145634
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunLoadScreen.PropertyName._dateLabel)
			{
				this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._embarkButton)
			{
				this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._characterContainer)
			{
				this._characterContainer = VariantUtils.ConvertTo<NDailyRunCharacterContainer>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._leaderboard)
			{
				this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._modifiersTitleLabel)
			{
				this._modifiersTitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._modifiersContainer)
			{
				this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer)
			{
				this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002D9C RID: 11676 RVA: 0x00147558 File Offset: 0x00145758
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunLoadScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._dateLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._embarkButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._characterContainer)
			{
				value = VariantUtils.CreateFrom<NDailyRunCharacterContainer>(ref this._characterContainer);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._leaderboard)
			{
				value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._modifiersTitleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._modifiersTitleLabel);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._modifiersContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer)
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
		// Token: 0x06002D9D RID: 11677 RVA: 0x001476D0 File Offset: 0x001458D0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._dateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._embarkButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._characterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._leaderboard, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._modifiersContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D9E RID: 11678 RVA: 0x00147850 File Offset: 0x00145A50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunLoadScreen.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._characterContainer, Variant.From<NDailyRunCharacterContainer>(ref this._characterContainer));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, Variant.From<MegaLabel>(ref this._modifiersTitleLabel));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
		}

		/// <inheritdoc />
		// Token: 0x06002D9F RID: 11679 RVA: 0x00147940 File Offset: 0x00145B40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._dateLabel, ref variant))
			{
				this._dateLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._embarkButton, ref variant2))
			{
				this._embarkButton = variant2.As<NConfirmButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._backButton, ref variant3))
			{
				this._backButton = variant3.As<NBackButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._unreadyButton, ref variant4))
			{
				this._unreadyButton = variant4.As<NBackButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._characterContainer, ref variant5))
			{
				this._characterContainer = variant5.As<NDailyRunCharacterContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._leaderboard, ref variant6))
			{
				this._leaderboard = variant6.As<NDailyRunLeaderboard>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, ref variant7))
			{
				this._modifiersTitleLabel = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._modifiersContainer, ref variant8))
			{
				this._modifiersContainer = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._remotePlayerContainer, ref variant9))
			{
				this._remotePlayerContainer = variant9.As<NRemoteLoadLobbyPlayerContainer>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, ref variant10))
			{
				this._readyAndWaitingContainer = variant10.As<Control>();
			}
		}

		// Token: 0x0400116B RID: 4459
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_load_screen");

		// Token: 0x0400116C RID: 4460
		private static readonly LocString _ascensionLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.ASCENSION");

		// Token: 0x0400116D RID: 4461
		public static readonly string dateFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.DATE_FORMAT");

		// Token: 0x0400116E RID: 4462
		private MegaRichTextLabel _dateLabel;

		// Token: 0x0400116F RID: 4463
		private NConfirmButton _embarkButton;

		// Token: 0x04001170 RID: 4464
		private NBackButton _backButton;

		// Token: 0x04001171 RID: 4465
		private NBackButton _unreadyButton;

		// Token: 0x04001172 RID: 4466
		private NDailyRunCharacterContainer _characterContainer;

		// Token: 0x04001173 RID: 4467
		private NDailyRunLeaderboard _leaderboard;

		// Token: 0x04001174 RID: 4468
		private MegaLabel _modifiersTitleLabel;

		// Token: 0x04001175 RID: 4469
		private Control _modifiersContainer;

		// Token: 0x04001176 RID: 4470
		private readonly List<NDailyRunScreenModifier> _modifierContainers = new List<NDailyRunScreenModifier>();

		// Token: 0x04001177 RID: 4471
		private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x04001178 RID: 4472
		private Control _readyAndWaitingContainer;

		// Token: 0x04001179 RID: 4473
		[Nullable(2)]
		private LoadRunLobby _lobby;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200159C RID: 5532
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040052E3 RID: 21219
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040052E4 RID: 21220
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x040052E5 RID: 21221
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x040052E6 RID: 21222
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'InitializeDisplay' method.
			/// </summary>
			// Token: 0x040052E7 RID: 21223
			public static readonly StringName InitializeDisplay = "InitializeDisplay";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x040052E8 RID: 21224
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x040052E9 RID: 21225
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x040052EA RID: 21226
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040052EB RID: 21227
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x040052EC RID: 21228
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'PlayerConnected' method.
			/// </summary>
			// Token: 0x040052ED RID: 21229
			public static readonly StringName PlayerConnected = "PlayerConnected";

			/// <summary>
			/// Cached name for the 'PlayerReadyChanged' method.
			/// </summary>
			// Token: 0x040052EE RID: 21230
			public static readonly StringName PlayerReadyChanged = "PlayerReadyChanged";

			/// <summary>
			/// Cached name for the 'RemotePlayerDisconnected' method.
			/// </summary>
			// Token: 0x040052EF RID: 21231
			public static readonly StringName RemotePlayerDisconnected = "RemotePlayerDisconnected";

			/// <summary>
			/// Cached name for the 'BeginRun' method.
			/// </summary>
			// Token: 0x040052F0 RID: 21232
			public static readonly StringName BeginRun = "BeginRun";

			/// <summary>
			/// Cached name for the 'AfterMultiplayerStarted' method.
			/// </summary>
			// Token: 0x040052F1 RID: 21233
			public static readonly StringName AfterMultiplayerStarted = "AfterMultiplayerStarted";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200159D RID: 5533
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x040052F2 RID: 21234
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_dateLabel' field.
			/// </summary>
			// Token: 0x040052F3 RID: 21235
			public static readonly StringName _dateLabel = "_dateLabel";

			/// <summary>
			/// Cached name for the '_embarkButton' field.
			/// </summary>
			// Token: 0x040052F4 RID: 21236
			public static readonly StringName _embarkButton = "_embarkButton";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x040052F5 RID: 21237
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x040052F6 RID: 21238
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_characterContainer' field.
			/// </summary>
			// Token: 0x040052F7 RID: 21239
			public static readonly StringName _characterContainer = "_characterContainer";

			/// <summary>
			/// Cached name for the '_leaderboard' field.
			/// </summary>
			// Token: 0x040052F8 RID: 21240
			public static readonly StringName _leaderboard = "_leaderboard";

			/// <summary>
			/// Cached name for the '_modifiersTitleLabel' field.
			/// </summary>
			// Token: 0x040052F9 RID: 21241
			public static readonly StringName _modifiersTitleLabel = "_modifiersTitleLabel";

			/// <summary>
			/// Cached name for the '_modifiersContainer' field.
			/// </summary>
			// Token: 0x040052FA RID: 21242
			public static readonly StringName _modifiersContainer = "_modifiersContainer";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x040052FB RID: 21243
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_readyAndWaitingContainer' field.
			/// </summary>
			// Token: 0x040052FC RID: 21244
			public static readonly StringName _readyAndWaitingContainer = "_readyAndWaitingContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200159E RID: 5534
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
