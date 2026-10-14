using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Laser Beams
	/// </summary>
	// Token: 0x02000056 RID: 86
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect6Epoch : EpochModel
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00013D9D File Offset: 0x00011F9D
		public override string Id
		{
			get
			{
				return "DEFECT6_EPOCH";
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00013DA4 File Offset: 0x00011FA4
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight2;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00013DAB File Offset: 0x00011FAB
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00013DAE File Offset: 0x00011FAE
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00013DB8 File Offset: 0x00011FB8
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<GoldPlatedCables>();
				num2++;
				*span[num2] = ModelDb.Relic<EmotionChip>();
				num2++;
				*span[num2] = ModelDb.Relic<PowerCell>();
				return list;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00013E0F File Offset: 0x0001200F
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Defect6Epoch.Relics);
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00013E1C File Offset: 0x0001201C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Defect6Epoch.Relics);
		}
	}
}
