using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A1 RID: 1441
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Orrery : RelicModel
	{
		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06005315 RID: 21269 RVA: 0x0024410D File Offset: 0x0024230D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06005316 RID: 21270 RVA: 0x00244110 File Offset: 0x00242310
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06005317 RID: 21271 RVA: 0x00244113 File Offset: 0x00242313
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(5));
			}
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x00244120 File Offset: 0x00242320
		public override async Task AfterObtained()
		{
			List<Reward> list = new List<Reward>();
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, null);
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				list.Add(new CardReward(cardCreationOptions, 3, base.Owner, null));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
		}
	}
}
