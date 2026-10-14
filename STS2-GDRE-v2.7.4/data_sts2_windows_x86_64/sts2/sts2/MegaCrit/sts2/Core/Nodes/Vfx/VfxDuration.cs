using System;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Various VFX may allow a shorter or longer version, this enum allows specifying the duration based on feel and not
	/// exact numbers (as they may be scaled based on the game speed)
	/// </summary>
	// Token: 0x02000213 RID: 531
	public enum VfxDuration
	{
		// Token: 0x04000A4C RID: 2636
		None,
		// Token: 0x04000A4D RID: 2637
		VeryShort,
		// Token: 0x04000A4E RID: 2638
		Short,
		// Token: 0x04000A4F RID: 2639
		Standard,
		// Token: 0x04000A50 RID: 2640
		Long,
		// Token: 0x04000A51 RID: 2641
		VeryLong,
		// Token: 0x04000A52 RID: 2642
		Custom,
		// Token: 0x04000A53 RID: 2643
		Forever
	}
}
