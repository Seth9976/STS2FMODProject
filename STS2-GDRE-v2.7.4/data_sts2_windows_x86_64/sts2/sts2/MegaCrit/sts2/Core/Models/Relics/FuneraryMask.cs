using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
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
	// Token: 0x02000550 RID: 1360
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FuneraryMask : RelicModel
	{
		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x00240039 File Offset: 0x0023E239
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x0024003C File Offset: 0x0023E23C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x00240049 File Offset: 0x0023E249
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Soul>(false);
			}
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x00240054 File Offset: 0x0023E254
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					int i = 0;
					while (i < base.DynamicVars.Cards.BaseValue)
					{
						CardModel cardModel = combatState.CreateCard(ModelDb.Card<Soul>(), base.Owner);
						CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, base.Owner, CardPilePosition.Random);
						CardPileAddResult cardPileAddResult2 = cardPileAddResult;
						CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
						i++;
					}
				}
			}
		}
	}
}
