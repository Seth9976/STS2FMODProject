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
	/// Traitors: One of the many Ancients, Neow, grows jealous of the awe-inspiring will of the Architect.
	/// </summary>
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	[Nullable(0)]
	public class Colorless3Epoch : EpochModel
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00013765 File Offset: 0x00011965
		public override string Id
		{
			get
			{
				return "COLORLESS3_EPOCH";
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0001376C File Offset: 0x0001196C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds2;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0001376F File Offset: 0x0001196F
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00013772 File Offset: 0x00011972
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0001377C File Offset: 0x0001197C
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Rend>();
				num2++;
				*span[num2] = ModelDb.Card<BeatDown>();
				num2++;
				*span[num2] = ModelDb.Card<Prowess>();
				return list;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x000137D3 File Offset: 0x000119D3
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Colorless3Epoch.Cards);
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000137E0 File Offset: 0x000119E0
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Colorless3Epoch.Cards);
		}
	}
}
