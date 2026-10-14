using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200056A RID: 1386
	public sealed class JeweledMask : RelicModel
	{
		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x002414DF File Offset: 0x0023F6DF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x002414E4 File Offset: 0x0023F6E4
		[NullableContext(1)]
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					IReadOnlyList<CardModel> cards = PileType.Draw.GetPile(player).Cards;
					List<CardModel> list = cards.Where<CardModel>((CardModel c) => c.Type == CardType.Power).ToList<CardModel>();
					if (list.Count != 0)
					{
						CardModel cardModel = player.RunState.Rng.CombatCardSelection.NextItem<CardModel>(list);
						base.Flash();
						cardModel.SetToFreeThisTurn();
						await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
					}
				}
			}
		}
	}
}
