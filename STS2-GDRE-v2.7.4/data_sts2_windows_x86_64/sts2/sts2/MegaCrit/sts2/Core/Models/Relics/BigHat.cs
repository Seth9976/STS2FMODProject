using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FF RID: 1279
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BigHat : RelicModel
	{
		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06004F1F RID: 20255 RVA: 0x0023CC1A File Offset: 0x0023AE1A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06004F20 RID: 20256 RVA: 0x0023CC1D File Offset: 0x0023AE1D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06004F21 RID: 20257 RVA: 0x0023CC2A File Offset: 0x0023AE2A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0023CC38 File Offset: 0x0023AE38
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					IReadOnlyList<CardModel> readOnlyList = (from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
						where c.Keywords.Contains(CardKeyword.Ethereal)
						select c).ToList<CardModel>();
					if (readOnlyList.Count > 0)
					{
						base.Flash();
						List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, readOnlyList, base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
						base.Flash();
						await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
					}
				}
			}
		}
	}
}
