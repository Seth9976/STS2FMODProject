using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4F RID: 3407
	[NullableContext(1)]
	[Nullable(0)]
	public class CardPlayFinishedEntry : CombatHistoryEntry
	{
		/// <summary>
		/// The instance of the card play that finished.
		/// </summary>
		// Token: 0x170025DD RID: 9693
		// (get) Token: 0x06008A03 RID: 35331 RVA: 0x002E3A05 File Offset: 0x002E1C05
		public CardPlay CardPlay { get; }

		/// <summary>
		/// Tracks if the card was Ethereal at the time it was played.
		///
		/// This is a HACK in order to make <see cref="T:MegaCrit.Sts2.Core.Models.Cards.BansheesCry" /> work.
		/// If we end up with more WasX properties like this, we should add some sort of snapshot mechanism so that
		/// <see cref="P:MegaCrit.Sts2.Core.Combat.History.Entries.CardPlayFinishedEntry.CardPlay" /> represents the state of the card at the time it was played.
		/// </summary>
		// Token: 0x170025DE RID: 9694
		// (get) Token: 0x06008A04 RID: 35332 RVA: 0x002E3A0D File Offset: 0x002E1C0D
		public bool WasEthereal { get; }

		// Token: 0x170025DF RID: 9695
		// (get) Token: 0x06008A05 RID: 35333 RVA: 0x002E3A18 File Offset: 0x002E1C18
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Actor.Player.Character.Id.Entry + " played " + this.CardPlay.Card.Id.Entry);
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

		// Token: 0x06008A06 RID: 35334 RVA: 0x002E3ABA File Offset: 0x002E1CBA
		public CardPlayFinishedEntry(CardPlay cardPlay, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(cardPlay.Card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.CardPlay = cardPlay;
			this.WasEthereal = cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal);
		}
	}
}
