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
	/// We Need a Hero!
	/// </summary>
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent3Epoch : EpochModel
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00014D81 File Offset: 0x00012F81
		public override string Id
		{
			get
			{
				return "REGENT3_EPOCH";
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00014D88 File Offset: 0x00012F88
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation2;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00014D8F File Offset: 0x00012F8F
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00014D92 File Offset: 0x00012F92
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00014D9C File Offset: 0x00012F9C
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<FencingManual>();
				num2++;
				*span[num2] = ModelDb.Relic<GalacticDust>();
				num2++;
				*span[num2] = ModelDb.Relic<LunarPastry>();
				return list;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00014DF3 File Offset: 0x00012FF3
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Regent3Epoch.Relics);
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014E00 File Offset: 0x00013000
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Regent3Epoch.Relics);
		}
	}
}
