using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200056C RID: 1388
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JossPaper : RelicModel
	{
		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x060051AB RID: 20907 RVA: 0x00241593 File Offset: 0x0023F793
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x060051AC RID: 20908 RVA: 0x0024159A File Offset: 0x0023F79A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x060051AD RID: 20909 RVA: 0x0024159D File Offset: 0x0023F79D
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060051AE RID: 20910 RVA: 0x002415A0 File Offset: 0x0023F7A0
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.CardsExhausted;
				}
				return base.DynamicVars["ExhaustAmount"].IntValue;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x060051AF RID: 20911 RVA: 0x002415C6 File Offset: 0x0023F7C6
		// (set) Token: 0x060051B0 RID: 20912 RVA: 0x002415CE File Offset: 0x0023F7CE
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

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x060051B1 RID: 20913 RVA: 0x002415E3 File Offset: 0x0023F7E3
		// (set) Token: 0x060051B2 RID: 20914 RVA: 0x002415EC File Offset: 0x0023F7EC
		[SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
		public int CardsExhausted
		{
			get
			{
				return this._cardsExhausted;
			}
			set
			{
				base.AssertMutable();
				this._cardsExhausted = value;
				base.Status = ((this._cardsExhausted == base.DynamicVars["ExhaustAmount"].BaseValue - 1m) ? RelicStatus.Active : RelicStatus.Normal);
				base.InvokeDisplayAmountChanged();
			}
		}

		/// <summary>
		/// If Ethereal cards are exhausted at turn end, we want to give the resulting cards to the player after the
		/// flush occurs. In STS1 this is handled because the card draws are put on the queue, but here we have to
		/// manually defer.
		/// </summary>
		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x060051B3 RID: 20915 RVA: 0x00241647 File Offset: 0x0023F847
		// (set) Token: 0x060051B4 RID: 20916 RVA: 0x0024164F File Offset: 0x0023F84F
		private int EtherealCount
		{
			get
			{
				return this._etherealCount;
			}
			set
			{
				base.AssertMutable();
				this._etherealCount = value;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x060051B5 RID: 20917 RVA: 0x0024165E File Offset: 0x0023F85E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x060051B6 RID: 20918 RVA: 0x0024166B File Offset: 0x0023F86B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("ExhaustAmount", 5m),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x00241694 File Offset: 0x0023F894
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			if (card.Owner == base.Owner)
			{
				if (causedByEthereal)
				{
					int num = this.EtherealCount;
					this.EtherealCount = num + 1;
				}
				else
				{
					int num = this.CardsExhausted;
					this.CardsExhausted = num + 1;
					await this.DrawIfThresholdMet(choiceContext);
				}
			}
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x002416F0 File Offset: 0x0023F8F0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				this.CardsExhausted += this.EtherealCount;
				this.EtherealCount = 0;
				await this.DrawIfThresholdMet(choiceContext);
			}
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00241744 File Offset: 0x0023F944
		private async Task DrawIfThresholdMet(PlayerChoiceContext choiceContext)
		{
			if (!(this.CardsExhausted < base.DynamicVars["ExhaustAmount"].BaseValue))
			{
				TaskHelper.RunSafely(this.DoActivateVisuals());
				await CardPileCmd.Draw(choiceContext, (int)(this.CardsExhausted / base.DynamicVars["ExhaustAmount"].BaseValue), base.Owner, false);
				this.CardsExhausted %= base.DynamicVars["ExhaustAmount"].IntValue;
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00241790 File Offset: 0x0023F990
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x002417D3 File Offset: 0x0023F9D3
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.EtherealCount = 0;
			return Task.CompletedTask;
		}

		// Token: 0x04002461 RID: 9313
		private const string _exhaustAmountKey = "ExhaustAmount";

		// Token: 0x04002462 RID: 9314
		private bool _isActivating;

		// Token: 0x04002463 RID: 9315
		private int _cardsExhausted;

		// Token: 0x04002464 RID: 9316
		private int _etherealCount;
	}
}
