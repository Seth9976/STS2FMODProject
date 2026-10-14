using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NecrobinderStory : StoryModel
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600028A RID: 650 RVA: 0x000133D4 File Offset: 0x000115D4
		protected override string Id
		{
			get
			{
				return "NECROBINDER";
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600028B RID: 651 RVA: 0x000133DB File Offset: 0x000115DB
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Necrobinder2Epoch>(),
					EpochModel.Get<Necrobinder3Epoch>(),
					EpochModel.Get<Necrobinder4Epoch>(),
					EpochModel.Get<Necrobinder5Epoch>(),
					EpochModel.Get<Necrobinder6Epoch>(),
					EpochModel.Get<Necrobinder1Epoch>(),
					EpochModel.Get<Necrobinder7Epoch>()
				};
			}
		}
	}
}
