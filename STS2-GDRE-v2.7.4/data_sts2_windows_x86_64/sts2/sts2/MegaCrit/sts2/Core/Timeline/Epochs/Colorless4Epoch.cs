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
	/// Slumber
	/// </summary>
	// Token: 0x0200004C RID: 76
	[NullableContext(1)]
	[Nullable(0)]
	public class Colorless4Epoch : EpochModel
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002BC RID: 700 RVA: 0x000137F9 File Offset: 0x000119F9
		public override string Id
		{
			get
			{
				return "COLORLESS4_EPOCH";
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00013800 File Offset: 0x00011A00
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds3;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00013803 File Offset: 0x00011A03
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00013806 File Offset: 0x00011A06
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x00013810 File Offset: 0x00011A10
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Alchemize>();
				num2++;
				*span[num2] = ModelDb.Card<Nostalgia>();
				num2++;
				*span[num2] = ModelDb.Card<Scrawl>();
				return list;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00013867 File Offset: 0x00011A67
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Colorless4Epoch.Cards);
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00013874 File Offset: 0x00011A74
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Colorless4Epoch.Cards);
		}
	}
}
