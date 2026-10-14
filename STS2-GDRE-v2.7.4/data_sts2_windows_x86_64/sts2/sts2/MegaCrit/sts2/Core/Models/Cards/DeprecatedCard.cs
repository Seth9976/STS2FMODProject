using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	/// <summary>
	/// Placeholder event if we can't find the card we are looking for (such as in the case that it's been deprecated)
	/// </summary>
	// Token: 0x0200098E RID: 2446
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedCard : CardModel
	{
		/// <summary>
		/// Placeholder event if we can't find the card we are looking for (such as in the case that it's been deprecated)
		/// </summary>
		// Token: 0x06007091 RID: 28817 RVA: 0x002899FC File Offset: 0x00287BFC
		public DeprecatedCard()
			: base(0, CardType.Status, CardRarity.Status, TargetType.None, false)
		{
		}

		// Token: 0x17001D7F RID: 7551
		// (get) Token: 0x06007092 RID: 28818 RVA: 0x00289A09 File Offset: 0x00287C09
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D80 RID: 7552
		// (get) Token: 0x06007093 RID: 28819 RVA: 0x00289A0C File Offset: 0x00287C0C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007094 RID: 28820 RVA: 0x00289A14 File Offset: 0x00287C14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, 1m, base.Owner, false);
			if (base.DeckVersion != null)
			{
				await CardPileCmd.RemoveFromDeck(base.DeckVersion, true);
				base.DeckVersion = null;
			}
		}
	}
}
