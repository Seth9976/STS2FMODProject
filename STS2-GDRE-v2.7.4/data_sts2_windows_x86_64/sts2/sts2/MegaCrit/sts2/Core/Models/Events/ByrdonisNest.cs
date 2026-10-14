using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082E RID: 2094
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ByrdonisNest : EventModel
	{
		// Token: 0x0600678A RID: 26506 RVA: 0x00273088 File Offset: 0x00271288
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Eat), "BYRDONIS_NEST.pages.INITIAL.options.EAT", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Take), "BYRDONIS_NEST.pages.INITIAL.options.TAKE", HoverTipFactory.FromCardWithCardHoverTips<ByrdonisEgg>(false))
			});
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x002730DF File Offset: 0x002712DF
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => !p.HasEventPet());
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600678C RID: 26508 RVA: 0x0027310B File Offset: 0x0027130B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new MaxHpVar(7m),
					new StringVar("Card", ModelDb.Card<ByrdonisEgg>().Title)
				});
			}
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x00273140 File Offset: 0x00271340
		private async Task Eat()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			base.SetEventFinished(base.L10NLookup("BYRDONIS_NEST.pages.EAT.description"));
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x00273184 File Offset: 0x00271384
		private async Task Take()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<ByrdonisEgg>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("BYRDONIS_NEST.pages.TAKE.description"));
		}

		// Token: 0x0400275D RID: 10077
		private const string _cardKey = "Card";
	}
}
