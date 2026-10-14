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
	/// Futility
	/// </summary>
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder3Epoch : EpochModel
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000145F9 File Offset: 0x000127F9
		public override string Id
		{
			get
			{
				return "NECROBINDER3_EPOCH";
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00014600 File Offset: 0x00012800
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation1;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00014607 File Offset: 0x00012807
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0001460A File Offset: 0x0001280A
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00014614 File Offset: 0x00012814
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<BoneFlute>();
				num2++;
				*span[num2] = ModelDb.Relic<FuneraryMask>();
				num2++;
				*span[num2] = ModelDb.Relic<BookRepairKnife>();
				return list;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0001466B File Offset: 0x0001286B
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Necrobinder3Epoch.Relics);
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00014678 File Offset: 0x00012878
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Necrobinder3Epoch.Relics);
		}
	}
}
