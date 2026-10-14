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
	/// Ablaze
	/// </summary>
	// Token: 0x0200005E RID: 94
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad5Epoch : EpochModel
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000345 RID: 837 RVA: 0x000142DC File Offset: 0x000124DC
		public override string Id
		{
			get
			{
				return "IRONCLAD5_EPOCH";
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000142E3 File Offset: 0x000124E3
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight2;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000142EA File Offset: 0x000124EA
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000348 RID: 840 RVA: 0x000142ED File Offset: 0x000124ED
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000349 RID: 841 RVA: 0x000142F4 File Offset: 0x000124F4
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Cinder>();
				num2++;
				*span[num2] = ModelDb.Card<PactsEnd>();
				num2++;
				*span[num2] = ModelDb.Card<DrumOfBattle>();
				return list;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0001434B File Offset: 0x0001254B
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Ironclad5Epoch.Cards);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00014358 File Offset: 0x00012558
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Ironclad5Epoch.Cards);
		}
	}
}
