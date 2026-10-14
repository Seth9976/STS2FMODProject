using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Represents a card cost that should only exist temporarily.
	/// Used for effects that set a card's cost to a specific value.
	/// For effects that modify a card's existing cost by a certain amount, see <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.TemporaryCardCostOffset" />.
	/// Used for both temporary energy and star card costs.
	/// </summary>
	// Token: 0x02000CC9 RID: 3273
	[NullableContext(1)]
	[Nullable(0)]
	public class TemporaryCardCost
	{
		/// <summary>
		/// The amount that this card's cost is set to.
		/// </summary>
		// Token: 0x17002454 RID: 9300
		// (get) Token: 0x060084F2 RID: 34034 RVA: 0x002CB40E File Offset: 0x002C960E
		// (set) Token: 0x060084F3 RID: 34035 RVA: 0x002CB416 File Offset: 0x002C9616
		public int Cost { get; private set; }

		/// <summary>
		/// Should this temporary cost be cleared when the turn ends?
		/// This is true for effects that say something like "It costs N _this turn_" (like Attack Potion).
		/// </summary>
		// Token: 0x17002455 RID: 9301
		// (get) Token: 0x060084F4 RID: 34036 RVA: 0x002CB41F File Offset: 0x002C961F
		// (set) Token: 0x060084F5 RID: 34037 RVA: 0x002CB427 File Offset: 0x002C9627
		public bool ClearsWhenTurnEnds { get; private set; }

		/// <summary>
		/// Should this temporary cost be cleared when the card is played?
		/// This is true for effects that say something like "It costs N _until played_" (like King's Gambit),
		/// AND for effects that say something like "It costs N _this turn_" (like Attack Potion).
		/// </summary>
		// Token: 0x17002456 RID: 9302
		// (get) Token: 0x060084F6 RID: 34038 RVA: 0x002CB430 File Offset: 0x002C9630
		// (set) Token: 0x060084F7 RID: 34039 RVA: 0x002CB438 File Offset: 0x002C9638
		public bool ClearsWhenCardIsPlayed { get; private set; }

		/// <summary>
		/// A temporary cost that is cleared only when the card is played.
		/// </summary>
		// Token: 0x060084F8 RID: 34040 RVA: 0x002CB441 File Offset: 0x002C9641
		public static TemporaryCardCost UntilPlayed(int cost)
		{
			return new TemporaryCardCost
			{
				Cost = Math.Max(cost, 0),
				ClearsWhenTurnEnds = false,
				ClearsWhenCardIsPlayed = true
			};
		}

		/// <summary>
		/// A temporary cost that is cleared when the card is played OR at the end of the current
		/// turn, whichever comes first.
		/// </summary>
		// Token: 0x060084F9 RID: 34041 RVA: 0x002CB463 File Offset: 0x002C9663
		public static TemporaryCardCost ThisTurn(int cost)
		{
			return new TemporaryCardCost
			{
				Cost = Math.Max(cost, 0),
				ClearsWhenTurnEnds = true,
				ClearsWhenCardIsPlayed = true
			};
		}

		/// <summary>
		/// A temporary cost that remains for the entire combat.
		/// </summary>
		// Token: 0x060084FA RID: 34042 RVA: 0x002CB485 File Offset: 0x002C9685
		public static TemporaryCardCost ThisCombat(int cost)
		{
			return new TemporaryCardCost
			{
				Cost = Math.Max(cost, 0),
				ClearsWhenTurnEnds = false,
				ClearsWhenCardIsPlayed = false
			};
		}
	}
}
