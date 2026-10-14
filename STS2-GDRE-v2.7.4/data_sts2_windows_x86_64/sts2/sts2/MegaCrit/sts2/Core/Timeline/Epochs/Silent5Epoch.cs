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
	/// Poisoner
	/// </summary>
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent5Epoch : EpochModel
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0001570C File Offset: 0x0001390C
		public override string Id
		{
			get
			{
				return "SILENT5_EPOCH";
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00015713 File Offset: 0x00013913
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight2;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001571A File Offset: 0x0001391A
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001571D File Offset: 0x0001391D
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00015724 File Offset: 0x00013924
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Reflex>();
				num2++;
				*span[num2] = ModelDb.Card<MasterPlanner>();
				num2++;
				*span[num2] = ModelDb.Card<HandTrick>();
				return list;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001577B File Offset: 0x0001397B
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Silent5Epoch.Cards);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00015788 File Offset: 0x00013988
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Silent5Epoch.Cards);
		}
	}
}
