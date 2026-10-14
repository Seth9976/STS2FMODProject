using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020008FE RID: 2302
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Anointed : CardModel
	{
		// Token: 0x06006DA8 RID: 28072 RVA: 0x00283C1B File Offset: 0x00281E1B
		public Anointed()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x06006DA9 RID: 28073 RVA: 0x00283C28 File Offset: 0x00281E28
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x00283C30 File Offset: 0x00281E30
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num = CardPile.MaxCardsInHand - PileType.Hand.GetPile(base.Owner).Cards.Count;
			List<CardModel> list = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Rarity == CardRarity.Rare).TakeRandom<CardModel>(num, base.Owner.RunState.Rng.CombatCardSelection).ToList<CardModel>();
			await CardPileCmd.Add(list, PileType.Hand, CardPilePosition.Bottom, null, false);
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x00283C73 File Offset: 0x00281E73
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
