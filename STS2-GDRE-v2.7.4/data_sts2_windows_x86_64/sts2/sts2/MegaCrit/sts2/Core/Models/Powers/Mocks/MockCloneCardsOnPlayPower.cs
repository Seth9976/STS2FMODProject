using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x02000732 RID: 1842
	public sealed class MockCloneCardsOnPlayPower : PowerModel
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06005CBD RID: 23741 RVA: 0x00257063 File Offset: 0x00255263
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06005CBE RID: 23742 RVA: 0x00257066 File Offset: 0x00255266
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06005CBF RID: 23743 RVA: 0x00257069 File Offset: 0x00255269
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x0025706C File Offset: 0x0025526C
		[NullableContext(1)]
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				CardModel cardModel = cardPlay.Card.CreateClone();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
			}
		}
	}
}
