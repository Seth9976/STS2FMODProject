using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Seeds: Enamored by the growth and stability of flora and fauna.
	/// </summary>
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	[Nullable(0)]
	public class CustomAndSeedsEpoch : EpochModel
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00013925 File Offset: 0x00011B25
		public override string Id
		{
			get
			{
				return "CUSTOM_AND_SEEDS_EPOCH";
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0001392C File Offset: 0x00011B2C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds0;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0001392F File Offset: 0x00011B2F
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00013932 File Offset: 0x00011B32
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001393C File Offset: 0x00011B3C
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
