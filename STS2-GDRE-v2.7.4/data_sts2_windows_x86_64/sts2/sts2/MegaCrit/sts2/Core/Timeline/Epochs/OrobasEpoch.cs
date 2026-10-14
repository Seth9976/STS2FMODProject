using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Orobas
	/// </summary>
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	public class OrobasEpoch : EpochModel
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00014A60 File Offset: 0x00012C60
		public override string Id
		{
			get
			{
				return "OROBAS_EPOCH";
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00014A67 File Offset: 0x00012C67
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish0;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00014A6E File Offset: 0x00012C6E
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00014A71 File Offset: 0x00012C71
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00014A78 File Offset: 0x00012C78
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
