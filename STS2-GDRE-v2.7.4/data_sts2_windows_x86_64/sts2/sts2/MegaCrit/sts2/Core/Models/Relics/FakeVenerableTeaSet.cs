using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000547 RID: 1351
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeVenerableTeaSet : RelicModel
	{
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x0023FA2F File Offset: 0x0023DC2F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060050C2 RID: 20674 RVA: 0x0023FA32 File Offset: 0x0023DC32
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x0023FA36 File Offset: 0x0023DC36
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060050C4 RID: 20676 RVA: 0x0023FA43 File Offset: 0x0023DC43
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060050C5 RID: 20677 RVA: 0x0023FA50 File Offset: 0x0023DC50
		// (set) Token: 0x060050C6 RID: 20678 RVA: 0x0023FA58 File Offset: 0x0023DC58
		[SavedProperty]
		public bool GainEnergyInNextCombat
		{
			get
			{
				return this._gainEnergyInNextCombat;
			}
			set
			{
				base.AssertMutable();
				if (this._gainEnergyInNextCombat == value)
				{
					return;
				}
				this._gainEnergyInNextCombat = value;
				base.Status = (this._gainEnergyInNextCombat ? RelicStatus.Active : RelicStatus.Normal);
			}
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x0023FA83 File Offset: 0x0023DC83
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is RestSiteRoom))
			{
				return Task.CompletedTask;
			}
			this.GainEnergyInNextCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x0023FAA0 File Offset: 0x0023DCA0
		public override async Task AfterEnergyReset(Player player)
		{
			if (base.Owner == player)
			{
				if (this.GainEnergyInNextCombat)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					this.GainEnergyInNextCombat = false;
				}
			}
		}

		// Token: 0x04002448 RID: 9288
		private bool _gainEnergyInNextCombat;
	}
}
