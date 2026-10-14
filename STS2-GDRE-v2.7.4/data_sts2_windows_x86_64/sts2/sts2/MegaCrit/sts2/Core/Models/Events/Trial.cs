using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000862 RID: 2146
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Trial : EventModel
	{
		// Token: 0x0600697C RID: 27004 RVA: 0x0027CA90 File Offset: 0x0027AC90
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Accept), "TRIAL.pages.INITIAL.options.ACCEPT", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Reject), "TRIAL.pages.INITIAL.options.REJECT", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600697D RID: 27005 RVA: 0x0027CAE6 File Offset: 0x0027ACE6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("EntrantNumber", -1m));
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x0600697E RID: 27006 RVA: 0x0027CAFC File Offset: 0x0027ACFC
		private static string TrialStartedPath
		{
			get
			{
				return ImageHelper.GetImagePath("events/trial_started.png");
			}
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x0027CB08 File Offset: 0x0027AD08
		public override IEnumerable<string> GetAssetPaths(IRunState runState)
		{
			List<string> list = new List<string>();
			list.AddRange(base.GetAssetPaths(runState));
			list.Add(Trial.TrialStartedPath);
			list.Add(Trial._trialMerchantVfx);
			list.Add(Trial._trialNobleVfx);
			list.Add(Trial._trialNondescriptVfx);
			return new <>z__ReadOnlyList<string>(list);
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x0027CB58 File Offset: 0x0027AD58
		private Task Accept()
		{
			if (LocalContext.IsMe(base.Owner))
			{
				NEventRoom.Instance.Layout.RemoveNodesOnPortrait();
			}
			string text;
			string text2;
			EventOption[] array;
			switch (base.Rng.NextInt(3))
			{
			case 0:
				text = Trial._trialMerchantVfx;
				text2 = "TRIAL.pages.MERCHANT.description";
				array = new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.MerchantGuilty), "TRIAL.pages.MERCHANT.options.GUILTY", HoverTipFactory.FromCardWithCardHoverTips<Regret>(false)),
					new EventOption(this, new Func<Task>(this.MerchantInnocent), "TRIAL.pages.MERCHANT.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Shame>(false))
				};
				break;
			case 1:
				text = Trial._trialNobleVfx;
				text2 = "TRIAL.pages.NOBLE.description";
				array = new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.NobleGuilty), "TRIAL.pages.NOBLE.options.GUILTY", Array.Empty<IHoverTip>()),
					new EventOption(this, new Func<Task>(this.NobleInnocent), "TRIAL.pages.NOBLE.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Regret>(false))
				};
				break;
			case 2:
				text = Trial._trialNondescriptVfx;
				text2 = "TRIAL.pages.NONDESCRIPT.description";
				array = new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.NondescriptGuilty), "TRIAL.pages.NONDESCRIPT.options.GUILTY", HoverTipFactory.FromCardWithCardHoverTips<Doubt>(false)),
					new EventOption(this, new Func<Task>(this.NondescriptInnocent), "TRIAL.pages.NONDESCRIPT.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Doubt>(false).Concat<IHoverTip>(new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()))))
				};
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.AddVfxAnchoredToPortrait(text);
			if (LocalContext.IsMe(base.Owner))
			{
				NEventRoom.Instance.SetPortrait(PreloadManager.Cache.GetTexture2D(Trial.TrialStartedPath));
			}
			LocString locString = base.L10NLookup("TRIAL.trialFormat");
			locString.Add(new StringVar("TrialStory", base.L10NLookup(text2).GetRawText()));
			this.SetEventState(locString, array);
			return Task.CompletedTask;
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x0027CD28 File Offset: 0x0027AF28
		private Task Reject()
		{
			EventOption[] array = new EventOption[2];
			array[0] = new EventOption(this, new Func<Task>(this.Accept), "TRIAL.pages.REJECT.options.ACCEPT", Array.Empty<IHoverTip>());
			array[1] = new EventOption(this, new Func<Task>(this.DoubleDown), "TRIAL.pages.REJECT.options.DOUBLE_DOWN", false, true, Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Player _) => true);
			EventOption[] array2 = array;
			LocString locString = base.L10NLookup("TRIAL.pages.REJECT.description");
			this.SetEventState(locString, array2);
			return Task.CompletedTask;
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x0027CDB9 File Offset: 0x0027AFB9
		private Task DoubleDown()
		{
			NModalContainer.Instance.Add(NAbandonRunConfirmPopup.Create(null), true);
			return Task.CompletedTask;
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x0027CDD4 File Offset: 0x0027AFD4
		private void AddVfxAnchoredToPortrait(string portraitPath)
		{
			if (LocalContext.IsMe(base.Owner))
			{
				Node2D node2D = PreloadManager.Cache.GetScene(portraitPath).Instantiate<Node2D>(0L);
				node2D.Position = new Vector2(292f, 68f);
				NEventRoom.Instance.Layout.AddVfxAnchoredToPortrait(node2D);
			}
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x0027CE28 File Offset: 0x0027B028
		private async Task MerchantGuilty()
		{
			await CardPileCmd.AddCurseToDeck<Regret>(base.Owner);
			for (int i = 0; i < 2; i++)
			{
				await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable(), base.Owner, -1);
			}
			this.SetTrialFinished("TRIAL.pages.MERCHANT_GUILTY.description");
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x0027CE6C File Offset: 0x0027B06C
		private async Task MerchantInnocent()
		{
			await CardPileCmd.AddCurseToDeck<Shame>(base.Owner);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 2);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForUpgrade(base.Owner, cardSelectorPrefs);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			this.SetTrialFinished("TRIAL.pages.MERCHANT_INNOCENT.description");
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x0027CEB0 File Offset: 0x0027B0B0
		private async Task NobleGuilty()
		{
			await CreatureCmd.Heal(base.Owner.Creature, 10m, true);
			this.SetTrialFinished("TRIAL.pages.NOBLE_GUILTY.description");
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x0027CEF4 File Offset: 0x0027B0F4
		private async Task NobleInnocent()
		{
			await CardPileCmd.AddCurseToDeck<Regret>(base.Owner);
			await PlayerCmd.GainGold(300m, base.Owner, false);
			this.SetTrialFinished("TRIAL.pages.NOBLE_INNOCENT.description");
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x0027CF38 File Offset: 0x0027B138
		private async Task NondescriptGuilty()
		{
			await CardPileCmd.AddCurseToDeck<Doubt>(base.Owner);
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < 2; i++)
			{
				list.Add(new CardReward(CardCreationOptions.ForNonCombatWithDefaultOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), null), 3, base.Owner, null));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
			this.SetTrialFinished("TRIAL.pages.NONDESCRIPT_GUILTY.description");
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x0027CF7C File Offset: 0x0027B17C
		private async Task NondescriptInnocent()
		{
			await CardPileCmd.AddCurseToDeck<Doubt>(base.Owner);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 2);
			List<CardModel> list = (await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, null)).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			this.SetTrialFinished("TRIAL.pages.NONDESCRIPT_INNOCENT.description");
		}

		/// <summary>
		/// Helper function because we create this event's descriptions dynamically
		/// </summary>
		/// <param name="trialResultLoc"></param>
		// Token: 0x0600698A RID: 27018 RVA: 0x0027CFC0 File Offset: 0x0027B1C0
		private void SetTrialFinished(string trialResultLoc)
		{
			LocString locString = base.L10NLookup("TRIAL.trialResult");
			locString.Add(new StringVar("TrialResult", base.L10NLookup(trialResultLoc).GetRawText()));
			base.SetEventFinished(locString);
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x0027CFFC File Offset: 0x0027B1FC
		public override void CalculateVars()
		{
			if (base.DynamicVars["EntrantNumber"].BaseValue == -1m)
			{
				base.DynamicVars["EntrantNumber"].BaseValue = Rng.Chaotic.NextInt(101, 999);
			}
		}

		// Token: 0x040027DF RID: 10207
		private static readonly string _trialMerchantVfx = SceneHelper.GetScenePath("vfx/events/trial_merchant_vfx");

		// Token: 0x040027E0 RID: 10208
		private static readonly string _trialNobleVfx = SceneHelper.GetScenePath("vfx/events/trial_noble_vfx");

		// Token: 0x040027E1 RID: 10209
		private static readonly string _trialNondescriptVfx = SceneHelper.GetScenePath("vfx/events/trial_nondescript_vfx");

		// Token: 0x040027E2 RID: 10210
		private const string _entrantNumberKey = "EntrantNumber";

		// Token: 0x040027E3 RID: 10211
		private const string _trialResultKey = "TrialResult";

		// Token: 0x040027E4 RID: 10212
		private const string _trialStoryKey = "TrialStory";
	}
}
