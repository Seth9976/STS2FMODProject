using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Underdocks
	/// </summary>
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	[Nullable(0)]
	public class UnderdocksEpoch : EpochModel
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000158D1 File Offset: 0x00013AD1
		public override string Id
		{
			get
			{
				return "UNDERDOCKS_EPOCH";
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000158D8 File Offset: 0x00013AD8
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation1;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000158DF File Offset: 0x00013ADF
		public override int EraPosition
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000158E2 File Offset: 0x00013AE2
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000158E9 File Offset: 0x00013AE9
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Colorless2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Relic2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Potion2Epoch>())
			};
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00015918 File Offset: 0x00013B18
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
