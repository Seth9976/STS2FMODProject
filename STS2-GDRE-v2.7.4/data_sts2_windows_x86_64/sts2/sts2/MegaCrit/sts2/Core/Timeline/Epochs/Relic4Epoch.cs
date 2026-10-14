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
	/// Consequences
	/// </summary>
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	public class Relic4Epoch : EpochModel
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000152B3 File Offset: 0x000134B3
		public override string Id
		{
			get
			{
				return "RELIC4_EPOCH";
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000152BA File Offset: 0x000134BA
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight0;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x000152C1 File Offset: 0x000134C1
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000152C4 File Offset: 0x000134C4
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x000152CC File Offset: 0x000134CC
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<MiniatureCannon>();
				num2++;
				*span[num2] = ModelDb.Relic<TungstenRod>();
				num2++;
				*span[num2] = ModelDb.Relic<WhiteStar>();
				return list;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00015323 File Offset: 0x00013523
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Relic4Epoch.Relics);
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00015330 File Offset: 0x00013530
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Event1Epoch>()),
				EpochModel.Get(EpochModel.GetId<Colorless5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Relic5Epoch>())
			};
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001535F File Offset: 0x0001355F
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Relic4Epoch.Relics);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
