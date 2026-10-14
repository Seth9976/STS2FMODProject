using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	// Token: 0x02000C98 RID: 3224
	[NullableContext(1)]
	[Nullable(0)]
	public class MerchantInventory
	{
		// Token: 0x170023FB RID: 9211
		// (get) Token: 0x060083CD RID: 33741 RVA: 0x002C7EEB File Offset: 0x002C60EB
		public IReadOnlyList<MerchantCardEntry> CharacterCardEntries
		{
			get
			{
				return this._characterCardEntries;
			}
		}

		// Token: 0x170023FC RID: 9212
		// (get) Token: 0x060083CE RID: 33742 RVA: 0x002C7EF3 File Offset: 0x002C60F3
		public IReadOnlyList<MerchantCardEntry> ColorlessCardEntries
		{
			get
			{
				return this._colorlessCardEntries;
			}
		}

		// Token: 0x170023FD RID: 9213
		// (get) Token: 0x060083CF RID: 33743 RVA: 0x002C7EFB File Offset: 0x002C60FB
		public IReadOnlyList<MerchantRelicEntry> RelicEntries
		{
			get
			{
				return this._relicEntries;
			}
		}

		// Token: 0x170023FE RID: 9214
		// (get) Token: 0x060083D0 RID: 33744 RVA: 0x002C7F03 File Offset: 0x002C6103
		public IReadOnlyList<MerchantPotionEntry> PotionEntries
		{
			get
			{
				return this._potionEntries;
			}
		}

		// Token: 0x170023FF RID: 9215
		// (get) Token: 0x060083D1 RID: 33745 RVA: 0x002C7F0B File Offset: 0x002C610B
		// (set) Token: 0x060083D2 RID: 33746 RVA: 0x002C7F13 File Offset: 0x002C6113
		[Nullable(2)]
		public MerchantCardRemovalEntry CardRemovalEntry
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17002400 RID: 9216
		// (get) Token: 0x060083D3 RID: 33747 RVA: 0x002C7F1C File Offset: 0x002C611C
		public Player Player { get; }

		// Token: 0x17002401 RID: 9217
		// (get) Token: 0x060083D4 RID: 33748 RVA: 0x002C7F24 File Offset: 0x002C6124
		public IEnumerable<MerchantEntry> AllEntries
		{
			get
			{
				IEnumerable<MerchantEntry>[] array = new IEnumerable<MerchantEntry>[4];
				array[0] = this.CardEntries;
				array[1] = this.RelicEntries;
				array[2] = this.PotionEntries;
				int num = 3;
				IEnumerable<MerchantEntry> enumerable2;
				if (this.CardRemovalEntry == null)
				{
					IEnumerable<MerchantEntry> enumerable = Array.Empty<MerchantEntry>();
					enumerable2 = enumerable;
				}
				else
				{
					IEnumerable<MerchantEntry> enumerable = new <>z__ReadOnlySingleElementList<MerchantEntry>(this.CardRemovalEntry);
					enumerable2 = enumerable;
				}
				array[num] = enumerable2;
				return array.SelectMany<IEnumerable<MerchantEntry>, MerchantEntry>((IEnumerable<MerchantEntry> e) => e);
			}
		}

		// Token: 0x17002402 RID: 9218
		// (get) Token: 0x060083D5 RID: 33749 RVA: 0x002C7F97 File Offset: 0x002C6197
		public IEnumerable<MerchantCardEntry> CardEntries
		{
			get
			{
				return this.CharacterCardEntries.Concat<MerchantCardEntry>(this.ColorlessCardEntries);
			}
		}

		// Token: 0x060083D6 RID: 33750 RVA: 0x002C7FAA File Offset: 0x002C61AA
		public MerchantInventory(Player player)
		{
			this.Player = player;
		}

		// Token: 0x060083D7 RID: 33751 RVA: 0x002C7FE8 File Offset: 0x002C61E8
		public static MerchantInventory CreateForNormalMerchant(Player player)
		{
			MerchantInventory merchantInventory = new MerchantInventory(player);
			merchantInventory.PopulateCharacterCardEntries();
			merchantInventory.PopulateColorlessCardEntries();
			merchantInventory.PopulateRelicEntries();
			merchantInventory.PopulatePotionEntries();
			merchantInventory.CardRemovalEntry = new MerchantCardRemovalEntry(player);
			foreach (MerchantEntry merchantEntry in merchantInventory.AllEntries)
			{
				merchantEntry.PurchaseCompleted += merchantInventory.UpdateEntries;
			}
			return merchantInventory;
		}

		// Token: 0x060083D8 RID: 33752 RVA: 0x002C806C File Offset: 0x002C626C
		public void AddRelicEntry(MerchantRelicEntry entry)
		{
			this._relicEntries.Add(entry);
		}

		// Token: 0x060083D9 RID: 33753 RVA: 0x002C807C File Offset: 0x002C627C
		private void PopulateCharacterCardEntries()
		{
			int num = this.Player.PlayerRng.Shops.NextInt(MerchantInventory._coloredCardTypes.Length);
			List<CardModel> list = this.Player.Character.CardPool.GetUnlockedCards(this.Player.UnlockState, this.Player.RunState.CardMultiplayerConstraint).ToList<CardModel>();
			for (int i = 0; i < MerchantInventory._coloredCardTypes.Length; i++)
			{
				MerchantCardEntry merchantCardEntry = new MerchantCardEntry(this.Player, this, list, MerchantInventory._coloredCardTypes[i]);
				merchantCardEntry.Populate();
				this._characterCardEntries.Add(merchantCardEntry);
				if (num == i)
				{
					merchantCardEntry.SetOnSale();
				}
			}
		}

		// Token: 0x060083DA RID: 33754 RVA: 0x002C8120 File Offset: 0x002C6320
		private void PopulateColorlessCardEntries()
		{
			List<CardModel> list = ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(this.Player.UnlockState, this.Player.RunState.CardMultiplayerConstraint).ToList<CardModel>();
			foreach (CardRarity cardRarity in MerchantInventory._colorlessCardRarities)
			{
				MerchantCardEntry merchantCardEntry = new MerchantCardEntry(this.Player, this, list, cardRarity);
				merchantCardEntry.Populate();
				this._colorlessCardEntries.Add(merchantCardEntry);
			}
		}

		// Token: 0x060083DB RID: 33755 RVA: 0x002C8194 File Offset: 0x002C6394
		private void PopulateRelicEntries()
		{
			RelicRarity[] array = new RelicRarity[]
			{
				RelicFactory.RollRarity(this.Player),
				RelicFactory.RollRarity(this.Player),
				RelicRarity.Shop
			};
			foreach (RelicRarity relicRarity in array)
			{
				this.AddRelicEntry(new MerchantRelicEntry(relicRarity, this.Player));
			}
		}

		// Token: 0x060083DC RID: 33756 RVA: 0x002C81F0 File Offset: 0x002C63F0
		private void PopulatePotionEntries()
		{
			List<PotionModel> list = PotionFactory.CreateRandomPotionsOutOfCombat(this.Player, 3, this.Player.PlayerRng.Shops, null).ToList<PotionModel>();
			foreach (PotionModel potionModel in list)
			{
				this._potionEntries.Add(new MerchantPotionEntry(potionModel.ToMutable(), this.Player));
			}
		}

		// Token: 0x060083DD RID: 33757 RVA: 0x002C8278 File Offset: 0x002C6478
		private void UpdateEntries(PurchaseStatus _, MerchantEntry __)
		{
			foreach (MerchantEntry merchantEntry in this.AllEntries)
			{
				merchantEntry.OnMerchantInventoryUpdated();
			}
		}

		// Token: 0x04002D05 RID: 11525
		private static readonly CardType[] _coloredCardTypes = new CardType[]
		{
			CardType.Attack,
			CardType.Attack,
			CardType.Skill,
			CardType.Skill,
			CardType.Power
		};

		// Token: 0x04002D06 RID: 11526
		private static readonly CardRarity[] _colorlessCardRarities = new CardRarity[]
		{
			CardRarity.Uncommon,
			CardRarity.Rare
		};

		// Token: 0x04002D07 RID: 11527
		private readonly List<MerchantCardEntry> _characterCardEntries = new List<MerchantCardEntry>();

		// Token: 0x04002D08 RID: 11528
		private readonly List<MerchantCardEntry> _colorlessCardEntries = new List<MerchantCardEntry>();

		// Token: 0x04002D09 RID: 11529
		private readonly List<MerchantRelicEntry> _relicEntries = new List<MerchantRelicEntry>();

		// Token: 0x04002D0A RID: 11530
		private readonly List<MerchantPotionEntry> _potionEntries = new List<MerchantPotionEntry>();
	}
}
