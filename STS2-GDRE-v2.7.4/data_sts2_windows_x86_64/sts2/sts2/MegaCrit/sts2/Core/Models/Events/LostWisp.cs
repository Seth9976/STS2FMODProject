using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000840 RID: 2112
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LostWisp : EventModel
	{
		// Token: 0x06006821 RID: 26657 RVA: 0x00275B44 File Offset: 0x00273D44
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			EventOption[] array = new EventOption[2];
			int num = 0;
			Func<Task> func = new Func<Task>(this.Claim);
			string text = "LOST_WISP.pages.INITIAL.options.CLAIM";
			List<IHoverTip> list = new List<IHoverTip>();
			list.AddRange(HoverTipFactory.FromRelic<LostWisp>());
			list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Decay>(false));
			array[num] = new EventOption(this, func, text, list.ToArray());
			array[1] = new EventOption(this, new Func<Task>(this.Search), "LOST_WISP.pages.INITIAL.options.SEARCH", Array.Empty<IHoverTip>());
			return new <>z__ReadOnlyArray<EventOption>(array);
		}

		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x06006822 RID: 26658 RVA: 0x00275BB8 File Offset: 0x00273DB8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(60),
					new StringVar("Relic", ModelDb.Relic<LostWisp>().Title.GetFormattedText()),
					new StringVar("Curse", ModelDb.Card<Decay>().Title)
				});
			}
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x00275C0D File Offset: 0x00273E0D
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue += base.Rng.NextInt(-15, 16);
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x00275C40 File Offset: 0x00273E40
		private async Task Claim()
		{
			await CardPileCmd.AddCursesToDeck(new <>z__ReadOnlySingleElementList<CardModel>(ModelDb.Card<Decay>()), base.Owner);
			await RelicCmd.Obtain<LostWisp>(base.Owner);
			base.SetEventFinished(base.L10NLookup("LOST_WISP.pages.CLAIM.description"));
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x00275C84 File Offset: 0x00273E84
		private async Task Search()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.IntValue, base.Owner, false);
			base.SetEventFinished(base.L10NLookup("LOST_WISP.pages.SEARCH.description"));
		}

		// Token: 0x0400278B RID: 10123
		private const string _relicKey = "Relic";

		// Token: 0x0400278C RID: 10124
		private const string _curseKey = "Curse";

		// Token: 0x0400278D RID: 10125
		private const int _baseGold = 60;

		// Token: 0x0400278E RID: 10126
		private const int _goldVariance = 15;
	}
}
