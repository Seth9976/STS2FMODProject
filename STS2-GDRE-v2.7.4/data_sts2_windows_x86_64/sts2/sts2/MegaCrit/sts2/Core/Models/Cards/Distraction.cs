using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000995 RID: 2453
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Distraction : CardModel
	{
		// Token: 0x060070B6 RID: 28854 RVA: 0x00289E27 File Offset: 0x00288027
		public Distraction()
			: base(1, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001D90 RID: 7568
		// (get) Token: 0x060070B7 RID: 28855 RVA: 0x00289E34 File Offset: 0x00288034
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<SilentCardPool>();
			}
		}

		// Token: 0x17001D91 RID: 7569
		// (get) Token: 0x060070B8 RID: 28856 RVA: 0x00289E3B File Offset: 0x0028803B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060070B9 RID: 28857 RVA: 0x00289E44 File Offset: 0x00288044
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardModel cardModel = CardFactory.GetDistinctForCombat(base.Owner, from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
				where c.Type == CardType.Skill
				select c, 1, base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				cardModel.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x00289E87 File Offset: 0x00288087
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
