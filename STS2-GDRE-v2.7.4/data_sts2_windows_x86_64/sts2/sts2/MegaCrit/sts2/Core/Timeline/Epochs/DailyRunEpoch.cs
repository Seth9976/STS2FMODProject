using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// The Daily
	/// </summary>
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	public class DailyRunEpoch : EpochModel
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00013985 File Offset: 0x00011B85
		public override string Id
		{
			get
			{
				return "DAILY_RUN_EPOCH";
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0001398C File Offset: 0x00011B8C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation6;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00013993 File Offset: 0x00011B93
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00013996 File Offset: 0x00011B96
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000139A0 File Offset: 0x00011BA0
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
