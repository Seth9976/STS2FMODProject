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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000571 RID: 1393
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Kusarigama : RelicModel
	{
		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060051DD RID: 20957 RVA: 0x00241B57 File Offset: 0x0023FD57
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x00241B5A File Offset: 0x0023FD5A
		public override bool ShowCounter
		{
			get
			{
				return CombatManager.Instance.IsInProgress;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060051DF RID: 20959 RVA: 0x00241B66 File Offset: 0x0023FD66
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

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060051E0 RID: 20960 RVA: 0x00241B98 File Offset: 0x0023FD98
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new DamageVar(6m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060051E1 RID: 20961 RVA: 0x00241BBD File Offset: 0x0023FDBD
		// (set) Token: 0x060051E2 RID: 20962 RVA: 0x00241BC5 File Offset: 0x0023FDC5
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

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x00241BDA File Offset: 0x0023FDDA
		// (set) Token: 0x060051E4 RID: 20964 RVA: 0x00241BE2 File Offset: 0x0023FDE2
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

		// Token: 0x060051E5 RID: 20965 RVA: 0x00241BF8 File Offset: 0x0023FDF8
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

		// Token: 0x060051E6 RID: 20966 RVA: 0x00241C44 File Offset: 0x0023FE44
		public override Task BeforeCombatStart()
		{
			this.AttacksPlayedThisTurn = 0;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00241C59 File Offset: 0x0023FE59
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.AttacksPlayedThisTurn = 0;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00241C88 File Offset: 0x0023FE88
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
							Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(base.Owner.Creature.CombatState.HittableEnemies);
							if (creature != null)
							{
								TaskHelper.RunSafely(this.DoActivateVisuals());
								await CreatureCmd.Damage(choiceContext, creature, base.DynamicVars.Damage, base.Owner.Creature);
							}
						}
					}
				}
			}
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x00241CDC File Offset: 0x0023FEDC
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x00241D1F File Offset: 0x0023FF1F
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.IsActivating = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002468 RID: 9320
		private bool _isActivating;

		// Token: 0x04002469 RID: 9321
		private int _attacksPlayedThisTurn;
	}
}
