using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// Life and Death
	/// </summary>
	// Token: 0x02000061 RID: 97
	[NullableContext(1)]
	[Nullable(0)]
	public class Necrobinder1Epoch : EpochModel
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600035D RID: 861 RVA: 0x000144A1 File Offset: 0x000126A1
		public override string Id
		{
			get
			{
				return "NECROBINDER1_EPOCH";
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600035E RID: 862 RVA: 0x000144A8 File Offset: 0x000126A8
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation5;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000144AF File Offset: 0x000126AF
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000144B2 File Offset: 0x000126B2
		public override string StoryId
		{
			get
			{
				return "Necrobinder";
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000144BC File Offset: 0x000126BC
		public override EpochModel[] GetTimelineExpansion()
		{
			return new EpochModel[]
			{
				EpochModel.Get(EpochModel.GetId<Necrobinder2Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder3Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder4Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder5Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder6Epoch>()),
				EpochModel.Get(EpochModel.GetId<Necrobinder7Epoch>()),
				EpochModel.Get(EpochModel.GetId<Defect1Epoch>())
			};
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001452A File Offset: 0x0001272A
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueCharacterUnlock<Necrobinder>(this);
			SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Necrobinder>().Id;
			EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
		}
	}
}
