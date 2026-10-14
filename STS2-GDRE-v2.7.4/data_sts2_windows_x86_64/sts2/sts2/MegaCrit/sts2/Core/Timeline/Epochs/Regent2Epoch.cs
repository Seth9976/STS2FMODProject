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
	/// I Arrive
	/// </summary>
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent2Epoch : EpochModel
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00014CEB File Offset: 0x00012EEB
		public override string Id
		{
			get
			{
				return "REGENT2_EPOCH";
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00014CF2 File Offset: 0x00012EF2
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation1;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00014CF9 File Offset: 0x00012EF9
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00014CFC File Offset: 0x00012EFC
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00014D04 File Offset: 0x00012F04
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<SpoilsOfBattle>();
				num2++;
				*span[num2] = ModelDb.Card<SwordSage>();
				num2++;
				*span[num2] = ModelDb.Card<Furnace>();
				return list;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00014D5B File Offset: 0x00012F5B
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Regent2Epoch.Cards);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014D68 File Offset: 0x00012F68
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Regent2Epoch.Cards);
		}
	}
}
