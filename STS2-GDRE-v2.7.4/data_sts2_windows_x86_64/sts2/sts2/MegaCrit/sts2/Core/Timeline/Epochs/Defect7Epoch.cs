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
	/// Ooh Shiny!
	/// </summary>
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect7Epoch : EpochModel
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00013E35 File Offset: 0x00012035
		public override string Id
		{
			get
			{
				return "DEFECT7_EPOCH";
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00013E3C File Offset: 0x0001203C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation6;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00013E43 File Offset: 0x00012043
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00013E46 File Offset: 0x00012046
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00013E50 File Offset: 0x00012050
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Turbo>();
				num2++;
				*span[num2] = ModelDb.Card<Scavenge>();
				num2++;
				*span[num2] = ModelDb.Card<HelixDrill>();
				return list;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00013EA7 File Offset: 0x000120A7
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Defect7Epoch.Cards);
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00013EB4 File Offset: 0x000120B4
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Defect7Epoch.Cards);
		}
	}
}
