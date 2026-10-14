using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000552 RID: 1362
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GalacticDust : RelicModel
	{
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005110 RID: 20752 RVA: 0x002404E7 File Offset: 0x0023E6E7
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005111 RID: 20753 RVA: 0x002404EA File Offset: 0x0023E6EA
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.StarsSpent % base.DynamicVars.Stars.IntValue;
				}
				return base.DynamicVars.Stars.IntValue;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x0024051C File Offset: 0x0023E71C
		// (set) Token: 0x06005113 RID: 20755 RVA: 0x00240524 File Offset: 0x0023E724
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

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005114 RID: 20756 RVA: 0x00240539 File Offset: 0x0023E739
		// (set) Token: 0x06005115 RID: 20757 RVA: 0x00240541 File Offset: 0x0023E741
		[SavedProperty]
		public int StarsSpent
		{
			get
			{
				return this._starsSpent;
			}
			set
			{
				base.AssertMutable();
				this._starsSpent = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00240558 File Offset: 0x0023E758
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				int intValue = base.DynamicVars.Stars.IntValue;
				base.Status = ((this.StarsSpent == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005117 RID: 20759 RVA: 0x002405A2 File Offset: 0x0023E7A2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x002405B4 File Offset: 0x0023E7B4
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005119 RID: 20761 RVA: 0x002405B7 File Offset: 0x0023E7B7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StarsVar(10),
					new BlockVar(10m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x002405E0 File Offset: 0x0023E7E0
		public override async Task AfterStarsSpent(int amount, Player spender)
		{
			if (spender == base.Owner)
			{
				this.StarsSpent += amount;
				if (this.StarsSpent >= base.DynamicVars.Stars.IntValue)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					await CreatureCmd.GainBlock(base.Owner.Creature, Mathf.FloorToInt((float)this.StarsSpent / (float)base.DynamicVars.Stars.IntValue) * base.DynamicVars.Block.IntValue, ValueProp.Unpowered, null, false);
					this.StarsSpent %= base.DynamicVars.Stars.IntValue;
				}
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x00240634 File Offset: 0x0023E834
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002452 RID: 9298
		private bool _isActivating;

		// Token: 0x04002453 RID: 9299
		private int _starsSpent;
	}
}
