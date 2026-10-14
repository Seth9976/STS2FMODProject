using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Potions.Mocks
{
	// Token: 0x02000783 RID: 1923
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockDiscardAndAddShivsPotion : PotionModel
	{
		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06005ECA RID: 24266 RVA: 0x00259923 File Offset: 0x00257B23
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06005ECB RID: 24267 RVA: 0x00259926 File Offset: 0x00257B26
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new DynamicVar("Shivs", 2m)
				});
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06005ECC RID: 24268 RVA: 0x0025994F File Offset: 0x00257B4F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.None;
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06005ECD RID: 24269 RVA: 0x00259952 File Offset: 0x00257B52
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06005ECE RID: 24270 RVA: 0x00259955 File Offset: 0x00257B55
		public override TargetType TargetType
		{
			get
			{
				return TargetType.Self;
			}
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x00259958 File Offset: 0x00257B58
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			await CardCmd.Discard(choiceContext, enumerable2);
			await Shiv.CreateInHand(base.Owner, base.DynamicVars["Shivs"].IntValue, base.Owner.Creature.CombatState, null);
		}

		// Token: 0x04002535 RID: 9525
		private const string _shivKey = "Shivs";
	}
}
