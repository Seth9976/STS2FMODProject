using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A04 RID: 2564
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hologram : CardModel
	{
		// Token: 0x06007315 RID: 29461 RVA: 0x0028EADC File Offset: 0x0028CCDC
		public Hologram()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E98 RID: 7832
		// (get) Token: 0x06007316 RID: 29462 RVA: 0x0028EAE9 File Offset: 0x0028CCE9
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E99 RID: 7833
		// (get) Token: 0x06007317 RID: 29463 RVA: 0x0028EAEC File Offset: 0x0028CCEC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(3m, ValueProp.Move));
			}
		}

		// Token: 0x17001E9A RID: 7834
		// (get) Token: 0x06007318 RID: 29464 RVA: 0x0028EAFF File Offset: 0x0028CCFF
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007319 RID: 29465 RVA: 0x0028EB08 File Offset: 0x0028CD08
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(base.Owner), base.Owner, cardSelectorPrefs);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}

		// Token: 0x0600731A RID: 29466 RVA: 0x0028EB5B File Offset: 0x0028CD5B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
