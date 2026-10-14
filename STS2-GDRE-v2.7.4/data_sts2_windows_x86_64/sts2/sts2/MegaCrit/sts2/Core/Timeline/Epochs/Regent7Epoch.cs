using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Little King
	/// </summary>
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent7Epoch : EpochModel
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001501D File Offset: 0x0001321D
		public override string Id
		{
			get
			{
				return "REGENT7_EPOCH";
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00015024 File Offset: 0x00013224
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation5;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001502B File Offset: 0x0001322B
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0001502E File Offset: 0x0001322E
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00015038 File Offset: 0x00013238
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Patter>();
				num2++;
				*span[num2] = ModelDb.Card<HeavenlyDrill>();
				num2++;
				*span[num2] = ModelDb.Card<LunarBlast>();
				return list;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001508F File Offset: 0x0001328F
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Regent7Epoch.Cards);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001509C File Offset: 0x0001329C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Regent7Epoch.Cards);
		}
	}
}
