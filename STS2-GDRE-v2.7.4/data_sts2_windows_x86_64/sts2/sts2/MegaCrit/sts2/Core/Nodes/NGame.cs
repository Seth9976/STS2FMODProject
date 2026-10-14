using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Reaction;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Unlocks;
using Steamworks;

namespace MegaCrit.Sts2.Core.Nodes
{
	// Token: 0x02000178 RID: 376
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/NGame.cs")]
	public class NGame : Control
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x000666A8 File Offset: 0x000648A8
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x000666AF File Offset: 0x000648AF
		[Nullable(2)]
		public static NGame Instance
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x000666B7 File Offset: 0x000648B7
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x000666BF File Offset: 0x000648BF
		public NSceneContainer RootSceneContainer { get; private set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x000666C8 File Offset: 0x000648C8
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x000666D0 File Offset: 0x000648D0
		[Nullable(2)]
		public Node HoverTipsContainer
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x000666D9 File Offset: 0x000648D9
		[Nullable(2)]
		public NMainMenu MainMenu
		{
			[NullableContext(2)]
			get
			{
				return this.RootSceneContainer.CurrentScene as NMainMenu;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x000666EB File Offset: 0x000648EB
		[Nullable(2)]
		public NRun CurrentRunNode
		{
			[NullableContext(2)]
			get
			{
				return this.RootSceneContainer.CurrentScene as NRun;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x000666FD File Offset: 0x000648FD
		[Nullable(2)]
		public NLogoAnimation LogoAnimation
		{
			[NullableContext(2)]
			get
			{
				return this.RootSceneContainer.CurrentScene as NLogoAnimation;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x0006670F File Offset: 0x0006490F
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x00066717 File Offset: 0x00064917
		public NTransition Transition { get; private set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00066720 File Offset: 0x00064920
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x00066728 File Offset: 0x00064928
		public NMultiplayerTimeoutOverlay TimeoutOverlay { get; private set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00066731 File Offset: 0x00064931
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x00066739 File Offset: 0x00064939
		public NAudioManager AudioManager { get; private set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x00066742 File Offset: 0x00064942
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x0006674A File Offset: 0x0006494A
		public NRemoteMouseCursorContainer RemoteCursorContainer { get; private set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x00066753 File Offset: 0x00064953
		// (set) Token: 0x060011A1 RID: 4513 RVA: 0x0006675B File Offset: 0x0006495B
		public NInputManager InputManager { get; private set; }

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00066764 File Offset: 0x00064964
		// (set) Token: 0x060011A3 RID: 4515 RVA: 0x0006676C File Offset: 0x0006496C
		public NHotkeyManager HotkeyManager { get; private set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00066775 File Offset: 0x00064975
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x0006677D File Offset: 0x0006497D
		[Nullable(2)]
		public NSendFeedbackScreen FeedbackScreen
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00066786 File Offset: 0x00064986
		// (set) Token: 0x060011A7 RID: 4519 RVA: 0x0006678E File Offset: 0x0006498E
		public NReactionWheel ReactionWheel { get; private set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x00066797 File Offset: 0x00064997
		// (set) Token: 0x060011A9 RID: 4521 RVA: 0x0006679F File Offset: 0x0006499F
		public NReactionContainer ReactionContainer { get; private set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x000667A8 File Offset: 0x000649A8
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x000667B0 File Offset: 0x000649B0
		public NCursorManager CursorManager { get; private set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x000667B9 File Offset: 0x000649B9
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x000667C1 File Offset: 0x000649C1
		public NDebugAudioManager DebugAudio { get; private set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x000667CA File Offset: 0x000649CA
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x000667D2 File Offset: 0x000649D2
		[Nullable(2)]
		public string DebugSeedOverride
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x000667DB File Offset: 0x000649DB
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x000667E3 File Offset: 0x000649E3
		public bool StartOnMainMenu { get; set; } = true;

		/// <summary>
		/// Completes when <see cref="M:MegaCrit.Sts2.Core.Nodes.NGame.GameStartup" /> has finished (including <see cref="T:MegaCrit.Sts2.Core.Models.ModelDb" /> initialization), whether
		/// it succeeded or threw. Used by <see cref="T:MegaCrit.Sts2.Core.Nodes.Debug.NSceneBootstrapper" /> to wait for initialization before starting a
		/// debug run, since it can create its own NGame and would otherwise race the async startup.
		/// </summary>
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x000667EC File Offset: 0x000649EC
		public Task GameStartupComplete
		{
			get
			{
				return this._gameStartupComplete.Task;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x000667F9 File Offset: 0x000649F9
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x00066800 File Offset: 0x00064A00
		public static bool IsTrailerMode { get; private set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00066808 File Offset: 0x00064A08
		// (set) Token: 0x060011B6 RID: 4534 RVA: 0x0006680F File Offset: 0x00064A0F
		public static bool IsDebugHidingHoverTips { get; private set; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00066817 File Offset: 0x00064A17
		// (set) Token: 0x060011B8 RID: 4536 RVA: 0x0006681E File Offset: 0x00064A1E
		public static bool IsDebugHidingProceedButton { get; private set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060011B9 RID: 4537 RVA: 0x00066828 File Offset: 0x00064A28
		// (remove) Token: 0x060011BA RID: 4538 RVA: 0x00066860 File Offset: 0x00064A60
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DebugToggleProceedButton;

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00066895 File Offset: 0x00064A95
		// (set) Token: 0x060011BC RID: 4540 RVA: 0x0006689D File Offset: 0x00064A9D
		[Nullable(2)]
		public NInspectRelicScreen InspectRelicScreen
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x000668A6 File Offset: 0x00064AA6
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x000668AE File Offset: 0x00064AAE
		[Nullable(2)]
		public NInspectCardScreen InspectCardScreen
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x000668B7 File Offset: 0x00064AB7
		[Nullable(2)]
		public Control ScreenshakeTarget
		{
			[NullableContext(2)]
			get
			{
				return this._screenShake.ShakeTarget;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x000668C4 File Offset: 0x00064AC4
		// (set) Token: 0x060011C1 RID: 4545 RVA: 0x000668CC File Offset: 0x00064ACC
		private WorldEnvironment WorldEnvironment { get; set; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x000668D5 File Offset: 0x00064AD5
		// (set) Token: 0x060011C3 RID: 4547 RVA: 0x000668DD File Offset: 0x00064ADD
		private NHitStop HitStop { get; set; }

		// Token: 0x060011C4 RID: 4548 RVA: 0x000668E8 File Offset: 0x00064AE8
		public override void _EnterTree()
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
			if (NGame.Instance != null)
			{
				Log.Error("NGame already exists.", 2);
				this.QueueFreeSafely();
				return;
			}
			NGame.Instance = this;
			SentryService.Initialize();
			this.RootSceneContainer = base.GetNode<NSceneContainer>("%RootSceneContainer");
			this.HoverTipsContainer = base.GetNode<Node>("%HoverTipsContainer");
			this.DebugAudio = base.GetNode<NDebugAudioManager>("%DebugAudioManager");
			this.AudioManager = base.GetNode<NAudioManager>("%AudioManager");
			this.RemoteCursorContainer = base.GetNode<NRemoteMouseCursorContainer>("%RemoteCursorContainer");
			this.InputManager = base.GetNode<NInputManager>("%InputManager");
			this.CursorManager = base.GetNode<NCursorManager>("%CursorManager");
			this.ReactionWheel = base.GetNode<NReactionWheel>("%ReactionWheel");
			this.ReactionContainer = base.GetNode<NReactionContainer>("%ReactionContainer");
			this.TimeoutOverlay = base.GetNode<NMultiplayerTimeoutOverlay>("%MultiplayerTimeoutOverlay");
			this.WorldEnvironment = base.GetNode<WorldEnvironment>("%WorldEnvironment");
			this.HotkeyManager = base.GetNode<NHotkeyManager>("%HotkeyManager");
			this._inspectionContainer = base.GetNode<Control>("%InspectionContainer");
			this._screenShake = base.GetNode<NScreenShake>("ScreenShake");
			this.HitStop = base.GetNode<NHitStop>("HitStop");
			this.Transition = base.GetNode<NTransition>("%GameTransitionRect");
			NGame._mainThreadId = new int?(Environment.CurrentManagedThreadId);
			GodotObject window = base.GetWindow();
			StringName filesDropped = Window.SignalName.FilesDropped;
			Action<string[]> action;
			if ((action = NGame.<>O.<0>__OnFilesDropped) == null)
			{
				action = (NGame.<>O.<0>__OnFilesDropped = new Action<string[]>(FileDropHandler.OnFilesDropped));
			}
			window.Connect(filesDropped, Callable.From<string[]>(action), 0U);
			TaskHelper.RunSafely(this.GameStartupWrapper());
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00066ADA File Offset: 0x00064CDA
		public override void _Ready()
		{
			NGame._window = base.GetTree().Root;
			NGame._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.RemoveChildSafely(this.WorldEnvironment);
		}

		/// <summary>
		/// Wraps game startup and shows an error if there is a problem.
		/// </summary>
		// Token: 0x060011C6 RID: 4550 RVA: 0x00066B1C File Offset: 0x00064D1C
		private async Task GameStartupWrapper()
		{
			TaskAwaiter<bool> taskAwaiter = this.InitializePlatform().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this._gameStartupComplete.TrySetResult();
			}
			else
			{
				TaskHelper.RunSafely(OsDebugInfo.LogSystemInfo());
				TaskHelper.RunSafely(GitHelper.Initialize());
				try
				{
					await this.GameStartup();
					this._gameStartupComplete.TrySetResult();
				}
				catch
				{
					this._gameStartupComplete.TrySetResult();
					TaskHelper.RunSafely(this.GameStartupError());
					throw;
				}
			}
		}

		/// <summary>
		/// Initializes only the components of the game we need to display an error.
		/// </summary>
		// Token: 0x060011C7 RID: 4551 RVA: 0x00066B60 File Offset: 0x00064D60
		private async Task TryErrorInit()
		{
			try
			{
				if (SaveManager.Instance.SettingsSave == null)
				{
					SaveManager.Instance.InitSettingsData();
				}
				if (LocManager.Instance == null)
				{
					LocManager.Initialize();
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to show error dialog! Exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				base.GetTree().Quit(0);
				throw;
			}
			if (!base.IsNodeReady())
			{
				await base.ToSignal(this, Node.SignalName.Ready);
			}
			this.Transition.Visible = false;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00066BA4 File Offset: 0x00064DA4
		private async Task GameStartupError()
		{
			Log.Error("Encountered error on game startup! Attempting to show error dialog", 2);
			await this.TryErrorInit();
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			if (ngenericPopup == null || NModalContainer.Instance == null)
			{
				Log.Error("Cannot show error dialog: UI not initialized. Quitting immediately.", 2);
				base.GetTree().Quit(0);
			}
			else
			{
				NModalContainer.Instance.Add(ngenericPopup, true);
				await ngenericPopup.WaitForConfirmation(new LocString("main_menu_ui", "STARTUP_ERROR.description"), new LocString("main_menu_ui", "STARTUP_ERROR.title"), null, new LocString("main_menu_ui", "QUIT"));
				base.GetTree().Quit(0);
			}
		}

		/// <summary>
		/// Does all the game-level initialization tasks asynchronously.
		/// Stuff that should happen as early as possible should go in _EnterTree.
		/// Beware of changing the order of operations in this method. There are dependencies that are non-obvious, and you
		/// should read and understand the methods you change.
		/// </summary>
		// Token: 0x060011C9 RID: 4553 RVA: 0x00066BE8 File Offset: 0x00064DE8
		private async Task GameStartup()
		{
			await OneTimeInitialization.ExecuteVeryEarly();
			Node node = NDevConsole.Create();
			this.AddChildSafely(node);
			this.MoveChildSafely(node, base.GetChildCount(false) - 1);
			AccountScopeUserDataMigrator.MigrateToUserScopedDirectories();
			AccountScopeUserDataMigrator.ArchiveLegacyData();
			ProfileAccountScopeMigrator.MigrateToProfileScopedDirectories();
			ProfileAccountScopeMigrator.ArchiveLegacyData();
			Task cloudSavesTask = this.DoCloudSync();
			this.InitPools();
			OneTimeInitialization.ExecuteEssential();
			if (!base.IsNodeReady())
			{
				await base.ToSignal(this, Node.SignalName.Ready);
			}
			Callable.From(new Action(this.InitializeGraphicsPreferences)).CallDeferred(Array.Empty<Variant>());
			this.AudioManager.SetMasterVol(SaveManager.Instance.SettingsSave.VolumeMaster);
			this.AudioManager.SetSfxVol(SaveManager.Instance.SettingsSave.VolumeSfx);
			this.AudioManager.SetAmbienceVol(SaveManager.Instance.SettingsSave.VolumeAmbience);
			this.AudioManager.SetBgmVol(SaveManager.Instance.SettingsSave.VolumeBgm);
			this.DebugAudio.SetMasterAudioVolume(SaveManager.Instance.SettingsSave.VolumeMaster);
			this.DebugAudio.SetSfxAudioVolume(SaveManager.Instance.SettingsSave.VolumeSfx);
			LeaderboardManager.Initialize();
			SteamStatsManager.Initialize();
			this.TimeoutOverlay.Relocalize();
			while (!cloudSavesTask.IsCompleted)
			{
				if (!SteamInitializer.Initialized)
				{
					Log.Error("Steam became uninitialized while the cloud sync was in-progress! This might result in unpredictable behavior", 2);
					break;
				}
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			SaveManager.Instance.InitProfileId(null);
			ReadSaveResult<SerializableProgress> progressReadResult = SaveManager.Instance.InitProgressData();
			ReadSaveResult<PrefsSave> prefsReadResult = SaveManager.Instance.InitPrefsData();
			SentryService.AfterGameInit(PlatformUtil.GetPlatformBranch().ToName(), SaveManager.Instance.Progress.UniqueId, base.GetTree().Root);
			this._screenShake.SetMultiplier(NScreenshakePaginator.GetShakeMultiplier(SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex));
			if (!OS.HasFeature("editor") && SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
			}
			if (!NGame.IsReleaseGame() && CommandLineHelper.HasArg("autoslay"))
			{
				await this.LaunchMainMenu(true);
				string text = CommandLineHelper.GetValue("seed") ?? SeedHelper.GetRandomSeed(null, 12);
				string value = CommandLineHelper.GetValue("log-file");
				new AutoSlayer().Start(text, value);
			}
			else if (CommandLineHelper.HasArg("bootstrap"))
			{
				this.AddChildSafely(SceneHelper.Instantiate<NSceneBootstrapper>("debug/scene_bootstrapper"));
			}
			else if (this.StartOnMainMenu)
			{
				await this.LaunchMainMenu(DebugSettings.DevSkip || SaveManager.Instance.SettingsSave.SkipIntroLogo || CommandLineHelper.HasArg("fastmp"));
				this.CheckShowSaveFileError(progressReadResult, prefsReadResult, OneTimeInitialization.SettingsReadResult);
				this.CheckShowLocalizationOverrideErrors();
				this.CheckShowModdedSaveFilePopup();
			}
			ModManager.OnModDetected += this.OnNewModDetected;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00066C2C File Offset: 0x00064E2C
		private async Task DoCloudSync()
		{
			bool flag = SaveManager.Instance.ShouldOverwriteCloudWithLocal();
			if (flag)
			{
				await SaveManager.Instance.OverwriteCloudWithLocal();
			}
			else
			{
				await SaveManager.Instance.SyncCloudToLocal();
			}
		}

		/// <summary>
		/// Called when we set our Aspect Ratio to Auto mode OR when the size of the window is changed while Aspect Ratio is Auto.
		/// </summary>
		// Token: 0x060011CB RID: 4555 RVA: 0x00066C68 File Offset: 0x00064E68
		private void OnWindowChange()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Window changed! New size: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.WindowGetSize(0));
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			base.EmitSignal(NGame.SignalName.WindowChange, new Variant[] { SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto });
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00066CD8 File Offset: 0x00064ED8
		public static bool IsMainThread()
		{
			if (NGame._mainThreadId == null)
			{
				NGame._mainThreadId = new int?(Environment.CurrentManagedThreadId);
				return true;
			}
			int? mainThreadId = NGame._mainThreadId;
			int currentManagedThreadId = Environment.CurrentManagedThreadId;
			return (mainThreadId.GetValueOrDefault() == currentManagedThreadId) & (mainThreadId != null);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00066D20 File Offset: 0x00064F20
		public override void _ExitTree()
		{
			ModManager.OnModDetected -= this.OnNewModDetected;
			ModManager.Dispose();
			SteamJoinCallbackHandler joinCallbackHandler = this._joinCallbackHandler;
			if (joinCallbackHandler != null)
			{
				joinCallbackHandler.Dispose();
			}
			SteamInitializer.Uninitialize();
			SentryService.Shutdown();
		}

		/// <summary>
		/// If the game is a release build. i.e. Steam build
		/// </summary>
		// Token: 0x060011CE RID: 4558 RVA: 0x00066D53 File Offset: 0x00064F53
		public static bool IsReleaseGame()
		{
			return true;
		}

		/// <summary>
		/// Called when the game starts to apply the player's graphics settings.
		/// </summary>
		// Token: 0x060011CF RID: 4559 RVA: 0x00066D56 File Offset: 0x00064F56
		private void InitializeGraphicsPreferences()
		{
			if (!DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase))
			{
				this.ApplyDisplaySettings();
				NGame.ApplySyncSetting();
			}
			Engine.MaxFps = SaveManager.Instance.SettingsSave.FpsLimit;
		}

		/// <summary>
		/// Used to update graphics settings when we change the resolution or display mode from the options screen.
		/// </summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		// Token: 0x060011D0 RID: 4560 RVA: 0x00066D8C File Offset: 0x00064F8C
		public void ApplyDisplaySettings()
		{
			bool flag = false;
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (settingsSave.TargetDisplay == -1)
			{
				Log.Info("First time setup for display settings...", 2);
				settingsSave.TargetDisplay = DisplayServer.GetPrimaryScreen();
			}
			bool flag2 = settingsSave.Fullscreen;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
			{
				if (!flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(125, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Settings has fullscreen set to false, but we're forcing fullscreen because the platform reports our supported window mode as ");
					defaultInterpolatedStringHandler.AppendFormatted<SupportedWindowMode>(PlatformUtil.GetSupportedWindowMode());
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				flag2 = true;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Applying display settings...");
			defaultInterpolatedStringHandler.AppendLiteral("\n  FULLSCREEN: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(flag2);
			defaultInterpolatedStringHandler.AppendLiteral("\n  ASPECT_RATIO: (");
			defaultInterpolatedStringHandler.AppendFormatted<AspectRatioSetting>(settingsSave.AspectRatioSetting);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			defaultInterpolatedStringHandler.AppendLiteral("\n  TARGET_DISPLAY: (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			defaultInterpolatedStringHandler.AppendLiteral("\n  WINDOW_SIZE: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(settingsSave.WindowSize);
			defaultInterpolatedStringHandler.AppendLiteral("\n  POSITION: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(settingsSave.WindowPosition);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[Display] Min size: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.WindowGetMinSize(0));
			defaultInterpolatedStringHandler.AppendLiteral(" Max size: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.WindowGetMaxSize(0));
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (settingsSave.AspectRatioSetting != AspectRatioSetting.Auto)
			{
				NGame._window.ContentScaleAspect = 1L;
			}
			switch (settingsSave.AspectRatioSetting)
			{
			case AspectRatioSetting.FourByThree:
				NGame._window.ContentScaleSize = new Vector2I(1680, 1260);
				goto IL_0253;
			case AspectRatioSetting.SixteenByTen:
				NGame._window.ContentScaleSize = new Vector2I(1920, 1200);
				goto IL_0253;
			case AspectRatioSetting.SixteenByNine:
				NGame._window.ContentScaleSize = new Vector2I(1920, 1080);
				goto IL_0253;
			case AspectRatioSetting.TwentyOneByNine:
				NGame._window.ContentScaleSize = new Vector2I(2580, 1080);
				goto IL_0253;
			case AspectRatioSetting.Auto:
				flag = true;
				goto IL_0253;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid Aspect Ratio: ");
			defaultInterpolatedStringHandler.AppendFormatted<AspectRatioSetting>(settingsSave.AspectRatioSetting);
			throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_0253:
			int num = Environment.GetCommandLineArgs().IndexOf("-wpos");
			if (flag2 && num < 0)
			{
				if (NGame._window.Unresizable)
				{
					NGame._window.Unresizable = false;
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Display] Setting FULLSCREEN on Display: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(DisplayServer.GetScreenCount());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (settingsSave.TargetDisplay >= DisplayServer.GetScreenCount())
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
					defaultInterpolatedStringHandler.AppendLiteral("[Display] FAILED: Display ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay);
					defaultInterpolatedStringHandler.AppendLiteral(" is missing. Fallback to primary.");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					DisplayServer.WindowSetCurrentScreen(DisplayServer.GetPrimaryScreen(), 0);
					settingsSave.TargetDisplay = DisplayServer.GetPrimaryScreen();
				}
				else
				{
					DisplayServer.WindowSetCurrentScreen(settingsSave.TargetDisplay, 0);
				}
				DisplayServer.WindowSetMode(3L, 0);
			}
			else
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[Display] Attempting WINDOWED mode on Display ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(DisplayServer.GetScreenCount());
				defaultInterpolatedStringHandler.AppendLiteral(" at position ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(settingsSave.WindowPosition);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				DisplayServer.WindowSetMode(0L, 0);
				if (NGame._window.Unresizable != !settingsSave.ResizeWindows)
				{
					NGame._window.Unresizable = !settingsSave.ResizeWindows;
				}
				if (num >= 0)
				{
					Log.Info("[Display] -wpos called. Applying special logic.", 2);
					settingsSave.Fullscreen = false;
					Vector2I vector2I;
					vector2I..ctor(int.Parse(Environment.GetCommandLineArgs()[num + 1]), int.Parse(Environment.GetCommandLineArgs()[num + 2]));
					Vector2I vector2I2 = DisplayServer.ScreenGetPosition(DisplayServer.WindowGetCurrentScreen(0));
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Applying window position from command line arg: ");
					defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(vector2I);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", Environment.GetCommandLineArgs()));
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(vector2I2);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					DisplayServer.WindowSetPosition(vector2I2 + vector2I, 0);
					DisplayServer.WindowSetSize(settingsSave.WindowSize, 0);
				}
				else
				{
					Vector2I vector2I3 = DisplayServer.ScreenGetSize(settingsSave.TargetDisplay);
					Vector2I windowSize = settingsSave.WindowSize;
					if (settingsSave.WindowPosition == new Vector2I(-1, -1))
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 1);
						defaultInterpolatedStringHandler.AppendLiteral("[Display] Going from fullscreen to windowed. Attempting to center window on screen ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay);
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						settingsSave.WindowPosition = vector2I3 / 2 - windowSize / 2;
					}
					Vector2I windowPosition = settingsSave.WindowPosition;
					if (windowPosition.X < 0 || windowPosition.Y < 0 || windowPosition.X > vector2I3.X || windowPosition.Y > vector2I3.Y)
					{
						Log.Warn("[Display] WARN: Game Window was offscreen. Resetting to top left corner.", 2);
						windowPosition..ctor(8, 48);
					}
					if (settingsSave.TargetDisplay >= DisplayServer.GetScreenCount())
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
						defaultInterpolatedStringHandler.AppendLiteral("[Display] FAILED: Display ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay + 1);
						defaultInterpolatedStringHandler.AppendLiteral(" is missing. Fallback to primary.");
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						settingsSave.WindowPosition = new Vector2I(8, 48);
						DisplayServer.WindowSetSize(DisplayServer.ScreenGetSize(DisplayServer.GetPrimaryScreen()) - new Vector2I(8, 48), 0);
						DisplayServer.WindowSetPosition(DisplayServer.ScreenGetPosition(DisplayServer.GetPrimaryScreen()) + settingsSave.WindowPosition, 0);
					}
					else
					{
						Vector2I vector2I4 = DisplayServer.ScreenGetPosition(settingsSave.TargetDisplay);
						if (windowSize.X > vector2I3.X)
						{
							windowSize.X = vector2I3.X;
						}
						if (windowSize.Y > vector2I3.Y)
						{
							windowSize.Y = vector2I3.Y;
						}
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
						defaultInterpolatedStringHandler.AppendLiteral("[Display] SUCCESS: ");
						defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(windowSize);
						defaultInterpolatedStringHandler.AppendLiteral(" Windowed mode in Display ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay);
						defaultInterpolatedStringHandler.AppendLiteral(": Position ");
						defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(vector2I4 + windowPosition);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(windowPosition);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						DisplayServer.WindowSetSize(windowSize, 0);
						DisplayServer.WindowSetPosition(vector2I4 + windowPosition, 0);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
						defaultInterpolatedStringHandler.AppendLiteral("[Display] New size: ");
						defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.WindowGetSize(0));
						defaultInterpolatedStringHandler.AppendLiteral(" position: ");
						defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(DisplayServer.WindowGetPosition(0));
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
			if (flag)
			{
				Log.Info("Manual window change signal because of auto scaling", 2);
				base.EmitSignal(NGame.SignalName.WindowChange, new Variant[] { settingsSave.AspectRatioSetting == AspectRatioSetting.Auto });
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0006750D File Offset: 0x0006570D
		public NInspectRelicScreen GetInspectRelicScreen()
		{
			if (this.InspectRelicScreen == null)
			{
				this.InspectRelicScreen = NInspectRelicScreen.Create();
				this._inspectionContainer.AddChildSafely(this.InspectRelicScreen);
			}
			return this.InspectRelicScreen;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00067539 File Offset: 0x00065739
		public NInspectCardScreen GetInspectCardScreen()
		{
			if (this.InspectCardScreen == null)
			{
				this.InspectCardScreen = NInspectCardScreen.Create();
				this._inspectionContainer.AddChildSafely(this.InspectCardScreen);
			}
			return this.InspectCardScreen;
		}

		/// <summary>
		/// Used to update VSync settings when changed from the options screen.
		/// </summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		// Token: 0x060011D3 RID: 4563 RVA: 0x00067568 File Offset: 0x00065768
		public static void ApplySyncSetting()
		{
			switch (SaveManager.Instance.SettingsSave.VSync)
			{
			case VSyncType.Off:
				Log.Info("VSync: Off", 2);
				DisplayServer.WindowSetVsyncMode(0L, 0);
				return;
			case VSyncType.On:
				Log.Info("VSync: On", 2);
				DisplayServer.WindowSetVsyncMode(1L, 0);
				return;
			case VSyncType.Adaptive:
				Log.Info("VSync: Adaptive", 2);
				DisplayServer.WindowSetVsyncMode(2L, 0);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid VSync type: ");
			defaultInterpolatedStringHandler.AppendFormatted<VSyncType>(SaveManager.Instance.SettingsSave.VSync);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Reset the game instance. Only meant to be called from tests.
		/// </summary>
		// Token: 0x060011D4 RID: 4564 RVA: 0x00067616 File Offset: 0x00065816
		public static void Reset()
		{
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.QueueFreeSafely();
			}
			NGame.Instance = null;
		}

		/// <summary>
		/// Listens for game close request
		/// </summary>
		// Token: 0x060011D5 RID: 4565 RVA: 0x0006762E File Offset: 0x0006582E
		public override void _Notification(int what)
		{
			if ((long)what == 1006L)
			{
				this.Quit();
			}
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00067640 File Offset: 0x00065840
		public void Quit()
		{
			Log.Info("NGame.Quit called", 2);
			if (!PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen() && !SaveManager.Instance.SettingsSave.Fullscreen)
			{
				SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
				settingsSave.WindowSize = DisplayServer.WindowGetSize(0);
				settingsSave.TargetDisplay = DisplayServer.WindowGetCurrentScreen(0);
				settingsSave.WindowPosition = DisplayServer.WindowGetPosition(0) - DisplayServer.ScreenGetPosition(SaveManager.Instance.SettingsSave.TargetDisplay);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[Display] On exit, saving window size: ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(settingsSave.WindowSize);
				defaultInterpolatedStringHandler.AppendLiteral(" display: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(settingsSave.TargetDisplay);
				defaultInterpolatedStringHandler.AppendLiteral(" position: ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(settingsSave.WindowPosition);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			SaveManager.Instance.SaveSettings();
			if (SaveManager.Instance.IsProfileInitialized)
			{
				SaveManager.Instance.SavePrefsFile();
				SaveManager.Instance.SaveProgressFile();
				SaveManager.Instance.SaveProfile();
			}
			SteamInitializer.SteamNoLongerRunning -= this.OnSteamNoLongerRunning;
			MegaLabel.DisposeCachedParagraph();
			MegaRichTextLabel.DisposeCachedParagraph();
			FontManager.ClearCache();
			base.GetTree().Quit(0);
		}

		/// <summary>
		/// Called when the Main Menu is launched for the first time
		/// </summary>
		// Token: 0x060011D7 RID: 4567 RVA: 0x00067788 File Offset: 0x00065988
		private async Task LaunchMainMenu(bool skipLogo)
		{
			NLogoAnimation logoAnimation = null;
			if (!skipLogo)
			{
				await PreloadManager.LoadLogoAnimation();
				logoAnimation = NLogoAnimation.Create();
				this.RootSceneContainer.SetCurrentScene(logoAnimation);
				await PreloadManager.LoadMainMenuEssentials();
			}
			else
			{
				await PreloadManager.LoadMainMenuEssentials();
			}
			if (logoAnimation != null)
			{
				this._logoCancelToken = new CancellationTokenSource();
				await this.Transition.FadeIn(0.8f, "res://materials/transitions/fade_transition_mat.tres", new CancellationToken?(this._logoCancelToken.Token));
				await logoAnimation.PlayAnimation(this._logoCancelToken.Token);
				await this.Transition.FadeOut(0.8f, "res://materials/transitions/fade_transition_mat.tres", null);
			}
			await this.LoadMainMenu(false);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("[Startup] Time to main menu: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(Time.GetTicksMsec(), "N0");
			defaultInterpolatedStringHandler.AppendLiteral("ms");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			NGame.LogResourceStats("main menu loaded (essential)");
			TaskHelper.RunSafely(this.LoadDeferredStartupAssetsAsync());
			SteamJoinCallbackHandler joinCallbackHandler = this._joinCallbackHandler;
			if (joinCallbackHandler != null)
			{
				joinCallbackHandler.CheckForCommandLineJoin();
			}
		}

		/// <summary>
		/// Loads remaining atlases, runs ModelDb.Preload, and loads all common/menu assets in background.
		/// Called after main menu is displayed to not block the user.
		/// </summary>
		// Token: 0x060011D8 RID: 4568 RVA: 0x000677D4 File Offset: 0x000659D4
		private async Task LoadDeferredStartupAssetsAsync()
		{
			OneTimeInitialization.ExecuteDeferred();
			await PreloadManager.LoadCommonAndMainMenuAssets();
			NGame.LogResourceStats("main menu loaded (complete)");
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00067810 File Offset: 0x00065A10
		public async Task GoToTimelineAfterRun()
		{
			await this.GoToTimeline();
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00067854 File Offset: 0x00065A54
		public async Task ReturnToMainMenuAfterRun()
		{
			await this.ReturnToMainMenu();
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00067898 File Offset: 0x00065A98
		public async Task ReturnToMainMenuWithInternalError(Exception e)
		{
			if (this.MainMenu == null)
			{
				await this.ReturnToMainMenu();
			}
			LocString locString = new LocString("main_menu_ui", "INTERNAL_ERROR.description");
			locString.Add("info", e.GetType().Name + ": " + e.Message);
			NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "INTERNAL_ERROR.title"), locString, null, true);
			NModalContainer.Instance.Add(nerrorPopup, true);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x000678E4 File Offset: 0x00065AE4
		public async Task GoToTimeline()
		{
			await this.Transition.FadeOut(0.8f, "res://materials/transitions/fade_transition_mat.tres", null);
			await PreloadManager.LoadCommonAndMainMenuAssets();
			RunManager.Instance.CleanUp(true);
			await this.LoadMainMenu(true);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00067928 File Offset: 0x00065B28
		public async Task ReturnToMainMenu()
		{
			await this.Transition.FadeOut(0.8f, "res://materials/transitions/fade_transition_mat.tres", null);
			await PreloadManager.LoadCommonAndMainMenuAssets();
			RunManager.Instance.CleanUp(true);
			await this.LoadMainMenu(false);
		}

		/// <summary>
		/// Almost nothing changes text when the language is changed. This is called when the language is changed to
		/// reload all text.
		/// </summary>
		// Token: 0x060011DE RID: 4574 RVA: 0x0006796B File Offset: 0x00065B6B
		public void Relocalize()
		{
			if (this.RootSceneContainer.CurrentScene is NMainMenu)
			{
				this.ReloadMainMenu();
			}
			NSendFeedbackScreen feedbackScreen = this.FeedbackScreen;
			if (feedbackScreen != null)
			{
				feedbackScreen.Relocalize();
			}
			this.TimeoutOverlay.Relocalize();
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000679A1 File Offset: 0x00065BA1
		public void ReloadMainMenu()
		{
			if (this.MainMenu == null)
			{
				throw new InvalidOperationException("Tried to reload main menu when not already on the main menu!");
			}
			TaskHelper.RunSafely(this.LoadMainMenu(false));
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000679C4 File Offset: 0x00065BC4
		private async Task LoadMainMenu(bool openTimeline = false)
		{
			Task currentRunSaveTask = SaveManager.Instance.CurrentRunSaveTask;
			if (currentRunSaveTask != null)
			{
				Log.Info("Saving in progress, waiting for it to be finished before loading the main menu", 2);
				try
				{
					await currentRunSaveTask;
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Save task failed while waiting to load main menu: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			NMainMenu nmainMenu = NMainMenu.Create(openTimeline);
			this.RootSceneContainer.SetCurrentScene(nmainMenu);
		}

		/// <summary>
		/// Start a new singleplayer run on both the front and back ends.
		/// </summary>
		/// <param name="character">The character that the player is playing as.</param>
		/// <param name="shouldSave">Whether a save file should be created for the run.</param>
		/// <param name="acts">The canonical acts that should be in the run.</param>
		/// <param name="modifiers">The modifiers that are applied to the run.</param>
		/// <param name="seed">The seed that the run's RNG should use.</param>
		/// <param name="gameMode">The type of game mode that we're starting.</param>
		/// <param name="ascensionLevel">The ascension level that the run should be played at.</param>
		/// <param name="dailyTime">Time used to determine the daily run info. Null if not a daily run.</param>
		// Token: 0x060011E1 RID: 4577 RVA: 0x00067A10 File Offset: 0x00065C10
		public async Task<RunState> StartNewSingleplayerRun(CharacterModel character, bool shouldSave, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers, string seed, GameMode gameMode, int ascensionLevel = 0, DateTimeOffset? dailyTime = null)
		{
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			RunState runState = RunState.CreateForNewRun(new <>z__ReadOnlySingleElementList<Player>(Player.CreateForNewRun(character, unlockState, 1UL)), acts.Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), modifiers, gameMode, ascensionLevel, seed);
			RunManager.Instance.SetUpNewSingleplayer(runState, shouldSave, dailyTime);
			await this.StartRun(runState);
			return runState;
		}

		/// <summary>
		/// Start a new multiplayer run on both the front and back ends.
		/// </summary>
		/// <param name="lobby">The lobby containing the multiplayer info.</param>
		/// <param name="shouldSave">Whether a save file should be created for the run.</param>
		/// <param name="acts">The canonical acts that should be in the run.</param>
		/// <param name="modifiers">The modifiers that are applied to the run.</param>
		/// <param name="seed">The seed that the run's RNG should use.</param>
		/// <param name="ascensionLevel">The ascension level that the run should be played at.</param>
		/// <param name="dailyTime">Time used to determine the daily run info. Null if not a daily run.</param>
		// Token: 0x060011E2 RID: 4578 RVA: 0x00067A98 File Offset: 0x00065C98
		public async Task<RunState> StartNewMultiplayerRun(StartRunLobby lobby, bool shouldSave, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers, string seed, int ascensionLevel, DateTimeOffset? dailyTime = null)
		{
			RunState runState = RunState.CreateForNewRun(lobby.Players.Select<LobbyPlayer, Player>((LobbyPlayer p) => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id)).ToList<Player>(), acts.Select<ActModel, ActModel>((ActModel a) => a.ToMutable()).ToList<ActModel>(), modifiers, lobby.GameMode, ascensionLevel, seed);
			RunManager.Instance.SetUpNewMultiplayer(runState, lobby, shouldSave, dailyTime);
			await this.StartRun(runState);
			return runState;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00067B18 File Offset: 0x00065D18
		public async Task LoadRun(RunState runState, [Nullable(2)] SerializableRoom preFinishedRoom)
		{
			await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Player p) => p.Character));
			await PreloadManager.LoadActAssets(runState.Act);
			RunManager.Instance.Launch();
			this.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
			await RunManager.Instance.GenerateMap();
			await RunManager.Instance.LoadIntoLatestMapCoord(AbstractRoom.FromSerializable(preFinishedRoom, runState));
			if (RunManager.Instance.MapDrawingsToLoad != null)
			{
				NRun.Instance.GlobalUi.MapScreen.Drawings.LoadDrawings(RunManager.Instance.MapDrawingsToLoad);
				RunManager.Instance.MapDrawingsToLoad = null;
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00067B6C File Offset: 0x00065D6C
		private async Task StartRun(RunState runState)
		{
			using (new NetLoadingHandle(RunManager.Instance.NetService))
			{
				await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Player p) => p.Character));
				await PreloadManager.LoadActAssets(runState.Acts[0]);
				await RunManager.Instance.FinalizeStartingRelics();
				RunManager.Instance.Launch();
				this.RootSceneContainer.SetCurrentScene(NRun.Create(runState));
				await RunManager.Instance.EnterAct(0, false);
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00067BB8 File Offset: 0x00065DB8
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.speedUp, false))
			{
				this.DebugModifyTimescale(0.1);
			}
			else if (inputEvent.IsActionReleased(DebugHotkey.speedDown, false))
			{
				this.DebugModifyTimescale(-0.1);
			}
			else if (inputEvent.IsActionReleased(DebugHotkey.hideProceedButton, false))
			{
				NGame.IsDebugHidingProceedButton = !NGame.IsDebugHidingProceedButton;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NGame.IsDebugHidingProceedButton ? "Hide Proceed Button" : "Show Proceed Button"));
				Action debugToggleProceedButton = this.DebugToggleProceedButton;
				if (debugToggleProceedButton != null)
				{
					debugToggleProceedButton();
				}
			}
			else if (inputEvent.IsActionReleased(DebugHotkey.hideHoverTips, false))
			{
				NGame.IsDebugHidingHoverTips = !NGame.IsDebugHidingHoverTips;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NGame.IsDebugHidingHoverTips ? "Hide HoverTips" : "Show HoverTips"));
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if ((inputEventMouseButton != null && inputEventMouseButton.Pressed) || inputEvent.IsActionPressed(MegaInput.select, false, false) || inputEvent.IsActionPressed(MegaInput.cancel, false, false))
			{
				CancellationTokenSource logoCancelToken = this._logoCancelToken;
				if (logoCancelToken != null)
				{
					logoCancelToken.Cancel();
				}
			}
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null)
			{
				if (OS.GetName().Contains("Windows"))
				{
					if (inputEventKey.Pressed && inputEventKey.AltPressed && inputEventKey.Keycode == 4194309L)
					{
						this.ToggleFullscreen();
						return;
					}
				}
				else if (OS.GetName().Contains("macOS") && inputEventKey.Pressed && inputEventKey.CtrlPressed && inputEventKey.MetaPressed && inputEventKey.Keycode == 70L)
				{
					this.ToggleFullscreen();
				}
			}
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00067D51 File Offset: 0x00065F51
		private void ToggleFullscreen()
		{
			Log.Info("Used FULLSCREEN shortcut", 2);
			NFullscreenTickbox.SetFullscreen(!SaveManager.Instance.SettingsSave.Fullscreen);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00067D78 File Offset: 0x00065F78
		private void DebugModifyTimescale(double offset)
		{
			double num = Math.Round(Engine.TimeScale + offset, 1);
			num = Math.Clamp(num, 0.1, 4.0);
			Engine.TimeScale = num;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
			defaultInterpolatedStringHandler.AppendLiteral("TimeScale:");
			defaultInterpolatedStringHandler.AppendFormatted<double>(Engine.TimeScale);
			this.AddChildSafely(NFullscreenTextVfx.Create(defaultInterpolatedStringHandler.ToStringAndClear()));
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00067DE6 File Offset: 0x00065FE6
		public WorldEnvironment ActivateWorldEnvironment()
		{
			this.AddChildSafely(this.WorldEnvironment);
			return this.WorldEnvironment;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00067DFA File Offset: 0x00065FFA
		public void DeactivateWorldEnvironment()
		{
			this.RemoveChildSafely(this.WorldEnvironment);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00067E08 File Offset: 0x00066008
		public void SetScreenShakeTarget(Control target)
		{
			this._screenShake.SetTarget(target);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00067E16 File Offset: 0x00066016
		public void ClearScreenShakeTarget()
		{
			this._screenShake.ClearTarget();
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00067E23 File Offset: 0x00066023
		public void ScreenShake(ShakeStrength strength, ShakeDuration duration, float degAngle = -1f)
		{
			if (degAngle < 0f)
			{
				degAngle = Rng.Chaotic.NextFloat(360f);
			}
			this._screenShake.Shake(strength, duration, degAngle);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00067E4C File Offset: 0x0006604C
		public void ScreenRumble(ShakeStrength strength, ShakeDuration duration, RumbleStyle style)
		{
			this._screenShake.Rumble(strength, duration, style);
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00067E5C File Offset: 0x0006605C
		public void ScreenShakeTrauma(ShakeStrength strength)
		{
			this._screenShake.AddTrauma(strength);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00067E6A File Offset: 0x0006606A
		public void DoHitStop(ShakeStrength strength, ShakeDuration duration)
		{
			this.HitStop.DoHitStop(strength, duration);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00067E79 File Offset: 0x00066079
		public static void ToggleTrailerMode()
		{
			NGame.IsTrailerMode = !NGame.IsTrailerMode;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00067E88 File Offset: 0x00066088
		public void SetScreenshakeMultiplier(float multiplier)
		{
			this._screenShake.SetMultiplier(multiplier);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00067E96 File Offset: 0x00066096
		private void InitPools()
		{
			NCard.InitPool();
			NGridCardHolder.InitPool();
		}

		/// <summary>
		/// Called when we detect that a mod is installed during runtime. Displays a warning telling the user that the mod
		/// won't be loaded until after a restart.
		/// </summary>
		// Token: 0x060011F3 RID: 4595 RVA: 0x00067EA4 File Offset: 0x000660A4
		private void OnNewModDetected(Mod mod)
		{
			if (!NModalContainer.Instance.GetChildren(false).OfType<NErrorPopup>().Any<NErrorPopup>())
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "MOD_NOT_LOADED_POPUP.title"), new LocString("main_menu_ui", "MOD_NOT_LOADED_POPUP.description"), null, false);
				NModalContainer.Instance.Add(nerrorPopup, true);
			}
		}

		/// <summary>
		/// If either the settings or the progress save failed to load, then this displays an error popup notifying the player
		/// and asking them to send logs. Does nothing otherwise.
		/// </summary>
		// Token: 0x060011F4 RID: 4596 RVA: 0x00067EFC File Offset: 0x000660FC
		public void CheckShowSaveFileError(ReadSaveResult<SerializableProgress> progressReadResult, ReadSaveResult<PrefsSave> prefsReadResult, [Nullable(new byte[] { 2, 1 })] ReadSaveResult<SettingsSave> settingsReadResult)
		{
			LocString locString = null;
			if (!progressReadResult.Success && progressReadResult.Status != ReadSaveStatus.FileNotFound)
			{
				locString = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_progress");
			}
			else if (settingsReadResult != null && !settingsReadResult.Success && settingsReadResult.Status != ReadSaveStatus.FileNotFound)
			{
				locString = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_settings");
			}
			else if (!prefsReadResult.Success && prefsReadResult.Status != ReadSaveStatus.FileNotFound)
			{
				locString = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_settings");
			}
			if (locString != null)
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), locString, new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true);
				NModalContainer.Instance.Add(nerrorPopup, true);
			}
		}

		/// <summary>
		/// If localization validation errors were found in user override files, displays a popup notifying the user.
		/// Shows summary of errors (count and file paths) and directs user to check console logs for details.
		/// </summary>
		// Token: 0x060011F5 RID: 4597 RVA: 0x00067FAC File Offset: 0x000661AC
		private void CheckShowLocalizationOverrideErrors()
		{
			if (LocManager.Instance.ValidationErrors.Count == 0)
			{
				return;
			}
			List<IGrouping<string, LocValidationError>> list = (from e in LocManager.Instance.ValidationErrors
				group e by e.FilePath).ToList<IGrouping<string, LocValidationError>>();
			string text = string.Join("\n", list.Take<IGrouping<string, LocValidationError>>(5).Select<IGrouping<string, LocValidationError>, string>(delegate(IGrouping<string, LocValidationError> g)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler2.AppendFormatted(Path.GetFileName(g.Key));
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(g.Count<LocValidationError>());
				defaultInterpolatedStringHandler2.AppendLiteral(" errors)");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}));
			if (list.Count > 5)
			{
				string text2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler.AppendLiteral("\n... and ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count - 5);
				defaultInterpolatedStringHandler.AppendLiteral(" more files");
				text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string text3 = "Errors found in the following localization override files:\n\n" + text + "\n\n[gold]Check the console logs for detailed error messages.[/gold]\n\nTo fix: Remove or correct invalid override files in your localization_override folder.";
			NErrorPopup nerrorPopup = NErrorPopup.Create("Localization Override Errors", text3, false);
			NModalContainer.Instance.Add(nerrorPopup, true);
		}

		/// <summary>
		/// If the player launched with mods for the first time, then display a dialog saying their saves were copied.
		/// </summary>
		// Token: 0x060011F6 RID: 4598 RVA: 0x000680A8 File Offset: 0x000662A8
		private void CheckShowModdedSaveFilePopup()
		{
			if (!ModManager.UnmoddedSavesWereCopied)
			{
				return;
			}
			NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "MODDED_SAVE_COPY_POPUP.title"), new LocString("main_menu_ui", "MODDED_SAVE_COPY_POPUP.body"), null, false);
			NModalContainer.Instance.Add(nerrorPopup, true);
		}

		/// <summary>
		/// Contains platform-specific initialization. This must complete before the game can be allowed to proceed.
		/// </summary>
		// Token: 0x060011F7 RID: 4599 RVA: 0x000680F0 File Offset: 0x000662F0
		private async Task<bool> InitializePlatform()
		{
			bool flag = CommandLineHelper.HasArg("force-steam");
			string text = CommandLineHelper.GetValue("force-steam") ?? "";
			bool flag2;
			if (!text.Equals("on", StringComparison.OrdinalIgnoreCase) && (!flag || !(text == string.Empty)) && (text.Equals("off", StringComparison.OrdinalIgnoreCase) || OS.HasFeature("editor")))
			{
				Log.Info("Steam initialization skipped (editor mode). Use --force-steam to enable.", 2);
				flag2 = true;
			}
			else
			{
				bool steamInitialized = SteamInitializer.Initialize(this);
				if (!steamInitialized)
				{
					Log.Error("Failed to initialize Steam! Attempting to show error popup", 2);
					await this.TryErrorInit();
					NGenericPopup ngenericPopup = NGenericPopup.Create();
					if (ngenericPopup == null || NModalContainer.Instance == null)
					{
						Log.Error("Cannot show Steam error dialog: UI not initialized. Quitting immediately.", 2);
						base.GetTree().Quit(0);
						return false;
					}
					NModalContainer.Instance.Add(ngenericPopup, true);
					LocString locString = new LocString("main_menu_ui", "STEAM_INIT_ERROR.description");
					locString.Add("details", SteamInitializer.InitErrorMessage ?? "<null>");
					await ngenericPopup.WaitForConfirmation(locString, new LocString("main_menu_ui", "STEAM_INIT_ERROR.title"), null, new LocString("main_menu_ui", "QUIT"));
					base.GetTree().Quit(0);
				}
				else
				{
					this._joinCallbackHandler = new SteamJoinCallbackHandler();
					SteamInitializer.SteamNoLongerRunning += this.OnSteamNoLongerRunning;
				}
				flag2 = steamInitialized;
			}
			return flag2;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00068134 File Offset: 0x00066334
		private void OnSteamNoLongerRunning()
		{
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer instance = NModalContainer.Instance;
			if (instance != null)
			{
				instance.Add(ngenericPopup, true);
			}
			LocString locString = new LocString("main_menu_ui", "STEAM_STOPPED_ERROR.description");
			LocString locString2 = locString;
			string text = "details";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<ESteamAPIInitResult?>(SteamInitializer.InitResult);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(SteamInitializer.InitErrorMessage);
			locString2.Add(text, defaultInterpolatedStringHandler.ToStringAndClear());
			TaskHelper.RunSafely(ngenericPopup.WaitForConfirmation(locString, new LocString("main_menu_ui", "STEAM_STOPPED_ERROR.title"), null, new LocString("main_menu_ui", "GENERIC_POPUP.ok")));
			SteamInitializer.SteamNoLongerRunning -= this.OnSteamNoLongerRunning;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000681E8 File Offset: 0x000663E8
		public static void LogResourceStats(string context)
		{
			ulong staticMemoryUsage = OS.GetStaticMemoryUsage();
			ulong renderingInfo = RenderingServer.GetRenderingInfo(5L);
			int num = (int)Performance.GetMonitor(7L);
			int num2 = (int)Performance.GetMonitor(8L);
			int num3 = (int)Performance.GetMonitor(9L);
			int num4 = PreloadManager.Cache.GetCacheKeys().Count<string>();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(91, 7);
			defaultInterpolatedStringHandler.AppendLiteral("[Startup] Resource stats (");
			defaultInterpolatedStringHandler.AppendFormatted(context);
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendLiteral("StaticMem=");
			defaultInterpolatedStringHandler.AppendFormatted(NGame.FormatBytes(staticMemoryUsage));
			defaultInterpolatedStringHandler.AppendLiteral(", VRAM=");
			defaultInterpolatedStringHandler.AppendFormatted(NGame.FormatBytes(renderingInfo));
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendLiteral("Objects=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num, "N0");
			defaultInterpolatedStringHandler.AppendLiteral(", Resources=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2, "N0");
			defaultInterpolatedStringHandler.AppendLiteral(", Nodes=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num3, "N0");
			defaultInterpolatedStringHandler.AppendLiteral(", CachedAssets=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num4, "N0");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0006830C File Offset: 0x0006650C
		internal static string FormatBytes(ulong bytes)
		{
			string[] array = new string[] { "B", "KB", "MB", "GB" };
			int num = 0;
			double num2 = bytes;
			while (num2 >= 1024.0 && num < array.Length - 1)
			{
				num2 /= 1024.0;
				num++;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted<double>(num2, "0.#");
			defaultInterpolatedStringHandler.AppendFormatted(array[num]);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00068393 File Offset: 0x00066593
		public static bool IsGameFocusedWindow()
		{
			return !PlatformUtil.IsPlatformOverlayOpen() && DisplayServer.WindowIsFocused(0);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x000683A4 File Offset: 0x000665A4
		public NSendFeedbackScreen GetOrCreateFeedbackScreen()
		{
			if (this.FeedbackScreen == null)
			{
				this.FeedbackScreen = NSendFeedbackScreen.Create();
				this.AddChildSafely(this.FeedbackScreen);
				this.MoveChildSafely(this.FeedbackScreen, this.Transition.GetIndex(false) - 1);
			}
			return this.FeedbackScreen;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x000683F0 File Offset: 0x000665F0
		public static string GetGameVersion()
		{
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			string text;
			if ((text = ((releaseInfo != null) ? releaseInfo.Version : null)) == null)
			{
				text = GitHelper.ShortCommitId ?? "UNKNOWN";
			}
			return text;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060011FE RID: 4606 RVA: 0x0006841C File Offset: 0x0006661C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(38)
			{
				new MethodInfo(NGame.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.IsMainThread, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.IsReleaseGame, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName.InitializeGraphicsPreferences, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.ApplyDisplaySettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.GetInspectRelicScreen, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NGame.MethodName.GetInspectCardScreen, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NGame.MethodName.ApplySyncSetting, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName.Reset, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.Quit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.Relocalize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.ReloadMainMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NGame.MethodName.ToggleFullscreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.DebugModifyTimescale, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "offset", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.ActivateWorldEnvironment, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("WorldEnvironment"), false), 1L, null, null),
				new MethodInfo(NGame.MethodName.DeactivateWorldEnvironment, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.SetScreenShakeTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "target", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGame.MethodName.ClearScreenShakeTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.ScreenShake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false),
					new PropertyInfo(3L, "degAngle", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.ScreenRumble, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false),
					new PropertyInfo(2L, "style", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.ScreenShakeTrauma, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.DoHitStop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.ToggleTrailerMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName.SetScreenshakeMultiplier, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "multiplier", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.InitPools, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.CheckShowLocalizationOverrideErrors, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.CheckShowModdedSaveFilePopup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.OnSteamNoLongerRunning, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.MethodName.LogResourceStats, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "context", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.FormatBytes, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "bytes", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGame.MethodName.IsGameFocusedWindow, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGame.MethodName.GetOrCreateFeedbackScreen, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NGame.MethodName.GetGameVersion, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060011FF RID: 4607 RVA: 0x00068DB4 File Offset: 0x00066FB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGame.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.IsMainThread && args.Count == 0)
			{
				bool flag = NGame.IsMainThread();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NGame.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.IsReleaseGame && args.Count == 0)
			{
				bool flag2 = NGame.IsReleaseGame();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NGame.MethodName.InitializeGraphicsPreferences && args.Count == 0)
			{
				this.InitializeGraphicsPreferences();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ApplyDisplaySettings && args.Count == 0)
			{
				this.ApplyDisplaySettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.GetInspectRelicScreen && args.Count == 0)
			{
				NInspectRelicScreen inspectRelicScreen = this.GetInspectRelicScreen();
				ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref inspectRelicScreen);
				return true;
			}
			if ((ref method) == NGame.MethodName.GetInspectCardScreen && args.Count == 0)
			{
				NInspectCardScreen inspectCardScreen = this.GetInspectCardScreen();
				ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref inspectCardScreen);
				return true;
			}
			if ((ref method) == NGame.MethodName.ApplySyncSetting && args.Count == 0)
			{
				NGame.ApplySyncSetting();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.Reset && args.Count == 0)
			{
				NGame.Reset();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.Quit && args.Count == 0)
			{
				this.Quit();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.Relocalize && args.Count == 0)
			{
				this.Relocalize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ReloadMainMenu && args.Count == 0)
			{
				this.ReloadMainMenu();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ToggleFullscreen && args.Count == 0)
			{
				this.ToggleFullscreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.DebugModifyTimescale && args.Count == 1)
			{
				this.DebugModifyTimescale(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ActivateWorldEnvironment && args.Count == 0)
			{
				WorldEnvironment worldEnvironment = this.ActivateWorldEnvironment();
				ret = VariantUtils.CreateFrom<WorldEnvironment>(ref worldEnvironment);
				return true;
			}
			if ((ref method) == NGame.MethodName.DeactivateWorldEnvironment && args.Count == 0)
			{
				this.DeactivateWorldEnvironment();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.SetScreenShakeTarget && args.Count == 1)
			{
				this.SetScreenShakeTarget(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ClearScreenShakeTarget && args.Count == 0)
			{
				this.ClearScreenShakeTarget();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ScreenShake && args.Count == 3)
			{
				this.ScreenShake(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]), VariantUtils.ConvertTo<float>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ScreenRumble && args.Count == 3)
			{
				this.ScreenRumble(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]), VariantUtils.ConvertTo<RumbleStyle>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ScreenShakeTrauma && args.Count == 1)
			{
				this.ScreenShakeTrauma(VariantUtils.ConvertTo<ShakeStrength>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.DoHitStop && args.Count == 2)
			{
				this.DoHitStop(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ToggleTrailerMode && args.Count == 0)
			{
				NGame.ToggleTrailerMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.SetScreenshakeMultiplier && args.Count == 1)
			{
				this.SetScreenshakeMultiplier(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.InitPools && args.Count == 0)
			{
				this.InitPools();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.CheckShowLocalizationOverrideErrors && args.Count == 0)
			{
				this.CheckShowLocalizationOverrideErrors();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.CheckShowModdedSaveFilePopup && args.Count == 0)
			{
				this.CheckShowModdedSaveFilePopup();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.OnSteamNoLongerRunning && args.Count == 0)
			{
				this.OnSteamNoLongerRunning();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.LogResourceStats && args.Count == 1)
			{
				NGame.LogResourceStats(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.FormatBytes && args.Count == 1)
			{
				string text = NGame.FormatBytes(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NGame.MethodName.IsGameFocusedWindow && args.Count == 0)
			{
				bool flag3 = NGame.IsGameFocusedWindow();
				ret = VariantUtils.CreateFrom<bool>(ref flag3);
				return true;
			}
			if ((ref method) == NGame.MethodName.GetOrCreateFeedbackScreen && args.Count == 0)
			{
				NSendFeedbackScreen orCreateFeedbackScreen = this.GetOrCreateFeedbackScreen();
				ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref orCreateFeedbackScreen);
				return true;
			}
			if ((ref method) == NGame.MethodName.GetGameVersion && args.Count == 0)
			{
				string gameVersion = NGame.GetGameVersion();
				ret = VariantUtils.CreateFrom<string>(ref gameVersion);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00069460 File Offset: 0x00067660
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGame.MethodName.IsMainThread && args.Count == 0)
			{
				bool flag = NGame.IsMainThread();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NGame.MethodName.IsReleaseGame && args.Count == 0)
			{
				bool flag2 = NGame.IsReleaseGame();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NGame.MethodName.ApplySyncSetting && args.Count == 0)
			{
				NGame.ApplySyncSetting();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.Reset && args.Count == 0)
			{
				NGame.Reset();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.ToggleTrailerMode && args.Count == 0)
			{
				NGame.ToggleTrailerMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.LogResourceStats && args.Count == 1)
			{
				NGame.LogResourceStats(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGame.MethodName.FormatBytes && args.Count == 1)
			{
				string text = NGame.FormatBytes(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NGame.MethodName.IsGameFocusedWindow && args.Count == 0)
			{
				bool flag3 = NGame.IsGameFocusedWindow();
				ret = VariantUtils.CreateFrom<bool>(ref flag3);
				return true;
			}
			if ((ref method) == NGame.MethodName.GetGameVersion && args.Count == 0)
			{
				string gameVersion = NGame.GetGameVersion();
				ret = VariantUtils.CreateFrom<string>(ref gameVersion);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001201 RID: 4609 RVA: 0x000695FC File Offset: 0x000677FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGame.MethodName._EnterTree || (ref method) == NGame.MethodName._Ready || (ref method) == NGame.MethodName.OnWindowChange || (ref method) == NGame.MethodName.IsMainThread || (ref method) == NGame.MethodName._ExitTree || (ref method) == NGame.MethodName.IsReleaseGame || (ref method) == NGame.MethodName.InitializeGraphicsPreferences || (ref method) == NGame.MethodName.ApplyDisplaySettings || (ref method) == NGame.MethodName.GetInspectRelicScreen || (ref method) == NGame.MethodName.GetInspectCardScreen || (ref method) == NGame.MethodName.ApplySyncSetting || (ref method) == NGame.MethodName.Reset || (ref method) == NGame.MethodName._Notification || (ref method) == NGame.MethodName.Quit || (ref method) == NGame.MethodName.Relocalize || (ref method) == NGame.MethodName.ReloadMainMenu || (ref method) == NGame.MethodName._Input || (ref method) == NGame.MethodName.ToggleFullscreen || (ref method) == NGame.MethodName.DebugModifyTimescale || (ref method) == NGame.MethodName.ActivateWorldEnvironment || (ref method) == NGame.MethodName.DeactivateWorldEnvironment || (ref method) == NGame.MethodName.SetScreenShakeTarget || (ref method) == NGame.MethodName.ClearScreenShakeTarget || (ref method) == NGame.MethodName.ScreenShake || (ref method) == NGame.MethodName.ScreenRumble || (ref method) == NGame.MethodName.ScreenShakeTrauma || (ref method) == NGame.MethodName.DoHitStop || (ref method) == NGame.MethodName.ToggleTrailerMode || (ref method) == NGame.MethodName.SetScreenshakeMultiplier || (ref method) == NGame.MethodName.InitPools || (ref method) == NGame.MethodName.CheckShowLocalizationOverrideErrors || (ref method) == NGame.MethodName.CheckShowModdedSaveFilePopup || (ref method) == NGame.MethodName.OnSteamNoLongerRunning || (ref method) == NGame.MethodName.LogResourceStats || (ref method) == NGame.MethodName.FormatBytes || (ref method) == NGame.MethodName.IsGameFocusedWindow || (ref method) == NGame.MethodName.GetOrCreateFeedbackScreen || (ref method) == NGame.MethodName.GetGameVersion || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001202 RID: 4610 RVA: 0x0006984C File Offset: 0x00067A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGame.PropertyName.RootSceneContainer)
			{
				this.RootSceneContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HoverTipsContainer)
			{
				this.HoverTipsContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.Transition)
			{
				this.Transition = VariantUtils.ConvertTo<NTransition>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.TimeoutOverlay)
			{
				this.TimeoutOverlay = VariantUtils.ConvertTo<NMultiplayerTimeoutOverlay>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.AudioManager)
			{
				this.AudioManager = VariantUtils.ConvertTo<NAudioManager>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.RemoteCursorContainer)
			{
				this.RemoteCursorContainer = VariantUtils.ConvertTo<NRemoteMouseCursorContainer>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InputManager)
			{
				this.InputManager = VariantUtils.ConvertTo<NInputManager>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HotkeyManager)
			{
				this.HotkeyManager = VariantUtils.ConvertTo<NHotkeyManager>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.FeedbackScreen)
			{
				this.FeedbackScreen = VariantUtils.ConvertTo<NSendFeedbackScreen>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.ReactionWheel)
			{
				this.ReactionWheel = VariantUtils.ConvertTo<NReactionWheel>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.ReactionContainer)
			{
				this.ReactionContainer = VariantUtils.ConvertTo<NReactionContainer>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.CursorManager)
			{
				this.CursorManager = VariantUtils.ConvertTo<NCursorManager>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.DebugAudio)
			{
				this.DebugAudio = VariantUtils.ConvertTo<NDebugAudioManager>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.DebugSeedOverride)
			{
				this.DebugSeedOverride = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.StartOnMainMenu)
			{
				this.StartOnMainMenu = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InspectRelicScreen)
			{
				this.InspectRelicScreen = VariantUtils.ConvertTo<NInspectRelicScreen>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InspectCardScreen)
			{
				this.InspectCardScreen = VariantUtils.ConvertTo<NInspectCardScreen>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.WorldEnvironment)
			{
				this.WorldEnvironment = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HitStop)
			{
				this.HitStop = VariantUtils.ConvertTo<NHitStop>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName._inspectionContainer)
			{
				this._inspectionContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGame.PropertyName._screenShake)
			{
				this._screenShake = VariantUtils.ConvertTo<NScreenShake>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001203 RID: 4611 RVA: 0x00069A98 File Offset: 0x00067C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGame.PropertyName.RootSceneContainer)
			{
				NSceneContainer rootSceneContainer = this.RootSceneContainer;
				value = VariantUtils.CreateFrom<NSceneContainer>(ref rootSceneContainer);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HoverTipsContainer)
			{
				Node hoverTipsContainer = this.HoverTipsContainer;
				value = VariantUtils.CreateFrom<Node>(ref hoverTipsContainer);
				return true;
			}
			if ((ref name) == NGame.PropertyName.MainMenu)
			{
				NMainMenu mainMenu = this.MainMenu;
				value = VariantUtils.CreateFrom<NMainMenu>(ref mainMenu);
				return true;
			}
			if ((ref name) == NGame.PropertyName.CurrentRunNode)
			{
				NRun currentRunNode = this.CurrentRunNode;
				value = VariantUtils.CreateFrom<NRun>(ref currentRunNode);
				return true;
			}
			if ((ref name) == NGame.PropertyName.LogoAnimation)
			{
				NLogoAnimation logoAnimation = this.LogoAnimation;
				value = VariantUtils.CreateFrom<NLogoAnimation>(ref logoAnimation);
				return true;
			}
			if ((ref name) == NGame.PropertyName.Transition)
			{
				NTransition transition = this.Transition;
				value = VariantUtils.CreateFrom<NTransition>(ref transition);
				return true;
			}
			if ((ref name) == NGame.PropertyName.TimeoutOverlay)
			{
				NMultiplayerTimeoutOverlay timeoutOverlay = this.TimeoutOverlay;
				value = VariantUtils.CreateFrom<NMultiplayerTimeoutOverlay>(ref timeoutOverlay);
				return true;
			}
			if ((ref name) == NGame.PropertyName.AudioManager)
			{
				NAudioManager audioManager = this.AudioManager;
				value = VariantUtils.CreateFrom<NAudioManager>(ref audioManager);
				return true;
			}
			if ((ref name) == NGame.PropertyName.RemoteCursorContainer)
			{
				NRemoteMouseCursorContainer remoteCursorContainer = this.RemoteCursorContainer;
				value = VariantUtils.CreateFrom<NRemoteMouseCursorContainer>(ref remoteCursorContainer);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InputManager)
			{
				NInputManager inputManager = this.InputManager;
				value = VariantUtils.CreateFrom<NInputManager>(ref inputManager);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HotkeyManager)
			{
				NHotkeyManager hotkeyManager = this.HotkeyManager;
				value = VariantUtils.CreateFrom<NHotkeyManager>(ref hotkeyManager);
				return true;
			}
			if ((ref name) == NGame.PropertyName.FeedbackScreen)
			{
				NSendFeedbackScreen feedbackScreen = this.FeedbackScreen;
				value = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref feedbackScreen);
				return true;
			}
			if ((ref name) == NGame.PropertyName.ReactionWheel)
			{
				NReactionWheel reactionWheel = this.ReactionWheel;
				value = VariantUtils.CreateFrom<NReactionWheel>(ref reactionWheel);
				return true;
			}
			if ((ref name) == NGame.PropertyName.ReactionContainer)
			{
				NReactionContainer reactionContainer = this.ReactionContainer;
				value = VariantUtils.CreateFrom<NReactionContainer>(ref reactionContainer);
				return true;
			}
			if ((ref name) == NGame.PropertyName.CursorManager)
			{
				NCursorManager cursorManager = this.CursorManager;
				value = VariantUtils.CreateFrom<NCursorManager>(ref cursorManager);
				return true;
			}
			if ((ref name) == NGame.PropertyName.DebugAudio)
			{
				NDebugAudioManager debugAudio = this.DebugAudio;
				value = VariantUtils.CreateFrom<NDebugAudioManager>(ref debugAudio);
				return true;
			}
			if ((ref name) == NGame.PropertyName.DebugSeedOverride)
			{
				string debugSeedOverride = this.DebugSeedOverride;
				value = VariantUtils.CreateFrom<string>(ref debugSeedOverride);
				return true;
			}
			if ((ref name) == NGame.PropertyName.StartOnMainMenu)
			{
				bool startOnMainMenu = this.StartOnMainMenu;
				value = VariantUtils.CreateFrom<bool>(ref startOnMainMenu);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InspectRelicScreen)
			{
				NInspectRelicScreen inspectRelicScreen = this.InspectRelicScreen;
				value = VariantUtils.CreateFrom<NInspectRelicScreen>(ref inspectRelicScreen);
				return true;
			}
			if ((ref name) == NGame.PropertyName.InspectCardScreen)
			{
				NInspectCardScreen inspectCardScreen = this.InspectCardScreen;
				value = VariantUtils.CreateFrom<NInspectCardScreen>(ref inspectCardScreen);
				return true;
			}
			if ((ref name) == NGame.PropertyName.ScreenshakeTarget)
			{
				Control screenshakeTarget = this.ScreenshakeTarget;
				value = VariantUtils.CreateFrom<Control>(ref screenshakeTarget);
				return true;
			}
			if ((ref name) == NGame.PropertyName.WorldEnvironment)
			{
				WorldEnvironment worldEnvironment = this.WorldEnvironment;
				value = VariantUtils.CreateFrom<WorldEnvironment>(ref worldEnvironment);
				return true;
			}
			if ((ref name) == NGame.PropertyName.HitStop)
			{
				NHitStop hitStop = this.HitStop;
				value = VariantUtils.CreateFrom<NHitStop>(ref hitStop);
				return true;
			}
			if ((ref name) == NGame.PropertyName._inspectionContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inspectionContainer);
				return true;
			}
			if ((ref name) == NGame.PropertyName._screenShake)
			{
				value = VariantUtils.CreateFrom<NScreenShake>(ref this._screenShake);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001204 RID: 4612 RVA: 0x00069E28 File Offset: 0x00068028
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGame.PropertyName.RootSceneContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.HoverTipsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.MainMenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.CurrentRunNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.LogoAnimation, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.Transition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.TimeoutOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.AudioManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.RemoteCursorContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.InputManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.HotkeyManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.FeedbackScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.ReactionWheel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.ReactionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.CursorManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.DebugAudio, 0L, "", 4096L, false),
				new PropertyInfo(4L, NGame.PropertyName.DebugSeedOverride, 0L, "", 4096L, false),
				new PropertyInfo(1L, NGame.PropertyName.StartOnMainMenu, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.InspectRelicScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.InspectCardScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.ScreenshakeTarget, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.WorldEnvironment, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName.HitStop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName._inspectionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGame.PropertyName._screenShake, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001205 RID: 4613 RVA: 0x0006A174 File Offset: 0x00068374
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName rootSceneContainer = NGame.PropertyName.RootSceneContainer;
			NSceneContainer rootSceneContainer2 = this.RootSceneContainer;
			info.AddProperty(rootSceneContainer, Variant.From<NSceneContainer>(ref rootSceneContainer2));
			StringName hoverTipsContainer = NGame.PropertyName.HoverTipsContainer;
			Node hoverTipsContainer2 = this.HoverTipsContainer;
			info.AddProperty(hoverTipsContainer, Variant.From<Node>(ref hoverTipsContainer2));
			StringName transition = NGame.PropertyName.Transition;
			NTransition transition2 = this.Transition;
			info.AddProperty(transition, Variant.From<NTransition>(ref transition2));
			StringName timeoutOverlay = NGame.PropertyName.TimeoutOverlay;
			NMultiplayerTimeoutOverlay timeoutOverlay2 = this.TimeoutOverlay;
			info.AddProperty(timeoutOverlay, Variant.From<NMultiplayerTimeoutOverlay>(ref timeoutOverlay2));
			StringName audioManager = NGame.PropertyName.AudioManager;
			NAudioManager audioManager2 = this.AudioManager;
			info.AddProperty(audioManager, Variant.From<NAudioManager>(ref audioManager2));
			StringName remoteCursorContainer = NGame.PropertyName.RemoteCursorContainer;
			NRemoteMouseCursorContainer remoteCursorContainer2 = this.RemoteCursorContainer;
			info.AddProperty(remoteCursorContainer, Variant.From<NRemoteMouseCursorContainer>(ref remoteCursorContainer2));
			StringName inputManager = NGame.PropertyName.InputManager;
			NInputManager inputManager2 = this.InputManager;
			info.AddProperty(inputManager, Variant.From<NInputManager>(ref inputManager2));
			StringName hotkeyManager = NGame.PropertyName.HotkeyManager;
			NHotkeyManager hotkeyManager2 = this.HotkeyManager;
			info.AddProperty(hotkeyManager, Variant.From<NHotkeyManager>(ref hotkeyManager2));
			StringName feedbackScreen = NGame.PropertyName.FeedbackScreen;
			NSendFeedbackScreen feedbackScreen2 = this.FeedbackScreen;
			info.AddProperty(feedbackScreen, Variant.From<NSendFeedbackScreen>(ref feedbackScreen2));
			StringName reactionWheel = NGame.PropertyName.ReactionWheel;
			NReactionWheel reactionWheel2 = this.ReactionWheel;
			info.AddProperty(reactionWheel, Variant.From<NReactionWheel>(ref reactionWheel2));
			StringName reactionContainer = NGame.PropertyName.ReactionContainer;
			NReactionContainer reactionContainer2 = this.ReactionContainer;
			info.AddProperty(reactionContainer, Variant.From<NReactionContainer>(ref reactionContainer2));
			StringName cursorManager = NGame.PropertyName.CursorManager;
			NCursorManager cursorManager2 = this.CursorManager;
			info.AddProperty(cursorManager, Variant.From<NCursorManager>(ref cursorManager2));
			StringName debugAudio = NGame.PropertyName.DebugAudio;
			NDebugAudioManager debugAudio2 = this.DebugAudio;
			info.AddProperty(debugAudio, Variant.From<NDebugAudioManager>(ref debugAudio2));
			StringName debugSeedOverride = NGame.PropertyName.DebugSeedOverride;
			string debugSeedOverride2 = this.DebugSeedOverride;
			info.AddProperty(debugSeedOverride, Variant.From<string>(ref debugSeedOverride2));
			StringName startOnMainMenu = NGame.PropertyName.StartOnMainMenu;
			bool startOnMainMenu2 = this.StartOnMainMenu;
			info.AddProperty(startOnMainMenu, Variant.From<bool>(ref startOnMainMenu2));
			StringName inspectRelicScreen = NGame.PropertyName.InspectRelicScreen;
			NInspectRelicScreen inspectRelicScreen2 = this.InspectRelicScreen;
			info.AddProperty(inspectRelicScreen, Variant.From<NInspectRelicScreen>(ref inspectRelicScreen2));
			StringName inspectCardScreen = NGame.PropertyName.InspectCardScreen;
			NInspectCardScreen inspectCardScreen2 = this.InspectCardScreen;
			info.AddProperty(inspectCardScreen, Variant.From<NInspectCardScreen>(ref inspectCardScreen2));
			StringName worldEnvironment = NGame.PropertyName.WorldEnvironment;
			WorldEnvironment worldEnvironment2 = this.WorldEnvironment;
			info.AddProperty(worldEnvironment, Variant.From<WorldEnvironment>(ref worldEnvironment2));
			StringName hitStop = NGame.PropertyName.HitStop;
			NHitStop hitStop2 = this.HitStop;
			info.AddProperty(hitStop, Variant.From<NHitStop>(ref hitStop2));
			info.AddProperty(NGame.PropertyName._inspectionContainer, Variant.From<Control>(ref this._inspectionContainer));
			info.AddProperty(NGame.PropertyName._screenShake, Variant.From<NScreenShake>(ref this._screenShake));
			info.AddSignalEventDelegate(NGame.SignalName.WindowChange, this.backing_WindowChange);
			info.AddSignalEventDelegate(NGame.SignalName.PhobiaModeToggled, this.backing_PhobiaModeToggled);
		}

		/// <inheritdoc />
		// Token: 0x06001206 RID: 4614 RVA: 0x0006A3C0 File Offset: 0x000685C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGame.PropertyName.RootSceneContainer, ref variant))
			{
				this.RootSceneContainer = variant.As<NSceneContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGame.PropertyName.HoverTipsContainer, ref variant2))
			{
				this.HoverTipsContainer = variant2.As<Node>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGame.PropertyName.Transition, ref variant3))
			{
				this.Transition = variant3.As<NTransition>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGame.PropertyName.TimeoutOverlay, ref variant4))
			{
				this.TimeoutOverlay = variant4.As<NMultiplayerTimeoutOverlay>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGame.PropertyName.AudioManager, ref variant5))
			{
				this.AudioManager = variant5.As<NAudioManager>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGame.PropertyName.RemoteCursorContainer, ref variant6))
			{
				this.RemoteCursorContainer = variant6.As<NRemoteMouseCursorContainer>();
			}
			Variant variant7;
			if (info.TryGetProperty(NGame.PropertyName.InputManager, ref variant7))
			{
				this.InputManager = variant7.As<NInputManager>();
			}
			Variant variant8;
			if (info.TryGetProperty(NGame.PropertyName.HotkeyManager, ref variant8))
			{
				this.HotkeyManager = variant8.As<NHotkeyManager>();
			}
			Variant variant9;
			if (info.TryGetProperty(NGame.PropertyName.FeedbackScreen, ref variant9))
			{
				this.FeedbackScreen = variant9.As<NSendFeedbackScreen>();
			}
			Variant variant10;
			if (info.TryGetProperty(NGame.PropertyName.ReactionWheel, ref variant10))
			{
				this.ReactionWheel = variant10.As<NReactionWheel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NGame.PropertyName.ReactionContainer, ref variant11))
			{
				this.ReactionContainer = variant11.As<NReactionContainer>();
			}
			Variant variant12;
			if (info.TryGetProperty(NGame.PropertyName.CursorManager, ref variant12))
			{
				this.CursorManager = variant12.As<NCursorManager>();
			}
			Variant variant13;
			if (info.TryGetProperty(NGame.PropertyName.DebugAudio, ref variant13))
			{
				this.DebugAudio = variant13.As<NDebugAudioManager>();
			}
			Variant variant14;
			if (info.TryGetProperty(NGame.PropertyName.DebugSeedOverride, ref variant14))
			{
				this.DebugSeedOverride = variant14.As<string>();
			}
			Variant variant15;
			if (info.TryGetProperty(NGame.PropertyName.StartOnMainMenu, ref variant15))
			{
				this.StartOnMainMenu = variant15.As<bool>();
			}
			Variant variant16;
			if (info.TryGetProperty(NGame.PropertyName.InspectRelicScreen, ref variant16))
			{
				this.InspectRelicScreen = variant16.As<NInspectRelicScreen>();
			}
			Variant variant17;
			if (info.TryGetProperty(NGame.PropertyName.InspectCardScreen, ref variant17))
			{
				this.InspectCardScreen = variant17.As<NInspectCardScreen>();
			}
			Variant variant18;
			if (info.TryGetProperty(NGame.PropertyName.WorldEnvironment, ref variant18))
			{
				this.WorldEnvironment = variant18.As<WorldEnvironment>();
			}
			Variant variant19;
			if (info.TryGetProperty(NGame.PropertyName.HitStop, ref variant19))
			{
				this.HitStop = variant19.As<NHitStop>();
			}
			Variant variant20;
			if (info.TryGetProperty(NGame.PropertyName._inspectionContainer, ref variant20))
			{
				this._inspectionContainer = variant20.As<Control>();
			}
			Variant variant21;
			if (info.TryGetProperty(NGame.PropertyName._screenShake, ref variant21))
			{
				this._screenShake = variant21.As<NScreenShake>();
			}
			NGame.WindowChangeEventHandler windowChangeEventHandler;
			if (info.TryGetSignalEventDelegate<NGame.WindowChangeEventHandler>(NGame.SignalName.WindowChange, ref windowChangeEventHandler))
			{
				this.backing_WindowChange = windowChangeEventHandler;
			}
			NGame.PhobiaModeToggledEventHandler phobiaModeToggledEventHandler;
			if (info.TryGetSignalEventDelegate<NGame.PhobiaModeToggledEventHandler>(NGame.SignalName.PhobiaModeToggled, ref phobiaModeToggledEventHandler))
			{
				this.backing_PhobiaModeToggled = phobiaModeToggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001207 RID: 4615 RVA: 0x0006A650 File Offset: 0x00068850
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NGame.SignalName.WindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGame.SignalName.PhobiaModeToggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.NGame.WindowChangeEventHandler" />
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001208 RID: 4616 RVA: 0x0006A6C3 File Offset: 0x000688C3
		// (remove) Token: 0x06001209 RID: 4617 RVA: 0x0006A6DC File Offset: 0x000688DC
		[Nullable(0)]
		public event NGame.WindowChangeEventHandler WindowChange
		{
			[NullableContext(0)]
			add
			{
				this.backing_WindowChange = (NGame.WindowChangeEventHandler)Delegate.Combine(this.backing_WindowChange, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_WindowChange = (NGame.WindowChangeEventHandler)Delegate.Remove(this.backing_WindowChange, value);
			}
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0006A6F5 File Offset: 0x000688F5
		protected void EmitSignalWindowChange()
		{
			base.EmitSignal(NGame.SignalName.WindowChange, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.NGame.PhobiaModeToggledEventHandler" />
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600120B RID: 4619 RVA: 0x0006A708 File Offset: 0x00068908
		// (remove) Token: 0x0600120C RID: 4620 RVA: 0x0006A721 File Offset: 0x00068921
		[Nullable(0)]
		public event NGame.PhobiaModeToggledEventHandler PhobiaModeToggled
		{
			[NullableContext(0)]
			add
			{
				this.backing_PhobiaModeToggled = (NGame.PhobiaModeToggledEventHandler)Delegate.Combine(this.backing_PhobiaModeToggled, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_PhobiaModeToggled = (NGame.PhobiaModeToggledEventHandler)Delegate.Remove(this.backing_PhobiaModeToggled, value);
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0006A73A File Offset: 0x0006893A
		protected void EmitSignalPhobiaModeToggled()
		{
			base.EmitSignal(NGame.SignalName.PhobiaModeToggled, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x0600120E RID: 4622 RVA: 0x0006A750 File Offset: 0x00068950
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NGame.SignalName.WindowChange && args.Count == 0)
			{
				NGame.WindowChangeEventHandler windowChangeEventHandler = this.backing_WindowChange;
				if (windowChangeEventHandler == null)
				{
					return;
				}
				windowChangeEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NGame.SignalName.PhobiaModeToggled) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NGame.PhobiaModeToggledEventHandler phobiaModeToggledEventHandler = this.backing_PhobiaModeToggled;
				if (phobiaModeToggledEventHandler == null)
				{
					return;
				}
				phobiaModeToggledEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x0600120F RID: 4623 RVA: 0x0006A7B3 File Offset: 0x000689B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NGame.SignalName.WindowChange || (ref signal) == NGame.SignalName.PhobiaModeToggled || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000640 RID: 1600
		public static readonly Vector2 devResolution = new Vector2(1920f, 1080f);

		// Token: 0x04000651 RID: 1617
		private readonly TaskCompletionSource _gameStartupComplete = new TaskCompletionSource();

		// Token: 0x0400065A RID: 1626
		private Control _inspectionContainer;

		// Token: 0x0400065B RID: 1627
		private NScreenShake _screenShake;

		// Token: 0x0400065C RID: 1628
		private static int? _mainThreadId;

		// Token: 0x0400065D RID: 1629
		private static Window _window = null;

		// Token: 0x0400065E RID: 1630
		[Nullable(2)]
		private CancellationTokenSource _logoCancelToken;

		// Token: 0x0400065F RID: 1631
		[Nullable(2)]
		private SteamJoinCallbackHandler _joinCallbackHandler;

		// Token: 0x04000660 RID: 1632
		[Nullable(0)]
		private NGame.WindowChangeEventHandler backing_WindowChange;

		// Token: 0x04000661 RID: 1633
		[Nullable(0)]
		private NGame.PhobiaModeToggledEventHandler backing_PhobiaModeToggled;

		/// <summary>
		/// Signal used to force other Scenes to refresh their UI when the
		/// application Window is updated via updated Settings or ctrl-F or something.
		/// </summary>
		// Token: 0x02000F5E RID: 3934
		// (Invoke) Token: 0x060097BA RID: 38842
		[NullableContext(0)]
		[Signal]
		public delegate void WindowChangeEventHandler();

		// Token: 0x02000F5F RID: 3935
		// (Invoke) Token: 0x060097BE RID: 38846
		[NullableContext(0)]
		[Signal]
		public delegate void PhobiaModeToggledEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F60 RID: 3936
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04003D0C RID: 15628
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003D0D RID: 15629
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04003D0E RID: 15630
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'IsMainThread' method.
			/// </summary>
			// Token: 0x04003D0F RID: 15631
			public static readonly StringName IsMainThread = "IsMainThread";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003D10 RID: 15632
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'IsReleaseGame' method.
			/// </summary>
			// Token: 0x04003D11 RID: 15633
			public static readonly StringName IsReleaseGame = "IsReleaseGame";

			/// <summary>
			/// Cached name for the 'InitializeGraphicsPreferences' method.
			/// </summary>
			// Token: 0x04003D12 RID: 15634
			public static readonly StringName InitializeGraphicsPreferences = "InitializeGraphicsPreferences";

			/// <summary>
			/// Cached name for the 'ApplyDisplaySettings' method.
			/// </summary>
			// Token: 0x04003D13 RID: 15635
			public static readonly StringName ApplyDisplaySettings = "ApplyDisplaySettings";

			/// <summary>
			/// Cached name for the 'GetInspectRelicScreen' method.
			/// </summary>
			// Token: 0x04003D14 RID: 15636
			public static readonly StringName GetInspectRelicScreen = "GetInspectRelicScreen";

			/// <summary>
			/// Cached name for the 'GetInspectCardScreen' method.
			/// </summary>
			// Token: 0x04003D15 RID: 15637
			public static readonly StringName GetInspectCardScreen = "GetInspectCardScreen";

			/// <summary>
			/// Cached name for the 'ApplySyncSetting' method.
			/// </summary>
			// Token: 0x04003D16 RID: 15638
			public static readonly StringName ApplySyncSetting = "ApplySyncSetting";

			/// <summary>
			/// Cached name for the 'Reset' method.
			/// </summary>
			// Token: 0x04003D17 RID: 15639
			public static readonly StringName Reset = "Reset";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04003D18 RID: 15640
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'Quit' method.
			/// </summary>
			// Token: 0x04003D19 RID: 15641
			public static readonly StringName Quit = "Quit";

			/// <summary>
			/// Cached name for the 'Relocalize' method.
			/// </summary>
			// Token: 0x04003D1A RID: 15642
			public static readonly StringName Relocalize = "Relocalize";

			/// <summary>
			/// Cached name for the 'ReloadMainMenu' method.
			/// </summary>
			// Token: 0x04003D1B RID: 15643
			public static readonly StringName ReloadMainMenu = "ReloadMainMenu";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04003D1C RID: 15644
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ToggleFullscreen' method.
			/// </summary>
			// Token: 0x04003D1D RID: 15645
			public static readonly StringName ToggleFullscreen = "ToggleFullscreen";

			/// <summary>
			/// Cached name for the 'DebugModifyTimescale' method.
			/// </summary>
			// Token: 0x04003D1E RID: 15646
			public static readonly StringName DebugModifyTimescale = "DebugModifyTimescale";

			/// <summary>
			/// Cached name for the 'ActivateWorldEnvironment' method.
			/// </summary>
			// Token: 0x04003D1F RID: 15647
			public static readonly StringName ActivateWorldEnvironment = "ActivateWorldEnvironment";

			/// <summary>
			/// Cached name for the 'DeactivateWorldEnvironment' method.
			/// </summary>
			// Token: 0x04003D20 RID: 15648
			public static readonly StringName DeactivateWorldEnvironment = "DeactivateWorldEnvironment";

			/// <summary>
			/// Cached name for the 'SetScreenShakeTarget' method.
			/// </summary>
			// Token: 0x04003D21 RID: 15649
			public static readonly StringName SetScreenShakeTarget = "SetScreenShakeTarget";

			/// <summary>
			/// Cached name for the 'ClearScreenShakeTarget' method.
			/// </summary>
			// Token: 0x04003D22 RID: 15650
			public static readonly StringName ClearScreenShakeTarget = "ClearScreenShakeTarget";

			/// <summary>
			/// Cached name for the 'ScreenShake' method.
			/// </summary>
			// Token: 0x04003D23 RID: 15651
			public static readonly StringName ScreenShake = "ScreenShake";

			/// <summary>
			/// Cached name for the 'ScreenRumble' method.
			/// </summary>
			// Token: 0x04003D24 RID: 15652
			public static readonly StringName ScreenRumble = "ScreenRumble";

			/// <summary>
			/// Cached name for the 'ScreenShakeTrauma' method.
			/// </summary>
			// Token: 0x04003D25 RID: 15653
			public static readonly StringName ScreenShakeTrauma = "ScreenShakeTrauma";

			/// <summary>
			/// Cached name for the 'DoHitStop' method.
			/// </summary>
			// Token: 0x04003D26 RID: 15654
			public static readonly StringName DoHitStop = "DoHitStop";

			/// <summary>
			/// Cached name for the 'ToggleTrailerMode' method.
			/// </summary>
			// Token: 0x04003D27 RID: 15655
			public static readonly StringName ToggleTrailerMode = "ToggleTrailerMode";

			/// <summary>
			/// Cached name for the 'SetScreenshakeMultiplier' method.
			/// </summary>
			// Token: 0x04003D28 RID: 15656
			public static readonly StringName SetScreenshakeMultiplier = "SetScreenshakeMultiplier";

			/// <summary>
			/// Cached name for the 'InitPools' method.
			/// </summary>
			// Token: 0x04003D29 RID: 15657
			public static readonly StringName InitPools = "InitPools";

			/// <summary>
			/// Cached name for the 'CheckShowLocalizationOverrideErrors' method.
			/// </summary>
			// Token: 0x04003D2A RID: 15658
			public static readonly StringName CheckShowLocalizationOverrideErrors = "CheckShowLocalizationOverrideErrors";

			/// <summary>
			/// Cached name for the 'CheckShowModdedSaveFilePopup' method.
			/// </summary>
			// Token: 0x04003D2B RID: 15659
			public static readonly StringName CheckShowModdedSaveFilePopup = "CheckShowModdedSaveFilePopup";

			/// <summary>
			/// Cached name for the 'OnSteamNoLongerRunning' method.
			/// </summary>
			// Token: 0x04003D2C RID: 15660
			public static readonly StringName OnSteamNoLongerRunning = "OnSteamNoLongerRunning";

			/// <summary>
			/// Cached name for the 'LogResourceStats' method.
			/// </summary>
			// Token: 0x04003D2D RID: 15661
			public static readonly StringName LogResourceStats = "LogResourceStats";

			/// <summary>
			/// Cached name for the 'FormatBytes' method.
			/// </summary>
			// Token: 0x04003D2E RID: 15662
			public static readonly StringName FormatBytes = "FormatBytes";

			/// <summary>
			/// Cached name for the 'IsGameFocusedWindow' method.
			/// </summary>
			// Token: 0x04003D2F RID: 15663
			public static readonly StringName IsGameFocusedWindow = "IsGameFocusedWindow";

			/// <summary>
			/// Cached name for the 'GetOrCreateFeedbackScreen' method.
			/// </summary>
			// Token: 0x04003D30 RID: 15664
			public static readonly StringName GetOrCreateFeedbackScreen = "GetOrCreateFeedbackScreen";

			/// <summary>
			/// Cached name for the 'GetGameVersion' method.
			/// </summary>
			// Token: 0x04003D31 RID: 15665
			public static readonly StringName GetGameVersion = "GetGameVersion";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F61 RID: 3937
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'RootSceneContainer' property.
			/// </summary>
			// Token: 0x04003D32 RID: 15666
			public static readonly StringName RootSceneContainer = "RootSceneContainer";

			/// <summary>
			/// Cached name for the 'HoverTipsContainer' property.
			/// </summary>
			// Token: 0x04003D33 RID: 15667
			public static readonly StringName HoverTipsContainer = "HoverTipsContainer";

			/// <summary>
			/// Cached name for the 'MainMenu' property.
			/// </summary>
			// Token: 0x04003D34 RID: 15668
			public static readonly StringName MainMenu = "MainMenu";

			/// <summary>
			/// Cached name for the 'CurrentRunNode' property.
			/// </summary>
			// Token: 0x04003D35 RID: 15669
			public static readonly StringName CurrentRunNode = "CurrentRunNode";

			/// <summary>
			/// Cached name for the 'LogoAnimation' property.
			/// </summary>
			// Token: 0x04003D36 RID: 15670
			public static readonly StringName LogoAnimation = "LogoAnimation";

			/// <summary>
			/// Cached name for the 'Transition' property.
			/// </summary>
			// Token: 0x04003D37 RID: 15671
			public static readonly StringName Transition = "Transition";

			/// <summary>
			/// Cached name for the 'TimeoutOverlay' property.
			/// </summary>
			// Token: 0x04003D38 RID: 15672
			public static readonly StringName TimeoutOverlay = "TimeoutOverlay";

			/// <summary>
			/// Cached name for the 'AudioManager' property.
			/// </summary>
			// Token: 0x04003D39 RID: 15673
			public static readonly StringName AudioManager = "AudioManager";

			/// <summary>
			/// Cached name for the 'RemoteCursorContainer' property.
			/// </summary>
			// Token: 0x04003D3A RID: 15674
			public static readonly StringName RemoteCursorContainer = "RemoteCursorContainer";

			/// <summary>
			/// Cached name for the 'InputManager' property.
			/// </summary>
			// Token: 0x04003D3B RID: 15675
			public static readonly StringName InputManager = "InputManager";

			/// <summary>
			/// Cached name for the 'HotkeyManager' property.
			/// </summary>
			// Token: 0x04003D3C RID: 15676
			public static readonly StringName HotkeyManager = "HotkeyManager";

			/// <summary>
			/// Cached name for the 'FeedbackScreen' property.
			/// </summary>
			// Token: 0x04003D3D RID: 15677
			public static readonly StringName FeedbackScreen = "FeedbackScreen";

			/// <summary>
			/// Cached name for the 'ReactionWheel' property.
			/// </summary>
			// Token: 0x04003D3E RID: 15678
			public static readonly StringName ReactionWheel = "ReactionWheel";

			/// <summary>
			/// Cached name for the 'ReactionContainer' property.
			/// </summary>
			// Token: 0x04003D3F RID: 15679
			public static readonly StringName ReactionContainer = "ReactionContainer";

			/// <summary>
			/// Cached name for the 'CursorManager' property.
			/// </summary>
			// Token: 0x04003D40 RID: 15680
			public static readonly StringName CursorManager = "CursorManager";

			/// <summary>
			/// Cached name for the 'DebugAudio' property.
			/// </summary>
			// Token: 0x04003D41 RID: 15681
			public static readonly StringName DebugAudio = "DebugAudio";

			/// <summary>
			/// Cached name for the 'DebugSeedOverride' property.
			/// </summary>
			// Token: 0x04003D42 RID: 15682
			public static readonly StringName DebugSeedOverride = "DebugSeedOverride";

			/// <summary>
			/// Cached name for the 'StartOnMainMenu' property.
			/// </summary>
			// Token: 0x04003D43 RID: 15683
			public static readonly StringName StartOnMainMenu = "StartOnMainMenu";

			/// <summary>
			/// Cached name for the 'InspectRelicScreen' property.
			/// </summary>
			// Token: 0x04003D44 RID: 15684
			public static readonly StringName InspectRelicScreen = "InspectRelicScreen";

			/// <summary>
			/// Cached name for the 'InspectCardScreen' property.
			/// </summary>
			// Token: 0x04003D45 RID: 15685
			public static readonly StringName InspectCardScreen = "InspectCardScreen";

			/// <summary>
			/// Cached name for the 'ScreenshakeTarget' property.
			/// </summary>
			// Token: 0x04003D46 RID: 15686
			public static readonly StringName ScreenshakeTarget = "ScreenshakeTarget";

			/// <summary>
			/// Cached name for the 'WorldEnvironment' property.
			/// </summary>
			// Token: 0x04003D47 RID: 15687
			public static readonly StringName WorldEnvironment = "WorldEnvironment";

			/// <summary>
			/// Cached name for the 'HitStop' property.
			/// </summary>
			// Token: 0x04003D48 RID: 15688
			public static readonly StringName HitStop = "HitStop";

			/// <summary>
			/// Cached name for the '_inspectionContainer' field.
			/// </summary>
			// Token: 0x04003D49 RID: 15689
			public static readonly StringName _inspectionContainer = "_inspectionContainer";

			/// <summary>
			/// Cached name for the '_screenShake' field.
			/// </summary>
			// Token: 0x04003D4A RID: 15690
			public static readonly StringName _screenShake = "_screenShake";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F62 RID: 3938
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'WindowChange' signal.
			/// </summary>
			// Token: 0x04003D4B RID: 15691
			public static readonly StringName WindowChange = "WindowChange";

			/// <summary>
			/// Cached name for the 'PhobiaModeToggled' signal.
			/// </summary>
			// Token: 0x04003D4C RID: 15692
			public static readonly StringName PhobiaModeToggled = "PhobiaModeToggled";
		}

		// Token: 0x02000F63 RID: 3939
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003D4D RID: 15693
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Action<string[]> <0>__OnFilesDropped;
		}
	}
}
