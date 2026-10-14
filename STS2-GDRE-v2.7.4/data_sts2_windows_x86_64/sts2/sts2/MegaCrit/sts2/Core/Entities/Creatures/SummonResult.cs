using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Entities.Creatures
{
	/// <summary>
	/// A class containing meta-info about the result of a Summon.
	/// </summary>
	// Token: 0x02000CA5 RID: 3237
	[NullableContext(2)]
	[Nullable(0)]
	public class SummonResult
	{
		/// <summary>
		/// The creature that was summoned.
		/// Null if something blocks the summon.
		/// </summary>
		// Token: 0x1700242E RID: 9262
		// (get) Token: 0x0600846E RID: 33902 RVA: 0x002C9D9C File Offset: 0x002C7F9C
		// (set) Token: 0x0600846F RID: 33903 RVA: 0x002C9DA4 File Offset: 0x002C7FA4
		public Creature Creature { get; set; }

		/// <summary>
		/// The amount that they were summoned for.
		/// 0 if something blocks the summon.
		/// </summary>
		// Token: 0x1700242F RID: 9263
		// (get) Token: 0x06008470 RID: 33904 RVA: 0x002C9DAD File Offset: 0x002C7FAD
		// (set) Token: 0x06008471 RID: 33905 RVA: 0x002C9DB5 File Offset: 0x002C7FB5
		public decimal Amount { get; set; }

		// Token: 0x06008472 RID: 33906 RVA: 0x002C9DBE File Offset: 0x002C7FBE
		public SummonResult(Creature creature, decimal amount)
		{
			this.Creature = creature;
			this.Amount = amount;
		}
	}
}
