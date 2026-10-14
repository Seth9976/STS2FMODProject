using System;

namespace MegaCrit.Sts2.Core.Models
{
	/// <summary>
	/// Model which listens to events and keeps track of stats for badges.
	/// This holds no backing data for the badge itself. See <see cref="T:MegaCrit.Sts2.Core.Models.Badges.Badge" /> for that.
	/// Only a few badges use this because most unlocks can be computed from the SerializedRun at the end of the run.
	/// Badge could probably get merged into this at some point, but be wary, because the IDs don't match up with the IDs
	/// generated for models.
	/// </summary>
	// Token: 0x020004DA RID: 1242
	public abstract class BadgeModel : AbstractModel
	{
		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06004ADA RID: 19162 RVA: 0x00227F98 File Offset: 0x00226198
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}
	}
}
