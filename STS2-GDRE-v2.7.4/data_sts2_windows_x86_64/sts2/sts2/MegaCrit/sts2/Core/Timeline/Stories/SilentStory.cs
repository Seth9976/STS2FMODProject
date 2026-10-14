using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilentStory : StoryModel
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00013472 File Offset: 0x00011672
		protected override string Id
		{
			get
			{
				return "SILENT";
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00013479 File Offset: 0x00011679
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Silent6Epoch>(),
					EpochModel.Get<Silent4Epoch>(),
					EpochModel.Get<Silent5Epoch>(),
					EpochModel.Get<Silent2Epoch>(),
					EpochModel.Get<Silent3Epoch>(),
					EpochModel.Get<Silent1Epoch>(),
					EpochModel.Get<Silent7Epoch>()
				};
			}
		}
	}
}
