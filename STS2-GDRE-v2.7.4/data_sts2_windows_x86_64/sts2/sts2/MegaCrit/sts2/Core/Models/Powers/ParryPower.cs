using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This power doesn't actually do anything on its own. Instead, the <see cref="T:MegaCrit.Sts2.Core.Models.Cards.SovereignBlade" /> card checks for its
	/// existence and modifies its block.
	/// </summary>
	// Token: 0x020006C6 RID: 1734
	public class ParryPower : PowerModel
	{
		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06005A13 RID: 23059 RVA: 0x002518D9 File Offset: 0x0024FAD9
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06005A14 RID: 23060 RVA: 0x002518DC File Offset: 0x0024FADC
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}
	}
}
