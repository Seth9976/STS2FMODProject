using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Preon: A void of nothingness.
	/// But then, the world comes into existence!
	/// </summary>
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	public class Colorless1Epoch : EpochModel
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000135FD File Offset: 0x000117FD
		public override string Id
		{
			get
			{
				return "COLORLESS1_EPOCH";
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00013604 File Offset: 0x00011804
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Prehistoria0;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0001360B File Offset: 0x0001180B
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0001360E File Offset: 0x0001180E
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00013618 File Offset: 0x00011818
		public unsafe static List<CardModel> Cards
		{
			get
			{
				int num = 3;
				List<CardModel> list = new List<CardModel>(num);
				CollectionsMarshal.SetCount<CardModel>(list, num);
				Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Card<Automation>();
				num2++;
				*span[num2] = ModelDb.Card<Entropy>();
				num2++;
				*span[num2] = ModelDb.Card<Catastrophe>();
				return list;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0001366F File Offset: 0x0001186F
		public override string UnlockText
		{
			get
			{
				return base.CreateCardUnlockText(Colorless1Epoch.Cards);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001367C File Offset: 0x0001187C
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Potion1Epoch>()),
				EpochModel.Get(EpochModel.GetId<Relic1Epoch>()),
				EpochModel.Get(EpochModel.GetId<UnderdocksEpoch>())
			};
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000136AB File Offset: 0x000118AB
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCardUnlock(Colorless1Epoch.Cards);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
