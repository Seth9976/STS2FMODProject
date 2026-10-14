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
	/// The Giant
	/// </summary>
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder5Epoch : EpochModel
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00014768 File Offset: 0x00012968
		public override string Id
		{
			get
			{
				return "NECROBINDER5_EPOCH";
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0001476F File Offset: 0x0001296F
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation3;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00014776 File Offset: 0x00012976
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00014779 File Offset: 0x00012979
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00014780 File Offset: 0x00012980
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Afterlife>();
				num2++;
				*span[num2] = ModelDb.Card<Sacrifice>();
				num2++;
				*span[num2] = ModelDb.Card<Calcify>();
				return list;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000381 RID: 897 RVA: 0x000147D7 File Offset: 0x000129D7
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Necrobinder5Epoch.Cards);
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000147E4 File Offset: 0x000129E4
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Necrobinder5Epoch.Cards);
		}
	}
}
