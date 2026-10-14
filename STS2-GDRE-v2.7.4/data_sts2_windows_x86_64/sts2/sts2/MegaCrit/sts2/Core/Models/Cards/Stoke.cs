using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AEA RID: 2794
	public sealed class Stoke : CardModel
	{
		// Token: 0x06007801 RID: 30721 RVA: 0x00298DD3 File Offset: 0x00296FD3
		public Stoke()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x00298DE0 File Offset: 0x00296FE0
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.ToList<CardModel>();
			int exhaustCount = list.Count;
			foreach (CardModel cardModel in list)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			List<CardModel> list2 = CardFactory.GetForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), exhaustCount, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
			if (base.IsUpgraded)
			{
				CardCmd.Upgrade(list2, CardPreviewStyle.None);
			}
			await CardPileCmd.AddGeneratedCardsToCombat(list2, PileType.Hand, base.Owner, CardPilePosition.Bottom);
		}
	}
}
