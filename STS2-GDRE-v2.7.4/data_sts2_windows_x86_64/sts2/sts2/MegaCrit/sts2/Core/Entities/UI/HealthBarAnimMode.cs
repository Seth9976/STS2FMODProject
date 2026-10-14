using System;

namespace MegaCrit.Sts2.Core.Entities.UI
{
	// Token: 0x02000C58 RID: 3160
	public enum HealthBarAnimMode
	{
		// Token: 0x04002BA7 RID: 11175
		None,
		/// <summary>
		/// Enemy was spawned at combat start, and their HP bar should animate in after a randomized small delay
		/// </summary>
		// Token: 0x04002BA8 RID: 11176
		SpawnedAtCombatStart,
		/// <summary>
		/// Enemy was spawned in a new space during combat, and their HP bar should animate in immediately, but slowly
		/// </summary>
		// Token: 0x04002BA9 RID: 11177
		SpawnedDuringCombat,
		/// <summary>
		/// Enemy already exists and we're animating its HP bar in from a hidden state
		/// </summary>
		// Token: 0x04002BAA RID: 11178
		FromHidden
	}
}
