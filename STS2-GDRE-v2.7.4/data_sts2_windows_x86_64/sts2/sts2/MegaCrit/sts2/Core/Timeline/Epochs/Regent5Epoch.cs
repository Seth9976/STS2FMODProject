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
	/// Friends
	/// </summary>
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent5Epoch : EpochModel
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00014EF0 File Offset: 0x000130F0
		public override string Id
		{
			get
			{
				return "REGENT5_EPOCH";
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00014EF7 File Offset: 0x000130F7
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish3;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00014EFE File Offset: 0x000130FE
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00014F01 File Offset: 0x00013101
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00014F08 File Offset: 0x00013108
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Begone>();
				num2++;
				*span[num2] = ModelDb.Card<Arsenal>();
				num2++;
				*span[num2] = ModelDb.Card<Supermassive>();
				return list;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00014F5F File Offset: 0x0001315F
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Regent5Epoch.Cards);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00014F6C File Offset: 0x0001316C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Regent5Epoch.Cards);
		}
	}
}
