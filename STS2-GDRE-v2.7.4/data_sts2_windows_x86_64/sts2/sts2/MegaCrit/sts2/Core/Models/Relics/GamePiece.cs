using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000554 RID: 1364
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GamePiece : RelicModel
	{
		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005120 RID: 20768 RVA: 0x002406DF File Offset: 0x0023E8DF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x002406E2 File Offset: 0x0023E8E2
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x002406E9 File Offset: 0x0023E8E9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x002406F8 File Offset: 0x0023E8F8
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					if (cardPlay.Card.Type == CardType.Power)
					{
						base.Flash();
						await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
					}
				}
			}
		}
	}
}
