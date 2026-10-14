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
	// Token: 0x02000570 RID: 1392
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Kunai : RelicModel
	{
		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060051CE RID: 20942 RVA: 0x0024195F File Offset: 0x0023FB5F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060051CF RID: 20943 RVA: 0x00241962 File Offset: 0x0023FB62
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x0024196E File Offset: 0x0023FB6E
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

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060051D1 RID: 20945 RVA: 0x002419A0 File Offset: 0x0023FBA0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060051D2 RID: 20946 RVA: 0x002419C4 File Offset: 0x0023FBC4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060051D3 RID: 20947 RVA: 0x002419E4 File Offset: 0x0023FBE4
		// (set) Token: 0x060051D4 RID: 20948 RVA: 0x002419EC File Offset: 0x0023FBEC
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

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060051D5 RID: 20949 RVA: 0x00241A01 File Offset: 0x0023FC01
		// (set) Token: 0x060051D6 RID: 20950 RVA: 0x00241A09 File Offset: 0x0023FC09
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

		// Token: 0x060051D7 RID: 20951 RVA: 0x00241A20 File Offset: 0x0023FC20
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

		// Token: 0x060051D8 RID: 20952 RVA: 0x00241A6C File Offset: 0x0023FC6C
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

		// Token: 0x060051D9 RID: 20953 RVA: 0x00241A9C File Offset: 0x0023FC9C
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
							await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, null, false);
						}
					}
				}
			}
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x00241AF0 File Offset: 0x0023FCF0
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00241B33 File Offset: 0x0023FD33
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.AttacksPlayedThisTurn = 0;
			this.IsActivating = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002466 RID: 9318
		private bool _isActivating;

		// Token: 0x04002467 RID: 9319
		private int _attacksPlayedThisTurn;
	}
}
