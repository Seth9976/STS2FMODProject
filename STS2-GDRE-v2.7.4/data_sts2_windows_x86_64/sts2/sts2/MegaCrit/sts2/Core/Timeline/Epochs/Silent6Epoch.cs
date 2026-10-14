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
	/// Bigger Prey: The other sisters judge the fourth one in silence...
	/// </summary>
	// Token: 0x0200007D RID: 125
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent6Epoch : EpochModel
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x000157A1 File Offset: 0x000139A1
		public override string Id
		{
			get
			{
				return "SILENT6_EPOCH";
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000157A8 File Offset: 0x000139A8
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Prehistoria2;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000157AF File Offset: 0x000139AF
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000157B2 File Offset: 0x000139B2
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x000157BC File Offset: 0x000139BC
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<TwistedFunnel>();
				num2++;
				*span[num2] = ModelDb.Relic<SneckoSkull>();
				num2++;
				*span[num2] = ModelDb.Relic<HelicalDart>();
				return list;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00015813 File Offset: 0x00013A13
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Silent6Epoch.Relics);
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00015820 File Offset: 0x00013A20
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Silent6Epoch.Relics);
		}
	}
}
