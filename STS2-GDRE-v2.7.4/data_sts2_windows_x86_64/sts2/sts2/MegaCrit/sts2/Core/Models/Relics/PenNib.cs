using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B3 RID: 1459
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class PenNib : RelicModel
	{
		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x002452AD File Offset: 0x002434AD
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x002452B0 File Offset: 0x002434B0
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x002452B3 File Offset: 0x002434B3
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.AttacksPlayed % 10;
				}
				return 10;
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x002452C9 File Offset: 0x002434C9
		// (set) Token: 0x060053AE RID: 21422 RVA: 0x002452D1 File Offset: 0x002434D1
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x002452E6 File Offset: 0x002434E6
		// (set) Token: 0x060053B0 RID: 21424 RVA: 0x002452EE File Offset: 0x002434EE
		[SavedProperty]
		public int AttacksPlayed
		{
			get
			{
				return this._attacksPlayed;
			}
			private set
			{
				base.AssertMutable();
				this._attacksPlayed = value % 10;
				this.UpdateDisplay();
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x060053B1 RID: 21425 RVA: 0x00245306 File Offset: 0x00243506
		// (set) Token: 0x060053B2 RID: 21426 RVA: 0x0024530E File Offset: 0x0024350E
		private CardModel AttackToDouble
		{
			get
			{
				return this._attackToDouble;
			}
			set
			{
				base.AssertMutable();
				this._attackToDouble = value;
			}
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x0024531D File Offset: 0x0024351D
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				base.Status = ((this.AttacksPlayed == 9) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		/// <summary>
		/// This is public so we can call it from save tests.
		/// </summary>
		// Token: 0x060053B4 RID: 21428 RVA: 0x0024534C File Offset: 0x0024354C
		public void NotifyAttackPlayed()
		{
			int attacksPlayed = this.AttacksPlayed;
			this.AttacksPlayed = attacksPlayed + 1;
			if (this.AttacksPlayed == 0)
			{
				TaskHelper.RunSafely(this.DoActivateVisuals());
			}
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x00245380 File Offset: 0x00243580
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			if (dealer != base.Owner.Creature && dealer != base.Owner.Osty)
			{
				return 1m;
			}
			if (this.AttackToDouble == null)
			{
				CardPile pile = cardSource.Pile;
				if ((pile == null || pile.Type != PileType.Play) && this.AttacksPlayed == 9)
				{
					return 2m;
				}
				return 1m;
			}
			else
			{
				if (cardSource == this.AttackToDouble)
				{
					return 2m;
				}
				return 1m;
			}
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x0024541C File Offset: 0x0024361C
		[NullableContext(1)]
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			this.NotifyAttackPlayed();
			if (this.AttacksPlayed == 0)
			{
				this.AttackToDouble = cardPlay.Card;
			}
			return Task.CompletedTask;
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x00245475 File Offset: 0x00243675
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.AttackToDouble == null)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card != this.AttackToDouble)
			{
				return Task.CompletedTask;
			}
			this.AttackToDouble = null;
			return Task.CompletedTask;
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x002454A8 File Offset: 0x002436A8
		[NullableContext(1)]
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002499 RID: 9369
		private const int _attacksThreshold = 10;

		// Token: 0x0400249A RID: 9370
		private bool _isActivating;

		// Token: 0x0400249B RID: 9371
		private int _attacksPlayed;

		// Token: 0x0400249C RID: 9372
		private CardModel _attackToDouble;
	}
}
