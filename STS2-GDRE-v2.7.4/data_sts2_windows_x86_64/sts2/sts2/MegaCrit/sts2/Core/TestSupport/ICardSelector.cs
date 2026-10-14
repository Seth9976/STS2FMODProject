using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.TestSupport
{
	/// <summary>
	/// Interface for automated card selection, used by both test mode and AutoSlay.
	/// </summary>
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	public interface ICardSelector
	{
		/// <summary>
		/// Selects cards from the available options.
		/// </summary>
		/// <param name="options">Cards available for selection.</param>
		/// <param name="minSelect">Minimum number of cards to select.</param>
		/// <param name="maxSelect">Maximum number of cards to select.</param>
		// Token: 0x06000453 RID: 1107
		Task<IEnumerable<CardModel>> GetSelectedCards(IEnumerable<CardModel> options, int minSelect, int maxSelect);

		/// <summary>
		/// Selects a card reward from the available options.
		/// </summary>
		// Token: 0x06000454 RID: 1108
		CardRewardSelection GetSelectedCardReward(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives);
	}
}
