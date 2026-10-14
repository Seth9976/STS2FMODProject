using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CBB RID: 3259
	public static class CardTypeExtensions
	{
		// Token: 0x060084DC RID: 34012 RVA: 0x002CAF64 File Offset: 0x002C9164
		[NullableContext(1)]
		public static LocString ToLocString(this CardType cardType)
		{
			LocString locString;
			switch (cardType)
			{
			case CardType.None:
				locString = new LocString("gameplay_ui", "CARD_TYPE.NONE");
				break;
			case CardType.Attack:
				locString = new LocString("gameplay_ui", "CARD_TYPE.ATTACK");
				break;
			case CardType.Skill:
				locString = new LocString("gameplay_ui", "CARD_TYPE.SKILL");
				break;
			case CardType.Power:
				locString = new LocString("gameplay_ui", "CARD_TYPE.POWER");
				break;
			case CardType.Status:
				locString = new LocString("gameplay_ui", "CARD_TYPE.STATUS");
				break;
			case CardType.Curse:
				locString = new LocString("gameplay_ui", "CARD_TYPE.CURSE");
				break;
			case CardType.Quest:
				locString = new LocString("gameplay_ui", "CARD_TYPE.QUEST");
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(cardType);
				break;
			}
			return locString;
		}
	}
}
