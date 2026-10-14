using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000700 RID: 1792
	public sealed class StampedePower : PowerModel
	{
		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x0025442B File Offset: 0x0025262B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06005B77 RID: 23415 RVA: 0x0025442E File Offset: 0x0025262E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x00254434 File Offset: 0x00252634
		[NullableContext(1)]
		public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				CardPile hand = PileType.Hand.GetPile(base.Owner.Player);
				for (int i = 0; i < base.Amount; i++)
				{
					List<CardModel> list = hand.Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList<CardModel>();
					CardModel cardModel = base.Owner.Player.RunState.Rng.Shuffle.NextItem<CardModel>(list);
					if (cardModel != null)
					{
						await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.Default, false, false);
					}
				}
			}
		}
	}
}
