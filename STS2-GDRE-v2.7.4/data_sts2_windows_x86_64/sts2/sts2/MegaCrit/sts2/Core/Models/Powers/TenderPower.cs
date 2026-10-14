using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000717 RID: 1815
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TenderPower : PowerModel
	{
		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x00255C1F File Offset: 0x00253E1F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06005C1E RID: 23582 RVA: 0x00255C22 File Offset: 0x00253E22
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x06005C1F RID: 23583 RVA: 0x00255C25 File Offset: 0x00253E25
		public override int DisplayAmount
		{
			get
			{
				return this.CardsPlayedThisTurn;
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x06005C20 RID: 23584 RVA: 0x00255C30 File Offset: 0x00253E30
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06005C21 RID: 23585 RVA: 0x00255C6A File Offset: 0x00253E6A
		// (set) Token: 0x06005C22 RID: 23586 RVA: 0x00255C72 File Offset: 0x00253E72
		private int CardsPlayedThisTurn
		{
			get
			{
				return this._cardsPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._cardsPlayedThisTurn = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x00255C88 File Offset: 0x00253E88
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				int cardsPlayedThisTurn = this.CardsPlayedThisTurn;
				this.CardsPlayedThisTurn = cardsPlayedThisTurn + 1;
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -1m, base.Applier, null, true);
				await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, -1m, base.Applier, null, true);
			}
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x00255CDC File Offset: 0x00253EDC
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, this.CardsPlayedThisTurn, base.Applier, null, true);
				await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, this.CardsPlayedThisTurn, base.Applier, null, true);
				this.CardsPlayedThisTurn = 0;
			}
		}

		// Token: 0x04002525 RID: 9509
		private int _cardsPlayedThisTurn;
	}
}
