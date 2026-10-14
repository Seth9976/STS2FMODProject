using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000942 RID: 2370
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CalculatedGamble : CardModel
	{
		// Token: 0x06006F02 RID: 28418 RVA: 0x002868FC File Offset: 0x00284AFC
		public CalculatedGamble()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x06006F03 RID: 28419 RVA: 0x00286909 File Offset: 0x00284B09
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x00286914 File Offset: 0x00284B14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards;
			int num = cards.Count<CardModel>();
			await CardCmd.DiscardAndDraw(choiceContext, cards, num);
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x0028695F File Offset: 0x00284B5F
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
