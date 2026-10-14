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
	/// Ill
	/// </summary>
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent3Epoch : EpochModel
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001559D File Offset: 0x0001379D
		public override string Id
		{
			get
			{
				return "SILENT3_EPOCH";
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000155A4 File Offset: 0x000137A4
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish3;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000155AB File Offset: 0x000137AB
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x000155AE File Offset: 0x000137AE
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x000155B8 File Offset: 0x000137B8
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<ToughBandages>();
				num2++;
				*span[num2] = ModelDb.Relic<PaperKrane>();
				num2++;
				*span[num2] = ModelDb.Relic<Tingsha>();
				return list;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0001560F File Offset: 0x0001380F
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Silent3Epoch.Relics);
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001561C File Offset: 0x0001381C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Silent3Epoch.Relics);
		}
	}
}
