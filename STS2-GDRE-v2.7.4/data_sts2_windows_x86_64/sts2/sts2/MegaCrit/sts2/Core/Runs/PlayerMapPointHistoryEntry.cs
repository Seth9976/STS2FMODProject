using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Tracks the current stats and changes experienced for a specific player at a single room.
	/// </summary>
	// Token: 0x0200010A RID: 266
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerMapPointHistoryEntry : IPacketSerializable
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0004CAC1 File Offset: 0x0004ACC1
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x0004CAC9 File Offset: 0x0004ACC9
		[JsonPropertyName("player_id")]
		public ulong PlayerId { get; set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0004CAD2 File Offset: 0x0004ACD2
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0004CADA File Offset: 0x0004ACDA
		[JsonPropertyName("gold_gained")]
		public int GoldGained { get; set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0004CAE3 File Offset: 0x0004ACE3
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0004CAEB File Offset: 0x0004ACEB
		[JsonPropertyName("gold_spent")]
		public int GoldSpent { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0004CAF4 File Offset: 0x0004ACF4
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0004CAFC File Offset: 0x0004ACFC
		[JsonPropertyName("gold_lost")]
		public int GoldLost { get; set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0004CB05 File Offset: 0x0004AD05
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x0004CB0D File Offset: 0x0004AD0D
		[JsonPropertyName("gold_stolen")]
		public int GoldStolen { get; set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0004CB16 File Offset: 0x0004AD16
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x0004CB1E File Offset: 0x0004AD1E
		[JsonPropertyName("stolen_loot")]
		public int StolenLoot { get; set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0004CB27 File Offset: 0x0004AD27
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x0004CB2F File Offset: 0x0004AD2F
		[JsonPropertyName("current_gold")]
		public int CurrentGold { get; set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0004CB38 File Offset: 0x0004AD38
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0004CB40 File Offset: 0x0004AD40
		[JsonPropertyName("current_hp")]
		public int CurrentHp { get; set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0004CB49 File Offset: 0x0004AD49
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x0004CB51 File Offset: 0x0004AD51
		[JsonPropertyName("max_hp")]
		public int MaxHp { get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0004CB5A File Offset: 0x0004AD5A
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x0004CB62 File Offset: 0x0004AD62
		[JsonPropertyName("damage_taken")]
		public int DamageTaken { get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0004CB6B File Offset: 0x0004AD6B
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x0004CB73 File Offset: 0x0004AD73
		[JsonPropertyName("hp_healed")]
		public int HpHealed { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0004CB7C File Offset: 0x0004AD7C
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0004CB84 File Offset: 0x0004AD84
		[JsonPropertyName("max_hp_lost")]
		public int MaxHpLost { get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0004CB8D File Offset: 0x0004AD8D
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0004CB95 File Offset: 0x0004AD95
		[JsonPropertyName("max_hp_gained")]
		public int MaxHpGained { get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0004CB9E File Offset: 0x0004AD9E
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0004CBA6 File Offset: 0x0004ADA6
		[JsonPropertyName("ancient_choice")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<AncientChoiceHistoryEntry> AncientChoices { get; set; } = new List<AncientChoiceHistoryEntry>();

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0004CBAF File Offset: 0x0004ADAF
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0004CBB7 File Offset: 0x0004ADB7
		[JsonPropertyName("cards_gained")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializableCard> CardsGained { get; set; } = new List<SerializableCard>();

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0004CBC0 File Offset: 0x0004ADC0
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0004CBC8 File Offset: 0x0004ADC8
		[JsonPropertyName("card_choices")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<CardChoiceHistoryEntry> CardChoices { get; set; } = new List<CardChoiceHistoryEntry>();

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0004CBD1 File Offset: 0x0004ADD1
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x0004CBD9 File Offset: 0x0004ADD9
		[JsonPropertyName("relic_choices")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelChoiceHistoryEntry> RelicChoices { get; set; } = new List<ModelChoiceHistoryEntry>();

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0004CBE2 File Offset: 0x0004ADE2
		// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x0004CBEA File Offset: 0x0004ADEA
		[JsonPropertyName("potion_choices")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelChoiceHistoryEntry> PotionChoices { get; set; } = new List<ModelChoiceHistoryEntry>();

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0004CBF3 File Offset: 0x0004ADF3
		// (set) Token: 0x06000BC8 RID: 3016 RVA: 0x0004CBFB File Offset: 0x0004ADFB
		[JsonPropertyName("potion_discarded")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> PotionDiscarded { get; set; } = new List<ModelId>();

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0004CC04 File Offset: 0x0004AE04
		// (set) Token: 0x06000BCA RID: 3018 RVA: 0x0004CC0C File Offset: 0x0004AE0C
		[JsonPropertyName("potion_used")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> PotionUsed { get; set; } = new List<ModelId>();

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x0004CC15 File Offset: 0x0004AE15
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x0004CC1D File Offset: 0x0004AE1D
		[JsonPropertyName("cards_removed")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializableCard> CardsRemoved { get; set; } = new List<SerializableCard>();

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0004CC26 File Offset: 0x0004AE26
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0004CC2E File Offset: 0x0004AE2E
		[JsonPropertyName("relics_removed")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> RelicsRemoved { get; set; } = new List<ModelId>();

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0004CC37 File Offset: 0x0004AE37
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x0004CC3F File Offset: 0x0004AE3F
		[JsonPropertyName("cards_enchanted")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<CardEnchantmentHistoryEntry> CardsEnchanted { get; set; } = new List<CardEnchantmentHistoryEntry>();

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0004CC48 File Offset: 0x0004AE48
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x0004CC50 File Offset: 0x0004AE50
		[JsonPropertyName("cards_transformed")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<CardTransformationHistoryEntry> CardsTransformed { get; set; } = new List<CardTransformationHistoryEntry>();

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0004CC59 File Offset: 0x0004AE59
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x0004CC61 File Offset: 0x0004AE61
		[JsonPropertyName("upgraded_cards")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> UpgradedCards { get; set; } = new List<ModelId>();

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0004CC6A File Offset: 0x0004AE6A
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x0004CC72 File Offset: 0x0004AE72
		[JsonPropertyName("downgraded_cards")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> DowngradedCards { get; set; } = new List<ModelId>();

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0004CC7B File Offset: 0x0004AE7B
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x0004CC83 File Offset: 0x0004AE83
		[JsonPropertyName("event_choices")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<EventOptionHistoryEntry> EventChoices { get; set; } = new List<EventOptionHistoryEntry>();

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0004CC8C File Offset: 0x0004AE8C
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x0004CC94 File Offset: 0x0004AE94
		[JsonPropertyName("rest_site_choices")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<string> RestSiteChoices { get; set; } = new List<string>();

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0004CC9D File Offset: 0x0004AE9D
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x0004CCA5 File Offset: 0x0004AEA5
		[JsonPropertyName("bought_relics")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> BoughtRelics { get; set; } = new List<ModelId>();

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0004CCAE File Offset: 0x0004AEAE
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0004CCB6 File Offset: 0x0004AEB6
		[JsonPropertyName("bought_potions")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> BoughtPotions { get; set; } = new List<ModelId>();

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0004CCBF File Offset: 0x0004AEBF
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x0004CCC7 File Offset: 0x0004AEC7
		[JsonPropertyName("bought_colorless")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> BoughtColorless { get; set; } = new List<ModelId>();

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0004CCD0 File Offset: 0x0004AED0
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x0004CCD8 File Offset: 0x0004AED8
		[JsonPropertyName("completed_quests")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> CompletedQuests { get; set; } = new List<ModelId>();

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0004CCE1 File Offset: 0x0004AEE1
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x0004CCE9 File Offset: 0x0004AEE9
		[JsonPropertyName("is_affected_by_fur_coat")]
		public bool IsAffectedByFurCoat { get; set; }

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0004CCF2 File Offset: 0x0004AEF2
		[NullableContext(2)]
		public LocString GetAncientPickedChoiceLoc()
		{
			AncientChoiceHistoryEntry ancientChoiceHistoryEntry = this.AncientChoices.FirstOrDefault<AncientChoiceHistoryEntry>((AncientChoiceHistoryEntry o) => o.WasChosen);
			if (ancientChoiceHistoryEntry == null)
			{
				return null;
			}
			return ancientChoiceHistoryEntry.Title;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0004CD2C File Offset: 0x0004AF2C
		public List<LocString> GetAncientSkippedChoiceLoc()
		{
			return (from o in this.AncientChoices
				where !o.WasChosen
				select o.Title).ToList<LocString>();
		}

		/// <summary>
		/// True if a thief took gold or cards from this player this combat and did not give them back.
		/// </summary>
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0004CD8C File Offset: 0x0004AF8C
		[JsonIgnore]
		public bool WasMugged
		{
			get
			{
				return this.StolenLoot > 0;
			}
		}

		/// <summary>Records that loot was stolen from this player (gold by amount, or 1 per card).</summary>
		// Token: 0x06000BE8 RID: 3048 RVA: 0x0004CD97 File Offset: 0x0004AF97
		public void MarkLootStolen(int amount = 1)
		{
			this.StolenLoot += amount;
		}

		/// <summary>Records that previously stolen loot was returned to this player.</summary>
		// Token: 0x06000BE9 RID: 3049 RVA: 0x0004CDA7 File Offset: 0x0004AFA7
		public void MarkLootReturned(int amount = 1)
		{
			this.StolenLoot = Math.Max(0, this.StolenLoot - amount);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0004CDC0 File Offset: 0x0004AFC0
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.PlayerId, 64);
			writer.WriteInt(this.GoldGained, 32);
			writer.WriteInt(this.GoldSpent, 32);
			writer.WriteInt(this.GoldLost, 32);
			writer.WriteInt(this.GoldStolen, 32);
			writer.WriteInt(this.StolenLoot, 32);
			writer.WriteInt(this.CurrentGold, 32);
			writer.WriteInt(this.CurrentHp, 32);
			writer.WriteInt(this.MaxHp, 32);
			writer.WriteInt(this.DamageTaken, 32);
			writer.WriteInt(this.HpHealed, 32);
			writer.WriteInt(this.MaxHpGained, 32);
			writer.WriteInt(this.MaxHpLost, 32);
			writer.WriteList<EventOptionHistoryEntry>(this.EventChoices, 32);
			writer.WriteList<AncientChoiceHistoryEntry>(this.AncientChoices, 32);
			writer.WriteList<SerializableCard>(this.CardsGained, 32);
			writer.WriteInt(this.CardChoices.Count, 32);
			foreach (CardChoiceHistoryEntry cardChoiceHistoryEntry in this.CardChoices)
			{
				cardChoiceHistoryEntry.Serialize<CardModel>(writer);
			}
			writer.WriteInt(this.RelicChoices.Count, 32);
			foreach (ModelChoiceHistoryEntry modelChoiceHistoryEntry in this.RelicChoices)
			{
				modelChoiceHistoryEntry.Serialize<RelicModel>(writer);
			}
			writer.WriteInt(this.PotionChoices.Count, 32);
			foreach (ModelChoiceHistoryEntry modelChoiceHistoryEntry2 in this.PotionChoices)
			{
				modelChoiceHistoryEntry2.Serialize<PotionModel>(writer);
			}
			writer.WriteModelEntriesInList(this.PotionDiscarded);
			writer.WriteModelEntriesInList(this.PotionUsed);
			writer.WriteList<SerializableCard>(this.CardsRemoved, 32);
			writer.WriteModelEntriesInList(this.RelicsRemoved);
			writer.WriteList<CardEnchantmentHistoryEntry>(this.CardsEnchanted, 32);
			writer.WriteList<CardTransformationHistoryEntry>(this.CardsTransformed, 32);
			writer.WriteModelEntriesInList(this.UpgradedCards);
			writer.WriteModelEntriesInList(this.DowngradedCards);
			writer.WriteList<EventOptionHistoryEntry>(this.EventChoices, 32);
			writer.WriteInt(this.RestSiteChoices.Count, 32);
			foreach (string text in this.RestSiteChoices)
			{
				writer.WriteString(text);
			}
			writer.WriteModelEntriesInList(this.BoughtRelics);
			writer.WriteModelEntriesInList(this.BoughtPotions);
			writer.WriteModelEntriesInList(this.BoughtColorless);
			writer.WriteModelEntriesInList(this.CompletedQuests);
			writer.WriteBool(this.IsAffectedByFurCoat);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0004D0C0 File Offset: 0x0004B2C0
		public void Deserialize(PacketReader reader)
		{
			this.PlayerId = reader.ReadULong(64);
			this.GoldGained = reader.ReadInt(32);
			this.GoldSpent = reader.ReadInt(32);
			this.GoldLost = reader.ReadInt(32);
			this.GoldStolen = reader.ReadInt(32);
			this.StolenLoot = reader.ReadInt(32);
			this.CurrentGold = reader.ReadInt(32);
			this.CurrentHp = reader.ReadInt(32);
			this.MaxHp = reader.ReadInt(32);
			this.DamageTaken = reader.ReadInt(32);
			this.HpHealed = reader.ReadInt(32);
			this.MaxHpGained = reader.ReadInt(32);
			this.MaxHpLost = reader.ReadInt(32);
			this.EventChoices = reader.ReadList<EventOptionHistoryEntry>(32);
			this.AncientChoices = reader.ReadList<AncientChoiceHistoryEntry>(32);
			this.CardsGained = reader.ReadList<SerializableCard>(32);
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				CardChoiceHistoryEntry cardChoiceHistoryEntry = default(CardChoiceHistoryEntry);
				cardChoiceHistoryEntry.Deserialize<CardModel>(reader);
				this.CardChoices.Add(cardChoiceHistoryEntry);
			}
			int num2 = reader.ReadInt(32);
			for (int j = 0; j < num2; j++)
			{
				ModelChoiceHistoryEntry modelChoiceHistoryEntry = default(ModelChoiceHistoryEntry);
				modelChoiceHistoryEntry.Deserialize<RelicModel>(reader);
				this.RelicChoices.Add(modelChoiceHistoryEntry);
			}
			int num3 = reader.ReadInt(32);
			for (int k = 0; k < num3; k++)
			{
				ModelChoiceHistoryEntry modelChoiceHistoryEntry2 = default(ModelChoiceHistoryEntry);
				modelChoiceHistoryEntry2.Deserialize<PotionModel>(reader);
				this.PotionChoices.Add(modelChoiceHistoryEntry2);
			}
			this.PotionDiscarded = reader.ReadModelIdListAssumingType<PotionModel>();
			this.PotionUsed = reader.ReadModelIdListAssumingType<PotionModel>();
			this.CardsRemoved = reader.ReadList<SerializableCard>(32);
			this.RelicsRemoved = reader.ReadModelIdListAssumingType<RelicModel>();
			this.CardsEnchanted = reader.ReadList<CardEnchantmentHistoryEntry>(32);
			this.CardsTransformed = reader.ReadList<CardTransformationHistoryEntry>(32);
			this.UpgradedCards = reader.ReadModelIdListAssumingType<CardModel>();
			this.DowngradedCards = reader.ReadModelIdListAssumingType<CardModel>();
			this.EventChoices = reader.ReadList<EventOptionHistoryEntry>(32);
			int num4 = reader.ReadInt(32);
			for (int l = 0; l < num4; l++)
			{
				this.RestSiteChoices.Add(reader.ReadString());
			}
			this.BoughtRelics = reader.ReadModelIdListAssumingType<RelicModel>();
			this.BoughtPotions = reader.ReadModelIdListAssumingType<PotionModel>();
			this.BoughtColorless = reader.ReadModelIdListAssumingType<CardModel>();
			this.CompletedQuests = reader.ReadModelIdListAssumingType<CardModel>();
			this.IsAffectedByFurCoat = reader.ReadBool();
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0004D32C File Offset: 0x0004B52C
		public PlayerMapPointHistoryEntry Anonymized()
		{
			return new PlayerMapPointHistoryEntry
			{
				PlayerId = IdAnonymizer.Anonymize(this.PlayerId),
				GoldGained = this.GoldGained,
				GoldSpent = this.GoldSpent,
				GoldLost = this.GoldLost,
				GoldStolen = this.GoldStolen,
				StolenLoot = this.StolenLoot,
				CurrentGold = this.CurrentGold,
				CurrentHp = this.CurrentHp,
				MaxHp = this.MaxHp,
				DamageTaken = this.DamageTaken,
				HpHealed = this.HpHealed,
				MaxHpLost = this.MaxHpLost,
				MaxHpGained = this.MaxHpGained,
				AncientChoices = this.AncientChoices,
				CardsGained = this.CardsGained,
				CardChoices = this.CardChoices,
				RelicChoices = this.RelicChoices,
				PotionChoices = this.PotionChoices,
				PotionDiscarded = this.PotionDiscarded,
				PotionUsed = this.PotionUsed,
				CardsRemoved = this.CardsRemoved,
				RelicsRemoved = this.RelicsRemoved,
				CardsEnchanted = this.CardsEnchanted,
				CardsTransformed = this.CardsTransformed,
				UpgradedCards = this.UpgradedCards,
				DowngradedCards = this.DowngradedCards,
				EventChoices = this.EventChoices,
				RestSiteChoices = this.RestSiteChoices,
				BoughtRelics = this.BoughtRelics,
				BoughtPotions = this.BoughtPotions,
				BoughtColorless = this.BoughtColorless,
				CompletedQuests = this.CompletedQuests
			};
		}
	}
}
