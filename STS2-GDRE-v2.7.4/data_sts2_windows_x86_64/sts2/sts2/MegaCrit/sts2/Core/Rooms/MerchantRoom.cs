using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012D RID: 301
	[NullableContext(1)]
	[Nullable(0)]
	public class MerchantRoom : AbstractRoom
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0005C1E1 File Offset: 0x0005A3E1
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Shop;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0005C1E4 File Offset: 0x0005A3E4
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x0005C1EC File Offset: 0x0005A3EC
		public List<MerchantInventory> Inventories { get; private set; } = new List<MerchantInventory>();

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0005C1F5 File Offset: 0x0005A3F5
		[Nullable(2)]
		public override ModelId ModelId
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0005C1F8 File Offset: 0x0005A3F8
		public static MerchantDialogueSet Dialogue
		{
			get
			{
				if (MerchantRoom._dialogue != null)
				{
					return MerchantRoom._dialogue;
				}
				LocTable table = LocManager.Instance.GetTable("merchant_room");
				IReadOnlyList<LocString> locStringsWithPrefix = table.GetLocStringsWithPrefix("MERCHANT.talk.");
				MerchantRoom._dialogue = MerchantDialogueSet.CreateFromLocStrings(locStringsWithPrefix);
				return MerchantRoom._dialogue;
			}
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005C240 File Offset: 0x0005A440
		public MerchantInventory GetLocalInventory()
		{
			int playerSlotIndex = this._runState.GetPlayerSlotIndex(LocalContext.GetMe(this._runState));
			return this.Inventories[playerSlotIndex];
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005C270 File Offset: 0x0005A470
		public override async Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			if (isRestoringRoomStackBase)
			{
				throw new InvalidOperationException("MerchantRoom does not support room stack reconstruction.");
			}
			this._runState = runState;
			foreach (Player player in (((runState != null) ? runState.Players : null) ?? Array.Empty<Player>()))
			{
				MerchantInventory merchantInventory = MerchantInventory.CreateForNormalMerchant(player);
				this.Inventories.Add(merchantInventory);
			}
			await PreloadManager.LoadRoomMerchantAssets();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.SetCurrentRoom(NMerchantRoom.Create(this, ((runState != null) ? runState.Players : null) ?? Array.Empty<Player>()));
			}
			if (runState != null)
			{
				await Hook.AfterRoomEntered(runState, this);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005C2C4 File Offset: 0x0005A4C4
		public override Task Exit([Nullable(2)] IRunState runState)
		{
			if (TestMode.IsOn)
			{
				return Task.CompletedTask;
			}
			for (int i = 0; i < this.Inventories.Count; i++)
			{
				MerchantInventory merchantInventory = this.Inventories[i];
				Player player = this._runState.Players[i];
				PlayerMapPointHistoryEntry playerMapPointHistoryEntry;
				if (runState == null)
				{
					playerMapPointHistoryEntry = null;
				}
				else
				{
					MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
					playerMapPointHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.GetEntry(player.NetId) : null);
				}
				PlayerMapPointHistoryEntry playerMapPointHistoryEntry2 = playerMapPointHistoryEntry;
				if (playerMapPointHistoryEntry2 != null)
				{
					foreach (MerchantCardEntry merchantCardEntry in merchantInventory.CharacterCardEntries)
					{
						if (merchantCardEntry.IsStocked)
						{
							playerMapPointHistoryEntry2.CardChoices.Add(new CardChoiceHistoryEntry(merchantCardEntry.CreationResult.Card, false));
						}
					}
					foreach (MerchantCardEntry merchantCardEntry2 in merchantInventory.ColorlessCardEntries)
					{
						if (merchantCardEntry2.IsStocked)
						{
							playerMapPointHistoryEntry2.CardChoices.Add(new CardChoiceHistoryEntry(merchantCardEntry2.CreationResult.Card, false));
						}
					}
					foreach (MerchantRelicEntry merchantRelicEntry in merchantInventory.RelicEntries)
					{
						if (merchantRelicEntry.IsStocked)
						{
							playerMapPointHistoryEntry2.RelicChoices.Add(new ModelChoiceHistoryEntry(merchantRelicEntry.Model.Id, false));
						}
					}
					foreach (MerchantPotionEntry merchantPotionEntry in merchantInventory.PotionEntries)
					{
						if (merchantPotionEntry.IsStocked)
						{
							playerMapPointHistoryEntry2.PotionChoices.Add(new ModelChoiceHistoryEntry(merchantPotionEntry.Model.Id, false));
						}
					}
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0005C4CC File Offset: 0x0005A6CC
		public override Task Resume(AbstractRoom _, [Nullable(2)] IRunState runState)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000549 RID: 1353
		[Nullable(2)]
		private IRunState _runState;

		// Token: 0x0400054A RID: 1354
		[Nullable(2)]
		private static MerchantDialogueSet _dialogue;
	}
}
