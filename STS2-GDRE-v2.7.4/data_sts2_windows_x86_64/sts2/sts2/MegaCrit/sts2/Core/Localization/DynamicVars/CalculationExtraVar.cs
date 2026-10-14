using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	/// <summary>
	/// A special version of <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar" /> that is used to represent extra value in a calculated value card.
	/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" /> uses this for its 2 extra damage done for each Strike.
	/// </summary>
	// Token: 0x02000BDE RID: 3038
	public class CalculationExtraVar : DynamicVar
	{
		// Token: 0x06007E02 RID: 32258 RVA: 0x002AD6B6 File Offset: 0x002AB8B6
		public CalculationExtraVar(decimal baseValue)
			: base("CalculationExtra", baseValue)
		{
		}

		// Token: 0x04002A27 RID: 10791
		[Nullable(1)]
		public const string defaultName = "CalculationExtra";
	}
}
