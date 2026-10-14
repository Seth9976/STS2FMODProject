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
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x02000341 RID: 833
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NMultiplayerLoadGameScreen.cs")]
	public class NMultiplayerLoadGameScreen : NSubmenu, ILoadRunLobbyListener
	{
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x0015995E File Offset: 0x00157B5E
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NMultiplayerLoadGameScreen._scenePath,
					"res://scenes/screens/char_select/char_select_button.tscn"
				});
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x0015997B File Offset: 0x00157B7B
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x0015997E File Offset: 0x00157B7E
		[NullableContext(2)]
		public static NMultiplayerLoadGameScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMultiplayerLoadGameScreen._scenePath).Instantiate<NMultiplayerLoadGameScreen>(0L);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x001599A0 File Offset: 0x00157BA0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._infoPanel = base.GetNode<Control>("InfoPanel");
			this._name = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/Name");
			this._hp = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Hp/Label");
			this._gold = base.GetNode<MegaLabel>("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Gold/Label");
			this._actLabel = base.GetNode<MegaRichTextLabel>("InfoPanel/VBoxContainer/RunLocation/ActLabel");
			this._floorLabel = base.GetNode<MegaRichTextLabel>("InfoPanel/VBoxContainer/RunLocation/FloorLabel");
			this._bgContainer = base.GetNode<Control>("AnimatedBg");
			this._ascensionPanel = base.GetNode<NAscensionPanel>("%AscensionPanel");
			this._remotePlayerContainer = base.GetNode<NRemoteLoadLobbyPlayerContainer>("RemotePlayerLoadContainer");
			this._confirmButton = base.GetNode<NConfirmButton>("ConfirmButton");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._unreadyButton = base.GetNode<NBackButton>("UnreadyButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
			this._unreadyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
			this._unreadyButton.Disable();
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00159B0C File Offset: 0x00157D0C
		public void InitializeAsHost(INetGameService gameService, SerializableRun run)
		{
			if (gameService.Type != NetGameType.Host)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with GameService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when hosting!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._runLobby = new LoadRunLobby(gameService, this, run);
			try
			{
				this._runLobby.AddLocalHostPlayer();
				this.AfterMultiplayerStarted();
			}
			catch
			{
				this.CleanUpLobby(true, NetError.InternalError);
				throw;
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x00159B9C File Offset: 0x00157D9C
		public void InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)
		{
			if (gameService.Type != NetGameType.Client)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Initialized character select screen with GameService of type ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(gameService.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" when joining!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._runLobby = new LoadRunLobby(gameService, this, message);
			this.AfterMultiplayerStarted();
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x00159C04 File Offset: 0x00157E04
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._confirmButton.Enable();
			this._remotePlayerContainer.Initialize(this._runLobby, false);
			this._ascensionPanel.Initialize(MultiplayerUiMode.Load);
			this._ascensionPanel.SetAscensionLevel(this._runLobby.Run.Ascension);
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x00159C5C File Offset: 0x00157E5C
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			this._confirmButton.Disable();
			this._remotePlayerContainer.Cleanup();
			if (this._runLobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			}
			this.CleanUpLobby(true, NetError.Quit);
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00159CB8 File Offset: 0x00157EB8
		private void OnEmbarkPressed(NButton _)
		{
			this._confirmButton.Disable();
			this._backButton.Disable();
			this._runLobby.SetReady(true);
			if (!this._runLobby.IsAboutToBeginGame())
			{
				this._unreadyButton.Enable();
			}
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x00159CF4 File Offset: 0x00157EF4
		private void OnUnreadyPressed(NButton _)
		{
			this._confirmButton.Enable();
			this._backButton.Enable();
			this._unreadyButton.Disable();
			this._runLobby.SetReady(false);
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x00159D24 File Offset: 0x00157F24
		private void UpdateRichPresence()
		{
			if (this._runLobby.NetService.Type.IsMultiplayer())
			{
				PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._runLobby.NetService.GetRawLobbyIdentifier(), new int?(this._runLobby.ConnectedPlayerIds.Count));
			}
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x00159D77 File Offset: 0x00157F77
		public override void _Process(double delta)
		{
			if (this._runLobby == null)
			{
				return;
			}
			if (this._runLobby.NetService.IsConnected)
			{
				this._runLobby.NetService.Update();
			}
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x00159DA4 File Offset: 0x00157FA4
		private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
		{
			this._runLobby.CleanUp(disconnectSession, error);
			this._runLobby = null;
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x00159DBC File Offset: 0x00157FBC
		public async Task<bool> ShouldAllowRunToBegin()
		{
			bool flag;
			if (this._runLobby.ConnectedPlayerIds.Count >= this._runLobby.Run.Players.Count)
			{
				flag = true;
			}
			else
			{
				LocString locString = new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.body");
				locString.Add("MissingCount", this._runLobby.Run.Players.Count - this._runLobby.ConnectedPlayerIds.Count);
				NGenericPopup ngenericPopup = NGenericPopup.Create();
				NModalContainer.Instance.Add(ngenericPopup, true);
				bool flag2 = await ngenericPopup.WaitForConfirmation(locString, new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.header"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.cancel"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.confirm"));
				bool flag3 = flag2;
				flag = flag3;
			}
			return flag;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x00159E00 File Offset: 0x00158000
		private async Task StartRun()
		{
			int num = 0;
			try
			{
				Log.Info("Loading a multiplayer run. Players: " + string.Join<ulong>(",", this._runLobby.ConnectedPlayerIds) + ".", 2);
				SerializablePlayer serializablePlayer = this._runLobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == this._runLobby.NetService.NetId);
				SfxCmd.Play(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterSelectTransitionPath, null);
				RunState runState = RunState.FromSerializable(this._runLobby.Run);
				await RunManager.Instance.SetUpSavedMultiplayer(runState, this._runLobby);
				await NGame.Instance.LoadRun(runState, this._runLobby.Run.PreFinishedRoom);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception loading multiplayer run : ");
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

		// Token: 0x06002F6D RID: 12141 RVA: 0x00159E44 File Offset: 0x00158044
		public void PlayerConnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player connected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerConnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x00159E90 File Offset: 0x00158090
		public void PlayerReadyChanged(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ready changed: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerChanged(playerId);
			if (playerId == this._runLobby.NetService.NetId && !this._runLobby.IsPlayerReady(playerId))
			{
				this._confirmButton.Enable();
				this._backButton.Enable();
				this._unreadyButton.Disable();
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x00159F18 File Offset: 0x00158118
		public void RemotePlayerDisconnected(ulong playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player disconnected: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._remotePlayerContainer.OnPlayerDisconnected(playerId);
			this.UpdateRichPresence();
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x00159F62 File Offset: 0x00158162
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

		// Token: 0x06002F71 RID: 12145 RVA: 0x00159F98 File Offset: 0x00158198
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

		// Token: 0x06002F72 RID: 12146 RVA: 0x0015A000 File Offset: 0x00158200
		private void AfterMultiplayerStarted()
		{
			NGame.Instance.RemoteCursorContainer.Initialize(this._runLobby.InputSynchronizer, this._runLobby.ConnectedPlayerIds);
			NGame.Instance.ReactionContainer.InitializeNetworking(this._runLobby.NetService);
			SerializablePlayer serializablePlayer = this._runLobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == this._runLobby.NetService.NetId);
			CharacterModel byId = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
			SfxCmd.Play(byId.CharacterSelectSfx, 1f);
			foreach (Node node in this._bgContainer.GetChildren(false))
			{
				this._bgContainer.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
			Control control = PreloadManager.Cache.GetScene(byId.CharacterSelectBg).Instantiate<Control>(0L);
			control.Name = byId.Id.Entry + "_bg";
			this._bgContainer.AddChildSafely(control);
			this._name.SetTextAutoSize(byId.Title.GetFormattedText());
			MegaLabel hp = this._hp;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.CurrentHp);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.MaxHp);
			hp.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			MegaLabel gold = this._gold;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.Gold);
			gold.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			LocString locString = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.FLOOR");
			locString.Add("floor", this._runLobby.Run.VisitedMapCoords.Count);
			this._floorLabel.Text = locString.GetFormattedText();
			LocString locString2 = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.ACT");
			locString2.Add("act", this._runLobby.Run.CurrentActIndex + 1);
			this._actLabel.Text = locString2.GetFormattedText();
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
		// Token: 0x06002F73 RID: 12147 RVA: 0x0015A25C File Offset: 0x0015845C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.CleanUpLobby, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "disconnectSession", 0L, "", 6L, false),
					new PropertyInfo(2L, "error", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.PlayerConnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.BeginRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F74 RID: 12148 RVA: 0x0015A64C File Offset: 0x0015884C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.Create && args.Count == 0)
			{
				NMultiplayerLoadGameScreen nmultiplayerLoadGameScreen = NMultiplayerLoadGameScreen.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref nmultiplayerLoadGameScreen);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed && args.Count == 1)
			{
				this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed && args.Count == 1)
			{
				this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence && args.Count == 0)
			{
				this.UpdateRichPresence();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.CleanUpLobby && args.Count == 2)
			{
				this.CleanUpLobby(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<NetError>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.PlayerConnected && args.Count == 1)
			{
				this.PlayerConnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged && args.Count == 1)
			{
				this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected && args.Count == 1)
			{
				this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.BeginRun && args.Count == 0)
			{
				this.BeginRun();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted && args.Count == 0)
			{
				this.AfterMultiplayerStarted();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x0015A8E0 File Offset: 0x00158AE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerLoadGameScreen.MethodName.Create && args.Count == 0)
			{
				NMultiplayerLoadGameScreen nmultiplayerLoadGameScreen = NMultiplayerLoadGameScreen.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref nmultiplayerLoadGameScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002F76 RID: 12150 RVA: 0x0015A920 File Offset: 0x00158B20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerLoadGameScreen.MethodName.Create || (ref method) == NMultiplayerLoadGameScreen.MethodName._Ready || (ref method) == NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened || (ref method) == NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed || (ref method) == NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed || (ref method) == NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed || (ref method) == NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence || (ref method) == NMultiplayerLoadGameScreen.MethodName._Process || (ref method) == NMultiplayerLoadGameScreen.MethodName.CleanUpLobby || (ref method) == NMultiplayerLoadGameScreen.MethodName.PlayerConnected || (ref method) == NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged || (ref method) == NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected || (ref method) == NMultiplayerLoadGameScreen.MethodName.BeginRun || (ref method) == NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002F77 RID: 12151 RVA: 0x0015AA08 File Offset: 0x00158C08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._name)
			{
				this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanel)
			{
				this._infoPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._hp)
			{
				this._hp = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._gold)
			{
				this._gold = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._selectedButton)
			{
				this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._bgContainer)
			{
				this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._unreadyButton)
			{
				this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._ascensionPanel)
			{
				this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._floorLabel)
			{
				this._floorLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._actLabel)
			{
				this._actLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer)
			{
				this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanelTween)
			{
				this._infoPanelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal)
			{
				this._infoPanelPosFinalVal = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002F78 RID: 12152 RVA: 0x0015ABB4 File Offset: 0x00158DB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._name)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._infoPanel);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._hp)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._hp);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._gold)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._gold);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._selectedButton)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._bgContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._unreadyButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._ascensionPanel)
			{
				value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._floorLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._floorLabel);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._actLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._actLabel);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer)
			{
				value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._infoPanelTween);
				return true;
			}
			if ((ref name) == NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._infoPanelPosFinalVal);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F79 RID: 12153 RVA: 0x0015ADCC File Offset: 0x00158FCC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._name, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._infoPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._hp, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._gold, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._selectedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._bgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._unreadyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._floorLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._actLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerLoadGameScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F7A RID: 12154 RVA: 0x0015AFF0 File Offset: 0x001591F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanel, Variant.From<Control>(ref this._infoPanel));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._hp, Variant.From<MegaLabel>(ref this._hp));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._gold, Variant.From<MegaLabel>(ref this._gold));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._floorLabel, Variant.From<MegaRichTextLabel>(ref this._floorLabel));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._actLabel, Variant.From<MegaRichTextLabel>(ref this._actLabel));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, Variant.From<Tween>(ref this._infoPanelTween));
			info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, Variant.From<Vector2>(ref this._infoPanelPosFinalVal));
		}

		/// <inheritdoc />
		// Token: 0x06002F7B RID: 12155 RVA: 0x0015B150 File Offset: 0x00159350
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._name, ref variant))
			{
				this._name = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanel, ref variant2))
			{
				this._infoPanel = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._hp, ref variant3))
			{
				this._hp = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._gold, ref variant4))
			{
				this._gold = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._selectedButton, ref variant5))
			{
				this._selectedButton = variant5.As<NCharacterSelectButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._bgContainer, ref variant6))
			{
				this._bgContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._confirmButton, ref variant7))
			{
				this._confirmButton = variant7.As<NConfirmButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._backButton, ref variant8))
			{
				this._backButton = variant8.As<NBackButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._unreadyButton, ref variant9))
			{
				this._unreadyButton = variant9.As<NBackButton>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, ref variant10))
			{
				this._ascensionPanel = variant10.As<NAscensionPanel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._floorLabel, ref variant11))
			{
				this._floorLabel = variant11.As<MegaRichTextLabel>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._actLabel, ref variant12))
			{
				this._actLabel = variant12.As<MegaRichTextLabel>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, ref variant13))
			{
				this._remotePlayerContainer = variant13.As<NRemoteLoadLobbyPlayerContainer>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, ref variant14))
			{
				this._infoPanelTween = variant14.As<Tween>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, ref variant15))
			{
				this._infoPanelPosFinalVal = variant15.As<Vector2>();
			}
		}

		// Token: 0x04001232 RID: 4658
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_load_game_screen");

		// Token: 0x04001233 RID: 4659
		private MegaLabel _name;

		// Token: 0x04001234 RID: 4660
		private Control _infoPanel;

		// Token: 0x04001235 RID: 4661
		private MegaLabel _hp;

		// Token: 0x04001236 RID: 4662
		private MegaLabel _gold;

		// Token: 0x04001237 RID: 4663
		[Nullable(2)]
		private NCharacterSelectButton _selectedButton;

		// Token: 0x04001238 RID: 4664
		private Control _bgContainer;

		// Token: 0x04001239 RID: 4665
		private NConfirmButton _confirmButton;

		// Token: 0x0400123A RID: 4666
		private NBackButton _backButton;

		// Token: 0x0400123B RID: 4667
		private NBackButton _unreadyButton;

		// Token: 0x0400123C RID: 4668
		private NAscensionPanel _ascensionPanel;

		// Token: 0x0400123D RID: 4669
		private MegaRichTextLabel _floorLabel;

		// Token: 0x0400123E RID: 4670
		private MegaRichTextLabel _actLabel;

		// Token: 0x0400123F RID: 4671
		private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;

		// Token: 0x04001240 RID: 4672
		[Nullable(2)]
		private Tween _infoPanelTween;

		// Token: 0x04001241 RID: 4673
		private Vector2 _infoPanelPosFinalVal;

		// Token: 0x04001242 RID: 4674
		private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";

		// Token: 0x04001243 RID: 4675
		private LoadRunLobby _runLobby;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E9 RID: 5609
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040054CD RID: 21709
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040054CE RID: 21710
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x040054CF RID: 21711
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x040054D0 RID: 21712
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnEmbarkPressed' method.
			/// </summary>
			// Token: 0x040054D1 RID: 21713
			public static readonly StringName OnEmbarkPressed = "OnEmbarkPressed";

			/// <summary>
			/// Cached name for the 'OnUnreadyPressed' method.
			/// </summary>
			// Token: 0x040054D2 RID: 21714
			public static readonly StringName OnUnreadyPressed = "OnUnreadyPressed";

			/// <summary>
			/// Cached name for the 'UpdateRichPresence' method.
			/// </summary>
			// Token: 0x040054D3 RID: 21715
			public static readonly StringName UpdateRichPresence = "UpdateRichPresence";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040054D4 RID: 21716
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'CleanUpLobby' method.
			/// </summary>
			// Token: 0x040054D5 RID: 21717
			public static readonly StringName CleanUpLobby = "CleanUpLobby";

			/// <summary>
			/// Cached name for the 'PlayerConnected' method.
			/// </summary>
			// Token: 0x040054D6 RID: 21718
			public static readonly StringName PlayerConnected = "PlayerConnected";

			/// <summary>
			/// Cached name for the 'PlayerReadyChanged' method.
			/// </summary>
			// Token: 0x040054D7 RID: 21719
			public static readonly StringName PlayerReadyChanged = "PlayerReadyChanged";

			/// <summary>
			/// Cached name for the 'RemotePlayerDisconnected' method.
			/// </summary>
			// Token: 0x040054D8 RID: 21720
			public static readonly StringName RemotePlayerDisconnected = "RemotePlayerDisconnected";

			/// <summary>
			/// Cached name for the 'BeginRun' method.
			/// </summary>
			// Token: 0x040054D9 RID: 21721
			public static readonly StringName BeginRun = "BeginRun";

			/// <summary>
			/// Cached name for the 'AfterMultiplayerStarted' method.
			/// </summary>
			// Token: 0x040054DA RID: 21722
			public static readonly StringName AfterMultiplayerStarted = "AfterMultiplayerStarted";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015EA RID: 5610
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x040054DB RID: 21723
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_name' field.
			/// </summary>
			// Token: 0x040054DC RID: 21724
			public static readonly StringName _name = "_name";

			/// <summary>
			/// Cached name for the '_infoPanel' field.
			/// </summary>
			// Token: 0x040054DD RID: 21725
			public static readonly StringName _infoPanel = "_infoPanel";

			/// <summary>
			/// Cached name for the '_hp' field.
			/// </summary>
			// Token: 0x040054DE RID: 21726
			public static readonly StringName _hp = "_hp";

			/// <summary>
			/// Cached name for the '_gold' field.
			/// </summary>
			// Token: 0x040054DF RID: 21727
			public static readonly StringName _gold = "_gold";

			/// <summary>
			/// Cached name for the '_selectedButton' field.
			/// </summary>
			// Token: 0x040054E0 RID: 21728
			public static readonly StringName _selectedButton = "_selectedButton";

			/// <summary>
			/// Cached name for the '_bgContainer' field.
			/// </summary>
			// Token: 0x040054E1 RID: 21729
			public static readonly StringName _bgContainer = "_bgContainer";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040054E2 RID: 21730
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x040054E3 RID: 21731
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unreadyButton' field.
			/// </summary>
			// Token: 0x040054E4 RID: 21732
			public static readonly StringName _unreadyButton = "_unreadyButton";

			/// <summary>
			/// Cached name for the '_ascensionPanel' field.
			/// </summary>
			// Token: 0x040054E5 RID: 21733
			public static readonly StringName _ascensionPanel = "_ascensionPanel";

			/// <summary>
			/// Cached name for the '_floorLabel' field.
			/// </summary>
			// Token: 0x040054E6 RID: 21734
			public static readonly StringName _floorLabel = "_floorLabel";

			/// <summary>
			/// Cached name for the '_actLabel' field.
			/// </summary>
			// Token: 0x040054E7 RID: 21735
			public static readonly StringName _actLabel = "_actLabel";

			/// <summary>
			/// Cached name for the '_remotePlayerContainer' field.
			/// </summary>
			// Token: 0x040054E8 RID: 21736
			public static readonly StringName _remotePlayerContainer = "_remotePlayerContainer";

			/// <summary>
			/// Cached name for the '_infoPanelTween' field.
			/// </summary>
			// Token: 0x040054E9 RID: 21737
			public static readonly StringName _infoPanelTween = "_infoPanelTween";

			/// <summary>
			/// Cached name for the '_infoPanelPosFinalVal' field.
			/// </summary>
			// Token: 0x040054EA RID: 21738
			public static readonly StringName _infoPanelPosFinalVal = "_infoPanelPosFinalVal";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015EB RID: 5611
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
