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
	/// The Spire
	/// </summary>
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	public class Event3Epoch : EpochModel
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0001402F File Offset: 0x0001222F
		public override string Id
		{
			get
			{
				return "EVENT3_EPOCH";
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00014036 File Offset: 0x00012236
		public override EpochEra Era
		{
			get
			{
				return EpochEra.Blight1;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0001403D File Offset: 0x0001223D
		public override int EraPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00014040 File Offset: 0x00012240
		public override string StoryId
		{
			get
			{
				return "Magnum_Opus";
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00014048 File Offset: 0x00012248
		public unsafe static List<EventModel> Events
		{
			get
			{
				int num = 1;
				List<EventModel> list = new List<EventModel>(num);
				CollectionsMarshal.SetCount<EventModel>(list, num);
				Span<EventModel> span = CollectionsMarshal.AsSpan<EventModel>(list);
				int num2 = 0;
				*span[num2] = ModelDb.Event<ColorfulPhilosophers>();
				return list;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0001407C File Offset: 0x0001227C
		public override string UnlockText
		{
			get
			{
				LocString locString = new LocString("epochs", this.Id + ".unlockText");
				locString.Add("Event", Event3Epoch.Events[0].Title);
				return locString.GetFormattedText();
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000140C5 File Offset: 0x000122C5
		public override void QueueUnlocks()
		{
			NTimelineScreen.Instance.QueueMiscUnlock(this.UnlockText);
		}
	}
}
