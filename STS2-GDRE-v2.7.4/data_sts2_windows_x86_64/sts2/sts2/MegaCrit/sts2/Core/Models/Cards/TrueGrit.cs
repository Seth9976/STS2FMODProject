using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B20 RID: 2848
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TrueGrit : CardModel
	{
		// Token: 0x0600791D RID: 31005 RVA: 0x0029B31E File Offset: 0x0029951E
		public TrueGrit()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x1700210F RID: 8463
		// (get) Token: 0x0600791E RID: 31006 RVA: 0x0029B32B File Offset: 0x0029952B
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002110 RID: 8464
		// (get) Token: 0x0600791F RID: 31007 RVA: 0x0029B32E File Offset: 0x0029952E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x17002111 RID: 8465
		// (get) Token: 0x06007920 RID: 31008 RVA: 0x0029B341 File Offset: 0x00299541
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x0029B350 File Offset: 0x00299550
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			if (base.IsUpgraded)
			{
				CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
				CardModel cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this)).FirstOrDefault<CardModel>();
				if (cardModel != null)
				{
					await CardCmd.Exhaust(choiceContext, cardModel, false, false);
				}
			}
			else
			{
				CardPile pile = PileType.Hand.GetPile(base.Owner);
				CardModel cardModel2 = base.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(pile.Cards);
				if (cardModel2 != null)
				{
					await CardCmd.Exhaust(choiceContext, cardModel2, false, false);
				}
			}
		}

		// Token: 0x06007922 RID: 31010 RVA: 0x0029B3A3 File Offset: 0x002995A3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
