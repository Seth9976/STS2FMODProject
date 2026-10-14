using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles treasure rooms by opening the chest, picking relics, and proceeding.
	/// </summary>
	// Token: 0x02000D86 RID: 3462
	[NullableContext(1)]
	[Nullable(0)]
	public class TreasureRoomHandler : IRoomHandler, IHandler
	{
		// Token: 0x17002650 RID: 9808
		// (get) Token: 0x06008B5C RID: 35676 RVA: 0x002E8097 File Offset: 0x002E6297
		public RoomType[] HandledTypes
		{
			get
			{
				return new RoomType[] { RoomType.Treasure };
			}
		}

		// Token: 0x17002651 RID: 9809
		// (get) Token: 0x06008B5D RID: 35677 RVA: 0x002E80A3 File Offset: 0x002E62A3
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B5E RID: 35678 RVA: 0x002E80B0 File Offset: 0x002E62B0
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for treasure room");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			NTreasureRoom ntreasureRoom = await WaitHelper.ForNode<NTreasureRoom>(root, "/root/Game/RootSceneContainer/Run/RoomContainer/TreasureRoom", ct, null);
			NTreasureRoom room = ntreasureRoom;
			NClickableControl node = room.GetNode<NClickableControl>("Chest");
			AutoSlayLog.Action("Opening chest");
			await UiHelper.Click(node, 100);
			await Task.Delay(1000, ct);
			List<NTreasureRoomRelicHolder> list = UiHelper.FindAll<NTreasureRoomRelicHolder>(room);
			foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in list)
			{
				if (ntreasureRoomRelicHolder.IsEnabled && ntreasureRoomRelicHolder.Visible)
				{
					AutoSlayLog.Action("Picking up relic");
					await UiHelper.Click(ntreasureRoomRelicHolder, 100);
					await Task.Delay(500, ct);
				}
			}
			List<NTreasureRoomRelicHolder>.Enumerator enumerator = default(List<NTreasureRoomRelicHolder>.Enumerator);
			NProceedButton proceedButton = room.ProceedButton;
			await WaitHelper.Until(() => proceedButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Proceed button not enabled after picking relics");
			AutoSlayLog.Action("Clicking proceed");
			await UiHelper.Click(proceedButton, 100);
		}

		// Token: 0x04003046 RID: 12358
		private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/TreasureRoom";
	}
}
