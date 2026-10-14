using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles event rooms by selecting random choices.
	/// Some events trigger combat mid-event (e.g., DenseVegetation's "Rest" option).
	/// This handler detects and handles those combats before returning to the event.
	/// </summary>
	// Token: 0x02000D83 RID: 3459
	[NullableContext(1)]
	[Nullable(0)]
	public class EventRoomHandler : IRoomHandler, IHandler
	{
		// Token: 0x1700264A RID: 9802
		// (get) Token: 0x06008B4B RID: 35659 RVA: 0x002E7DDB File Offset: 0x002E5FDB
		public RoomType[] HandledTypes
		{
			get
			{
				return new RoomType[] { RoomType.Event };
			}
		}

		// Token: 0x1700264B RID: 9803
		// (get) Token: 0x06008B4C RID: 35660 RVA: 0x002E7DE7 File Offset: 0x002E5FE7
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromMinutes(3L);
			}
		}

		// Token: 0x06008B4D RID: 35661 RVA: 0x002E7DF0 File Offset: 0x002E5FF0
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			EventRoomHandler.<>c__DisplayClass6_0 CS$<>8__locals1 = new EventRoomHandler.<>c__DisplayClass6_0();
			AutoSlayLog.Action("Waiting for event room");
			Node node = await this.WaitForEventRoom(ct);
			CS$<>8__locals1.eventRoom = node;
			bool flag = await this.WaitForEventOptions(CS$<>8__locals1.eventRoom, ct);
			if (flag)
			{
				AutoSlayLog.Action("Event room completed");
			}
			else
			{
				int iterations = 0;
				while (iterations < 50)
				{
					EventRoomHandler.<>c__DisplayClass6_1 CS$<>8__locals2 = new EventRoomHandler.<>c__DisplayClass6_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					ct.ThrowIfCancellationRequested();
					if (!GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) || !CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
					{
						RunState runState = RunManager.Instance.DebugOnlyGetState();
						if (runState != null && runState.CurrentRoomCount > 1)
						{
							AbstractRoom baseRoom = runState.BaseRoom;
							if (baseRoom != null && baseRoom.RoomType == RoomType.Event)
							{
								AutoSlayLog.Action("Event triggered combat, handling combat first");
								await this.HandleEventCombat(ct);
								AutoSlayLog.Action("Combat finished, checking if event resumes");
								Node nodeOrNull = ((SceneTree)Engine.GetMainLoop()).Root.GetNodeOrNull("/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom");
								if (nodeOrNull == null)
								{
									AutoSlayLog.Action("Event ended after combat (no event room)");
									break;
								}
								CS$<>8__locals2.CS$<>8__locals1.eventRoom = nodeOrNull;
								await Task.Delay(500, ct);
								if ((from o in UiHelper.FindAll<NEventOptionButton>(CS$<>8__locals2.CS$<>8__locals1.eventRoom)
									where !o.Option.IsLocked
									select o).ToList<NEventOptionButton>().Count == 0)
								{
									AutoSlayLog.Action("Event finished after combat");
									break;
								}
								iterations++;
								continue;
							}
						}
						AutoSlayLog.Action("Event room no longer valid, exiting");
						break;
					}
					List<NEventOptionButton> list = (from o in UiHelper.FindAll<NEventOptionButton>(CS$<>8__locals2.CS$<>8__locals1.eventRoom)
						where !o.Option.IsLocked
						select o).ToList<NEventOptionButton>();
					if (list.Count == 0)
					{
						break;
					}
					List<NEventOptionButton> list2 = list.Where<NEventOptionButton>((NEventOptionButton o) => o.Option.WillKillPlayer == null || o.Event.Owner == null || !o.Option.WillKillPlayer(o.Event.Owner)).ToList<NEventOptionButton>();
					if (list2.Count > 0)
					{
						list = list2;
					}
					NEventOptionButton choice = random.NextItem<NEventOptionButton>(list);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Selecting event option: ");
					defaultInterpolatedStringHandler.AppendFormatted(choice.Event.Id.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" (option: ");
					defaultInterpolatedStringHandler.AppendFormatted(choice.Option.Title.GetFormattedText());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
					CS$<>8__locals2.previousButtons = new HashSet<NEventOptionButton>(from o in UiHelper.FindAll<NEventOptionButton>(CS$<>8__locals2.CS$<>8__locals1.eventRoom)
						where !o.Option.IsLocked
						select o);
					await UiHelper.Click(choice, 100);
					if (choice.Option.IsProceed)
					{
						AutoSlayLog.Action("Clicked proceed, exiting event");
						Func<bool> func;
						if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__3) == null)
						{
							EventRoomHandler.<>c__DisplayClass6_0 CS$<>8__locals3 = CS$<>8__locals2.CS$<>8__locals1;
							Func<bool> func2 = delegate
							{
								if (GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) && CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
								{
									NMapScreen instance2 = NMapScreen.Instance;
									return instance2 != null && instance2.IsOpen;
								}
								return true;
							};
							CS$<>8__locals3.<>9__3 = func2;
							func = func2;
						}
						await WaitHelper.Until(func, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event room did not close after clicking proceed");
						break;
					}
					await WaitHelper.Until(delegate
					{
						NOverlayStack instance3 = NOverlayStack.Instance;
						if (instance3 != null && instance3.ScreenCount > 0)
						{
							return true;
						}
						NMapScreen instance4 = NMapScreen.Instance;
						if (instance4 != null && instance4.IsOpen)
						{
							return true;
						}
						if (!GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) || !CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
						{
							return true;
						}
						if (CombatManager.Instance.IsInProgress)
						{
							return true;
						}
						List<NEventOptionButton> list3 = (from o in UiHelper.FindAll<NEventOptionButton>(CS$<>8__locals2.CS$<>8__locals1.eventRoom)
							where !o.Option.IsLocked
							select o).ToList<NEventOptionButton>();
						return list3.Count == 0 || !CS$<>8__locals2.previousButtons.SetEquals(list3);
					}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event options did not change after choice");
					NOverlayStack instance = NOverlayStack.Instance;
					if (instance != null && instance.ScreenCount > 0)
					{
						AutoSlayLog.Action("Overlay screen opened during event, deferring to drain loop");
						break;
					}
					if (CombatManager.Instance.IsInProgress)
					{
						AutoSlayLog.Action("Combat started during event (combat layout), handling combat");
						await this.HandleEventCombat(ct);
						if (!GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) || !CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
						{
							AutoSlayLog.Action("Event room gone after combat");
							break;
						}
						Func<bool> func3;
						if ((func3 = CS$<>8__locals2.CS$<>8__locals1.<>9__5) == null)
						{
							EventRoomHandler.<>c__DisplayClass6_0 CS$<>8__locals4 = CS$<>8__locals2.CS$<>8__locals1;
							Func<bool> func4 = delegate
							{
								if (GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) && CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
								{
									NMapScreen instance5 = NMapScreen.Instance;
									if (instance5 == null || !instance5.IsOpen)
									{
										NOverlayStack instance6 = NOverlayStack.Instance;
										if (instance6 == null || instance6.ScreenCount <= 0)
										{
											return UiHelper.FindAll<NEventOptionButton>(CS$<>8__locals2.CS$<>8__locals1.eventRoom).Any<NEventOptionButton>((NEventOptionButton o) => !o.Option.IsLocked);
										}
									}
								}
								return true;
							};
							CS$<>8__locals4.<>9__5 = func4;
							func3 = func4;
						}
						await WaitHelper.Until(func3, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Event did not resume after combat layout combat");
						if (!GodotObject.IsInstanceValid(CS$<>8__locals2.CS$<>8__locals1.eventRoom) || !CS$<>8__locals2.CS$<>8__locals1.eventRoom.IsInsideTree())
						{
							AutoSlayLog.Action("Event room gone after combat");
							break;
						}
					}
					iterations++;
					CS$<>8__locals2 = null;
					choice = null;
				}
				if (iterations >= 50)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Event room hit iteration limit (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(50);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AutoSlayLog.Action("Event room completed");
			}
		}

		// Token: 0x06008B4E RID: 35662 RVA: 0x002E7E44 File Offset: 0x002E6044
		private async Task HandleEventCombat(CancellationToken ct)
		{
			await WaitHelper.Until(() => CombatManager.Instance.IsInProgress, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Event combat not started");
			AutoSlayLog.Action("Event combat started, applying buffs and killing enemies");
			Creature player = LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState()).Creature;
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player, 100m, player, null, false);
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), player, 100m, player, null, false);
			await PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), player, 100m, player, null, false);
			int killAttempts = 0;
			int noEnemyWaitLoops = 0;
			while (CombatManager.Instance.IsInProgress && killAttempts < 20)
			{
				ct.ThrowIfCancellationRequested();
				CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
				if (combatState == null)
				{
					AutoSlayLog.Info("Combat state became null, exiting kill loop");
					break;
				}
				foreach (Creature creature in combatState.Enemies)
				{
					foreach (PowerModel powerModel in creature.Powers.Where<PowerModel>((PowerModel p) => p.ShouldStopCombatFromEnding()).ToList<PowerModel>())
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Removing blocking power ");
						defaultInterpolatedStringHandler.AppendFormatted(powerModel.GetType().Name);
						defaultInterpolatedStringHandler.AppendLiteral(" from ");
						defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
						AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						await PowerCmd.Remove(powerModel);
					}
					List<PowerModel>.Enumerator enumerator2 = default(List<PowerModel>.Enumerator);
					creature = null;
				}
				IEnumerator<Creature> enumerator = null;
				List<Creature> list = combatState.Enemies.Where<Creature>((Creature e) => e.IsAlive).ToList<Creature>();
				if (list.Count > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Killing ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" event combat enemies (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(killAttempts + 1);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
					await CreatureCmd.Kill(list, false);
					killAttempts++;
					noEnemyWaitLoops = 0;
					await Task.Delay(1000, ct);
					await CombatManager.Instance.CheckWinCondition();
				}
				else
				{
					noEnemyWaitLoops++;
					if (noEnemyWaitLoops > 100)
					{
						AutoSlayLog.Info("Event combat still in progress but no enemies for 10s, breaking loop");
						break;
					}
					await Task.Delay(100, ct);
				}
				combatState = null;
			}
			if (CombatManager.Instance.IsInProgress)
			{
				await CombatManager.Instance.CheckWinCondition();
			}
			await WaitHelper.Until(() => !CombatManager.Instance.IsInProgress, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Event combat did not end");
			AutoSlayLog.Action("Event combat finished");
		}

		// Token: 0x06008B4F RID: 35663 RVA: 0x002E7E88 File Offset: 0x002E6088
		private async Task<Node> WaitForEventRoom(CancellationToken ct)
		{
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			return await WaitHelper.ForNode<Node>(root, "/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom", ct, null);
		}

		/// <summary>
		/// Waits for event options to become available.
		/// </summary>
		/// <returns>True if the event was fully handled (custom events), false if options loop should run.</returns>
		// Token: 0x06008B50 RID: 35664 RVA: 0x002E7ECC File Offset: 0x002E60CC
		private async Task<bool> WaitForEventOptions(Node eventRoom, CancellationToken ct)
		{
			NAncientEventLayout nancientEventLayout = UiHelper.FindFirst<NAncientEventLayout>(eventRoom);
			bool flag;
			if (nancientEventLayout != null)
			{
				await this.HandleAncientEventDialogue(nancientEventLayout, ct);
				flag = false;
			}
			else
			{
				NFakeMerchant nfakeMerchant = UiHelper.FindFirst<NFakeMerchant>(eventRoom);
				if (nfakeMerchant != null)
				{
					AutoSlayLog.Info("Detected custom event: FakeMerchant");
					await this.HandleFakeMerchantEvent(nfakeMerchant, ct);
					flag = true;
				}
				else
				{
					int waitCycles = 0;
					await WaitHelper.Until(delegate
					{
						int waitCycles2 = waitCycles;
						waitCycles = waitCycles2 + 1;
						if (waitCycles % 50 == 0)
						{
							int childCount = eventRoom.GetChildCount(false);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Waiting for event options: ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(childCount);
							defaultInterpolatedStringHandler.AppendLiteral(" children in event room");
							AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
							Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
							if (currentWatchdog != null)
							{
								currentWatchdog.Reset("Waiting for event options to load");
							}
						}
						return UiHelper.FindAll<NEventOptionButton>(eventRoom).Count > 0;
					}, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Event options not loaded");
					flag = false;
				}
			}
			return flag;
		}

		/// <summary>
		/// Handles the FakeMerchant custom event.
		/// This event uses NProceedButton instead of NEventOptionButton.
		/// We simply click the proceed button to leave.
		/// </summary>
		// Token: 0x06008B51 RID: 35665 RVA: 0x002E7F20 File Offset: 0x002E6120
		private async Task HandleFakeMerchantEvent(NFakeMerchant fakeMerchant, CancellationToken ct)
		{
			AutoSlayLog.Action("Handling FakeMerchant event");
			NProceedButton proceedButton = null;
			await WaitHelper.Until(delegate
			{
				proceedButton = UiHelper.FindFirst<NProceedButton>(fakeMerchant);
				return proceedButton != null && proceedButton.IsEnabled && proceedButton.Visible;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "FakeMerchant proceed button not available");
			AutoSlayLog.Action("Clicking FakeMerchant proceed button");
			await UiHelper.Click(proceedButton, 100);
		}

		/// <summary>
		/// Handles Ancient events which require clicking through dialogue before options appear.
		/// </summary>
		// Token: 0x06008B52 RID: 35666 RVA: 0x002E7F6C File Offset: 0x002E616C
		private async Task HandleAncientEventDialogue(NAncientEventLayout ancientLayout, CancellationToken ct)
		{
			AutoSlayLog.Info("Detected Ancient event, clicking through dialogue");
			int clicks = 0;
			while (clicks < 50)
			{
				ct.ThrowIfCancellationRequested();
				if (!GodotObject.IsInstanceValid(ancientLayout))
				{
					break;
				}
				List<NEventOptionButton> list = (from b in UiHelper.FindAll<NEventOptionButton>(ancientLayout)
					where b.IsEnabled && !b.Option.IsLocked
					select b).ToList<NEventOptionButton>();
				if (list.Count > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Ancient dialogue finished, ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" options available");
					AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					break;
				}
				NButton nodeOrNull = ancientLayout.GetNodeOrNull<NButton>("%DialogueHitbox");
				if (nodeOrNull != null && nodeOrNull.Visible && nodeOrNull.IsEnabled)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Clicking Ancient dialogue (click ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(clicks + 1);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
					if (currentWatchdog != null)
					{
						currentWatchdog.Reset("Clicking Ancient event dialogue");
					}
					nodeOrNull.EmitSignal(NClickableControl.SignalName.Released, new Variant[] { nodeOrNull });
					int num = clicks;
					clicks = num + 1;
					await Task.Delay(500, ct);
				}
				else
				{
					await Task.Delay(100, ct);
				}
			}
			await WaitHelper.Until(() => UiHelper.FindAll<NEventOptionButton>(ancientLayout).Any<NEventOptionButton>((NEventOptionButton b) => b.IsEnabled && !b.Option.IsLocked), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Ancient event options did not become available after dialogue");
		}

		// Token: 0x04003042 RID: 12354
		private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom";

		// Token: 0x04003043 RID: 12355
		private const int _maxIterations = 50;
	}
}
