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
	/// My Face
	/// </summary>
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad7Epoch : EpochModel
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00014409 File Offset: 0x00012609
		public override string Id
		{
			get
			{
				return "IRONCLAD7_EPOCH";
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00014410 File Offset: 0x00012610
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation5;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00014417 File Offset: 0x00012617
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0001441A File Offset: 0x0001261A
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00014424 File Offset: 0x00012624
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<BloodWall>();
				num2++;
				*span[num2] = ModelDb.Card<TearAsunder>();
				num2++;
				*span[num2] = ModelDb.Card<Inferno>();
				return list;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0001447B File Offset: 0x0001267B
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Ironclad7Epoch.Cards);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00014488 File Offset: 0x00012688
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Ironclad7Epoch.Cards);
		}
	}
}
