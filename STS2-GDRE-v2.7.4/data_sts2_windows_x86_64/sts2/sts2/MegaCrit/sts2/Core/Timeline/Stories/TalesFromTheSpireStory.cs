using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TalesFromTheSpireStory : StoryModel
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000293 RID: 659 RVA: 0x000134C1 File Offset: 0x000116C1
		protected override string Id
		{
			get
			{
				return "TALES_FROM_THE_SPIRE";
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000294 RID: 660 RVA: 0x000134C8 File Offset: 0x000116C8
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Colorless5Epoch>(),
					EpochModel.Get<OrobasEpoch>(),
					EpochModel.Get<Event1Epoch>(),
					EpochModel.Get<UnderdocksEpoch>(),
					EpochModel.Get<DarvEpoch>(),
					EpochModel.Get<DailyRunEpoch>()
				};
			}
		}
	}
}
