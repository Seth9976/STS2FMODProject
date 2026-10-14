using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Darv
	/// </summary>
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	public class DarvEpoch : EpochModel
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x000139E9 File Offset: 0x00011BE9
		public override string Id
		{
			get
			{
				return "DARV_EPOCH";
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x000139F0 File Offset: 0x00011BF0
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation4;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000139F7 File Offset: 0x00011BF7
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002DB RID: 731 RVA: 0x000139FA File Offset: 0x00011BFA
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00013A04 File Offset: 0x00011C04
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
