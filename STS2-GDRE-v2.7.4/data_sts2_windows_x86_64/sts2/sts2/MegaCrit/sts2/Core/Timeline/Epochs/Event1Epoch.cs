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
	/// Cultists
	/// </summary>
	// Token: 0x02000058 RID: 88
	[NullableContext(1)]
	[Nullable(0)]
	public class Event1Epoch : EpochModel
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00013ECD File Offset: 0x000120CD
		public override string Id
		{
			get
			{
				return "EVENT1_EPOCH";
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00013ED4 File Offset: 0x000120D4
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Flourish1;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00013EDB File Offset: 0x000120DB
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00013EDE File Offset: 0x000120DE
		public override string StoryId
		{
			get
			{
				return "Tales_From_The_Spire";
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00013EE8 File Offset: 0x000120E8
		public unsafe static List<EventModel> Events
		{
			get
			{
				int num = 1;
				List<EventModel> list = new List<EventModel>(num);
				CollectionsMarshal.SetCount<EventModel>(list, num);
				Span<EventModel> span = CollectionsMarshal.AsSpan<EventModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Event<TrashHeap>();
				return list;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00013F1C File Offset: 0x0001211C
		public override string UnlockText
		{
			get
			{
				LocString locString = new LocString("epochs", this.Id + ".unlockText");
				locString.Add("Event", Event1Epoch.Events[0].Title);
				return locString.GetFormattedText();
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00013F65 File Offset: 0x00012165
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueMiscUnlock(this.UnlockText);
		}
	}
}
