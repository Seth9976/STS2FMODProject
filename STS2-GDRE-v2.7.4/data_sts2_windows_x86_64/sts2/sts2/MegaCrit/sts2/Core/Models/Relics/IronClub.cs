using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000568 RID: 1384
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IronClub : RelicModel
	{
		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x002412E3 File Offset: 0x0023F4E3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x002412E6 File Offset: 0x0023F4E6
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x002412ED File Offset: 0x0023F4ED
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x002412F0 File Offset: 0x0023F4F0
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.CardsPlayed % base.DynamicVars.Cards.IntValue;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x00241322 File Offset: 0x0023F522
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(4));
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x0024132F File Offset: 0x0023F52F
		// (set) Token: 0x06005197 RID: 20887 RVA: 0x00241337 File Offset: 0x0023F537
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

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x0024134C File Offset: 0x0023F54C
		// (set) Token: 0x06005199 RID: 20889 RVA: 0x00241354 File Offset: 0x0023F554
		[SavedProperty]
		public int CardsPlayed
		{
			get
			{
				return this._cardsPlayed;
			}
			set
			{
				base.AssertMutable();
				this._cardsPlayed = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x0024136C File Offset: 0x0023F56C
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars.Cards.IntValue;
				base.Status = ((this.CardsPlayed % intValue == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x002413B8 File Offset: 0x0023F5B8
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				int cardsPlayed = this.CardsPlayed;
				this.CardsPlayed = cardsPlayed + 1;
				int intValue = base.DynamicVars.Cards.IntValue;
				if (CombatManager.Instance.IsInProgress && this.CardsPlayed % intValue == 0)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await CardPileCmd.Draw(choiceContext, 1m, base.Owner, false);
				}
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x0024140C File Offset: 0x0023F60C
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x0400245E RID: 9310
		private bool _isActivating;

		// Token: 0x0400245F RID: 9311
		private int _cardsPlayed;
	}
}
