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
	/// Devastation
	/// </summary>
	// Token: 0x0200005C RID: 92
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad3Epoch : EpochModel
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00014175 File Offset: 0x00012375
		public override string Id
		{
			get
			{
				return "IRONCLAD3_EPOCH";
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0001417C File Offset: 0x0001237C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds3;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0001417F File Offset: 0x0001237F
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00014182 File Offset: 0x00012382
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0001418C File Offset: 0x0001238C
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<RedSkull>();
				num2++;
				*span[num2] = ModelDb.Relic<PaperPhrog>();
				num2++;
				*span[num2] = ModelDb.Relic<RuinedHelmet>();
				return list;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600033A RID: 826 RVA: 0x000141E3 File Offset: 0x000123E3
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Ironclad3Epoch.Relics);
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000141F0 File Offset: 0x000123F0
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Ironclad3Epoch.Relics);
		}
	}
}
