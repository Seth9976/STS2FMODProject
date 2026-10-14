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
	/// Snuffed
	/// </summary>
	// Token: 0x0200005F RID: 95
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad6Epoch : EpochModel
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00014371 File Offset: 0x00012571
		public override string Id
		{
			get
			{
				return "IRONCLAD6_EPOCH";
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00014378 File Offset: 0x00012578
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish3;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0001437F File Offset: 0x0001257F
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00014382 File Offset: 0x00012582
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0001438C File Offset: 0x0001258C
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<SelfFormingClay>();
				num2++;
				*span[num2] = ModelDb.Relic<CharonsAshes>();
				num2++;
				*span[num2] = ModelDb.Relic<DemonTongue>();
				return list;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000352 RID: 850 RVA: 0x000143E3 File Offset: 0x000125E3
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Ironclad6Epoch.Relics);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000143F0 File Offset: 0x000125F0
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Ironclad6Epoch.Relics);
		}
	}
}
