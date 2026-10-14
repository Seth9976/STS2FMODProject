using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000814 RID: 2068
	[NullableContext(1)]
	[Nullable(0)]
	public class BigGameHunter : ModifierModel
	{
		// Token: 0x06006725 RID: 26405 RVA: 0x00271BFC File Offset: 0x0026FDFC
		public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
		{
			ulong seed = runState.Rng.Seed;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("act_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(runState.CurrentActIndex + 1);
			defaultInterpolatedStringHandler.AppendLiteral("_map");
			Rng rng = new Rng(seed, defaultInterpolatedStringHandler.ToStringAndClear());
			MapPointTypeCounts mapPointTypeCounts = new MapPointTypeCounts(map);
			mapPointTypeCounts.NumOfElites = (int)Math.Round((double)((float)map.GetAllMapPoints().Count<MapPoint>((MapPoint p) => p.PointType == MapPointType.Elite) * 2.5f));
			mapPointTypeCounts.PointTypesThatIgnoreRules = new HashSet<MapPointType> { MapPointType.Elite };
			MapPointTypeCounts mapPointTypeCounts2 = mapPointTypeCounts;
			if (map is StandardActMap)
			{
				StandardActMap standardActMap = map as StandardActMap;
				bool flag = standardActMap != null && standardActMap.ShouldReplaceTreasureWithElites;
				return new StandardActMap(rng, runState.Act, runState.Players.Count > 1, flag, runState.Act.HasSecondBoss, mapPointTypeCounts2, true);
			}
			if (map is SpoilsActMap)
			{
				return new SpoilsActMap(runState, mapPointTypeCounts2);
			}
			return map;
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x00271D04 File Offset: 0x0026FF04
		public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
		{
			if (options.Source != CardCreationSource.Encounter)
			{
				return options;
			}
			if (options.RarityOdds != CardRarityOddsType.EliteEncounter)
			{
				return options;
			}
			if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
			{
				return options;
			}
			if (options.Flags.HasFlag(CardCreationFlags.NoRarityModification))
			{
				return options;
			}
			options = options.WithRarityOdds(CardRarityOddsType.Uniform).WithFilter((CardModel c) => c.Rarity == CardRarity.Rare);
			List<CardModel> list = options.GetPossibleCards(player).ToList<CardModel>();
			if (list.Count <= 0)
			{
				options = options.WithCardPools(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool));
			}
			return options;
		}
	}
}
