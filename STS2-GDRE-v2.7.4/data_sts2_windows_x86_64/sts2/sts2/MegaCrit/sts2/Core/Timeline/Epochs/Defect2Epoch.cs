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
	/// Tricked
	/// </summary>
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect2Epoch : EpochModel
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00013B02 File Offset: 0x00011D02
		public override string Id
		{
			get
			{
				return "DEFECT2_EPOCH";
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00013B09 File Offset: 0x00011D09
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation2;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00013B10 File Offset: 0x00011D10
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00013B13 File Offset: 0x00011D13
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00013B1C File Offset: 0x00011D1C
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Loop>();
				num2++;
				*span[num2] = ModelDb.Card<Null>();
				num2++;
				*span[num2] = ModelDb.Card<ConsumingShadow>();
				return list;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00013B73 File Offset: 0x00011D73
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Defect2Epoch.Cards);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00013B80 File Offset: 0x00011D80
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Defect2Epoch.Cards);
		}
	}
}
