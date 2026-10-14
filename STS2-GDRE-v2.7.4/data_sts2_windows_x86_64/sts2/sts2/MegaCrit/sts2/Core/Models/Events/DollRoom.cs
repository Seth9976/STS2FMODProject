using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000836 RID: 2102
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DollRoom : EventModel
	{
		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x060067C3 RID: 26563 RVA: 0x002741C2 File Offset: 0x002723C2
		// (set) Token: 0x060067C4 RID: 26564 RVA: 0x002741CA File Offset: 0x002723CA
		private int? AmbienceHandle
		{
			get
			{
				return this._ambienceHandle;
			}
			set
			{
				base.AssertMutable();
				this._ambienceHandle = value;
			}
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x002741D9 File Offset: 0x002723D9
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex == 1;
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x060067C6 RID: 26566 RVA: 0x002741E4 File Offset: 0x002723E4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar("TakeTimeHpLoss", 5m, ValueProp.Unblockable | ValueProp.Unpowered),
					new DamageVar("ExamineHpLoss", 15m, ValueProp.Unblockable | ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x0027421C File Offset: 0x0027241C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.ChooseRandom), "DOLL_ROOM.pages.INITIAL.options.RANDOM", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.TakeSomeTime), "DOLL_ROOM.pages.INITIAL.options.TAKE_SOME_TIME", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars["TakeTimeHpLoss"].BaseValue),
				new EventOption(this, new Func<Task>(this.Examine), "DOLL_ROOM.pages.INITIAL.options.EXAMINE", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars["ExamineHpLoss"].BaseValue)
			});
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x002742C5 File Offset: 0x002724C5
		protected override Task BeforeEventStarted(bool isPreFinished)
		{
			if (LocalContext.IsMe(base.Owner) && TestMode.IsOff)
			{
				this.AmbienceHandle = new int?(NDebugAudioManager.Instance.Play("doll_room_amb.mp3", 1f, PitchVariance.None));
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// The top option. Get a random one without taking damage.
		/// </summary>
		// Token: 0x060067C9 RID: 26569 RVA: 0x00274300 File Offset: 0x00272500
		private async Task ChooseRandom()
		{
			await this.ChooseDollAndShowDescription(base.Rng.NextItem<DollRoom.DollChoice>(DollRoom._dolls));
		}

		/// <summary>
		/// Choose 1 of 2
		/// </summary>
		// Token: 0x060067CA RID: 26570 RVA: 0x00274344 File Offset: 0x00272544
		private async Task TakeSomeTime()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, (DamageVar)base.DynamicVars["TakeTimeHpLoss"], null, null, null);
			IEnumerable<DollRoom.DollChoice> enumerable = DollRoom._dolls.ToList<DollRoom.DollChoice>().StableShuffle<DollRoom.DollChoice>(base.Rng).Take<DollRoom.DollChoice>(2);
			List<EventOption> list = new List<EventOption>();
			foreach (DollRoom.DollChoice dollChoice in enumerable)
			{
				list.Add(this.OptionFromChoice(dollChoice));
			}
			this.SetEventState(base.L10NLookup("DOLL_ROOM.pages.TAKE_SOME_TIME.description"), list);
		}

		/// <summary>
		/// Choose 1 of 3
		/// </summary>
		// Token: 0x060067CB RID: 26571 RVA: 0x00274388 File Offset: 0x00272588
		private async Task Examine()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, (DamageVar)base.DynamicVars["ExamineHpLoss"], null, null, null);
			IEnumerable<DollRoom.DollChoice> enumerable = DollRoom._dolls.ToList<DollRoom.DollChoice>().StableShuffle<DollRoom.DollChoice>(base.Rng);
			List<EventOption> list = new List<EventOption>();
			foreach (DollRoom.DollChoice dollChoice in enumerable)
			{
				list.Add(this.OptionFromChoice(dollChoice));
			}
			this.SetEventState(base.L10NLookup("DOLL_ROOM.pages.EXAMINE.description"), list);
		}

		// Token: 0x060067CC RID: 26572 RVA: 0x002743CC File Offset: 0x002725CC
		private EventOption OptionFromChoice(DollRoom.DollChoice choice)
		{
			DollRoom.<>c__DisplayClass16_0 CS$<>8__locals1 = new DollRoom.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.choice = choice;
			LocString title = CS$<>8__locals1.choice.relic.Title;
			LocString locString = base.L10NLookup("DOLL_ROOM.pages.TAKE.options.TAKE.description");
			locString.Add("RelicName", CS$<>8__locals1.choice.relic.Title);
			return new EventOption(this, new Func<Task>(CS$<>8__locals1.<OptionFromChoice>g__Func|0), title, locString, CS$<>8__locals1.choice.relic.Title.GetRawText(), HoverTipFactory.FromRelic(CS$<>8__locals1.choice.relic)).WithOverridenHistoryName(CS$<>8__locals1.choice.relic.Title);
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x00274474 File Offset: 0x00272674
		private async Task ChooseDollAndShowDescription(DollRoom.DollChoice choice)
		{
			this.StopAudio();
			await RelicCmd.Obtain(choice.relic.ToMutable(), base.Owner, -1);
			base.SetEventFinished(base.L10NLookup(choice.descriptionKey));
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x002744BF File Offset: 0x002726BF
		protected override void OnEventFinished()
		{
			this.StopAudio();
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x002744C8 File Offset: 0x002726C8
		private void StopAudio()
		{
			if (this.AmbienceHandle != null)
			{
				NDebugAudioManager.Instance.Stop(this.AmbienceHandle.Value, 0.5f);
				this.AmbienceHandle = null;
			}
		}

		// Token: 0x04002771 RID: 10097
		private const string _takeTimeHpLossKey = "TakeTimeHpLoss";

		// Token: 0x04002772 RID: 10098
		private const string _examineHpLossKey = "ExamineHpLoss";

		// Token: 0x04002773 RID: 10099
		private int? _ambienceHandle;

		// Token: 0x04002774 RID: 10100
		private static readonly DollRoom.DollChoice[] _dolls = new DollRoom.DollChoice[]
		{
			new DollRoom.DollChoice
			{
				relic = ModelDb.Relic<DaughterOfTheWind>(),
				descriptionKey = "DOLL_ROOM.pages.DAUGHTER_OF_WIND.description"
			},
			new DollRoom.DollChoice
			{
				relic = ModelDb.Relic<MrStruggles>(),
				descriptionKey = "DOLL_ROOM.pages.MR_STRUGGLES.description"
			},
			new DollRoom.DollChoice
			{
				relic = ModelDb.Relic<BingBong>(),
				descriptionKey = "DOLL_ROOM.pages.FABLE.description"
			}
		};

		// Token: 0x02001F01 RID: 7937
		[NullableContext(0)]
		private struct DollChoice : IComparable<DollRoom.DollChoice>
		{
			// Token: 0x0600B6CC RID: 46796 RVA: 0x003C45B2 File Offset: 0x003C27B2
			public int CompareTo(DollRoom.DollChoice other)
			{
				return this.relic.CompareTo(other.relic);
			}

			// Token: 0x04007E93 RID: 32403
			[Nullable(1)]
			public RelicModel relic;

			// Token: 0x04007E94 RID: 32404
			[Nullable(1)]
			public string descriptionKey;
		}
	}
}
