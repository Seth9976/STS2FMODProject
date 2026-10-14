using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000501 RID: 1281
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BiiigHug : RelicModel
	{
		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06004F2C RID: 20268 RVA: 0x0023CD81 File Offset: 0x0023AF81
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06004F2D RID: 20269 RVA: 0x0023CD84 File Offset: 0x0023AF84
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Soot>(false);
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06004F2E RID: 20270 RVA: 0x0023CD8C File Offset: 0x0023AF8C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(4));
			}
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x0023CD9C File Offset: 0x0023AF9C
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x0023CDE0 File Offset: 0x0023AFE0
		public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
		{
			if (shuffler == base.Owner)
			{
				CardModel soot = shuffler.Creature.CombatState.CreateCard<Soot>(base.Owner);
				await CardPileCmd.AddGeneratedCardToCombat(soot, PileType.Draw, base.Owner, CardPilePosition.Random);
				base.Flash();
				CardCmd.Preview(soot, 0.75f, CardPreviewStyle.HorizontalLayout);
				await Cmd.Wait(1f, false);
			}
		}
	}
}
