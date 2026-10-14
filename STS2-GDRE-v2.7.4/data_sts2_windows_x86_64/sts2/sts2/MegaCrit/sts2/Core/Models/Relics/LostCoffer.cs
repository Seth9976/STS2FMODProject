using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057E RID: 1406
	public sealed class LostCoffer : RelicModel
	{
		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x0024292B File Offset: 0x00240B2B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06005244 RID: 21060 RVA: 0x0024292E File Offset: 0x00240B2E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x00242934 File Offset: 0x00240B34
		[NullableContext(1)]
		public override async Task AfterObtained()
		{
			List<Reward> list = new List<Reward>();
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, null);
			list.Add(new CardReward(cardCreationOptions, 3, base.Owner, null));
			list.Add(new PotionReward(base.Owner));
			await RewardsCmd.OfferCustom(base.Owner, list);
		}
	}
}
