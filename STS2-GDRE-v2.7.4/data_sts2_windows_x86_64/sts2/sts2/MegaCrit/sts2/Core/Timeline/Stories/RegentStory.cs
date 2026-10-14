using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegentStory : StoryModel
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00013423 File Offset: 0x00011623
		protected override string Id
		{
			get
			{
				return "REGENT";
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0001342A File Offset: 0x0001162A
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Regent6Epoch>(),
					EpochModel.Get<Regent1Epoch>(),
					EpochModel.Get<Regent5Epoch>(),
					EpochModel.Get<Regent2Epoch>(),
					EpochModel.Get<Regent3Epoch>(),
					EpochModel.Get<Regent4Epoch>(),
					EpochModel.Get<Regent7Epoch>()
				};
			}
		}
	}
}
