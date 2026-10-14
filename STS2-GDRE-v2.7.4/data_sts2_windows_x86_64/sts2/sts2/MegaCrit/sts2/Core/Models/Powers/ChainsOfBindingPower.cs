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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000642 RID: 1602
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChainsOfBindingPower : PowerModel
	{
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06005727 RID: 22311 RVA: 0x0024C507 File Offset: 0x0024A707
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06005728 RID: 22312 RVA: 0x0024C50A File Offset: 0x0024A70A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x0024C50D File Offset: 0x0024A70D
		protected override object InitInternalData()
		{
			return new ChainsOfBindingPower.Data();
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x0600572A RID: 22314 RVA: 0x0024C514 File Offset: 0x0024A714
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromAffliction<Bound>(1);
			}
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x0024C51C File Offset: 0x0024A71C
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner == base.Owner.Player)
			{
				if (base.CombatState.CurrentSide == base.Owner.Side)
				{
					if (ModelDb.Affliction<Bound>().CanAfflict(card))
					{
						int num = CombatManager.Instance.History.Entries.OfType<CardAfflictedEntry>().Count<CardAfflictedEntry>((CardAfflictedEntry e) => e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner && e.Affliction is Bound);
						if (num < base.Amount)
						{
							await CardCmd.AfflictAndPreview<Bound>(new <>z__ReadOnlySingleElementList<CardModel>(card), base.Amount, CardPreviewStyle.None);
						}
					}
				}
			}
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x0024C568 File Offset: 0x0024A768
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			CardModel card = cardPlay.Card;
			if (card.IsDupe)
			{
				return Task.CompletedTask;
			}
			if (card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!(card.Affliction is Bound))
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed = true;
			return Task.CompletedTask;
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x0024C5C7 File Offset: 0x0024A7C7
		public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
		{
			return card.Owner.Creature != base.Owner || !(card.Affliction is Bound) || !base.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed;
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x0024C5FC File Offset: 0x0024A7FC
		public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed = false;
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
				if (cardModel.Affliction is Bound)
				{
					CardCmd.ClearAffliction(cardModel);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x02001B91 RID: 7057
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006D9B RID: 28059
			public bool boundCardPlayed;
		}
	}
}
