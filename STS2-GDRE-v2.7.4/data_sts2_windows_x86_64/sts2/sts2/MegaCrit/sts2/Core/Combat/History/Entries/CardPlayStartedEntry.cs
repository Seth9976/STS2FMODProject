using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D50 RID: 3408
	[NullableContext(1)]
	[Nullable(0)]
	public class CardPlayStartedEntry : CombatHistoryEntry
	{
		/// <summary>
		/// The instance of the card play that started.
		/// </summary>
		// Token: 0x170025E0 RID: 9696
		// (get) Token: 0x06008A07 RID: 35335 RVA: 0x002E3AF6 File Offset: 0x002E1CF6
		public CardPlay CardPlay { get; }

		// Token: 0x170025E1 RID: 9697
		// (get) Token: 0x06008A08 RID: 35336 RVA: 0x002E3B00 File Offset: 0x002E1D00
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Actor.Player.Character.Id.Entry + " started playing " + this.CardPlay.Card.Id.Entry);
				if (this.CardPlay.Target != null)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral(" targeting ");
					appendInterpolatedStringHandler.AppendFormatted(this.CardPlay.Target.Monster.Id.Entry);
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06008A09 RID: 35337 RVA: 0x002E3BA2 File Offset: 0x002E1DA2
		public CardPlayStartedEntry(CardPlay cardPlay, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(cardPlay.Card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.CardPlay = cardPlay;
		}
	}
}
