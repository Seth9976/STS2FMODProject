using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	// Token: 0x02000C96 RID: 3222
	[NullableContext(1)]
	[Nullable(0)]
	public class MerchantDialogueSet
	{
		/// <summary>
		/// Lines for when the player enters the merchant room.
		/// </summary>
		// Token: 0x170023F4 RID: 9204
		// (get) Token: 0x060083B3 RID: 33715 RVA: 0x002C79C8 File Offset: 0x002C5BC8
		public IReadOnlyList<LocString> WelcomeLines
		{
			get
			{
				return this._welcomeLines;
			}
		}

		/// <summary>
		/// Lines for when the player throws a <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FoulPotion" /> at the merchant.
		/// </summary>
		// Token: 0x170023F5 RID: 9205
		// (get) Token: 0x060083B4 RID: 33716 RVA: 0x002C79D0 File Offset: 0x002C5BD0
		public IReadOnlyList<LocString> FoulPotionLines
		{
			get
			{
				return this._foulPotionLines;
			}
		}

		/// <summary>
		/// Lines for when the player is dead.
		/// Rare, but can happen in multiplayer if one player is dead and the others go to the merchant.
		/// </summary>
		// Token: 0x170023F6 RID: 9206
		// (get) Token: 0x060083B5 RID: 33717 RVA: 0x002C79D8 File Offset: 0x002C5BD8
		public IReadOnlyList<LocString> PlayerDeadLines
		{
			get
			{
				return this._playerDeadLines;
			}
		}

		/// <summary>
		/// Lines for when the player opens the merchant's inventory.
		/// </summary>
		// Token: 0x170023F7 RID: 9207
		// (get) Token: 0x060083B6 RID: 33718 RVA: 0x002C79E0 File Offset: 0x002C5BE0
		public IReadOnlyList<LocString> OpenInventoryLines
		{
			get
			{
				return this._openInventoryLines;
			}
		}

		// Token: 0x060083B7 RID: 33719 RVA: 0x002C79E8 File Offset: 0x002C5BE8
		public static MerchantDialogueSet CreateFromLocStrings(IEnumerable<LocString> locStrings)
		{
			MerchantDialogueSet merchantDialogueSet = new MerchantDialogueSet();
			foreach (LocString locString in locStrings)
			{
				string[] array = locString.LocEntryKey.Split('.', StringSplitOptions.None);
				string text = array[array.Length - 2];
				if (text != null)
				{
					int length = text.Length;
					List<LocString> list;
					if (length <= 13)
					{
						if (length != 7)
						{
							if (length != 10)
							{
								if (length != 13)
								{
									goto IL_017A;
								}
								if (!(text == "openInventory"))
								{
									goto IL_017A;
								}
								list = merchantDialogueSet._openInventoryLines;
							}
							else
							{
								char c = text[0];
								if (c != 'f')
								{
									if (c != 'p')
									{
										goto IL_017A;
									}
									if (!(text == "playerDead"))
									{
										goto IL_017A;
									}
									list = merchantDialogueSet._playerDeadLines;
								}
								else
								{
									if (!(text == "foulPotion"))
									{
										goto IL_017A;
									}
									list = merchantDialogueSet._foulPotionLines;
								}
							}
						}
						else
						{
							if (!(text == "welcome"))
							{
								goto IL_017A;
							}
							list = merchantDialogueSet._welcomeLines;
						}
					}
					else if (length <= 19)
					{
						if (length != 15)
						{
							if (length != 19)
							{
								goto IL_017A;
							}
							if (!(text == "purchaseFailureGold"))
							{
								goto IL_017A;
							}
							list = merchantDialogueSet._purchaseFailureGoldLines;
						}
						else
						{
							if (!(text == "purchaseSuccess"))
							{
								goto IL_017A;
							}
							list = merchantDialogueSet._purchaseSuccessLines;
						}
					}
					else if (length != 20)
					{
						if (length != 24)
						{
							goto IL_017A;
						}
						if (!(text == "purchaseFailureForbidden"))
						{
							goto IL_017A;
						}
						list = merchantDialogueSet._purchaseFailureForbiddenLines;
					}
					else
					{
						if (!(text == "purchaseFailureSpace"))
						{
							goto IL_017A;
						}
						list = merchantDialogueSet._purchaseFailureSpaceLines;
					}
					List<LocString> list2 = list;
					list2.Add(locString);
					continue;
				}
				IL_017A:
				throw new InvalidOperationException("Unexpected merchant dialogue key: " + text);
			}
			return merchantDialogueSet;
		}

		/// <summary>
		/// Get all the valid lines for a given purchase status.
		/// </summary>
		// Token: 0x060083B8 RID: 33720 RVA: 0x002C7BC0 File Offset: 0x002C5DC0
		public IReadOnlyList<LocString> GetPurchaseSuccessLines(PurchaseStatus status)
		{
			List<LocString> list;
			switch (status)
			{
			case PurchaseStatus.Success:
				list = this._purchaseSuccessLines;
				break;
			case PurchaseStatus.FailureGold:
				list = this._purchaseFailureGoldLines;
				break;
			case PurchaseStatus.FailureSpace:
				list = this._purchaseFailureSpaceLines;
				break;
			case PurchaseStatus.FailureForbidden:
				list = this._purchaseFailureForbiddenLines;
				break;
			default:
				throw new ArgumentOutOfRangeException("status", status, null);
			}
			return list;
		}

		// Token: 0x04002CF8 RID: 11512
		private readonly List<LocString> _welcomeLines = new List<LocString>();

		// Token: 0x04002CF9 RID: 11513
		private readonly List<LocString> _openInventoryLines = new List<LocString>();

		// Token: 0x04002CFA RID: 11514
		private readonly List<LocString> _foulPotionLines = new List<LocString>();

		// Token: 0x04002CFB RID: 11515
		private readonly List<LocString> _playerDeadLines = new List<LocString>();

		// Token: 0x04002CFC RID: 11516
		private readonly List<LocString> _purchaseSuccessLines = new List<LocString>();

		// Token: 0x04002CFD RID: 11517
		private readonly List<LocString> _purchaseFailureGoldLines = new List<LocString>();

		// Token: 0x04002CFE RID: 11518
		private readonly List<LocString> _purchaseFailureSpaceLines = new List<LocString>();

		// Token: 0x04002CFF RID: 11519
		private readonly List<LocString> _purchaseFailureForbiddenLines = new List<LocString>();
	}
}
