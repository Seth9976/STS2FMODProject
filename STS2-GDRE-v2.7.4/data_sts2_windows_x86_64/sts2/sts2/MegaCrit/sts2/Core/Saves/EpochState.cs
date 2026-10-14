using System;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// The status of which an SerializableEpoch can be in.
	/// The states are:
	///     Revealed: Complete!
	///     Obtained: The player has met the requirements to get this AND the slot is available in the Timeline.
	///     ObtainedNoSlot: The player has met the requirements to get this but the slot isn't visible.
	///     Unobtained: The player has unlocked the slot but has not yet obtained this Epoch.
	/// </summary>
	// Token: 0x02000096 RID: 150
	public enum EpochState
	{
		// Token: 0x0400019B RID: 411
		None,
		// Token: 0x0400019C RID: 412
		NoSlot,
		// Token: 0x0400019D RID: 413
		NotObtained,
		// Token: 0x0400019E RID: 414
		ObtainedNoSlot,
		// Token: 0x0400019F RID: 415
		Obtained,
		// Token: 0x040001A0 RID: 416
		Revealed
	}
}
