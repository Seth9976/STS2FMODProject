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
	/// Buy Something!
	/// </summary>
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	public class Colorless5Epoch : EpochModel
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0001388D File Offset: 0x00011A8D
		public override string Id
		{
			get
			{
				return "COLORLESS5_EPOCH";
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00013894 File Offset: 0x00011A94
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight1;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0001389B File Offset: 0x00011A9B
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0001389E File Offset: 0x00011A9E
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000138A8 File Offset: 0x00011AA8
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Splash>();
				num2++;
				*span[num2] = ModelDb.Card<Anointed>();
				num2++;
				*span[num2] = ModelDb.Card<Calamity>();
				return list;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x000138FF File Offset: 0x00011AFF
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Colorless5Epoch.Cards);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001390C File Offset: 0x00011B0C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Colorless5Epoch.Cards);
		}
	}
}
