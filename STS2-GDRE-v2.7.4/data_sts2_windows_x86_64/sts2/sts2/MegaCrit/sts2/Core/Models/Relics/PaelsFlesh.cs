using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A5 RID: 1445
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsFlesh : RelicModel
	{
		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x00244530 File Offset: 0x00242730
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06005334 RID: 21300 RVA: 0x00244533 File Offset: 0x00242733
		public override int DisplayAmount
		{
			get
			{
				PlayerCombatState playerCombatState = base.Owner.PlayerCombatState;
				if (playerCombatState == null)
				{
					return 1;
				}
				return playerCombatState.TurnNumber;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x0024454B File Offset: 0x0024274B
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress && base.Status == RelicStatus.Normal;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x00244564 File Offset: 0x00242764
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06005337 RID: 21303 RVA: 0x00244571 File Offset: 0x00242771
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x0024457E File Offset: 0x0024277E
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			if (player.PlayerCombatState.TurnNumber < 3)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.BaseValue;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x002445B1 File Offset: 0x002427B1
		public override Task BeforeCombatStart()
		{
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x002445BE File Offset: 0x002427BE
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x002445E4 File Offset: 0x002427E4
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			if (base.Owner.PlayerCombatState.TurnNumber < 3)
			{
				return Task.CompletedTask;
			}
			if (base.Status == RelicStatus.Active)
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Active;
			base.InvokeDisplayAmountChanged();
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0024464A File Offset: 0x0024284A
		public override Task AfterCombatEnd(CombatRoom room)
		{
			base.Status = RelicStatus.Normal;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}
	}
}
