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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A0 RID: 1440
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OrnamentalFan : RelicModel
	{
		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06005306 RID: 21254 RVA: 0x00243F2E File Offset: 0x0024212E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06005307 RID: 21255 RVA: 0x00243F31 File Offset: 0x00242131
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06005308 RID: 21256 RVA: 0x00243F3D File Offset: 0x0024213D
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

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06005309 RID: 21257 RVA: 0x00243F6F File Offset: 0x0024216F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new BlockVar(4m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x0600530A RID: 21258 RVA: 0x00243F94 File Offset: 0x00242194
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x0600530B RID: 21259 RVA: 0x00243FA6 File Offset: 0x002421A6
		// (set) Token: 0x0600530C RID: 21260 RVA: 0x00243FAE File Offset: 0x002421AE
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

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x0600530D RID: 21261 RVA: 0x00243FC3 File Offset: 0x002421C3
		// (set) Token: 0x0600530E RID: 21262 RVA: 0x00243FCB File Offset: 0x002421CB
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

		// Token: 0x0600530F RID: 21263 RVA: 0x00243FE0 File Offset: 0x002421E0
		private void UpdateDisplay()
		{
			if (this.IsActivating || !CombatManager.Instance.IsInProgress)
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

		// Token: 0x06005310 RID: 21264 RVA: 0x00244038 File Offset: 0x00242238
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

		// Token: 0x06005311 RID: 21265 RVA: 0x00244068 File Offset: 0x00242268
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
							await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
						}
					}
				}
			}
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x002440B4 File Offset: 0x002422B4
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x002440F7 File Offset: 0x002422F7
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.IsActivating = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002483 RID: 9347
		private bool _isActivating;

		// Token: 0x04002484 RID: 9348
		private int _attacksPlayedThisTurn;
	}
}
