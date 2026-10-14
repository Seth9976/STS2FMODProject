using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Timeline.Epochs
{
	/// <summary>
	/// The Finest
	/// </summary>
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	[Nullable(0)]
	public class Event2Epoch : EpochModel
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00013F7F File Offset: 0x0001217F
		public override string Id
		{
			get
			{
				return "EVENT2_EPOCH";
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00013F86 File Offset: 0x00012186
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Invitation1;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00013F8D File Offset: 0x0001218D
		public override int EraPosition
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00013F90 File Offset: 0x00012190
		public override string StoryId
		{
			get
			{
				return "Reopening";
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00013F98 File Offset: 0x00012198
		public unsafe static List<EventModel> Events
		{
			get
			{
				int num = 1;
				List<EventModel> list = new List<EventModel>(num);
				CollectionsMarshal.SetCount<EventModel>(list, num);
				Span<EventModel> span = CollectionsMarshal.AsSpan<EventModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Event<Reflections>();
				return list;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00013FCC File Offset: 0x000121CC
		public override string UnlockText
		{
			get
			{
				LocString locString = new LocString("epochs", this.Id + ".unlockText");
				locString.Add("Event", Event2Epoch.Events[0].Title);
				return locString.GetFormattedText();
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00014015 File Offset: 0x00012215
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueMiscUnlock(this.UnlockText);
		}
	}
}
