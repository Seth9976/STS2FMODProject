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
	// Token: 0x020006DD RID: 1757
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RingingPower : PowerModel
	{
		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x00252B1B File Offset: 0x00250D1B
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x00252B1E File Offset: 0x00250D1E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x00252B24 File Offset: 0x00250D24
		[NullableContext(2)]
		[return: Nullable(1)]
		public override async Task AfterApplied(Creature applier, CardModel cardSource)
		{
			IEnumerable<CardModel> allCards = base.Owner.Player.PlayerCombatState.AllCards;
			foreach (CardModel cardModel in allCards)
			{
				if (cardModel.Affliction == null)
				{
					await CardCmd.Afflict<Ringing>(cardModel, 1m);
				}
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x00252B68 File Offset: 0x00250D68
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Owner == base.Owner.Player)
			{
				if (card.Affliction == null)
				{
					await CardCmd.Afflict<Ringing>(card, 1m);
				}
			}
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x00252BB4 File Offset: 0x00250DB4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x00252C00 File Offset: 0x00250E00
		public override Task AfterRemoved(Creature oldOwner)
		{
			Player player = oldOwner.Player;
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
				if (cardModel.Affliction is Ringing)
				{
					CardCmd.ClearAffliction(cardModel);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x00252C84 File Offset: 0x00250E84
		public override bool ShouldPlay(CardModel card, AutoPlayType _)
		{
			return card.Owner.Creature != base.Owner || !(card.Affliction is Ringing) || !CombatManager.Instance.History.CardPlaysStarted.Any<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Player == base.Owner.Player);
		}
	}
}
