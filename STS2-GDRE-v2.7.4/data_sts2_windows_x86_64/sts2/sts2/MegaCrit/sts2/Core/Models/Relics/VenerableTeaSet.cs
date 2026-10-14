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
	// Token: 0x0200060E RID: 1550
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VenerableTeaSet : RelicModel
	{
		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x0600560A RID: 22026 RVA: 0x00249BF5 File Offset: 0x00247DF5
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x0600560B RID: 22027 RVA: 0x00249BF8 File Offset: 0x00247DF8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x0600560C RID: 22028 RVA: 0x00249C05 File Offset: 0x00247E05
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x0600560D RID: 22029 RVA: 0x00249C12 File Offset: 0x00247E12
		// (set) Token: 0x0600560E RID: 22030 RVA: 0x00249C1A File Offset: 0x00247E1A
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

		// Token: 0x0600560F RID: 22031 RVA: 0x00249C45 File Offset: 0x00247E45
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is RestSiteRoom))
			{
				return Task.CompletedTask;
			}
			this.GainEnergyInNextCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x00249C64 File Offset: 0x00247E64
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

		// Token: 0x040024E3 RID: 9443
		private bool _gainEnergyInNextCombat;
	}
}
