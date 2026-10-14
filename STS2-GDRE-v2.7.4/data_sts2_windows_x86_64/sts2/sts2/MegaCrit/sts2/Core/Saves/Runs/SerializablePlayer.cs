using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CA RID: 202
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializablePlayer : IPacketSerializable
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0004475C File Offset: 0x0004295C
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x00044764 File Offset: 0x00042964
		[Nullable(2)]
		[JsonPropertyName("character_id")]
		public ModelId CharacterId
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0004476D File Offset: 0x0004296D
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x00044775 File Offset: 0x00042975
		[JsonPropertyName("current_hp")]
		public int CurrentHp { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0004477E File Offset: 0x0004297E
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x00044786 File Offset: 0x00042986
		[JsonPropertyName("max_hp")]
		public int MaxHp { get; set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0004478F File Offset: 0x0004298F
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x00044797 File Offset: 0x00042997
		[JsonPropertyName("max_energy")]
		public int MaxEnergy { get; set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x000447A0 File Offset: 0x000429A0
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x000447A8 File Offset: 0x000429A8
		[JsonPropertyName("max_potion_slot_count")]
		public int MaxPotionSlotCount { get; set; } = 3;

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x000447B1 File Offset: 0x000429B1
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x000447B9 File Offset: 0x000429B9
		[JsonPropertyName("gold")]
		public int Gold { get; set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x000447C2 File Offset: 0x000429C2
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x000447CA File Offset: 0x000429CA
		[JsonPropertyName("base_orb_slot_count")]
		public int BaseOrbSlotCount { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x000447D3 File Offset: 0x000429D3
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x000447DB File Offset: 0x000429DB
		[JsonPropertyName("net_id")]
		public ulong NetId { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x000447E4 File Offset: 0x000429E4
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x000447EC File Offset: 0x000429EC
		[JsonPropertyName("deck")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializableCard> Deck { get; set; } = new List<SerializableCard>();

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x000447F5 File Offset: 0x000429F5
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x000447FD File Offset: 0x000429FD
		[JsonPropertyName("relics")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializableRelic> Relics { get; set; } = new List<SerializableRelic>();

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00044806 File Offset: 0x00042A06
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0004480E File Offset: 0x00042A0E
		[JsonPropertyName("potions")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializablePotion> Potions { get; set; } = new List<SerializablePotion>();

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00044817 File Offset: 0x00042A17
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0004481F File Offset: 0x00042A1F
		[JsonPropertyName("rng")]
		public SerializablePlayerRngSet Rng { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00044828 File Offset: 0x00042A28
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x00044830 File Offset: 0x00042A30
		[JsonPropertyName("odds")]
		public SerializablePlayerOddsSet Odds { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00044839 File Offset: 0x00042A39
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x00044841 File Offset: 0x00042A41
		[JsonPropertyName("relic_grab_bag")]
		public SerializableRelicGrabBag RelicGrabBag { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0004484A File Offset: 0x00042A4A
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x00044852 File Offset: 0x00042A52
		[JsonPropertyName("extra_fields")]
		public SerializableExtraPlayerFields ExtraFields { get; set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0004485B File Offset: 0x00042A5B
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x00044863 File Offset: 0x00042A63
		[JsonPropertyName("unlock_state")]
		public SerializableUnlockState UnlockState { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0004486C File Offset: 0x00042A6C
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x00044874 File Offset: 0x00042A74
		[JsonPropertyName("discovered_cards")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> DiscoveredCards { get; set; } = new List<ModelId>();

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0004487D File Offset: 0x00042A7D
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x00044885 File Offset: 0x00042A85
		[JsonPropertyName("discovered_enemies")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> DiscoveredEnemies { get; set; } = new List<ModelId>();

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0004488E File Offset: 0x00042A8E
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x00044896 File Offset: 0x00042A96
		[JsonPropertyName("discovered_epochs")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		[JsonConverter(typeof(EpochIdListConverter))]
		public List<string> DiscoveredEpochs { get; set; } = new List<string>();

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0004489F File Offset: 0x00042A9F
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x000448A7 File Offset: 0x00042AA7
		[JsonPropertyName("discovered_potions")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> DiscoveredPotions { get; set; } = new List<ModelId>();

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x000448B0 File Offset: 0x00042AB0
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x000448B8 File Offset: 0x00042AB8
		[JsonPropertyName("discovered_relics")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> DiscoveredRelics { get; set; } = new List<ModelId>();

		// Token: 0x060009C1 RID: 2497 RVA: 0x000448C4 File Offset: 0x00042AC4
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.NetId, 64);
			writer.WriteModelEntry(this.CharacterId);
			writer.WriteInt(this.CurrentHp, 32);
			writer.WriteInt(this.MaxHp, 32);
			writer.WriteInt(this.MaxEnergy, 16);
			writer.WriteInt(this.MaxPotionSlotCount, 8);
			writer.WriteInt(this.Gold, 32);
			writer.WriteInt(this.BaseOrbSlotCount, 16);
			writer.WriteList<SerializableCard>(this.Deck, 32);
			writer.WriteList<SerializableRelic>(this.Relics, 32);
			writer.WriteList<SerializablePotion>(this.Potions, 32);
			writer.Write<SerializablePlayerRngSet>(this.Rng);
			writer.Write<SerializablePlayerOddsSet>(this.Odds);
			writer.Write<SerializableRelicGrabBag>(this.RelicGrabBag);
			writer.Write<SerializableExtraPlayerFields>(this.ExtraFields);
			writer.Write<SerializableUnlockState>(this.UnlockState);
			writer.WriteFullModelIdList(this.DiscoveredCards);
			writer.WriteFullModelIdList(this.DiscoveredEnemies);
			writer.WriteInt(this.DiscoveredEpochs.Count, 32);
			foreach (string text in this.DiscoveredEpochs)
			{
				writer.WriteEpochId(text);
			}
			writer.WriteFullModelIdList(this.DiscoveredPotions);
			writer.WriteFullModelIdList(this.DiscoveredRelics);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00044A30 File Offset: 0x00042C30
		public void Deserialize(PacketReader reader)
		{
			this.NetId = reader.ReadULong(64);
			this.CharacterId = reader.ReadModelIdAssumingType<CharacterModel>();
			this.CurrentHp = reader.ReadInt(32);
			this.MaxHp = reader.ReadInt(32);
			this.MaxEnergy = reader.ReadInt(16);
			this.MaxPotionSlotCount = reader.ReadInt(8);
			this.Gold = reader.ReadInt(32);
			this.BaseOrbSlotCount = reader.ReadInt(16);
			this.Deck = reader.ReadList<SerializableCard>(32);
			this.Relics = reader.ReadList<SerializableRelic>(32);
			this.Potions = reader.ReadList<SerializablePotion>(32);
			this.Rng = reader.Read<SerializablePlayerRngSet>();
			this.Odds = reader.Read<SerializablePlayerOddsSet>();
			this.RelicGrabBag = reader.Read<SerializableRelicGrabBag>();
			this.ExtraFields = reader.Read<SerializableExtraPlayerFields>();
			this.UnlockState = reader.Read<SerializableUnlockState>();
			this.DiscoveredCards = reader.ReadFullModelIdList();
			this.DiscoveredEnemies = reader.ReadFullModelIdList();
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				this.DiscoveredEpochs.Add(reader.ReadEpochId());
			}
			this.DiscoveredPotions = reader.ReadFullModelIdList();
			this.DiscoveredRelics = reader.ReadFullModelIdList();
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00044B68 File Offset: 0x00042D68
		public SerializablePlayer Anonymized()
		{
			return new SerializablePlayer
			{
				CharacterId = this.CharacterId,
				CurrentHp = this.CurrentHp,
				MaxHp = this.MaxHp,
				MaxEnergy = this.MaxEnergy,
				MaxPotionSlotCount = this.MaxPotionSlotCount,
				Gold = this.Gold,
				BaseOrbSlotCount = this.BaseOrbSlotCount,
				NetId = IdAnonymizer.Anonymize(this.NetId),
				Deck = this.Deck,
				Relics = this.Relics,
				Potions = this.Potions,
				Rng = this.Rng,
				Odds = this.Odds,
				RelicGrabBag = this.RelicGrabBag,
				ExtraFields = this.ExtraFields,
				UnlockState = this.UnlockState,
				DiscoveredCards = this.DiscoveredCards,
				DiscoveredEnemies = this.DiscoveredEnemies,
				DiscoveredEpochs = this.DiscoveredEpochs,
				DiscoveredPotions = this.DiscoveredPotions,
				DiscoveredRelics = this.DiscoveredRelics
			};
		}
	}
}
