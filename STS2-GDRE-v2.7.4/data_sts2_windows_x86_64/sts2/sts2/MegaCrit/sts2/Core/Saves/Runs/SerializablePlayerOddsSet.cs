using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000CB RID: 203
	public class SerializablePlayerOddsSet : IPacketSerializable
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00044CEE File Offset: 0x00042EEE
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x00044CF6 File Offset: 0x00042EF6
		[JsonPropertyName("card_rarity_odds_value")]
		public float CardRarityOddsValue { get; set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x00044CFF File Offset: 0x00042EFF
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x00044D07 File Offset: 0x00042F07
		[JsonPropertyName("potion_reward_odds_value")]
		public float PotionRewardOddsValue { get; set; }

		// Token: 0x060009C9 RID: 2505 RVA: 0x00044D10 File Offset: 0x00042F10
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteFloat(this.CardRarityOddsValue, null);
			writer.WriteFloat(this.PotionRewardOddsValue, null);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00044D48 File Offset: 0x00042F48
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.CardRarityOddsValue = reader.ReadFloat(null);
			this.PotionRewardOddsValue = reader.ReadFloat(null);
		}
	}
}
