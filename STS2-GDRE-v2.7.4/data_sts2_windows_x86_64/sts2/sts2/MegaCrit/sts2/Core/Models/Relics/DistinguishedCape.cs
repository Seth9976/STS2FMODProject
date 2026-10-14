using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200052F RID: 1327
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DistinguishedCape : RelicModel
	{
		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005030 RID: 20528 RVA: 0x0023EB6C File Offset: 0x0023CD6C
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005031 RID: 20529 RVA: 0x0023EB6F File Offset: 0x0023CD6F
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005032 RID: 20530 RVA: 0x0023EB72 File Offset: 0x0023CD72
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Apparition>(false);
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x0023EB7A File Offset: 0x0023CD7A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Curses", 2m),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x0023EBA4 File Offset: 0x0023CDA4
		public override async Task AfterObtained()
		{
			List<CardModel> availableCurses = (from c in ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where c.CanBeGeneratedByModifiers
				orderby c.Id
				select c).ToList<CardModel>();
			List<CardPileAddResult> curseResults = new List<CardPileAddResult>();
			for (int i = 0; i < base.DynamicVars["Curses"].IntValue; i++)
			{
				CardModel cardModel = base.Owner.RunState.Rng.Niche.NextItem<CardModel>(availableCurses);
				availableCurses.Remove(cardModel);
				CardModel cardModel2 = base.Owner.RunState.CreateCard(cardModel, base.Owner);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel2, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				curseResults.Add(cardPileAddResult2);
			}
			CardCmd.PreviewCardPileAdd(curseResults, 2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.75f, false);
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				CardModel cardModel3 = base.Owner.RunState.CreateCard<Apparition>(base.Owner);
				List<CardPileAddResult> list = results;
				list.Add(await CardPileCmd.Add(cardModel3, PileType.Deck, CardPilePosition.Bottom, null, false));
				list = null;
			}
			CardCmd.PreviewCardPileAdd(results, 2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x0400243C RID: 9276
		private const string _cursesKey = "Curses";
	}
}
