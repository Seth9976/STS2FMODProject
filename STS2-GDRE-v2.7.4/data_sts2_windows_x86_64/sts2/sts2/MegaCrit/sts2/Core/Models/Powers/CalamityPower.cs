using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063F RID: 1599
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CalamityPower : PowerModel
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x0024C36B File Offset: 0x0024A56B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x0024C36E File Offset: 0x0024A56E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x0024C371 File Offset: 0x0024A571
		protected override object InitInternalData()
		{
			return new CalamityPower.Data();
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x0024C378 File Offset: 0x0024A578
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<CalamityPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x0024C3D8 File Offset: 0x0024A5D8
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num;
			if (base.GetInternalData<CalamityPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num))
			{
				List<CardModel> list = CardFactory.GetForCombat(base.Owner.Player, from c in base.Owner.Player.Character.CardPool.GetUnlockedCards(base.Owner.Player.UnlockState, base.Owner.Player.RunState.CardMultiplayerConstraint)
					where c.Type == CardType.Attack
					select c, base.Amount, base.Owner.Player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
				foreach (CardModel cardModel in list)
				{
					await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				}
				List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			}
		}

		// Token: 0x02001B8C RID: 7052
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006D8B RID: 28043
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
