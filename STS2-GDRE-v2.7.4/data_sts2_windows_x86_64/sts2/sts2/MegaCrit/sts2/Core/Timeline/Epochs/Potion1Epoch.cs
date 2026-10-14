using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Open
	/// </summary>
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	public class Potion1Epoch : EpochModel
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00014AC1 File Offset: 0x00012CC1
		public override string Id
		{
			get
			{
				return "POTION1_EPOCH";
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00014AC8 File Offset: 0x00012CC8
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation0;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00014ACF File Offset: 0x00012CCF
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00014AD2 File Offset: 0x00012CD2
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00014ADC File Offset: 0x00012CDC
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<BeetleJuice>();
				num2++;
				*span[num2] = ModelDb.Potion<MazalethsGift>();
				num2++;
				*span[num2] = ModelDb.Potion<DropletOfPrecognition>();
				return list;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00014B33 File Offset: 0x00012D33
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Potion1Epoch.Potions);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00014B40 File Offset: 0x00012D40
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Potion1Epoch.Potions);
		}
	}
}
