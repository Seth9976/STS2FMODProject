using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000046 RID: 70
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheReopeningStory : StoryModel
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00013508 File Offset: 0x00011708
		protected override string Id
		{
			get
			{
				return "REOPENING";
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0001350F File Offset: 0x0001170F
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Potion2Epoch>(),
					EpochModel.Get<Relic5Epoch>(),
					EpochModel.Get<Relic3Epoch>(),
					EpochModel.Get<Potion1Epoch>(),
					EpochModel.Get<NeowEpoch>(),
					EpochModel.Get<Event2Epoch>()
				};
			}
		}
	}
}
