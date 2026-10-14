using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200058D RID: 1421
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MiniRegent : RelicModel
	{
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06005294 RID: 21140 RVA: 0x002431BC File Offset: 0x002413BC
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06005295 RID: 21141 RVA: 0x002431BF File Offset: 0x002413BF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(1m));
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06005296 RID: 21142 RVA: 0x002431D0 File Offset: 0x002413D0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06005297 RID: 21143 RVA: 0x002431F0 File Offset: 0x002413F0
		// (set) Token: 0x06005298 RID: 21144 RVA: 0x002431F8 File Offset: 0x002413F8
		private bool UsedThisTurn
		{
			get
			{
				return this._usedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._usedThisTurn = value;
			}
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x00243208 File Offset: 0x00241408
		public override async Task AfterStarsSpent(int amount, Player spender)
		{
			if (spender == base.Owner)
			{
				if (!this.UsedThisTurn)
				{
					this.UsedThisTurn = true;
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, null, false);
				}
			}
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00243253 File Offset: 0x00241453
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.UsedThisTurn = false;
			return Task.CompletedTask;
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x0024327A File Offset: 0x0024147A
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.UsedThisTurn = false;
			return Task.CompletedTask;
		}

		// Token: 0x0400247A RID: 9338
		private bool _usedThisTurn;
	}
}
