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
	/// Bigger Prey
	/// </summary>
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent2Epoch : EpochModel
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00015507 File Offset: 0x00013707
		public override string Id
		{
			get
			{
				return "SILENT2_EPOCH";
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001550E File Offset: 0x0001370E
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish0;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00015515 File Offset: 0x00013715
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00015518 File Offset: 0x00013718
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00015520 File Offset: 0x00013720
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Snakebite>();
				num2++;
				*span[num2] = ModelDb.Card<BubbleBubble>();
				num2++;
				*span[num2] = ModelDb.Card<Accelerant>();
				return list;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00015577 File Offset: 0x00013777
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Silent2Epoch.Cards);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00015584 File Offset: 0x00013784
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Silent2Epoch.Cards);
		}
	}
}
