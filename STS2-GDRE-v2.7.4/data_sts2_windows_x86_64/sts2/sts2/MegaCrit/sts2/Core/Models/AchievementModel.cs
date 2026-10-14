using System;

namespace MegaCrit.Sts2.Core.Models
{
	/// <summary>
	/// Model which listens to events and unlocks an achievement when conditions are met.
	/// Note only combat-related achievements are unlocked this way. Some are unlocked via calls from nodes. Look at all uses
	/// of AchievementsUtil.Unlock to find all achievement unlocks.
	/// </summary>
	// Token: 0x020004D6 RID: 1238
	public abstract class AchievementModel : AbstractModel
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004A40 RID: 19008 RVA: 0x00226836 File Offset: 0x00224A36
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}
	}
}
