using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Odds
{
	// Token: 0x02000172 RID: 370
	[NullableContext(1)]
	[Nullable(0)]
	public class RunOddsSet
	{
		/// <summary>
		/// Manages the odds of what types of rooms are rolled when entering an unknown point.
		/// </summary>
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00064D32 File Offset: 0x00062F32
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x00064D3A File Offset: 0x00062F3A
		public UnknownMapPointOdds UnknownMapPoint { get; private set; }

		// Token: 0x06001146 RID: 4422 RVA: 0x00064D43 File Offset: 0x00062F43
		private RunOddsSet()
		{
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00064D4B File Offset: 0x00062F4B
		public RunOddsSet(Rng unknownMapPointRng)
		{
			this.UnknownMapPoint = new UnknownMapPointOdds(unknownMapPointRng);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00064D60 File Offset: 0x00062F60
		public SerializableRunOddsSet ToSerializable()
		{
			return new SerializableRunOddsSet
			{
				UnknownMapPointMonsterOddsValue = this.UnknownMapPoint.MonsterOdds,
				UnknownMapPointEliteOddsValue = this.UnknownMapPoint.EliteOdds,
				UnknownMapPointTreasureOddsValue = this.UnknownMapPoint.TreasureOdds,
				UnknownMapPointShopOddsValue = this.UnknownMapPoint.ShopOdds
			};
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00064DB8 File Offset: 0x00062FB8
		public static RunOddsSet FromSerializable(SerializableRunOddsSet save, Rng unknownMapPointRng)
		{
			return new RunOddsSet
			{
				UnknownMapPoint = new UnknownMapPointOdds(unknownMapPointRng)
				{
					MonsterOdds = save.UnknownMapPointMonsterOddsValue,
					EliteOdds = save.UnknownMapPointEliteOddsValue,
					TreasureOdds = save.UnknownMapPointTreasureOddsValue,
					ShopOdds = save.UnknownMapPointShopOddsValue
				}
			};
		}
	}
}
