using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CF RID: 207
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableReward : IPacketSerializable
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00044FA0 File Offset: 0x000431A0
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00044FA8 File Offset: 0x000431A8
		[JsonPropertyName("reward_type")]
		public RewardType RewardType { get; set; }

		/// <summary>
		/// The model ID that this reward is predetermined to be.
		/// This is used for rewards that are predetermined to be a specific model, such as
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.FakeMerchantEventEncounter" /> granting the specific relics that were originally being sold by
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" />.
		///
		/// Note: If you want to set a pre-determined CardModel reward, you should probably use
		/// <see cref="T:MegaCrit.Sts2.Core.Rewards.SpecialCardReward" /> and the <see cref="P:MegaCrit.Sts2.Core.Saves.Runs.SerializableReward.SpecialCard" /> property instead.
		///
		/// TODO: This only supports RelicModel right now. Add support for PotionModel later if necessary.
		/// </summary>
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00044FB1 File Offset: 0x000431B1
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x00044FB9 File Offset: 0x000431B9
		[JsonPropertyName("predetermined_model_id")]
		public ModelId PredeterminedModelId { get; set; } = ModelId.none;

		/// <summary>
		/// The card for a <see cref="T:MegaCrit.Sts2.Core.Rewards.SpecialCardReward" />.
		/// This has to be a full serialized card instead of just an ID, because <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper" /> steals a card
		/// from you and then returns it via this reward. This card may have modifications (upgrade, enchantment, etc.), so
		/// we need to be able to fully reconstruct it.
		/// Only non-null when RewardType is SpecialCard.
		/// </summary>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00044FC2 File Offset: 0x000431C2
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x00044FCA File Offset: 0x000431CA
		[Nullable(2)]
		[JsonPropertyName("special_card")]
		public SerializableCard SpecialCard
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// These properties are only used for gold rewards.
		/// </summary>
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00044FD3 File Offset: 0x000431D3
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x00044FDB File Offset: 0x000431DB
		[JsonPropertyName("gold_amount")]
		public int GoldAmount { get; set; }

		/// <summary>
		/// Whether or not the gold was "stolen back".
		/// Usually false, but true in situations where an enemy/event "stole" some of your gold (such as
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.GremlinMercNormal" />), and this reward represents you taking it back.
		/// </summary>
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00044FE4 File Offset: 0x000431E4
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x00044FEC File Offset: 0x000431EC
		[JsonPropertyName("was_gold_stolen_back")]
		public bool WasGoldStolenBack { get; set; }

		/// <summary>
		/// These properties are only used for randomized card rewards.
		/// </summary>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00044FF5 File Offset: 0x000431F5
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x00044FFD File Offset: 0x000431FD
		[JsonPropertyName("source")]
		public CardCreationSource Source { get; set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00045006 File Offset: 0x00043206
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0004500E File Offset: 0x0004320E
		[JsonPropertyName("rarity_odds")]
		public CardRarityOddsType RarityOdds { get; set; }

		/// <summary>
		/// The IDs of the card pools that this card reward can be rolled from.
		/// </summary>
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x00045017 File Offset: 0x00043217
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0004501F File Offset: 0x0004321F
		[JsonPropertyName("card_pools")]
		public List<ModelId> CardPoolIds { get; set; } = new List<ModelId>();

		/// <summary>
		/// The number of cards that should be available to choose from in this card reward.
		/// </summary>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00045028 File Offset: 0x00043228
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x00045030 File Offset: 0x00043230
		[JsonPropertyName("option_count")]
		public int OptionCount { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Models.EncounterModel.CustomRewardDescription" /> for details.
		/// </summary>
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00045039 File Offset: 0x00043239
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x00045041 File Offset: 0x00043241
		[JsonPropertyName("custom_description_encounter_source_id")]
		public ModelId CustomDescriptionEncounterSourceId { get; set; } = ModelId.none;

		// Token: 0x060009F5 RID: 2549 RVA: 0x0004504C File Offset: 0x0004324C
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt((int)this.RewardType, 32);
			writer.WriteFullModelId(this.PredeterminedModelId);
			if (this.RewardType == RewardType.SpecialCard)
			{
				writer.Write<SerializableCard>(this.SpecialCard);
			}
			writer.WriteInt(this.GoldAmount, 32);
			writer.WriteBool(this.WasGoldStolenBack);
			writer.WriteEnum<CardCreationSource>(this.Source);
			writer.WriteEnum<CardRarityOddsType>(this.RarityOdds);
			writer.WriteModelEntriesInList(this.CardPoolIds);
			writer.WriteInt(this.OptionCount, 32);
			writer.WriteModelEntry(this.CustomDescriptionEncounterSourceId);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000450E0 File Offset: 0x000432E0
		public void Deserialize(PacketReader reader)
		{
			this.RewardType = (RewardType)reader.ReadInt(32);
			this.PredeterminedModelId = reader.ReadFullModelId();
			if (this.RewardType == RewardType.SpecialCard)
			{
				this.SpecialCard = reader.Read<SerializableCard>();
			}
			this.GoldAmount = reader.ReadInt(32);
			this.WasGoldStolenBack = reader.ReadBool();
			this.Source = reader.ReadEnum<CardCreationSource>();
			this.RarityOdds = reader.ReadEnum<CardRarityOddsType>();
			this.CardPoolIds = reader.ReadModelIdListAssumingType<CardPoolModel>();
			this.OptionCount = reader.ReadInt(32);
			this.CustomDescriptionEncounterSourceId = reader.ReadModelIdAssumingType<EncounterModel>();
		}
	}
}
