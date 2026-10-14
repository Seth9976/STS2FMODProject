using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A9 RID: 1449
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsTears : RelicModel
	{
		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06005360 RID: 21344 RVA: 0x00244A88 File Offset: 0x00242C88
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x00244A8B File Offset: 0x00242C8B
		// (set) Token: 0x06005362 RID: 21346 RVA: 0x00244A93 File Offset: 0x00242C93
		private bool HadLeftoverEnergy
		{
			get
			{
				return this._hadLeftoverEnergy;
			}
			set
			{
				base.AssertMutable();
				this._hadLeftoverEnergy = value;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06005363 RID: 21347 RVA: 0x00244AA2 File Offset: 0x00242CA2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06005364 RID: 21348 RVA: 0x00244AAF File Offset: 0x00242CAF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x00244ABC File Offset: 0x00242CBC
		public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.HadLeftoverEnergy = base.Owner.PlayerCombatState.Energy > 0;
			return Task.CompletedTask;
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x00244AF8 File Offset: 0x00242CF8
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (this.HadLeftoverEnergy)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
			}
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x00244B43 File Offset: 0x00242D43
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.HadLeftoverEnergy = false;
			return Task.CompletedTask;
		}

		// Token: 0x0400248E RID: 9358
		private bool _hadLeftoverEnergy;
	}
}
