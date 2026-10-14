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
	/// To Kill
	/// </summary>
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder7Epoch : EpochModel
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00014895 File Offset: 0x00012A95
		public override string Id
		{
			get
			{
				return "NECROBINDER7_EPOCH";
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0001489C File Offset: 0x00012A9C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation7;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000148A3 File Offset: 0x00012AA3
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000148A6 File Offset: 0x00012AA6
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000148B0 File Offset: 0x00012AB0
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<SculptingStrike>();
				num2++;
				*span[num2] = ModelDb.Card<Veilpiercer>();
				num2++;
				*span[num2] = ModelDb.Card<BansheesCry>();
				return list;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00014907 File Offset: 0x00012B07
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Necrobinder7Epoch.Cards);
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00014914 File Offset: 0x00012B14
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Necrobinder7Epoch.Cards);
		}
	}
}
