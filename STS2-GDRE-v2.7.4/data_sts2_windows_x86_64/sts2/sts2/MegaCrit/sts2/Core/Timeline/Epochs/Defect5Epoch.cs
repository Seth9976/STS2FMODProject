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
	/// Confrontation
	/// </summary>
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect5Epoch : EpochModel
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00013D08 File Offset: 0x00011F08
		public override string Id
		{
			get
			{
				return "DEFECT5_EPOCH";
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00013D0F File Offset: 0x00011F0F
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish3;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00013D16 File Offset: 0x00011F16
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00013D19 File Offset: 0x00011F19
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00013D20 File Offset: 0x00011F20
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Barrage>();
				num2++;
				*span[num2] = ModelDb.Card<FlakCannon>();
				num2++;
				*span[num2] = ModelDb.Card<Smokestack>();
				return list;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00013D77 File Offset: 0x00011F77
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Defect5Epoch.Cards);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00013D84 File Offset: 0x00011F84
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Defect5Epoch.Cards);
		}
	}
}
