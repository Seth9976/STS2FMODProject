using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F7 RID: 1271
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArtOfWar : RelicModel
	{
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004EE4 RID: 20196 RVA: 0x0023C4E2 File Offset: 0x0023A6E2
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x0023C4E5 File Offset: 0x0023A6E5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004EE6 RID: 20198 RVA: 0x0023C4F2 File Offset: 0x0023A6F2
		// (set) Token: 0x06004EE7 RID: 20199 RVA: 0x0023C4FA File Offset: 0x0023A6FA
		private bool AnyAttacksPlayedLastTurn
		{
			get
			{
				return this._anyAttacksPlayedLastTurn;
			}
			set
			{
				base.AssertMutable();
				this._anyAttacksPlayedLastTurn = value;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06004EE8 RID: 20200 RVA: 0x0023C509 File Offset: 0x0023A709
		// (set) Token: 0x06004EE9 RID: 20201 RVA: 0x0023C511 File Offset: 0x0023A711
		private bool AnyAttacksPlayedThisTurn
		{
			get
			{
				return this._anyAttacksPlayedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._anyAttacksPlayedThisTurn = value;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x0023C520 File Offset: 0x0023A720
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x0023C530 File Offset: 0x0023A730
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (base.Owner != cardPlay.Card.Owner)
			{
				return Task.CompletedTask;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Attack)
			{
				return Task.CompletedTask;
			}
			if (this.AnyAttacksPlayedLastTurn)
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Normal;
			this.AnyAttacksPlayedThisTurn = true;
			return Task.CompletedTask;
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x0023C59D File Offset: 0x0023A79D
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.AnyAttacksPlayedLastTurn = this.AnyAttacksPlayedThisTurn;
			this.AnyAttacksPlayedThisTurn = false;
			return Task.CompletedTask;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x0023C5D0 File Offset: 0x0023A7D0
		public override async Task AfterEnergyReset(Player player)
		{
			if (player == base.Owner)
			{
				base.Status = RelicStatus.Active;
				if (base.Owner.PlayerCombatState.TurnNumber > 1)
				{
					if (!this.AnyAttacksPlayedLastTurn)
					{
						base.Flash();
						await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					}
					this.AnyAttacksPlayedLastTurn = false;
					this.AnyAttacksPlayedThisTurn = false;
				}
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x0023C61B File Offset: 0x0023A81B
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.AnyAttacksPlayedLastTurn = false;
			this.AnyAttacksPlayedThisTurn = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002420 RID: 9248
		private bool _anyAttacksPlayedLastTurn;

		// Token: 0x04002421 RID: 9249
		private bool _anyAttacksPlayedThisTurn;
	}
}
