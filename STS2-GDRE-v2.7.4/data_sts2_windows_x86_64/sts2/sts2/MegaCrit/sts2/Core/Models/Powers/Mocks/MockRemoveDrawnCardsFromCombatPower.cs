using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x0200073C RID: 1852
	public sealed class MockRemoveDrawnCardsFromCombatPower : PowerModel
	{
		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06005D03 RID: 23811 RVA: 0x0025742F File Offset: 0x0025562F
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x00257432 File Offset: 0x00255632
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x06005D05 RID: 23813 RVA: 0x00257435 File Offset: 0x00255635
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x00257438 File Offset: 0x00255638
		[NullableContext(1)]
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			await CardPileCmd.RemoveFromCombat(card, true);
		}
	}
}
