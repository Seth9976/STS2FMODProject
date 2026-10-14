using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006AF RID: 1711
	public sealed class MinionPower : PowerModel
	{
		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06005986 RID: 22918 RVA: 0x00250A4B File Offset: 0x0024EC4B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06005987 RID: 22919 RVA: 0x00250A4E File Offset: 0x0024EC4E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x00250A51 File Offset: 0x0024EC51
		public override bool ShouldPlayVfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06005989 RID: 22921 RVA: 0x00250A54 File Offset: 0x0024EC54
		public override bool OwnerIsSecondaryEnemy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x00250A57 File Offset: 0x0024EC57
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x00250A5A File Offset: 0x0024EC5A
		public override bool ShouldOwnerDeathTriggerFatal()
		{
			return false;
		}
	}
}
