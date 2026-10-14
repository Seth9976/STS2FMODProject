using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MagnumOpusStory : StoryModel
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00013366 File Offset: 0x00011566
		protected override string Id
		{
			get
			{
				return "MAGNUM_OPUS";
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00013370 File Offset: 0x00011570
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Colorless1Epoch>(),
					EpochModel.Get<Relic1Epoch>(),
					EpochModel.Get<Colorless2Epoch>(),
					EpochModel.Get<CustomAndSeedsEpoch>(),
					EpochModel.Get<Relic2Epoch>(),
					EpochModel.Get<Colorless3Epoch>(),
					EpochModel.Get<Colorless4Epoch>(),
					EpochModel.Get<Relic4Epoch>(),
					EpochModel.Get<Event3Epoch>()
				};
			}
		}
	}
}
