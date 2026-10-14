using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles rest site rooms by selecting a random rest option.
	/// </summary>
	// Token: 0x02000D84 RID: 3460
	[NullableContext(1)]
	[Nullable(0)]
	public class RestSiteRoomHandler : IRoomHandler, IHandler
	{
		// Token: 0x1700264C RID: 9804
		// (get) Token: 0x06008B54 RID: 35668 RVA: 0x002E7FBF File Offset: 0x002E61BF
		public RoomType[] HandledTypes
		{
			get
			{
				return new RoomType[] { RoomType.RestSite };
			}
		}

		// Token: 0x1700264D RID: 9805
		// (get) Token: 0x06008B55 RID: 35669 RVA: 0x002E7FCB File Offset: 0x002E61CB
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B56 RID: 35670 RVA: 0x002E7FD8 File Offset: 0x002E61D8
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for rest site room");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			NRestSiteRoom nrestSiteRoom = await WaitHelper.ForNode<NRestSiteRoom>(root, "/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom", ct, null);
			NRestSiteRoom room = nrestSiteRoom;
			List<NRestSiteButton> list = (from b in UiHelper.FindAll<NRestSiteButton>(room)
				where b.Option.IsEnabled
				select b).ToList<NRestSiteButton>();
			if (list.Count == 0)
			{
				AutoSlayLog.Warn("No clickable rest site buttons found");
			}
			else
			{
				NRestSiteButton nrestSiteButton = random.NextItem<NRestSiteButton>(list);
				AutoSlayLog.Action("Selecting rest site option: " + nrestSiteButton.Option.GetType().Name);
				await UiHelper.Click(nrestSiteButton, 100);
				NProceedButton proceedButton = room.ProceedButton;
				await WaitHelper.Until(delegate
				{
					if (!proceedButton.IsEnabled)
					{
						NOverlayStack instance2 = NOverlayStack.Instance;
						return instance2 != null && instance2.ScreenCount > 0;
					}
					return true;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rest site option did not respond");
				NOverlayStack instance = NOverlayStack.Instance;
				if (instance != null && instance.ScreenCount > 0)
				{
					AutoSlayLog.Action("Overlay screen detected, deferring proceed to drain loop");
				}
				else
				{
					AutoSlayLog.Action("Clicking proceed");
					await UiHelper.Click(proceedButton, 100);
				}
			}
		}

		// Token: 0x04003044 RID: 12356
		private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom";
	}
}
