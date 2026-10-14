using System;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B93 RID: 2963
	// (Invoke) Token: 0x06007BBE RID: 31678
	public delegate IEnumerable<AbstractModel> CombatHookSubscriptionDelegate(CombatState combatState);
}
