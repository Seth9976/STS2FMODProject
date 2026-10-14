using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	/// <summary>
	/// Represents a way to serialize the entire state of a card to save file or for sending to a multiplayer peer.
	/// This should only be used in multiplayer if you need to recreate a card from scratch. If you are referencing an
	/// already-existing card owned by a player, you should use NetCombatCard or NetDeckCard.
	/// </summary>
	// Token: 0x020000C5 RID: 197
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableCard : IPacketSerializable
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00044039 File Offset: 0x00042239
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00044041 File Offset: 0x00042241
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0004404A File Offset: 0x0004224A
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00044052 File Offset: 0x00042252
		[JsonPropertyName("current_upgrade_level")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int CurrentUpgradeLevel { get; set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0004405B File Offset: 0x0004225B
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x00044063 File Offset: 0x00042263
		[JsonPropertyName("enchantment")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SerializableEnchantment Enchantment { get; set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0004406C File Offset: 0x0004226C
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00044074 File Offset: 0x00042274
		[JsonPropertyName("props")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SavedProperties Props { get; set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0004407D File Offset: 0x0004227D
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x00044085 File Offset: 0x00042285
		[JsonPropertyName("floor_added_to_deck")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? FloorAddedToDeck { get; set; }

		// Token: 0x06000969 RID: 2409 RVA: 0x00044090 File Offset: 0x00042290
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.WriteInt(this.CurrentUpgradeLevel, 8);
			writer.WriteBool(this.Enchantment != null);
			if (this.Enchantment != null)
			{
				writer.Write<SerializableEnchantment>(this.Enchantment);
			}
			writer.WriteBool(this.Props != null);
			if (this.Props != null)
			{
				writer.Write<SavedProperties>(this.Props);
			}
			writer.WriteBool(this.FloorAddedToDeck != null);
			if (this.FloorAddedToDeck != null)
			{
				writer.WriteInt(this.FloorAddedToDeck.Value, 8);
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00044138 File Offset: 0x00042338
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<CardModel>();
			this.CurrentUpgradeLevel = reader.ReadInt(8);
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.Enchantment = reader.Read<SerializableEnchantment>();
			}
			bool flag2 = reader.ReadBool();
			if (flag2)
			{
				this.Props = reader.Read<SavedProperties>();
			}
			bool flag3 = reader.ReadBool();
			if (flag3)
			{
				this.FloorAddedToDeck = new int?(reader.ReadInt(8));
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000441A8 File Offset: 0x000423A8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			SerializableCard serializableCard = (SerializableCard)obj;
			return this.Id.Equals(serializableCard.Id) && this.CurrentUpgradeLevel == serializableCard.CurrentUpgradeLevel && object.Equals(this.Enchantment, serializableCard.Enchantment);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0004420A File Offset: 0x0004240A
		public override int GetHashCode()
		{
			return HashCode.Combine<ModelId, int, SerializableEnchantment>(this.Id, this.CurrentUpgradeLevel, this.Enchantment);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00044224 File Offset: 0x00042424
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 5);
			defaultInterpolatedStringHandler.AppendLiteral("SerializableCard ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". Upgrades: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentUpgradeLevel);
			defaultInterpolatedStringHandler.AppendLiteral(" Enchantment: ");
			defaultInterpolatedStringHandler.AppendFormatted<SerializableEnchantment>(this.Enchantment);
			defaultInterpolatedStringHandler.AppendLiteral(" Props: ");
			defaultInterpolatedStringHandler.AppendFormatted<SavedProperties>(this.Props);
			defaultInterpolatedStringHandler.AppendLiteral(" Floor: ");
			defaultInterpolatedStringHandler.AppendFormatted<int?>(this.FloorAddedToDeck);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
