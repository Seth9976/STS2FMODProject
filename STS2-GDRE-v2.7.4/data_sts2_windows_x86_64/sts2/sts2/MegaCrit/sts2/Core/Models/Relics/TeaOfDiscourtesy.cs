using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F6 RID: 1526
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TeaOfDiscourtesy : RelicModel
	{
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06005559 RID: 21849 RVA: 0x00248663 File Offset: 0x00246863
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x0600555A RID: 21850 RVA: 0x00248666 File Offset: 0x00246866
		public override bool IsUsedUp
		{
			get
			{
				return this.CombatsLeft <= 0;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600555B RID: 21851 RVA: 0x00248674 File Offset: 0x00246874
		public override bool ShowCounter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600555C RID: 21852 RVA: 0x00248678 File Offset: 0x00246878
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HealVar(1m),
					new DynamicVar("Combats", this.CombatsLeft),
					new DynamicVar("DazedCount", 2m)
				});
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600555D RID: 21853 RVA: 0x002486C8 File Offset: 0x002468C8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Dazed>(false);
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x0600555E RID: 21854 RVA: 0x002486D0 File Offset: 0x002468D0
		// (set) Token: 0x0600555F RID: 21855 RVA: 0x002486D8 File Offset: 0x002468D8
		[SavedProperty]
		private int CombatsLeft
		{
			get
			{
				return this._combatsLeft;
			}
			set
			{
				base.AssertMutable();
				this._combatsLeft = value;
				base.DynamicVars["Combats"].BaseValue = this._combatsLeft;
				base.InvokeDisplayAmountChanged();
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x00248728 File Offset: 0x00246928
		public override async Task BeforeCombatStart()
		{
			if (this.CombatsLeft > 0)
			{
				await CardPileCmd.AddToCombatAndPreview<Dazed>(base.Owner.Creature, PileType.Draw, base.DynamicVars["DazedCount"].IntValue, base.Owner, CardPilePosition.Random);
				this.CombatsLeft--;
				base.Flash();
			}
		}

		// Token: 0x040024C8 RID: 9416
		private const string _combatsKey = "Combats";

		// Token: 0x040024C9 RID: 9417
		private const string _dazedCountKey = "DazedCount";

		// Token: 0x040024CA RID: 9418
		private int _combatsLeft = 1;
	}
}
