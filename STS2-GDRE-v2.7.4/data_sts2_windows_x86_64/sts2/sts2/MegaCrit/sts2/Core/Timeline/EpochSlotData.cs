using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	[Nullable(0)]
	public class EpochSlotData
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000126DF File Offset: 0x000108DF
		public EpochModel Model { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000126E7 File Offset: 0x000108E7
		public EpochSlotState State { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000126EF File Offset: 0x000108EF
		public EpochEra Era { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000126F7 File Offset: 0x000108F7
		public int EraPosition { get; }

		// Token: 0x06000266 RID: 614 RVA: 0x000126FF File Offset: 0x000108FF
		public EpochSlotData(string modelId, EpochSlotState state)
		{
			this.Model = EpochModel.Get(modelId);
			this.Era = this.Model.Era;
			this.EraPosition = this.Model.EraPosition;
			this.State = state;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0001273C File Offset: 0x0001093C
		public EpochSlotData(EpochModel model, EpochSlotState state)
		{
			this.Model = model;
			this.Era = this.Model.Era;
			this.EraPosition = this.Model.EraPosition;
			this.State = state;
		}
	}
}
