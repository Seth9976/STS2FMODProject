using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000773 RID: 1907
	public sealed class PowerPotion : PotionModel
	{
		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06005E60 RID: 24160 RVA: 0x00259047 File Offset: 0x00257247
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06005E61 RID: 24161 RVA: 0x0025904A File Offset: 0x0025724A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x06005E62 RID: 24162 RVA: 0x0025904D File Offset: 0x0025724D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x00259050 File Offset: 0x00257250
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player player = target.Player;
			List<CardModel> list = CardFactory.GetDistinctForCombat(player, from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Power
				select c, 3, player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
			CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, player, true);
			CardModel cardModel2 = cardModel;
			if (cardModel2 != null)
			{
				cardModel2.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}
	}
}
