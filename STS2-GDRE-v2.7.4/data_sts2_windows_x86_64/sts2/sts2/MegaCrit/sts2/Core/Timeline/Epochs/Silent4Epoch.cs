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
	/// Trophies
	/// </summary>
	// Token: 0x0200007B RID: 123
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent4Epoch : EpochModel
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00015635 File Offset: 0x00013835
		public override string Id
		{
			get
			{
				return "SILENT4_EPOCH";
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0001563C File Offset: 0x0001383C
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight1;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00015643 File Offset: 0x00013843
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00015646 File Offset: 0x00013846
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00015650 File Offset: 0x00013850
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<PoisonPotion>();
				num2++;
				*span[num2] = ModelDb.Potion<GhostInAJar>();
				num2++;
				*span[num2] = ModelDb.Potion<CunningPotion>();
				return list;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x000156A7 File Offset: 0x000138A7
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Silent4Epoch.Potions);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000156B4 File Offset: 0x000138B4
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Silent4Epoch.Potions);
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
