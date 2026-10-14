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
	/// The Architect: Too many Beasts of Imagination begin to populate Preon and things aren’t simple and fun anymore.
	/// </summary>
	// Token: 0x0200004A RID: 74
	[NullableContext(1)]
	[Nullable(0)]
	public class Colorless2Epoch : EpochModel
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000136CF File Offset: 0x000118CF
		public override string Id
		{
			get
			{
				return "COLORLESS2_EPOCH";
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000136D6 File Offset: 0x000118D6
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Prehistoria2;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002AE RID: 686 RVA: 0x000136DD File Offset: 0x000118DD
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000136E0 File Offset: 0x000118E0
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x000136E8 File Offset: 0x000118E8
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<EternalArmor>();
				num2++;
				*span[num2] = ModelDb.Card<Jackpot>();
				num2++;
				*span[num2] = ModelDb.Card<PrepTime>();
				return list;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0001373F File Offset: 0x0001193F
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Colorless2Epoch.Cards);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001374C File Offset: 0x0001194C
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Colorless2Epoch.Cards);
		}
	}
}
