using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Booting Up
	/// </summary>
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	public class Defect1Epoch : EpochModel
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00013A4D File Offset: 0x00011C4D
		public override string Id
		{
			get
			{
				return "DEFECT1_EPOCH";
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00013A54 File Offset: 0x00011C54
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation1;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00013A5B File Offset: 0x00011C5B
		public override int EraPosition
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00013A5E File Offset: 0x00011C5E
		public override string StoryId
		{
			get
			{
				return "Defect";
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00013A68 File Offset: 0x00011C68
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Defect2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect6Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect7Epoch>())
			};
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00013AC9 File Offset: 0x00011CC9
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCharacterUnlock<Defect>(this);
			SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Defect>().Id;
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
