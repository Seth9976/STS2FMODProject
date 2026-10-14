using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Neow
	/// </summary>
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public class NeowEpoch : EpochModel
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0001492D File Offset: 0x00012B2D
		public override string Id
		{
			get
			{
				return "NEOW_EPOCH";
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00014934 File Offset: 0x00012B34
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation0;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0001493B File Offset: 0x00012B3B
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0001493E File Offset: 0x00012B3E
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00014948 File Offset: 0x00012B48
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Colorless1Epoch>()),
				EpochModel.Get(EpochModel.GetId<CustomAndSeedsEpoch>()),
				EpochModel.Get(EpochModel.GetId<DailyRunEpoch>()),
				EpochModel.Get(EpochModel.GetId<DarvEpoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad6Epoch>()),
				EpochModel.Get(EpochModel.GetId<Ironclad7Epoch>()),
				EpochModel.Get(EpochModel.GetId<OrobasEpoch>()),
				EpochModel.Get(EpochModel.GetId<Silent1Epoch>())
			};
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000149FC File Offset: 0x00012BFC
		public override void QueueUnlocks()
		{
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
			SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Silent1Epoch>(), EpochState.ObtainedNoSlot);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
