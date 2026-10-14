using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Grand Strategy
	/// </summary>
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent4Epoch : EpochModel
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00014E19 File Offset: 0x00013019
		public override string Id
		{
			get
			{
				return "REGENT4_EPOCH";
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00014E20 File Offset: 0x00013020
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation3;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00014E27 File Offset: 0x00013027
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00014E2A File Offset: 0x0001302A
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00014E34 File Offset: 0x00013034
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<StarPotion>();
				num2++;
				*span[num2] = ModelDb.Potion<CosmicConcoction>();
				num2++;
				*span[num2] = ModelDb.Potion<KingsCourage>();
				return list;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00014E8B File Offset: 0x0001308B
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Regent4Epoch.Potions);
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00014E98 File Offset: 0x00013098
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Regent4Epoch.Potions);
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
