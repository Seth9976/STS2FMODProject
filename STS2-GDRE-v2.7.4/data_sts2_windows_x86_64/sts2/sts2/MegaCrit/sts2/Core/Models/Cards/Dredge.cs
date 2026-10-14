using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099D RID: 2461
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dredge : CardModel
	{
		// Token: 0x060070E3 RID: 28899 RVA: 0x0028A333 File Offset: 0x00288533
		public Dredge()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DA6 RID: 7590
		// (get) Token: 0x060070E4 RID: 28900 RVA: 0x0028A340 File Offset: 0x00288540
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001DA7 RID: 7591
		// (get) Token: 0x060070E5 RID: 28901 RVA: 0x0028A34D File Offset: 0x0028854D
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x0028A358 File Offset: 0x00288558
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int num = Math.Min(base.DynamicVars.Cards.IntValue, CardPile.MaxCardsInHand - PileType.Hand.GetPile(base.Owner).Cards.Count);
			if (num > 0)
			{
				await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, num)), PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x0028A3A3 File Offset: 0x002885A3
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
