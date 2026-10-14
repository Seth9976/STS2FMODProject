using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.AutoSlay.Handlers;
using MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;
using MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.AutoSlay
{
	/// <summary>
	/// Main orchestrator for AutoSlay. Runs the game automatically for smoke testing.
	/// </summary>
	// Token: 0x02000D6A RID: 3434
	[NullableContext(1)]
	[Nullable(0)]
	public class AutoSlayer
	{
		// Token: 0x06008ACA RID: 35530 RVA: 0x002E569B File Offset: 0x002E389B
		static AutoSlayer()
		{
			NonInteractiveMode.AutoSlayerCheck = () => AutoSlayer.IsActive;
		}

		/// <summary>Static flag indicating if AutoSlay is currently running.</summary>
		// Token: 0x17002629 RID: 9769
		// (get) Token: 0x06008ACB RID: 35531 RVA: 0x002E56B2 File Offset: 0x002E38B2
		// (set) Token: 0x06008ACC RID: 35532 RVA: 0x002E56B9 File Offset: 0x002E38B9
		public static bool IsActive { get; private set; }

		/// <summary>Gets the current watchdog instance (for WaitHelper integration).</summary>
		// Token: 0x1700262A RID: 9770
		// (get) Token: 0x06008ACD RID: 35533 RVA: 0x002E56C1 File Offset: 0x002E38C1
		// (set) Token: 0x06008ACE RID: 35534 RVA: 0x002E56C8 File Offset: 0x002E38C8
		[Nullable(2)]
		public static Watchdog CurrentWatchdog
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06008ACF RID: 35535 RVA: 0x002E56D0 File Offset: 0x002E38D0
		public AutoSlayer()
		{
			CombatRoomHandler combatRoomHandler = new CombatRoomHandler();
			Dictionary<RoomType, IRoomHandler> dictionary = new Dictionary<RoomType, IRoomHandler>();
			dictionary[RoomType.Monster] = combatRoomHandler;
			dictionary[RoomType.Elite] = combatRoomHandler;
			dictionary[RoomType.Boss] = combatRoomHandler;
			dictionary[RoomType.Event] = new EventRoomHandler();
			dictionary[RoomType.Shop] = new ShopRoomHandler();
			dictionary[RoomType.Treasure] = new TreasureRoomHandler();
			dictionary[RoomType.RestSite] = new RestSiteRoomHandler();
			this._roomHandlers = dictionary;
			this._mapHandler = new MapScreenHandler();
			Dictionary<Type, IScreenHandler> dictionary2 = new Dictionary<Type, IScreenHandler>();
			Type typeFromHandle = typeof(NRewardsScreen);
			dictionary2[typeFromHandle] = new RewardsScreenHandler();
			Type typeFromHandle2 = typeof(NCardRewardSelectionScreen);
			dictionary2[typeFromHandle2] = new CardRewardScreenHandler();
			Type typeFromHandle3 = typeof(NDeckUpgradeSelectScreen);
			dictionary2[typeFromHandle3] = new DeckUpgradeScreenHandler();
			Type typeFromHandle4 = typeof(NDeckTransformSelectScreen);
			dictionary2[typeFromHandle4] = new DeckTransformScreenHandler();
			Type typeFromHandle5 = typeof(NDeckEnchantSelectScreen);
			dictionary2[typeFromHandle5] = new DeckEnchantScreenHandler();
			Type typeFromHandle6 = typeof(NDeckCardSelectScreen);
			dictionary2[typeFromHandle6] = new DeckCardSelectScreenHandler();
			Type typeFromHandle7 = typeof(NSimpleCardSelectScreen);
			dictionary2[typeFromHandle7] = new SimpleCardSelectScreenHandler();
			Type typeFromHandle8 = typeof(NChooseACardSelectionScreen);
			dictionary2[typeFromHandle8] = new ChooseACardScreenHandler();
			Type typeFromHandle9 = typeof(NChooseABundleSelectionScreen);
			dictionary2[typeFromHandle9] = new ChooseABundleScreenHandler();
			Type typeFromHandle10 = typeof(NChooseARelicSelection);
			dictionary2[typeFromHandle10] = new ChooseARelicScreenHandler();
			Type typeFromHandle11 = typeof(NGameOverScreen);
			dictionary2[typeFromHandle11] = new GameOverScreenHandler();
			Type typeFromHandle12 = typeof(NCrystalSphereScreen);
			dictionary2[typeFromHandle12] = new CrystalSphereScreenHandler();
			this._screenHandlers = dictionary2;
		}

		/// <summary>Starts an AutoSlay run with the given seed.</summary>
		// Token: 0x06008AD0 RID: 35536 RVA: 0x002E587C File Offset: 0x002E3A7C
		public void Start(string seed, [Nullable(2)] string logFile = null)
		{
			if (logFile != null)
			{
				AutoSlayLog.OpenLogFile(logFile);
			}
			SentryService.SetTag("autoslay", "true");
			SentryService.SetTag("autoslay.seed", seed);
			AutoSlayer.IsActive = true;
			this._cts = new CancellationTokenSource();
			Task task = this.RunAsync(seed, this._cts.Token);
			TaskHelper.RunSafely(task);
		}

		/// <summary>Stops the current AutoSlay run.</summary>
		// Token: 0x06008AD1 RID: 35537 RVA: 0x002E58D7 File Offset: 0x002E3AD7
		public void Stop()
		{
			AutoSlayer.IsActive = false;
			CancellationTokenSource cts = this._cts;
			if (cts != null)
			{
				cts.Cancel();
			}
			this._cts = null;
		}

		/// <summary>Gets the current overlay screen cast to the expected type.</summary>
		// Token: 0x06008AD2 RID: 35538 RVA: 0x002E58F7 File Offset: 0x002E3AF7
		public static T GetCurrentScreen<[Nullable(0)] T>() where T : Node
		{
			return (T)((object)NOverlayStack.Instance.Peek());
		}

		// Token: 0x06008AD3 RID: 35539 RVA: 0x002E5908 File Offset: 0x002E3B08
		private async Task RunAsync(string seed, CancellationToken ct)
		{
			AutoSlayLog.RunStarted(seed);
			try
			{
				await WaitHelper.WithTimeout((CancellationToken token) => this.PlayRunAsync(seed, token), AutoSlayConfig.runTimeout, ct);
				AutoSlayLog.RunCompleted(seed);
			}
			catch (Exception ex)
			{
				AutoSlayer._exitCode = 1;
				AutoSlayLog.RunFailed(seed, ex);
				throw;
			}
			finally
			{
				AutoSlayer.IsActive = false;
				AutoSlayer.CurrentWatchdog = null;
				this._watchdog = null;
				IDisposable cardSelectorScope = this._cardSelectorScope;
				if (cardSelectorScope != null)
				{
					cardSelectorScope.Dispose();
				}
				this._cardSelectorScope = null;
				MemoryProfiler.Reset();
				AutoSlayLog.CloseLogFile();
				AutoSlayer.QuitGame(AutoSlayer._exitCode);
			}
		}

		// Token: 0x06008AD4 RID: 35540 RVA: 0x002E595C File Offset: 0x002E3B5C
		private async Task PlayRunAsync(string seed, CancellationToken ct)
		{
			AutoSlayer.<>c__DisplayClass22_0 CS$<>8__locals1 = new AutoSlayer.<>c__DisplayClass22_0();
			await WaitHelper.Until(() => NGame.Instance != null, ct, new TimeSpan?(AutoSlayConfig.gameInitTimeout), "Game instance not initialized");
			NGame.Instance.DebugSeedOverride = seed;
			SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
			SaveManager.Instance.SetFtuesEnabled(false);
			SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Silent1Epoch>(), EpochState.Revealed);
			SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Regent1Epoch>(), EpochState.Revealed);
			SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Defect1Epoch>(), EpochState.Revealed);
			SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Necrobinder1Epoch>(), EpochState.Revealed);
			this._random = new Rng(StringHelper.GetDeterministicHashCode(seed));
			this._cardSelectorScope = CardSelectCmd.UseSelector(new AutoSlayCardSelector(this._random), false);
			this._watchdog = new Watchdog();
			AutoSlayer.CurrentWatchdog = this._watchdog;
			this._watchdog.Reset("Playing main menu");
			await this.PlayMainMenuAsync(ct);
			await WaitHelper.Until(() => RunManager.Instance.DebugOnlyGetState() != null, ct, new TimeSpan?(AutoSlayConfig.runStateTimeout), "Run state not initialized");
			CS$<>8__locals1.runState = RunManager.Instance.DebugOnlyGetState();
			MemoryProfiler.SetBaseline();
			await WaitHelper.Until(() => CS$<>8__locals1.runState.CurrentRoom != null && CS$<>8__locals1.runState.CurrentRoom.RoomType > RoomType.Unassigned, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Room type not assigned");
			while (CS$<>8__locals1.runState.TotalFloor < 49)
			{
				ct.ThrowIfCancellationRequested();
				RoomType roomType = CS$<>8__locals1.runState.CurrentRoom.RoomType;
				Watchdog watchdog = this._watchdog;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Entering ");
				defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
				defaultInterpolatedStringHandler.AppendLiteral(" room (Act ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.CurrentActIndex + 1);
				defaultInterpolatedStringHandler.AppendLiteral(", Floor ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.ActFloor);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				watchdog.Reset(defaultInterpolatedStringHandler.ToStringAndClear());
				AutoSlayLog.EnterRoom(roomType, CS$<>8__locals1.runState.CurrentActIndex, CS$<>8__locals1.runState.ActFloor);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 3);
				defaultInterpolatedStringHandler.AppendLiteral("pre-room:");
				defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
				defaultInterpolatedStringHandler.AppendLiteral(":Act");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.CurrentActIndex + 1);
				defaultInterpolatedStringHandler.AppendLiteral(":F");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.ActFloor);
				MemoryProfiler.LogSnapshot(defaultInterpolatedStringHandler.ToStringAndClear());
				await this.HandleRoomAsync(roomType, ct);
				if (roomType - RoomType.Monster <= 2)
				{
					await this.WaitForRewardsScreenAsync(ct);
				}
				else
				{
					await Task.Delay(500, ct);
				}
				await this.DrainOverlayScreensAsync(ct);
				if (roomType == RoomType.RestSite)
				{
					await this.ClickRestSiteProceedIfNeeded(ct);
				}
				if (roomType == RoomType.Event)
				{
					await this.ClickEventProceedIfNeeded(ct);
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 3);
				defaultInterpolatedStringHandler.AppendLiteral("post-room:");
				defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
				defaultInterpolatedStringHandler.AppendLiteral(":Act");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.CurrentActIndex + 1);
				defaultInterpolatedStringHandler.AppendLiteral(":F");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.runState.ActFloor);
				MemoryProfiler.LogSnapshot(defaultInterpolatedStringHandler.ToStringAndClear());
				bool flag = roomType == RoomType.Boss && CS$<>8__locals1.runState.Map.SecondBossMapPoint != null && CS$<>8__locals1.runState.CurrentMapCoord == CS$<>8__locals1.runState.Map.BossMapPoint.coord;
				if (roomType == RoomType.Boss && !flag)
				{
					AutoSlayer.<>c__DisplayClass22_1 CS$<>8__locals2 = new AutoSlayer.<>c__DisplayClass22_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					this._watchdog.Reset("Waiting for act transition after boss");
					CS$<>8__locals2.postBossRoomType = RoomType.Boss;
					await WaitHelper.Until(delegate
					{
						AbstractRoom currentRoom = CS$<>8__locals2.CS$<>8__locals1.runState.CurrentRoom;
						if (currentRoom == null)
						{
							return false;
						}
						CS$<>8__locals2.postBossRoomType = currentRoom.RoomType;
						return CS$<>8__locals2.postBossRoomType != RoomType.Boss;
					}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Act transition did not start after boss");
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Post-boss transition: room type is now ");
					defaultInterpolatedStringHandler.AppendFormatted<RoomType>(CS$<>8__locals2.postBossRoomType);
					AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					if (CS$<>8__locals2.postBossRoomType == RoomType.Event && CS$<>8__locals2.CS$<>8__locals1.runState.CurrentActIndex >= CS$<>8__locals2.CS$<>8__locals1.runState.Acts.Count - 1)
					{
						Watchdog watchdog2 = this._watchdog;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Entering ");
						defaultInterpolatedStringHandler.AppendFormatted<RoomType>(CS$<>8__locals2.postBossRoomType);
						defaultInterpolatedStringHandler.AppendLiteral(" room (Act ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals2.CS$<>8__locals1.runState.CurrentActIndex + 1);
						defaultInterpolatedStringHandler.AppendLiteral(", Floor ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals2.CS$<>8__locals1.runState.ActFloor);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						watchdog2.Reset(defaultInterpolatedStringHandler.ToStringAndClear());
						AutoSlayLog.EnterRoom(CS$<>8__locals2.postBossRoomType, CS$<>8__locals2.CS$<>8__locals1.runState.CurrentActIndex, CS$<>8__locals2.CS$<>8__locals1.runState.ActFloor);
						await this.HandleRoomAsync(CS$<>8__locals2.postBossRoomType, ct);
						await this.WaitForGameOverScreenAsync(ct);
						await this.DrainOverlayScreensAsync(ct);
						this._watchdog.Reset("Waiting for main menu after victory");
						await this.WaitForMainMenuAsync(ct);
						AutoSlayLog.Action("Victory! Run completed and returned to main menu");
						return;
					}
					Func<bool> func;
					if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
					{
						AutoSlayer.<>c__DisplayClass22_0 CS$<>8__locals3 = CS$<>8__locals2.CS$<>8__locals1;
						Func<bool> func2 = () => CS$<>8__locals2.CS$<>8__locals1.runState.VisitedMapCoords.Count == 0;
						CS$<>8__locals3.<>9__4 = func2;
						func = func2;
					}
					await WaitHelper.Until(func, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Act transition did not complete (VisitedMapCoords not cleared)");
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
					defaultInterpolatedStringHandler.AppendLiteral("act-transition:Act");
					defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals2.CS$<>8__locals1.runState.CurrentActIndex + 1);
					MemoryProfiler.LogSnapshot(defaultInterpolatedStringHandler.ToStringAndClear());
					CS$<>8__locals2 = null;
				}
				this._watchdog.Reset("Navigating map");
				await this._mapHandler.HandleAsync(this._random, ct);
			}
			AutoSlayLog.Action("Run completed (max floor reached). Abandoning");
			await this.AbandonRunAsync(ct);
		}

		// Token: 0x06008AD5 RID: 35541 RVA: 0x002E59B0 File Offset: 0x002E3BB0
		private async Task HandleRoomAsync(RoomType roomType, CancellationToken ct)
		{
			IRoomHandler handler;
			if (!this._roomHandlers.TryGetValue(roomType, out handler))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No handler for room type: ");
				defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
				AutoSlayLog.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				await WaitHelper.WithTimeout((CancellationToken token) => handler.HandleAsync(this._random, token), handler.Timeout, ct);
				AutoSlayLog.ExitRoom(roomType);
			}
		}

		// Token: 0x06008AD6 RID: 35542 RVA: 0x002E5A04 File Offset: 0x002E3C04
		private async Task DrainOverlayScreensAsync(CancellationToken ct)
		{
			if (NOverlayStack.Instance == null)
			{
				await WaitHelper.Until(() => NOverlayStack.Instance != null, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Overlay stack not initialized");
			}
			HashSet<IOverlayScreen> handledScreens = new HashSet<IOverlayScreen>();
			int consecutiveFailures = 0;
			IOverlayScreen currentOverlay;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			Type type;
			for (;;)
			{
				NOverlayStack instance = NOverlayStack.Instance;
				if (instance == null || instance.ScreenCount <= 0)
				{
					return;
				}
				ct.ThrowIfCancellationRequested();
				currentOverlay = NOverlayStack.Instance.Peek();
				if (currentOverlay == null)
				{
					return;
				}
				if (handledScreens.Contains(currentOverlay))
				{
					consecutiveFailures++;
					if (consecutiveFailures >= 3)
					{
						break;
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Screen ");
					defaultInterpolatedStringHandler.AppendFormatted(currentOverlay.GetType().Name);
					defaultInterpolatedStringHandler.AppendLiteral(" still present after handling (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(consecutiveFailures);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					handledScreens.Add(currentOverlay);
					consecutiveFailures = 0;
				}
				type = ((Node)currentOverlay).GetType();
				IScreenHandler handler;
				if (!this._screenHandlers.TryGetValue(type, out handler))
				{
					goto Block_6;
				}
				this._watchdog.Reset("Handling screen: " + type.Name);
				AutoSlayLog.Info("Handling screen: " + type.Name);
				await WaitHelper.WithTimeout((CancellationToken token) => handler.HandleAsync(this._random, token), handler.Timeout, ct);
				if (currentOverlay is NRewardsScreen)
				{
					NMapScreen instance2 = NMapScreen.Instance;
					if (instance2 != null && instance2.IsOpen)
					{
						goto Block_9;
					}
				}
				await Task.Delay(100, ct);
				currentOverlay = null;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Infinite loop detected: screen ");
			defaultInterpolatedStringHandler.AppendFormatted(currentOverlay.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(" not closing after ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(3);
			defaultInterpolatedStringHandler.AppendLiteral(" attempts");
			AutoSlayLog.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			throw new InvalidOperationException("Screen " + currentOverlay.GetType().Name + " not closing after being handled");
			Block_6:
			AutoSlayLog.Warn("No handler for screen type: " + type.Name);
			return;
			Block_9:
			AutoSlayLog.Info("Rewards screen handled and map is open, exiting drain loop");
		}

		// Token: 0x06008AD7 RID: 35543 RVA: 0x002E5A50 File Offset: 0x002E3C50
		private async Task ClickRestSiteProceedIfNeeded(CancellationToken ct)
		{
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			NProceedButton nodeOrNull = root.GetNodeOrNull<NProceedButton>("/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom/ProceedButton");
			if (nodeOrNull != null && nodeOrNull.IsEnabled)
			{
				AutoSlayLog.Action("Clicking rest site proceed button");
				await UiHelper.Click(nodeOrNull, 100);
			}
		}

		// Token: 0x06008AD8 RID: 35544 RVA: 0x002E5A8C File Offset: 0x002E3C8C
		private async Task ClickEventProceedIfNeeded(CancellationToken ct)
		{
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			Node eventRoom = root.GetNodeOrNull("/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom");
			if (eventRoom == null)
			{
				AutoSlayLog.Info("Event room not found for proceed check");
			}
			else
			{
				NEventOptionButton proceedOption = null;
				await WaitHelper.Until(delegate
				{
					NMapScreen instance = NMapScreen.Instance;
					if (instance != null && instance.IsOpen)
					{
						return true;
					}
					List<NEventOptionButton> list = (from o in UiHelper.FindAll<NEventOptionButton>(eventRoom)
						where !o.Option.IsLocked && o.Option.IsProceed
						select o).ToList<NEventOptionButton>();
					if (list.Count > 0)
					{
						proceedOption = list[0];
						return true;
					}
					return false;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event proceed option or map did not appear");
				if (proceedOption != null)
				{
					AutoSlayLog.Action("Clicking event proceed option");
					await UiHelper.Click(proceedOption, 100);
				}
				else
				{
					AutoSlayLog.Info("Map already open, no proceed needed");
				}
			}
		}

		// Token: 0x06008AD9 RID: 35545 RVA: 0x002E5AD0 File Offset: 0x002E3CD0
		private async Task WaitForRewardsScreenAsync(CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for rewards screen");
			await WaitHelper.Until(delegate
			{
				NOverlayStack instance = NOverlayStack.Instance;
				if (!(((instance != null) ? instance.Peek() : null) is NRewardsScreen))
				{
					NMapScreen instance2 = NMapScreen.Instance;
					return instance2 != null && instance2.IsOpen;
				}
				return true;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rewards screen did not appear after combat");
		}

		// Token: 0x06008ADA RID: 35546 RVA: 0x002E5B14 File Offset: 0x002E3D14
		private async Task WaitForGameOverScreenAsync(CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for game over screen");
			await WaitHelper.Until(delegate
			{
				NOverlayStack instance = NOverlayStack.Instance;
				return ((instance != null) ? instance.Peek() : null) is NGameOverScreen;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Game over screen did not appear");
		}

		// Token: 0x06008ADB RID: 35547 RVA: 0x002E5B58 File Offset: 0x002E3D58
		private async Task WaitForMainMenuAsync(CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for main menu");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			await WaitHelper.Until(delegate
			{
				Control nodeOrNull = root.GetNodeOrNull<Control>("/root/Game/RootSceneContainer/MainMenu");
				return nodeOrNull != null && nodeOrNull.IsVisibleInTree();
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Main menu did not appear after game over");
			AutoSlayLog.Action("Main menu appeared");
		}

		// Token: 0x06008ADC RID: 35548 RVA: 0x002E5B9C File Offset: 0x002E3D9C
		private async Task PlayMainMenuAsync(CancellationToken ct)
		{
			AutoSlayLog.Action("Playing main menu");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			Control control = await WaitHelper.ForNode<Control>(root, "/root/Game/RootSceneContainer/MainMenu", ct, new TimeSpan?(TimeSpan.FromSeconds(30L)));
			Control mainMenu = control;
			NButton node = mainMenu.GetNode<NButton>("MainMenuTextButtons/AbandonRunButton");
			if (node.Visible)
			{
				AutoSlayLog.Action("Abandoning existing run");
				await UiHelper.Click(node, 100);
				await WaitHelper.Until(delegate
				{
					NModalContainer instance = NModalContainer.Instance;
					return ((instance != null) ? instance.OpenModal : null) != null;
				}, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not appear");
				NButton node2 = ((Node)NModalContainer.Instance.OpenModal).GetNode<NButton>("VerticalPopup/YesButton");
				AutoSlayLog.Action("Confirming abandon");
				await UiHelper.Click(node2, 100);
				await WaitHelper.Until(() => NModalContainer.Instance.OpenModal == null, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not close");
			}
			NButton node3 = mainMenu.GetNode<NButton>("MainMenuTextButtons/SingleplayerButton");
			AutoSlayLog.Action("Clicking singleplayer");
			await UiHelper.Click(node3, 100);
			Control charSelectScreen = mainMenu.GetNodeOrNull<Control>("Submenus/CharacterSelectScreen");
			NButton standardButton = mainMenu.GetNodeOrNull<NButton>("Submenus/SingleplayerSubmenu/StandardButton");
			await WaitHelper.Until(delegate
			{
				charSelectScreen = mainMenu.GetNodeOrNull<Control>("Submenus/CharacterSelectScreen");
				standardButton = mainMenu.GetNodeOrNull<NButton>("Submenus/SingleplayerSubmenu/StandardButton");
				Control charSelectScreen2 = charSelectScreen;
				bool flag = charSelectScreen2 != null && charSelectScreen2.Visible;
				NButton standardButton2 = standardButton;
				bool flag2 = standardButton2 != null && standardButton2.Visible;
				return flag || flag2;
			}, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Neither CharacterSelectScreen nor SingleplayerSubmenu became visible");
			NButton standardButton3 = standardButton;
			if (standardButton3 != null && standardButton3.Visible)
			{
				Control charSelectScreen3 = charSelectScreen;
				if (charSelectScreen3 == null || !charSelectScreen3.Visible)
				{
					AutoSlayLog.Action("Clicking standard run");
					await UiHelper.Click(standardButton, 100);
					await WaitHelper.Until(delegate
					{
						Control nodeOrNull = mainMenu.GetNodeOrNull<Control>("Submenus/CharacterSelectScreen");
						return nodeOrNull != null && nodeOrNull.Visible;
					}, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "CharacterSelectScreen did not become visible");
					charSelectScreen = mainMenu.GetNode<Control>("Submenus/CharacterSelectScreen");
					goto IL_05E6;
				}
			}
			AutoSlayLog.Action("Skipping submenu (first run)");
			IL_05E6:
			Node node4 = charSelectScreen.GetNode("CharSelectButtons/ButtonContainer");
			List<NCharacterSelectButton> list = UiHelper.FindAll<NCharacterSelectButton>(node4);
			foreach (NCharacterSelectButton ncharacterSelectButton in list)
			{
				ncharacterSelectButton.UnlockIfPossible();
			}
			List<NCharacterSelectButton> list2 = list.Where<NCharacterSelectButton>((NCharacterSelectButton b) => !b.IsLocked).ToList<NCharacterSelectButton>();
			NCharacterSelectButton ncharacterSelectButton2 = this._random.NextItem<NCharacterSelectButton>(list2);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Selecting character: ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(ncharacterSelectButton2.Character.Id);
			AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
			ncharacterSelectButton2.Select();
			await Task.Delay(100, ct);
			NButton nbutton = await WaitHelper.ForNode<NButton>(mainMenu, "Submenus/CharacterSelectScreen/ConfirmButton", ct, null);
			AutoSlayLog.Action("Confirming character");
			await UiHelper.Click(nbutton, 100);
		}

		// Token: 0x06008ADD RID: 35549 RVA: 0x002E5BE8 File Offset: 0x002E3DE8
		private async Task AbandonRunAsync(CancellationToken ct)
		{
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			await Task.Delay(1000, ct);
			NTopBarPauseButton ntopBarPauseButton = await WaitHelper.ForNode<NTopBarPauseButton>(root, "/root/Game/RootSceneContainer/Run/GlobalUi/TopBar/RightAlignedStuff/PauseButton", ct, null);
			await UiHelper.Click(ntopBarPauseButton, 100);
			NPauseMenu pauseMenu = null;
			await WaitHelper.Until(() => (pauseMenu = UiHelper.FindFirst<NPauseMenu>(root)) != null && pauseMenu.IsVisibleInTree(), ct, null, "Pause menu did not open");
			NPauseMenuButton node = pauseMenu.GetNode<Control>("%ButtonContainer").GetNode<NPauseMenuButton>("GiveUp");
			await UiHelper.Click(node, 100);
			NAbandonRunConfirmPopup confirmPopup = null;
			await WaitHelper.Until(() => (confirmPopup = UiHelper.FindFirst<NAbandonRunConfirmPopup>(root)) != null, ct, null, "Abandon confirm popup did not appear");
			NVerticalPopup node2 = confirmPopup.GetNode<NVerticalPopup>("VerticalPopup");
			await UiHelper.Click(node2.YesButton, 100);
			await this.WaitForGameOverScreenAsync(ct);
			await this.DrainOverlayScreensAsync(ct);
			await this.WaitForMainMenuAsync(ct);
		}

		// Token: 0x06008ADE RID: 35550 RVA: 0x002E5C34 File Offset: 0x002E3E34
		private static void QuitGame(int exitCode)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Quitting game with exit code ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(exitCode);
			AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
			MegaLabel.DisposeCachedParagraph();
			MegaRichTextLabel.DisposeCachedParagraph();
			FontManager.ClearCache();
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.GetTree().Quit(exitCode);
		}

		// Token: 0x0400302E RID: 12334
		private readonly Dictionary<RoomType, IRoomHandler> _roomHandlers;

		// Token: 0x0400302F RID: 12335
		private readonly Dictionary<Type, IScreenHandler> _screenHandlers;

		// Token: 0x04003030 RID: 12336
		private readonly MapScreenHandler _mapHandler;

		// Token: 0x04003031 RID: 12337
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x04003032 RID: 12338
		[Nullable(2)]
		private Rng _random;

		// Token: 0x04003033 RID: 12339
		[Nullable(2)]
		private Watchdog _watchdog;

		// Token: 0x04003034 RID: 12340
		[Nullable(2)]
		private IDisposable _cardSelectorScope;

		// Token: 0x04003036 RID: 12342
		private static int _exitCode;
	}
}
