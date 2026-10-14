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
	/// Civilization
	/// </summary>
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	public class Relic3Epoch : EpochModel
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000151E1 File Offset: 0x000133E1
		public override string Id
		{
			get
			{
				return "RELIC3_EPOCH";
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x000151E8 File Offset: 0x000133E8
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish1;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000151EF File Offset: 0x000133EF
		public override int EraPosition
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000151F2 File Offset: 0x000133F2
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x000151FC File Offset: 0x000133FC
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<VexingPuzzlebox>();
				num2++;
				*span[num2] = ModelDb.Relic<RippleBasin>();
				num2++;
				*span[num2] = ModelDb.Relic<FestivePopper>();
				return list;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00015253 File Offset: 0x00013453
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Relic3Epoch.Relics);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00015260 File Offset: 0x00013460
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Act3BEpoch>()),
				EpochModel.Get(EpochModel.GetId<Colorless4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Relic4Epoch>())
			};
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001528F File Offset: 0x0001348F
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Relic3Epoch.Relics);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
