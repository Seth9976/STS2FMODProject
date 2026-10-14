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
	/// The Ironclad
	/// </summary>
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad2Epoch : EpochModel
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000140DF File Offset: 0x000122DF
		public override string Id
		{
			get
			{
				return "IRONCLAD2_EPOCH";
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000140E6 File Offset: 0x000122E6
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight1;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000140ED File Offset: 0x000122ED
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000140F0 File Offset: 0x000122F0
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000140F8 File Offset: 0x000122F8
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<MoltenFist>();
				num2++;
				*span[num2] = ModelDb.Card<Cruelty>();
				num2++;
				*span[num2] = ModelDb.Card<Dominate>();
				return list;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0001414F File Offset: 0x0001234F
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Ironclad2Epoch.Cards);
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001415C File Offset: 0x0001235C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Ironclad2Epoch.Cards);
		}
	}
}
