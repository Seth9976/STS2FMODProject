using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200050B RID: 1291
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BookOfFiveRings : RelicModel
	{
		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06004F5F RID: 20319 RVA: 0x0023D3BD File Offset: 0x0023B5BD
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x0023D3C0 File Offset: 0x0023B5C0
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06004F61 RID: 20321 RVA: 0x0023D3C8 File Offset: 0x0023B5C8
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06004F62 RID: 20322 RVA: 0x0023D3CB File Offset: 0x0023B5CB
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.CardsAddedSinceLastTrigger;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06004F63 RID: 20323 RVA: 0x0023D3EC File Offset: 0x0023B5EC
		// (set) Token: 0x06004F64 RID: 20324 RVA: 0x0023D3F4 File Offset: 0x0023B5F4
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
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x0023D409 File Offset: 0x0023B609
		// (set) Token: 0x06004F66 RID: 20326 RVA: 0x0023D411 File Offset: 0x0023B611
		[SavedProperty]
		public int CardsAdded
		{
			get
			{
				return this._cardsAdded;
			}
			set
			{
				base.AssertMutable();
				this._cardsAdded = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x0023D426 File Offset: 0x0023B626
		private int CardsAddedSinceLastTrigger
		{
			get
			{
				return this.CardsAdded % base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x0023D43F File Offset: 0x0023B63F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(5),
					new HealVar(20m)
				});
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x0023D464 File Offset: 0x0023B664
		public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			if (!base.Owner.Creature.IsDead)
			{
				if (card.Owner == base.Owner)
				{
					CardPile pile = card.Pile;
					if (pile != null && pile.Type == PileType.Deck)
					{
						int cardsAdded = this.CardsAdded;
						this.CardsAdded = cardsAdded + 1;
						if (this.CardsAddedSinceLastTrigger == 0)
						{
							TaskHelper.RunSafely(this.DoActivateVisuals());
							await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
						}
					}
				}
			}
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x0023D4B0 File Offset: 0x0023B6B0
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002429 RID: 9257
		private bool _isActivating;

		// Token: 0x0400242A RID: 9258
		private int _cardsAdded;
	}
}
