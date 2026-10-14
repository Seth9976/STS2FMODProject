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
	// Token: 0x0200076B RID: 1899
	public sealed class OrobicAcid : PotionModel
	{
		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06005E2D RID: 24109 RVA: 0x00258C23 File Offset: 0x00256E23
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06005E2E RID: 24110 RVA: 0x00258C26 File Offset: 0x00256E26
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x06005E2F RID: 24111 RVA: 0x00258C29 File Offset: 0x00256E29
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00258C2C File Offset: 0x00256E2C
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player player = target.Player;
			List<CardModel> list = new List<CardModel>();
			list.AddRange(CardFactory.GetDistinctForCombat(player, from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Attack
				select c, 1, player.RunState.Rng.CombatCardGeneration));
			list.AddRange(CardFactory.GetDistinctForCombat(player, from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Skill
				select c, 1, player.RunState.Rng.CombatCardGeneration));
			list.AddRange(CardFactory.GetDistinctForCombat(player, from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Power
				select c, 1, player.RunState.Rng.CombatCardGeneration));
			foreach (CardModel cardModel in list)
			{
				cardModel.SetToFreeThisTurn();
			}
			await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
		}
	}
}
