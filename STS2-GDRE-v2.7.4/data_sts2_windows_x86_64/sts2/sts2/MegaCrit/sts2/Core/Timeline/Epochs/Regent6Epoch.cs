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
	/// Discontent
	/// </summary>
	// Token: 0x02000071 RID: 113
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent6Epoch : EpochModel
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00014F85 File Offset: 0x00013185
		public override string Id
		{
			get
			{
				return "REGENT6_EPOCH";
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00014F8C File Offset: 0x0001318C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish1;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00014F93 File Offset: 0x00013193
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00014F96 File Offset: 0x00013196
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00014FA0 File Offset: 0x000131A0
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<Regalite>();
				num2++;
				*span[num2] = ModelDb.Relic<MiniRegent>();
				num2++;
				*span[num2] = ModelDb.Relic<OrangeDough>();
				return list;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00014FF7 File Offset: 0x000131F7
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Regent6Epoch.Relics);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00015004 File Offset: 0x00013204
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Regent6Epoch.Relics);
		}
	}
}
