using System;

namespace MegaCrit.Sts2.Core.Models.Afflictions
{
	/// <summary>
	/// Most of this Affliction's logic lives in <see cref="T:MegaCrit.Sts2.Core.Models.Powers.ChainsOfBindingPower" />.
	/// </summary>
	// Token: 0x02000B76 RID: 2934
	public sealed class Bound : AfflictionModel
	{
		// Token: 0x170021F2 RID: 8690
		// (get) Token: 0x06007B04 RID: 31492 RVA: 0x002A028A File Offset: 0x0029E48A
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}
	}
}
