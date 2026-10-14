using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Odds
{
	// Token: 0x02000170 RID: 368
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerOddsSet
	{
		/// <summary>
		/// Manages the odds of rolling various rarities when generating cards (encounter rewards, shops, etc.) for each player.
		/// </summary>
		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00064BBB File Offset: 0x00062DBB
		// (set) Token: 0x06001139 RID: 4409 RVA: 0x00064BC3 File Offset: 0x00062DC3
		public CardRarityOdds CardRarity { get; private set; }

		/// <summary>
		/// Manages the odds of whether or not a potion drops after beating an encounter for each player.
		/// </summary>
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00064BCC File Offset: 0x00062DCC
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x00064BD4 File Offset: 0x00062DD4
		public PotionRewardOdds PotionReward { get; private set; }

		// Token: 0x0600113C RID: 4412 RVA: 0x00064BDD File Offset: 0x00062DDD
		private PlayerOddsSet()
		{
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00064BE5 File Offset: 0x00062DE5
		public PlayerOddsSet(PlayerRngSet rng)
		{
			this.CardRarity = new CardRarityOdds(rng.Rewards);
			this.PotionReward = new PotionRewardOdds(rng.Rewards);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00064C0F File Offset: 0x00062E0F
		public SerializablePlayerOddsSet ToSerializable()
		{
			return new SerializablePlayerOddsSet
			{
				CardRarityOddsValue = this.CardRarity.CurrentValue,
				PotionRewardOddsValue = this.PotionReward.CurrentValue
			};
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00064C38 File Offset: 0x00062E38
		public static PlayerOddsSet FromSerializable(SerializablePlayerOddsSet save, PlayerRngSet rng)
		{
			return new PlayerOddsSet
			{
				CardRarity = new CardRarityOdds(save.CardRarityOddsValue, rng.Rewards),
				PotionReward = new PotionRewardOdds(save.PotionRewardOddsValue, rng.Rewards)
			};
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00064C6D File Offset: 0x00062E6D
		public void LoadFromSerializable(SerializablePlayerOddsSet save)
		{
			this.CardRarity.OverrideCurrentValue(save.CardRarityOddsValue);
			this.PotionReward.OverrideCurrentValue(save.PotionRewardOddsValue);
		}
	}
}
