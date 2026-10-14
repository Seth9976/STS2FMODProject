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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000599 RID: 1433
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Nunchaku : RelicModel
	{
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060052D7 RID: 21207 RVA: 0x00243A2F File Offset: 0x00241C2F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x060052D8 RID: 21208 RVA: 0x00243A32 File Offset: 0x00241C32
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x060052D9 RID: 21209 RVA: 0x00243A35 File Offset: 0x00241C35
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.AttacksPlayed % base.DynamicVars.Cards.IntValue;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060052DA RID: 21210 RVA: 0x00243A67 File Offset: 0x00241C67
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(10),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060052DB RID: 21211 RVA: 0x00243A87 File Offset: 0x00241C87
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060052DC RID: 21212 RVA: 0x00243A94 File Offset: 0x00241C94
		// (set) Token: 0x060052DD RID: 21213 RVA: 0x00243A9C File Offset: 0x00241C9C
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

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060052DE RID: 21214 RVA: 0x00243AB1 File Offset: 0x00241CB1
		// (set) Token: 0x060052DF RID: 21215 RVA: 0x00243AB9 File Offset: 0x00241CB9
		[SavedProperty]
		public int AttacksPlayed
		{
			get
			{
				return this._attacksPlayed;
			}
			set
			{
				base.AssertMutable();
				this._attacksPlayed = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x00243AD0 File Offset: 0x00241CD0
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars.Cards.IntValue;
				base.Status = ((this.AttacksPlayed % intValue == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x00243B1C File Offset: 0x00241D1C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Card.Type == CardType.Attack)
				{
					int attacksPlayed = this.AttacksPlayed;
					this.AttacksPlayed = attacksPlayed + 1;
					int intValue = base.DynamicVars.Cards.IntValue;
					if (CombatManager.Instance.IsInProgress && this.AttacksPlayed % intValue == 0)
					{
						TaskHelper.RunSafely(this.DoActivateVisuals());
						await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					}
				}
			}
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x00243B68 File Offset: 0x00241D68
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002480 RID: 9344
		private bool _isActivating;

		// Token: 0x04002481 RID: 9345
		private int _attacksPlayed;
	}
}
