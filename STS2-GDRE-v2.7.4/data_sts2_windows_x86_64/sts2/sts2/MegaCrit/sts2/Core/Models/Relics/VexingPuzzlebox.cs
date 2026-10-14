using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000610 RID: 1552
	public sealed class VexingPuzzlebox : RelicModel
	{
		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06005617 RID: 22039 RVA: 0x00249D27 File Offset: 0x00247F27
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x00249D2C File Offset: 0x00247F2C
		[NullableContext(1)]
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					CardModel cardModel = CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), 1, base.Owner.RunState.Rng.CombatCardGeneration).First<CardModel>();
					cardModel.SetToFreeThisTurn();
					await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, player, CardPilePosition.Bottom);
				}
			}
		}
	}
}
