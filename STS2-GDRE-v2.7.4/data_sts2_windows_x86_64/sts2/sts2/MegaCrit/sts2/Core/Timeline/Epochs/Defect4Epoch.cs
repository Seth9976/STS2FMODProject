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
	/// Resolving Bugs
	/// </summary>
	// Token: 0x02000054 RID: 84
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect4Epoch : EpochModel
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00013C31 File Offset: 0x00011E31
		public override string Id
		{
			get
			{
				return "DEFECT4_EPOCH";
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00013C38 File Offset: 0x00011E38
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish2;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00013C3F File Offset: 0x00011E3F
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00013C42 File Offset: 0x00011E42
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00013C4C File Offset: 0x00011E4C
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<FocusPotion>();
				num2++;
				*span[num2] = ModelDb.Potion<EssenceOfDarkness>();
				num2++;
				*span[num2] = ModelDb.Potion<PotionOfCapacity>();
				return list;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00013CA3 File Offset: 0x00011EA3
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Defect4Epoch.Potions);
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00013CB0 File Offset: 0x00011EB0
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Defect4Epoch.Potions);
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
