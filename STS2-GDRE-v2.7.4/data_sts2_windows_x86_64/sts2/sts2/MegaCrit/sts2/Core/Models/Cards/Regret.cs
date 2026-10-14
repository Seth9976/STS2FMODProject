using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A9D RID: 2717
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Regret : CardModel
	{
		// Token: 0x06007647 RID: 30279 RVA: 0x002955DF File Offset: 0x002937DF
		public Regret()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001FE7 RID: 8167
		// (get) Token: 0x06007648 RID: 30280 RVA: 0x002955ED File Offset: 0x002937ED
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001FE8 RID: 8168
		// (get) Token: 0x06007649 RID: 30281 RVA: 0x002955F0 File Offset: 0x002937F0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001FE9 RID: 8169
		// (get) Token: 0x0600764A RID: 30282 RVA: 0x002955F8 File Offset: 0x002937F8
		// (set) Token: 0x0600764B RID: 30283 RVA: 0x00295600 File Offset: 0x00293800
		private int CardsInHand
		{
			get
			{
				return this._cardsInHand;
			}
			set
			{
				base.AssertMutable();
				this._cardsInHand = value;
			}
		}

		// Token: 0x17001FEA RID: 8170
		// (get) Token: 0x0600764C RID: 30284 RVA: 0x0029560F File Offset: 0x0029380F
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600764D RID: 30285 RVA: 0x00295614 File Offset: 0x00293814
		public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			if (base.Pile.Type != PileType.Hand)
			{
				return Task.CompletedTask;
			}
			this.CardsInHand = base.Pile.Cards.Count;
			return Task.CompletedTask;
		}

		// Token: 0x0600764E RID: 30286 RVA: 0x0029566C File Offset: 0x0029386C
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, this.CardsInHand, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, null);
			this.CardsInHand = 0;
		}

		// Token: 0x040028B7 RID: 10423
		private int _cardsInHand;
	}
}
