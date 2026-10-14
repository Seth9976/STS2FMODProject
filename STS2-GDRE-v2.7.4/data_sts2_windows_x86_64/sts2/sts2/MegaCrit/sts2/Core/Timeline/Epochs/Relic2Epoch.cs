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
	/// Ancients: The Ancient Trees are not all the same. Each individual tree is shaped by its surroundings.
	/// </summary>
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	public class Relic2Epoch : EpochModel
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001514D File Offset: 0x0001334D
		public override string Id
		{
			get
			{
				return "RELIC2_EPOCH";
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00015154 File Offset: 0x00013354
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds1;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00015157 File Offset: 0x00013357
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001515A File Offset: 0x0001335A
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00015164 File Offset: 0x00013364
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<BookOfFiveRings>();
				num2++;
				*span[num2] = ModelDb.Relic<IceCream>();
				num2++;
				*span[num2] = ModelDb.Relic<Kusarigama>();
				return list;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000151BB File Offset: 0x000133BB
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Relic2Epoch.Relics);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000151C8 File Offset: 0x000133C8
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Relic2Epoch.Relics);
		}
	}
}
