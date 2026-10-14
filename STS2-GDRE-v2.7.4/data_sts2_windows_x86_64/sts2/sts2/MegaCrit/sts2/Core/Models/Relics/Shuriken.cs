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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E0 RID: 1504
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shuriken : RelicModel
	{
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x002474DC File Offset: 0x002456DC
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060054C7 RID: 21703 RVA: 0x002474DF File Offset: 0x002456DF
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x060054C8 RID: 21704 RVA: 0x002474EB File Offset: 0x002456EB
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.AttacksPlayedThisTurn % base.DynamicVars.Cards.IntValue;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x060054C9 RID: 21705 RVA: 0x0024751D File Offset: 0x0024571D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new PowerVar<StrengthPower>(1m)
				});
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x060054CA RID: 21706 RVA: 0x00247540 File Offset: 0x00245740
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x060054CB RID: 21707 RVA: 0x00247560 File Offset: 0x00245760
		// (set) Token: 0x060054CC RID: 21708 RVA: 0x00247568 File Offset: 0x00245768
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

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x060054CD RID: 21709 RVA: 0x0024757D File Offset: 0x0024577D
		// (set) Token: 0x060054CE RID: 21710 RVA: 0x00247585 File Offset: 0x00245785
		private int AttacksPlayedThisTurn
		{
			get
			{
				return this._attacksPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._attacksPlayedThisTurn = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x0024759C File Offset: 0x0024579C
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars.Cards.IntValue;
				base.Status = ((this.AttacksPlayedThisTurn % intValue == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x002475E8 File Offset: 0x002457E8
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.AttacksPlayedThisTurn = 0;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x00247618 File Offset: 0x00245818
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (CombatManager.Instance.IsInProgress)
				{
					if (cardPlay.Card.Type == CardType.Attack)
					{
						int attacksPlayedThisTurn = this.AttacksPlayedThisTurn;
						this.AttacksPlayedThisTurn = attacksPlayedThisTurn + 1;
						int intValue = base.DynamicVars.Cards.IntValue;
						if (this.AttacksPlayedThisTurn % intValue == 0)
						{
							TaskHelper.RunSafely(this.DoActivateVisuals());
							await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
						}
					}
				}
			}
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x0024766C File Offset: 0x0024586C
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x002476AF File Offset: 0x002458AF
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.AttacksPlayedThisTurn = 0;
			this.IsActivating = false;
			return Task.CompletedTask;
		}

		// Token: 0x040024BB RID: 9403
		private bool _isActivating;

		// Token: 0x040024BC RID: 9404
		private int _attacksPlayedThisTurn;
	}
}
