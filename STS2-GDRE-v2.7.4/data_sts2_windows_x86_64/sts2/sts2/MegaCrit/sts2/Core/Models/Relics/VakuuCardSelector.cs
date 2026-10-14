using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	/// <summary>
	/// Card selector used by Vakuu (via WhisperingEarring) during auto-play.
	/// Selects cards in row-major order (top-left to bottom-right).
	/// </summary>
	// Token: 0x0200060B RID: 1547
	public class VakuuCardSelector : ICardSelector
	{
		// Token: 0x060055EE RID: 21998 RVA: 0x002498EF File Offset: 0x00247AEF
		[NullableContext(1)]
		public Task<IEnumerable<CardModel>> GetSelectedCards(IEnumerable<CardModel> options, int minSelect, int maxSelect)
		{
			return Task.FromResult<IEnumerable<CardModel>>(options.Take<CardModel>(maxSelect).ToList<CardModel>());
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x00249904 File Offset: 0x00247B04
		[NullableContext(1)]
		public CardRewardSelection GetSelectedCardReward(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives)
		{
			CardRewardSelection cardRewardSelection = default(CardRewardSelection);
			CardCreationResult cardCreationResult = options.FirstOrDefault<CardCreationResult>();
			cardRewardSelection.card = ((cardCreationResult != null) ? cardCreationResult.Card : null);
			return cardRewardSelection;
		}
	}
}
