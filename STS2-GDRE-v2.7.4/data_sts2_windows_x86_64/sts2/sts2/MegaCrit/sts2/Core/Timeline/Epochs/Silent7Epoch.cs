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
	/// Soup
	/// </summary>
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent7Epoch : EpochModel
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00015839 File Offset: 0x00013A39
		public override string Id
		{
			get
			{
				return "SILENT7_EPOCH";
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00015840 File Offset: 0x00013A40
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation5;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00015847 File Offset: 0x00013A47
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001584A File Offset: 0x00013A4A
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00015854 File Offset: 0x00013A54
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<HiddenDaggers>();
				num2++;
				*span[num2] = ModelDb.Card<BladeOfInk>();
				num2++;
				*span[num2] = ModelDb.Card<PhantomBlades>();
				return list;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x000158AB File Offset: 0x00013AAB
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Silent7Epoch.Cards);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000158B8 File Offset: 0x00013AB8
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Silent7Epoch.Cards);
		}
	}
}
