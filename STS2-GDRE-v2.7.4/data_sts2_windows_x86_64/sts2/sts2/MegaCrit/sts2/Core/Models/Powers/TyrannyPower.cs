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
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000724 RID: 1828
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TyrannyPower : PowerModel
	{
		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06005C62 RID: 23650 RVA: 0x002562C3 File Offset: 0x002544C3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06005C63 RID: 23651 RVA: 0x002562C6 File Offset: 0x002544C6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06005C64 RID: 23652 RVA: 0x002562C9 File Offset: 0x002544C9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x002562D6 File Offset: 0x002544D6
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner.Player)
			{
				return count;
			}
			return count + base.Amount;
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x002562FC File Offset: 0x002544FC
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, base.Amount), null, this);
				IEnumerable<CardModel> enumerable2 = enumerable;
				foreach (CardModel cardModel in enumerable2)
				{
					await CardCmd.Exhaust(choiceContext, cardModel, false, false);
				}
				IEnumerator<CardModel> enumerator = null;
			}
		}
	}
}
