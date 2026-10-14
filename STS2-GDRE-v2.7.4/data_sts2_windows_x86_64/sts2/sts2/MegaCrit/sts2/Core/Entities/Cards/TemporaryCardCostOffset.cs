using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Represents a card cost offset that should only exist temporarily.
	/// Used for effects that modify a card's cost by a certain amount.
	/// For effects that set a card's cost to a specific value, see <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.TemporaryCardCost" />.
	/// </summary>
	// Token: 0x02000CCA RID: 3274
	[NullableContext(1)]
	[Nullable(0)]
	public class TemporaryCardCostOffset
	{
		/// <summary>
		/// The amount that this card's cost is modified by.
		/// Positive for cost increases, negative for cost decreases.
		/// </summary>
		// Token: 0x17002457 RID: 9303
		// (get) Token: 0x060084FC RID: 34044 RVA: 0x002CB4AF File Offset: 0x002C96AF
		// (set) Token: 0x060084FD RID: 34045 RVA: 0x002CB4B7 File Offset: 0x002C96B7
		public int Offset { get; private set; }

		/// <summary>
		/// Should this temporary cost offset be cleared when the turn ends?
		/// This is true for effects that say something like "It costs N more _this turn_".
		/// </summary>
		// Token: 0x17002458 RID: 9304
		// (get) Token: 0x060084FE RID: 34046 RVA: 0x002CB4C0 File Offset: 0x002C96C0
		// (set) Token: 0x060084FF RID: 34047 RVA: 0x002CB4C8 File Offset: 0x002C96C8
		public bool ClearsWhenTurnEnds { get; private set; }

		/// <summary>
		/// Should this temporary cost offset be cleared when the card is played?
		/// This is true for effects that say something like "It costs N more _until played_",
		/// AND for effects that say something like "It costs N more _this turn_".
		/// </summary>
		// Token: 0x17002459 RID: 9305
		// (get) Token: 0x06008500 RID: 34048 RVA: 0x002CB4D1 File Offset: 0x002C96D1
		// (set) Token: 0x06008501 RID: 34049 RVA: 0x002CB4D9 File Offset: 0x002C96D9
		public bool ClearsWhenCardIsPlayed { get; private set; }

		/// <summary>
		/// A temporary cost offset that is cleared only when the card is played.
		/// </summary>
		// Token: 0x06008502 RID: 34050 RVA: 0x002CB4E2 File Offset: 0x002C96E2
		public static TemporaryCardCostOffset UntilPlayed(int offset)
		{
			return new TemporaryCardCostOffset
			{
				Offset = offset,
				ClearsWhenTurnEnds = false,
				ClearsWhenCardIsPlayed = true
			};
		}

		/// <summary>
		/// A temporary cost offset that is cleared when the card is played OR at the end of the current
		/// turn, whichever comes first.
		/// </summary>
		// Token: 0x06008503 RID: 34051 RVA: 0x002CB4FE File Offset: 0x002C96FE
		public static TemporaryCardCostOffset ThisTurn(int offset)
		{
			return new TemporaryCardCostOffset
			{
				Offset = offset,
				ClearsWhenTurnEnds = true,
				ClearsWhenCardIsPlayed = true
			};
		}

		/// <summary>
		/// A temporary cost offset that remains for the entire combat.
		/// </summary>
		// Token: 0x06008504 RID: 34052 RVA: 0x002CB51A File Offset: 0x002C971A
		public static TemporaryCardCostOffset ThisCombat(int offset)
		{
			return new TemporaryCardCostOffset
			{
				Offset = offset,
				ClearsWhenTurnEnds = false,
				ClearsWhenCardIsPlayed = false
			};
		}
	}
}
