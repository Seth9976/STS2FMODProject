using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062A RID: 1578
	public sealed class AggressionPower : PowerModel
	{
		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060056A4 RID: 22180 RVA: 0x0024B89B File Offset: 0x00249A9B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x0024B89E File Offset: 0x00249A9E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x0024B8A4 File Offset: 0x00249AA4
		[NullableContext(1)]
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				CardPile pile = PileType.Discard.GetPile(base.Owner.Player);
				IEnumerable<CardModel> enumerable = pile.Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack);
				IEnumerable<CardModel> enumerable2 = enumerable.ToList<CardModel>().UnstableShuffle<CardModel>(base.Owner.Player.RunState.Rng.CombatCardSelection).Take<CardModel>(base.Amount);
				foreach (CardModel card in enumerable2)
				{
					await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Bottom, null, false);
					if (card.IsUpgradable)
					{
						CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
					}
					card = null;
				}
				IEnumerator<CardModel> enumerator = null;
			}
		}
	}
}
