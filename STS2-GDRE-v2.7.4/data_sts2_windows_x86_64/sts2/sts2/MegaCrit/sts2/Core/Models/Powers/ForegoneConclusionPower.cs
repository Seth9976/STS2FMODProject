using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200067D RID: 1661
	public sealed class ForegoneConclusionPower : PowerModel
	{
		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06005869 RID: 22633 RVA: 0x0024E8B8 File Offset: 0x0024CAB8
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x0600586A RID: 22634 RVA: 0x0024E8BB File Offset: 0x0024CABB
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x0024E8C0 File Offset: 0x0024CAC0
		[NullableContext(1)]
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner.Player);
				IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner.Player), base.Owner.Player, new CardSelectorPrefs(base.SelectionScreenPrompt, base.Amount));
				await CardPileCmd.Add(enumerable, PileType.Hand, CardPilePosition.Bottom, null, false);
				await PowerCmd.Remove(this);
			}
		}
	}
}
