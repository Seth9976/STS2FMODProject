using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Dance of Death
	/// </summary>
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public class Silent1Epoch : EpochModel
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00015446 File Offset: 0x00013646
		public override string Id
		{
			get
			{
				return "SILENT1_EPOCH";
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001544D File Offset: 0x0001364D
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation0;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00015454 File Offset: 0x00013654
		public override int EraPosition
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00015457 File Offset: 0x00013657
		public override string StoryId
		{
			get
			{
				return "Silent";
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00015460 File Offset: 0x00013660
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Regent1Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent6Epoch>()),
				EpochModel.Get(EpochModel.GetId<Silent7Epoch>())
			};
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000154CE File Offset: 0x000136CE
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCharacterUnlock<Silent>(this);
			SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Silent>().Id;
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
