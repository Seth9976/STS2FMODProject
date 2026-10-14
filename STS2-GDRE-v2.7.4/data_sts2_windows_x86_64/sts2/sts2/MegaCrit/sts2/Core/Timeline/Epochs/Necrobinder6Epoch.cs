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
	/// Annihilation
	/// </summary>
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder6Epoch : EpochModel
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000147FD File Offset: 0x000129FD
		public override string Id
		{
			get
			{
				return "NECROBINDER6_EPOCH";
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00014804 File Offset: 0x00012A04
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation4;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001480B File Offset: 0x00012A0B
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0001480E File Offset: 0x00012A0E
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00014818 File Offset: 0x00012A18
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<IvoryTile>();
				num2++;
				*span[num2] = ModelDb.Relic<BigHat>();
				num2++;
				*span[num2] = ModelDb.Relic<Bookmark>();
				return list;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0001486F File Offset: 0x00012A6F
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Necrobinder6Epoch.Relics);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001487C File Offset: 0x00012A7C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Necrobinder6Epoch.Relics);
		}
	}
}
