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
	/// The Pact
	/// </summary>
	// Token: 0x0200005D RID: 93
	[NullableContext(1)]
	[Nullable(0)]
	public class Ironclad4Epoch : EpochModel
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00014209 File Offset: 0x00012409
		public override string Id
		{
			get
			{
				return "IRONCLAD4_EPOCH";
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00014210 File Offset: 0x00012410
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Seeds2;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00014213 File Offset: 0x00012413
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00014216 File Offset: 0x00012416
		public override string StoryId
		{
			get
			{
				return "Ironclad";
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00014220 File Offset: 0x00012420
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<BloodPotion>();
				num2++;
				*span[num2] = ModelDb.Potion<SoldiersStew>();
				num2++;
				*span[num2] = ModelDb.Potion<Ashwater>();
				return list;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00014277 File Offset: 0x00012477
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Ironclad4Epoch.Potions);
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00014284 File Offset: 0x00012484
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Ironclad4Epoch.Potions);
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
