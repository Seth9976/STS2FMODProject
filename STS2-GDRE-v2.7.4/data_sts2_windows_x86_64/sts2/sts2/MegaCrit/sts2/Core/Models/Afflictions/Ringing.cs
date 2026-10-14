using System;

namespace MegaCrit.Sts2.Core.Models.Afflictions
{
	/// <summary>
	/// Most of this Affliction's logic lives in <see cref="T:MegaCrit.Sts2.Core.Models.Powers.RingingPower" />.
	/// </summary>
	// Token: 0x02000B7A RID: 2938
	public sealed class Ringing : AfflictionModel
	{
		// Token: 0x170021F6 RID: 8694
		// (get) Token: 0x06007B0D RID: 31501 RVA: 0x002A02F9 File Offset: 0x0029E4F9
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}
	}
}
