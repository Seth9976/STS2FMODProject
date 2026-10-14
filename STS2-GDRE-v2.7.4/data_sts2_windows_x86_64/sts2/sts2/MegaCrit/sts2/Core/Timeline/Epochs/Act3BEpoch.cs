using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Elsewhere: Placeholder
	/// </summary>
	// Token: 0x02000048 RID: 72
	[NullableContext(1)]
	[Nullable(0)]
	public class Act3BEpoch : EpochModel
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600029E RID: 670 RVA: 0x000135A5 File Offset: 0x000117A5
		public override string Id
		{
			get
			{
				return "ACT3_B_EPOCH";
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000135AC File Offset: 0x000117AC
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds0;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x000135AF File Offset: 0x000117AF
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000135B4 File Offset: 0x000117B4
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
