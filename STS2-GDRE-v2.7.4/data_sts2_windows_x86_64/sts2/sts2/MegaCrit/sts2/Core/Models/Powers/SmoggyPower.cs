using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Afflictions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F6 RID: 1782
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SmoggyPower : PowerModel
	{
		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x00253EF7 File Offset: 0x002520F7
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06005B46 RID: 23366 RVA: 0x00253EFA File Offset: 0x002520FA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x00253F00 File Offset: 0x00252100
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner)
			{
				if (cardPlay.Card.Type == CardType.Skill)
				{
					base.Flash();
					IEnumerable<CardModel> allCards = base.Owner.Player.PlayerCombatState.AllCards;
					foreach (CardModel cardModel in allCards)
					{
						if (cardModel.Type == CardType.Skill && cardModel.Affliction == null)
						{
							await CardCmd.Afflict<Smog>(cardModel, 1m);
						}
					}
					IEnumerator<CardModel> enumerator = null;
				}
			}
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x00253F4C File Offset: 0x0025214C
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Owner == base.Owner.Player)
			{
				if (card.Affliction == null)
				{
					if (card.Type == CardType.Skill)
					{
						if (CombatManager.Instance.History.CardPlaysStarted.Any<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == base.Owner.Player))
						{
							await CardCmd.Afflict<Smog>(card, 1m);
						}
					}
				}
			}
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x00253F98 File Offset: 0x00252198
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			Player player = base.Owner.Player;
			IEnumerable<CardModel> enumerable;
			if (player == null)
			{
				enumerable = null;
			}
			else
			{
				PlayerCombatState playerCombatState = player.PlayerCombatState;
				enumerable = ((playerCombatState != null) ? playerCombatState.AllCards : null);
			}
			IEnumerable<CardModel> enumerable2 = enumerable ?? Array.Empty<CardModel>();
			foreach (CardModel cardModel in enumerable2)
			{
				if (cardModel.Affliction is Smog)
				{
					CardCmd.ClearAffliction(cardModel);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x00254034 File Offset: 0x00252234
		public override bool ShouldPlay(CardModel card, AutoPlayType _)
		{
			return card.Owner != base.Owner.Player || !(card.Affliction is Smog);
		}
	}
}
