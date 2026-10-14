using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Hello! Hello!
	/// </summary>
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect3Epoch : EpochModel
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00013B99 File Offset: 0x00011D99
		public override string Id
		{
			get
			{
				return "DEFECT3_EPOCH";
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00013BA0 File Offset: 0x00011DA0
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish1;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00013BA7 File Offset: 0x00011DA7
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00013BAA File Offset: 0x00011DAA
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<DataDisk>();
				num2++;
				*span[num2] = ModelDb.Relic<SymbioticVirus>();
				num2++;
				*span[num2] = ModelDb.Relic<Metronome>();
				return list;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00013C0B File Offset: 0x00011E0B
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Defect3Epoch.Relics);
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00013C18 File Offset: 0x00011E18
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Defect3Epoch.Relics);
		}
	}
}
