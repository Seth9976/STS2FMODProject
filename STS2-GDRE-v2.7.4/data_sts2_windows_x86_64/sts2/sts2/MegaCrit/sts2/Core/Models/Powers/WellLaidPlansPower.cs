using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200072F RID: 1839
	public sealed class WellLaidPlansPower : PowerModel
	{
		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x06005CAC RID: 23724 RVA: 0x00256EDB File Offset: 0x002550DB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x06005CAD RID: 23725 RVA: 0x00256EDE File Offset: 0x002550DE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x00256EE1 File Offset: 0x002550E1
		[NullableContext(1)]
		public override bool ShouldFlush(Player player)
		{
			return player != base.Owner.Player;
		}
	}
}
