using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefectStory : StoryModel
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000132D0 File Offset: 0x000114D0
		protected override string Id
		{
			get
			{
				return "DEFECT";
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000282 RID: 642 RVA: 0x000132D7 File Offset: 0x000114D7
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Defect6Epoch>(),
					EpochModel.Get<Defect3Epoch>(),
					EpochModel.Get<Defect4Epoch>(),
					EpochModel.Get<Defect5Epoch>(),
					EpochModel.Get<Defect1Epoch>(),
					EpochModel.Get<Defect2Epoch>(),
					EpochModel.Get<Defect7Epoch>()
				};
			}
		}
	}
}
