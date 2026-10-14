using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Odds
{
	// Token: 0x0200016F RID: 367
	[NullableContext(1)]
	[Nullable(0)]
	public class CardRarityOdds : AbstractOdds
	{
		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00064871 File Offset: 0x00062A71
		public float RarityGrowth
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.005f, 0.01f);
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x00064883 File Offset: 0x00062A83
		public static float RegularRareOdds
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.0149f, 0.03f);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00064895 File Offset: 0x00062A95
		public static float EliteCommonOdds
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.549f, 0.5f);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x000648A7 File Offset: 0x00062AA7
		public static float EliteRareOdds
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.05f, 0.1f);
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x000648B9 File Offset: 0x00062AB9
		public static float ShopCommonOdds
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.585f, 0.54f);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x000648CB File Offset: 0x00062ACB
		public static float ShopRareOdds
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.045f, 0.09f);
			}
		}

		/// <summary>
		/// For creating at the start of a run.
		/// </summary>
		/// <param name="rng">RNG to use for rolls.</param>
		// Token: 0x06001130 RID: 4400 RVA: 0x000648DD File Offset: 0x00062ADD
		public CardRarityOdds(Rng rng)
			: base(-0.05f, rng)
		{
		}

		/// <summary>
		/// For restoring from save.
		/// </summary>
		/// <param name="initialValue">Restored value at the saved spot in the run.</param>
		/// <param name="rng">RNG to use for rolls.</param>
		// Token: 0x06001131 RID: 4401 RVA: 0x000648EB File Offset: 0x00062AEB
		public CardRarityOdds(float initialValue, Rng rng)
			: base(initialValue, rng)
		{
		}

		/// <summary>
		/// Roll for what rarity of card should be generated.
		/// Using this will NOT modify the odds of future generated card rarities.
		/// Use for things like encounter rewards.
		/// </summary>
		/// <param name="type">Odds type to roll for.</param>
		/// <returns>What rarity a generated card should be.</returns>
		// Token: 0x06001132 RID: 4402 RVA: 0x000648F8 File Offset: 0x00062AF8
		public CardRarity Roll(CardRarityOddsType type)
		{
			CardRarity cardRarity = this.RollWithoutChangingFutureOdds(type, (type == CardRarityOddsType.BossEncounter) ? 0f : base.CurrentValue);
			if (cardRarity == CardRarity.Rare)
			{
				base.CurrentValue = -0.05f;
			}
			else
			{
				base.CurrentValue = Math.Min(base.CurrentValue + this.RarityGrowth, 0.4f);
			}
			return cardRarity;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0006494D File Offset: 0x00062B4D
		public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType oddsType)
		{
			return this.RollWithoutChangingFutureOdds(oddsType, base.CurrentValue);
		}

		/// <summary>
		/// Roll for what rarity of card should be generated.
		/// Using this will NOT modify the odds of future generated card rarities.
		/// Use for things like shops.
		/// </summary>
		/// <param name="type">Odds type to roll for.</param>
		/// <param name="offset">offset for rare odds</param>
		/// <returns>What rarity a generated card should be.</returns>
		// Token: 0x06001134 RID: 4404 RVA: 0x0006495C File Offset: 0x00062B5C
		public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType type, float offset)
		{
			float num = this._rng.NextFloat(1f);
			float num2 = CardRarityOdds.GetBaseOdds(type, CardRarity.Rare) + offset;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Card rarity: Rolled ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num);
			defaultInterpolatedStringHandler.AppendLiteral(", need < ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num2);
			defaultInterpolatedStringHandler.AppendLiteral(" for rare (offset = ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(offset);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (num < num2)
			{
				return CardRarity.Rare;
			}
			if (num < CardRarityOdds.GetBaseOdds(type, CardRarity.Uncommon) + num2)
			{
				return CardRarity.Uncommon;
			}
			return CardRarity.Common;
		}

		/// <summary>
		/// Roll for what rarity of card should be generated.
		/// Using this will use only the base odds for the given card source.
		/// Used for some events.
		/// </summary>
		/// <param name="type">Odds type to roll for.</param>
		/// <returns>What rarity a generated card should be.</returns>
		// Token: 0x06001135 RID: 4405 RVA: 0x000649F8 File Offset: 0x00062BF8
		public CardRarity RollWithBaseOdds(CardRarityOddsType type)
		{
			float num = this._rng.NextFloat(1f);
			if (num < CardRarityOdds.GetBaseOdds(type, CardRarity.Rare))
			{
				return CardRarity.Rare;
			}
			if (num < CardRarityOdds.GetBaseOdds(type, CardRarity.Uncommon))
			{
				return CardRarity.Uncommon;
			}
			return CardRarity.Common;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00064A30 File Offset: 0x00062C30
		private static float GetBaseOdds(CardRarityOddsType type, CardRarity rarity)
		{
			float num2;
			switch (type)
			{
			case CardRarityOddsType.RegularEncounter:
			{
				float num;
				switch (rarity)
				{
				case CardRarity.Common:
					num = CardRarityOdds.regularCommonOdds;
					break;
				case CardRarity.Uncommon:
					num = 0.37f;
					break;
				case CardRarity.Rare:
					num = CardRarityOdds.RegularRareOdds;
					break;
				default:
					throw new ArgumentOutOfRangeException("rarity");
				}
				num2 = num;
				break;
			}
			case CardRarityOddsType.EliteEncounter:
			{
				float num;
				switch (rarity)
				{
				case CardRarity.Common:
					num = CardRarityOdds.EliteCommonOdds;
					break;
				case CardRarity.Uncommon:
					num = 0.4f;
					break;
				case CardRarity.Rare:
					num = CardRarityOdds.EliteRareOdds;
					break;
				default:
					throw new ArgumentOutOfRangeException("rarity");
				}
				num2 = num;
				break;
			}
			case CardRarityOddsType.BossEncounter:
			{
				float num;
				switch (rarity)
				{
				case CardRarity.Common:
					num = 0f;
					break;
				case CardRarity.Uncommon:
					num = 0f;
					break;
				case CardRarity.Rare:
					num = 1f;
					break;
				default:
					throw new ArgumentOutOfRangeException("rarity");
				}
				num2 = num;
				break;
			}
			case CardRarityOddsType.Shop:
			{
				float num;
				switch (rarity)
				{
				case CardRarity.Common:
					num = CardRarityOdds.ShopCommonOdds;
					break;
				case CardRarity.Uncommon:
					num = 0.37f;
					break;
				case CardRarity.Rare:
					num = CardRarityOdds.ShopRareOdds;
					break;
				default:
					throw new ArgumentOutOfRangeException("rarity");
				}
				num2 = num;
				break;
			}
			case CardRarityOddsType.Uniform:
			{
				float num;
				switch (rarity)
				{
				case CardRarity.Common:
					num = 0.33f;
					break;
				case CardRarity.Uncommon:
					num = 0.33f;
					break;
				case CardRarity.Rare:
					num = 0.33f;
					break;
				default:
					throw new ArgumentOutOfRangeException("rarity");
				}
				num2 = num;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("type");
			}
			return num2;
		}

		// Token: 0x04000618 RID: 1560
		public static float regularCommonOdds = AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.615f, 0.6f);

		// Token: 0x04000619 RID: 1561
		public const float regularUncommonOdds = 0.37f;

		// Token: 0x0400061A RID: 1562
		public const float eliteUncommonOdds = 0.4f;

		// Token: 0x0400061B RID: 1563
		public const float bossCommonOdds = 0f;

		// Token: 0x0400061C RID: 1564
		public const float bossUncommonOdds = 0f;

		// Token: 0x0400061D RID: 1565
		public const float bossRareOdds = 1f;

		// Token: 0x0400061E RID: 1566
		public const float shopUncommonOdds = 0.37f;

		// Token: 0x0400061F RID: 1567
		private const float _baseRarityOffset = -0.05f;

		// Token: 0x04000620 RID: 1568
		private const float _maxRarityOffset = 0.4f;
	}
}
