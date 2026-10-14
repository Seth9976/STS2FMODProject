using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085D RID: 2141
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheLanternKey : EventModel
	{
		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x06006950 RID: 26960 RVA: 0x0027BE95 File Offset: 0x0027A095
		public override EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Combat;
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x06006951 RID: 26961 RVA: 0x0027BE98 File Offset: 0x0027A098
		public override EncounterModel CanonicalEncounter
		{
			get
			{
				return ModelDb.Encounter<MysteriousKnightEventEncounter>();
			}
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x06006952 RID: 26962 RVA: 0x0027BE9F File Offset: 0x0027A09F
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x06006953 RID: 26963 RVA: 0x0027BEA2 File Offset: 0x0027A0A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(100));
			}
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x0027BEB0 File Offset: 0x0027A0B0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.ReturnTheKey), "THE_LANTERN_KEY.pages.INITIAL.options.RETURN_THE_KEY", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.KeepTheKey), "THE_LANTERN_KEY.pages.INITIAL.options.KEEP_THE_KEY", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x0027BF08 File Offset: 0x0027A108
		private async Task ReturnTheKey()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			base.SetEventFinished(base.L10NLookup("THE_LANTERN_KEY.pages.DONE.options.RETURN_THE_KEY.description"));
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x0027BF4B File Offset: 0x0027A14B
		private Task KeepTheKey()
		{
			this.SetEventState(base.L10NLookup("THE_LANTERN_KEY.pages.KEEP_THE_KEY.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.Fight), "THE_LANTERN_KEY.pages.KEEP_THE_KEY.options.FIGHT", Array.Empty<IHoverTip>())));
			return Task.CompletedTask;
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x0027BF84 File Offset: 0x0027A184
		private unsafe Task Fight()
		{
			int num = 1;
			List<Reward> list = new List<Reward>(num);
			CollectionsMarshal.SetCount<Reward>(list, num);
			Span<Reward> span = CollectionsMarshal.AsSpan<Reward>(list);
			int num2 = 0;
			*span[num2] = new SpecialCardReward(base.Owner.RunState.CreateCard<LanternKey>(base.Owner), base.Owner);
			List<Reward> list2 = list;
			base.EnterCombatWithoutExitingEvent<MysteriousKnightEventEncounter>(list2, false);
			return Task.CompletedTask;
		}
	}
}
