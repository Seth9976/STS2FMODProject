using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000780 RID: 1920
	public sealed class TouchOfInsanity : PotionModel
	{
		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06005EB7 RID: 24247 RVA: 0x0025978F File Offset: 0x0025798F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x00259792 File Offset: 0x00257992
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x00259795 File Offset: 0x00257995
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x00259798 File Offset: 0x00257998
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player player = target.Player;
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, player, cardSelectorPrefs, (CardModel c) => c.CostsEnergyOrStars(false) || c.CostsEnergyOrStars(true), this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			CardModel cardModel = enumerable2.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				cardModel.SetToFreeThisCombat();
			}
		}
	}
}
