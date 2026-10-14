using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles shop rooms by attempting to buy every affordable item.
	/// </summary>
	// Token: 0x02000D85 RID: 3461
	[NullableContext(1)]
	[Nullable(0)]
	public class ShopRoomHandler : IRoomHandler, IHandler
	{
		// Token: 0x1700264E RID: 9806
		// (get) Token: 0x06008B58 RID: 35672 RVA: 0x002E802B File Offset: 0x002E622B
		public RoomType[] HandledTypes
		{
			get
			{
				return new RoomType[] { RoomType.Shop };
			}
		}

		// Token: 0x1700264F RID: 9807
		// (get) Token: 0x06008B59 RID: 35673 RVA: 0x002E8037 File Offset: 0x002E6237
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(120L);
			}
		}

		// Token: 0x06008B5A RID: 35674 RVA: 0x002E8044 File Offset: 0x002E6244
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for shop room");
			Node root = ((SceneTree)Engine.GetMainLoop()).Root;
			NMerchantRoom nmerchantRoom = await WaitHelper.ForNode<NMerchantRoom>(root, "/root/Game/RootSceneContainer/Run/RoomContainer/MerchantRoom", ct, null);
			NMerchantRoom room = nmerchantRoom;
			AutoSlayLog.Action("Opening merchant inventory");
			room.OpenInventory();
			await Task.Delay(500, ct);
			int maxAttempts = 50;
			int attempts = 0;
			while (attempts < maxAttempts)
			{
				ct.ThrowIfCancellationRequested();
				attempts++;
				List<NMerchantSlot> list = (from slot in room.Inventory.GetAllSlots()
					where !(slot is NMerchantCardRemoval)
					where slot.Entry.IsStocked && slot.Entry.EnoughGold
					select slot).ToList<NMerchantSlot>();
				if (list.Count == 0)
				{
					AutoSlayLog.Action("No more affordable items to buy");
					break;
				}
				NMerchantSlot nmerchantSlot = random.NextItem<NMerchantSlot>(list);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Buying item (cost: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(nmerchantSlot.Entry.Cost);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
				await nmerchantSlot.Entry.OnTryPurchaseWrapper(room.Inventory.Inventory, false);
				await Task.Delay(300, ct);
			}
			NBackButton nbackButton = UiHelper.FindFirst<NBackButton>(room);
			if (nbackButton != null)
			{
				AutoSlayLog.Action("Closing inventory");
				await UiHelper.Click(nbackButton, 100);
				await Task.Delay(300, ct);
			}
			NProceedButton proceedButton = room.ProceedButton;
			AutoSlayLog.Action("Clicking proceed");
			await UiHelper.Click(proceedButton, 100);
		}

		// Token: 0x04003045 RID: 12357
		private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/MerchantRoom";
	}
}
