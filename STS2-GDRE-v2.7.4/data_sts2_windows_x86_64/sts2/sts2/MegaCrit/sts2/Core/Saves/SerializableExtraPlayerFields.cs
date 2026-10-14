using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000AF RID: 175
	public class SerializableExtraPlayerFields : IPacketSerializable
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00040306 File Offset: 0x0003E506
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x0004030E File Offset: 0x0003E50E
		[JsonPropertyName("card_shop_removals_used")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int CardShopRemovalsUsed { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00040317 File Offset: 0x0003E517
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x0004031F File Offset: 0x0003E51F
		[JsonPropertyName("wongo_points")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int WongoPoints { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x00040328 File Offset: 0x0003E528
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00040330 File Offset: 0x0003E530
		[JsonPropertyName("ccccombo_badge_unlocked")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool CccomboBadgeUnlocked { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00040339 File Offset: 0x0003E539
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00040341 File Offset: 0x0003E541
		[JsonPropertyName("damage_dealt")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int DamageDealt { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0004034A File Offset: 0x0003E54A
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00040352 File Offset: 0x0003E552
		[JsonPropertyName("debuffs_applied")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int DebuffsApplied { get; set; }

		// Token: 0x06000816 RID: 2070 RVA: 0x0004035C File Offset: 0x0003E55C
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.CardShopRemovalsUsed, 32);
			writer.WriteInt(this.WongoPoints, 32);
			writer.WriteBool(this.CccomboBadgeUnlocked);
			writer.WriteInt(this.DamageDealt, 32);
			writer.WriteInt(this.DebuffsApplied, 32);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000403B0 File Offset: 0x0003E5B0
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.CardShopRemovalsUsed = reader.ReadInt(32);
			this.WongoPoints = reader.ReadInt(32);
			this.CccomboBadgeUnlocked = reader.ReadBool();
			this.DamageDealt = reader.ReadInt(32);
			this.DebuffsApplied = reader.ReadInt(32);
		}
	}
}
