using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000706 RID: 1798
	public sealed class StratagemPower : PowerModel
	{
		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06005B9A RID: 23450 RVA: 0x0025482B File Offset: 0x00252A2B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x0025482E File Offset: 0x00252A2E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x00254834 File Offset: 0x00252A34
		[NullableContext(1)]
		public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				base.Flash();
				IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner.Player), base.Owner.Player, new CardSelectorPrefs(base.SelectionScreenPrompt, base.Amount));
				IEnumerable<CardModel> enumerable2 = enumerable;
				foreach (CardModel cardModel in enumerable2)
				{
					await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
				}
				IEnumerator<CardModel> enumerator = null;
			}
		}
	}
}
