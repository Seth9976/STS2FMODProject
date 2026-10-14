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
	/// Spireborn
	/// </summary>
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder4Epoch : EpochModel
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00014691 File Offset: 0x00012891
		public override string Id
		{
			get
			{
				return "NECROBINDER4_EPOCH";
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00014698 File Offset: 0x00012898
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation2;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0001469F File Offset: 0x0001289F
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000377 RID: 887 RVA: 0x000146A2 File Offset: 0x000128A2
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000146AC File Offset: 0x000128AC
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<PotionOfDoom>();
				num2++;
				*span[num2] = ModelDb.Potion<PotOfGhouls>();
				num2++;
				*span[num2] = ModelDb.Potion<BoneBrew>();
				return list;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00014703 File Offset: 0x00012903
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Necrobinder4Epoch.Potions);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014710 File Offset: 0x00012910
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Necrobinder4Epoch.Potions);
			LocString locString = new LocString("epochs", this.Id + ".unlock");
			NTimelineScreen.Instance.QueueMiscUnlock(locString.GetFormattedText() ?? "");
		}
	}
}
