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
	/// Expeditionists
	/// </summary>
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder2Epoch : EpochModel
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00014563 File Offset: 0x00012763
		public override string Id
		{
			get
			{
				return "NECROBINDER2_EPOCH";
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0001456A File Offset: 0x0001276A
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation0;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00014571 File Offset: 0x00012771
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00014574 File Offset: 0x00012774
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0001457C File Offset: 0x0001277C
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Scourge>();
				num2++;
				*span[num2] = ModelDb.Card<Oblivion>();
				num2++;
				*span[num2] = ModelDb.Card<Countdown>();
				return list;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000145D3 File Offset: 0x000127D3
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Necrobinder2Epoch.Cards);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000145E0 File Offset: 0x000127E0
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Necrobinder2Epoch.Cards);
		}
	}
}
