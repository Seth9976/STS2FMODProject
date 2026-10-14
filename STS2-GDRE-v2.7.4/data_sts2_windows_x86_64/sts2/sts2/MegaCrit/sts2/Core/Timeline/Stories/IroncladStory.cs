using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline.Stories
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IroncladStory : StoryModel
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0001331F File Offset: 0x0001151F
		protected override string Id
		{
			get
			{
				return "IRONCLAD";
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00013326 File Offset: 0x00011526
		public override EpochModel[] Epochs
		{
			get
			{
				return new EpochModel[]
				{
					EpochModel.Get<Ironclad4Epoch>(),
					EpochModel.Get<Ironclad3Epoch>(),
					EpochModel.Get<Ironclad2Epoch>(),
					EpochModel.Get<Ironclad5Epoch>(),
					EpochModel.Get<Ironclad6Epoch>(),
					EpochModel.Get<Ironclad7Epoch>()
				};
			}
		}
	}
}
