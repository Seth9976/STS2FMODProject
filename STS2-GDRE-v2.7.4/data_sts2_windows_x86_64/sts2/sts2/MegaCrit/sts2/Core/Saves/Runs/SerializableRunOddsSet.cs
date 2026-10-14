using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D2 RID: 210
	public class SerializableRunOddsSet : IPacketSerializable
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00045769 File Offset: 0x00043969
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x00045771 File Offset: 0x00043971
		[JsonPropertyName("unknown_map_point_monster_odds_value")]
		public float UnknownMapPointMonsterOddsValue { get; set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0004577A File Offset: 0x0004397A
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00045782 File Offset: 0x00043982
		[JsonPropertyName("unknown_map_point_elite_odds_value")]
		public float UnknownMapPointEliteOddsValue { get; set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0004578B File Offset: 0x0004398B
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x00045793 File Offset: 0x00043993
		[JsonPropertyName("unknown_map_point_treasure_odds_value")]
		public float UnknownMapPointTreasureOddsValue { get; set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0004579C File Offset: 0x0004399C
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x000457A4 File Offset: 0x000439A4
		[JsonPropertyName("unknown_map_point_shop_odds_value")]
		public float UnknownMapPointShopOddsValue { get; set; }

		// Token: 0x06000A2C RID: 2604 RVA: 0x000457B0 File Offset: 0x000439B0
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteFloat(this.UnknownMapPointMonsterOddsValue, null);
			writer.WriteFloat(this.UnknownMapPointEliteOddsValue, null);
			writer.WriteFloat(this.UnknownMapPointTreasureOddsValue, null);
			writer.WriteFloat(this.UnknownMapPointShopOddsValue, null);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00045814 File Offset: 0x00043A14
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.UnknownMapPointMonsterOddsValue = reader.ReadFloat(null);
			this.UnknownMapPointEliteOddsValue = reader.ReadFloat(null);
			this.UnknownMapPointTreasureOddsValue = reader.ReadFloat(null);
			this.UnknownMapPointShopOddsValue = reader.ReadFloat(null);
		}
	}
}
