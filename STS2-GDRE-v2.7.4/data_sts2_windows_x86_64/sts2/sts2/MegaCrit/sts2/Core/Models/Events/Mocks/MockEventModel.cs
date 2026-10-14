using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Events.Mocks
{
	// Token: 0x0200086C RID: 2156
	[NullableContext(1)]
	[Nullable(0)]
	public class MockEventModel : EventModel
	{
		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x060069DE RID: 27102 RVA: 0x0027E955 File Offset: 0x0027CB55
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x060069DF RID: 27103 RVA: 0x0027E958 File Offset: 0x0027CB58
		public override bool IsShared
		{
			get
			{
				return this.isShared;
			}
		}

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x060069E0 RID: 27104 RVA: 0x0027E960 File Offset: 0x0027CB60
		public string OptionKey
		{
			get
			{
				return base.Id.Entry + ".pages.INITIAL.options.TEST";
			}
		}

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x060069E1 RID: 27105 RVA: 0x0027E978 File Offset: 0x0027CB78
		private unsafe List<EventOption> DefaultInitialOptions
		{
			get
			{
				int num = 2;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = new EventOption(this, delegate
				{
					this.optionChosen = new int?(0);
					return Task.CompletedTask;
				}, this.OptionKey, Array.Empty<IHoverTip>());
				num2++;
				*span[num2] = new EventOption(this, delegate
				{
					this.optionChosen = new int?(1);
					return Task.CompletedTask;
				}, this.OptionKey, Array.Empty<IHoverTip>());
				return list;
			}
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x0027E9F0 File Offset: 0x0027CBF0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = new List<EventOption>();
			Func<EventModel, IReadOnlyList<EventOption>> func = this.generateInitialOptions;
			foreach (EventOption eventOption in (((func != null) ? func(this) : null) ?? this.DefaultInitialOptions))
			{
				list.Add(new EventOption(eventOption));
			}
			return list;
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x0027EA60 File Offset: 0x0027CC60
		public new void EnterCombatWithoutExitingEvent<[Nullable(0)] T>(IReadOnlyList<Reward> extraRewards, bool shouldResumeAfterCombat) where T : EncounterModel
		{
			base.EnterCombatWithoutExitingEvent<T>(extraRewards, shouldResumeAfterCombat);
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x0027EA6A File Offset: 0x0027CC6A
		public void SetEventState(IEnumerable<EventOption> options)
		{
			this.SetEventState(base.L10NLookup(""), options);
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x0027EA7E File Offset: 0x0027CC7E
		public void SetEventFinished()
		{
			base.SetEventFinished(base.L10NLookup(""));
		}

		// Token: 0x040027FD RID: 10237
		public bool isShared;

		// Token: 0x040027FE RID: 10238
		public int? optionChosen;

		// Token: 0x040027FF RID: 10239
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		public Func<EventModel, IReadOnlyList<EventOption>> generateInitialOptions;
	}
}
