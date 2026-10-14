using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// The Calm
	/// </summary>
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	public class Potion2Epoch : EpochModel
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00014B59 File Offset: 0x00012D59
		public override string Id
		{
			get
			{
				return "POTION2_EPOCH";
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00014B60 File Offset: 0x00012D60
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish0;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00014B67 File Offset: 0x00012D67
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00014B6A File Offset: 0x00012D6A
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00014B74 File Offset: 0x00012D74
		public unsafe static List<PotionModel> Potions
		{
			get
			{
				int num = 3;
				List<PotionModel> list = new List<PotionModel>(num);
				CollectionsMarshal.SetCount<PotionModel>(list, num);
				Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Potion<PowderedDemise>();
				num2++;
				*span[num2] = ModelDb.Potion<ShipInABottle>();
				num2++;
				*span[num2] = ModelDb.Potion<TouchOfInsanity>();
				return list;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00014BCB File Offset: 0x00012DCB
		public override string UnlockText
		{
			get
			{
				return base.CreatePotionUnlockText(Potion2Epoch.Potions);
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014BD8 File Offset: 0x00012DD8
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Act2BEpoch>()),
				EpochModel.Get(EpochModel.GetId<Colorless3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Relic3Epoch>())
			};
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00014C07 File Offset: 0x00012E07
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueuePotionUnlock(Potion2Epoch.Potions);
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
