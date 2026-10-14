using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	// Token: 0x02000047 RID: 71
	[NullableContext(1)]
	[Nullable(0)]
	public class Act2BEpoch : EpochModel
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0001354F File Offset: 0x0001174F
		public override string Id
		{
			get
			{
				return "ACT2_B_EPOCH";
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00013556 File Offset: 0x00011756
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds3;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00013559 File Offset: 0x00011759
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0001355C File Offset: 0x0001175C
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
