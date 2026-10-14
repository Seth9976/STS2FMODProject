using System;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	/// <summary>
	/// This power doesn't actually do anything on its own. Instead, the <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Shiv" /> card checks for its existence
	/// and modifies its targeting.
	/// </summary>
	// Token: 0x02000671 RID: 1649
	public sealed class FanOfKnivesPower : PowerModel
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x0024E2DE File Offset: 0x0024C4DE
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x0024E2E1 File Offset: 0x0024C4E1
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}
	}
}
