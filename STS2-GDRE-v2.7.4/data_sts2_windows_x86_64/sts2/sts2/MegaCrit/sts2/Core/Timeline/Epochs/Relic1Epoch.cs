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
	/// Scribbles: On this quiet planet, Beasts of Imagination begin to dot the world.
	/// </summary>
	// Token: 0x02000073 RID: 115
	[NullableContext(1)]
	[Nullable(0)]
	public class Relic1Epoch : EpochModel
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x000150B5 File Offset: 0x000132B5
		public override string Id
		{
			get
			{
				return "RELIC1_EPOCH";
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x000150BC File Offset: 0x000132BC
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Prehistoria1;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000150C3 File Offset: 0x000132C3
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x000150C6 File Offset: 0x000132C6
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000150D0 File Offset: 0x000132D0
		public unsafe static List<RelicModel> Relics
		{
			get
			{
				int num = 3;
				List<RelicModel> list = new List<RelicModel>(num);
				CollectionsMarshal.SetCount<RelicModel>(list, num);
				Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Relic<UnsettlingLamp>();
				num2++;
				*span[num2] = ModelDb.Relic<IntimidatingHelmet>();
				num2++;
				*span[num2] = ModelDb.Relic<ReptileTrinket>();
				return list;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00015127 File Offset: 0x00013327
		public override string UnlockText
		{
			get
			{
				return base.CreateRelicUnlockText(Relic1Epoch.Relics);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015134 File Offset: 0x00013334
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueRelicUnlock(Relic1Epoch.Relics);
		}
	}
}
