using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Factories
{
	// Token: 0x02000C44 RID: 3140
	[NullableContext(1)]
	[Nullable(0)]
	public static class PotionFactory
	{
		/// <summary>
		/// Create a random potion while out of combat.
		/// This can get any potion from the valid potion pools, including potions that can do restricted things like heal
		/// the player.
		/// </summary>
		/// <param name="player">Player to create a potion for.</param>
		/// <param name="rng">RNG to use to determine what potion to create.</param>
		/// <param name="blacklist">Canonical potions that should be blocked from being generated.</param>
		/// <returns>Newly-created potion.</returns>
		// Token: 0x06008184 RID: 33156 RVA: 0x002BEE30 File Offset: 0x002BD030
		public static PotionModel CreateRandomPotionOutOfCombat(Player player, Rng rng, [Nullable(new byte[] { 2, 1 })] IEnumerable<PotionModel> blacklist = null)
		{
			return PotionFactory.CreateRandomPotionsOutOfCombat(player, 1, rng, blacklist).First<PotionModel>();
		}

		/// <summary>
		/// Create random potions while out of combat.
		/// This can get any potion from the valid potion pools, including potions that can do restricted things like heal
		/// the player.
		/// </summary>
		/// <param name="player">Player to create a potion for.</param>
		/// <param name="count">How many potions you want to generate.</param>
		/// <param name="rng">RNG to use to determine what potion to create.</param>
		/// <param name="blacklist">Canonical potions that should be blocked from being generated.</param>
		/// <returns>Newly-created potions.</returns>
		// Token: 0x06008185 RID: 33157 RVA: 0x002BEE40 File Offset: 0x002BD040
		public static IEnumerable<PotionModel> CreateRandomPotionsOutOfCombat(Player player, int count, Rng rng, [Nullable(new byte[] { 2, 1 })] IEnumerable<PotionModel> blacklist = null)
		{
			IEnumerable<PotionModel> enumerable = PotionFactory.GetPotionOptions(player);
			if (blacklist != null)
			{
				enumerable = enumerable.Except<PotionModel>(blacklist);
			}
			return PotionFactory.CreateRandomPotions(enumerable, count, rng);
		}

		/// <summary>
		/// Create a random potion while in combat.
		/// This can get any potion from the valid potion pools, except potions that cannot be generated in combat (like
		/// healing potions).
		/// </summary>
		/// <param name="player">Player to create a potion for.</param>
		/// <param name="rng">RNG to use to determine what potion to create.</param>
		/// <param name="blacklist">Canonical potions that should be blocked from being generated.</param>
		/// <returns>Newly-created potion.</returns>
		// Token: 0x06008186 RID: 33158 RVA: 0x002BEE68 File Offset: 0x002BD068
		public static PotionModel CreateRandomPotionInCombat(Player player, Rng rng, [Nullable(new byte[] { 2, 1 })] IEnumerable<PotionModel> blacklist = null)
		{
			IEnumerable<PotionModel> enumerable = from p in PotionFactory.GetPotionOptions(player)
				where p.CanBeGeneratedInCombat
				select p;
			if (blacklist != null)
			{
				enumerable = enumerable.Except<PotionModel>(blacklist);
			}
			return PotionFactory.CreateRandomPotions(enumerable, 1, rng).First<PotionModel>();
		}

		// Token: 0x06008187 RID: 33159 RVA: 0x002BEEB8 File Offset: 0x002BD0B8
		private static IEnumerable<PotionModel> CreateRandomPotions(IEnumerable<PotionModel> options, int count, Rng rng)
		{
			List<PotionModel> list = options.ToList<PotionModel>();
			List<PotionModel> list2 = new List<PotionModel>();
			for (int i = 0; i < count; i++)
			{
				float num = rng.NextFloat(1f);
				PotionRarity potionRarity;
				if (num > 0.1f)
				{
					if (num > 0.35f)
					{
						potionRarity = PotionRarity.Common;
					}
					else
					{
						potionRarity = PotionRarity.Uncommon;
					}
				}
				else
				{
					potionRarity = PotionRarity.Rare;
				}
				PotionRarity rarity = potionRarity;
				PotionModel potionModel = rng.NextItem<PotionModel>(list.Where<PotionModel>((PotionModel potion) => potion.Rarity == rarity));
				list2.Add(potionModel);
				list.Remove(potionModel);
			}
			return list2;
		}

		// Token: 0x06008188 RID: 33160 RVA: 0x002BEF46 File Offset: 0x002BD146
		public static IEnumerable<PotionModel> GetPotionOptions(Player player)
		{
			return player.Character.PotionPool.GetUnlockedPotions(player.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(player.UnlockState));
		}

		// Token: 0x04002B65 RID: 11109
		private const float _rareChance = 0.1f;

		// Token: 0x04002B66 RID: 11110
		private const float _rareThreshold = 0.1f;

		// Token: 0x04002B67 RID: 11111
		private const float _uncommonChance = 0.25f;

		// Token: 0x04002B68 RID: 11112
		private const float _uncommonThreshold = 0.35f;
	}
}
