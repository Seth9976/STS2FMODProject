using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000861 RID: 2145
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TrashHeap : EventModel
	{
		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x06006974 RID: 26996 RVA: 0x0027C8A5 File Offset: 0x0027AAA5
		private static RelicModel[] Relics
		{
			get
			{
				return new RelicModel[]
				{
					ModelDb.Relic<DarkstonePeriapt>(),
					ModelDb.Relic<DreamCatcher>(),
					ModelDb.Relic<HandDrill>(),
					ModelDb.Relic<MawBank>(),
					ModelDb.Relic<TheBoot>()
				};
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x06006975 RID: 26997 RVA: 0x0027C8D8 File Offset: 0x0027AAD8
		private static CardModel[] Cards
		{
			get
			{
				return new CardModel[]
				{
					ModelDb.Card<Caltrops>(),
					ModelDb.Card<Clash>(),
					ModelDb.Card<Distraction>(),
					ModelDb.Card<DualWield>(),
					ModelDb.Card<Entrench>(),
					ModelDb.Card<HelloWorld>(),
					ModelDb.Card<Outmaneuver>(),
					ModelDb.Card<Rebound>(),
					ModelDb.Card<RipAndTear>(),
					ModelDb.Card<Stack>()
				};
			}
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x0027C93D File Offset: 0x0027AB3D
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player player) => player.Creature.CurrentHp > 5);
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x06006977 RID: 26999 RVA: 0x0027C969 File Offset: 0x0027AB69
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(8m),
					new GoldVar(100)
				});
			}
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x0027C990 File Offset: 0x0027AB90
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.DiveIn), "TRASH_HEAP.pages.INITIAL.options.DIVE_IN", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.HpLoss.IntValue),
				new EventOption(this, new Func<Task>(this.Grab), "TRASH_HEAP.pages.INITIAL.options.GRAB", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x0027CA00 File Offset: 0x0027AC00
		private async Task DiveIn()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
			RelicModel relicModel = base.Rng.NextItem<RelicModel>(TrashHeap.Relics);
			await RelicCmd.Obtain(relicModel.ToMutable(), base.Owner, -1);
			base.SetEventFinished(base.L10NLookup("TRASH_HEAP.pages.DIVE_IN.description"));
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x0027CA44 File Offset: 0x0027AC44
		private async Task Grab()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			CardModel cardModel = base.Owner.RunState.CreateCard(base.Rng.NextItem<CardModel>(TrashHeap.Cards), base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("TRASH_HEAP.pages.GRAB.description"));
		}
	}
}
