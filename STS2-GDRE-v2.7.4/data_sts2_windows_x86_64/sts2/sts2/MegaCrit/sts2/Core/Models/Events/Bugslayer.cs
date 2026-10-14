using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082D RID: 2093
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bugslayer : EventModel
	{
		// Token: 0x06006784 RID: 26500 RVA: 0x00272F18 File Offset: 0x00271118
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Extermination), "BUGSLAYER.pages.INITIAL.options.EXTERMINATION", HoverTipFactory.FromCardWithCardHoverTips<Exterminate>(false)),
				new EventOption(this, new Func<Task>(this.Squash), "BUGSLAYER.pages.INITIAL.options.SQUASH", HoverTipFactory.FromCardWithCardHoverTips<Squash>(false))
			});
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x00272F70 File Offset: 0x00271170
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Card1", ModelDb.Card<Exterminate>().Title),
					new StringVar("Card2", ModelDb.Card<Squash>().Title)
				});
			}
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x00272FAC File Offset: 0x002711AC
		private async Task Extermination()
		{
			await this.AddAndPreview<Exterminate>(base.L10NLookup("BUGSLAYER.pages.EXTERMINATION.description"));
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00272FF0 File Offset: 0x002711F0
		private async Task Squash()
		{
			await this.AddAndPreview<Squash>(base.L10NLookup("BUGSLAYER.pages.SQUASH.description"));
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x00273034 File Offset: 0x00271234
		private async Task AddAndPreview<[Nullable(0)] T>(LocString loc) where T : CardModel
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<T>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(loc);
		}
	}
}
