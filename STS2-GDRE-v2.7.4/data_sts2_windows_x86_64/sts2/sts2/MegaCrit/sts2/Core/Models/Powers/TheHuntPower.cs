using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This power acts as a visual indicator to the player that the Hunt card was successful. The actual behavior
	/// lives in <see cref="T:MegaCrit.Sts2.Core.Models.Cards.TheHunt" /> .
	/// </summary>
	// Token: 0x0200071B RID: 1819
	public sealed class TheHuntPower : PowerModel
	{
		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06005C36 RID: 23606 RVA: 0x00255EBC File Offset: 0x002540BC
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06005C37 RID: 23607 RVA: 0x00255EBF File Offset: 0x002540BF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}
	}
}
