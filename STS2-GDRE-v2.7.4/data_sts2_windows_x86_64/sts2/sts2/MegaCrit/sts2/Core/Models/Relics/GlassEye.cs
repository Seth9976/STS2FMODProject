using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000557 RID: 1367
	public sealed class GlassEye : RelicModel
	{
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005135 RID: 20789 RVA: 0x00240928 File Offset: 0x0023EB28
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x0024092C File Offset: 0x0023EB2C
		[NullableContext(1)]
		public override async Task AfterObtained()
		{
			List<Reward> list = new List<Reward>();
			CardRarity[] array = new CardRarity[]
			{
				CardRarity.Common,
				CardRarity.Common,
				CardRarity.Uncommon,
				CardRarity.Uncommon,
				CardRarity.Rare
			};
			CardRarity[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				CardRarity rarity = array2[i];
				CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), (CardModel c) => c.Rarity == rarity).WithFlags(CardCreationFlags.NoRarityModification);
				list.Add(new CardReward(cardCreationOptions, 3, base.Owner, null));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
		}
	}
}
