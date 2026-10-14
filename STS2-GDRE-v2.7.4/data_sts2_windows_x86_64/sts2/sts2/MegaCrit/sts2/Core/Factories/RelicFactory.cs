using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Factories
{
	// Token: 0x02000C45 RID: 3141
	[NullableContext(1)]
	[Nullable(0)]
	public static class RelicFactory
	{
		// Token: 0x1700235C RID: 9052
		// (get) Token: 0x06008189 RID: 33161 RVA: 0x002BEF73 File Offset: 0x002BD173
		public static RelicModel FallbackRelic
		{
			get
			{
				return ModelDb.Relic<Circlet>();
			}
		}

		/// <summary>
		/// Roll rarity and pull the next matching relic from the front of the list.
		/// The pulled relic will never be seen again on this run.
		/// Used by most relic reward sources (elite combat rewards etc).
		/// </summary>
		/// <returns>RelicModel</returns>
		// Token: 0x0600818A RID: 33162 RVA: 0x002BEF7A File Offset: 0x002BD17A
		public static RelicModel PullNextRelicFromFront(Player player, Rng rng)
		{
			return RelicFactory.PullNextRelicFromFront(player, RelicFactory.RollRarity(rng), (RelicModel _) => true);
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x002BEFA7 File Offset: 0x002BD1A7
		public static RelicModel PullNextRelicFromFront(Player player)
		{
			return RelicFactory.PullNextRelicFromFront(player, RelicFactory.RollRarity(player), (RelicModel _) => true);
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x002BEFD4 File Offset: 0x002BD1D4
		public static RelicModel PullNextRelicFromFront(Player player, RelicRarity rarity)
		{
			return RelicFactory.PullNextRelicFromFront(player, rarity, (RelicModel _) => true);
		}

		/// <summary>
		/// Pull the next matching relic from the front of the list.
		/// The pulled relic will never be seen again on this run.
		/// Used by most relic reward sources (elite combat rewards etc).
		/// </summary>
		/// <param name="player">The player for which we're obtaining a relic.</param>
		/// <param name="rarity">Rarity of the relic we want.</param>
		/// <param name="filter">Filter for what relics are allowed.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x0600818D RID: 33165 RVA: 0x002BEFFC File Offset: 0x002BD1FC
		public static RelicModel PullNextRelicFromFront(Player player, RelicRarity rarity, Func<RelicModel, bool> filter)
		{
			RelicModel relicModel;
			if ((relicModel = TestRngInjector.ConsumeRelicOverride()) == null)
			{
				relicModel = player.RelicGrabBag.PullFromFront(rarity, filter, player.RunState) ?? RelicFactory.FallbackRelic;
			}
			RelicModel relicModel2 = relicModel;
			player.RunState.SharedRelicGrabBag.Remove(relicModel2);
			return relicModel2;
		}

		/// <summary>
		/// Roll rarity and pull the next matching relic from the back of the list.
		/// The pulled relic will never be seen again on this run.
		/// Used in shops.
		/// </summary>
		/// <param name="player">The player for which we're obtaining a relic.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x0600818E RID: 33166 RVA: 0x002BF041 File Offset: 0x002BD241
		public static RelicModel PullNextRelicFromBack(Player player)
		{
			return RelicFactory.PullNextRelicFromBack(player, RelicFactory.RollRarity(player), (RelicModel _) => true);
		}

		/// <summary>
		/// Pull the next matching relic from the back of the list.
		/// The pulled relic will never be seen again on this run.
		/// Used in shops.
		/// </summary>
		/// <param name="player">The player for which we're obtaining a relic.</param>
		/// <param name="rarity">Rarity of the relic we want.</param>
		/// <param name="filter">Only relics that match the filter will be allowed to be returned.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x0600818F RID: 33167 RVA: 0x002BF070 File Offset: 0x002BD270
		public static RelicModel PullNextRelicFromBack(Player player, RelicRarity rarity, Func<RelicModel, bool> filter)
		{
			RelicModel relicModel;
			if ((relicModel = TestRngInjector.ConsumeRelicOverride()) == null)
			{
				relicModel = player.RelicGrabBag.PullFromBack(rarity, filter, player.RunState) ?? RelicFactory.FallbackRelic;
			}
			RelicModel relicModel2 = relicModel;
			player.RunState.SharedRelicGrabBag.Remove(relicModel2);
			return relicModel2;
		}

		// Token: 0x06008190 RID: 33168 RVA: 0x002BF0B5 File Offset: 0x002BD2B5
		public static RelicRarity RollRarity(Player player)
		{
			return RelicFactory.RollRarity(player.PlayerRng.Rewards);
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x002BF0C8 File Offset: 0x002BD2C8
		public static RelicRarity RollRarity(Rng rng)
		{
			RelicRarity? relicRarityOverride = TestRngInjector.GetRelicRarityOverride();
			RelicRarity relicRarity;
			if (relicRarityOverride != null)
			{
				relicRarity = relicRarityOverride.GetValueOrDefault();
			}
			else
			{
				float num = rng.NextFloat(1f);
				RelicRarity relicRarity2;
				if (num >= 0.5f)
				{
					if (num >= 0.83f)
					{
						relicRarity2 = RelicRarity.Rare;
					}
					else
					{
						relicRarity2 = RelicRarity.Uncommon;
					}
				}
				else
				{
					relicRarity2 = RelicRarity.Common;
				}
				relicRarity = relicRarity2;
			}
			return relicRarity;
		}
	}
}
