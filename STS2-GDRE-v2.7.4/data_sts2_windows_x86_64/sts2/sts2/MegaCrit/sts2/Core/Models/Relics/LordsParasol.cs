using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057D RID: 1405
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LordsParasol : RelicModel
	{
		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x0024289F File Offset: 0x00240A9F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x002428A4 File Offset: 0x00240AA4
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			MerchantRoom merchantRoom = room as MerchantRoom;
			if (merchantRoom == null)
			{
				return Task.CompletedTask;
			}
			TaskHelper.RunSafely(this.PurchaseEverything(merchantRoom.GetLocalInventory()));
			return Task.CompletedTask;
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x002428D8 File Offset: 0x00240AD8
		private async Task PurchaseEverything(MerchantInventory inventory)
		{
			if (inventory.Player == base.Owner)
			{
				bool uiBlocked = false;
				try
				{
					if (TestMode.IsOff)
					{
						NRun instance = NRun.Instance;
						if (instance != null)
						{
							instance.GlobalUi.TopBar.Map.Disable();
						}
						NRun instance2 = NRun.Instance;
						if (instance2 != null)
						{
							instance2.GlobalUi.TopBar.Deck.Disable();
						}
						NMapScreen instance3 = NMapScreen.Instance;
						if (instance3 != null)
						{
							instance3.SetTravelEnabled(false);
						}
						if (NRun.Instance != null)
						{
							await NRun.Instance.AwaitProcessFrame(default(CancellationToken));
						}
						uiBlocked = true;
						NMerchantRoom instance4 = NMerchantRoom.Instance;
						if (instance4 != null)
						{
							instance4.Inventory.BlockInput();
						}
						await Cmd.Wait(0.75f, false);
						NMerchantRoom instance5 = NMerchantRoom.Instance;
						if (instance5 != null)
						{
							instance5.Inventory.Open();
						}
						await Cmd.Wait(1f, false);
					}
					foreach (MerchantCardEntry merchantCardEntry in inventory.CharacterCardEntries)
					{
						if (!merchantCardEntry.IsStocked)
						{
							CardCreationResult creationResult = merchantCardEntry.CreationResult;
							SentryService.CaptureMessage("LordsParasol tried to buy an out-of-stock character card: " + (((creationResult != null) ? creationResult.Card.Id.Entry : null) ?? "NULL"), 1, null);
						}
						else
						{
							await merchantCardEntry.OnTryPurchaseWrapper(inventory, true);
							await Cmd.Wait(0.25f, false);
						}
					}
					IEnumerator<MerchantCardEntry> enumerator = null;
					foreach (MerchantCardEntry merchantCardEntry2 in inventory.ColorlessCardEntries)
					{
						if (!merchantCardEntry2.IsStocked)
						{
							CardCreationResult creationResult2 = merchantCardEntry2.CreationResult;
							SentryService.CaptureMessage("LordsParasol tried to buy an out-of-stock colorless card: " + (((creationResult2 != null) ? creationResult2.Card.Id.Entry : null) ?? "NULL"), 1, null);
						}
						else
						{
							await merchantCardEntry2.OnTryPurchaseWrapper(inventory, true);
							await Cmd.Wait(0.25f, false);
						}
					}
					enumerator = null;
					foreach (MerchantRelicEntry merchantRelicEntry in inventory.RelicEntries)
					{
						NRun instance6 = NRun.Instance;
						if (instance6 != null)
						{
							instance6.GlobalUi.TopBar.Map.Enable();
						}
						NRun instance7 = NRun.Instance;
						if (instance7 != null)
						{
							instance7.GlobalUi.TopBar.Deck.Enable();
						}
						await merchantRelicEntry.OnTryPurchaseWrapper(inventory, true);
						NRun instance8 = NRun.Instance;
						if (instance8 != null)
						{
							instance8.GlobalUi.TopBar.Deck.Disable();
						}
						NRun instance9 = NRun.Instance;
						if (instance9 != null)
						{
							instance9.GlobalUi.TopBar.Map.Disable();
						}
						await Cmd.Wait(0.25f, false);
					}
					IEnumerator<MerchantRelicEntry> enumerator2 = null;
					foreach (MerchantPotionEntry merchantPotionEntry in inventory.PotionEntries)
					{
						await merchantPotionEntry.OnTryPurchaseWrapper(inventory, true);
						await Cmd.Wait(0.25f, false);
					}
					IEnumerator<MerchantPotionEntry> enumerator3 = null;
				}
				finally
				{
					if (uiBlocked)
					{
						NMerchantRoom instance10 = NMerchantRoom.Instance;
						if (instance10 != null)
						{
							instance10.Inventory.UnblockInput();
						}
						NRun instance11 = NRun.Instance;
						if (instance11 != null)
						{
							instance11.GlobalUi.TopBar.Map.Enable();
						}
						NRun instance12 = NRun.Instance;
						if (instance12 != null)
						{
							instance12.GlobalUi.TopBar.Deck.Enable();
						}
						NMapScreen instance13 = NMapScreen.Instance;
						if (instance13 != null)
						{
							instance13.SetTravelEnabled(true);
						}
					}
				}
				if (inventory.CardRemovalEntry != null && RunManager.Instance.IsInProgress)
				{
					NMapScreen instance14 = NMapScreen.Instance;
					if (instance14 != null)
					{
						instance14.SetTravelEnabled(false);
					}
					await inventory.CardRemovalEntry.OnTryPurchaseWrapper(inventory, true, false);
					NMapScreen instance15 = NMapScreen.Instance;
					if (instance15 != null)
					{
						instance15.SetTravelEnabled(true);
					}
				}
			}
		}
	}
}
