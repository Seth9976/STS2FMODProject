using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	/// <summary>
	/// A special version of <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar" /> that is used to represent base value in a calculated value card.
	/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" /> uses this for its 6 base damage.
	/// </summary>
	// Token: 0x02000BDD RID: 3037
	public class CalculationBaseVar : DynamicVar
	{
		// Token: 0x06007E01 RID: 32257 RVA: 0x002AD6A8 File Offset: 0x002AB8A8
		public CalculationBaseVar(decimal baseValue)
			: base("CalculationBase", baseValue)
		{
		}

		// Token: 0x04002A26 RID: 10790
		[Nullable(1)]
		public const string defaultName = "CalculationBase";
	}
}
