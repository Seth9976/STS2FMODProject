using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DF RID: 2271
	public sealed class SlumberingEssence : EnchantmentModel
	{
		// Token: 0x06006C8D RID: 27789 RVA: 0x002820B4 File Offset: 0x002802B4
		[NullableContext(1)]
		public override Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
		{
			if (player != base.Card.Owner)
			{
				return Task.CompletedTask;
			}
			CardPile pile = base.Card.Pile;
			if (pile == null || pile.Type != PileType.Hand)
			{
				return Task.CompletedTask;
			}
			base.Card.EnergyCost.AddUntilPlayed(-1, false);
			return Task.CompletedTask;
		}
	}
}
