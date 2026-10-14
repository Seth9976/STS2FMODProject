using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Anomalous Planet
	/// </summary>
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	public class Regent1Epoch : EpochModel
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00014C2B File Offset: 0x00012E2B
		public override string Id
		{
			get
			{
				return "REGENT1_EPOCH";
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00014C32 File Offset: 0x00012E32
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish2;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00014C39 File Offset: 0x00012E39
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00014C3C File Offset: 0x00012E3C
		public override string StoryId
		{
			get
			{
				return "Regent";
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00014C44 File Offset: 0x00012E44
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Regent2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Regent3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Regent4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Regent5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Regent6Epoch>()),
				EpochModel.Get(EpochModel.GetId<Regent7Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder1Epoch>())
			};
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00014CB2 File Offset: 0x00012EB2
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCharacterUnlock<Regent>(this);
			SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Regent>().Id;
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
