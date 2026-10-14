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
	/// Insects
	/// </summary>
	// Token: 0x02000077 RID: 119
	[NullableContext(1)]
	[Nullable(0)]
	public class Relic5Epoch : EpochModel
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00015383 File Offset: 0x00013583
		public override string Id
		{
			get
			{
				return "RELIC5_EPOCH";
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001538A File Offset: 0x0001358A
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish1;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00015391 File Offset: 0x00013591
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00015394 File Offset: 0x00013594
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001539C File Offset: 0x0001359C
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<TinyMailbox>();
				num2++;
				*span[num2] = ModelDb.Relic<JossPaper>();
				num2++;
				*span[num2] = ModelDb.Relic<BeatingRemnant>();
				return list;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000153F3 File Offset: 0x000135F3
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Relic5Epoch.Relics);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00015400 File Offset: 0x00013600
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Event2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Event3Epoch>())
			};
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00015422 File Offset: 0x00013622
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Relic5Epoch.Relics);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
