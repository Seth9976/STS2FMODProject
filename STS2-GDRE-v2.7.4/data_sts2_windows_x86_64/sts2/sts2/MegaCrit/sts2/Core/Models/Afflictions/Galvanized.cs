using System;

namespace MegaCrit.Sts2.Core.Models.Afflictions
{
	/// <summary>
	/// Most of this Affliction's logic lives in <see cref="T:MegaCrit.Sts2.Core.Models.Powers.GalvanicPower" />.
	/// </summary>
	// Token: 0x02000B78 RID: 2936
	public sealed class Galvanized : AfflictionModel
	{
		// Token: 0x170021F3 RID: 8691
		// (get) Token: 0x06007B07 RID: 31495 RVA: 0x002A029D File Offset: 0x0029E49D
		public override bool IsStackable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021F4 RID: 8692
		// (get) Token: 0x06007B08 RID: 31496 RVA: 0x002A02A0 File Offset: 0x0029E4A0
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}
	}
}
