using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Odds
{
	// Token: 0x0200016E RID: 366
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AbstractOdds
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x00064841 File Offset: 0x00062A41
		protected AbstractOdds(float initialValue, Rng rng)
		{
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x00064857 File Offset: 0x00062A57
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x0006485F File Offset: 0x00062A5F
		public float CurrentValue { get; protected set; } = initialValue;

		/// <summary>
		/// Sets CurrentValue. Should be used only in instances where we're loading the player from another source, such as
		/// multiplayer host.
		/// </summary>
		/// <param name="newValue">The new value.</param>
		// Token: 0x06001129 RID: 4393 RVA: 0x00064868 File Offset: 0x00062A68
		public void OverrideCurrentValue(float newValue)
		{
			this.CurrentValue = newValue;
		}

		// Token: 0x04000617 RID: 1559
		protected readonly Rng _rng = rng;
	}
}
