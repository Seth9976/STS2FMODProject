using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles map screen navigation between rooms.
	/// </summary>
	// Token: 0x02000D7F RID: 3455
	[NullableContext(1)]
	[Nullable(0)]
	public class MapScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002642 RID: 9794
		// (get) Token: 0x06008B36 RID: 35638 RVA: 0x002E79E7 File Offset: 0x002E5BE7
		public Type ScreenType
		{
			get
			{
				return typeof(NMapScreen);
			}
		}

		// Token: 0x17002643 RID: 9795
		// (get) Token: 0x06008B37 RID: 35639 RVA: 0x002E79F3 File Offset: 0x002E5BF3
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B38 RID: 35640 RVA: 0x002E7A00 File Offset: 0x002E5C00
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NMapScreen");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			NRun runNode = root.GetNode<NRun>("/root/Game/RootSceneContainer/Run");
			await WaitHelper.Until(() => runNode.GlobalUi.MapScreen.IsVisibleInTree(), ct, new TimeSpan?(AutoSlayConfig.mapScreenTimeout), "Map screen not visible");
			NMapScreen mapScreen = runNode.GlobalUi.MapScreen;
			NMapPoint nextRoom = null;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			try
			{
				await WaitHelper.Until(delegate
				{
					NMapPoint nmapPoint = (nextRoom = MapScreenHandler.SelectNextRoom(mapScreen));
					return nmapPoint != null && nmapPoint.IsEnabled;
				}, ct, new TimeSpan?(AutoSlayConfig.mapPointEnabledTimeout), "Map point not enabled");
			}
			catch (AutoSlayTimeoutException)
			{
				string[] array = new string[6];
				array[0] = "[AutoSlay] Map point never became travelable: node=";
				array[1] = ((nextRoom == null) ? "none" : nextRoom.Point.coord.ToString());
				array[2] = ", state=";
				array[3] = ((nextRoom == null) ? "n/a" : nextRoom.State.ToString());
				array[4] = ", ";
				int num = 5;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler.AppendLiteral("screenTravelEnabled=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(mapScreen.IsTravelEnabled);
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				AutoSlayLog.Info(string.Concat(array));
				throw;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Selecting room at (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(nextRoom.Point.coord.row);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(nextRoom.Point.coord.col);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
			this._roomEnteredTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
			RunManager.Instance.RoomEntered += this.OnRoomEntered;
			try
			{
				await UiHelper.Click(nextRoom, 100);
				await WaitHelper.ForTask(this._roomEnteredTcs.Task, ct, new TimeSpan?(AutoSlayConfig.mapScreenTimeout), "Room not entered after map click");
			}
			finally
			{
				RunManager.Instance.RoomEntered -= this.OnRoomEntered;
				this._roomEnteredTcs = null;
			}
			AutoSlayLog.ExitScreen("NMapScreen");
		}

		/// <summary>
		/// Resolves the next room to travel to from the live map points, or null when the expected node is
		/// not present yet (for example mid act-transition while the new act's map is still being generated).
		/// Returning null keeps the caller polling instead of latching onto a stale node from the prior act.
		/// </summary>
		// Token: 0x06008B39 RID: 35641 RVA: 0x002E7A4C File Offset: 0x002E5C4C
		[return: Nullable(2)]
		private static NMapPoint SelectNextRoom(NMapScreen mapScreen)
		{
			MapScreenHandler.<>c__DisplayClass6_0 CS$<>8__locals1 = new MapScreenHandler.<>c__DisplayClass6_0();
			List<NMapPoint> list = UiHelper.FindAll<NMapPoint>(mapScreen);
			RunState runState = RunManager.Instance.DebugOnlyGetState();
			if (runState.VisitedMapCoords.Count == 0)
			{
				return list.FirstOrDefault<NMapPoint>((NMapPoint mp) => mp.Point.coord.row == 0);
			}
			MapScreenHandler.<>c__DisplayClass6_0 CS$<>8__locals2 = CS$<>8__locals1;
			IReadOnlyList<MapCoord> visitedMapCoords = runState.VisitedMapCoords;
			CS$<>8__locals2.lastCoord = visitedMapCoords[visitedMapCoords.Count - 1];
			NMapPoint nmapPoint = list.FirstOrDefault<NMapPoint>((NMapPoint mp) => mp.Point.coord.Equals(CS$<>8__locals1.lastCoord));
			CS$<>8__locals1.child = ((nmapPoint != null) ? nmapPoint.Point.Children.FirstOrDefault<MapPoint>() : null);
			if (CS$<>8__locals1.child != null)
			{
				return list.FirstOrDefault<NMapPoint>((NMapPoint mp) => mp.Point.coord.Equals(CS$<>8__locals1.child.coord));
			}
			return null;
		}

		// Token: 0x06008B3A RID: 35642 RVA: 0x002E7B08 File Offset: 0x002E5D08
		private void OnRoomEntered()
		{
			TaskCompletionSource roomEnteredTcs = this._roomEnteredTcs;
			if (roomEnteredTcs == null)
			{
				return;
			}
			roomEnteredTcs.TrySetResult();
		}

		// Token: 0x04003041 RID: 12353
		[Nullable(2)]
		private TaskCompletionSource _roomEnteredTcs;
	}
}
