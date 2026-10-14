using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.AutoSlay.Helpers
{
	/// <summary>
	/// Handles automatic card selection during AutoSlay runs.
	/// Randomly selects cards when the game would normally show a selection UI.
	/// </summary>
	// Token: 0x02000D6E RID: 3438
	[NullableContext(1)]
	[Nullable(0)]
	public class AutoSlayCardSelector : ICardSelector
	{
		// Token: 0x06008AF9 RID: 35577 RVA: 0x002E682A File Offset: 0x002E4A2A
		public AutoSlayCardSelector(Rng random)
		{
			this._random = random;
		}

		/// <summary>
		/// Randomly selects cards from the available options based on the selector preferences.
		/// </summary>
		// Token: 0x06008AFA RID: 35578 RVA: 0x002E683C File Offset: 0x002E4A3C
		public Task<IEnumerable<CardModel>> GetSelectedCards(IEnumerable<CardModel> options, int minSelect, int maxSelect)
		{
			List<CardModel> list = options.ToList<CardModel>();
			if (list.Count == 0)
			{
				return Task.FromResult<IEnumerable<CardModel>>(Array.Empty<CardModel>());
			}
			int num = Math.Min(maxSelect, list.Count);
			if (num < minSelect)
			{
				num = Math.Min(minSelect, list.Count);
			}
			this._random.Shuffle<CardModel>(list);
			IEnumerable<CardModel> enumerable = list.Take<CardModel>(num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Auto-selected ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" card(s) for selection prompt");
			AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			return Task.FromResult<IEnumerable<CardModel>>(enumerable);
		}

		/// <summary>
		/// Randomly selects a card reward from the available options.
		/// </summary>
		// Token: 0x06008AFB RID: 35579 RVA: 0x002E68D4 File Offset: 0x002E4AD4
		public CardRewardSelection GetSelectedCardReward(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives)
		{
			if (options.Count == 0)
			{
				return default(CardRewardSelection);
			}
			int num = this._random.NextInt(options.Count);
			return new CardRewardSelection
			{
				card = options[num].Card,
				alternative = null
			};
		}

		// Token: 0x0400303D RID: 12349
		private readonly Rng _random;
	}
}
